Public Class frmSelectImages
    Private hasActivated As Boolean = False


    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        ' Build dialog string of all supported grid types
        ' Show dialog box (allowing multiple selection)
        Dim dlg As New Windows.Forms.OpenFileDialog
        'Dim grd As New SuperGrid   'MapWinGIS.Grid()
        Dim img As New MapWinGIS.Image

        If g_Images Is Nothing Then g_Images = New ArrayList

        dlg.CheckFileExists = True
        dlg.CheckPathExists = True
        dlg.Multiselect = True

        dlg.Filter = BuildFilter(img.CdlgFilter)

        dlg.Title = "Open Image files"
        dlg.ValidateNames = True
        If dlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            Dim i As Integer
            Dim prg As New frmProgress
            Dim failedImages As String = vbNullString
            Dim curItem As Windows.Forms.ListViewItem

            ' Create some kind of progress indication.
            prg.NumSteps = dlg.FileNames.Length
            prg.Owner = Me
            prg.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
            prg.Show()

            ' Add each individual selected grid to the list box, showing attributes
            For i = 0 To dlg.FileNames.Length - 1
                Dim ext As String = LCase(dlg.FileNames(i).Substring(dlg.FileNames(i).LastIndexOf(".") + 1))
                Dim openOK As Boolean = False

                Me.Refresh()
                prg.CurrentStep = i
                prg.Taskname = "Opening Image Files(s)..."
                prg.Filename = dlg.FileNames(i)

                openOK = img.Open(dlg.FileNames(i), MapWinGIS.ImageType.USE_FILE_EXTENSION, True, prg)

                If openOK Then
                    curItem = lstImage.Items.Add(dlg.FileNames(i))
                    curItem.SubItems.Add(Format(Math.Round(FileSystem.FileLen(dlg.FileNames(i)) / 1024), "###,###,###,###"))
                    curItem.SubItems.Add(Microsoft.VisualBasic.Right(dlg.FileNames(i), 3))
                    curItem.SubItems.Add(img.Width)
                    curItem.SubItems.Add(img.Height)
                Else
                    failedImages &= vbCrLf & dlg.FileName
                End If
                g_Images.Add(img)
                img = New MapWinGIS.Image
            Next
            prg.Close()
            prg = Nothing

            If Not failedImages = vbNullString Then
                mapwinutility.logger.msg("The following grids failed to open:" & failedImages)
            End If
        End If
        UpdatePreview()
        DoEnables()
    End Sub

    Private Function BuildFilter(ByRef imgfilter As String) As String
        'build the new common dialog filter from what is available
        Dim vArr() As String, allNames As New ArrayList, allVals As New ArrayList
        Dim i As Integer

        vArr = Split(imgfilter, "|")

        On Error Resume Next
        For i = 0 To UBound(vArr) Step 2
            If LCase(vArr(i).Substring(0, Len("all supported"))) <> "all supported" Then
                allNames.Add(vArr(i)) ' value
                allVals.Add(vArr(i + 1)) ' key
            End If
        Next i

        'If Not allVals.Contains("*.flt") And Not allNames.Contains("USGS NED Grid Float (*.flt)") Then
        '    allNames.Add("USGS NED Grid Float (*.flt)")
        '    allVals.Add("*.flt")
        'End If

        'If Not allVals.Contains("*.dem") And Not allNames.Contains("USGS DEM (*.dem)") Then
        '    allNames.Add("USGS DEM (*.dem)")
        '    allVals.Add("*.dem")
        'End If

        'If Not allVals.Contains("*.grd") And Not allNames.Contains("Surfer 7 Grid (*.grd)") Then
        '    allNames.Add("Surfer 7 Grid (*.grd)")
        '    allVals.Add("*.grd")
        'End If

        Dim keys() As String
        keys = allVals.ToArray(GetType(String))

        Dim allExtensions As String = vbNullString, allTypes As String = vbNullString

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

    ' STILL SAME AS GRID, MAY NEED FIXING, not sure what preview is
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
        Dim i As Integer, img As MapWinGIS.Image

        If lstImage.SelectedItems.Count <= 0 Then
            DoEnables()
            Exit Sub
        End If

        ' Remove the selected Image(s) from the list
        For Each curItem In lstImage.SelectedItems
            For i = 0 To g_Images.Count - 1
                img = g_Images(i)
                If img.Filename = curItem.Text Then
                    g_Images.RemoveAt(i)
                    Exit For
                End If
            Next
            lstImage.Items.Remove(curItem)
        Next

        UpdatePreview()
        DoEnables()
    End Sub

    Private Sub btnMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveUp.Click
        ' Move the selected grid up
        Try
            If lstImage.Items.Count > 0 AndAlso lstImage.SelectedItems.Count = 1 Then
                Dim selItem As Windows.Forms.ListViewItem = lstImage.SelectedItems(0)
                Dim oldIndex As Integer = selItem.Index
                lstImage.Items.Remove(selItem)
                lstImage.Items.Insert(oldIndex - 1, selItem).Selected = True
            End If
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
        DoEnables()
        'UpdatePreview()
        lstImage.Focus()
    End Sub

    Private Sub btnMoveDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveDown.Click
        ' Move the selected grid down
        Try
            If lstImage.Items.Count > 0 AndAlso lstImage.SelectedItems.Count = 1 Then
                Dim selItem As Windows.Forms.ListViewItem = lstImage.SelectedItems(0)
                Dim oldIndex As Integer = selItem.Index
                lstImage.Items.Remove(selItem)
                lstImage.Items.Insert(oldIndex + 1, selItem).Selected = True
            End If
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
        DoEnables()
        ' UpdatePreview()
        lstImage.Focus()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Cleanup()
        Me.Close()
    End Sub

    Private Sub btOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        If lstImage.Items.Count < 1 Then
            MapWinUtility.Logger.Msg("Please add at least one image using the Plus icon before proceeding. Click Cancel if you'd like to abort.", MsgBoxStyle.Information, "Select an Image First")
            Exit Sub
        End If

        'Not sure if the following is necessary for images
        ' calculate the number of cells that will be in the new grid.  If this gets too big, then warn the user.
        'Dim l, r, t, b, minCellSize As Double
        'Dim grd As MapWinGIS.Grid, first As Boolean = True
        'For Each grd In g_Grids
        '    If first Then
        '        minCellSize = Math.Min(grd.Header.dX, grd.Header.dY)
        '        l = grd.Header.XllCenter - grd.Header.dX / 2
        '        b = grd.Header.YllCenter - grd.Header.dY / 2
        '        r = l + grd.Header.dX * grd.Header.NumberCols
        '        t = b + grd.Header.dY * grd.Header.NumberRows
        '        first = False
        '    Else
        '        minCellSize = Math.Min(minCellSize, Math.Min(grd.Header.dX, grd.Header.dY))
        '        l = Math.Min(l, grd.Header.XllCenter - grd.Header.dX / 2)
        '        b = Math.Min(b, grd.Header.YllCenter - grd.Header.dY / 2)
        '        r = Math.Max(r, (grd.Header.XllCenter - grd.Header.dX / 2) + grd.Header.dX * grd.Header.NumberCols)
        '        t = Math.Max(t, (grd.Header.YllCenter - grd.Header.dY / 2) + grd.Header.dY * grd.Header.NumberRows)
        '    End If
        'Next
        'Dim numCells As Long = (Math.Abs(r - l) / minCellSize) * (Math.Abs(t - b) / minCellSize)
        'Dim objWMI As New clsWMI
        'Dim maxMemory = objWMI.TotalPhysicalMemory
        'objWMI = Nothing
        'If numCells * 8 > maxMemory Then ' I'm using the value of 8 bytes/cell to be an average worst-case size for all grid types.
        '    If mapwinutility.logger.msg("WARNING!  The new grid will probably have " & Format(numCells, "#,###") & " cells (with a cell size of " & minCellSize & ") which could create a file of " & Format(Math.Round(numCells * 8) / (2 ^ 20), "#,###") & " MB (or more) which is larger than your physical memory of " & Format(Math.Round(maxMemory / (2 ^ 20)), "#,###") & " MB.  This may cause problems due to insufficient memory.  Do you wish to continue?", MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation, "Grid Wizard") = MsgBoxResult.No Then
        '        Exit Sub
        '    End If
        'End If

        Me.Close()
    End Sub

    Private Sub DoEnables()
        If lstImage.Items.Count > 0 AndAlso lstImage.SelectedItems.Count = 1 Then
            If lstImage.SelectedItems(0).Index = lstImage.Items.Count - 1 Then
                ' can't move it down because it is the last item
                btnMoveDown.Enabled = False
            Else
                btnMoveDown.Enabled = True
            End If
            If lstImage.SelectedItems(0).Index = 0 Then
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

    Private Sub lstImage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstImage.Click
        DoEnables()
    End Sub

    Private Sub grpLayout_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs)
        'UpdatePreview()
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