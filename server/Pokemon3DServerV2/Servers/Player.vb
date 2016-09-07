Option Strict On

Namespace Servers

    Public Class Player

#Region "Fields and constants"

        Public Const PLAYERDATAITEMSCOUNT As Integer = 15

        Private _gameVersion As String = ""
        Private _isGameJoltPlayer As String = "0"

        Private _serversID As Integer = 999
        Private _initialized As Boolean = False

        Private _gameJoltID As String = ""

        Private _name As String = ""
        Private _position As String = ""
        Private _skin As String = ""
        Private _facing As String = "0"
        Private _moving As String = "0"
        Private _levelFile As String = ""
        Private _decimalSeparator As String = ","
        Private _busyType As String = "0"
        Private _gameMode As String = ""

        Private _pokemonPosition As String = ""
        Private _pokemonFacing As String = "0"
        Private _pokemonSkin As String = ""
        Private _pokemonVisible As String = "0"

        Private _lastPackage As Package = Nothing

        Private _clientConnection As ClientConnection

#End Region

#Region "Properties"

        Public ReadOnly Property Moving() As String
            Get
                Return Me._moving
            End Get
        End Property

        Public ReadOnly Property LevelFile() As String
            Get
                Return Me._levelFile
            End Get
        End Property

        Public ReadOnly Property BusyType() As String
            Get
                Return Me._busyType
            End Get
        End Property

        Public Property ServersID() As Integer
            Get
                Return Me._serversID
            End Get
            Set(value As Integer)
                Me._serversID = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return Me._name
            End Get
            Set(value As String)
                Me._name = value
            End Set
        End Property

        Public ReadOnly Property GameJoltId() As String
            Get
                Return Me._gameJoltID
            End Get
        End Property

        Public ReadOnly Property GameMode() As String
            Get
                Return Me._gameMode
            End Get
        End Property

        Public ReadOnly Property IsGameJoltPlayer() As Boolean
            Get
                Return CBool(Me._isGameJoltPlayer)
            End Get
        End Property

        Public ReadOnly Property Initialized() As Boolean
            Get
                Return Me._initialized
            End Get
        End Property

        Public ReadOnly Property Position() As String
            Get
                Return Me._position
            End Get
        End Property

        Public ReadOnly Property Skin() As String
            Get
                Return Me._skin
            End Get
        End Property

        Public ReadOnly Property Facing() As String
            Get
                Return Me._facing
            End Get
        End Property

        Public ReadOnly Property PokemonPosition() As String
            Get
                Return Me._pokemonPosition
            End Get
        End Property

        Public ReadOnly Property PokemonFacing() As String
            Get
                Return Me._pokemonFacing
            End Get
        End Property

        Public ReadOnly Property PokemonSkin() As String
            Get
                Return Me._pokemonSkin
            End Get
        End Property

        Public ReadOnly Property PokemonVisible() As String
            Get
                Return Me._pokemonVisible
            End Get
        End Property

        ''' <summary>
        ''' The client connection that is linked to this player instance.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Networking() As ClientConnection
            Get
                Return Me._clientConnection
            End Get
        End Property

        ''' <summary>
        ''' The name of this player on lists. If it's a GameJolt player, the GameJolt ID will be used, prefaced with the "@" character.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ListName() As String
            Get
                If CBool(Me._isGameJoltPlayer) = True Then
                    Return "@" & Me._gameJoltID
                Else
                    Return Me._name
                End If
            End Get
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
            Me._clientConnection = New ClientConnection(Me)
        End Sub

#End Region

#Region "Methods"

        Public Sub ApplyNewData(ByVal p As Package)
            '---General information---
            '0: Active gamemode
            '1: isgamejoltsave
            '2: GameJoltID
            '3: DecimalSeparator

            '---Player Information---
            '4: playername
            '5: levelfile
            '6: position
            '7: facing
            '8: moving
            '9: skin
            '10: busytype

            '---OverworldPokemon---
            '11: Visible
            '12: Position
            '13: Skin
            '14: facing

            Dim d() As String = p.DataItems.ToArray()

            For i = 0 To PLAYERDATAITEMSCOUNT - 1
                Dim value As String = d(i)
                If value <> "" Then
                    Select Case i
                        Case 0 '0: Active gamemode
                            Me._gameMode = value
                        Case 1 '1: isgamejoltsave
                            Me._isGameJoltPlayer = value
                        Case 2 '2: GameJoltID
                            Me._gameJoltID = value
                        Case 3 '3: DecimalSeparator
                            Me._decimalSeparator = value
                        Case 4 '4: playername
                            Me._name = value
                        Case 5 '5: levelfile
                            Me._levelFile = value
                        Case 6 '6: position
                            Me._position = value
                        Case 7 '7: facing
                            Me._facing = value
                        Case 8 '8: moving
                            Me._moving = value
                        Case 9 '9: skin
                            Me._skin = value
                        Case 10 '10: busytype
                            Me._busyType = value
                            Basic.ServersManager.UpdatePlayerList()
                        Case 11 '11: Visible
                            Me._pokemonVisible = value
                        Case 12 '12: Position
                            Me._pokemonPosition = value
                        Case 13 '13: Skin
                            Me._pokemonSkin = value
                        Case 14 '14: facing
                            Me._pokemonFacing = value
                    End Select
                End If
            Next

            Me._initialized = True
        End Sub

        Public Function PlayerDataPackage(ByVal FullPackage As Boolean) As Package
            '---General information---
            '0: Active gamemode
            '1: isgamejoltsave
            '2: GameJoltID
            '3: DecimalSeparator

            Dim dataItems As New List(Of String)

            AddToDataItems(dataItems, Me._gameMode, 0, FullPackage)
            AddToDataItems(dataItems, Me._isGameJoltPlayer, 1, FullPackage)
            AddToDataItems(dataItems, Me._gameJoltID, 2, FullPackage)
            AddToDataItems(dataItems, Me._decimalSeparator, 3, FullPackage)

            '---Player Information---
            '4: playername
            '5: levelfile
            '6: position
            '7: facing
            '8: moving
            '9: skin
            '10: busytype

            AddToDataItems(dataItems, Me._name, 4, FullPackage)
            AddToDataItems(dataItems, Me._levelFile, 5, FullPackage)
            AddToDataItems(dataItems, Me._position, 6, FullPackage)
            AddToDataItems(dataItems, Me._facing.ToString(), 7, FullPackage)
            AddToDataItems(dataItems, Me._moving, 8, FullPackage)
            AddToDataItems(dataItems, Me._skin, 9, FullPackage)
            AddToDataItems(dataItems, Me._busyType, 10, FullPackage)

            '---OverworldPokemon---
            '11: Visible
            '12: Position
            '13: Skin
            '14: facing

            AddToDataItems(dataItems, Me._pokemonVisible, 11, FullPackage)
            AddToDataItems(dataItems, Me._pokemonPosition, 12, FullPackage)
            AddToDataItems(dataItems, Me._pokemonSkin, 13, FullPackage)
            AddToDataItems(dataItems, Me._pokemonFacing, 14, FullPackage)

            Dim p As New Package(Package.PackageTypes.GameData, Me._serversID, Package.ProtocolTypes.UDP, dataItems)

            If FullPackage = False Then
                ApplyLastPackage(p)
            End If

            Return p
        End Function

        Private Sub AddToDataItems(ByRef l As List(Of String), ByVal value As String, ByVal listIndex As Integer, ByVal forceAdd As Boolean)
            If forceAdd = True Then
                l.Add(value)
                Exit Sub
            End If

            Dim insertValue As String = value

            If Not Me._lastPackage Is Nothing Then
                If Me._lastPackage.DataItems.Count - 1 >= listIndex Then
                    If Me._lastPackage.DataItems(listIndex) = value Then
                        insertValue = ""
                    End If
                End If
            End If

            l.Add(insertValue)
        End Sub

        Private Sub ApplyLastPackage(ByVal newP As Package)
            If Me._lastPackage Is Nothing Then
                Me._lastPackage = newP
            Else
                For i = 0 To newP.DataItems.Count - 1
                    If newP.DataItems(i) <> "" Then
                        Me._lastPackage.DataItems(i) = newP.DataItems(i)
                    End If
                Next
            End If
        End Sub

        ''' <summary>
        ''' Kicks this player from the server.
        ''' </summary>
        ''' <param name="ClientReason">The kick reason that gets send to the client.</param>
        ''' <remarks></remarks>
        Public Sub Kick(ByVal ClientReason As String)
            Dim t As New Threading.Thread(AddressOf Me.InternalKick)
            t.IsBackground = True
            t.Start(ClientReason)
        End Sub

        Private Sub InternalKick(ByVal ClientReasonObject As Object)
            Dim ClientReason As String = CType(ClientReasonObject, String)

            If Basic.ServersManager.PlayerCollection.Contains(Me) = True Then
                Basic.ServersManager.PlayerCollection.Remove(Me)
            End If
            Basic.ServersManager.ServerHost.SendToAllPlayers(New Package(Package.PackageTypes.DestroyPlayer, -1, Package.ProtocolTypes.TCP, Me.ServersID.ToString()))

            Try
                Me._clientConnection.SendUnthreadedPackage(New Package(Package.PackageTypes.Kicked, -1, Package.ProtocolTypes.TCP, ClientReason))
            Catch : End Try

            Me._clientConnection.StopNetworking()

            Basic.ServersManager.WriteLine(String.Format(ServerMessages.SERVER_PLAYERKICKED, Me._name))
            Basic.ServersManager.UpdatePlayerList()
        End Sub

        Public Function GetBusyTypeName() As String
            Select Case Me._busyType
                Case "0"
                    Return ""
                Case "1"
                    Return " - " & ServerMessages.PLAYERLIST_BATTLING
                Case "2"
                    Return " - " & ServerMessages.PLAYERLIST_CHATTING
                Case "3"
                    Return " - " & ServerMessages.PLAYERLIST_INACTIVE
            End Select
            Return ""
        End Function

#End Region

    End Class

End Namespace