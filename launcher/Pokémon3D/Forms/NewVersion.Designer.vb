<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewVersion
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
        Me.download_progress = New System.Windows.Forms.ProgressBar()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.info_label = New System.Windows.Forms.Label()
        Me.button_cancel = New System.Windows.Forms.Button()
        Me.button_download = New System.Windows.Forms.Button()
        Me.statuslabel = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.readytimer = New System.Windows.Forms.Timer(Me.components)
        Me.zipFileTimer = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'download_progress
        '
        Me.download_progress.Location = New System.Drawing.Point(15, 158)
        Me.download_progress.Name = "download_progress"
        Me.download_progress.Size = New System.Drawing.Size(348, 23)
        Me.download_progress.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(191, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Download the latest game files:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.info_label)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(12, 28)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(348, 75)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Info"
        '
        'info_label
        '
        Me.info_label.AutoSize = True
        Me.info_label.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.info_label.Location = New System.Drawing.Point(15, 25)
        Me.info_label.Name = "info_label"
        Me.info_label.Size = New System.Drawing.Size(76, 26)
        Me.info_label.TabIndex = 0
        Me.info_label.Text = "Gameversion:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Release date:"
        '
        'button_cancel
        '
        Me.button_cancel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.button_cancel.Location = New System.Drawing.Point(285, 109)
        Me.button_cancel.Name = "button_cancel"
        Me.button_cancel.Size = New System.Drawing.Size(75, 23)
        Me.button_cancel.TabIndex = 3
        Me.button_cancel.Text = "Cancel"
        Me.button_cancel.UseVisualStyleBackColor = True
        '
        'button_download
        '
        Me.button_download.Enabled = False
        Me.button_download.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.button_download.Location = New System.Drawing.Point(204, 109)
        Me.button_download.Name = "button_download"
        Me.button_download.Size = New System.Drawing.Size(75, 23)
        Me.button_download.TabIndex = 3
        Me.button_download.Text = "Download"
        Me.button_download.UseVisualStyleBackColor = True
        '
        'statuslabel
        '
        Me.statuslabel.AutoSize = True
        Me.statuslabel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.statuslabel.Location = New System.Drawing.Point(12, 119)
        Me.statuslabel.Name = "statuslabel"
        Me.statuslabel.Size = New System.Drawing.Size(0, 13)
        Me.statuslabel.TabIndex = 4
        '
        'Timer1
        '
        Me.Timer1.Interval = 500
        '
        'readytimer
        '
        Me.readytimer.Enabled = True
        Me.readytimer.Interval = 10
        '
        'zipFileTimer
        '
        Me.zipFileTimer.Enabled = True
        Me.zipFileTimer.Interval = 1
        '
        'NewVersion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(372, 193)
        Me.ControlBox = False
        Me.Controls.Add(Me.statuslabel)
        Me.Controls.Add(Me.button_download)
        Me.Controls.Add(Me.button_cancel)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.download_progress)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "NewVersion"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Pokémon3D - Game Update"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents download_progress As System.Windows.Forms.ProgressBar
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents info_label As System.Windows.Forms.Label
    Friend WithEvents button_cancel As System.Windows.Forms.Button
    Friend WithEvents button_download As System.Windows.Forms.Button
    Friend WithEvents statuslabel As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents readytimer As System.Windows.Forms.Timer
    Friend WithEvents zipFileTimer As System.Windows.Forms.Timer
End Class
