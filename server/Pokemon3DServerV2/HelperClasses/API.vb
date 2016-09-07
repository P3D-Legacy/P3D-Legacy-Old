Public Class API

    Private Const ExportPath As String = "C:\inetpub\wwwroot\"

    ''' <summary>
    ''' Writes the game information to output files.
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared Sub WriteAPIOutput()
        If ApplicationArguments.APIOutput = True Then
            Dim targetPath As String = ExportPath
            If System.IO.Directory.Exists(ExportPath) = False Then
                targetPath = My.Application.Info.DirectoryPath
            End If

            If System.IO.Directory.Exists(targetPath & "\API") = False Then
                System.IO.Directory.CreateDirectory(targetPath & "\API")
            End If

            Dim data As String = Basic.ServersManager.PlayerCollection.Count & vbNewLine &
                  Basic.ServersManager.PropertyCollection.GetPropertyValue("MaxPlayers", "10") & vbNewLine &
                  Basic.ServersManager.World.CurrentSeason.ToString() & vbNewLine &
                  Basic.ServersManager.World.CurrentWeather.ToString() & vbNewLine &
                  Basic.ServersManager.World.GetTimeString()

            System.IO.File.WriteAllText(targetPath & "\API\p3dapi.txt", data)

            Dim sortedList As List(Of Servers.Player) = (From p As Servers.Player In Basic.ServersManager.PlayerCollection Order By p.Name Ascending).ToList()
            Dim playerData As String = ""
            For Each p As Servers.Player In sortedList
                If playerData <> "" Then
                    playerData &= vbNewLine
                End If
                playerData &= p.Name
            Next

            System.IO.File.WriteAllText(targetPath & "\API\p3dapi_players.txt", playerData)
        End If
    End Sub

    ''' <summary>
    ''' Starts the API output cycle. The API writes new information every second.
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub Start()
        Dim t As New Timer()
        t.Interval = 1000
        AddHandler t.Tick, AddressOf WriteAPIOutput
        t.Start()
    End Sub

End Class