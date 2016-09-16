<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMergeShapefiles
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMergeShapefiles))
        Me.Label1 = New System.Windows.Forms.Label
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.rdIgnore = New System.Windows.Forms.RadioButton
        Me.rdGeometry = New System.Windows.Forms.RadioButton
        Me.rdAttributes = New System.Windows.Forms.RadioButton
        Me.Label4 = New System.Windows.Forms.Label
        Me.btnBrowseToOut = New System.Windows.Forms.Button
        Me.txtOut = New System.Windows.Forms.TextBox
        Me.rdOneAttribute = New System.Windows.Forms.RadioButton
        Me.cmbField = New System.Windows.Forms.ComboBox
        Me.chbAddtoMap = New System.Windows.Forms.CheckBox
        Me.txtProgress = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AccessibleDescription = Nothing
        Me.Label1.AccessibleName = Nothing
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Font = Nothing
        Me.Label1.Name = "Label1"
        '
        'ListBox1
        '
        Me.ListBox1.AccessibleDescription = Nothing
        Me.ListBox1.AccessibleName = Nothing
        resources.ApplyResources(Me.ListBox1, "ListBox1")
        Me.ListBox1.BackgroundImage = Nothing
        Me.ListBox1.Font = Nothing
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Name = "ListBox1"
        '
        'Label2
        '
        Me.Label2.AccessibleDescription = Nothing
        Me.Label2.AccessibleName = Nothing
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Font = Nothing
        Me.Label2.Name = "Label2"
        '
        'Button1
        '
        Me.Button1.AccessibleDescription = Nothing
        Me.Button1.AccessibleName = Nothing
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.BackgroundImage = Nothing
        Me.Button1.Font = Nothing
        Me.Button1.Name = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.AccessibleDescription = Nothing
        Me.Button2.AccessibleName = Nothing
        resources.ApplyResources(Me.Button2, "Button2")
        Me.Button2.BackgroundImage = Nothing
        Me.Button2.Font = Nothing
        Me.Button2.Name = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.AccessibleDescription = Nothing
        Me.Button3.AccessibleName = Nothing
        resources.ApplyResources(Me.Button3, "Button3")
        Me.Button3.BackgroundImage = Nothing
        Me.Button3.Font = Nothing
        Me.Button3.Name = "Button3"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AccessibleDescription = Nothing
        Me.Label3.AccessibleName = Nothing
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Font = Nothing
        Me.Label3.Name = "Label3"
        '
        'rdIgnore
        '
        Me.rdIgnore.AccessibleDescription = Nothing
        Me.rdIgnore.AccessibleName = Nothing
        resources.ApplyResources(Me.rdIgnore, "rdIgnore")
        Me.rdIgnore.BackgroundImage = Nothing
        Me.rdIgnore.Font = Nothing
        Me.rdIgnore.Name = "rdIgnore"
        Me.rdIgnore.UseVisualStyleBackColor = True
        '
        'rdGeometry
        '
        Me.rdGeometry.AccessibleDescription = Nothing
        Me.rdGeometry.AccessibleName = Nothing
        resources.ApplyResources(Me.rdGeometry, "rdGeometry")
        Me.rdGeometry.BackgroundImage = Nothing
        Me.rdGeometry.Checked = True
        Me.rdGeometry.Font = Nothing
        Me.rdGeometry.Name = "rdGeometry"
        Me.rdGeometry.TabStop = True
        Me.rdGeometry.UseVisualStyleBackColor = True
        '
        'rdAttributes
        '
        Me.rdAttributes.AccessibleDescription = Nothing
        Me.rdAttributes.AccessibleName = Nothing
        resources.ApplyResources(Me.rdAttributes, "rdAttributes")
        Me.rdAttributes.BackgroundImage = Nothing
        Me.rdAttributes.Font = Nothing
        Me.rdAttributes.Name = "rdAttributes"
        Me.rdAttributes.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AccessibleDescription = Nothing
        Me.Label4.AccessibleName = Nothing
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Font = Nothing
        Me.Label4.Name = "Label4"
        '
        'btnBrowseToOut
        '
        Me.btnBrowseToOut.AccessibleDescription = Nothing
        Me.btnBrowseToOut.AccessibleName = Nothing
        resources.ApplyResources(Me.btnBrowseToOut, "btnBrowseToOut")
        Me.btnBrowseToOut.BackgroundImage = Nothing
        Me.btnBrowseToOut.Font = Nothing
        Me.btnBrowseToOut.Name = "btnBrowseToOut"
        '
        'txtOut
        '
        Me.txtOut.AccessibleDescription = Nothing
        Me.txtOut.AccessibleName = Nothing
        resources.ApplyResources(Me.txtOut, "txtOut")
        Me.txtOut.BackgroundImage = Nothing
        Me.txtOut.Font = Nothing
        Me.txtOut.Name = "txtOut"
        '
        'rdOneAttribute
        '
        Me.rdOneAttribute.AccessibleDescription = Nothing
        Me.rdOneAttribute.AccessibleName = Nothing
        resources.ApplyResources(Me.rdOneAttribute, "rdOneAttribute")
        Me.rdOneAttribute.BackgroundImage = Nothing
        Me.rdOneAttribute.Font = Nothing
        Me.rdOneAttribute.Name = "rdOneAttribute"
        Me.rdOneAttribute.UseVisualStyleBackColor = True
        '
        'cmbField
        '
        Me.cmbField.AccessibleDescription = Nothing
        Me.cmbField.AccessibleName = Nothing
        resources.ApplyResources(Me.cmbField, "cmbField")
        Me.cmbField.BackgroundImage = Nothing
        Me.cmbField.Font = Nothing
        Me.cmbField.FormattingEnabled = True
        Me.cmbField.Name = "cmbField"
        '
        'chbAddtoMap
        '
        Me.chbAddtoMap.AccessibleDescription = Nothing
        Me.chbAddtoMap.AccessibleName = Nothing
        resources.ApplyResources(Me.chbAddtoMap, "chbAddtoMap")
        Me.chbAddtoMap.BackgroundImage = Nothing
        Me.chbAddtoMap.Checked = True
        Me.chbAddtoMap.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chbAddtoMap.Font = Nothing
        Me.chbAddtoMap.Name = "chbAddtoMap"
        Me.chbAddtoMap.UseVisualStyleBackColor = True
        '
        'txtProgress
        '
        Me.txtProgress.AccessibleDescription = Nothing
        Me.txtProgress.AccessibleName = Nothing
        resources.ApplyResources(Me.txtProgress, "txtProgress")
        Me.txtProgress.BackgroundImage = Nothing
        Me.txtProgress.Font = Nothing
        Me.txtProgress.Name = "txtProgress"
        '
        'Label5
        '
        Me.Label5.AccessibleDescription = Nothing
        Me.Label5.AccessibleName = Nothing
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Font = Nothing
        Me.Label5.Name = "Label5"
        '
        'frmMergeShapefiles
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Nothing
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtProgress)
        Me.Controls.Add(Me.chbAddtoMap)
        Me.Controls.Add(Me.cmbField)
        Me.Controls.Add(Me.rdOneAttribute)
        Me.Controls.Add(Me.btnBrowseToOut)
        Me.Controls.Add(Me.txtOut)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.rdAttributes)
        Me.Controls.Add(Me.rdGeometry)
        Me.Controls.Add(Me.rdIgnore)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.Label1)
        Me.Font = Nothing
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmMergeShapefiles"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents rdIgnore As System.Windows.Forms.RadioButton
    Friend WithEvents rdGeometry As System.Windows.Forms.RadioButton
    Friend WithEvents rdAttributes As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnBrowseToOut As System.Windows.Forms.Button
    Friend WithEvents txtOut As System.Windows.Forms.TextBox
    Friend WithEvents rdOneAttribute As System.Windows.Forms.RadioButton
    Friend WithEvents cmbField As System.Windows.Forms.ComboBox
    Friend WithEvents chbAddtoMap As System.Windows.Forms.CheckBox
    Friend WithEvents txtProgress As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
End Class
