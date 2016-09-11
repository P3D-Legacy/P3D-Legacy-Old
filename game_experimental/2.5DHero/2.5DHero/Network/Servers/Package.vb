Namespace Servers

    ''' <summary>
    ''' A class to handle the network package protocol.
    ''' </summary>
    ''' <remarks>Call PackageHandler to handle incoming packages.</remarks>
    Public Class Package

#Region "Fields and Enums"

        Public Enum PackageTypes As Integer
            GameData = 0

            ''' <summary>
            ''' Not used anymore, use GameData instead.
            ''' </summary>
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

        Public Enum ProtocolTypes As Integer
            TCP = 0
            UDP = 1
        End Enum

        Private _dataModel As DataModel.Json.Network.PackageModel

        Private _protocolType As ProtocolTypes = ProtocolTypes.TCP 'Only to remember which protocol to use when sending the data.
        Private _isValid As Boolean = True

#End Region

#Region "Properties"

        ''' <summary>
        ''' The PackageType of this Package.
        ''' </summary>
        Public ReadOnly Property PackageType() As PackageTypes
            Get
                Return CType(_dataModel.PackageType, PackageTypes)
            End Get
        End Property

        ''' <summary>
        ''' The Origin ID of this Package.
        ''' </summary>
        Public ReadOnly Property Origin() As Integer
            Get
                Return _dataModel.Origin
            End Get
        End Property

        ''' <summary>
        ''' The DataItems of this Package.
        ''' </summary>
        Public ReadOnly Property DataItems() As List(Of String)
            Get
                Return _dataModel.Data
            End Get
        End Property

        ''' <summary>
        ''' Returns if the data used to create this Package was valid.
        ''' </summary>
        Public ReadOnly Property IsValid() As Boolean
            Get
                Return _isValid
            End Get
        End Property

        ''' <summary>
        ''' The protocol version of this package.
        ''' </summary>
        Public ReadOnly Property ProtocolVersion() As String
            Get
                Return _dataModel.ProtocolVersion
            End Get
        End Property

        ''' <summary>
        ''' The protocol type (TCP or UDP) this package is using when sending data.
        ''' </summary>
        Public Property ProtocolType() As ProtocolTypes
            Get
                Return _protocolType
            End Get
            Set(value As ProtocolTypes)
                _protocolType = value
            End Set
        End Property

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Creates a new instance of the Package class.
        ''' </summary>
        ''' <param name="FullData">The raw Package data.</param>
        Public Sub New(ByVal FullData As String)
            Try
                _dataModel = DataModel.Json.JsonDataModel.FromString(Of DataModel.Json.Network.PackageModel)(FullData)
                _isValid = True
            Catch ex As Exception
                _dataModel = New DataModel.Json.Network.PackageModel()
                _isValid = False
            End Try
        End Sub

        ''' <summary>
        ''' Creates a new instance of the Package class.
        ''' </summary>
        ''' <param name="PackageType">The PackageType of the new Package.</param>
        ''' <param name="Origin">The Origin computer ID of the new Package.</param>
        ''' <param name="ProtocolType">The ProtocolType this package is going to use.</param>
        ''' <param name="DataItems">An array of DataItems the Package contains.</param>
        Public Sub New(ByVal PackageType As PackageTypes, ByVal Origin As Integer, ByVal ProtocolType As ProtocolTypes, ByVal DataItems As List(Of String))
            _dataModel = New DataModel.Json.Network.PackageModel() With
            {
                .ProtocolVersion = ServersManager.PROTOCOLVERSION,
                .Data = DataItems,
                .Origin = Origin,
                .PackageType = PackageType
            }
            _protocolType = ProtocolType
        End Sub

        ''' <summary>
        ''' Creates a new instance of the Package class.
        ''' </summary>
        ''' <param name="PackageType">The PackageType of the new Package.</param>
        ''' <param name="Origin">The Origin computer ID of the new Package.</param>
        ''' <param name="ProtocolType">The ProtocolType this package is going to use.</param>
        Public Sub New(ByVal PackageType As PackageTypes, ByVal Origin As Integer, ByVal ProtocolType As ProtocolTypes)
            Me.New(PackageType, Origin, ProtocolType, New List(Of String))
        End Sub

        ''' <summary>
        ''' Creates a new instance of the Package class with a single data item.
        ''' </summary>
        ''' <param name="Packagetype">The PackageType of the new Package.</param>
        ''' <param name="Origin">The Origin computer ID of the new Package.</param>
        ''' <param name="ProtocolType">The ProtocolType this package is going to use.</param>
        ''' <param name="DataItem">The single Data Item to create the package with.</param>
        Public Sub New(ByVal Packagetype As PackageTypes, ByVal Origin As Integer, ByVal ProtocolType As ProtocolTypes, ByVal DataItem As String)
            Me.New(Packagetype, Origin, ProtocolType, {DataItem}.ToList())
        End Sub

#End Region

#Region "Methods"

        ''' <summary>
        ''' Returns the raw Package data from the members of this instance.
        ''' </summary>
        Public Overrides Function ToString() As String
            Return _dataModel.ToString()
        End Function

        ''' <summary>
        ''' Gives this package to the PackageHandler.
        ''' </summary>
        Public Sub Handle()
            PackageHandler.HandlePackage(Me)
        End Sub

#End Region

    End Class

End Namespace