Namespace Game

    ''' <summary>
    ''' This class simulates the game's environments: Season, Weather and Time.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class World

#Region "Enums"

        ''' <summary>
        ''' Seasons as defined in the game based upon the week of year.
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum SeasonTypes As Integer
            Winter = 0
            Spring = 1
            Summer = 2
            Fall = 3
        End Enum

        ''' <summary>
        ''' The WeatherTypes that are currently available in the game.
        ''' </summary>
        ''' <remarks></remarks>
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

#End Region

#Region "Fields and Constants"

        Private LastWorldUpdate As Date 'The date and time since the last world update.
        Private _currentWeather As WeatherTypes = WeatherTypes.Clear 'The current weather type. This gets overwritten by the property if that is used.

#End Region

#Region "Properties"

        Private ReadOnly Property WeekOfYear() As Integer
            Get
                Return CInt(((My.Computer.Clock.LocalTime.DayOfYear - (My.Computer.Clock.LocalTime.DayOfWeek - 1)) / 7) + 1)
            End Get
        End Property

        ''' <summary>
        ''' Returns the current season based on the week of year.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>If this program warps through a wormhole, Summer would get returned.</remarks>
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

        Private ReadOnly Property GetSeason() As SeasonTypes
            Get
                Dim seasonProperty As String = Basic.GetPropertyValue("Season", "")
                If seasonProperty <> "" Then
                    'Return the season in the property.

                    If IsNumeric(seasonProperty) = True Then
                        Return CType(CInt(seasonProperty), SeasonTypes)
                    End If
                End If

                'Return the season that is currently active depending on the computer's date.
                Return Me.CurrentSeason
            End Get
        End Property

        Public Property CurrentWeather() As WeatherTypes
            Get
                Dim weatherProperty As String = Basic.GetPropertyValue("Weather", "")
                If weatherProperty <> "" Then
                    'Return the weather in the property.

                    If IsNumeric(weatherProperty) = True Then
                        Return CType(CInt(weatherProperty), WeatherTypes)
                    End If
                End If

                'Return the randomly chosen weather that is currently active:
                Return Me._currentWeather
            End Get
            Set(value As WeatherTypes)
                Me._currentWeather = value
            End Set
        End Property

        Public ReadOnly Property GetTimeString() As String
            Get
                If CBool(Basic.GetPropertyValue("DoDayCycle", "1")) = True Then
                    With My.Computer.Clock.LocalTime
                        Return .Hour.ToString() & "," & .Minute.ToString() & "," & .Second.ToString()
                    End With
                Else
                    Return "12,0,0"
                End If
            End Get
        End Property

        ''' <summary>
        ''' Returns the Network Package containing the world information of this server.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GetWorldPackage() As Servers.Package
            Get
                'Data format: Season(0),Weather(1),TimeData(2)
                Dim p As New Servers.Package(Servers.Package.PackageTypes.WorldData, -1, Servers.Package.ProtocolTypes.TCP)

                p.DataItems.Add(CInt(Me.GetSeason).ToString())
                p.DataItems.Add(CInt(Me.CurrentWeather).ToString())
                p.DataItems.Add(GetTimeString())

                Return p
            End Get
        End Property

#End Region

#Region "Constructors"

#End Region

#Region "Methods"

        ''' <summary>
        ''' Initializes this instance of World and starts the async update thread.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Initialize()
            Me.UpdateWorld()

            Dim t As New Threading.Thread(AddressOf Me.InternalUpdate)
            t.IsBackground = True
            t.Start()
        End Sub

        ''' <summary>
        ''' Every 10 seconds, this threaded sub checks if the last update has been over an hour ago, and then updates the world.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InternalUpdate()
            While True
                If CInt(Math.Abs(DateDiff(DateInterval.Hour, LastWorldUpdate, Date.Now))) >= 1 Then 'Time difference between last update is larger or equal one hour.
                    Me.UpdateWorld()
                End If

                Threading.Thread.Sleep(10000)
            End While
        End Sub

        ''' <summary>
        ''' The world update sets new weather and distributes eventual changes amongst clients.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UpdateWorld()
            Me.LastWorldUpdate = Date.Now
            Me.SetWeather()
            Me.DistributeWorldData()

            Me.PrintWorldInformation()
        End Sub

        Public Sub PrintWorldInformation()
            Basic.ServersManager.WriteLine("[INFO] World data: Current season: " & Me.GetSeason.ToString() & ", Current weather: " & Me.CurrentWeather.ToString())
        End Sub

        Public Sub DistributeWorldData()
            Basic.ServersManager.ServerHost.SendToAllPlayers(Me.GetWorldPackage)
        End Sub

        ''' <summary>
        ''' This is setting a new random weather type depending on the season.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetWeather()
            Dim Random As New System.Random()
            Dim r As Integer = Random.Next(0, 100)

            Me._currentWeather = WeatherTypes.Clear

            Select Case Me.GetSeason()
                Case SeasonTypes.Winter
                    If r < 20 Then
                        Me._currentWeather = WeatherTypes.Rain
                    ElseIf r >= 20 And r < 50 Then
                        Me._currentWeather = WeatherTypes.Clear
                    Else
                        Me._currentWeather = WeatherTypes.Snow
                    End If
                Case SeasonTypes.Spring
                    If r < 5 Then
                        Me._currentWeather = WeatherTypes.Snow
                    ElseIf r >= 5 And r < 40 Then
                        Me._currentWeather = WeatherTypes.Rain
                    Else
                        Me._currentWeather = WeatherTypes.Clear
                    End If
                Case SeasonTypes.Summer
                    If r < 10 Then
                        Me._currentWeather = WeatherTypes.Rain
                    Else
                        Me._currentWeather = WeatherTypes.Clear
                    End If
                Case SeasonTypes.Fall
                    If r < 5 Then
                        Me._currentWeather = WeatherTypes.Snow
                    ElseIf r >= 5 And r < 80 Then
                        Me._currentWeather = WeatherTypes.Rain
                    Else
                        Me._currentWeather = WeatherTypes.Clear
                    End If
            End Select
        End Sub

#End Region

    End Class

End Namespace