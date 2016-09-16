<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmArchiveToolForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmArchiveToolForm))
        Me.chbAllFiles = New System.Windows.Forms.CheckBox
        Me.chbPreserveLocations = New System.Windows.Forms.CheckBox
        Me.txtNotes = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.grpArchive = New System.Windows.Forms.GroupBox
        Me.chkIncludeUserCompName = New System.Windows.Forms.CheckBox
        Me.btnArchive = New System.Windows.Forms.Button
        Me.lblCompression = New System.Windows.Forms.Label
        Me.tbCompression = New System.Windows.Forms.TrackBar
        Me.grpRestore = New System.Windows.Forms.GroupBox
        Me.btnRestore = New System.Windows.Forms.Button
        Me.chbIntoThisProject = New System.Windows.Forms.CheckBox
        Me.lnkDetails = New System.Windows.Forms.LinkLabel
        Me.btnRestoreTo = New System.Windows.Forms.Button
        Me.txtRestoreTo = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnOpenArchive = New System.Windows.Forms.Button
        Me.txtArchive = New System.Windows.Forms.TextBox
        Me.Button5 = New System.Windows.Forms.Button
        Me.prg1 = New System.Windows.Forms.ProgressBar
        Me.grpArchive.SuspendLayout()
        CType(Me.tbCompression, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpRestore.SuspendLayout()
        Me.SuspendLayout()
        '
        'chbAllFiles
        '
        resources.ApplyResources(Me.chbAllFiles, "chbAllFiles")
        Me.chbAllFiles.Name = "chbAllFiles"
        Me.chbAllFiles.UseVisualStyleBackColor = True
        '
        'chbPreserveLocations
        '
        resources.ApplyResources(Me.chbPreserveLocations, "chbPreserveLocations")
        Me.chbPreserveLocations.Name = "chbPreserveLocations"
        Me.chbPreserveLocations.UseVisualStyleBackColor = True
        '
        'txtNotes
        '
        resources.ApplyResources(Me.txtNotes, "txtNotes")
        Me.txtNotes.Name = "txtNotes"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'grpArchive
        '
        Me.grpArchive.Controls.Add(Me.chkIncludeUserCompName)
        Me.grpArchive.Controls.Add(Me.btnArchive)
        Me.grpArchive.Controls.Add(Me.lblCompression)
        Me.grpArchive.Controls.Add(Me.tbCompression)
        Me.grpArchive.Controls.Add(Me.txtNotes)
        Me.grpArchive.Controls.Add(Me.chbPreserveLocations)
        Me.grpArchive.Controls.Add(Me.Label1)
        Me.grpArchive.Controls.Add(Me.chbAllFiles)
        resources.ApplyResources(Me.grpArchive, "grpArchive")
        Me.grpArchive.Name = "grpArchive"
        Me.grpArchive.TabStop = False
        '
        'chkIncludeUserCompName
        '
        Me.chkIncludeUserCompName.Checked = True
        Me.chkIncludeUserCompName.CheckState = System.Windows.Forms.CheckState.Checked
        resources.ApplyResources(Me.chkIncludeUserCompName, "chkIncludeUserCompName")
        Me.chkIncludeUserCompName.Name = "chkIncludeUserCompName"
        Me.chkIncludeUserCompName.UseVisualStyleBackColor = True
        '
        'btnArchive
        '
        resources.ApplyResources(Me.btnArchive, "btnArchive")
        Me.btnArchive.Name = "btnArchive"
        Me.btnArchive.UseVisualStyleBackColor = True
        '
        'lblCompression
        '
        resources.ApplyResources(Me.lblCompression, "lblCompression")
        Me.lblCompression.Name = "lblCompression"
        '
        'tbCompression
        '
        Me.tbCompression.LargeChange = 2
        resources.ApplyResources(Me.tbCompression, "tbCompression")
        Me.tbCompression.Maximum = 9
        Me.tbCompression.Name = "tbCompression"
        Me.tbCompression.Value = 5
        '
        'grpRestore
        '
        Me.grpRestore.Controls.Add(Me.btnRestore)
        Me.grpRestore.Controls.Add(Me.chbIntoThisProject)
        Me.grpRestore.Controls.Add(Me.lnkDetails)
        Me.grpRestore.Controls.Add(Me.btnRestoreTo)
        Me.grpRestore.Controls.Add(Me.txtRestoreTo)
        Me.grpRestore.Controls.Add(Me.Label3)
        Me.grpRestore.Controls.Add(Me.Label2)
        Me.grpRestore.Controls.Add(Me.btnOpenArchive)
        Me.grpRestore.Controls.Add(Me.txtArchive)
        resources.ApplyResources(Me.grpRestore, "grpRestore")
        Me.grpRestore.Name = "grpRestore"
        Me.grpRestore.TabStop = False
        '
        'btnRestore
        '
        resources.ApplyResources(Me.btnRestore, "btnRestore")
        Me.btnRestore.Name = "btnRestore"
        Me.btnRestore.UseVisualStyleBackColor = True
        '
        'chbIntoThisProject
        '
        resources.ApplyResources(Me.chbIntoThisProject, "chbIntoThisProject")
        Me.chbIntoThisProject.Name = "chbIntoThisProject"
        Me.chbIntoThisProject.UseVisualStyleBackColor = True
        '
        'lnkDetails
        '
        resources.ApplyResources(Me.lnkDetails, "lnkDetails")
        Me.lnkDetails.Name = "lnkDetails"
        Me.lnkDetails.TabStop = True
        '
        'btnRestoreTo
        '
        resources.ApplyResources(Me.btnRestoreTo, "btnRestoreTo")
        Me.btnRestoreTo.Name = "btnRestoreTo"
        '
        'txtRestoreTo
        '
        resources.ApplyResources(Me.txtRestoreTo, "txtRestoreTo")
        Me.txtRestoreTo.Name = "txtRestoreTo"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'btnOpenArchive
        '
        resources.ApplyResources(Me.btnOpenArchive, "btnOpenArchive")
        Me.btnOpenArchive.Name = "btnOpenArchive"
        '
        'txtArchive
        '
        resources.ApplyResources(Me.txtArchive, "txtArchive")
        Me.txtArchive.BackColor = System.Drawing.SystemColors.Window
        Me.txtArchive.Name = "txtArchive"
        Me.txtArchive.ReadOnly = True
        '
        'Button5
        '
        Me.Button5.DialogResult = System.Windows.Forms.DialogResult.Cancel
        resources.ApplyResources(Me.Button5, "Button5")
        Me.Button5.Name = "Button5"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'prg1
        '
        resources.ApplyResources(Me.prg1, "prg1")
        Me.prg1.Name = "prg1"
        '
        'frmArchiveToolForm
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.prg1)
        Me.Controls.Add(Me.Button5)
        Me.Controls.Add(Me.grpRestore)
        Me.Controls.Add(Me.grpArchive)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmArchiveToolForm"
        Me.grpArchive.ResumeLayout(False)
        Me.grpArchive.PerformLayout()
        CType(Me.tbCompression, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpRestore.ResumeLayout(False)
        Me.grpRestore.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents chbAllFiles As System.Windows.Forms.CheckBox
    Friend WithEvents chbPreserveLocations As System.Windows.Forms.CheckBox
    Friend WithEvents txtNotes As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents grpArchive As System.Windows.Forms.GroupBox
    Friend WithEvents grpRestore As System.Windows.Forms.GroupBox
    Friend WithEvents lnkDetails As System.Windows.Forms.LinkLabel
    Friend WithEvents btnRestoreTo As System.Windows.Forms.Button
    Friend WithEvents txtRestoreTo As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnOpenArchive As System.Windows.Forms.Button
    Friend WithEvents txtArchive As System.Windows.Forms.TextBox
    Friend WithEvents btnArchive As System.Windows.Forms.Button
    Friend WithEvents btnRestore As System.Windows.Forms.Button
    Friend WithEvents chbIntoThisProject As System.Windows.Forms.CheckBox
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents prg1 As System.Windows.Forms.ProgressBar
    Friend WithEvents lblCompression As System.Windows.Forms.Label
    Friend WithEvents tbCompression As System.Windows.Forms.TrackBar
    Friend WithEvents chkIncludeUserCompName As System.Windows.Forms.CheckBox
End Class
