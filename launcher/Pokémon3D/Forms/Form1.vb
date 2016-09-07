Imports System.IO
Imports System.Runtime.InteropServices

Public Class Form1

    Dim opennewupdate As Boolean = False
    Dim openedCrashlog As Boolean = False

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If My.Application.CommandLineArgs.Count > 0 Then
            For Each s As String In My.Application.CommandLineArgs
                If s.StartsWith("CRASHLOG_") = True Then
                    CreateCrashlogPage(s.Remove(0, s.IndexOf("_") + 1))
                    openedCrashlog = True
                    Exit For
                End If
            Next
        End If

        If openedCrashlog = False Then
            web_display.Navigate(New Uri("")) ' CLASSIFIED
        End If

        Me.button_start.Enabled = GamefilesAvailable()
        Dim gameVersion As String = GetCurrentGameVersion()
        If gameVersion <> "<>" Then
            Me.button_start.Text = "Start game (" & gameVersion & ")"
        End If
    End Sub

    Private Sub CreateCrashlogPage(ByVal file As String)
        Dim html As String = "<html><body><code><h1>Looks like the game crashed!</h1>" &
                             "<a href=""http://pokemon3d.net/forum/forums/6/create-thread"">>Report this crash on the forum.</a><br />" & vbNewLine &
                             "<a href=""http://www.pokemon3d.net/launcher/launcher_page.php"">>Go back to the news page.</a><br />" & vbNewLine &
                             "<a href=""http://www.pokemon3d.net/"">>Visit the Pokémon3D homepage.</a>" & vbNewLine &
                             "<br /><br />"
        Dim fileContent As String = System.IO.File.ReadAllText(file).Replace("""http://pokemon3d.net/forum/forums/6/create-thread""", "<a href=""http://pokemon3d.net/forum/forums/6/create-thread"">""http://pokemon3d.net/forum/forums/6/create-thread""</a>")

        fileContent = fileContent.Replace("[CODE]", "").Replace("[/CODE]", "")

        html &= fileContent.Replace(vbNewLine, "<br />" & vbNewLine)
        html &= "</code></body></html>"

        web_display.Navigate("about:blank")
        If web_display.Document IsNot Nothing Then
            web_display.Document.Write(String.Empty)
        End If
        web_display.DocumentText = html

        btn_crashlog_open.Tag = file
        btn_crashlog_open.Visible = True

        Dim t As New Threading.Thread(AddressOf CrashlogHandler.SendCrashlog)
        t.IsBackground = True
        t.Start(fileContent)
    End Sub

    Private Sub Form1_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        Try
            NewVersion.ClearTemp()
        Catch : End Try

        If openedCrashlog = False Then
            If CBool(Settings.GetSetting("autostart")) = True And Me.button_start.Enabled = True Then
                autostart_group.Visible = True

                tickautostart.Interval = 1
                tickautostart.Start()
                autostartDelay = CInt(Settings.GetSetting("autostart_time")) * 100

                autostart_progress.Maximum = autostartDelay
            End If

            If Me.button_start.Enabled = True And CBool(Settings.GetSetting("game_updates")) = True Then
                Dim checkThread As New Threading.Thread(AddressOf CheckGameVersion)
                checkThread.Start()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Gets the current game version.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCurrentGameVersion() As String
        Dim oldVersion As String = "<>"
        If System.IO.File.Exists(My.Application.Info.DirectoryPath & "\Pokemon\ID.dat") = True Then
            oldVersion = System.IO.File.ReadAllText(My.Application.Info.DirectoryPath & "\Pokemon\ID.dat")
        End If

        Dim play_version As String = Settings.GetSetting("game_version")
        If play_version <> "" And play_version <> "[Latest]" Then
            oldVersion = play_version.Remove(0, 8)
            download_button.Text = "Download version " & oldVersion
        Else
            download_button.Text = "Download latest version"
        End If

        Return oldVersion
    End Function

    ''' <summary>
    ''' Checks for a new game version and sets the flag opennewupdate to true.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CheckGameVersion()
        Try
            Dim c As New Net.WebClient
            Dim gameData() As String = c.DownloadString("").Replace(vbNewLine, "§").Split(CChar("§")) ' CLASSIFIED

            Dim gameVersion As String = gameData(0)

            Dim oldVersion As String = GetCurrentGameVersion()

            Dim play_version As String = Settings.GetSetting("game_version")

            If play_version = "" Or play_version = "[Latest]" Then
                If oldVersion <> "<>" Then
                    If oldVersion <> gameVersion Then
                        stopAutostart = True
                        If MsgBox("There is a new version (" & gameVersion & ") available!" & vbNewLine & "Do you want to download it?", MsgBoxStyle.YesNo + MsgBoxStyle.Information, "New update!") = MsgBoxResult.Yes Then
                            opennewupdate = True
                        End If
                    End If
                End If
            End If
        Catch ex As Exception : End Try
    End Sub

    ''' <summary>
    ''' Checks if the game files are available.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GamefilesAvailable() As Boolean
        Dim filesExist As Boolean = True
        Dim iniPath As String = Settings.GetSetting("game_path")
        If iniPath = "" Or iniPath = "[Default]" Then
            iniPath = My.Application.Info.DirectoryPath
        End If

        Dim playVersion As String = Settings.GetSetting("game_version")
        If playVersion = "" Or playVersion = "[Latest]" Then
            playVersion = ""
        Else
            playVersion = "\" & playVersion
        End If

        If Directory.Exists(iniPath & playVersion & "\Pokemon\") = False Then
            filesExist = False
            Debug.Print(filesExist)
        Else
            Dim dirList() As String = {"maps", "Content", "Scripts"}
            For Each Dir As String In dirList
                If Directory.Exists(iniPath & playVersion & "\Pokemon\" & Dir) = False Then
                    filesExist = False
                    Debug.Print(Dir)
                End If
            Next
            If filesExist = True Then
                If File.Exists(iniPath & playVersion & "\Pokemon\Pokemon.exe") = False Then
                    filesExist = False
                End If
            End If
        End If

        Return filesExist
    End Function

    ''' <summary>
    ''' Handles the download button.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub download_button_Click(sender As System.Object, e As System.EventArgs) Handles download_button.Click
        DisableAutostart()
        ShowDownloadWindow()
    End Sub

    ''' <summary>
    ''' Displays the download window.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ShowDownloadWindow()
        NewVersion.info_label.Text = "Gameversion:" & vbNewLine & "Release date:"
        NewVersion.button_download.Enabled = False
        NewVersion.isReady = False
        NewVersion.ShowDialog()
    End Sub

    ''' <summary>
    ''' Starts the game.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub button_start_Click(sender As System.Object, e As System.EventArgs) Handles button_start.Click
        If GamefilesAvailable() = True Then
            DisableAutostart()

            If dotNet40installed() = True Then
                If XNAinstalled() = True Then
                    Dim d As Date = CurrentDate()
                    Dim dInteger As Integer = CInt(d.Year + d.Month + d.Day + d.Hour + d.Minute + d.Second)
                    Dim r As New System.Random(dInteger)

                    Dim startVar As Integer = r.Next(0, 100000)

                    Dim arg As String = d.Year & "," & d.Month & "," & d.Day & "," & d.Hour & "," & d.Minute & "," & d.Second & "|" & startVar & "|""" & Application.ExecutablePath & """"

                    Dim play_version As String = Settings.GetSetting("game_version")
                    If play_version <> "" And play_version <> "[Latest]" Then
                        play_version = "\" & play_version
                    Else
                        play_version = ""
                    End If

                    Dim play_path As String = Settings.GetSetting("game_path")
                    If play_path = "" Or play_path = "[Default]" Then
                        play_path = My.Application.Info.DirectoryPath
                    End If

                    Process.Start(play_path & play_version & "\Pokemon\Pokemon.exe", arg)
                    Me.Close()
                Else
                    If MsgBox("Pokémon3D requires Microsoft XNA Framework 4.0." & vbCrLf & "Please visit" & vbCrLf & "http://www.microsoft.com/en-us/download/details.aspx?id=20914" & vbCrLf & "to download." & vbCrLf & vbCrLf & "Would you like to open the webpage now?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "XNA 4.0") = MsgBoxResult.Yes Then
                        Process.Start("http://www.microsoft.com/en-us/download/details.aspx?id=20914")
                    End If
                End If
            Else
                If MsgBox("Pokémon3D requires Microsoft .Net Framework 4.0." & vbCrLf & "Please visit" & vbCrLf & "http://www.microsoft.com/en-us/download/details.aspx?id=17851" & vbCrLf & "to download." & vbCrLf & vbCrLf & "Would you like to open the webpage now?", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, ".Net 4.0") = MsgBoxResult.Yes Then
                    Process.Start("http://www.microsoft.com/en-us/download/details.aspx?id=17851")
                End If
            End If
        Else
            button_start.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' Gets the current date.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CurrentDate() As Date
        With My.Computer.Clock.LocalTime
            Return New Date(.Year, .Month, .Day, .TimeOfDay.Hours, .TimeOfDay.Minutes, .TimeOfDay.Seconds)
        End With
    End Function

    ''' <summary>
    ''' Ticks to open the udpate window when a new update is found.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ticknewupdate_Tick(sender As System.Object, e As System.EventArgs) Handles ticknewupdate.Tick
        If opennewupdate = True Then
            opennewupdate = False
            ShowDownloadWindow()
        End If
    End Sub

    ''' <summary>
    ''' Checks if XNA is installed.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function XNAinstalled() As Boolean
        Dim readvalue As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\XNA\Framework\v4.0", "Installed", Nothing)
        If readvalue = Nothing Then
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Checks if .Net 4.0 is installed.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function dotNet40installed() As Boolean
        Dim readvalue As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full", "Install", Nothing)
        If readvalue = Nothing Then
            readvalue = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client", "Install", Nothing)
        End If
        If readvalue = Nothing Then
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    ''' Handles the hyperlink clicks
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub web_display_NewWindow(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles web_display.NewWindow
        Dim myElement As HtmlElement = web_display.Document.ActiveElement
        Dim target As String = myElement.GetAttribute("href")

        If target <> "http://pokemon3d.net/launcher/launcher_page.php" Then
            Process.Start(target)
            e.Cancel = True
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles link_settings.LinkClicked
        DisableAutostart()
        SettingsWindow.ShowDialog(Me)
    End Sub

#Region "Autostart"

    Private autostartDelay As Integer = 1000
    Private stopAutostart As Boolean = False

    Private Sub tickautostart_Tick(sender As Object, e As System.EventArgs) Handles tickautostart.Tick
        If stopAutostart = True Then
            stopAutostart = False
            DisableAutostart()
        Else
            autostartDelay -= 1

            If autostart_progress.Value < autostart_progress.Maximum Then
                autostart_progress.Value += 1
            End If

            If autostartDelay < 0 Then
                If button_start.Enabled = True Then
                    tickautostart.Stop()
                    autostartDelay = 0

                    button_start.PerformClick()
                End If
            End If
        End If
    End Sub

    Private Sub DisableAutostart()
        tickautostart.Stop()

        autostart_group.Visible = False
    End Sub

    Private Sub button_stopautostart_Click(sender As System.Object, e As System.EventArgs) Handles button_stopautostart.Click
        DisableAutostart()
    End Sub

#End Region

    Private Sub btn_crashlog_open_Click(sender As Object, e As EventArgs) Handles btn_crashlog_open.Click
        Process.Start("explorer.exe", "/select,""" & btn_crashlog_open.Tag.ToString() & """")
    End Sub

End Class