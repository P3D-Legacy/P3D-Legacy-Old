''' <summary>
''' A map stub which holds basic level data and can be applied to any level.
''' </summary>
Public Class MapStub

    Private _mapFile As String

    Private _entities As New List(Of Entity)

    Private _floors As New List(Of Entity)

    Private _offsetMaps As New List(Of MapStub)

    Private _shaders As New List(Of Shader)

    Private _backDrops As New List(Of BackdropRenderer)

    Private _levelInformation As LevelInformation

    Private _actionInformation As ActionInformation

    Private _loaded As Boolean = False

    ''' <summary>
    ''' Creates a new instance of the MapStub class.
    ''' </summary>
    ''' <param name="mapFile">The map file that relates to this map stub.</param>
    Public Sub New(ByVal mapFile As String)
        _mapFile = mapFile
        _loaded = False
    End Sub

    Public Sub ApplyToLevel(ByVal level As Level)
        InternalApplyToLevel(level, False, Vector3.Zero)
    End Sub

    Public Sub ApplyToLevel(ByVal level As Level, ByVal offset As Vector3)
        InternalApplyToLevel(level, True, offset)
    End Sub

    Private Sub InternalApplyToLevel(ByVal level As Level, ByVal isOffset As Boolean, ByVal offset As Vector3)

    End Sub

#Region "Loading section"

    'We load the map from file here.

    Private Sub LoadFromFile()
        _loaded = False

        'Check if the file exists:
        If IO.File.Exists(GetFullMapFilePath()) = True Then

        End If
    End Sub

#End Region

    Public Function GetFullMapFilePath() As String
        Return GameModeManager.GetMapPath(_mapFile)
    End Function

End Class

Public Class LevelInformation

    Public Name As String = ""
    Public MusicLoop As String = ""
    Public WildPokemonFloor As Boolean = False
    Public ShowOverworldPokemon As Boolean = True
    Public CurrentRegion As String = "Johto"
    Public HiddenAbilityChance As Integer = 0

    Public Sub ApplyToLevel(ByVal level As Level)

    End Sub

End Class

Public Class ActionInformation

    Public CanTeleport As Boolean = False
    Public CanDig As Boolean = False
    Public CanFly As Boolean = False
    Public RideType As Integer = 0
    Public EnvironmentType As Integer = 0
    Public WeatherType As Integer = 0
    Public Lighting As Integer = 1
    Public IsDark As Boolean = False
    Public TerrainType As Terrain.TerrainTypes = Terrain.TerrainTypes.Plain
    Public IsSafariZone As Boolean = False
    Public BugCatchingContest As Boolean = False
    Public MapScript As String = ""
    Public RadioChannels As List(Of Decimal)
    Public BattleMapData As String = ""

    Public Sub ApplyToLevel(ByVal level As Level)

    End Sub

End Class