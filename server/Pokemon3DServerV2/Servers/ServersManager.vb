Namespace Servers

    Public Class ServersManager

#Region "Fields and constants"

        ''' <summary>
        ''' The version of the protocol used for packages.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PROTOCOLVERSION As String = "0.5"
        Public Const ID As Integer = -1

        Private _PlayerCollection As PlayerCollection = Nothing
        Private _ServerHost As ServerHost = Nothing
        Private _Logger As Logger = Nothing
        Private _ListManager As ListManager = Nothing
        Private _PropertyCollection As Properties.PropertyCollection = Nothing
        Private _World As Game.World = Nothing

#End Region

#Region "Delegates"

        Private Delegate Sub DWriteLine(ByVal Line As String)
        Private Delegate Sub DUpdatePlayerList()
        Private Delegate Sub DClearOutput()

#End Region

#Region "Properties"

        Public ReadOnly Property PlayerCollection() As PlayerCollection
            Get
                Return Me._PlayerCollection
            End Get
        End Property

        Public ReadOnly Property ServerHost() As ServerHost
            Get
                Return Me._ServerHost
            End Get
        End Property

        Public ReadOnly Property Logger() As Logger
            Get
                Return Me._Logger
            End Get
        End Property

        Public ReadOnly Property ListManager() As ListManager
            Get
                Return Me._ListManager
            End Get
        End Property

        Public ReadOnly Property PropertyCollection() As Properties.PropertyCollection
            Get
                Return Me._PropertyCollection
            End Get
        End Property

        ''' <summary>
        ''' Returns the info package for the current server instance.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Data: 0: Current player count, 1: Max player count, 2: Servername, 3: Servermessage</remarks>
        Public ReadOnly Property ServerInfoPackage() As Package
            Get
                'Data structure:
                '0: Current player count
                '1: Max player count
                '2: Server name
                '3: Server message
                '4: Player List (names)

                Dim infoPackage As New Package(Package.PackageTypes.ServerInfoData, -1, Package.ProtocolTypes.TCP, {Me._PlayerCollection.Count.ToString(),
                                                                                                        Me._PropertyCollection.GetPropertyValue("MaxPlayers", "10"),
                                                                                                        Me._PropertyCollection.GetPropertyValue("ServerName", "P3D Server"),
                                                                                                        Me._PropertyCollection.GetPropertyValue("ServerMessage", "")}.ToList())

                For Each p In Me._PlayerCollection
                    infoPackage.DataItems.Add(p.Name)
                Next

                Return infoPackage
            End Get
        End Property

        Public ReadOnly Property World() As Game.World
            Get
                Return Me._World
            End Get
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
            Me._Logger = New Logger()
            Me._PropertyCollection = New Properties.PropertyCollection()
            Me._PlayerCollection = New PlayerCollection()
            Me._ServerHost = New ServerHost()
            Me._ListManager = New ListManager()
            Me._World = New Game.World()
        End Sub

#End Region

#Region "Methods"

        ''' <summary>
        ''' Writes a new line to the GUI and the log file.
        ''' </summary>
        ''' <param name="Line">The line to write.</param>
        ''' <remarks></remarks>
        Public Sub WriteLine(ByVal Line As String)
            Basic.MainformReference.Invoke(New DWriteLine(AddressOf Basic.MainformReference.WriteGUILine), Line)

            Me._Logger.LogLine(Line)
        End Sub

        ''' <summary>
        ''' Clears the output textbox.
        ''' </summary>
        Public Sub ClearOutput()
            Basic.MainformReference.Invoke(New DClearOutput(AddressOf Basic.MainformReference.ClearOutput))
        End Sub

        ''' <summary>
        ''' Updates the player list control with the PlayerCollection enumaration.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UpdatePlayerList()
            Basic.MainformReference.Invoke(New DUpdatePlayerList(AddressOf Basic.MainformReference.UpdatePlayerList))
        End Sub

        Public Sub Host()
            Me._PropertyCollection.LoadFromFile()
            Me._ListManager.Load()
            Me._World.Initialize()

            'Print warning for offline servers:
            If CBool(Me._PropertyCollection.GetPropertyValue("OfflineMode", "0")) = True Then
                WriteLine(ServerMessages.SERVER_OFFLINEWARNING1)
                WriteLine(ServerMessages.SERVER_OFFLINEWARNING2)
            End If

            'Print GameMode:
            WriteLine(String.Format(ServerMessages.SERVER_STARTGAMEMODEINFO, Me._PropertyCollection.GetPropertyValue("GameMode", "Pokemon 3D")))

            Me._ServerHost.StartHost()

            'Start API:
            API.Start()
        End Sub

        ''' <summary>
        ''' Aborts the hosting process and closes the application.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Close()

        End Sub

#End Region

    End Class

End Namespace