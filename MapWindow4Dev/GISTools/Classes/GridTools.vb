Imports MapWinUtility

Module GridTools
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: DoGridReproject
    ' AUTHOR: Chris Michaelis
    ' DESCRIPTION: Method to reproject Grid
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 11/03/2005    ARA             Added Header
    ' 11/15/2006    JLK             Added logging
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Friend Sub DoGridReproject(ByVal m_Map As MapWindow.Interfaces.IMapWin)
        Dim Errors As Boolean = False
        Logger.Dbg("Start")

        Dim gridFinder As New frmSelectGrids
        gridFinder.ShowDialog(g_MapWindowForm)

        If g_Grids Is Nothing OrElse g_Grids.Count = 0 Then
            Logger.Dbg("NoGridsToProject")
        Else
            'Haven't cancelled; proceeding.
            Dim defaultProjection As String = ""

            'Open the first grid and get it's projection, just to try to give
            'the user the current projection.
            Try
                defaultProjection = CType(g_Grids(0), MapWinGIS.Grid).Header.Projection
                Logger.Dbg("DefaultProjectionSet")
            Catch
                'Noncritical; this is for user friendliness
            End Try

            Dim selectedProjection As String = m_Map.GetProjectionFromUser("Please select the new grid projection.", defaultProjection)
            Logger.Dbg("ProjectionSelected:'" & selectedProjection & "'")

            If Not selectedProjection = "" Then
                For j As Integer = 0 To g_Grids.Count - 1
                    Errors = False
                    Try
                        Logger.Dbg("GridProject " & j + 1 & " " & CType(g_Grids(j), MapWinGIS.Grid).Filename)
                        Dim oldProjection As String = CType(g_Grids(j), MapWinGIS.Grid).Header.Projection
                        Dim origFilename As String = CType(g_Grids(j), MapWinGIS.Grid).Filename

                        Try
                            If System.IO.File.Exists(System.IO.Path.GetDirectoryName(CType(g_Grids(j), MapWinGIS.Grid).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Grids(j), MapWinGIS.Grid).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt") Then
                                System.IO.File.Delete(System.IO.File.Exists(System.IO.Path.GetDirectoryName(CType(g_Grids(j), MapWinGIS.Grid).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Grids(j), MapWinGIS.Grid).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt"))
                            End If
                        Catch ex As Exception
                        End Try

                        Try
                            CType(g_Grids(j), MapWinGIS.Grid).Close()
                        Catch
                        End Try

                        If (oldProjection Is Nothing OrElse _
                            oldProjection = "" OrElse _
                            Not oldProjection.StartsWith("+")) Then
                            'mapwinutility.logger.msg("One of the grids you're attempting to project does not currently have a projection. In order to reproject this file, you must use the following dialog to select the current grid projection." + vbCrLf + vbCrLf + "Please choose the current projection of: " + origFilename, MsgBoxStyle.Exclamation, "No Projection for " + System.IO.Path.GetFileName(origFilename))
                            oldProjection = m_Map.GetProjectionFromUser("The current projection of the file " + System.IO.Path.GetFileName(origFilename) + " cannot be determined." + vbCrLf + "Please select the current projection of the file.", "")

                            'Give up if they fail to provide a projection
                            If oldProjection = "" Then
                                Errors = True 'prevent 'success' message
                                GoTo skip1
                            End If
                        End If

                        Dim newFilename As String = System.IO.Path.ChangeExtension(origFilename, "")
                        If newFilename.EndsWith(".") Then
                            newFilename = newFilename.Substring(0, newFilename.Length - 1)
                        End If
                        newFilename += "_Reprojected" + System.IO.Path.GetExtension(origFilename)

                        Logger.Dbg("OrigFileName " & origFilename)
                        Logger.Dbg("OldProjection " & oldProjection)
                        Logger.Dbg("newFilename " & newFilename)
                        Logger.Dbg("selectedProjection " & selectedProjection)
                        If Not MapWinGeoProc.SpatialReference.ProjectGrid(oldProjection, selectedProjection, origFilename, newFilename, True) Then
                            Errors = True
                            Throw New Exception("Reprojection failed. See the ErrorLog.txt for more info.")
                        End If

                        If Not System.IO.File.Exists(newFilename) Then
                            Errors = True
                            Throw New Exception("The reprojected grid (" & newFilename & ") wasn't found on the disk!")
                        End If
                        Dim newGrd As New MapWinGIS.Grid
                        newGrd.Open(newFilename)
                        newGrd.AssignNewProjection(selectedProjection)

                        g_Grids(j) = newGrd

                        If Not CType(g_Grids(j), MapWinGIS.Grid).Filename = "" Then
                            If System.IO.File.Exists(System.IO.Path.GetDirectoryName(CType(g_Grids(j), MapWinGIS.Grid).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Grids(j), MapWinGIS.Grid).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt") Then
                                Errors = True
                                Logger.Msg("One or more errors occurred during reprojection. Please see the file:" + vbCrLf + System.IO.Path.GetDirectoryName(CType(g_Grids(j), MapWinGIS.Grid).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Grids(j), MapWinGIS.Grid).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt" + vbCrLf + "for more information.")
                            End If
                        End If

                        CType(g_Grids(j), MapWinGIS.Grid).Close()

                        If Not Errors And System.IO.File.Exists(newFilename) Then
                            If Logger.Msg("The reprojection of " + System.IO.Path.GetFileName(origFilename) + " has completed." + vbCrLf + vbCrLf + "The reprojected file is called " + System.IO.Path.GetFileName(newFilename) + "." + vbCrLf + vbCrLf + "Add it to the map now?", _
                                          MsgBoxStyle.Question + MsgBoxStyle.YesNo, _
                                          "Finished! Add to map?") = MsgBoxResult.Yes Then
                                m_Map.Layers.Add(newFilename)
                            End If
                        End If
                    Catch ex As Exception
                        'If Not g_Grids(j).Filename = "" Then
                        Logger.Msg("An error occurred while reprojecting the grid(s): " + vbCrLf + vbCrLf + "The error text is:" + vbCrLf + ex.ToString())
                        'If System.IO.File.Exists(System.IO.Path.GetDirectoryName(CType(g_Grids(j), MapWinGIS.Grid).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Grids(j), MapWinGIS.Grid).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt") Then
                        'Errors = True
                        'mapwinutility.logger.msg("One or more errors occurred during reprojection. Please see the file:" + vbCrLf + System.IO.Path.GetDirectoryName(CType(g_Grids(j), MapWinGIS.Grid).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Grids(j), MapWinGIS.Grid).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt" + vbCrLf + "for more information.")
                        'End If
                        'End If
                    End Try

skip1:
                Next
            End If
        End If

        Cleanup()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: mnuChangeGridFormats
    ' AUTHOR: Chris Michaelis
    ' DESCRIPTION: Method to change grid formats
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 11/03/2005    ARA             Added Header
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Friend Sub mnuChangeGridFormats()
        Try
            Logger.Dbg("ChangeGridFormats")
            Dim Errors As Boolean = False
            Dim gridFinder As New frmSelectGrids
            gridFinder.ShowDialog(g_MapWindowForm)

            If g_Grids Is Nothing Then
                Logger.Dbg("ChangeGridFormats - Grids is Nothing")
                Return
            End If

            If g_Grids.Count > 0 Then
                'Haven't cancelled; proceeding.

                Dim output As New frmOutput
                output.SetOptionsEnabled(True, True, True, False, True, True)

                'Default to data type of first grid for output
                Select Case CType(g_Grids(0), MapWinGIS.Grid).DataType
                    Case MapWinGIS.GridDataType.DoubleDataType
                        output.cmbDataType.Text = "Double Precision Float (8 bytes)"
                    Case MapWinGIS.GridDataType.FloatDataType
                        output.cmbDataType.Text = "Single Precision Float (4 bytes)"
                    Case MapWinGIS.GridDataType.InvalidDataType
                        output.cmbDataType.Text = "Double Precision Float (8 bytes)"
                    Case MapWinGIS.GridDataType.LongDataType
                        output.cmbDataType.Text = "Long Integer (4 bytes)"
                    Case MapWinGIS.GridDataType.ShortDataType
                        output.cmbDataType.Text = "Short Integer (2 bytes)"
                    Case MapWinGIS.GridDataType.UnknownDataType
                        output.cmbDataType.Text = "Double Precision Float (8 bytes)"
                End Select

                output.txtName.Text = "(original filename)"
                output.ShowDialog(g_MapWindowForm)
                If g_OutputPath = "" Then Errors = True 'Prevent the "Succeeded!" dialog from proclaiming a lie

                If Not g_OutputPath = "" Then
                    For j As Integer = 0 To g_Grids.Count - 1
                        Try
                            Dim tGrd As MapWinGIS.Grid = CType(g_Grids(j), MapWinGIS.Grid)
                            Dim filename As String = ""
                            Try
                                filename = g_OutputPath + IIf(g_OutputPath.EndsWith("\"), "", "\") + IIf(System.IO.Path.GetFileNameWithoutExtension(tGrd.Filename) = "", "NewGrid", System.IO.Path.GetFileNameWithoutExtension(tGrd.Filename)) + g_newExt
                            Catch
                                filename = g_OutputPath + IIf(g_OutputPath.EndsWith("\"), "", "\") + "NewGrid" + g_newExt
                            End Try

                            If Not MapWinGeoProc.DataManagement.ChangeGridFormat(tGrd.Filename, filename, g_newFormat, g_newDataType, Double.Parse(output.txtMultiplier.Text)) Then Errors = True

                            If g_AddOutputToMW Then
                                Logger.Status("Adding Grid to Project")
                                g_MW.Layers.Add(filename)
                                Logger.Status("Done adding Grid to Project")
                            End If

                        Catch ex As Exception
                            Errors = True
                            Logger.Msg("An error has occurred. Processing will continue; the full error text for this operation follows below." + vbCrLf + vbCrLf + ex.ToString())
                        End Try
                    Next
                End If

                If Not Errors Then
                    Logger.Status("Grid format changed successfully.")
                End If
            End If

            Cleanup()
        Catch e As Exception
            'mapwinutility.logger.dbg("DEBUG: " + e.ToString())
            Logger.Dbg("ChangeGridFormats:Error:" & e.ToString())
        End Try
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: mnuCreateGridImages
    ' AUTHOR: Chris Michaelis
    ' DESCRIPTION: Method to create grid images
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 11/03/2005    ARA             Added Header
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Friend Sub mnuCreateGridImages()
        Dim Errors As Boolean = False
        Dim gridFinder As New frmSelectGrids
        gridFinder.ShowDialog(g_MapWindowForm)

        If g_Grids Is Nothing Then Return

        If g_Grids.Count > 0 Then
            'Haven't cancelled; proceeding.

            Dim output As New frmOutput
            output.SetOptionsEnabled(False, False, True, False, False)
            output.chkAdd.Checked = False
            output.txtName.Text = "(original filename)"
            output.ShowDialog(g_MapWindowForm)
            If Not g_OutputPath = "" Then

                For j As Integer = 0 To g_Grids.Count - 1
                    Try
                        Dim tGrd As MapWinGIS.Grid = CType(g_Grids(j), MapWinGIS.Grid)
                        'Prompt for a coloring scheme
                        Dim MyDialog As New frmColoringSchemeStylePicker
                        MyDialog.SetGridObject(tGrd)
                        MyDialog.ShowDialog(g_MapWindowForm)

                        Dim ProgressForm As New frmProgress
                        ProgressForm.Owner = g_MapWindowForm
                        ProgressForm.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
                        ProgressForm.Show()
                        ProgressForm.Taskname = "Creating Image..."

                        Dim filename As String = g_OutputPath + "\" + System.IO.Path.GetFileNameWithoutExtension(tGrd.Filename) + ".bmp"

                        If g_Scheme Is Nothing OrElse g_Scheme.NumBreaks = 0 Then
                            g_Scheme = New MapWinGIS.GridColorScheme
                            g_Scheme.UsePredefined(tGrd.Minimum, tGrd.Maximum, MapWinGIS.PredefinedColorScheme.SummerMountains)
                            CreateImage(filename, tGrd, g_Scheme, ProgressForm)
                            g_Scheme = Nothing
                        Else
                            CreateImage(filename, tGrd, g_Scheme, ProgressForm)
                        End If
                        ProgressForm.Close()
                        ProgressForm.Dispose()
                        ProgressForm = Nothing
                    Catch ex As Exception
                        Errors = True
                        MapWinUtility.Logger.Msg("An error has occurred. Processing will continue; the full error text for this operation follows below." + vbCrLf + vbCrLf + ex.ToString())
                    End Try
                Next

            End If
            If Not Errors Then MapWinUtility.Logger.Msg("The grid image(s) were created successfully.", MsgBoxStyle.Information, "MapWindow Tools 3.0")
        End If

        Cleanup()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: mnuResampleGrids
    ' AUTHOR: Chris Michaelis
    ' DESCRIPTION: Method to resample grids
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 11/03/2005    ARA             Added Header
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Friend Sub mnuResampleGrids()
        Dim Errors As Boolean = False
        Dim gridFinder As New frmSelectGrids
        gridFinder.ShowDialog(g_MapWindowForm)

        If g_Grids Is Nothing Then Return

        If g_Grids.Count > 0 Then
            'Haven't cancelled; proceeding.
            Dim resampleOpts As New frmResampleOpts
            resampleOpts.ShowDialog(g_MapWindowForm)

            If Not g_NewCellSize = -1 Then
                'Still haven't canceled. Proceed.
                Dim output As New frmOutput
                output.SetOptionsEnabled(True, True, True, False)
                output.txtName.Text = "(original filename)"
                output.ShowDialog(g_MapWindowForm)

                If Not g_newDataType = MapWinGIS.GridDataType.UnknownDataType _
                 And Not g_newFormat = MapWinGIS.GridFileType.InvalidGridFileType Then
                    Try
                        For j As Integer = 0 To g_Grids.Count - 1
                            Dim ProgressForm As New frmProgress
                            ProgressForm.Owner = g_MapWindowForm
                            ProgressForm.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
                            ProgressForm.Show()

                            ProgressForm.Taskname = "Resampling..."

                            ProgressForm.Filename = CType(g_Grids(j), MapWinGIS.Grid).Filename
                            DoResample(g_Grids(j), g_NewCellSize, CType(ProgressForm, MapWinGIS.ICallback))
                            ProgressForm.CurrentStep += 1
                            Dim filen As String = CType(g_Grids(j), MapWinGIS.Grid).Filename
                            If g_AddOutputToMW Then g_MW.Layers.Add(filen, System.IO.Path.GetFileNameWithoutExtension(filen))
                            ProgressForm.Close()
                            ProgressForm.Dispose()
                            ProgressForm = Nothing
                        Next
                    Catch ex As Exception
                        Errors = True
                        MapWinUtility.Logger.Msg("An error has occurred. Processing will continue; the full error text for this operation follows below." + vbCrLf + vbCrLf + ex.ToString())
                    End Try
                End If
                If Not Errors Then MapWinUtility.Logger.Msg("The grid(s) were resampled successfully.", MsgBoxStyle.Information, "MapWindow Tools 3.0")
            End If
        End If

        Cleanup()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: mnuMergeGrids
    ' AUTHOR: Chris Michaelis
    ' DESCRIPTION: Method to merge grids
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 11/03/2005    ARA             Added Header
    ' 11/15/2006    JLK             Added logging
    ' 11/21/2006    JLK             Use renderer from first grid in new grid
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Friend Sub mnuMergeGrids()
        Dim Errors As Boolean = False
        Logger.Dbg("Start")

        Dim gridFinder As New frmSelectGrids
        If gridFinder.ShowDialog(g_MapWindowForm) = Windows.Forms.DialogResult.Cancel Then
            Logger.Dbg("SelectGrids:UserCancel")
        Else
            If g_Grids Is Nothing OrElse g_Grids.Count = 0 Then
                Logger.Dbg("NoGridsToMerge")
            Else 'Process selected grids
                Logger.Dbg("Merge " & g_Grids.Count & " Grids")
                Dim ProgressForm As New frmProgress
                Try
                    Dim output As New frmOutput
                    output.SetOptionsEnabled(False, True, True, True)
                    output.SetDefaultDataType(CType(g_Grids(0), MapWinGIS.Grid).DataType)
                    output.SetDefaultOutputFormat(System.IO.Path.GetExtension(CType(g_Grids(0), MapWinGIS.Grid).Filename))
                    If output.ShowDialog() = Windows.Forms.DialogResult.Cancel Then
                        Logger.Dbg("UserCanceled")
                        Errors = True
                    Else
                        If g_newDataType = MapWinGIS.GridDataType.UnknownDataType Then
                            Logger.Dbg("UnknownDataType")
                        ElseIf g_newFormat = MapWinGIS.GridFileType.InvalidGridFileType Then
                            Logger.Dbg("InvalidGridFileType")
                        Else
                            ProgressForm.Owner = g_MapWindowForm
                            ProgressForm.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
                            ProgressForm.Show()
                            ProgressForm.Taskname = "Merging..."
                            ProgressForm.Filename = g_OutputPath & "\" & g_OutputName & g_newExt
                            ProgressForm.CurrentStep += 1
                            Logger.Dbg("MergeGrids")
                            Dim grd As MapWinGIS.Grid = DoMerge(g_OutputPath & "\" & g_OutputName & g_newExt, _
                                                                CType(ProgressForm, MapWinGIS.ICallback))
                            If grd Is Nothing Then
                                ' The merge function will have displayed an error message. Don't duplicate it here.
                                Logger.Dbg("NoGridCreated")
                                Errors = True
                            Else
                                Logger.Dbg("SaveMergedGrid")
                                ProgressForm.Taskname = "Saving merged grid..."
                                ProgressForm.Filename = g_OutputPath & "\" & g_OutputName & g_newExt
                                grd.Save(g_OutputPath & "\" & g_OutputName & g_newExt, g_newFormat, ProgressForm)

                                Logger.Dbg("CloseNewGrid")
                                grd.Close()

                                If g_AddOutputToMW Then
                                    Logger.Dbg("AddGridToProject")
                                    ProgressForm.Taskname = "Adding Grid to Project"
                                    'Note: The coloring scheme from the original grid (the first input file)
                                    'is already copied at this point -- it was done in the DoMerge() function.
                                    'Therefore when the layer is added, it will be displayed with the same rendering
                                    g_MW.Layers.Add(g_OutputPath & "\" & g_OutputName & g_newExt, g_OutputName, True, True)
                                End If
                            End If
                        End If
                        Logger.Dbg("CloseOldGrids")
                        For q As Integer = 0 To g_Grids.Count - 1
                            CType(g_Grids(q), MapWinGIS.Grid).Close()
                        Next
                        g_Grids.Clear()
                    End If
                Catch ex As Exception
                    Errors = True
                    Logger.Msg("An error has occurred. The full error text for this operation follows below." + vbCrLf + vbCrLf + ex.ToString(), _
                               "GISTools Merge Grids")
                End Try

                ProgressForm.Close()
                ProgressForm.Dispose()
                ProgressForm = Nothing

                If Not Errors Then
                    Logger.Msg("The grid(s) were merged successfully.", _
                               MsgBoxStyle.Information, _
                               "GISTools")
                End If
            End If
        End If
        Cleanup()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: DoApplyGridProj
    ' AUTHOR: Chris Michaelis
    ' DESCRIPTION: Method to apply grid projection
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 11/03/2005    ARA             Added Header
    ' 11/15/2006    JLK             Added logging
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Friend Sub DoApplyGridProj(ByVal m_Map As MapWindow.Interfaces.IMapWin)
        Dim Errors As Boolean = False
        Logger.Dbg("Start")

        Dim gridFinder As New frmSelectGrids
        gridFinder.ShowDialog(g_MapWindowForm)

        If g_Grids Is Nothing OrElse g_Grids.Count = 0 Then
            Logger.Dbg("NoGridsToApplyProjectionTo")
        Else
            'Haven't cancelled; proceeding.
            Dim defaultProjection As String = ""

            'Open the first grid and get it's projection, just to try to give
            'the user the current projection.
            Try
                defaultProjection = CType(g_Grids(0), MapWinGIS.Grid).Header.Projection
                Logger.Dbg("DefaultProjectionSet")
            Catch
                'Noncritical; this is for user friendliness
            End Try

            Dim selectedProjection As String = m_Map.GetProjectionFromUser("Please select the projection to be applied.", defaultProjection)
            Logger.Dbg("ProjectionSelected:'" & selectedProjection & "'")

            If Not selectedProjection = "" Then
                For j As Integer = 0 To g_Grids.Count - 1
                    Try
                        Logger.Dbg("GridProjectionApply " & j + 1 & " " & CType(g_Grids(j), MapWinGIS.Grid).Filename)
                        CType(g_Grids(j), MapWinGIS.Grid).AssignNewProjection(selectedProjection)
                    Catch ex As Exception
                        Errors = True
                        Logger.Msg("An error occurred while applying the specified projection to: " + vbCrLf + CType(g_Grids(j), MapWinGIS.Grid).Filename + vbCrLf + vbCrLf + "The error text is:" + vbCrLf + ex.ToString())
                    End Try
                Next
                If Not Errors Then
                    Logger.Msg("The projection(s) have been applied.", _
                                MsgBoxStyle.Information, _
                                "GISTools")
                End If
            End If
        End If

        Cleanup()
    End Sub

    Public Function DoResample(ByRef grd As MapWinGIS.Grid, ByVal CellSize As Double, ByRef Progress As MapWinGIS.ICallback) As Boolean
        Dim i, j As Integer
        Dim newGrid As New MapWinGIS.Grid
        Dim newHeader As New MapWinGIS.GridHeader
        Dim numCols, numRows As Integer
        Dim absLeft, absRight, absBottom, absTop As Double
        Dim halfDX, halfDY As Double
        Dim tX, tY, oldX, oldY, nDX, cDX As Double

        Dim newFilen As String = System.IO.Path.GetFileName(grd.Filename)

        Try
            With newHeader
                numCols = Int((grd.Header.dX * grd.Header.NumberCols) / CellSize)
                numRows = Int((grd.Header.dY * grd.Header.NumberRows) / CellSize)

                absLeft = grd.Header.XllCenter - (grd.Header.dX / 2)
                absBottom = grd.Header.YllCenter - (grd.Header.dY / 2)
                absRight = absLeft + (grd.Header.dX * grd.Header.NumberCols)
                absTop = absBottom + (grd.Header.dY * grd.Header.NumberRows)

                newHeader.NumberCols = numCols
                newHeader.NumberRows = numRows
                newHeader.dX = CellSize
                newHeader.dY = CellSize
                newHeader.XllCenter = absLeft + (CellSize / 2)
                newHeader.YllCenter = absBottom + (CellSize / 2)
                newHeader.NodataValue = grd.Header.NodataValue
                newHeader.Notes = grd.Header.Notes
                newHeader.Key = grd.Header.Key
                newHeader.Projection = grd.Header.Projection

                If newGrid.CreateNew(g_OutputPath + "\" + newFilen, newHeader, g_newDataType, grd.Header.NodataValue, True, g_newFormat) = False Then
                    Return False
                End If

                halfDX = newHeader.dX * 0.5
                halfDY = newHeader.dY * 0.5

                For j = 0 To numRows - 1
                    tY = absTop - (j * newHeader.dY) - halfDY
                    Progress.Progress(grd.Filename, j / numRows * 100, "Resampling " & grd.Filename & " row " & j)

                    nDX = newHeader.dX
                    cDX = grd.Header.dX

                    oldY = Int(grd.Header.NumberRows - ((tY - absBottom) / grd.Header.dY))

                    For i = 0 To numCols - 1
                        tX = absLeft + (i * nDX) + halfDX
                        oldX = Int((tX - absLeft) / cDX)
                        newGrid.Value(i, j) = grd.Value(oldX, oldY)
                    Next i
                Next j
            End With

            grd.Close()
            grd = newGrid
            grd.Save(g_OutputPath + "\" + newFilen, g_newFormat, Progress)

        Catch ex As Exception
            MapWinUtility.Logger.Msg(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Critical Or MsgBoxStyle.Information, "Grid Wizard 2.0 - Error")

        End Try
    End Function

    Public Function DoMerge(ByVal NewFilename As String, ByRef Progress As MapWinGIS.ICallback) As MapWinGIS.Grid
        Dim merger As New MapWinGIS.Utils
        Dim tGrids(), newGrid As MapWinGIS.Grid

        Logger.Dbg("OutputGridName: " & NewFilename & " Format: " & g_newFormat.ToString)
        Logger.Dbg("InputGridCount: " & g_Grids.Count)

        ReDim tGrids(g_Grids.Count - 1)
        tGrids = g_Grids.ToArray(GetType(MapWinGIS.Grid))
        For i As Integer = 0 To g_Grids.Count - 1
            Logger.Dbg("  InputGrid " & i + 1 & " " & tGrids(i).Filename)
        Next

        'merge the grids if there is more than one added
        If g_Grids.Count > 1 Then
            newGrid = merger.GridMerge(tGrids, NewFilename, True, g_newFormat, Progress)
            If newGrid Is Nothing Then
                Logger.Msg("Error merging grids!", _
                           MsgBoxStyle.Critical Or MsgBoxStyle.Information, _
                           "GISTools - DoMerge Error")
                Return Nothing
            Else
                ' What do do about coloring...? Bugzila 500
                ' Find all mins and maxes from all grids
                ' Ideally, find one grid that contains all mins and maxes
                ' use that one. Else, generate a new coloring scheme.
                Dim Mins As New ArrayList()
                Dim Maxs As New ArrayList()
                For i As Integer = 0 To g_Grids.Count - 1
                    Mins.Add(Double.Parse(CType(g_Grids(i), MapWinGIS.Grid).Minimum.ToString()))
                    Maxs.Add(Double.Parse(CType(g_Grids(i), MapWinGIS.Grid).Maximum.ToString()))
                Next

                Dim ColorSchemeToUse As Integer = -1
                For x As Integer = 0 To g_Grids.Count - 1
                    Dim AllGridsInThisOne As Boolean = True
                    For i As Integer = 0 To g_Grids.Count - 1
                        If Not x = i Then
                            If (CType(Mins(i), Double) < CType(Mins(x), Double)) Then
                                AllGridsInThisOne = False
                                Exit For
                            End If
                            If (CType(Maxs(i), Double) > CType(Maxs(x), Double)) Then
                                AllGridsInThisOne = False
                                Exit For
                            End If
                        End If
                    Next

                    If AllGridsInThisOne = True Then
                        ColorSchemeToUse = x
                        Exit For
                    End If
                Next

                If ColorSchemeToUse = -1 Then 'Need new coloring scheme
                    'Do nothing - just add it (no .mwleg == new coloring scheme)
                    Logger.Dbg("MergeGrid: No suitable coloring scheme contains full value range - will generate new one")
                Else
                    'Copy from the one we wanted
                    Logger.Dbg("MergeGrid: Coloring scheme " + ColorSchemeToUse.ToString() + " is suitable - copying")

                    Try
                        Dim fn1 As String = "noexist"
                        Dim o As MapWinGIS.Grid = CType(g_Grids(ColorSchemeToUse), MapWinGIS.Grid)
                        If Not o Is Nothing Then fn1 = o.Filename
                        o = Nothing
                        If Not fn1 = "noexist" AndAlso System.IO.File.Exists(System.IO.Path.ChangeExtension(fn1, ".mwleg")) Then
                            System.IO.File.Copy(System.IO.Path.ChangeExtension(fn1, ".mwleg"), System.IO.Path.ChangeExtension(NewFilename, ".mwleg"))
                            Logger.Dbg("MergeGrid: Coloring scheme from " + fn1 + " applied to output grid")
                        End If
                    Catch e As Exception
                        Logger.Dbg("Unable to copy old coloring scheme to merged grid: " + e.Message)
                    End Try
                End If
                Logger.Dbg("MergeComplete")
                Return newGrid
            End If
        Else
            Logger.Dbg("MergeNotNeeted:Only 1 Grid")
            Return CType(g_Grids(0), MapWinGIS.Grid)
        End If
    End Function

    Public Sub CreateImage(ByVal Filename As String, ByVal Grid As MapWinGIS.Grid, ByVal ColoringScheme As MapWinGIS.GridColorScheme, ByVal Progress As MapWinGIS.ICallback)
        Dim img As MapWinGIS.Image
        Dim g As New MapWinGIS.Utils

        img = g.GridToImage(Grid, ColoringScheme, Progress)
        If Not img Is Nothing Then
            'If newFormat = MapWinGIS.GridFileType.Esri Then
            '    Filename = Grid.Filename & ".bmp"
            'Else
            '    Filename = Grid.Filename.Substring(0, Grid.Filename.LastIndexOf(".")) & ".bmp"
            'End If
            img.Save(Filename, True, MapWinGIS.ImageType.USE_FILE_EXTENSION)
            If g_AddOutputToMW Then
                g_MW.Layers.Add(Filename)
            End If
        Else
            MapWinUtility.Logger.Msg("Image creation failed.", MsgBoxStyle.Critical Or MsgBoxStyle.Information, "Grid Wizard 2.0 - Error")
        End If
    End Sub
End Module
