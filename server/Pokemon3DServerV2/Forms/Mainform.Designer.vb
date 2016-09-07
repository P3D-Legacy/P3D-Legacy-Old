<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Mainform
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Mainform))
        Me.box_output = New System.Windows.Forms.GroupBox()
        Me.text_output = New System.Windows.Forms.TextBox()
        Me.logMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.logMenu_Copy = New System.Windows.Forms.ToolStripMenuItem()
        Me.logMenu_copyAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.logMenu_SelectAll = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.logMenu_GoToBottom = New System.Windows.Forms.ToolStripMenuItem()
        Me.text_input = New System.Windows.Forms.TextBox()
        Me.box_players = New System.Windows.Forms.GroupBox()
        Me.list_players = New System.Windows.Forms.ListBox()
        Me.playerMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.playerMenu_nameItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.playerMenu_PM = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.playerMenu_Mute = New System.Windows.Forms.ToolStripMenuItem()
        Me.playerMenu_Kick = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.playerMenu_whitelist = New System.Windows.Forms.ToolStripMenuItem()
        Me.playerMenu_blacklist = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.playerMenu_operator = New System.Windows.Forms.ToolStripMenuItem()
        Me.box_output.SuspendLayout()
        Me.logMenu.SuspendLayout()
        Me.box_players.SuspendLayout()
        Me.playerMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'box_output
        '
        Me.box_output.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.box_output.Controls.Add(Me.text_output)
        Me.box_output.Controls.Add(Me.text_input)
        Me.box_output.Location = New System.Drawing.Point(249, 3)
        Me.box_output.Name = "box_output"
        Me.box_output.Size = New System.Drawing.Size(485, 407)
        Me.box_output.TabIndex = 6
        Me.box_output.TabStop = False
        Me.box_output.Text = "Log and chat"
        '
        'text_output
        '
        Me.text_output.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.text_output.BackColor = System.Drawing.Color.White
        Me.text_output.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.text_output.ContextMenuStrip = Me.logMenu
        Me.text_output.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.text_output.Location = New System.Drawing.Point(6, 19)
        Me.text_output.Multiline = True
        Me.text_output.Name = "text_output"
        Me.text_output.ReadOnly = True
        Me.text_output.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.text_output.Size = New System.Drawing.Size(473, 350)
        Me.text_output.TabIndex = 2
        Me.text_output.WordWrap = False
        '
        'logMenu
        '
        Me.logMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.logMenu_Copy, Me.logMenu_copyAll, Me.ToolStripSeparator1, Me.logMenu_SelectAll, Me.ToolStripSeparator2, Me.logMenu_GoToBottom})
        Me.logMenu.Name = "logMenu"
        Me.logMenu.Size = New System.Drawing.Size(165, 104)
        '
        'logMenu_Copy
        '
        Me.logMenu_Copy.Name = "logMenu_Copy"
        Me.logMenu_Copy.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.logMenu_Copy.Size = New System.Drawing.Size(164, 22)
        Me.logMenu_Copy.Text = "Copy"
        '
        'logMenu_copyAll
        '
        Me.logMenu_copyAll.Name = "logMenu_copyAll"
        Me.logMenu_copyAll.Size = New System.Drawing.Size(164, 22)
        Me.logMenu_copyAll.Text = "Copy All"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(161, 6)
        '
        'logMenu_SelectAll
        '
        Me.logMenu_SelectAll.Name = "logMenu_SelectAll"
        Me.logMenu_SelectAll.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.logMenu_SelectAll.Size = New System.Drawing.Size(164, 22)
        Me.logMenu_SelectAll.Text = "Select All"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(161, 6)
        '
        'logMenu_GoToBottom
        '
        Me.logMenu_GoToBottom.Name = "logMenu_GoToBottom"
        Me.logMenu_GoToBottom.Size = New System.Drawing.Size(164, 22)
        Me.logMenu_GoToBottom.Text = "Scroll to end"
        '
        'text_input
        '
        Me.text_input.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.text_input.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.text_input.Location = New System.Drawing.Point(6, 375)
        Me.text_input.Name = "text_input"
        Me.text_input.Size = New System.Drawing.Size(473, 23)
        Me.text_input.TabIndex = 1
        '
        'box_players
        '
        Me.box_players.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.box_players.Controls.Add(Me.list_players)
        Me.box_players.Location = New System.Drawing.Point(4, 3)
        Me.box_players.Name = "box_players"
        Me.box_players.Size = New System.Drawing.Size(239, 407)
        Me.box_players.TabIndex = 5
        Me.box_players.TabStop = False
        Me.box_players.Text = "Players (0)"
        '
        'list_players
        '
        Me.list_players.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.list_players.ContextMenuStrip = Me.playerMenu
        Me.list_players.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.list_players.FormattingEnabled = True
        Me.list_players.ItemHeight = 15
        Me.list_players.Location = New System.Drawing.Point(9, 19)
        Me.list_players.Name = "list_players"
        Me.list_players.ScrollAlwaysVisible = True
        Me.list_players.Size = New System.Drawing.Size(224, 379)
        Me.list_players.TabIndex = 1
        '
        'playerMenu
        '
        Me.playerMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.playerMenu_nameItem, Me.ToolStripSeparator3, Me.playerMenu_PM, Me.ToolStripSeparator4, Me.playerMenu_Mute, Me.playerMenu_Kick, Me.ToolStripSeparator5, Me.playerMenu_whitelist, Me.playerMenu_blacklist, Me.ToolStripSeparator6, Me.playerMenu_operator})
        Me.playerMenu.Name = "playerMenu"
        Me.playerMenu.Size = New System.Drawing.Size(136, 182)
        '
        'playerMenu_nameItem
        '
        Me.playerMenu_nameItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.playerMenu_nameItem.Name = "playerMenu_nameItem"
        Me.playerMenu_nameItem.Size = New System.Drawing.Size(135, 22)
        Me.playerMenu_nameItem.Text = "<name>"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(132, 6)
        '
        'playerMenu_PM
        '
        Me.playerMenu_PM.Name = "playerMenu_PM"
        Me.playerMenu_PM.Size = New System.Drawing.Size(135, 22)
        Me.playerMenu_PM.Text = "Send PM"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(132, 6)
        '
        'playerMenu_Mute
        '
        Me.playerMenu_Mute.Name = "playerMenu_Mute"
        Me.playerMenu_Mute.Size = New System.Drawing.Size(135, 22)
        Me.playerMenu_Mute.Text = "<mute>"
        '
        'playerMenu_Kick
        '
        Me.playerMenu_Kick.Name = "playerMenu_Kick"
        Me.playerMenu_Kick.Size = New System.Drawing.Size(135, 22)
        Me.playerMenu_Kick.Text = "Kick"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(132, 6)
        '
        'playerMenu_whitelist
        '
        Me.playerMenu_whitelist.Name = "playerMenu_whitelist"
        Me.playerMenu_whitelist.Size = New System.Drawing.Size(135, 22)
        Me.playerMenu_whitelist.Text = "<whitelist>"
        '
        'playerMenu_blacklist
        '
        Me.playerMenu_blacklist.Name = "playerMenu_blacklist"
        Me.playerMenu_blacklist.Size = New System.Drawing.Size(135, 22)
        Me.playerMenu_blacklist.Text = "<blacklist>"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(132, 6)
        '
        'playerMenu_operator
        '
        Me.playerMenu_operator.Name = "playerMenu_operator"
        Me.playerMenu_operator.Size = New System.Drawing.Size(135, 22)
        Me.playerMenu_operator.Text = "<operator>"
        '
        'Mainform
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(738, 413)
        Me.Controls.Add(Me.box_output)
        Me.Controls.Add(Me.box_players)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(400, 300)
        Me.Name = "Mainform"
        Me.Text = "Pokémon3D - Server"
        Me.box_output.ResumeLayout(False)
        Me.box_output.PerformLayout()
        Me.logMenu.ResumeLayout(False)
        Me.box_players.ResumeLayout(False)
        Me.playerMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents box_output As System.Windows.Forms.GroupBox
    Friend WithEvents text_input As System.Windows.Forms.TextBox
    Friend WithEvents box_players As System.Windows.Forms.GroupBox
    Friend WithEvents list_players As System.Windows.Forms.ListBox
    Friend WithEvents text_output As System.Windows.Forms.TextBox
    Friend WithEvents logMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents logMenu_Copy As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents logMenu_copyAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents logMenu_SelectAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents logMenu_GoToBottom As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents playerMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents playerMenu_nameItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents playerMenu_PM As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents playerMenu_Mute As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents playerMenu_Kick As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents playerMenu_whitelist As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents playerMenu_blacklist As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents playerMenu_operator As System.Windows.Forms.ToolStripMenuItem

End Class
