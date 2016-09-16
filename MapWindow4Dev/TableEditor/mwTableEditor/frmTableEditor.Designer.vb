
Partial Public Class frmTableEditor
    Inherits System.Windows.Forms.Form

    Friend WithEvents MenuItem7 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem8 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem9 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuShapeID As System.Windows.Forms.MenuItem
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents MenuItem10 As System.Windows.Forms.MenuItem
    'gb_selected_column is a blobal variable providing access to the selected column in the table
    Public gb_selected_column As Integer
    Friend WithEvents MenuItem14 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem11 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem12 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem15 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem16 As System.Windows.Forms.MenuItem
    Friend WithEvents Panel1 As System.Windows.Forms.Panel

    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.Button
    Friend WithEvents btnZoomToSelected As System.Windows.Forms.Button
    Friend WithEvents btnShowSelected As System.Windows.Forms.Button
    Friend WithEvents btnImportFieldsFromDBF As System.Windows.Forms.Button
    Friend WithEvents btnFieldCalculator As System.Windows.Forms.Button
    Friend WithEvents tbbQuery As System.Windows.Forms.Button
    Friend WithEvents btnFieldSep As System.Windows.Forms.Button
    Friend WithEvents tbbRefresh As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Public WithEvents TableEditorDataGrid As System.Windows.Forms.DataGrid
    Friend WithEvents TableEditorHelp As System.Windows.Forms.HelpProvider
    Friend WithEvents lblUpdating As System.Windows.Forms.Label
    Friend WithEvents UpdateProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Selected_Label As System.Windows.Forms.Label
    Friend WithEvents Column_context As System.Windows.Forms.ContextMenu
    Friend WithEvents sort_asc As System.Windows.Forms.MenuItem
    Friend WithEvents Sort_desc As System.Windows.Forms.MenuItem
    Friend WithEvents Statistics_popup As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents Column_Expression_popup As System.Windows.Forms.MenuItem
    Friend WithEvents Selected_Input_popup As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem5 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem6 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem13 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem19 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem21 As System.Windows.Forms.MenuItem
    Friend WithEvents mnuRemoveField As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAddField As System.Windows.Forms.MenuItem
    Friend WithEvents mnuRenameField As System.Windows.Forms.MenuItem

    Friend WithEvents mnuShowSelected As System.Windows.Forms.MenuItem
    Friend WithEvents mnuZoomToSelected As System.Windows.Forms.MenuItem
    Friend WithEvents mnuQuery As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSelectAll As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSelectNone As System.Windows.Forms.MenuItem
    Friend WithEvents mnuSwitchSelection As System.Windows.Forms.MenuItem
    Friend WithEvents mnuExport As System.Windows.Forms.MenuItem
    Friend WithEvents mnuFind As System.Windows.Forms.MenuItem
    Friend WithEvents mnuReplace As System.Windows.Forms.MenuItem

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTableEditor))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Selected_Label = New System.Windows.Forms.Label
        Me.btnZoomToSelected = New System.Windows.Forms.Button
        Me.UpdateProgress = New System.Windows.Forms.ProgressBar
        Me.lblUpdating = New System.Windows.Forms.Label
        Me.btnShowSelected = New System.Windows.Forms.Button
        Me.btnImportFieldsFromDBF = New System.Windows.Forms.Button
        Me.btnFieldCalculator = New System.Windows.Forms.Button
        Me.tbbRefresh = New System.Windows.Forms.Button
        Me.tbbQuery = New System.Windows.Forms.Button
        Me.btnFieldSep = New System.Windows.Forms.Button
        Me.btnApply = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        Me.TableEditorDataGrid = New System.Windows.Forms.DataGrid
        Me.TableEditorHelp = New System.Windows.Forms.HelpProvider
        Me.Column_context = New System.Windows.Forms.ContextMenu
        Me.Column_Expression_popup = New System.Windows.Forms.MenuItem
        Me.Selected_Input_popup = New System.Windows.Forms.MenuItem
        Me.MenuItem14 = New System.Windows.Forms.MenuItem
        Me.MenuItem11 = New System.Windows.Forms.MenuItem
        Me.MenuItem12 = New System.Windows.Forms.MenuItem
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.sort_asc = New System.Windows.Forms.MenuItem
        Me.Sort_desc = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.Statistics_popup = New System.Windows.Forms.MenuItem
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.mnuAddField = New System.Windows.Forms.MenuItem
        Me.mnuRemoveField = New System.Windows.Forms.MenuItem
        Me.MenuItem21 = New System.Windows.Forms.MenuItem
        Me.mnuRenameField = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.mnuShowSelected = New System.Windows.Forms.MenuItem
        Me.mnuZoomToSelected = New System.Windows.Forms.MenuItem
        Me.MenuItem15 = New System.Windows.Forms.MenuItem
        Me.MenuItem16 = New System.Windows.Forms.MenuItem
        Me.MenuItem5 = New System.Windows.Forms.MenuItem
        Me.mnuQuery = New System.Windows.Forms.MenuItem
        Me.MenuItem13 = New System.Windows.Forms.MenuItem
        Me.mnuSelectAll = New System.Windows.Forms.MenuItem
        Me.mnuSelectNone = New System.Windows.Forms.MenuItem
        Me.mnuSwitchSelection = New System.Windows.Forms.MenuItem
        Me.MenuItem19 = New System.Windows.Forms.MenuItem
        Me.mnuExport = New System.Windows.Forms.MenuItem
        Me.MenuItem6 = New System.Windows.Forms.MenuItem
        Me.mnuFind = New System.Windows.Forms.MenuItem
        Me.mnuReplace = New System.Windows.Forms.MenuItem
        Me.MenuItem9 = New System.Windows.Forms.MenuItem
        Me.MenuItem7 = New System.Windows.Forms.MenuItem
        Me.MenuItem8 = New System.Windows.Forms.MenuItem
        Me.mnuShapeID = New System.Windows.Forms.MenuItem
        Me.MenuItem10 = New System.Windows.Forms.MenuItem
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.mnuResources = New System.Windows.Forms.MenuItem
        Me.mnuShowAll = New System.Windows.Forms.MenuItem
        Me.EnterTheNameOfTheTargetField = New System.Windows.Forms.MenuItem
        Me.DataWillBeOverwritten = New System.Windows.Forms.MenuItem
        Me.AssignFeatureIDSToAField = New System.Windows.Forms.MenuItem
        Me.msgSave = New System.Windows.Forms.MenuItem
        Me.qSave = New System.Windows.Forms.MenuItem
        Me.msgNoCollumn = New System.Windows.Forms.MenuItem
        Me.msgError = New System.Windows.Forms.MenuItem
        Me.DataGridTableStyle1 = New System.Windows.Forms.DataGridTableStyle
        Me.Panel1.SuspendLayout()
        CType(Me.TableEditorDataGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Selected_Label)
        Me.Panel1.Controls.Add(Me.btnZoomToSelected)
        Me.Panel1.Controls.Add(Me.UpdateProgress)
        Me.Panel1.Controls.Add(Me.lblUpdating)
        Me.Panel1.Controls.Add(Me.btnShowSelected)
        Me.Panel1.Controls.Add(Me.btnImportFieldsFromDBF)
        Me.Panel1.Controls.Add(Me.btnFieldCalculator)
        Me.Panel1.Controls.Add(Me.tbbRefresh)
        Me.Panel1.Controls.Add(Me.tbbQuery)
        Me.Panel1.Controls.Add(Me.btnFieldSep)
        Me.Panel1.Controls.Add(Me.btnApply)
        Me.Panel1.Controls.Add(Me.btnClose)
        resources.ApplyResources(Me.Panel1, "Panel1")
        Me.Panel1.Name = "Panel1"
        '
        'Selected_Label
        '
        resources.ApplyResources(Me.Selected_Label, "Selected_Label")
        Me.Selected_Label.Name = "Selected_Label"
        Me.TableEditorHelp.SetShowHelp(Me.Selected_Label, CType(resources.GetObject("Selected_Label.ShowHelp"), Boolean))
        '
        'btnZoomToSelected
        '
        resources.ApplyResources(Me.btnZoomToSelected, "btnZoomToSelected")
        Me.btnZoomToSelected.Name = "btnZoomToSelected"
        Me.ToolTip1.SetToolTip(Me.btnZoomToSelected, resources.GetString("btnZoomToSelected.ToolTip"))
        '
        'UpdateProgress
        '
        resources.ApplyResources(Me.UpdateProgress, "UpdateProgress")
        Me.UpdateProgress.Name = "UpdateProgress"
        Me.TableEditorHelp.SetShowHelp(Me.UpdateProgress, CType(resources.GetObject("UpdateProgress.ShowHelp"), Boolean))
        '
        'lblUpdating
        '
        resources.ApplyResources(Me.lblUpdating, "lblUpdating")
        Me.lblUpdating.Name = "lblUpdating"
        Me.TableEditorHelp.SetShowHelp(Me.lblUpdating, CType(resources.GetObject("lblUpdating.ShowHelp"), Boolean))
        '
        'btnShowSelected
        '
        resources.ApplyResources(Me.btnShowSelected, "btnShowSelected")
        Me.btnShowSelected.Name = "btnShowSelected"
        Me.ToolTip1.SetToolTip(Me.btnShowSelected, resources.GetString("btnShowSelected.ToolTip"))
        '
        'btnImportFieldsFromDBF
        '
        resources.ApplyResources(Me.btnImportFieldsFromDBF, "btnImportFieldsFromDBF")
        Me.btnImportFieldsFromDBF.Name = "btnImportFieldsFromDBF"
        Me.ToolTip1.SetToolTip(Me.btnImportFieldsFromDBF, resources.GetString("btnImportFieldsFromDBF.ToolTip"))
        '
        'btnFieldCalculator
        '
        resources.ApplyResources(Me.btnFieldCalculator, "btnFieldCalculator")
        Me.btnFieldCalculator.Name = "btnFieldCalculator"
        Me.ToolTip1.SetToolTip(Me.btnFieldCalculator, resources.GetString("btnFieldCalculator.ToolTip"))
        '
        'tbbRefresh
        '
        resources.ApplyResources(Me.tbbRefresh, "tbbRefresh")
        Me.tbbRefresh.Name = "tbbRefresh"
        Me.ToolTip1.SetToolTip(Me.tbbRefresh, resources.GetString("tbbRefresh.ToolTip"))
        '
        'tbbQuery
        '
        Me.tbbQuery.Image = Global.mwTableEditor.Resource.filter
        resources.ApplyResources(Me.tbbQuery, "tbbQuery")
        Me.tbbQuery.Name = "tbbQuery"
        Me.ToolTip1.SetToolTip(Me.tbbQuery, resources.GetString("tbbQuery.ToolTip"))
        '
        'btnFieldSep
        '
        resources.ApplyResources(Me.btnFieldSep, "btnFieldSep")
        Me.btnFieldSep.Name = "btnFieldSep"
        Me.ToolTip1.SetToolTip(Me.btnFieldSep, resources.GetString("btnFieldSep.ToolTip"))
        '
        'btnApply
        '
        resources.ApplyResources(Me.btnApply, "btnApply")
        Me.btnApply.Name = "btnApply"
        Me.TableEditorHelp.SetShowHelp(Me.btnApply, CType(resources.GetObject("btnApply.ShowHelp"), Boolean))
        '
        'btnClose
        '
        resources.ApplyResources(Me.btnClose, "btnClose")
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Name = "btnClose"
        Me.TableEditorHelp.SetShowHelp(Me.btnClose, CType(resources.GetObject("btnClose.ShowHelp"), Boolean))
        '
        'TableEditorDataGrid
        '
        Me.TableEditorDataGrid.AllowSorting = False
        Me.TableEditorDataGrid.BackgroundColor = System.Drawing.Color.LightGray
        Me.TableEditorDataGrid.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TableEditorDataGrid.CaptionBackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.TableEditorDataGrid, "TableEditorDataGrid")
        Me.TableEditorDataGrid.CaptionVisible = False
        Me.TableEditorDataGrid.DataMember = ""
        Me.TableEditorDataGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.TableEditorDataGrid.Name = "TableEditorDataGrid"
        Me.TableEditorDataGrid.SelectionBackColor = System.Drawing.SystemColors.ActiveCaption
        Me.TableEditorHelp.SetShowHelp(Me.TableEditorDataGrid, CType(resources.GetObject("TableEditorDataGrid.ShowHelp"), Boolean))
        Me.TableEditorDataGrid.TableStyles.AddRange(New System.Windows.Forms.DataGridTableStyle() {Me.DataGridTableStyle1})
        '
        'Column_context
        '
        Me.Column_context.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.Column_Expression_popup, Me.Selected_Input_popup, Me.MenuItem14, Me.MenuItem11, Me.MenuItem12, Me.MenuItem1, Me.sort_asc, Me.Sort_desc, Me.MenuItem2, Me.Statistics_popup})
        '
        'Column_Expression_popup
        '
        Me.Column_Expression_popup.Index = 0
        resources.ApplyResources(Me.Column_Expression_popup, "Column_Expression_popup")
        '
        'Selected_Input_popup
        '
        Me.Selected_Input_popup.Index = 1
        resources.ApplyResources(Me.Selected_Input_popup, "Selected_Input_popup")
        '
        'MenuItem14
        '
        Me.MenuItem14.Index = 2
        resources.ApplyResources(Me.MenuItem14, "MenuItem14")
        '
        'MenuItem11
        '
        Me.MenuItem11.Index = 3
        resources.ApplyResources(Me.MenuItem11, "MenuItem11")
        '
        'MenuItem12
        '
        Me.MenuItem12.Index = 4
        resources.ApplyResources(Me.MenuItem12, "MenuItem12")
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 5
        resources.ApplyResources(Me.MenuItem1, "MenuItem1")
        '
        'sort_asc
        '
        Me.sort_asc.Index = 6
        resources.ApplyResources(Me.sort_asc, "sort_asc")
        '
        'Sort_desc
        '
        Me.Sort_desc.Index = 7
        resources.ApplyResources(Me.Sort_desc, "Sort_desc")
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 8
        resources.ApplyResources(Me.MenuItem2, "MenuItem2")
        '
        'Statistics_popup
        '
        Me.Statistics_popup.Index = 9
        resources.ApplyResources(Me.Statistics_popup, "Statistics_popup")
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.MenuItem3, Me.MenuItem4, Me.MenuItem5, Me.MenuItem6, Me.mnuResources})
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 0
        Me.MenuItem3.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuAddField, Me.mnuRemoveField, Me.MenuItem21, Me.mnuRenameField})
        resources.ApplyResources(Me.MenuItem3, "MenuItem3")
        '
        'mnuAddField
        '
        Me.mnuAddField.Index = 0
        resources.ApplyResources(Me.mnuAddField, "mnuAddField")
        '
        'mnuRemoveField
        '
        Me.mnuRemoveField.Index = 1
        resources.ApplyResources(Me.mnuRemoveField, "mnuRemoveField")
        '
        'MenuItem21
        '
        Me.MenuItem21.Index = 2
        resources.ApplyResources(Me.MenuItem21, "MenuItem21")
        '
        'mnuRenameField
        '
        Me.mnuRenameField.Index = 3
        resources.ApplyResources(Me.mnuRenameField, "mnuRenameField")
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 1
        Me.MenuItem4.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuShowSelected, Me.mnuZoomToSelected, Me.MenuItem15, Me.MenuItem16})
        resources.ApplyResources(Me.MenuItem4, "MenuItem4")
        '
        'mnuShowSelected
        '
        Me.mnuShowSelected.Index = 0
        resources.ApplyResources(Me.mnuShowSelected, "mnuShowSelected")
        '
        'mnuZoomToSelected
        '
        Me.mnuZoomToSelected.Index = 1
        resources.ApplyResources(Me.mnuZoomToSelected, "mnuZoomToSelected")
        '
        'MenuItem15
        '
        Me.MenuItem15.Index = 2
        resources.ApplyResources(Me.MenuItem15, "MenuItem15")
        '
        'MenuItem16
        '
        Me.MenuItem16.Index = 3
        resources.ApplyResources(Me.MenuItem16, "MenuItem16")
        '
        'MenuItem5
        '
        Me.MenuItem5.Index = 2
        Me.MenuItem5.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuQuery, Me.MenuItem13, Me.mnuSelectAll, Me.mnuSelectNone, Me.mnuSwitchSelection, Me.MenuItem19, Me.mnuExport})
        resources.ApplyResources(Me.MenuItem5, "MenuItem5")
        '
        'mnuQuery
        '
        Me.mnuQuery.Index = 0
        resources.ApplyResources(Me.mnuQuery, "mnuQuery")
        '
        'MenuItem13
        '
        Me.MenuItem13.Index = 1
        resources.ApplyResources(Me.MenuItem13, "MenuItem13")
        '
        'mnuSelectAll
        '
        Me.mnuSelectAll.Index = 2
        resources.ApplyResources(Me.mnuSelectAll, "mnuSelectAll")
        '
        'mnuSelectNone
        '
        Me.mnuSelectNone.Index = 3
        resources.ApplyResources(Me.mnuSelectNone, "mnuSelectNone")
        '
        'mnuSwitchSelection
        '
        Me.mnuSwitchSelection.Index = 4
        resources.ApplyResources(Me.mnuSwitchSelection, "mnuSwitchSelection")
        '
        'MenuItem19
        '
        Me.MenuItem19.Index = 5
        resources.ApplyResources(Me.MenuItem19, "MenuItem19")
        '
        'mnuExport
        '
        Me.mnuExport.Index = 6
        resources.ApplyResources(Me.mnuExport, "mnuExport")
        '
        'MenuItem6
        '
        Me.MenuItem6.Index = 3
        Me.MenuItem6.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFind, Me.mnuReplace, Me.MenuItem9, Me.MenuItem7, Me.MenuItem8, Me.mnuShapeID, Me.MenuItem10})
        resources.ApplyResources(Me.MenuItem6, "MenuItem6")
        '
        'mnuFind
        '
        Me.mnuFind.Index = 0
        resources.ApplyResources(Me.mnuFind, "mnuFind")
        '
        'mnuReplace
        '
        Me.mnuReplace.Index = 1
        resources.ApplyResources(Me.mnuReplace, "mnuReplace")
        '
        'MenuItem9
        '
        Me.MenuItem9.Index = 2
        resources.ApplyResources(Me.MenuItem9, "MenuItem9")
        '
        'MenuItem7
        '
        Me.MenuItem7.Index = 3
        resources.ApplyResources(Me.MenuItem7, "MenuItem7")
        '
        'MenuItem8
        '
        Me.MenuItem8.Index = 4
        resources.ApplyResources(Me.MenuItem8, "MenuItem8")
        '
        'mnuShapeID
        '
        Me.mnuShapeID.Index = 5
        resources.ApplyResources(Me.mnuShapeID, "mnuShapeID")
        '
        'MenuItem10
        '
        Me.MenuItem10.Index = 6
        resources.ApplyResources(Me.MenuItem10, "MenuItem10")
        '
        'mnuResources
        '
        Me.mnuResources.Index = 4
        Me.mnuResources.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuShowAll, Me.EnterTheNameOfTheTargetField, Me.DataWillBeOverwritten, Me.AssignFeatureIDSToAField, Me.msgSave, Me.qSave, Me.msgNoCollumn, Me.msgError})
        resources.ApplyResources(Me.mnuResources, "mnuResources")
        '
        'mnuShowAll
        '
        Me.mnuShowAll.Index = 0
        resources.ApplyResources(Me.mnuShowAll, "mnuShowAll")
        '
        'EnterTheNameOfTheTargetField
        '
        Me.EnterTheNameOfTheTargetField.Index = 1
        resources.ApplyResources(Me.EnterTheNameOfTheTargetField, "EnterTheNameOfTheTargetField")
        '
        'DataWillBeOverwritten
        '
        Me.DataWillBeOverwritten.Index = 2
        resources.ApplyResources(Me.DataWillBeOverwritten, "DataWillBeOverwritten")
        '
        'AssignFeatureIDSToAField
        '
        Me.AssignFeatureIDSToAField.Index = 3
        resources.ApplyResources(Me.AssignFeatureIDSToAField, "AssignFeatureIDSToAField")
        '
        'msgSave
        '
        Me.msgSave.Index = 4
        resources.ApplyResources(Me.msgSave, "msgSave")
        '
        'qSave
        '
        Me.qSave.Index = 5
        resources.ApplyResources(Me.qSave, "qSave")
        '
        'msgNoCollumn
        '
        Me.msgNoCollumn.Index = 6
        resources.ApplyResources(Me.msgNoCollumn, "msgNoCollumn")
        '
        'msgError
        '
        Me.msgError.Index = 7
        resources.ApplyResources(Me.msgError, "msgError")
        '
        'DataGridTableStyle1
        '
        Me.DataGridTableStyle1.DataGrid = Me.TableEditorDataGrid
        Me.DataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText
        Me.DataGridTableStyle1.SelectionBackColor = System.Drawing.Color.PaleTurquoise
        '
        'frmTableEditor
        '
        resources.ApplyResources(Me, "$this")
        Me.CancelButton = Me.btnClose
        Me.Controls.Add(Me.TableEditorDataGrid)
        Me.Controls.Add(Me.Panel1)
        Me.DoubleBuffered = True
        Me.KeyPreview = True
        Me.Menu = Me.MainMenu1
        Me.Name = "frmTableEditor"
        Me.TableEditorHelp.SetShowHelp(Me, CType(resources.GetObject("$this.ShowHelp"), Boolean))
        Me.Panel1.ResumeLayout(False)
        CType(Me.TableEditorDataGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents mnuResources As System.Windows.Forms.MenuItem
    Friend WithEvents mnuShowAll As System.Windows.Forms.MenuItem
    Friend WithEvents EnterTheNameOfTheTargetField As System.Windows.Forms.MenuItem
    Friend WithEvents DataWillBeOverwritten As System.Windows.Forms.MenuItem
    Friend WithEvents AssignFeatureIDSToAField As System.Windows.Forms.MenuItem
    Friend WithEvents msgSave As System.Windows.Forms.MenuItem
    Friend WithEvents qSave As System.Windows.Forms.MenuItem
    Friend WithEvents msgNoCollumn As System.Windows.Forms.MenuItem
    Friend WithEvents msgError As System.Windows.Forms.MenuItem
    Friend WithEvents DataGridTableStyle1 As System.Windows.Forms.DataGridTableStyle
End Class

