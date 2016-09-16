Imports System
Imports System.Drawing
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Globalization
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports MapWinUtility

Public Class frmSelectGrids
    Inherits System.Windows.Forms.Form

    Private hasActivated As Boolean = False

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

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
    Friend WithEvents Filename As System.Windows.Forms.ColumnHeader
    Friend WithEvents FileSize As System.Windows.Forms.ColumnHeader
    Friend WithEvents FileType As System.Windows.Forms.ColumnHeader
    Friend WithEvents CellSizeX As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnMoveUp As System.Windows.Forms.Button
    Friend WithEvents btnMoveDown As System.Windows.Forms.Button
    Friend WithEvents lstGrids As System.Windows.Forms.ListView
    Friend WithEvents CellSizeY As System.Windows.Forms.ColumnHeader
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSelectGrids))
        Me.lstGrids = New System.Windows.Forms.ListView
        Me.Filename = New System.Windows.Forms.ColumnHeader
        Me.FileSize = New System.Windows.Forms.ColumnHeader
        Me.FileType = New System.Windows.Forms.ColumnHeader
        Me.CellSizeX = New System.Windows.Forms.ColumnHeader
        Me.CellSizeY = New System.Windows.Forms.ColumnHeader
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnNext = New System.Windows.Forms.Button
        Me.btnAdd = New System.Windows.Forms.Button
        Me.btnRemove = New System.Windows.Forms.Button
        Me.btnMoveUp = New System.Windows.Forms.Button
        Me.btnMoveDown = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'lstGrids
        '
        Me.lstGrids.AccessibleDescription = Nothing
        Me.lstGrids.AccessibleName = Nothing
        resources.ApplyResources(Me.lstGrids, "lstGrids")
        Me.lstGrids.BackgroundImage = Nothing
        Me.lstGrids.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Filename, Me.FileSize, Me.FileType, Me.CellSizeX, Me.CellSizeY})
        Me.lstGrids.Font = Nothing
        Me.lstGrids.FullRowSelect = True
        Me.lstGrids.GridLines = True
        Me.lstGrids.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lstGrids.HideSelection = False
        Me.lstGrids.Name = "lstGrids"
        Me.lstGrids.TabStop = False
        Me.ToolTip1.SetToolTip(Me.lstGrids, resources.GetString("lstGrids.ToolTip"))
        Me.lstGrids.UseCompatibleStateImageBehavior = False
        Me.lstGrids.View = System.Windows.Forms.View.Details
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
        'CellSizeX
        '
        resources.ApplyResources(Me.CellSizeX, "CellSizeX")
        '
        'CellSizeY
        '
        resources.ApplyResources(Me.CellSizeY, "CellSizeY")
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
        Me.ToolTip1.SetToolTip(Me.btnCancel, resources.GetString("btnCancel.ToolTip"))
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
        Me.ToolTip1.SetToolTip(Me.btnNext, resources.GetString("btnNext.ToolTip"))
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
        Me.ToolTip1.SetToolTip(Me.btnAdd, resources.GetString("btnAdd.ToolTip"))
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
        Me.ToolTip1.SetToolTip(Me.btnRemove, resources.GetString("btnRemove.ToolTip"))
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
        Me.ToolTip1.SetToolTip(Me.btnMoveUp, resources.GetString("btnMoveUp.ToolTip"))
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
        Me.ToolTip1.SetToolTip(Me.btnMoveDown, resources.GetString("btnMoveDown.ToolTip"))
        '
        'frmSelectGrids
        '
        Me.AcceptButton = Me.btnNext
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.btnCancel
        Me.Controls.Add(Me.btnMoveDown)
        Me.Controls.Add(Me.btnMoveUp)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.lstGrids)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnCancel)
        Me.Font = Nothing
        Me.HelpButton = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSelectGrids"
        Me.ToolTip1.SetToolTip(Me, resources.GetString("$this.ToolTip"))
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        ' Build dialog string of all supported grid types
        ' Show dialog box (allowing multiple selection)
        Dim dlg As New Windows.Forms.OpenFileDialog
        Dim grd As New SuperGrid  'MapWinGIupergrid.Grid()

        If g_Grids Is Nothing Then g_Grids = New ArrayList

        dlg.CheckFileExists = True
        dlg.CheckPathExists = True
        dlg.Multiselect = True

        dlg.Filter = BuildFilter(grd.CdlgFilter)

        dlg.Title = "Open grid files"
        dlg.ValidateNames = True
        If dlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim i As Integer
            'Dim prg As New frmProgress
            Dim failedGrids As String = ""
            Dim curItem As Windows.Forms.ListViewItem

            ' Create some kind of progress indication.
            'prg.NumSteps = dlg.FileNames.Length
            'prg.Owner = Me
            'prg.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
            'prg.Show()

            ' Add each individual selected grid to the list box, showing attributes
            For i = 0 To dlg.FileNames.Length - 1
                Dim ext As String = LCase(dlg.FileNames(i).Substring(dlg.FileNames(i).LastIndexOf(".") + 1))
                Dim openOK As Boolean = False

                Me.Refresh()
                'prg.CurrentStep = i
                'prg.Taskname = "Opening Grid(s)..."
                'prg.Filename = dlg.FileNames(i)
                Logger.Status("Opening Grid " & dlg.FileNames(i))

                Select Case ext
                    Case "asc"
                        openOK = grd.Open(dlg.FileNames(i), , True) ', , prg)
                    Case "bgd", "ddf", "dem", "flt"
                        openOK = grd.Open(dlg.FileNames(i), , False) ', , prg)
                    Case "adf"
                        openOK = grd.Open(dlg.FileNames(i), , False) ', , prg)
                    Case "grd"
                        'Open a surfer grid through the supergrid
                        openOK = grd.Open(dlg.FileNames(i), , False) ', , prg)
                    Case Else
                        ' check to see if it is an esri grid with a . in the folder name somewhere
                        If Dir(dlg.FileNames(i), FileAttribute.Directory) <> "" Then
                            openOK = grd.Open(dlg.FileNames(i), , False) ' , , prg)
                        End If
                End Select
                Logger.Status("Opened Grid, Status " & openOK)

                If openOK Then
                    curItem = lstGrids.Items.Add(dlg.FileNames(i))
                    Dim s As String = Format(Math.Round(FileSystem.FileLen(dlg.FileNames(i)) / 1024), "###,###,###,###")
                    If Not s.Trim() = "" Then curItem.SubItems.Add(s)
                    Select Case grd.DataType()
                        Case MapWinGIS.GridDataType.DoubleDataType
                            If s.Trim() = "" Then curItem.SubItems.Add(Format(Math.Round((8 * grd.Header.NumberCols * grd.Header.NumberRows) / 1024), "###,###,###,###"))
                            curItem.SubItems.Add("Double")
                        Case MapWinGIS.GridDataType.FloatDataType
                            If s.Trim() = "" Then curItem.SubItems.Add(Format(Math.Round((4 * grd.Header.NumberCols * grd.Header.NumberRows) / 1024), "###,###,###,###"))
                            curItem.SubItems.Add("Float")
                        Case MapWinGIS.GridDataType.LongDataType
                            If s.Trim() = "" Then curItem.SubItems.Add(Format(Math.Round((4 * grd.Header.NumberCols * grd.Header.NumberRows) / 1024), "###,###,###,###"))
                            curItem.SubItems.Add("Long Integer")
                        Case MapWinGIS.GridDataType.ShortDataType
                            If s.Trim() = "" Then curItem.SubItems.Add(Format(Math.Round((2 * grd.Header.NumberCols * grd.Header.NumberRows) / 1024), "###,###,###,###"))
                            curItem.SubItems.Add("Short Integer")
                        Case MapWinGIS.GridDataType.UnknownDataType
                            If s.Trim() = "" Then curItem.SubItems.Add(Format(Math.Round((1 * grd.Header.NumberCols * grd.Header.NumberRows) / 1024), "###,###,###,###"))
                            curItem.SubItems.Add("[Unknown]")
                        Case MapWinGIS.GridDataType.InvalidDataType
                            If s.Trim() = "" Then curItem.SubItems.Add(Format(Math.Round((1 * grd.Header.NumberCols * grd.Header.NumberRows) / 1024), "###,###,###,###"))
                            curItem.SubItems.Add("[Invalid]")
                    End Select
                    curItem.SubItems.Add(grd.Header.dX.ToString)
                    curItem.SubItems.Add(grd.Header.dY.ToString)
                Else
                    failedGrids &= vbCrLf & dlg.FileName
                End If
                g_Grids.Add(CType(grd, MapWinGIS.Grid))
                grd = New SuperGrid
            Next
            'prg.Close()
            'prg = Nothing

            If failedGrids <> "" Then
                Logger.Msg("The following grids failed to open:" & failedGrids, MsgBoxStyle.OkOnly, "Select Grids Problem")
            End If
        End If
        UpdatePreview()
        DoEnables()
    End Sub

    Private Function BuildFilter(ByRef grdfilter As String) As String
        'build the new common dialog filter from what is available
        Dim vArr() As String, allNames As New ArrayList, allVals As New ArrayList
        Dim i As Integer

        vArr = Split(grdfilter, "|")

        On Error Resume Next
        For i = 0 To UBound(vArr) Step 2
            If LCase(vArr(i).Substring(0, Len("all supported"))) <> "all supported" Then
                allNames.Add(vArr(i)) ' value
                allVals.Add(vArr(i + 1)) ' key
            End If
        Next i

        If Not allVals.Contains("*.flt") And Not allNames.Contains("USGS NED Grid Float (*.flt)") Then
            allNames.Add("USGS NED Grid Float (*.flt)")
            allVals.Add("*.flt")
        End If

        If Not allVals.Contains("*.dem") And Not allNames.Contains("USGS DEM (*.dem)") Then
            allNames.Add("USGS DEM (*.dem)")
            allVals.Add("*.dem")
        End If

        If Not allVals.Contains("*.grd") And Not allNames.Contains("Surfer 7 Grid (*.grd)") Then
            allNames.Add("Surfer 7 Grid (*.grd)")
            allVals.Add("*.grd")
        End If

        Dim keys() As String
        keys = allVals.ToArray(GetType(String))

        Dim allExtensions As String = "", allTypes As String = ""

        For i = 0 To UBound(keys)
            If Len(allExtensions) = 0 Then
                If keys(i).Substring(keys(i).Length - 2) = ";" Then
                    allExtensions = Trim(keys(i).Substring(0, Len(keys(i)) - 1))
                Else
                    allExtensions = Trim(keys(i))
                End If
            Else
                If keys(i).Substring(CStr(keys(i)).Length - 2) = ";" Then
                    allExtensions &= ";" & Trim(keys(i).Substring(0, Len(keys(i)) - 1))
                Else
                    allExtensions &= ";" & Trim(keys(i))
                End If
            End If

            If Len(allTypes) = 0 Then
                allTypes = allNames(allVals.IndexOf(keys(i))) & "|" & Trim(keys(i))
            Else
                allTypes &= "|" & Trim(allNames(allVals.IndexOf(keys(i)))) & "|" & Trim(keys(i))
            End If
        Next i

        Return "All supported formats|" & allExtensions & "|" & allTypes
    End Function

    Private Sub UpdatePreview()
        ' Draw the preview.
        ' TODO:  Make the aspect ratios correct
        Dim minX, minY, maxX, maxY As Double
        Dim g As MapWinGIS.Grid
        Dim t As Double
        Dim firstGo As Boolean = True

        Try
            ' Find the maximum extents
            For Each g In g_Grids
                If firstGo Then
                    minX = g.Header.XllCenter - (g.Header.dX / 2)
                    minY = g.Header.YllCenter - (g.Header.dY / 2)
                    maxX = minX + g.Header.NumberCols * g.Header.dX
                    maxY = minY + g.Header.NumberRows * g.Header.dY
                    firstGo = False
                End If
                t = g.Header.XllCenter - (g.Header.dX / 2)
                If t < minX Then
                    minX = t
                ElseIf t > maxX Then
                    maxX = t
                End If
                t = t + g.Header.NumberCols * g.Header.dX
                If t < minX Then
                    minX = t
                ElseIf t > maxX Then
                    maxX = t
                End If
                t = g.Header.YllCenter - (g.Header.dY / 2)
                If t < minY Then
                    minY = t
                ElseIf t > maxY Then
                    maxY = t
                End If
                t = t + g.Header.NumberRows * g.Header.dY
                If t < minY Then
                    minY = t
                ElseIf t > maxY Then
                    maxY = t
                End If
            Next

            ' Pad the extents
            Dim pad As Double = (maxX - minX) * 0.15 * 0.5
            maxX += pad
            minX -= pad
            pad = (maxY - minY) * 0.15 * 0.5
            maxY += pad
            minY -= pad

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        ' Find which grid(s) is/are selected
        Dim curItem As Windows.Forms.ListViewItem
        Dim i As Integer, grd As MapWinGIS.Grid

        If lstGrids.SelectedItems.Count <= 0 Then
            DoEnables()
            Exit Sub
        End If

        ' Remoe the selected grid(s) from the list
        For Each curItem In lstGrids.SelectedItems
            For i = 0 To g_Grids.Count - 1
                grd = g_Grids(i)
                If grd.Filename = curItem.Text Then
                    g_Grids.RemoveAt(i)
                    Exit For
                End If
            Next
            lstGrids.Items.Remove(curItem)
        Next

        UpdatePreview()
        DoEnables()
    End Sub

    Private Sub btnMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveUp.Click
        ' Move the selected grid up
        Try
            If lstGrids.Items.Count > 0 AndAlso lstGrids.SelectedItems.Count = 1 Then
                Dim selItem As Windows.Forms.ListViewItem = lstGrids.SelectedItems(0)
                Dim oldIndex As Integer = selItem.Index
                lstGrids.Items.Remove(selItem)
                lstGrids.Items.Insert(oldIndex - 1, selItem).Selected = True
            End If
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
        DoEnables()
        UpdatePreview()
        lstGrids.Focus()
    End Sub

    Private Sub btnMoveDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveDown.Click
        ' Move the selected grid down
        Try
            If lstGrids.Items.Count > 0 AndAlso lstGrids.SelectedItems.Count = 1 Then
                Dim selItem As Windows.Forms.ListViewItem = lstGrids.SelectedItems(0)
                Dim oldIndex As Integer = selItem.Index
                lstGrids.Items.Remove(selItem)
                lstGrids.Items.Insert(oldIndex + 1, selItem).Selected = True
            End If
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
        DoEnables()
        UpdatePreview()
        lstGrids.Focus()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Logger.Dbg("User Cancelled")
        Cleanup()

        'Must set dialog result or GISTools routines will hang.
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        If lstGrids.Items.Count < 1 Then
            Logger.Msg("Please add at least one grid using the Plus icon before proceeding. Click Cancel if you'd like to abort.", MsgBoxStyle.Information, "Select a Grid First")
            Exit Sub
        End If

        ' calculate the number of cells that will be in the new grid.  If this gets too big, then warn the user.
        Dim l, r, t, b, minCellSize As Double
        Dim grd As MapWinGIS.Grid, first As Boolean = True
        For Each grd In g_Grids
            If first Then
                minCellSize = Math.Min(grd.Header.dX, grd.Header.dY)
                l = grd.Header.XllCenter - grd.Header.dX / 2
                b = grd.Header.YllCenter - grd.Header.dY / 2
                r = l + grd.Header.dX * grd.Header.NumberCols
                t = b + grd.Header.dY * grd.Header.NumberRows
                first = False
            Else
                minCellSize = Math.Min(minCellSize, Math.Min(grd.Header.dX, grd.Header.dY))
                l = Math.Min(l, grd.Header.XllCenter - grd.Header.dX / 2)
                b = Math.Min(b, grd.Header.YllCenter - grd.Header.dY / 2)
                r = Math.Max(r, (grd.Header.XllCenter - grd.Header.dX / 2) + grd.Header.dX * grd.Header.NumberCols)
                t = Math.Max(t, (grd.Header.YllCenter - grd.Header.dY / 2) + grd.Header.dY * grd.Header.NumberRows)
            End If
        Next
        Dim numCells As Long = (Math.Abs(r - l) / minCellSize) * (Math.Abs(t - b) / minCellSize)
        Dim objWMI As New clsWMI
        Dim maxMemory As Double = objWMI.TotalPhysicalMemory
        objWMI = Nothing
        If numCells * 8 > maxMemory Then ' I'm using the value of 8 bytes/cell to be an average worst-case size for all grid types.
            If Logger.Msg("WARNING!  The new grid will probably have " & Format(numCells, "#,###") & " cells (with a cell size of " & minCellSize & ") which could create a file of " & Format(Math.Round(numCells * 8) / (2 ^ 20), "#,###") & " MB (or more) which is larger than your physical memory of " & Format(Math.Round(maxMemory / (2 ^ 20)), "#,###") & " MB.  This may cause problems due to insufficient memory.  Do you wish to continue?", MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation, "Grid Wizard") = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        'Must set dialog result or GISTools routines will hang.
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub DoEnables()
        If lstGrids.Items.Count > 0 AndAlso lstGrids.SelectedItems.Count = 1 Then
            If lstGrids.SelectedItems(0).Index = lstGrids.Items.Count - 1 Then
                ' can't move it down because it is the last item
                btnMoveDown.Enabled = False
            Else
                btnMoveDown.Enabled = True
            End If
            If lstGrids.SelectedItems(0).Index = 0 Then
                ' can't move it up because it is the first item
                btnMoveUp.Enabled = False
            Else
                btnMoveUp.Enabled = True
            End If
        Else
            btnMoveDown.Enabled = False
            btnMoveUp.Enabled = False
        End If
    End Sub

    Private Sub chkResample_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DoEnables()
    End Sub

    Private Sub chkMerge_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DoEnables()
    End Sub

    Private Sub chkCreateImage_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        DoEnables()
    End Sub

    Private Sub lstGrids_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstGrids.Click
        DoEnables()
    End Sub

    Private Sub grpLayout_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs)
        UpdatePreview()
    End Sub

    Private Sub cmdScheme_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'TODO: Fix this...
        'Dim DialogProvider As IWindowsFormsEditorService ' This object will start the dialog form
        'DialogProvider = CType(provider.GetService(GetType(IWindowsFormsEditorService)), Windows.Forms.Design.IWindowsFormsEditorService)
        'Dim dlg As New GridColoringSchemeForm(DialogProvider, g_Scheme)
        'dlg.ShowDialog(g_MapWindowForm)
    End Sub

    Private Sub ImportForm_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        If Not hasActivated Then
            hasActivated = True
            btnAdd_Click(Me, New System.EventArgs)
        End If
    End Sub

End Class
