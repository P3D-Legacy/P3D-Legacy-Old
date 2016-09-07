Imports System.Net.Sockets
Imports System.IO
Imports System.Net

Public Class Form1

    Const VERSION As String = "Indev 0.45.1" 'This is the version that corresponds with the main game's one.
    Const PROTOCOLVERSION As String = "0.0.9" 'This is the internal version of the server. Everytime the server gets a new release, this changes.

#Region "ServerStuff"

    Private Server As TcpListener
    Private Client As TcpClient
    Private ipEndPoint As IPEndPoint = New IPEndPoint(IPAddress.Any, 15124) 'The IP/Port the server is running on.
    Private list As New List(Of Player) 'The list of connected players
    Private MaxPlayers As Integer = 10 'The maximum players to be on this server

    'White and black lists, OPList and MuteList (based on names, case sensitive)
    Public Whitelist As New List(Of String)
    Public Blacklist As New List(Of String)
    Public OPList As New List(Of String)
    Public MuteList As New List(Of String)

    'AFKKickTimer
    Dim AFKKickTimer As Timer

    Private Function IsMuted(ByVal playerName As String) As Boolean
        For Each m As String In MuteList
            If m.ToLower() = playerName.ToLower() Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function IsOP(ByVal playerName As String) As Boolean
        For Each o As String In OPList
            If o.ToLower() = playerName.ToLower() Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function IsWhitelisted(ByVal playerName As String) As Boolean
        For Each o As String In Whitelist
            If o.ToLower() = playerName.ToLower() Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function IsBlacklisted(ByVal playerName As String) As Boolean
        For Each o As String In Blacklist
            If o.ToLower() = playerName.ToLower() Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' Checks if a player with the given name already exists in the player list.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PlayerNameExists(ByVal name As String) As Boolean
        For i = 0 To Me.list.Count - 1
            If i <= Me.list.Count - 1 Then
                Dim p As Player = list(i)
                If p.Name.ToLower() = name.ToLower() Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' Returns a player name from a given ID.
    ''' </summary>
    ''' <param name="ID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPlayerName(ByVal ID As Integer) As String
        For i = 0 To Me.list.Count - 1
            If i <= Me.list.Count - 1 Then
                Dim p As Player = list(i)
                If p.ID = ID Then
                    Return p.Name
                End If
            End If
        Next
        Return ""
    End Function

    ''' <summary>
    ''' Returns the ID of the first player in the list with the given name.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPlayerID(ByVal name As String) As Integer
        For i = 0 To Me.list.Count - 1
            If i <= Me.list.Count - 1 Then
                Dim p As Player = list(i)
                If p.Name.ToLower() = name.ToLower() Then
                    Return p.ID
                End If
            End If
        Next
        Return -1
    End Function

    ''' <summary>
    ''' Gets the first free player ID available that isn't assigned to a player.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetFreePlayerID() As Integer
        If list.Count = 0 Then
            Return 0
        Else
            Dim ID As Integer = 0
            Dim foundID As Boolean = False

            While foundID = False
                foundID = True

                For i = 0 To list.Count - 1
                    If i <= list.Count - 1 Then
                        Dim p As Player = list(i)
                        If p.ID = ID Then
                            ID += 1
                            foundID = False
                            Exit For
                        End If
                    End If
                Next
            End While

            Return ID
        End If
    End Function

    ''' <summary>
    ''' Removes the player with the given ID from the list.
    ''' </summary>
    ''' <param name="ID"></param>
    ''' <remarks></remarks>
    Private Sub RemovePlayer(ByVal ID As Integer)
        For i = 0 To list.Count - 1
            If list(i).ID = ID Then
                list.RemoveAt(i)
                Exit For
            End If
        Next
    End Sub

    ''' <summary>
    ''' Gets a player by ID.
    ''' </summary>
    ''' <param name="ID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPlayer(ByVal ID As Integer) As Player
        For i = 0 To list.Count - 1
            If i <= list.Count - 1 Then
                Dim p As Player = list(i)
                If p.ID = ID Then
                    Return p
                End If
            End If
        Next

        Return Nothing
    End Function

    ''' <summary>
    ''' A class that describes a P3D player.
    ''' </summary>
    ''' <remarks></remarks>
    Class Player

        Public stream As NetworkStream
        Public streamw As StreamWriter
        Public streamr As StreamReader

        Public Version As String = "" 'The version this player is playing on.
        Public IsOnlinePlayer As Boolean = False 'Determines if the player is logged into a GJ account.

        Public ID As Integer = 0 'The ID that is assigned to this player.
        Public Initialized As Boolean = False

        Public Name As String = ""
        Public GameJoltID As String = ""
        Public Position As Vector3 = New Vector3(0)
        Public Skin As String = ""
        Public Facing As Integer = 0
        Public Moving As Boolean = False
        Public LevelFile As String = ""
        Public DecSeparator As String = ","
        Public BusyType As Integer = 0
        Public GameMode As String = ""

        Public PokePosition As Vector3 = New Vector3(0)
        Public PokeFacing As Integer = 0
        Public PokeSkin As String = ""
        Public PokeVisible As Boolean = False

        Public AFKTime As Integer = 0
        Public LastPingTime As Date = Date.Now

        ''' <summary>
        ''' Processes a full game data input.
        ''' </summary>
        ''' <param name="s"></param>
        ''' <remarks></remarks>
        Public Sub GameDataInput(ByVal s As String)
            'Version(0),IsOnline(1),Position(2,3,4),Skin(5),Facing(6),Moving(7),LevelFile(8),Playername(9),GamejoltID(10),DecSeperator(11),BusyType(12),PokePosition(13,14,15),PokeFacing(16),PokeSkin(17),PokeVisible(18),GameMode(19)

            Dim data() As String = s.Split(CChar("|"))
            Me.Version = data(0)
            Me.IsOnlinePlayer = CBool(data(1))
            Me.Position = New Vector3(CSng(data(2).Replace(".", DecSeparator)), CSng(data(3).Replace(".", DecSeparator)), CSng(data(4).Replace(".", DecSeparator)))
            Me.Skin = data(5).Replace("VERTICALBREAK", "|")
            Me.Facing = CInt(data(6))
            Me.Moving = CBool(data(7))
            Me.LevelFile = data(8)
            Me.Name = data(9)
            Me.GameJoltID = data(10)
            Me.DecSeparator = data(11)
            Me.BusyType = CInt(data(12))
            Me.PokePosition = New Vector3(CSng(data(13).Replace(".", DecSeparator)), CSng(data(14).Replace(".", DecSeparator)), CSng(data(15).Replace(".", DecSeparator)))
            Me.PokeFacing = CInt(data(16))
            Me.PokeSkin = data(17).Replace("VERTICALBREAK", "|")
            Me.PokeVisible = CBool(data(18))
            Me.GameMode = data(19)

            Me.Initialized = True

            LastPingTime = Date.Now
        End Sub

        ''' <summary>
        ''' Returns the full game data of this player.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetGameData() As String
            Return Me.Version & "|" &
                Me.IsOnlinePlayer.ToString() & "|" &
                Me.Position.X.ToString() & "|" & Me.Position.Y.ToString() & "|" & Me.Position.Z & "|" &
                Me.Skin.Replace("|", "VERTICALBREAK") & "|" &
                Me.Facing & "|" &
                Me.Moving.ToString() & "|" &
                Me.LevelFile & "|" &
                Me.Name & "|" &
                Me.GameJoltID & "|" &
                Me.DecSeparator & "|" &
                Me.BusyType.ToString() & "|" &
                Me.PokePosition.X.ToString() & "|" & Me.PokePosition.Y.ToString() & "|" & Me.PokePosition.Z & "|" &
                Me.PokeFacing.ToString() & "|" &
                Me.PokeSkin.Replace("|", "VERTICALBREAK") & "|" &
                Me.PokeVisible.ToString() & "|" &
                Me.GameMode.ToString()
        End Function

        ''' <summary>
        ''' Processes the ingame values of a player.
        ''' </summary>
        ''' <param name="s"></param>
        ''' <remarks></remarks>
        Public Sub PlayDataInput(ByVal s As String)
            'Position(0,1,2),Skin(3),Facing(4),Moving(5),Levelfile(6),Playername(7),BusyType(8),PokePosition(9,10,11),PokeFacing(12),PokeSkin(13),PokeVisible(14)

            Try
                Dim data() As String = s.Split(CChar("|"))
                Me.Position = New Vector3(CSng(data(0).Replace(".", DecSeparator)), CSng(data(1).Replace(".", DecSeparator)), CSng(data(2).Replace(".", DecSeparator)))
                Me.Skin = data(3).Replace("VERTICALBREAK", "|")
                Me.Facing = CInt(data(4))
                Me.Moving = CBool(data(5))
                Me.LevelFile = data(6)
                Me.Name = data(7)
                Me.BusyType = CInt(data(8))
                Me.PokePosition = New Vector3(CSng(data(9).Replace(".", DecSeparator)), CSng(data(10).Replace(".", DecSeparator)), CSng(data(11).Replace(".", DecSeparator)))
                Me.PokeFacing = CInt(data(12))
                Me.PokeSkin = data(13).Replace("VERTICALBREAK", "|")
                Me.PokeVisible = CBool(data(14))
            Catch : End Try

            LastPingTime = Date.Now
        End Sub

        ''' <summary>
        ''' Returns the data needed to display the player.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function GetPlayData() As String
            'Position(0,1,2),Skin(3),Facing(4),Moving(5),Levelfile(6),Playername(7),BusyType(8),PokePosition(9,10,11),PokeFacing(12),PokeSkin(13),PokeVisible(14)

            Return Me.Position.X.ToString() & "|" & Me.Position.Y.ToString() & "|" & Me.Position.Z.ToString() & "|" &
                Me.Skin.Replace("|", "VERTICALBREAK") & "|" &
                Me.Facing & "|" &
                Me.Moving.ToString() & "|" &
                Me.LevelFile & "|" &
                Me.Name & "|" &
                Me.BusyType.ToString() & "|" &
                Me.PokePosition.X.ToString() & "|" & Me.PokePosition.Y.ToString() & "|" & Me.PokePosition.Z & "|" &
                Me.PokeFacing.ToString() & "|" &
                Me.PokeSkin.Replace("|", "VERTICALBREAK") & "|" &
                Me.PokeVisible.ToString()
        End Function

        ''' <summary>
        ''' Updates the ping time for this player when receiving a ping package.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Ping()
            Me.LastPingTime = Date.Now
        End Sub

    End Class

    ''' <summary>
    ''' Sends the given TCP package to all connected players.
    ''' </summary>
    ''' <param name="p"></param>
    ''' <remarks></remarks>
    Private Sub SendToAllClients(ByVal p As Package)
        If p.PackageType = Package.PackageTypes.ChatMessage Then
            Dim sender As String = "<" & GetPlayerName(p.Origin) & ">"

            If p.Origin = -1 Then '-1 means message comes from the server.
                sender = "[SERVER]"
            End If

            Me.Invoke(New DAddItem(AddressOf AddItem), sender & ": " & p.Data) 'Adds message to output when its a chat message.
        End If
        For i = 0 To list.Count - 1
            If i <= list.Count - 1 Then
                Dim c As Player = list(i)
                Try
                    Dim streamw As New StreamWriter(c.stream)
                    streamw.WriteLine(p.ToString())
                    streamw.Flush()
                Catch ex As Exception
                End Try
            End If
        Next
    End Sub

    ''' <summary>
    ''' Sends the given package to a single player (identified by a ClientID)
    ''' </summary>
    ''' <param name="ClientID"></param>
    ''' <param name="p"></param>
    ''' <remarks></remarks>
    Private Sub SendToClient(ByVal ClientID As Integer, ByVal p As Package)
        For i = 0 To list.Count - 1
            If i <= list.Count - 1 Then
                Dim c As Player = list(i)
                If c.ID = ClientID Then
                    c.streamw.WriteLine(p.ToString())
                    c.streamw.Flush()
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Sends the give package to the given player.
    ''' </summary>
    ''' <param name="c"></param>
    ''' <param name="p"></param>
    ''' <remarks></remarks>
    Private Sub SendToClient(ByVal c As Player, ByVal p As Package)
        c.streamw.WriteLine(p.ToString())
        c.streamw.Flush()
    End Sub

    ''' <summary>
    ''' Adds a line of text to the server output.
    ''' </summary>
    ''' <param name="s"></param>
    ''' <remarks></remarks>
    Private Sub WriteLocalLine(ByVal s As String)
        Me.Invoke(New DAddItem(AddressOf AddItem), s)
    End Sub

    ''' <summary>
    ''' Listens to the player input.
    ''' </summary>
    ''' <param name="Player"></param>
    ''' <remarks></remarks>
    Private Sub ListenToConnection(ByVal Player As Player)
        Do
            Try
                Dim tmp As String = Player.streamr.ReadLine 'waiting for input from the connection
                HandlePackage(New Package(tmp)) 'Handle the package.
            Catch 'Receiving an error message / time out -> close the connection.
                Try
                    SendToAllClients(New Package(3, -1, Player.Name & " quit the game."))
                Catch : End Try
                Try
                    SendToAllClients(New Package(Package.PackageTypes.DestroyPlayer, -1, Player.ID.ToString()))
                Catch : End Try

                list.Remove(Player)

                Debug.Print(Player.Name & " quit the game.")

                Try
                    Me.Invoke(New DUpdatePlayerList(AddressOf UpdatePlayerList))
                Catch : End Try

                Exit Do
            End Try
        Loop
    End Sub

    ''' <summary>
    ''' Decides what to do with a received package.
    ''' </summary>
    ''' <param name="p"></param>
    ''' <remarks></remarks>
    Private Sub HandlePackage(ByVal p As Package)
        Select Case p.PackageType
            Case Package.PackageTypes.ChatMessage
                If IsMuted(GetPlayerName(p.Origin)) = False Then
                    If p.Data.StartsWith("/") = True Then
                        HandleClientCommand(p)  'Command incoming from client.
                    Else
                        SendToAllClients(p) 'Sends chat message to all clients.
                    End If
                Else
                    SendToClient(p.Origin, New Package(Package.PackageTypes.ChatMessage, -1, "You are muted on this server."))
                End If
            Case Package.PackageTypes.PrivateMessage
                If IsMuted(GetPlayerName(p.Origin)) = False Then
                    Dim playerName As String = p.Data.Remove(0, 4)
                    While PlayerNameExists(playerName) = False And playerName.Contains(" ") = True
                        playerName = playerName.Remove(playerName.LastIndexOf(" "))
                    End While
                    If playerName <> "" Then
                        If GetPlayerID(playerName) <> p.Origin Then
                            Dim message As String = "[PM]" & p.Data.Remove(0, 4 + playerName.Length)
                            SendToClient(GetPlayerID(playerName), New Package(Package.PackageTypes.PrivateMessage, p.Origin, message))
                            SendToClient(p.Origin, New Package(Package.PackageTypes.ChatMessage, p.Origin, p.Data))
                        End If
                    End If
                Else
                    SendToClient(p.Origin, New Package(Package.PackageTypes.ChatMessage, -1, "You are muted on this server."))
                End If
            Case Package.PackageTypes.GameData
                Dim player As Player = GetPlayer(p.Origin)
                If Not player Is Nothing Then
                    player.GameDataInput(p.Data)
                End If
                SendToAllClients(New Package(0, p.Origin, p.Data))
            Case Package.PackageTypes.PlayData
                Dim player As Player = GetPlayer(p.Origin)
                If Not player Is Nothing Then
                    player.PlayDataInput(p.Data)
                End If
                SendToAllClients(New Package(1, p.Origin, p.Data))
            Case Package.PackageTypes.Ping
                Dim player As Player = GetPlayer(p.Origin)
                If Not player Is Nothing Then
                    player.Ping()
                End If
            Case Package.PackageTypes.GamestateMessage
                Dim player As Player = GetPlayer(p.Origin)
                If IsMuted(player.Name) = False Then
                    If Not player Is Nothing Then
                        SendToAllClients(New Package(Package.PackageTypes.ChatMessage, -1, "The player " & player.Name & " " & p.Data))
                    End If
                End If

            Case Package.PackageTypes.TradeJoin
                Dim ID As Integer = CInt(p.Data)
                SendToClient(ID, New Package(Package.PackageTypes.TradeJoin, p.Origin, ""))
            Case Package.PackageTypes.TradeOffer
                Dim ID As Integer = CInt(p.Data.Remove(p.Data.IndexOf(",")))
                Dim PokemonData As String = p.Data.Remove(0, p.Data.IndexOf(",") + 1)

                SendToClient(ID, New Package(Package.PackageTypes.TradeOffer, p.Origin, PokemonData))
            Case Package.PackageTypes.TradeQuit
                Dim ID As Integer = CInt(p.Data)
                SendToClient(ID, New Package(Package.PackageTypes.TradeQuit, p.Origin, ""))
            Case Package.PackageTypes.TradeStart
                Dim ID As Integer = CInt(p.Data)
                SendToClient(ID, New Package(Package.PackageTypes.TradeStart, p.Origin, ""))
            Case Package.PackageTypes.TradeRequest
                If IsMuted(GetPlayerName(p.Origin)) = False Then
                    Dim ID As Integer = CInt(p.Data)
                    SendToClient(ID, New Package(Package.PackageTypes.TradeRequest, p.Origin, ""))
                End If

            Case Package.PackageTypes.BattleJoin
                Dim ID As Integer = CInt(p.Data)
                SendToClient(ID, New Package(Package.PackageTypes.BattleJoin, p.Origin, ""))
            Case Package.PackageTypes.BattleQuit
                Dim ID As Integer = CInt(p.Data)
                SendToClient(ID, New Package(Package.PackageTypes.BattleQuit, p.Origin, ""))
            Case Package.PackageTypes.BattleOffer
                Dim ID As Integer = CInt(p.Data.Remove(p.Data.IndexOf(",")))
                Dim PokemonData As String = p.Data.Remove(0, p.Data.IndexOf(",") + 1)

                SendToClient(ID, New Package(Package.PackageTypes.BattleOffer, p.Origin, PokemonData))
            Case Package.PackageTypes.BattleStart
                Dim ID As Integer = CInt(p.Data)
                SendToClient(ID, New Package(Package.PackageTypes.BattleStart, p.Origin, ""))
            Case Package.PackageTypes.BattleClientData
                Dim ID As Integer = CInt(p.Data.Remove(p.Data.IndexOf(",")))
                Dim ClientData As String = p.Data.Remove(0, p.Data.IndexOf(",") + 1)

                SendToClient(ID, New Package(Package.PackageTypes.BattleClientData, p.Origin, ClientData))
            Case Package.PackageTypes.BattleHostData
                Dim ID As Integer = CInt(p.Data.Remove(p.Data.IndexOf(",")))
                Dim HostData As String = p.Data.Remove(0, p.Data.IndexOf(",") + 1)

                SendToClient(ID, New Package(Package.PackageTypes.BattleHostData, p.Origin, HostData))
            Case Package.PackageTypes.BattlePokemonData
                Dim ID As Integer = CInt(p.Data.Remove(p.Data.IndexOf(",")))
                Dim HostData As String = p.Data.Remove(0, p.Data.IndexOf(",") + 1)

                SendToClient(ID, New Package(Package.PackageTypes.BattlePokemonData, p.Origin, HostData))
            Case Package.PackageTypes.BattleRequest
                If IsMuted(GetPlayerName(p.Origin)) = False Then
                    Dim ID As Integer = CInt(p.Data)
                    SendToClient(ID, New Package(Package.PackageTypes.BattleRequest, p.Origin, ""))
                End If
        End Select
    End Sub

    Private Sub HandleClientCommand(ByVal p As Package)
        If IsOP(GetPlayerName(p.Origin)) = True Then
            If CBool(GetPropertyValue("AllowOP", "1")) = True Then
                HandleCommand(p.Data, p.Origin)
            End If
        Else
            WriteLocalLine("[INFO] Player " & GetPlayerName(p.Origin) & " (" & p.Origin.ToString() & ") tried to use a command without op permissions!")
            Try
                SendToClient(p.Origin, New Package(Package.PackageTypes.ChatMessage, -1, "You don't have permissions to use commands on this server!"))
            Catch : End Try
        End If
    End Sub

    ''' <summary>
    ''' The main loop to accept new players.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub TickKickTimer()
        Dim AFKTime As Integer = CInt(GetPropertyValue("IdleKickTime", "0"))

        For i = 0 To Me.list.Count - 1
            If i <= Me.list.Count - 1 Then
                Dim p As Player = list(i)
                If DateDiff(DateInterval.Second, p.LastPingTime, Date.Now) > 20 Then
                    Try
                        SendToClient(p.ID, New Package(Package.PackageTypes.Kicked, -1, "You got kicked for bad connection speed."))
                    Catch : End Try

                    list.Remove(p)

                    WriteLocalLine("[INFO] " & p.Name & " quit the game (no ping received for 20 seconds).")

                    Try
                        Me.Invoke(New DUpdatePlayerList(AddressOf UpdatePlayerList))
                    Catch : End Try

                    Exit For
                Else
                    If AFKTime > 0 Then
                        If p.BusyType = 3 Then
                            p.AFKTime += 1
                        Else
                            p.AFKTime = 0
                        End If

                        If p.AFKTime >= AFKTime Then
                            Try
                                SendToClient(p.ID, New Package(Package.PackageTypes.Kicked, -1, "You got kicked for lack of activity."))
                            Catch : End Try

                            list.Remove(p)

                            WriteLocalLine("[INFO] " & p.Name & " quit the game (lack of activity).")

                            Try
                                Me.Invoke(New DUpdatePlayerList(AddressOf UpdatePlayerList))
                            Catch : End Try

                            Exit For
                        End If
                    End If
                End If
            End If
        Next
    End Sub

    Sub Main()
        WriteLocalLine("[INFO] Starting Pokémon3D server on " & Me.ipEndPoint.Address.ToString() & ":" & Me.ipEndPoint.Port.ToString())
        Debug.Print("---Started server---")

        Try
            Server = New TcpListener(IPAddress.Any, CInt(GetPropertyValue("Port", "15124")))
            Server.Start()
        Catch ex As Exception
            WriteLocalLine("[WARNING] Error starting the server: " & ex.Message & ". Shutting the server down...")
            Application.Exit()
        End Try

        While True
            Try
                Client = Server.AcceptTcpClient()
                Debug.Print("New player tries to join (IP: " & CType(Client.Client.RemoteEndPoint, Net.IPEndPoint).Address.ToString() & ").")

                'Found new player, setup stream and assign new ID:
                Dim c As New Player
                c.stream = Client.GetStream()
                c.streamr = New StreamReader(c.stream)
                c.streamw = New StreamWriter(c.stream)

                Dim p As Package = New Package(c.streamr.ReadLine())
                If p.PackageType = Package.PackageTypes.GameData Then
                    If list.Count < MaxPlayers Then 'Check if max player limit isnt reached.
                        c.ID = GetFreePlayerID()
                        Debug.Print("Assigning ID " & c.ID & " to new player.")
                        c.GameDataInput(p.Data)

                        If c.Version = PROTOCOLVERSION Then 'Check if the vesion is the same.

                            If c.GameMode.ToLower() = GetPropertyValue("GameMode", "Pokemon 3D").ToLower() Then
                                If PlayerNameExists(c.Name) = False Then 'Check if player with this name is already on the server.
                                    If CBool(GetPropertyValue("OfflineMode", "0")) = True Or (CBool(GetPropertyValue("OfflineMode", "0")) = False And c.IsOnlinePlayer = True) = True Then
                                        Dim whiteListed As Boolean = True
                                        If CBool(GetPropertyValue("whitelist", "0")) = True Then
                                            If Me.IsWhitelisted(c.Name) = False Then
                                                whiteListed = False
                                            End If
                                        End If

                                        If whiteListed = True Then 'Check if player is whitelisted (when whitelists are used)
                                            Dim blackListed As Boolean = False
                                            If CBool(GetPropertyValue("blacklist", "0")) = True Then
                                                If Me.IsBlacklisted(c.Name) = True Then
                                                    blackListed = True
                                                End If
                                            End If

                                            If blackListed = False Then 'Check if player is blacklisted (if blacklists are used)
                                                list.Add(c)
                                                Debug.Print(c.Name & " joined the game!")

                                                'Send ID package to new player:
                                                SendToClient(c.ID, New Package(Package.PackageTypes.ID, -1, c.ID.ToString()))

                                                'Send all connected player information to the new player.
                                                For i = 0 To Me.list.Count - 1
                                                    If i <= Me.list.Count - 1 Then
                                                        Dim eP As Player = Me.list(i)
                                                        If eP.ID <> c.ID Then
                                                            SendToClient(c.ID, New Package(Package.PackageTypes.CreatePlayer, -1, eP.ID.ToString()))
                                                            SendToClient(c.ID, New Package(Package.PackageTypes.GameData, eP.ID, eP.GetGameData()))
                                                        End If
                                                    End If
                                                Next

                                                'Send the new player information to all connected players.
                                                SendToAllClients(New Package(Package.PackageTypes.ChatMessage, -1, c.Name & " joined the game!"))
                                                SendToAllClients(New Package(Package.PackageTypes.CreatePlayer, -1, c.ID.ToString()))
                                                SendToAllClients(New Package(Package.PackageTypes.GameData, c.ID, c.GetGameData()))
                                                'Send world information to new player:
                                                SendToClient(c.ID, GetWorldPackage())

                                                'Send welcome message to client if using it.
                                                Dim WelcomeMessage As String = GetPropertyValue("WelcomeMessage", "")
                                                If WelcomeMessage <> "" Then
                                                    SendToClient(c.ID, New Package(Package.PackageTypes.ChatMessage, -1, WelcomeMessage))
                                                End If

                                                Me.Invoke(New DUpdatePlayerList(AddressOf UpdatePlayerList))

                                                'Start listening to the new player connection.
                                                Dim t As New Threading.Thread(AddressOf ListenToConnection)
                                                t.IsBackground = True
                                                t.Start(c)
                                            Else
                                                WriteLocalLine("[INFO] Player " & c.Name & " tried to connect but is blacklisted!")
                                                SendToClient(c, New Package(Package.PackageTypes.Kicked, -1, "You are banned from this server!"))
                                            End If
                                        Else
                                            WriteLocalLine("[INFO] Player " & c.Name & " tried to connect but is not whitelisted!")
                                            SendToClient(c, New Package(Package.PackageTypes.Kicked, -1, "You have to be whitelisted to join this server!"))
                                        End If
                                    Else
                                        WriteLocalLine("[INFO] Player " & c.Name & " tried to connect to with an offline profile.")
                                        SendToClient(c, New Package(Package.PackageTypes.Kicked, -1, "Server requires a GameJolt profile to join."))
                                    End If
                                Else
                                    WriteLocalLine("[INFO] Player with the name """ & c.Name & """ tried to connect. This name already exists on the server!")
                                    SendToClient(c, New Package(Package.PackageTypes.Kicked, -1, "A player with the name """ & c.Name & """ already exists on the server."))
                                End If
                            Else
                                WriteLocalLine("[INFO] Player with a GameMode """ & c.GameMode & """ tried to join.")
                                SendToClient(c, New Package(Package.PackageTypes.Kicked, -1, "The server requires the GameMode """ & GetPropertyValue("GameMode", "Pokemon 3D") & """."))
                            End If
                        Else
                            WriteLocalLine("[INFO] Player tried to connect with protocol version " & c.Version & ". Server version: " & PROTOCOLVERSION)
                            SendToClient(c, New Package(Package.PackageTypes.Kicked, -1, "Version doesn't match the server's version!"))
                        End If
                    Else
                        WriteLocalLine("[INFO] Player tried to connect, but the server is full.")
                        SendToClient(c, New Package(Package.PackageTypes.Kicked, -1, "The server is full."))
                    End If
                Else
                    If p.PackageType = Package.PackageTypes.ServerDataRequest Then
                        Debug.Print("Send info package to client with IP: " & CType(Client.Client.RemoteEndPoint, Net.IPEndPoint).Address.ToString())
                        SendToClient(c, New Package(Package.PackageTypes.ServerInfoData, -1, Me.list.Count & "/" & GetPropertyValue("MaxPlayers", "10") & "," & GetPropertyValue("servername", "P3D Server") & "," & GetPropertyValue("ServerMessage", "") & "," & PROTOCOLVERSION))
                    End If
                End If
            Catch ex As Exception
                WriteLocalLine("[WARNING] Unexpected error while receiving client data: " & ex.Message)
            End Try
        End While
    End Sub

#End Region

#Region "GUI"

    Private Delegate Sub DAddItem(ByVal s As String)
    Private Delegate Sub DUpdatePlayerList()

    Private CurrentLogName As String = ""

    Private Sub Form1_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SendToAllClients(New Package(Package.PackageTypes.ServerClose, -1, ""))
        SaveProperties()
        Debug.Print("---Close server---")
    End Sub

    Private Sub Form1_Shown(sender As System.Object, e As System.EventArgs) Handles MyBase.Shown
        WriteLocalLine("[INFO] Starting Pokémon3D server version " & Form1.VERSION & " (" & Form1.PROTOCOLVERSION & ")")
        LoadProperties()

        Dim t As New Threading.Thread(AddressOf Main)
        t.IsBackground = True
        t.Start()

        SetWorldProperties()

        Dim timer As New Timer()
        timer.Interval = 10000
        AddHandler timer.Tick, AddressOf Me.SetTime
        timer.Start()

        Dim timer2 As New Timer()
        timer2.Interval = 1000
        AddHandler timer2.Tick, AddressOf Me.TickKickTimer
        timer2.Start()

        Dim timer3 As New Timer()
        timer3.Interval = 350
        AddHandler timer3.Tick, AddressOf Me.DrawThreads
        timer3.Start()

        UpdatePlayerList()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With My.Computer.Clock.LocalTime
            Dim month As String = .Month.ToString()
            If month.Length = 1 Then
                month = "0" & month
            End If
            Dim day As String = .Day.ToString()
            If day.Length = 1 Then
                day = "0" & day
            End If
            Dim hour As String = .Hour.ToString()
            If hour.Length = 1 Then
                hour = "0" & hour
            End If
            Dim minute As String = .Minute.ToString()
            If minute.Length = 1 Then
                minute = "0" & minute
            End If
            Dim second As String = .Second.ToString()
            If second.Length = 1 Then
                second = "0" & second
            End If

            Me.CurrentLogName = "Log_" & .Year & "-" & month & "-" & day & "_" & hour & "." & minute & "." & second & ".dat"
        End With
    End Sub

    Private Sub AddItem(ByVal s As String)
        If IsNothing(s) = False Then
            LogLine(s)
            list_m.Items.Add(s)
            Dim items As Integer = CInt(list_m.Height / list_m.ItemHeight)

            If list_m.TopIndex + items >= list_m.Items.Count + 1 Then
                list_m.TopIndex = list_m.Items.Count - 1
            End If
        End If
    End Sub

    Dim loadedLog As Boolean = False
    Dim tempLog As New List(Of String)
    Private Sub LogLine(ByVal s As String)
        If System.IO.Directory.Exists(My.Application.Info.DirectoryPath & "\logs") = False Then
            System.IO.Directory.CreateDirectory(My.Application.Info.DirectoryPath & "\logs")
        End If
        If System.IO.File.Exists(My.Application.Info.DirectoryPath & "\logs\" & CurrentLogName) = False Then
            System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\logs\" & CurrentLogName, "")
        Else
            If loadedLog = False Then
                loadedLog = True
                tempLog = System.IO.File.ReadAllLines(My.Application.Info.DirectoryPath & "\logs\" & CurrentLogName).ToList()
            End If
        End If

        Dim logC As List(Of String) = tempLog
        With My.Computer.Clock.LocalTime
            Dim Hour As String = .Hour.ToString()
            If Hour.Length = 1 Then
                Hour = "0" & Hour
            End If
            Dim minute As String = .Minute.ToString()
            If minute.Length = 1 Then
                minute = "0" & minute
            End If
            Dim second As String = .Second.ToString()
            If second.Length = 1 Then
                second = "0" & second
            End If

            logC.Add("[" & Hour & ":" & minute & ":" & second & "] " & s)
        End With
        System.IO.File.WriteAllLines(My.Application.Info.DirectoryPath & "\logs\" & CurrentLogName, logC.ToArray())
    End Sub

    Private Sub UpdatePlayerList()
        list_players.Items.Clear()

        For i = 0 To Me.list.Count - 1
            If i <= Me.list.Count - 1 Then
                list_players.Items.Add(Me.list(i).Name & " (" & list(i).ID & ")")
            End If
        Next

        GroupBox1.Text = "Players (" & list_players.Items.Count & " / " & GetPropertyValue("MaxPlayers", "10") & ")"

        ExportServerData()
    End Sub

    Const ExportPath As String = "C:\inetpub\wwwroot\"

    Private Sub ExportServerData()
#If CONFIG = "KolbenRelease" Then
        Dim data As String = Me.list.Count & vbNewLine &
            GetPropertyValue("MaxPlayers", "10") & vbNewLine &
            Me.Season.ToString() & vbNewLine &
            Me.Weather.ToString() & vbNewLine &
            Me.CurrentTime.ToString()
        If System.IO.Directory.Exists(ExportPath) = True Then
            System.IO.File.WriteAllText(ExportPath & "p3dapi.txt", data)
        Else
            System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\p3dapi.txt", data)
        End If

        Dim sortedList As List(Of Player) = (From p As Player In Me.list Order By p.Name Ascending).ToList()
        Dim playerData As String = ""
        For Each p As Player In sortedList
            If playerData <> "" Then
                playerData &= vbNewLine
            End If
            playerData &= p.Name
        Next
        If System.IO.Directory.Exists(ExportPath) = True Then
            System.IO.File.WriteAllText(ExportPath & "p3dapi_players.txt", playerData)
        Else
            System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\p3dapi_players.txt", playerData)
        End If
#End If
    End Sub

    Private Sub text_input_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles text_input.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) And text_input.Text <> "" Then
            If text_input.Text(0) = CChar("/") Then
                HandleCommand(text_input.Text, -1)
            Else
                SendToAllClients(New Package(3, -1, text_input.Text))
            End If

            text_input.Text = ""
            e.Handled = True
        End If
    End Sub

    Private Sub HandleCommand(ByVal s As String, ByVal origin As Integer)
        Dim command As String = s.Remove(0, 1)

        Dim arg As String = ""
        If command.Contains(" ") = True Then
            arg = command.Remove(0, command.IndexOf(" ") + 1)
            command = command.Remove(command.IndexOf(" "))
        End If

        Select Case command.ToLower()
            Case "kick" 'Kicks a player from the server.
                Dim playerID As Integer = GetPlayerID(arg)
                If playerID > -1 Then
                    Try
                        SendToClient(playerID, New Package(Package.PackageTypes.Kicked, -1, "You got manually kicked from the server."))
                    Catch : End Try
                    Try
                        SendToAllClients(New Package(Package.PackageTypes.DestroyPlayer, -1, playerID.ToString()))
                    Catch : End Try
                    RemovePlayer(playerID)
                    Me.Invoke(New DUpdatePlayerList(AddressOf UpdatePlayerList))
                End If
            Case "stop" 'Stops the server and sends a message to everyone before doing so.
                Try
                    SendToAllClients(New Package(Package.PackageTypes.ServerClose, -1, ""))
                Catch : End Try
                WriteLocalLine("[INFO] Stopping the server...")
                Application.Exit()
            Case "restart"
                Try
                    SendToAllClients(New Package(Package.PackageTypes.ServerClose, -1, ""))
                Catch ex As Exception : End Try
                WriteLocalLine("[INFO] Restarting the server...")
                Application.Restart()
            Case "servermessage" 'Sets the server message to the given argument
                SetPropertyValue("ServerMessage", arg)
                SendToAllClients(New Package(Package.PackageTypes.ServerMessage, -1, GetPropertyValue("ServerMessage", "")))
            Case "weather"
                Me.Weather = CType(CInt(arg), WeatherTypes)
                WriteWorldInfo()
                SendToAllClients(GetWorldPackage())
            Case "season"
                Me.Season = CType(CInt(arg), SeasonTypes)
                WriteWorldInfo()
                SendToAllClients(GetWorldPackage())
            Case "whitelist", "white"
                If arg = "" Then
                    SetPropertyValue("Whitelist", CInt(Not CBool(GetPropertyValue("Whitelist", "0"))).ToString())
                Else
                    Dim exists As Boolean = False
                    For Each n As String In Me.Whitelist
                        If n.ToLower() = arg.ToLower() Then
                            exists = True
                            Exit For
                        End If
                    Next

                    If exists = False Then
                        Whitelist.Add(arg)
                        WriteLocalLine("[INFO] Whitelisted " & arg & ".")
                    End If

                    SaveLists()
                End If
            Case "blacklist", "black"
                If arg = "" Then
                    SetPropertyValue("Blacklist", CInt(Not CBool(GetPropertyValue("Blacklist", "0"))).ToString())
                Else
                    Dim exists As Boolean = False
                    For Each n As String In Me.Blacklist
                        If n.ToLower() = arg.ToLower() Then
                            exists = True
                            Exit For
                        End If
                    Next

                    If exists = False Then
                        Blacklist.Add(arg)
                        WriteLocalLine("[INFO] Blacklisted " & arg & ".")
                    End If

                    SaveLists()
                End If
            Case "unlist"
                Dim args() As String = arg.Split(CChar(" "))
                Dim name As String = args(1)
                Select Case args(0)
                    Case "white", "whitelist"
                        For i = 0 To Whitelist.Count - 1
                            If Whitelist(i).ToLower() = name.ToLower() Then
                                Whitelist.RemoveAt(i)
                                WriteLocalLine("[INFO] Removed " & name & " from the whitelist.")
                                Exit For
                            End If
                        Next
                    Case "black", "blacklist"
                        For i = 0 To Blacklist.Count - 1
                            If Blacklist(i).ToLower() = name.ToLower() Then
                                Blacklist.RemoveAt(i)
                                WriteLocalLine("[INFO] Removed " & name & " from the blacklist.")
                                Exit For
                            End If
                        Next
                End Select
                SaveLists()
            Case "timeoffset"
                TimeOffset = CInt(arg)
                CurrentTime = GetTime
                WriteWorldInfo()
                SendToAllClients(GetWorldPackage())
            Case "timepreset"
                SetPropertyValue("TimePreset", arg)
                CurrentTime = GetTime
                WriteWorldInfo()
                SendToAllClients(GetWorldPackage())
            Case "pm"
                Dim playerName As String = arg
                While PlayerNameExists(playerName) = False And playerName.Contains(" ") = True
                    playerName = playerName.Remove(playerName.LastIndexOf(" "))
                End While
                If playerName <> "" Then
                    Dim message As String = "[PM]" & arg.Remove(0, playerName.Length)
                    WriteLocalLine(message)
                    Try
                        SendToClient(GetPlayerID(playerName), New Package(Package.PackageTypes.PrivateMessage, -1, message))
                    Catch : End Try
                End If
            Case "allowop"
                Dim allowedValues() As String = {"false", "true", "0", "1"}
                If allowedValues.Contains(arg.ToLower()) = True Then
                    SetPropertyValue("AllowOP", arg)

                    If CBool(GetPropertyValue("AllowOP", "1")) = True Then
                        WriteLocalLine("[INFO] Activated operator control.")
                    Else
                        WriteLocalLine("[INFO] Deactivated operator control.")
                    End If
                End If
            Case "op"
                Dim playerName As String = arg
                If IsOP(playerName) = False Then
                    WriteLocalLine("[INFO] Added """ & playerName & """ as operator.")
                    OPList.Add(playerName)

                    SaveLists()
                Else
                    WriteLocalLine("[INFO] """ & playerName & """ is already an operator.")
                End If
            Case "deop"
                Dim playerName As String = arg
                If IsOP(playerName) = True Then
                    If GetPlayerName(origin) <> playerName Then
                        WriteLocalLine("[INFO] Removed """ & playerName & """ as operator.")
                        For i = 0 To OPList.Count - 1
                            If OPList(i).ToLower() = playerName.ToLower() Then
                                OPList.RemoveAt(i)
                                Exit For
                            End If
                        Next

                        SaveLists()
                    Else
                        Try
                            SendToClient(origin, New Package(Package.PackageTypes.ChatMessage, -1, "You cannot deop yourself!"))
                        Catch : End Try
                    End If
                Else
                    WriteLocalLine("[INFO] """ & playerName & """ is not an operator.")
                End If
            Case "mute"
                Dim playerName As String = arg
                If IsMuted(playerName) = False Then
                    WriteLocalLine("[INFO] Muted """ & playerName & """.")
                    MuteList.Add(playerName)

                    SaveLists()
                Else
                    WriteLocalLine("[INFO] """ & playerName & " is already muted.")
                End If
            Case "unmute"
                Dim playerName As String = arg
                If IsMuted(playerName) = True Then
                    WriteLocalLine("[INFO] Unmuted """ & playerName & """.")

                    For i = 0 To MuteList.Count - 1
                        If MuteList(i).ToLower() = playerName.ToLower() Then
                            MuteList.RemoveAt(i)
                            Exit For
                        End If
                    Next

                    SaveLists()
                Else
                    WriteLocalLine("[INFO] """ & playerName & " is not muted.")
                End If
            Case Else
                Try
                    SendToAllClients(New Package(Package.PackageTypes.ChatMessage, -1, s.Remove(0, 1)))
                Catch : End Try
        End Select
    End Sub

    Private ReadOnly Property DecSeparator() As String
        Get
            Return My.Application.Culture.NumberFormat.NumberDecimalSeparator
        End Get
    End Property

#Region "ContextMenu"

    Private Sub playerMenu_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles playerMenu.Opening
        If list_players.SelectedItems.Count = 0 Then
            e.Cancel = True
        Else
            playerMenu.Tag = list_players.SelectedItem.ToString()
            playerMenu.Tag = playerMenu.Tag.ToString().Remove(playerMenu.Tag.ToString().Length - 4, 4)
            playerMenu.Items(0).Text = playerMenu.Tag.ToString()

            If IsMuted(playerMenu.Tag.ToString()) = True Then
                playerMenu.Items(4).Text = "Unmute"
            Else
                playerMenu.Items(4).Text = "Mute"
            End If
            If IsWhitelisted(playerMenu.Tag.ToString()) = True Then
                playerMenu.Items(7).Text = "Remove from Whitelist"
            Else
                playerMenu.Items(7).Text = "Add to Whitelist"
            End If
            If IsBlacklisted(playerMenu.Tag.ToString()) = True Then
                playerMenu.Items(8).Text = "Remove from Blacklist"
            Else
                playerMenu.Items(8).Text = "Add to Blacklist"
            End If
            If IsOP(playerMenu.Tag.ToString()) = True Then
                playerMenu.Items(10).Text = "Remove Operator permissions"
            Else
                playerMenu.Items(10).Text = "Make Operator"
            End If
        End If
    End Sub

    Private Sub playerMenu_PM_Click(sender As Object, e As EventArgs) Handles playerMenu_PM.Click
        text_input.Text = "/pm " & playerMenu.Tag.ToString() & " "
        text_input.Focus()
    End Sub

    Private Sub playerMenu_Kick_Click(sender As Object, e As EventArgs) Handles playerMenu_Kick.Click
        text_input.Text = "/kick " & playerMenu.Tag.ToString()
        text_input.Focus()
    End Sub

    Private Sub playerMenu_whitelist_Click(sender As Object, e As EventArgs) Handles playerMenu_whitelist.Click
        If IsWhitelisted(playerMenu.Tag.ToString()) = True Then
            text_input.Text = "/unlist whitelist " & playerMenu.Tag.ToString()
        Else
            text_input.Text = "/whitelist " & playerMenu.Tag.ToString()
        End If
        text_input.Focus()
    End Sub

    Private Sub playerMenu_blacklist_Click(sender As Object, e As EventArgs) Handles playerMenu_blacklist.Click
        If IsBlacklisted(playerMenu.Tag.ToString()) = True Then
            text_input.Text = "/unlist blacklist " & playerMenu.Tag.ToString()
        Else
            text_input.Text = "/blacklist " & playerMenu.Tag.ToString()
        End If
        text_input.Focus()
    End Sub

    Private Sub playerMenu_operator_Click(sender As Object, e As EventArgs) Handles playerMenu_operator.Click
        If IsOP(playerMenu.Tag.ToString()) = True Then
            text_input.Text = "/deop " & playerMenu.Tag.ToString()
        Else
            text_input.Text = "/op " & playerMenu.Tag.ToString()
        End If
        text_input.Focus()
    End Sub

    Private Sub playerMenu_Mute_Click(sender As Object, e As EventArgs) Handles playerMenu_Mute.Click
        If IsMuted(playerMenu.Tag.ToString()) = True Then
            text_input.Text = "/unmute " & playerMenu.Tag.ToString()
        Else
            text_input.Text = "/mute " & playerMenu.Tag.ToString()
        End If
        text_input.Focus()
    End Sub

#End Region

#End Region

#Region "Properties and Lists"

    Private Properties As New List(Of ServerProperty)

    Private Sub LoadProperties()
        WriteLocalLine("[INFO] Loading properties")

        If System.IO.File.Exists(My.Application.Info.DirectoryPath & "\properties.dat") = False Then 'Properties.dat doesnt exist, create default one.
            WriteLocalLine("[WARNING] properties.dat does not exist!")
            WriteLocalLine("[INFO] Generating new properties file.")

            Dim s As String = "ServerName=P3D Server" & vbNewLine &
                "IP-Address=127.0.0.1" & vbNewLine &
                "Port=15124" & vbNewLine &
                "MaxPlayers=10" & vbNewLine &
                "BlackList=0" & vbNewLine &
                "WhiteList=0" & vbNewLine &
                "OfflineMode=0" & vbNewLine &
                "ServerMessage=" & vbNewLine &
                "Weather=" & vbNewLine &
                "Season=" & vbNewLine &
                "TimeOffset=" & vbNewLine &
                "TimePreset=" & vbNewLine &
                "GameMode=Pokemon 3D" & vbNewLine &
                "IdleKickTime=0" & vbNewLine &
                "WelcomeMessage=" & vbNewLine &
                "AllowOP=1"

            System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\properties.dat", s)
        End If

        If System.IO.File.Exists(My.Application.Info.DirectoryPath & "\whitelist.dat") = False Then 'whitelist.dat doesnt exist.
            WriteLocalLine("[WARNING] whitelist.dat does not exist!")
            System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\whitelist.dat", "")
        End If

        If System.IO.File.Exists(My.Application.Info.DirectoryPath & "\blacklist.dat") = False Then 'blacklist.dat doesnt exist.
            WriteLocalLine("[WARNING] blacklist.dat does not exist!")
            System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\blacklist.dat", "")
        End If

        If System.IO.File.Exists(My.Application.Info.DirectoryPath & "\ops.dat") = False Then 'ops.dat doesnt exist.
            WriteLocalLine("[WARNING] ops.dat does not exist!")
            System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\ops.dat", "")
        End If

        If System.IO.File.Exists(My.Application.Info.DirectoryPath & "\mutelist.dat") = False Then 'mutelist.dat doesnt exist.
            WriteLocalLine("[WARNING] mutelist.dat does not exist!")
            System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\mutelist.dat", "")
        End If

        Dim data() As String = System.IO.File.ReadAllLines(My.Application.Info.DirectoryPath & "\properties.dat")
        For Each line As String In data
            Me.Properties.Add(New ServerProperty(line))
        Next

        Me.Whitelist = System.IO.File.ReadAllLines(My.Application.Info.DirectoryPath & "\whitelist.dat").ToList()
        Me.Blacklist = System.IO.File.ReadAllLines(My.Application.Info.DirectoryPath & "\blacklist.dat").ToList()
        Me.OPList = System.IO.File.ReadAllLines(My.Application.Info.DirectoryPath & "\ops.dat").ToList()
        Me.MuteList = System.IO.File.ReadAllLines(My.Application.Info.DirectoryPath & "\mutelist.dat").ToList()

        Dim server_ip As IPAddress = IPAddress.Any
        If GetPropertyValue("IP-Address", "") <> "" Then
            server_ip = IPAddress.Parse(GetPropertyValue("IP-Address", ""))
        End If

        If CBool(GetPropertyValue("OfflineMode", "0")) = True Then
            WriteLocalLine("[WARNING] Starting the server in offline mode. Players with hacked profiles are able to join the server.")
        End If

        WriteLocalLine("[INFO] Starting with GameMode: " & GetPropertyValue("GameMode", "Pokemon 3D"))

        Me.ipEndPoint = New IPEndPoint(server_ip, CInt(GetPropertyValue("Port", "15124")))
        Me.MaxPlayers = CInt(GetPropertyValue("MaxPlayers", "10"))

        Dim tOffset As String = GetPropertyValue("TimeOffset", "")
        If IsNumeric(tOffset) = True Then
            Me.TimeOffset = CInt(tOffset)
        End If
    End Sub

    Private Sub SaveProperties()
        Dim s As String = ""
        For Each p As ServerProperty In Me.Properties
            If s <> "" Then
                s &= vbNewLine
            End If
            s &= p.name & "=" & p.value
        Next
        System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\properties.dat", s)
    End Sub

    Private Sub SaveLists()
        Dim w As String = ""
        For Each sW As String In Me.Whitelist
            If w <> "" Then
                w &= vbNewLine
            End If
            w &= sW
        Next
        Dim b As String = ""
        For Each sB As String In Me.Blacklist
            If b <> "" Then
                b &= vbNewLine
            End If
            b &= sB
        Next
        Dim o As String = ""
        For Each sO As String In Me.OPList
            If o <> "" Then
                o &= vbNewLine
            End If
            o &= sO
        Next
        Dim m As String = ""
        For Each sM As String In Me.MuteList
            If m <> "" Then
                m &= vbNewLine
            End If
            m &= sM
        Next

        System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\whitelist.dat", w)
        System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\blacklist.dat", b)
        System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\ops.dat", o)
        System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\mutelist.dat", m)
    End Sub

    Class ServerProperty

        Public name As String
        Public value As String

        Public Sub New(ByVal data As String)
            Me.name = data.Remove(data.IndexOf("="))
            Me.value = data.Remove(0, data.IndexOf("=") + 1)
        End Sub

        Public Sub New(ByVal name As String, ByVal value As String)
            Me.name = name
            Me.value = value
        End Sub

    End Class

    ''' <summary>
    ''' Returns the content of a property with the given name. If the property cannot be found, it returns the default value.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <param name="defaultValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPropertyValue(ByVal name As String, ByVal defaultValue As String) As String
        For Each p As ServerProperty In Me.Properties
            If p.name.ToLower() = name.ToLower() Then
                Return p.value
            End If
        Next
        Return defaultValue
    End Function

    Private Sub SetPropertyValue(ByVal name As String, ByVal value As String)
        For Each p As ServerProperty In Me.Properties
            If p.name.ToLower() = name.ToLower() Then
                p.value = value
                Exit Sub
            End If
        Next
        Me.Properties.Add(New ServerProperty(name, value))
    End Sub

#End Region

#Region "ExtraStuff"

    ''' <summary>
    ''' A 3 dimensional vector class to simulate the Microsoft.XNA.Framework.Graphics.VertexElementFormat.Vector3 class.
    ''' </summary>
    ''' <remarks></remarks>
    Class Vector3

        Public X As Single
        Public Y As Single
        Public Z As Single

        Public Sub New(ByVal val As Single)
            Me.X = val
            Me.Y = val
            Me.Z = val
        End Sub

        Public Sub New(ByVal x As Single, ByVal y As Single, ByVal z As Single)
            Me.X = x
            Me.Y = y
            Me.Z = z
        End Sub

    End Class

    ''' <summary>
    ''' this class is a package that consists of the package ID, a origin ID (-1 for server) and the package data.
    ''' </summary>
    ''' <remarks></remarks>
    Class Package

        Public Enum PackageTypes As Integer
            GameData = 0
            PlayData = 1
            PrivateMessage = 2
            ChatMessage = 3
            Kicked = 4
            ID = 7
            CreatePlayer = 8
            DestroyPlayer = 9
            ServerClose = 10
            ServerMessage = 11
            WorldData = 12
            Ping = 13
            GamestateMessage = 14

            TradeRequest = 30
            TradeJoin = 31
            TradeQuit = 32

            TradeOffer = 33
            TradeStart = 34

            BattleRequest = 50
            BattleJoin = 51
            BattleQuit = 52

            BattleOffer = 53
            BattleStart = 54

            BattleClientData = 55
            BattleHostData = 56
            BattlePokemonData = 57

            ServerInfoData = 98
            ServerDataRequest = 99
        End Enum

        Public Origin As Integer
        Public ID As Integer
        Public Data As String

        Public Overrides Function ToString() As String
            Return ID.ToString() & "|" & Origin.ToString() & "|" & Data
        End Function

        Public Sub New(ByVal FullData As String)
            Me.ID = CInt(FullData.Remove(FullData.IndexOf("|")))

            Dim oStr As String = FullData.Remove(0, FullData.IndexOf("|") + 1)
            Me.Origin = CInt(oStr.Remove(oStr.IndexOf("|")))

            Data = oStr.Remove(0, oStr.IndexOf("|") + 1)
        End Sub

        Public Sub New(ByVal PackageType As PackageTypes, ByVal Origin As Integer, ByVal Data As String)
            Me.ID = PackageType
            Me.Origin = Origin
            Me.Data = Data
        End Sub

        Public Sub New(ByVal ID As Integer, ByVal Origin As Integer, ByVal Data As String)
            Me.ID = ID
            Me.Origin = Origin
            Me.Data = Data
        End Sub

        Public Property PackageType As PackageTypes
            Get
                Return Me.ID
            End Get
            Set(value As PackageTypes)
                Me.ID = value
            End Set
        End Property

    End Class

#End Region

#Region "GameMechanics"

    Private Season As SeasonTypes = SeasonTypes.Spring
    Private CurrentTime As DayTime = DayTime.Day
    Private Weather As WeatherTypes = WeatherTypes.Underwater
    Private TimeOffset As Integer = 0

    Public Enum SeasonTypes As Integer
        Winter = 0
        Spring = 1
        Summer = 2
        Fall = 3
    End Enum

    Public Enum WeatherTypes As Integer
        Clear = 0
        Rain = 1
        Snow = 2
        Underwater = 3
        Sunny = 4
        Fog = 5
        Thunderstorm = 6
        Sandstorm = 7
        Ash = 8
        Blizzard = 9
    End Enum

    Public Enum DayTime As Integer
        Night = 0
        Morning = 1
        Day = 2
        Evening = 3
    End Enum

    Public ReadOnly Property WeekOfYear() As Integer
        Get
            Return CInt(((My.Computer.Clock.LocalTime.DayOfYear - (My.Computer.Clock.LocalTime.DayOfWeek - 1)) / 7) + 1)
        End Get
    End Property

    Public ReadOnly Property CurrentSeason() As SeasonTypes
        Get
            Select Case WeekOfYear Mod 4
                Case 1
                    Return SeasonTypes.Winter
                Case 2
                    Return SeasonTypes.Spring
                Case 3
                    Return SeasonTypes.Summer
                Case 0
                    Return SeasonTypes.Fall
            End Select
            Return SeasonTypes.Summer
        End Get
    End Property

    Public ReadOnly Property GetTime() As DayTime
        Get
            Dim time As DayTime = DayTime.Day

            Dim preset As String = GetPropertyValue("TimePreset", "")
            If IsNumeric(preset) = True Then
                time = CType(CInt(preset), DayTime)
            Else
                With My.Computer.Clock.LocalTime
                    Select Case CurrentSeason
                        Case SeasonTypes.Winter
                            If .Hour > 16 Or .Hour < 9 Then
                                time = DayTime.Night
                            ElseIf .Hour > 8 And .Hour < 11 Then
                                time = DayTime.Morning
                            ElseIf .Hour > 10 And .Hour < 15 Then
                                time = DayTime.Day
                            ElseIf .Hour > 14 And .Hour < 17 Then
                                time = DayTime.Evening
                            End If
                        Case SeasonTypes.Spring
                            If .Hour > 19 Or .Hour < 7 Then
                                time = DayTime.Night
                            ElseIf .Hour > 6 And .Hour < 10 Then
                                time = DayTime.Morning
                            ElseIf .Hour > 9 And .Hour < 16 Then
                                time = DayTime.Day
                            ElseIf .Hour > 16 And .Hour < 20 Then
                                time = DayTime.Evening
                            End If
                        Case SeasonTypes.Summer
                            If .Hour > 21 Or .Hour < 5 Then
                                time = DayTime.Night
                            ElseIf .Hour > 4 And .Hour < 9 Then
                                time = DayTime.Morning
                            ElseIf .Hour > 8 And .Hour < 18 Then
                                time = DayTime.Day
                            ElseIf .Hour > 17 And .Hour < 22 Then
                                time = DayTime.Evening
                            End If
                        Case SeasonTypes.Fall
                            If .Hour > 19 Or .Hour < 7 Then
                                time = DayTime.Night
                            ElseIf .Hour > 6 And .Hour < 8 Then
                                time = DayTime.Morning
                            ElseIf .Hour > 7 And .Hour < 14 Then
                                time = DayTime.Day
                            ElseIf .Hour > 13 And .Hour < 20 Then
                                time = DayTime.Evening
                            End If
                    End Select
                End With
            End If

            Dim timeInt As Integer = CInt(time) + TimeOffset
            While timeInt > 3
                timeInt -= 4
            End While

            Return CType(timeInt, DayTime)
        End Get
    End Property

    Public Function GetRegionWeather(ByVal Season As SeasonTypes) As WeatherTypes
        Dim Random As New System.Random()
        Dim r As Integer = Random.Next(0, 100)

        Select Case Season
            Case SeasonTypes.Winter
                If r < 20 Then
                    Return WeatherTypes.Rain
                ElseIf r >= 20 And r < 50 Then
                    Return WeatherTypes.Clear
                Else
                    Return WeatherTypes.Snow
                End If
            Case SeasonTypes.Spring
                If r < 5 Then
                    Return WeatherTypes.Snow
                ElseIf r >= 5 And r < 40 Then
                    Return WeatherTypes.Rain
                Else
                    Return WeatherTypes.Clear
                End If
            Case SeasonTypes.Summer
                If r < 10 Then
                    Return WeatherTypes.Rain
                Else
                    Return WeatherTypes.Clear
                End If
            Case SeasonTypes.Fall
                If r < 5 Then
                    Return WeatherTypes.Snow
                ElseIf r >= 5 And r < 80 Then
                    Return WeatherTypes.Rain
                Else
                    Return WeatherTypes.Clear
                End If
        End Select

        Return WeatherTypes.Clear
    End Function

    Private Sub SetWorldProperties()
        CurrentTime = GetTime
        If GetPropertyValue("Season", "") <> "" Then
            Season = CType(CInt(GetPropertyValue("Season", "")), SeasonTypes)
        Else
            Season = CurrentSeason
        End If
        If GetPropertyValue("Weather", "") <> "" Then
            Weather = CType(CInt(GetPropertyValue("Weather", "")), WeatherTypes)
        Else
            Weather = GetRegionWeather(Season)
        End If

        WriteWorldInfo()
    End Sub

    Private Sub WriteWorldInfo()
        WriteLocalLine("[INFO] Current season: " & Season.ToString() & ", Weather: " & Weather.ToString() & ", Time: " & CurrentTime.ToString())
        Me.ExportServerData()
    End Sub

    Private Sub SetTime()
        If CurrentTime <> GetTime Then
            If GetPropertyValue("Weather", "") = "" Then
                Weather = GetRegionWeather(Season)
            End If
        End If

        CurrentTime = GetTime
        SendToAllClients(GetWorldPackage())
    End Sub

    Private Function GetWorldPackage() As Package
        Return New Package(Package.PackageTypes.WorldData, -1, CInt(CurrentTime) & "," & CInt(Season) & "," & CInt(Weather))
    End Function

#End Region

    Dim threadHistory As New List(Of Integer)
    Const StatHeight As Integer = 60
    Dim CriticalThreads As Integer = 8000

    Private Sub DrawThreads()
        Me.Panel1_Paint()

        Me.radio_threads.Text = "Threads: " & Process.GetCurrentProcess().Threads.Count.ToString()
        Me.radio_RAMusage.Text = "RAM usage: " & Math.Round(RAMCounter.NextValue(), 0) & " %"
    End Sub

    Private Sub Panel1_Paint()
        If radio_RAMusage.Checked = True Then
            threadHistory.Add(RAMCounter.NextValue() * 100.0F)
        Else
            threadHistory.Add(Process.GetCurrentProcess().Threads.Count)
        End If

        Dim g = Graphics.FromHwnd(Panel1.Handle)

        With g
            Dim start As Integer = threadHistory.Count - 90
            Dim count As Integer = threadHistory.Count - 1
            If start < 0 Then
                start = 0
                count = threadHistory.Count - 1
            End If

            Dim max As Integer = Integer.MinValue

            For i = 0 To count - start
                Dim v As Integer = threadHistory(i + start)
                If max < v Then
                    max = v
                End If
            Next

            For i = 0 To count - start
                Dim v As Integer = threadHistory(i + start)

                Dim s As Integer = StatHeight / max * v
                Dim c As Single = 1.0F
                If v <= CriticalThreads Then
                    c = CSng(v / CriticalThreads)
                End If

                Dim p As New Pen(Color.FromArgb(CInt(255 * c), 0, 0), 2.0F)

                .DrawLine(New Pen(Brushes.White, 2.0F), New Point(20 + i * 2, 20), New Point(20 + i * 2, 20 + (StatHeight - s)))
                .DrawLine(p, New Point(20 + i * 2, 20 + (StatHeight - s)), New Point(20 + i * 2, StatHeight + 20))
            Next
        End With

        g.Dispose()
    End Sub

    Private Sub CheckedChanged(sender As Object, e As EventArgs) Handles radio_threads.CheckedChanged, radio_RAMusage.CheckedChanged
        threadHistory.Clear()
        Panel1.Invalidate()
        If radio_threads.Checked = True Then
            CriticalThreads = 8000
        Else
            CriticalThreads = 50000000
        End If
    End Sub

End Class