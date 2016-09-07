Public Class Mainform

#Region "Fields"

    Public ServersManager As Servers.ServersManager

#End Region

#Region "Properties"

    Public ReadOnly Property DecSeparator() As String
        Get
            Return My.Application.Culture.NumberFormat.NumberDecimalSeparator
        End Get
    End Property

#End Region

#Region "Methods"

    Private Sub Mainform_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Me.ServersManager.ServerHost.StartedHosting = True Then
            If Me.ServersManager.PlayerCollection.Count > 0 Then
                Me.ServersManager.ServerHost.SendToAllPlayers(New Servers.Package(Servers.Package.PackageTypes.ServerClose, -1, Servers.Package.ProtocolTypes.TCP, ""))
            End If
        End If

        Me.ServersManager.ListManager.Save()
        Me.ServersManager.PropertyCollection.SaveToFile()

        Basic.ServersManager.WriteLine(String.Format(Servers.ServerMessages.SERVER_CLOSE, e.CloseReason.ToString()))
        Debug.Print("---Close Server---")
    End Sub

    Private Sub Mainform_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Debug.Print("---Start Server---")

        Basic.SetMainform(Me)

        Me.ServersManager = New Servers.ServersManager()
        Me.ServersManager.WriteLine(String.Format(Servers.ServerMessages.SERVER_START, Servers.ServersManager.PROTOCOLVERSION))

        Me.ServersManager.Host()

        Me.UpdatePlayerList()
    End Sub

    Public Sub WriteGUILine(ByVal Line As String)
        If text_output.Text <> "" Then
            text_output.Text &= vbNewLine
        End If

        text_output.Text &= Line

        'Shorten the content of the textbox to at most 1000 lines:
        If text_output.Lines.Length > 1000 Then
            Dim newLines As List(Of String) = text_output.Lines.ToList()
            While newLines.Count > 1000
                newLines.RemoveAt(0)
            End While

            text_output.Lines = newLines.ToArray()
        End If

        'Set selection start to the beginning of the last line:
        Dim selectionStart As Integer = text_output.Text.Length - text_output.Lines.Last().Length

        text_output.SelectionStart = selectionStart
        text_output.ScrollToCaret()
    End Sub

    ''' <summary>
    ''' Clears the output textbox.
    ''' </summary>
    Public Sub ClearOutput()
        text_output.Text = ""
    End Sub

    Public Sub UpdatePlayerList()
        Dim selectedName As String = ""
        If Me.list_players.SelectedIndex > -1 Then
            selectedName = Me.list_players.SelectedItem.ToString()
        End If

        Me.list_players.Items.Clear()

        For i = 0 To Me.ServersManager.PlayerCollection.Count - 1
            If i <= Me.ServersManager.PlayerCollection.Count - 1 Then
                With Me.ServersManager.PlayerCollection(i)
                    Me.list_players.Items.Add(String.Format(Servers.ServerMessages.GUI_PLAYERLIST_ENTRY, .Name, .ServersID.ToString(), .GetBusyTypeName()))
                End With
            End If
        Next

        If selectedName <> "" Then
            If Me.list_players.Items.Contains(selectedName) = True Then
                Me.list_players.SelectedIndex = Me.list_players.Items.IndexOf(selectedName)
            End If
        End If

        Me.box_players.Text = String.Format(Servers.ServerMessages.GUI_PLAYERLIST_TITLE, Me.list_players.Items.Count.ToString(), Me.ServersManager.PropertyCollection.GetPropertyValue("MaxPlayers", "10"))
    End Sub

    Private Sub text_input_KeyPress(sender As Object, e As KeyPressEventArgs) Handles text_input.KeyPress
        If text_input.Text.Length > 0 And text_input.Text <> "" Then
            If e.KeyChar = ChrW(Keys.Enter) Then
                Servers.ServerCommands.Execute(text_input.Text, -1, False)

                text_input.Text = ""
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub logMenu_Copy_Click(sender As Object, e As EventArgs) Handles logMenu_Copy.Click
        text_output.Copy()
    End Sub

    Private Sub logMenu_copyAll_Click(sender As Object, e As EventArgs) Handles logMenu_copyAll.Click
        Dim start As Integer = text_output.SelectionStart
        Dim length As Integer = text_output.SelectionLength

        text_output.SelectAll()
        text_output.Copy()

        text_output.SelectionStart = start
        text_output.SelectionLength = length
    End Sub

    Private Sub logMenu_GoToBottom_Click(sender As Object, e As EventArgs) Handles logMenu_GoToBottom.Click
        text_output.SelectionStart = text_output.Text.Length
        text_output.ScrollToCaret()
    End Sub

    Private Sub logMenu_SelectAll_Click(sender As Object, e As EventArgs) Handles logMenu_SelectAll.Click
        text_output.SelectAll()
    End Sub

#End Region

#Region "ContextMenu"

    Private Structure PlayerNameInformation
        Public Sub New(ByVal DisplayName As String, ByVal ListName As String)
            Me.DisplayName = DisplayName
            Me.ListName = ListName
        End Sub

        Public DisplayName As String
        Public ListName As String
    End Structure

    Private Sub playerMenu_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles playerMenu.Opening
        If list_players.SelectedItems.Count = 0 Then
            e.Cancel = True
        Else
            Dim playerName As String = list_players.SelectedItem.ToString()
            playerName = playerName.Remove(playerName.LastIndexOf("(") - 1)

            Dim nameInformation As New PlayerNameInformation(playerName, Basic.ServersManager.PlayerCollection.GetPlayer(playerName).ListName)
            playerMenu.Items(0).Text = nameInformation.DisplayName

            If Basic.ServersManager.ListManager.Mutelist.ContainsPlayer(nameInformation.ListName) = True Then
                playerMenu.Items(4).Text = Servers.ServerMessages.GUI_PLAYERMENU_UNMUTE
            Else
                playerMenu.Items(4).Text = Servers.ServerMessages.GUI_PLAYERMENU_MUTE
            End If
            If Basic.ServersManager.ListManager.WhiteList.ContainsPlayer(nameInformation.ListName) = True Then
                playerMenu.Items(7).Text = Servers.ServerMessages.GUI_PLAYERMENU_REMOVE_WHITELIST
            Else
                playerMenu.Items(7).Text = Servers.ServerMessages.GUI_PLAYERMENU_ADD_WHITELIST
            End If
            If Basic.ServersManager.ListManager.BlackList.ContainsPlayer(nameInformation.ListName) = True Then
                playerMenu.Items(8).Text = Servers.ServerMessages.GUI_PLAYERMENU_REMOVE_BLACKLIST
            Else
                playerMenu.Items(8).Text = Servers.ServerMessages.GUI_PLAYERMENU_ADD_BLACKLIST
            End If
            If Basic.ServersManager.ListManager.Operators.ContainsPlayer(nameInformation.ListName) = True Then
                playerMenu.Items(10).Text = Servers.ServerMessages.GUI_PLAYERMENU_REMOVE_OPERATOR
            Else
                playerMenu.Items(10).Text = Servers.ServerMessages.GUI_PLAYERMENU_ADD_OPERATOR
            End If

            playerMenu.Tag = nameInformation
        End If
    End Sub

    Private Sub playerMenu_PM_Click(sender As Object, e As EventArgs) Handles playerMenu_PM.Click
        SetInputText("/pm {0} ")
    End Sub

    Private Sub playerMenu_Kick_Click(sender As Object, e As EventArgs) Handles playerMenu_Kick.Click
        SetInputText("/kick {0}")
    End Sub

    Private Sub playerMenu_whitelist_Click(sender As Object, e As EventArgs) Handles playerMenu_whitelist.Click
        If Basic.ServersManager.ListManager.WhiteList.ContainsPlayer(CType(playerMenu.Tag, PlayerNameInformation).ListName) = True Then
            SetInputText("/list remove whitelist {0}")
        Else
            SetInputText("/list add whitelist {0}")
        End If
    End Sub

    Private Sub playerMenu_blacklist_Click(sender As Object, e As EventArgs) Handles playerMenu_blacklist.Click
        If Basic.ServersManager.ListManager.BlackList.ContainsPlayer(CType(playerMenu.Tag, PlayerNameInformation).ListName) = True Then
            SetInputText("/list remove blacklist {0}")
        Else
            SetInputText("/list add blacklist {0}")
        End If
    End Sub

    Private Sub playerMenu_operator_Click(sender As Object, e As EventArgs) Handles playerMenu_operator.Click
        If Basic.ServersManager.ListManager.Operators.ContainsPlayer(CType(playerMenu.Tag, PlayerNameInformation).ListName) = True Then
            SetInputText("/list remove operators {0}")
        Else
            SetInputText("/list add operators {0}")
        End If
    End Sub

    Private Sub playerMenu_Mute_Click(sender As Object, e As EventArgs) Handles playerMenu_Mute.Click
        If Basic.ServersManager.ListManager.Mutelist.ContainsPlayer(CType(playerMenu.Tag, PlayerNameInformation).ListName) = True Then
            SetInputText("/list remove mutelist {0}")
        Else
            SetInputText("/list add mutelist {0}")
        End If
    End Sub

    Private Sub SetInputText(ByVal text As String)
        Dim nameInformation = CType(playerMenu.Tag, PlayerNameInformation)
        text_input.Text = String.Format(text, nameInformation.DisplayName)

        text_input.Focus()
        text_input.SelectionLength = 0
        text_input.SelectionStart = text_input.Text.Length
    End Sub

#End Region

End Class