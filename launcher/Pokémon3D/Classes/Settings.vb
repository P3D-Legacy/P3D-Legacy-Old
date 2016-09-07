Public Class Settings

    Public Shared Sub CreateSettingsFile()
        If System.IO.Directory.Exists(My.Application.Info.DirectoryPath & "\Pokemon\") = False Then
            System.IO.Directory.CreateDirectory(My.Application.Info.DirectoryPath & "\Pokemon\")
        End If

        If System.IO.File.Exists(My.Application.Info.DirectoryPath & "\Pokemon\launcher_settings.dat") = False Then
            Dim default_settings As String = "game_updates|1" & vbNewLine &
                "autostart|0" & vbNewLine &
                "autostart_time|10" & vbNewLine &
                "game_path|[Default]" & vbNewLine &
                "game_version|[Latest]"

            System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\Pokemon\launcher_settings.dat", default_settings)
        End If
    End Sub

    Public Shared Function GetSetting(ByVal Name As String) As String
        CreateSettingsFile()

        Dim setting_lines() As String = System.IO.File.ReadAllLines(My.Application.Info.DirectoryPath & "\Pokemon\launcher_settings.dat")

        For Each line As String In setting_lines
            Dim setting_name As String = line.GetSplit(0, "|")
            Dim setting_value As String = line.GetSplit(1, "|")

            If setting_name = Name Then
                Return setting_value
            End If
        Next

        Return ""
    End Function

    Public Shared Sub SaveSetting(ByVal Name As String, ByVal Value As String)
        CreateSettingsFile()

        Dim setting_lines() As String = System.IO.File.ReadAllLines(My.Application.Info.DirectoryPath & "\Pokemon\launcher_settings.dat")
        Dim save_lines As New List(Of String)

        Dim added As Boolean = False

        For Each line As String In setting_lines
            Dim setting_name As String = line.GetSplit(0, "|")
            Dim setting_value As String = line.GetSplit(1, "|")

            If setting_name = Name Then
                added = True
                setting_value = Value
            End If

            save_lines.Add(setting_name & "|" & setting_value)
        Next

        If added = False Then
            save_lines.Add(Name & "|" & Value)
        End If

        System.IO.File.WriteAllLines(My.Application.Info.DirectoryPath & "\Pokemon\launcher_settings.dat", save_lines.ToArray())
    End Sub

End Class