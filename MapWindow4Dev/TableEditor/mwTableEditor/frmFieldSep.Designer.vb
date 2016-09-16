<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFieldSep
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFieldSep))
        Me.lblCheckedFld = New System.Windows.Forms.Label
        Me.grpSepatator = New System.Windows.Forms.GroupBox
        Me.tBoxOther = New System.Windows.Forms.TextBox
        Me.rdBtOther = New System.Windows.Forms.RadioButton
        Me.rdBtColon = New System.Windows.Forms.RadioButton
        Me.rdBtSpace = New System.Windows.Forms.RadioButton
        Me.lblField = New System.Windows.Forms.Label
        Me.grpNewFields = New System.Windows.Forms.GroupBox
        Me.lblFieldName = New System.Windows.Forms.Label
        Me.tBoxNField = New System.Windows.Forms.TextBox
        Me.btnAdd = New System.Windows.Forms.Button
        Me.btnRemove = New System.Windows.Forms.Button
        Me.lstFields = New System.Windows.Forms.ListBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.grpSepatator.SuspendLayout()
        Me.grpNewFields.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblCheckedFld
        '
        resources.ApplyResources(Me.lblCheckedFld, "lblCheckedFld")
        Me.lblCheckedFld.Name = "lblCheckedFld"
        '
        'grpSepatator
        '
        Me.grpSepatator.Controls.Add(Me.tBoxOther)
        Me.grpSepatator.Controls.Add(Me.rdBtOther)
        Me.grpSepatator.Controls.Add(Me.rdBtColon)
        Me.grpSepatator.Controls.Add(Me.rdBtSpace)
        resources.ApplyResources(Me.grpSepatator, "grpSepatator")
        Me.grpSepatator.Name = "grpSepatator"
        Me.grpSepatator.TabStop = False
        '
        'tBoxOther
        '
        resources.ApplyResources(Me.tBoxOther, "tBoxOther")
        Me.tBoxOther.Name = "tBoxOther"
        '
        'rdBtOther
        '
        resources.ApplyResources(Me.rdBtOther, "rdBtOther")
        Me.rdBtOther.Name = "rdBtOther"
        Me.rdBtOther.UseVisualStyleBackColor = True
        '
        'rdBtColon
        '
        resources.ApplyResources(Me.rdBtColon, "rdBtColon")
        Me.rdBtColon.Name = "rdBtColon"
        Me.rdBtColon.UseVisualStyleBackColor = True
        '
        'rdBtSpace
        '
        resources.ApplyResources(Me.rdBtSpace, "rdBtSpace")
        Me.rdBtSpace.Checked = True
        Me.rdBtSpace.Name = "rdBtSpace"
        Me.rdBtSpace.TabStop = True
        Me.rdBtSpace.UseVisualStyleBackColor = True
        '
        'lblField
        '
        resources.ApplyResources(Me.lblField, "lblField")
        Me.lblField.Name = "lblField"
        '
        'grpNewFields
        '
        Me.grpNewFields.Controls.Add(Me.lblFieldName)
        Me.grpNewFields.Controls.Add(Me.tBoxNField)
        Me.grpNewFields.Controls.Add(Me.btnAdd)
        Me.grpNewFields.Controls.Add(Me.btnRemove)
        Me.grpNewFields.Controls.Add(Me.lstFields)
        resources.ApplyResources(Me.grpNewFields, "grpNewFields")
        Me.grpNewFields.Name = "grpNewFields"
        Me.grpNewFields.TabStop = False
        '
        'lblFieldName
        '
        resources.ApplyResources(Me.lblFieldName, "lblFieldName")
        Me.lblFieldName.Name = "lblFieldName"
        '
        'tBoxNField
        '
        resources.ApplyResources(Me.tBoxNField, "tBoxNField")
        Me.tBoxNField.Name = "tBoxNField"
        '
        'btnAdd
        '
        resources.ApplyResources(Me.btnAdd, "btnAdd")
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        resources.ApplyResources(Me.btnRemove, "btnRemove")
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'lstFields
        '
        Me.lstFields.FormattingEnabled = True
        resources.ApplyResources(Me.lstFields, "lstFields")
        Me.lstFields.Name = "lstFields"
        '
        'btnOK
        '
        resources.ApplyResources(Me.btnOK, "btnOK")
        Me.btnOK.Name = "btnOK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        resources.ApplyResources(Me.btnCancel, "btnCancel")
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'frmFieldSep
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.grpNewFields)
        Me.Controls.Add(Me.lblField)
        Me.Controls.Add(Me.grpSepatator)
        Me.Controls.Add(Me.lblCheckedFld)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFieldSep"
        Me.grpSepatator.ResumeLayout(False)
        Me.grpSepatator.PerformLayout()
        Me.grpNewFields.ResumeLayout(False)
        Me.grpNewFields.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblCheckedFld As System.Windows.Forms.Label
    Friend WithEvents grpSepatator As System.Windows.Forms.GroupBox
    Friend WithEvents tBoxOther As System.Windows.Forms.TextBox
    Friend WithEvents rdBtOther As System.Windows.Forms.RadioButton
    Friend WithEvents rdBtColon As System.Windows.Forms.RadioButton
    Friend WithEvents rdBtSpace As System.Windows.Forms.RadioButton
    Friend WithEvents lblField As System.Windows.Forms.Label
    Friend WithEvents grpNewFields As System.Windows.Forms.GroupBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lstFields As System.Windows.Forms.ListBox
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents tBoxNField As System.Windows.Forms.TextBox
    Friend WithEvents lblFieldName As System.Windows.Forms.Label
End Class
