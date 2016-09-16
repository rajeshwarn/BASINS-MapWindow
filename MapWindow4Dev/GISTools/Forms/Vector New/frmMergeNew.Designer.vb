<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMergeNew
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMergeNew))
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.txtFilename = New System.Windows.Forms.TextBox
        Me.lblSaveAs = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblSelected1 = New System.Windows.Forms.Label
        Me.chkSelectedOnly1 = New System.Windows.Forms.CheckBox
        Me.cboLayer1 = New System.Windows.Forms.ComboBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblSelected2 = New System.Windows.Forms.Label
        Me.cboLayer2 = New System.Windows.Forms.ComboBox
        Me.chkSelectedOnly2 = New System.Windows.Forms.CheckBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btnBrowse)
        Me.GroupBox3.Controls.Add(Me.txtFilename)
        Me.GroupBox3.Controls.Add(Me.lblSaveAs)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 193)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(408, 91)
        Me.GroupBox3.TabIndex = 33
        Me.GroupBox3.TabStop = False
        '
        'btnBrowse
        '
        Me.btnBrowse.Image = CType(resources.GetObject("btnBrowse.Image"), System.Drawing.Image)
        Me.btnBrowse.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnBrowse.Location = New System.Drawing.Point(366, 43)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(25, 24)
        Me.btnBrowse.TabIndex = 20
        '
        'txtFilename
        '
        Me.txtFilename.Location = New System.Drawing.Point(15, 47)
        Me.txtFilename.Name = "txtFilename"
        Me.txtFilename.ReadOnly = True
        Me.txtFilename.Size = New System.Drawing.Size(342, 20)
        Me.txtFilename.TabIndex = 16
        '
        'lblSaveAs
        '
        Me.lblSaveAs.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblSaveAs.Location = New System.Drawing.Point(15, 28)
        Me.lblSaveAs.Name = "lblSaveAs"
        Me.lblSaveAs.Size = New System.Drawing.Size(191, 16)
        Me.lblSaveAs.TabIndex = 18
        Me.lblSaveAs.Text = "File to Save Results To:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.lblSelected1)
        Me.GroupBox2.Controls.Add(Me.chkSelectedOnly1)
        Me.GroupBox2.Controls.Add(Me.cboLayer1)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(408, 86)
        Me.GroupBox2.TabIndex = 32
        Me.GroupBox2.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(17, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 13)
        Me.Label1.TabIndex = 22
        Me.Label1.Text = "Vector Layer 1"
        '
        'lblSelected1
        '
        Me.lblSelected1.Location = New System.Drawing.Point(218, 62)
        Me.lblSelected1.Name = "lblSelected1"
        Me.lblSelected1.Size = New System.Drawing.Size(172, 13)
        Me.lblSelected1.TabIndex = 19
        Me.lblSelected1.Text = "Number of selected:"
        Me.lblSelected1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'chkSelectedOnly1
        '
        Me.chkSelectedOnly1.AutoSize = True
        Me.chkSelectedOnly1.Location = New System.Drawing.Point(20, 61)
        Me.chkSelectedOnly1.Name = "chkSelectedOnly1"
        Me.chkSelectedOnly1.Size = New System.Drawing.Size(127, 17)
        Me.chkSelectedOnly1.TabIndex = 4
        Me.chkSelectedOnly1.Text = "Selected objects only"
        Me.chkSelectedOnly1.UseVisualStyleBackColor = True
        '
        'cboLayer1
        '
        Me.cboLayer1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLayer1.FormattingEnabled = True
        Me.cboLayer1.Location = New System.Drawing.Point(17, 34)
        Me.cboLayer1.Name = "cboLayer1"
        Me.cboLayer1.Size = New System.Drawing.Size(373, 21)
        Me.cboLayer1.TabIndex = 3
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblSelected2)
        Me.GroupBox1.Controls.Add(Me.cboLayer2)
        Me.GroupBox1.Controls.Add(Me.chkSelectedOnly2)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 104)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(408, 83)
        Me.GroupBox1.TabIndex = 31
        Me.GroupBox1.TabStop = False
        '
        'lblSelected2
        '
        Me.lblSelected2.Location = New System.Drawing.Point(217, 56)
        Me.lblSelected2.Name = "lblSelected2"
        Me.lblSelected2.Size = New System.Drawing.Size(172, 13)
        Me.lblSelected2.TabIndex = 24
        Me.lblSelected2.Text = "Number of selected:"
        Me.lblSelected2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cboLayer2
        '
        Me.cboLayer2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboLayer2.FormattingEnabled = True
        Me.cboLayer2.Location = New System.Drawing.Point(16, 28)
        Me.cboLayer2.Name = "cboLayer2"
        Me.cboLayer2.Size = New System.Drawing.Size(373, 21)
        Me.cboLayer2.TabIndex = 22
        '
        'chkSelectedOnly2
        '
        Me.chkSelectedOnly2.AutoSize = True
        Me.chkSelectedOnly2.Location = New System.Drawing.Point(19, 55)
        Me.chkSelectedOnly2.Name = "chkSelectedOnly2"
        Me.chkSelectedOnly2.Size = New System.Drawing.Size(127, 17)
        Me.chkSelectedOnly2.TabIndex = 23
        Me.chkSelectedOnly2.Text = "Selected objects only"
        Me.chkSelectedOnly2.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(17, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(76, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Vector Layer 2"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(341, 290)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(79, 26)
        Me.btnCancel.TabIndex = 30
        Me.btnCancel.Text = "Close"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Location = New System.Drawing.Point(256, 290)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(79, 26)
        Me.btnOk.TabIndex = 29
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'frmMergeNew
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(429, 324)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMergeNew"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Merging shapefiles"
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents txtFilename As System.Windows.Forms.TextBox
    Friend WithEvents lblSaveAs As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblSelected1 As System.Windows.Forms.Label
    Friend WithEvents chkSelectedOnly1 As System.Windows.Forms.CheckBox
    Friend WithEvents cboLayer1 As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblSelected2 As System.Windows.Forms.Label
    Friend WithEvents cboLayer2 As System.Windows.Forms.ComboBox
    Friend WithEvents chkSelectedOnly2 As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
End Class
