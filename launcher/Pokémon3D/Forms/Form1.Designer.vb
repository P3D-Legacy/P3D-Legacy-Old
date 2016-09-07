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
        Me.web_display = New System.Windows.Forms.WebBrowser()
        Me.button_start = New System.Windows.Forms.Button()
        Me.download_button = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.ticknewupdate = New System.Windows.Forms.Timer(Me.components)
        Me.link_settings = New System.Windows.Forms.LinkLabel()
        Me.tickautostart = New System.Windows.Forms.Timer(Me.components)
        Me.autostart_group = New System.Windows.Forms.GroupBox()
        Me.autostart_progress = New System.Windows.Forms.ProgressBar()
        Me.button_stopautostart = New System.Windows.Forms.Button()
        Me.btn_crashlog_open = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.autostart_group.SuspendLayout()
        Me.SuspendLayout()
        '
        'web_display
        '
        Me.web_display.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.web_display.IsWebBrowserContextMenuEnabled = False
        Me.web_display.Location = New System.Drawing.Point(0, 0)
        Me.web_display.MinimumSize = New System.Drawing.Size(20, 20)
        Me.web_display.Name = "web_display"
        Me.web_display.ScriptErrorsSuppressed = True
        Me.web_display.Size = New System.Drawing.Size(843, 369)
        Me.web_display.TabIndex = 0
        Me.web_display.TabStop = False
        Me.web_display.WebBrowserShortcutsEnabled = False
        '
        'button_start
        '
        Me.button_start.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.button_start.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.button_start.Location = New System.Drawing.Point(661, 388)
        Me.button_start.Name = "button_start"
        Me.button_start.Size = New System.Drawing.Size(151, 23)
        Me.button_start.TabIndex = 1
        Me.button_start.Text = "Start game"
        Me.button_start.UseVisualStyleBackColor = True
        '
        'download_button
        '
        Me.download_button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.download_button.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.download_button.Location = New System.Drawing.Point(661, 417)
        Me.download_button.Name = "download_button"
        Me.download_button.Size = New System.Drawing.Size(151, 23)
        Me.download_button.TabIndex = 2
        Me.download_button.Text = "Download latest version"
        Me.download_button.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.PictureBox1.Image = Global.Pokémon3D.My.Resources.Resources.Logo___3D
        Me.PictureBox1.Location = New System.Drawing.Point(24, 391)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(283, 64)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'ticknewupdate
        '
        Me.ticknewupdate.Enabled = True
        Me.ticknewupdate.Interval = 10
        '
        'link_settings
        '
        Me.link_settings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.link_settings.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.link_settings.LinkColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(112, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.link_settings.Location = New System.Drawing.Point(669, 445)
        Me.link_settings.Name = "link_settings"
        Me.link_settings.Size = New System.Drawing.Size(143, 23)
        Me.link_settings.TabIndex = 3
        Me.link_settings.TabStop = True
        Me.link_settings.Text = "Settings"
        Me.link_settings.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tickautostart
        '
        Me.tickautostart.Interval = 1000
        '
        'autostart_group
        '
        Me.autostart_group.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.autostart_group.Controls.Add(Me.autostart_progress)
        Me.autostart_group.Controls.Add(Me.button_stopautostart)
        Me.autostart_group.Location = New System.Drawing.Point(612, 388)
        Me.autostart_group.Name = "autostart_group"
        Me.autostart_group.Size = New System.Drawing.Size(200, 80)
        Me.autostart_group.TabIndex = 6
        Me.autostart_group.TabStop = False
        Me.autostart_group.Text = "Autostarting..."
        Me.autostart_group.Visible = False
        '
        'autostart_progress
        '
        Me.autostart_progress.Location = New System.Drawing.Point(6, 19)
        Me.autostart_progress.Name = "autostart_progress"
        Me.autostart_progress.Size = New System.Drawing.Size(188, 17)
        Me.autostart_progress.TabIndex = 6
        '
        'button_stopautostart
        '
        Me.button_stopautostart.Location = New System.Drawing.Point(6, 44)
        Me.button_stopautostart.Name = "button_stopautostart"
        Me.button_stopautostart.Size = New System.Drawing.Size(188, 23)
        Me.button_stopautostart.TabIndex = 5
        Me.button_stopautostart.Text = "Cancel autostart"
        Me.button_stopautostart.UseVisualStyleBackColor = True
        '
        'btn_crashlog_open
        '
        Me.btn_crashlog_open.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btn_crashlog_open.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btn_crashlog_open.Location = New System.Drawing.Point(330, 388)
        Me.btn_crashlog_open.Name = "btn_crashlog_open"
        Me.btn_crashlog_open.Size = New System.Drawing.Size(242, 33)
        Me.btn_crashlog_open.TabIndex = 7
        Me.btn_crashlog_open.Text = "Open crashlog location"
        Me.btn_crashlog_open.UseVisualStyleBackColor = True
        Me.btn_crashlog_open.Visible = False
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(844, 481)
        Me.Controls.Add(Me.btn_crashlog_open)
        Me.Controls.Add(Me.autostart_group)
        Me.Controls.Add(Me.link_settings)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.download_button)
        Me.Controls.Add(Me.button_start)
        Me.Controls.Add(Me.web_display)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(600, 272)
        Me.Name = "Form1"
        Me.Text = "Pokémon 3D Launcher"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.autostart_group.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents web_display As System.Windows.Forms.WebBrowser
    Friend WithEvents button_start As System.Windows.Forms.Button
    Friend WithEvents download_button As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents ticknewupdate As System.Windows.Forms.Timer
    Friend WithEvents link_settings As System.Windows.Forms.LinkLabel
    Friend WithEvents tickautostart As System.Windows.Forms.Timer
    Friend WithEvents autostart_group As System.Windows.Forms.GroupBox
    Friend WithEvents autostart_progress As System.Windows.Forms.ProgressBar
    Friend WithEvents button_stopautostart As System.Windows.Forms.Button
    Friend WithEvents btn_crashlog_open As System.Windows.Forms.Button

End Class
