<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSelectImages
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectImages))
        Me.btnMoveDown = New System.Windows.Forms.Button
        Me.btnMoveUp = New System.Windows.Forms.Button
        Me.btnRemove = New System.Windows.Forms.Button
        Me.btnAdd = New System.Windows.Forms.Button
        Me.btnNext = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lstImage = New System.Windows.Forms.ListView
        Me.Filename = New System.Windows.Forms.ColumnHeader
        Me.FileSize = New System.Windows.Forms.ColumnHeader
        Me.FileType = New System.Windows.Forms.ColumnHeader
        Me.ImageSizeX = New System.Windows.Forms.ColumnHeader
        Me.ImageSizeY = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'btnMoveDown
        '
        Me.btnMoveDown.AccessibleDescription = Nothing
        Me.btnMoveDown.AccessibleName = Nothing
        resources.ApplyResources(Me.btnMoveDown, "btnMoveDown")
        Me.btnMoveDown.BackgroundImage = Nothing
        Me.btnMoveDown.Font = Nothing
        Me.btnMoveDown.Name = "btnMoveDown"
        Me.btnMoveDown.TabStop = False
        '
        'btnMoveUp
        '
        Me.btnMoveUp.AccessibleDescription = Nothing
        Me.btnMoveUp.AccessibleName = Nothing
        resources.ApplyResources(Me.btnMoveUp, "btnMoveUp")
        Me.btnMoveUp.BackgroundImage = Nothing
        Me.btnMoveUp.Font = Nothing
        Me.btnMoveUp.Name = "btnMoveUp"
        Me.btnMoveUp.TabStop = False
        '
        'btnRemove
        '
        Me.btnRemove.AccessibleDescription = Nothing
        Me.btnRemove.AccessibleName = Nothing
        resources.ApplyResources(Me.btnRemove, "btnRemove")
        Me.btnRemove.BackgroundImage = Nothing
        Me.btnRemove.Font = Nothing
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.TabStop = False
        '
        'btnAdd
        '
        Me.btnAdd.AccessibleDescription = Nothing
        Me.btnAdd.AccessibleName = Nothing
        resources.ApplyResources(Me.btnAdd, "btnAdd")
        Me.btnAdd.BackgroundImage = Nothing
        Me.btnAdd.Font = Nothing
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.TabStop = False
        '
        'btnNext
        '
        Me.btnNext.AccessibleDescription = Nothing
        Me.btnNext.AccessibleName = Nothing
        resources.ApplyResources(Me.btnNext, "btnNext")
        Me.btnNext.BackgroundImage = Nothing
        Me.btnNext.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnNext.Font = Nothing
        Me.btnNext.Name = "btnNext"
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleDescription = Nothing
        Me.btnCancel.AccessibleName = Nothing
        resources.ApplyResources(Me.btnCancel, "btnCancel")
        Me.btnCancel.BackgroundImage = Nothing
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = Nothing
        Me.btnCancel.Name = "btnCancel"
        '
        'lstImage
        '
        Me.lstImage.AccessibleDescription = Nothing
        Me.lstImage.AccessibleName = Nothing
        resources.ApplyResources(Me.lstImage, "lstImage")
        Me.lstImage.BackgroundImage = Nothing
        Me.lstImage.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Filename, Me.FileSize, Me.FileType, Me.ImageSizeX, Me.ImageSizeY})
        Me.lstImage.Font = Nothing
        Me.lstImage.FullRowSelect = True
        Me.lstImage.GridLines = True
        Me.lstImage.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lstImage.HideSelection = False
        Me.lstImage.Name = "lstImage"
        Me.lstImage.TabStop = False
        Me.lstImage.UseCompatibleStateImageBehavior = False
        Me.lstImage.View = System.Windows.Forms.View.Details
        '
        'Filename
        '
        resources.ApplyResources(Me.Filename, "Filename")
        '
        'FileSize
        '
        resources.ApplyResources(Me.FileSize, "FileSize")
        '
        'FileType
        '
        resources.ApplyResources(Me.FileType, "FileType")
        '
        'ImageSizeX
        '
        resources.ApplyResources(Me.ImageSizeX, "ImageSizeX")
        '
        'ImageSizeY
        '
        resources.ApplyResources(Me.ImageSizeY, "ImageSizeY")
        '
        'frmSelectImages
        '
        Me.AcceptButton = Me.btnNext
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.btnCancel
        Me.Controls.Add(Me.lstImage)
        Me.Controls.Add(Me.btnMoveDown)
        Me.Controls.Add(Me.btnMoveUp)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnCancel)
        Me.Font = Nothing
        Me.Icon = Nothing
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSelectImages"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnMoveDown As System.Windows.Forms.Button
    Friend WithEvents btnMoveUp As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lstImage As System.Windows.Forms.ListView
    Friend WithEvents Filename As System.Windows.Forms.ColumnHeader
    Friend WithEvents FileSize As System.Windows.Forms.ColumnHeader
    Friend WithEvents FileType As System.Windows.Forms.ColumnHeader
    Friend WithEvents ImageSizeX As System.Windows.Forms.ColumnHeader
    Friend WithEvents ImageSizeY As System.Windows.Forms.ColumnHeader
End Class
