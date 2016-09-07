Public Class NewVersion

    Dim gameVersion As String = ""
    Dim gameRelease As String = ""
    Dim gameDownloadPath As String = ""
    Dim gameSize As String = ""

    WithEvents c As New Net.WebClient
    Dim w As New WebBrowser

    Public isReady As Boolean = False

    Dim currentFile As String = ""
    Dim maxFiles As Integer = 100
    Dim filesDone As Integer = 0
    Dim download_percentage As Double = 0D

    Dim openZip As Boolean = True
    Dim zipThread As New Threading.Thread(AddressOf Unzip)

    Private Sub button_cancel_Click(sender As System.Object, e As System.EventArgs) Handles button_cancel.Click
        Form1.button_start.Enabled = Form1.GamefilesAvailable()

        openZip = False

        currentFile = ""
        maxFiles = 100
        filesDone = 0
        download_percentage = 0D

        If zipThread.IsAlive = True Then
            zipThread.Abort()
        End If
        If c.IsBusy = True Then
            c.CancelAsync()
        End If

        Me.statuslabel.Text = ""

        Me.download_progress.Value = 0

        Me.Close()
    End Sub

    Private Sub SendRequest()
        'System.Net.FtpWebRequest.Create(New System.Uri("" & gameVersion)).GetResponse()
    End Sub

    Private Sub button_download_Click(sender As System.Object, e As System.EventArgs) Handles button_download.Click
        Me.button_download.Enabled = False

        If System.IO.Directory.Exists(My.Application.Info.DirectoryPath & "\Temp\") = False Then
            Me.statuslabel.Text = "Create folder ""Temp""..."
            System.IO.Directory.CreateDirectory(My.Application.Info.DirectoryPath & "\Temp\")
        End If

        download_progress.Style = ProgressBarStyle.Continuous

        SendRequest()
        c.DownloadFileAsync(New Uri(Me.gameDownloadPath), My.Application.Info.DirectoryPath & "\Temp\temp.zip")
        Me.statuslabel.Text = "Downloading... (0%)" & vbNewLine & "Received 0,00 MB"
        Me.Timer1.Start()
    End Sub

    Private Sub DeleteOldFiles()
        Dim iniDir As String = My.Application.Info.DirectoryPath & "\Pokemon\"

        Dim Dirs() As String = {"Content", "maps", "Scripts"}
        Dim Files() As String = {"AppSharpGameLibrary.dll", "Pokemon.exe"}

        For Each Dir As String In Dirs
            If System.IO.Directory.Exists(iniDir & Dir) = True Then
                System.IO.Directory.Delete(iniDir & Dir, True)
            End If
        Next
        For Each file As String In Files
            If System.IO.File.Exists(iniDir & file) = True Then
                System.IO.File.Delete(iniDir & file)
            End If
        Next
    End Sub

    Private Sub NewVersion_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        currentFile = ""
        maxFiles = 100
        filesDone = 0
        download_percentage = 0D

        Me.statuslabel.Text = ""

        Me.download_progress.Value = 0
    End Sub

    Private Sub NewVersion_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim playVersion As String = Settings.GetSetting("game_version")
        If playVersion = "" Or playVersion = "[Latest]" Then
            DownloadGameInfos()
        Else
            SetupGameInfos(playVersion.Remove(0, 8))
        End If
    End Sub

    Public Sub SetupGameInfos(ByVal GameVersion As String)
        Dim data() As String = c.DownloadString("").Replace(vbNewLine, "§").Split(CChar("§")) ' CLASSIFIED

        Dim gameData() As String = {"", "", "", ""}
        For Each line As String In data
            If line.StartsWith(GameVersion & "|") = True Then
                gameData = line.Split("|")
            End If
        Next

        Me.gameVersion = gameData(0)
        Me.gameRelease = gameData(1)
        Me.gameDownloadPath = gameData(2)
        Me.gameSize = gameData(3)

        If Me.gameDownloadPath <> "" Then
            button_download.Enabled = True
        End If

        openZip = True
        Me.statuslabel.Text = ""
        Me.download_progress.Value = 0
        Me.info_label.Text = "Gameversion: " & Me.gameVersion & vbNewLine & "Release date: " & gameRelease
    End Sub

    Private Sub DownloadGameInfos()
        gameVersion = ""
        gameRelease = ""
        gameDownloadPath = ""
        gameSize = ""

        Try
            Dim gameData() As String = c.DownloadString("").Replace(vbNewLine, "§").Split(CChar("§")) ' CLASSIFIED

            gameVersion = gameData(0)
            gameRelease = gameData(1)
            gameDownloadPath = gameData(2)
            gameSize = gameData(3)
        Catch ex As Exception : End Try

        If Me.gameDownloadPath <> "" Then
            button_download.Enabled = True
        End If

        openZip = True
        Me.statuslabel.Text = ""
        Me.download_progress.Value = 0
        Me.info_label.Text = "Gameversion: " & Me.gameVersion & vbNewLine & "Release date: " & gameRelease
    End Sub

    Private Sub c_DownloadFileCompleted(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs) Handles c.DownloadFileCompleted
        Timer1.Stop()

        If openZip = True Then
            zipThread = New Threading.Thread(AddressOf Unzip)

            download_progress.Style = ProgressBarStyle.Continuous
            download_progress.Value = download_progress.Maximum
            Me.statuslabel.Text = "Preparing..."
            zipThread.Start()
        End If
    End Sub

    Private Sub Unzip()
        If System.IO.Directory.Exists(My.Application.Info.DirectoryPath & "\Temp\") = True Then
            If System.IO.File.Exists(My.Application.Info.DirectoryPath & "\Temp\temp.zip") = True Then
                Using Zip As Zimpler.ZipFile = Zimpler.ZipFile.FromFile(My.Application.Info.DirectoryPath & "\Temp\temp.zip")
                    maxFiles = Zip.ItemInfos.Count()
                    filesDone = 0

                    Dim play_version As String = Settings.GetSetting("game_version")
                    If play_version = "" Or play_version = "[Latest]" Then
                        play_version = ""
                    Else
                        play_version = play_version & "\"
                    End If

                    For Each i As Zimpler.ZipItemInfo In Zip.ItemInfos
                        currentFile = i.Name

                        Dim fileInfo As IO.FileInfo = New IO.FileInfo(play_version & i.Name)
                        If System.IO.File.Exists(play_version & i.Name) = True AndAlso GetFilePermissions(play_version & i.Name) <> Security.AccessControl.FileSystemRights.Modify Then
                            Dim fileAcl As New System.Security.AccessControl.FileSecurity
                            fileAcl.AddAccessRule(New System.Security.AccessControl.FileSystemAccessRule(System.Security.Principal.WindowsIdentity.GetCurrent().Name, Security.AccessControl.FileSystemRights.Modify, Security.AccessControl.AccessControlType.Allow, Security.AccessControl.PropagationFlags.None, Security.AccessControl.AccessControlType.Allow))
                            fileInfo.SetAccessControl(fileAcl)
                        End If

                        Zip.Export(i.Index, play_version & i.Name)

                        filesDone += 1
                    Next
                    currentFile = ""
                    filesDone = maxFiles

                    MsgBox("Update to version " & gameVersion & " successful.", MsgBoxStyle.Information, "Update")

                    If play_version = "" Then
                        Dim saveID As String = gameVersion
                        System.IO.File.WriteAllText(My.Application.Info.DirectoryPath & "\Pokemon\ID.dat", saveID)
                    End If
                End Using
            Else
                MsgBox("Update NOT successful!" & vbNewLine & "Could not find archive file to unzip.", MsgBoxStyle.Exclamation, "Update")
            End If
        Else
            MsgBox("Update NOT successful!" & vbNewLine & "Temp directory missing.", MsgBoxStyle.Exclamation, "Update")
        End If

        ClearTemp()

        Me.isReady = True
        currentFile = ""
    End Sub

    Private Function GetFilePermissions(ByVal filePath As String) As System.Security.AccessControl.FileSystemRights
        Dim identityReference As String = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToLower()
        Dim fileSecurity As System.Security.AccessControl.FileSecurity = IO.File.GetAccessControl(filePath, Security.AccessControl.AccessControlSections.Access)

        For Each fsRule As System.Security.AccessControl.FileSystemAccessRule In fileSecurity.GetAccessRules(True, True, GetType(System.Security.Principal.NTAccount))
            If fsRule.IdentityReference.Value.ToLower() = identityReference Then
                Return fsRule.FileSystemRights
            End If
        Next

        Return Nothing
    End Function

    Public Shared Sub ClearTemp()
        If System.IO.Directory.Exists(My.Application.Info.DirectoryPath & "\Temp\") = True Then
            System.IO.Directory.Delete(My.Application.Info.DirectoryPath & "\Temp\", True)
        End If
    End Sub

    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        If System.IO.Directory.Exists(My.Application.Info.DirectoryPath & "\Temp\") = True Then
            If System.IO.File.Exists(My.Application.Info.DirectoryPath & "\Temp\temp.zip") = True Then
                Dim info As New System.IO.FileInfo(My.Application.Info.DirectoryPath & "\Temp\temp.zip")

                Dim fileSize As Integer = info.Length

                statuslabel.Text = "Downloading... (" & download_percentage & "%)" & vbNewLine & "Received " & Math.Round(fileSize / 1048576, 2).ToString() & " MB / " & gameSize
            End If
        End If
    End Sub

    Private Sub readytimer_Tick(sender As System.Object, e As System.EventArgs) Handles readytimer.Tick
        If isReady = True Then
            Form1.button_start.Enabled = Form1.GamefilesAvailable()
            Dim gameVersion As String = Form1.GetCurrentGameVersion()
            If gameVersion <> "<>" Then
                Form1.button_start.Text = "Start game (" & gameVersion & ")"
            End If
            Me.Close()
        End If
    End Sub

    Private Sub c_DownloadProgressChanged(sender As Object, e As System.Net.DownloadProgressChangedEventArgs) Handles c.DownloadProgressChanged
        download_progress.Maximum = e.TotalBytesToReceive
        download_progress.Value = e.BytesReceived

        download_percentage = Math.Round(e.BytesReceived / e.TotalBytesToReceive * 100, 0)

        gameSize = Math.Round(e.TotalBytesToReceive / 1048576, 2) & " MB"

        InvokePaint(download_progress, Nothing)
    End Sub

    Private Sub zipFileTimer_Tick(sender As System.Object, e As System.EventArgs) Handles zipFileTimer.Tick
        If currentFile <> "" Then
            Dim percent As Double = Math.Round(filesDone / maxFiles * 100, 0)

            statuslabel.Text = "Extracting... (" & percent.ToString() & "%)" & vbNewLine & "Current file: """ & System.IO.Path.GetFileName(currentFile) & """"

            download_progress.Maximum = maxFiles
            download_progress.Value = filesDone
        End If
    End Sub

    Private Sub NewVersion_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        Try
            ClearTemp()
        Catch : End Try
    End Sub

End Class