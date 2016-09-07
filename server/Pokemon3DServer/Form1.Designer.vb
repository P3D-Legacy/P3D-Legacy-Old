<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.list_m = New System.Windows.Forms.ListBox()
        Me.list_players = New System.Windows.Forms.ListBox()
        Me.playerMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.playerMenu_nameItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.playerMenu_PM = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.playerMenu_Mute = New System.Windows.Forms.ToolStripMenuItem()
        Me.playerMenu_Kick = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.playerMenu_whitelist = New System.Windows.Forms.ToolStripMenuItem()
        Me.playerMenu_blacklist = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.playerMenu_operator = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.text_input = New System.Windows.Forms.TextBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.radio_RAMusage = New System.Windows.Forms.RadioButton()
        Me.radio_threads = New System.Windows.Forms.RadioButton()
        Me.RAMCounter = New System.Diagnostics.PerformanceCounter()
        Me.playerMenu.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.RAMCounter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'list_m
        '
        Me.list_m.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.list_m.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.list_m.FormattingEnabled = True
        Me.list_m.HorizontalScrollbar = True
        Me.list_m.ItemHeight = 15
        Me.list_m.Location = New System.Drawing.Point(6, 19)
        Me.list_m.Name = "list_m"
        Me.list_m.ScrollAlwaysVisible = True
        Me.list_m.Size = New System.Drawing.Size(473, 349)
        Me.list_m.TabIndex = 0
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
        Me.list_players.Size = New System.Drawing.Size(224, 184)
        Me.list_players.TabIndex = 1
        '
        'playerMenu
        '
        Me.playerMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.playerMenu_nameItem, Me.ToolStripSeparator1, Me.playerMenu_PM, Me.ToolStripSeparator2, Me.playerMenu_Mute, Me.playerMenu_Kick, Me.ToolStripSeparator3, Me.playerMenu_whitelist, Me.playerMenu_blacklist, Me.ToolStripSeparator4, Me.playerMenu_operator})
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
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(132, 6)
        '
        'playerMenu_PM
        '
        Me.playerMenu_PM.Name = "playerMenu_PM"
        Me.playerMenu_PM.Size = New System.Drawing.Size(135, 22)
        Me.playerMenu_PM.Text = "Send PM"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(132, 6)
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
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(132, 6)
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
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(132, 6)
        '
        'playerMenu_operator
        '
        Me.playerMenu_operator.Name = "playerMenu_operator"
        Me.playerMenu_operator.Size = New System.Drawing.Size(135, 22)
        Me.playerMenu_operator.Text = "<operator>"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.list_players)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 196)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(239, 212)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Players (0)"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.text_input)
        Me.GroupBox2.Controls.Add(Me.list_m)
        Me.GroupBox2.Location = New System.Drawing.Point(248, 1)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(485, 407)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Log and chat"
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
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Panel1)
        Me.GroupBox3.Location = New System.Drawing.Point(3, 1)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(239, 189)
        Me.GroupBox3.TabIndex = 4
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Statistics"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.radio_RAMusage)
        Me.Panel1.Controls.Add(Me.radio_threads)
        Me.Panel1.Location = New System.Drawing.Point(9, 19)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(224, 164)
        Me.Panel1.TabIndex = 0
        '
        'radio_RAMusage
        '
        Me.radio_RAMusage.AutoSize = True
        Me.radio_RAMusage.Checked = True
        Me.radio_RAMusage.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radio_RAMusage.Location = New System.Drawing.Point(15, 125)
        Me.radio_RAMusage.Name = "radio_RAMusage"
        Me.radio_RAMusage.Size = New System.Drawing.Size(123, 19)
        Me.radio_RAMusage.TabIndex = 1
        Me.radio_RAMusage.TabStop = True
        Me.radio_RAMusage.Text = "RAM usage: 0 %"
        Me.radio_RAMusage.UseVisualStyleBackColor = True
        '
        'radio_threads
        '
        Me.radio_threads.AutoSize = True
        Me.radio_threads.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radio_threads.Location = New System.Drawing.Point(15, 100)
        Me.radio_threads.Name = "radio_threads"
        Me.radio_threads.Size = New System.Drawing.Size(95, 19)
        Me.radio_threads.TabIndex = 1
        Me.radio_threads.Text = "Threads: 0"
        Me.radio_threads.UseVisualStyleBackColor = True
        '
        'RAMCounter
        '
        Me.RAMCounter.CategoryName = "Memory"
        Me.RAMCounter.CounterName = "% Committed Bytes In Use"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(738, 413)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(400, 300)
        Me.Name = "Form1"
        Me.Text = "Pokémon3D - Server"
        Me.playerMenu.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.RAMCounter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents list_m As System.Windows.Forms.ListBox
    Friend WithEvents list_players As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents text_input As System.Windows.Forms.TextBox
    Friend WithEvents playerMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents playerMenu_nameItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents playerMenu_PM As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents playerMenu_Kick As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents playerMenu_whitelist As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents playerMenu_blacklist As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents playerMenu_operator As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents playerMenu_Mute As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents radio_RAMusage As System.Windows.Forms.RadioButton
    Friend WithEvents radio_threads As System.Windows.Forms.RadioButton
    Friend WithEvents RAMCounter As System.Diagnostics.PerformanceCounter

End Class
