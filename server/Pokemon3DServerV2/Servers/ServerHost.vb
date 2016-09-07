Imports System.Net.Sockets
Imports System.IO
Imports System.Net

Namespace Servers

    Public Class ServerHost

#Region "Fields and constants"

        Private Listener As TcpListener

        Private IPEndPoint As IPEndPoint = New IPEndPoint(IPAddress.Any, 15124)

        Private _hostingThread As Threading.Thread
        Private _startedHosting As Boolean = False

#End Region

#Region "Properties"

        ''' <summary>
        ''' Indicates wether the server starting hosting successfully.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StartedHosting() As Boolean
            Get
                Return Me._startedHosting
            End Get
        End Property

#End Region

#Region "Methods"

        Public Sub StartHost()
            'Read IPAddress and Port from properties:
            Dim ServerIP As IPAddress = IPAddress.Any
            If Basic.GetPropertyValue("IP-Address", "") <> "" Then
                ServerIP = IPAddress.Parse(Basic.GetPropertyValue("IP-Address", ""))
            End If

            Me.IPEndPoint = New IPEndPoint(ServerIP, CInt(Basic.GetPropertyValue("Port", "15124")))

            'Start he hosting process:
            Me._hostingThread = New Threading.Thread(AddressOf Me.InternalHost)
            Me._hostingThread.IsBackground = True
            Me._hostingThread.Start()
        End Sub

        ''' <summary>
        ''' This is the thread that listens to incoming TCP connections.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InternalHost()
            Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_HOSTINFO, Me.IPEndPoint.Address.ToString(), Me.IPEndPoint.Port.ToString()))

            Me.StartListener()

            Me._startedHosting = True

            While True
                Try
                    'Get next TcpClient from Listener:
                    Dim TcpClient As New TcpClient()
                    TcpClient = Listener.AcceptTcpClient()

                    'Handle the new player joining:
                    Dim t As New Threading.Thread(AddressOf Me.InternalHandleNewPlayer)
                    t.IsBackground = True
                    t.Start(TcpClient)
                Catch ex As Exception
                    Basic.ServersManager.WriteLine(ServerMessages.SERVER_UNEXPECTEDCLIENTERROR & ex.Message)

                    If Me.Listener.Server.Connected = False Then
                        Me.StartListener()
                    End If
                End Try
            End While
        End Sub

        ''' <summary>
        ''' Starts the TcpListener.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub StartListener()
            Try
                If Not Me.Listener Is Nothing Then
                    Me.Listener.Stop()
                End If
            Catch : End Try
            Try
                Me.Listener = New TcpListener(IPAddress.Any, Me.IPEndPoint.Port)
                Me.Listener.Start()
            Catch ex As Exception
                Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_ERRORHOST, ex.Message))
                Basic.ServersManager.Close()
            End Try
        End Sub

        ''' <summary>
        ''' The sub to handle incoming player connections.
        ''' </summary>
        ''' <param name="TcpClientObject">The TcpClient instance.</param>
        ''' <remarks></remarks>
        Private Sub InternalHandleNewPlayer(ByVal TcpClientObject As Object)
            Dim TcpClient As TcpClient = CType(TcpClientObject, TcpClient) 'Convert the TcpClientObject from the Object parameter class back to the TcpClient class.
            Dim IsValidPlayer As Boolean = False 'This gets set to true if the player gets added to the server.

            Dim NewPlayer As New Player()
            Try
                NewPlayer.Networking.AssignNetworkingClient(TcpClient)

                'Get first package sent by the player:
                Dim iniData As String = NewPlayer.Networking.ReadUnthreadedData()

                If iniData.StartsWith(ServersManager.PROTOCOLVERSION & "|") = True Then
                    'Try to convert the data received in the stream to a package:
                    Dim iniPackage As Package = New Package(iniData)

                    If iniPackage.IsValid = True Then
                        Select Case iniPackage.PackageType
                            Case Package.PackageTypes.GameData
                                'The player sends their initial GameData and wants to connect to the server as client for the first time:

                                'Check if the max amount of players is already reached:
                                If Basic.ServersManager.PlayerCollection.Count < CInt(Basic.GetPropertyValue("MaxPlayers", "10")) Then

                                    'Assign the new ID and set the game data.
                                    NewPlayer.ServersID = Basic.ServersManager.PlayerCollection.NextFreePlayerID()
                                    Debug.Print("Assign ID " & NewPlayer.ServersID.ToString() & " to new player.")
                                    NewPlayer.ApplyNewData(iniPackage)
                                    'Check if the GameMode is the same:
                                    If NewPlayer.GameMode.ToLower() = Basic.GetPropertyValue("GameMode", "Pokemon 3D").ToLower() Then

                                        'Check if there is already a player with the same name logged in (can only happen in OfflineMode)
                                        If Basic.ServersManager.PlayerCollection.HasPlayer(NewPlayer.Name) = False Then

                                            'Check if either OfflineMode is true or the player is using an online profile:
                                            If NewPlayer.IsGameJoltPlayer = True Or CBool(Basic.GetPropertyValue("OfflineMode", "0")) = True Then

                                                'Check for whitelist:
                                                If CBool(Basic.GetPropertyValue("whitelist", "0")) = False OrElse Basic.ServersManager.ListManager.WhiteList.ContainsPlayer(NewPlayer.ListName) = True Then

                                                    'Check for blacklist:
                                                    If CBool(Basic.GetPropertyValue("blacklist", "0")) = False OrElse Basic.ServersManager.ListManager.BlackList.ContainsPlayer(NewPlayer.ListName) = False Then

                                                        'New player can successfully join the server!
                                                        IsValidPlayer = True
                                                        Me.NewPlayerJoin(NewPlayer)
                                                    Else

                                                        'Blacklist is used and the new player is on the list:
                                                        NewPlayer.Networking.SendUnthreadedPackage(New Package(Package.PackageTypes.Kicked, -1, Servers.Package.ProtocolTypes.TCP, ServerMessages.CLIENT_BLACKLIST))
                                                        Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_BLACKLIST, NewPlayer.Name))
                                                    End If

                                                Else

                                                    'Whitelist is used and the new player is not on the list:
                                                    NewPlayer.Networking.SendUnthreadedPackage(New Package(Package.PackageTypes.Kicked, -1, Servers.Package.ProtocolTypes.TCP, ServerMessages.CLIENT_WHITELIST))
                                                    Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_WHITELIST, NewPlayer.Name))
                                                End If

                                            Else

                                                'Offline player while offline mode is false:
                                                NewPlayer.Networking.SendUnthreadedPackage(New Package(Package.PackageTypes.Kicked, -1, Servers.Package.ProtocolTypes.TCP, ServerMessages.CLIENT_GAMEJOLTREQUIRED))
                                                Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_GAMEJOLTREQUIRED, NewPlayer.Name))
                                            End If
                                        Else

                                            'Already existing name:
                                            NewPlayer.Networking.SendUnthreadedPackage(New Package(Package.PackageTypes.Kicked, -1, Servers.Package.ProtocolTypes.TCP, String.Format(ServerMessages.CLIENT_DUPLICATENAME, NewPlayer.Name)))
                                            Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_DUPLICATENAME, NewPlayer.Name))
                                        End If
                                    Else

                                        'Incorrect GameMode:
                                        NewPlayer.Networking.SendUnthreadedPackage(New Package(Package.PackageTypes.Kicked, -1, Servers.Package.ProtocolTypes.TCP, String.Format(ServerMessages.CLIENT_WRONGGAMEMODE, Basic.GetPropertyValue("GameMode", "Pokemon 3D"))))
                                        Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_WRONGGAMEMODE, NewPlayer.GameMode))
                                    End If

                                Else

                                    'Server is full:
                                    NewPlayer.Networking.SendUnthreadedPackage(New Package(Package.PackageTypes.Kicked, -1, Servers.Package.ProtocolTypes.TCP, ServerMessages.CLIENT_FULLSERVER))
                                    Basic.ServersManager.WriteLine(ServerMessages.SERVER_FULLSERVER)
                                End If

                            Case Package.PackageTypes.ServerDataRequest

                                'The connection is not by a player, but a request to send the server data to a client:
                                NewPlayer.Networking.SendUnthreadedPackage(Basic.ServersManager.ServerInfoPackage)
                        End Select
                    Else
                        Basic.ServersManager.WriteLine(ServerMessages.SERVER_INVALIDPROTOCOL)
                        'No valid package sent? No valid player to add.
                    End If
                Else
                    'Basic.ServersManager.WriteLine(ServerMessages.INVALIDPROTOCOL)
                    'Don't attemp to send a package to the new player since it's expected that the player has a different network protocol running and won't understand the package data anyways.
                End If
            Catch ex As Exception
                IsValidPlayer = False
            End Try

            If IsValidPlayer = False Then
                NewPlayer.Networking.CloseConnection()
            End If
        End Sub

        ''' <summary>
        ''' Performs actions to add a new player to the server.
        ''' </summary>
        ''' <param name="p">The player to be added to the server.</param>
        ''' <remarks></remarks>
        Private Sub NewPlayerJoin(ByVal p As Player)
            Debug.Print(p.Name & " joined the game!")
            Basic.ServersManager.PlayerCollection.Add(p) 'Obligatory add player to player list.

            'Start the player's networking so he can send/receive packages.
            p.Networking.StartNetworking()

            'Send the ID of the player to the client, so it's aware of its own ID.
            p.Networking.SendPackage(New Package(Package.PackageTypes.ID, -1, Servers.Package.ProtocolTypes.TCP, p.ServersID.ToString()))

            'Send information about all connected players to the new player (huge data load incoming):
            With Basic.ServersManager.PlayerCollection
                For i = 0 To .Count - 1
                    If i <= .Count - 1 Then
                        Dim eP As Player = .Item(i)

                        'Don't send the own data:
                        If eP.ServersID <> p.ServersID Then
                            p.Networking.SendPackage(New Package(Package.PackageTypes.CreatePlayer, -1, Servers.Package.ProtocolTypes.TCP, eP.ServersID.ToString()))
                            p.Networking.SendPackage(eP.PlayerDataPackage(True)) 'Send the full data package to the new player.
                        End If
                    End If
                Next
            End With

            'Send the new player information to all clients:
            Me.SendToAllPlayers(New Package(Package.PackageTypes.CreatePlayer, -1, Servers.Package.ProtocolTypes.TCP, p.ServersID.ToString()))
            Me.SendToAllPlayers(p.PlayerDataPackage(False))

            'Send the current World information to the new player:
            p.Networking.SendPackage(Basic.ServersManager.World.GetWorldPackage())

            'Send welcome message, if used:
            If Basic.GetPropertyValue("WelcomeMessage", "") <> "" Then
                p.Networking.SendPackage(New Package(Package.PackageTypes.ChatMessage, -1, Servers.Package.ProtocolTypes.TCP, Basic.GetPropertyValue("WelcomeMessage", "")))
            End If

            Me.SendToAllPlayers(New Package(Package.PackageTypes.ChatMessage, -1, Servers.Package.ProtocolTypes.TCP, String.Format(ServerMessages.CLIENT_NEWPLAYER, p.Name)))

            'Update the GUI:
            Basic.ServersManager.UpdatePlayerList()
        End Sub

        ''' <summary>
        ''' Sends a package to all connected players.
        ''' </summary>
        ''' <param name="Package"></param>
        ''' <remarks></remarks>
        Public Sub SendToAllPlayers(ByVal Package As Package)
            If Package.PackageType = Servers.Package.PackageTypes.ChatMessage Then
                Dim sender As String = "[SERVER]"
                If Package.Origin > -1 Then
                    sender = "<" & Basic.ServersManager.PlayerCollection.GetPlayerName(Package.Origin) & ">"
                End If

                Basic.ServersManager.WriteLine(sender & ": " & Package.DataItems(0))
            End If
            For i = 0 To Basic.ServersManager.PlayerCollection.Count - 1
                If i <= Basic.ServersManager.PlayerCollection.Count - 1 Then
                    Basic.ServersManager.PlayerCollection(i).Networking.SendPackage(Package)
                End If
            Next
        End Sub

        ''' <summary>
        ''' Sends a package to the client with the specified ServersID.
        ''' </summary>
        ''' <param name="ServersID">The ID of the client.</param>
        ''' <param name="Package">The Package to send to the client.</param>
        ''' <remarks></remarks>
        Public Sub SendToPlayer(ByVal ServersID As Integer, ByVal Package As Package)
            If Basic.ServersManager.PlayerCollection.HasPlayer(ServersID) = True Then
                Basic.ServersManager.PlayerCollection.GetPlayer(ServersID).Networking.SendPackage(Package)
            End If
        End Sub

#End Region

    End Class

End Namespace