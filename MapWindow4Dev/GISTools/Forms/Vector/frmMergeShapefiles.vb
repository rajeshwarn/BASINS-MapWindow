Public Class frmMergeShapefiles
    Private m_map As MapWindow.Interfaces.IMapWin

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If ListBox1.Items.Count < 2 Then
            mapwinutility.logger.msg("Please choose at least two shapefiles to merge.", MsgBoxStyle.Exclamation, "Choose Two or More Shapefiles")
            Return
        End If

        If txtOut.Text = "" Then
            mapwinutility.logger.msg("Please select an output file by clicking the folder button.", MsgBoxStyle.Exclamation, "Choose Output File")
            Return
        End If

        Dim runningType As MapWinGIS.ShpfileType = MapWinGIS.ShpfileType.SHP_NULLSHAPE

        For i As Integer = 0 To ListBox1.Items.Count - 1
            Dim sf As New MapWinGIS.Shapefile
            If Not sf.Open(ListBox1.Items(i).ToString()) Then
                mapwinutility.logger.msg("The file " + ListBox1.Items(i).ToString() + " could not be opened.", MsgBoxStyle.Exclamation, "Could Not Open File")
                Return
            End If

            If (runningType = MapWinGIS.ShpfileType.SHP_NULLSHAPE) Then
                runningType = sf.ShapefileType
            Else
                If Not runningType = sf.ShapefileType Then
                    mapwinutility.logger.msg("One or more of the shapefiles are of different types. All shapefiles to be merged must be of the same type (point, line, polygon).", MsgBoxStyle.Exclamation, "Mismatched Shapefile Types")
                    Return
                End If
            End If
            sf.Close()
        Next

        Dim ar As New ArrayList
        For i As Integer = 0 To ListBox1.Items.Count - 1
            If Not ar.Contains(ListBox1.Items(i).ToString()) Then ar.Add(ListBox1.Items(i).ToString())
        Next

        Dim FilterDupsBySingleAttribute As String = ""
        If rdOneAttribute.Checked Then
            FilterDupsBySingleAttribute = cmbField.Text
            'Ensure this field exists in all shapefiles, if not, display error

            For i As Integer = 0 To ListBox1.Items.Count - 1
                Dim sf As New MapWinGIS.Shapefile
                If Not sf.Open(ListBox1.Items(i).ToString()) Then
                    mapwinutility.logger.msg("The file " + ListBox1.Items(i).ToString() + " could not be opened.", MsgBoxStyle.Exclamation, "Could Not Open File")
                    Return
                End If
                Dim foundThisShapefile As Boolean = False
                For j As Integer = 0 To sf.NumFields - 1
                    If sf.Field(j).Name.ToLower().Trim() = cmbField.Text.ToLower().Trim() Then
                        'found
                        foundThisShapefile = True
                        Exit For
                    End If
                Next
                If Not foundThisShapefile Then
                    mapwinutility.logger.msg("The specified field (" + cmbField.Text + ") doesn't exist in all shapefiles.", MsgBoxStyle.Exclamation, "Field Not Shared Among All Shapefiles")
                    rdGeometry.Checked = True
                    Return
                End If
                sf.Close()
            Next
        End If

        If MergeShapefiles(ar.ToArray(GetType(String)), txtOut.Text, rdGeometry.Checked, rdAttributes.Checked, FilterDupsBySingleAttribute) And System.IO.File.Exists(txtOut.Text) Then
            If chbAddtoMap.Checked And Not m_map Is Nothing Then
                m_map.Layers.Add(txtOut.Text, System.IO.Path.GetFileNameWithoutExtension(txtOut.Text))
            End If

            mapwinutility.logger.msg("The operation has completed successfully.", MsgBoxStyle.Information, "Done!")
            Me.Close()
        Else
            mapwinutility.logger.msg("The operation has completed with errors. Check the output file for validity.", MsgBoxStyle.Information, "Completed with Errors")
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim dlg As New System.Windows.Forms.OpenFileDialog
        dlg.Filter = "Shapefiles (*.shp)|*.shp"
        dlg.Multiselect = True
        If dlg.ShowDialog() Then
            For Each s As String In dlg.FileNames
                ListBox1.Items.Add(s)
            Next
        End If

        Button3.Enabled = (Not ListBox1.SelectedIndex = -1)
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If Not ListBox1.SelectedIndex = -1 Then ListBox1.Items.RemoveAt(ListBox1.SelectedIndex)
        Button3.Enabled = (Not ListBox1.SelectedIndex = -1)
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        Button3.Enabled = (Not ListBox1.SelectedIndex = -1)
    End Sub

    Public Function MergeShapefiles(ByVal InSFs() As String, ByVal OutSF As String, _
        ByVal FilterDupsByGeometry As Boolean, ByVal FilterDupsByAttributes As Boolean, _
        ByVal FilterDupsBySingleAttribute As String) As Boolean
        If InSFs.Length = 0 Then Return False

        'Error warning shown once already?
        Dim warnedAlready As Boolean = False

        'Delete the output file if it exists
        If System.IO.File.Exists(OutSF) Then System.IO.File.Delete(OutSF)
        If System.IO.File.Exists(System.IO.Path.ChangeExtension(OutSF, ".shx")) Then System.IO.File.Delete(System.IO.Path.ChangeExtension(OutSF, ".shx"))
        If System.IO.File.Exists(System.IO.Path.ChangeExtension(OutSF, ".dbf")) Then System.IO.File.Delete(System.IO.Path.ChangeExtension(OutSF, ".dbf"))
        If System.IO.File.Exists(System.IO.Path.ChangeExtension(OutSF, ".prj")) Then System.IO.File.Delete(System.IO.Path.ChangeExtension(OutSF, ".prj"))

        'Copy the first one by coping the file, rather than a slower shape by shape copy.
        'Dump the remaining files over the first shape by shape, using the comparison
        System.IO.File.Copy(InSFs(0), OutSF)
        System.IO.File.Copy(System.IO.Path.ChangeExtension(InSFs(0), ".dbf"), System.IO.Path.ChangeExtension(OutSF, ".dbf"))
        System.IO.File.Copy(System.IO.Path.ChangeExtension(InSFs(0), ".shx"), System.IO.Path.ChangeExtension(OutSF, ".shx"))
        If System.IO.File.Exists(System.IO.Path.ChangeExtension(InSFs(0), ".prj")) Then System.IO.File.Copy(System.IO.Path.ChangeExtension(InSFs(0), ".prj"), System.IO.Path.ChangeExtension(OutSF, ".prj"))

        If InSFs.Length = 1 Then Return True 'Already done!

        'Hold the actual shapefiles
        Dim inShapefiles(InSFs.Length - 1) As MapWinGIS.Shapefile

        'Open them all, including the first (used to compare against to ensure all are of same type)
        For z As Integer = 0 To InSFs.Length - 1
            inShapefiles(z) = New MapWinGIS.Shapefile
            inShapefiles(z).Open(InSFs(z))
            If Not inShapefiles(z).ShapefileType = inShapefiles(0).ShapefileType Then Return False
        Next

        'Open the destination shapefile, copied from the first source file above, begin editing
        Dim outp As New MapWinGIS.Shapefile
        outp.Open(OutSF)
        outp.StartEditingShapes(True)

        'Skipping first shapefile - it was copied directly to the destination file.
        For z As Integer = 1 To inShapefiles.Length - 1
            'Fields may differ between the shapefiles; get all available on beginning
            'each shapefile.
            Dim FieldIndexes As New Hashtable
            For j As Integer = 0 To inShapefiles(z).NumFields - 1
                FieldIndexes.Add(inShapefiles(z).Field(j).Name, j)
            Next

            For i As Integer = 0 To inShapefiles(z).NumShapes - 1
                txtProgress.Text = i & " of " & inShapefiles(z).NumShapes - 1
                System.Windows.Forms.Application.DoEvents()


                If FilterDupsByGeometry And IsDuplicateShapeByGeometry(outp, inShapefiles(z).Shape(i)) Then
                    Continue For
                End If

                If FilterDupsByAttributes And IsDuplicateShapeByAttributes(outp, inShapefiles(z), i) Then
                    Continue For
                End If

                If Not FilterDupsBySingleAttribute.Trim() = "" Then
                    If IsDuplicateShapeBySingleAttribute(outp, inShapefiles(z), i, FilterDupsBySingleAttribute) Then Continue For
                End If

                Dim sid As Integer = outp.NumShapes
                If Not outp.EditInsertShape(inShapefiles(z).Shape(i), sid) Then
                    If Not warnedAlready Then
                        MsgBox("Warning: One or more shapes could not be merged." + vbCrLf + vbCrLf + "Please ensure that both shapefiles have write permissions and enough disk space is available.", MsgBoxStyle.Exclamation, "Error During Merge")
                        warnedAlready = True
                    End If
                Else
                    For j As Integer = 0 To outp.NumFields - 1
                        If FieldIndexes.Contains(outp.Field(j).Name) Then
                            outp.EditCellValue(j, sid, inShapefiles(z).CellValue(FieldIndexes(outp.Field(j).Name), i))
                        End If
                    Next
                End If
            Next
        Next

        If outp.EditingShapes Then outp.StopEditingShapes(True, True)
        outp.Close()

        'Close all input files (Skipping first shapefile - it was copied directly to the destination file)
        For z As Integer = 1 To inShapefiles.Length - 1
            inShapefiles(z).Close()
        Next

        Return True
    End Function

    Private Shared Function IsDuplicateShapeByGeometry(ByRef searchSF As MapWinGIS.Shapefile, ByRef compareShape As MapWinGIS.Shape) As Boolean
        For i As Integer = 0 To searchSF.NumShapes - 1
            If Not compareShape.NumParts = searchSF.Shape(i).NumParts Then Continue For
            If Not compareShape.numPoints = searchSF.Shape(i).numPoints Then Continue For

            Dim ContinueOuterFor As Boolean = False
            For j As Integer = 0 To compareShape.numPoints - 1
                If Not (compareShape.Point(j).x = searchSF.Shape(i).Point(j).x AndAlso _
                   compareShape.Point(j).y = searchSF.Shape(i).Point(j).y) Then
                    ContinueOuterFor = True
                    Exit For
                End If
            Next
            If ContinueOuterFor Then Continue For

            'Appears to match! Parts, Points, and all point locations match
            Return True
        Next

        'Not found
        Return False
    End Function

    Private Shared Function IsDuplicateShapeBySingleAttribute(ByRef searchSF As MapWinGIS.Shapefile, ByRef compareSF As MapWinGIS.Shapefile, ByVal shpIndex As Integer, ByVal FieldName As String) As Boolean
        Dim searchField As Integer = -1
        Dim compareField As Integer = -1

        For i As Integer = 0 To searchSF.NumFields - 1
            If searchSF.Field(i).Name.ToLower().Trim() = FieldName.ToLower().Trim() Then
                searchField = i
                Exit For
            End If
        Next
        For i As Integer = 0 To compareSF.NumFields - 1
            If compareSF.Field(i).Name.ToLower().Trim() = FieldName.ToLower().Trim() Then
                compareField = i
                Exit For
            End If
        Next

        For i As Integer = 0 To searchSF.NumShapes - 1
            If searchSF.CellValue(searchField, i).ToString().ToLower().Trim() = compareSF.CellValue(compareField, shpIndex).ToString().ToLower().Trim() Then
                Return True 'Duplicate
            End If
        Next

        'Not found
        Return False
    End Function

    Private Shared Function IsDuplicateShapeByAttributes(ByRef searchSF As MapWinGIS.Shapefile, ByRef compareSF As MapWinGIS.Shapefile, ByVal shpIndex As Integer) As Boolean
        If Not compareSF.NumFields = searchSF.NumFields Then Return False 'Differ to much to compare

        For i As Integer = 0 To searchSF.NumShapes - 1
            Dim allMatch As Boolean = True
            For j As Integer = 0 To compareSF.NumFields - 1
                If Not compareSF.CellValue(j, shpIndex).ToString().ToLower().Trim() = searchSF.CellValue(j, i).ToString().ToLower().Trim() Then
                    allMatch = False
                    Exit For
                End If
            Next

            'Appears to match!
            If allMatch Then Return True
        Next

        'Not found
        Return False
    End Function

    Private Sub btnBrowseToOut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseToOut.Click
        Dim fld As New System.Windows.Forms.SaveFileDialog
        fld.Filter = "Shapefiles (*.shp)|*.shp"
        If fld.ShowDialog() Then txtOut.Text = fld.FileName
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdOneAttribute.CheckedChanged
        If rdOneAttribute.Checked Then
            If ListBox1.Items.Count = 0 Then
                MapWinUtility.Logger.Msg("Please add at least one shapefile before selecting this option.", MsgBoxStyle.Exclamation, "Select a Shapefile First")
                rdGeometry.Checked = True
                Return
            End If

            cmbField.Items.Clear()
            Dim sf As New MapWinGIS.Shapefile
            If Not sf.Open(ListBox1.Items(0).ToString()) Then
                MapWinUtility.Logger.Msg("One or more of the shapefiles could not be opened.", MsgBoxStyle.Exclamation, "Could not open shapefile")
                rdGeometry.Checked = True
                Return
            End If

            If (sf.NumFields = 0) Then
                MapWinUtility.Logger.Msg("There are no fields in the first shapefile!", MsgBoxStyle.Exclamation, "No Fields")
                rdGeometry.Checked = True
                Return
            End If

            For i As Integer = 0 To sf.NumFields - 1
                cmbField.Items.Add(sf.Field(i).Name)
            Next
            sf.Close()
            'Guaranteed to have at least one (See if above)
            cmbField.SelectedIndex = 0
        End If

        cmbField.Enabled = rdOneAttribute.Checked
    End Sub

    <CLSCompliant(False)> _
    Public Sub New(ByVal MapWin As MapWindow.Interfaces.IMapWin)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_map = MapWin
    End Sub
End Class