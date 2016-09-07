Public Class SettingsWindow

    Structure GameVersion
        Public Version As String
        Public DownloadPath As String
        Public Size As String
        Public ReleaseDate As String
    End Structure

    Public GameVersions As New List(Of GameVersion)

    Private Sub button_cancel_Click(sender As System.Object, e As System.EventArgs) Handles button_cancel.Click
        Me.Close()
    End Sub

    Private Sub button_ok_Click(sender As System.Object, e As System.EventArgs) Handles button_ok.Click
        SaveSettings()
        Me.Close()
    End Sub

    Private Sub check_autostart_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles check_autostart.CheckedChanged
        Label1.Visible = check_autostart.Checked
        Label2.Visible = check_autostart.Checked
        numeric_seconds.Visible = check_autostart.Checked
    End Sub

    Private Sub SettingsWindow_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Form1.button_start.Enabled = Form1.GamefilesAvailable()
        Dim gameVersion As String = Form1.GetCurrentGameVersion()
        If gameVersion <> "<>" Then
            Form1.button_start.Text = "Start game (" & gameVersion & ")"
        End If
    End Sub

    Private Sub SettingsWindow_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        LoadSettings()
    End Sub

    Private Sub LoadSettings()
        Me.check_updates.Checked = CBool(Settings.GetSetting("game_updates"))
        Me.check_autostart.Checked = CBool(Settings.GetSetting("autostart"))
        Me.numeric_seconds.Value = CInt(Settings.GetSetting("autostart_time"))

        Label1.Visible = check_autostart.Checked
        Label2.Visible = check_autostart.Checked
        numeric_seconds.Visible = check_autostart.Checked

        Dim game_path As String = Settings.GetSetting("game_path")
        If game_path = "[Default]" Or game_path = "" Then
            path_box.Text = "[Default]"
        Else
            path_box.Text = game_path
        End If

        version_box.Items.Clear()
        version_box.Items.Add("[Latest]")

        GetVersions()

        Dim game_version As String = Settings.GetSetting("game_version")
        If game_version = "[Latest]" Or game_version = "" Then
            version_box.SelectedIndex = 0
        Else
            version_box.SelectedIndex = version_box.Items.IndexOf(game_version)
        End If
    End Sub

    Private Sub SaveSettings()
        Settings.SaveSetting("game_updates", CStr(check_updates.Checked.ToInteger()))
        Settings.SaveSetting("autostart", CStr(check_autostart.Checked.ToInteger()))
        Settings.SaveSetting("autostart_time", numeric_seconds.Value.ToString())
        Settings.SaveSetting("game_path", path_box.Text)
        Settings.SaveSetting("game_version", version_box.Text)
    End Sub

    Private Sub GetVersions()
        Dim w As New Net.WebClient
        Dim s() As String = w.DownloadString("").Replace(vbNewLine, "§").Split(CChar("§")) ' CLASSIFIED
        For Each l As String In s
            If l.Contains("|") = True Then
                Dim c() As String = l.Split("|")
                GameVersions.Add(New GameVersion() With {.Version = c(0), .ReleaseDate = c(1), .DownloadPath = c(2), .Size = c(3)})
                version_box.Items.Add("Version " & c(0))
            End If
        Next
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://www.pokemon3d.net/")
    End Sub

End Class