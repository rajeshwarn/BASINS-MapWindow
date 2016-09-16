Module ImageTools
    ''' <summary>
    ''' DoApplyImageProj                                              
    ''' Opens an instance of frmSelectImages to allow image selection
    ''' Opens a dialog to allow the user to specify a projection
    ''' Creates a prj file with the projection string
    ''' </summary>
    ''' <remarks>
    ''' By Ted Dunsford on 6/27/2006
    ''' Derived from Chris's DoAPplyGridProj
    ''' </remarks>
    Friend Sub DoApplyImageProj(ByVal m_map As MapWindow.Interfaces.IMapWin)
        Dim Errors As Boolean = False
        Dim ImageFinder As New frmSelectImages
        ImageFinder.ShowDialog(g_MapWindowForm)
        Dim img As MapWinGIS.Image

        If g_Images Is Nothing Then Return

        If g_Images.Count > 0 Then
            'Haven't cancelled; proceeding.
            Dim defaultProjection As String = ""

            ''Open the first image and get it's projection, just to try to give
            ''the user the current projection.
            'Try
            '    defaultProjection = CType(g_Grids(0), MapWinGIS.Grid).Header.Projection
            'Catch
            '    'Noncritical; this is for user friendliness
            'End Try

            Dim selectedProjection As String = m_Map.GetProjectionFromUser("Please select the projection to be applied.", defaultProjection)

            If Not selectedProjection = "" Then
                For j As Integer = 0 To g_Images.Count - 1
                    Try
                        'Just paste the text into a file named filename.prj
                        img = g_Images(j)
                        img.SetProjection(selectedProjection)
                    Catch ex As Exception
                        Errors = True
                        Windows.Forms.MessageBox.Show("An error occurred while applying the specified projection to: " + vbCrLf + g_Images(j).Filename + vbCrLf + vbCrLf + "The error text is:" + vbCrLf + ex.ToString())
                    End Try
                Next
                If Not Errors Then MapWinUtility.Logger.Msg("The projection(s) have been applied.", MsgBoxStyle.Information, "Finished")
            End If
        End If
        Cleanup()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: DoImageReproject
    ' AUTHOR: Ted Dunsford
    ' DESCRIPTION: Method to reproject Images
    ' (This calling method was basically copied from Chris's ReProject Grid code)
    ' This uses the new Projective class in MapWinGeoProc 
    ' and the ProjectImage function stored there
    ' INPUTS:   None
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 11/03/2006    Ted           Created initial function
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Friend Sub DoImageReproject(ByVal m_map As MapWindow.Interfaces.IMapWin)
        Dim Errors As Boolean = False
        Dim imageFinder As New frmSelectImages
        imageFinder.ShowDialog(g_MapWindowForm)

        If g_Images Is Nothing Then Return

        If g_Images.Count > 0 Then
            'Haven't cancelled; proceeding.
            Dim defaultProjection As String = ""

            'Open the first grid and get it's projection, just to try to give
            'the user the current projection.
            Try
                defaultProjection = CType(g_Images(0), MapWinGIS.Grid).Header.Projection
            Catch
                'Noncritical; this is for user friendliness
            End Try

            Dim selectedProjection As String = m_Map.GetProjectionFromUser("Please select the new grid projection.", defaultProjection)

            If Not selectedProjection = "" Then
                For j As Integer = 0 To g_Images.Count - 1
                    Errors = False
                    Try
                        Dim oldProjection As String = CType(g_Images(j), MapWinGIS.Image).GetProjection
                        Dim origFilename As String = CType(g_Images(j), MapWinGIS.Image).Filename


                        Try
                            If System.IO.File.Exists(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt") Then
                                System.IO.File.Delete(System.IO.File.Exists(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt"))
                            End If
                        Catch ex As Exception
                        End Try

                        Try
                            CType(g_Images(j), MapWinGIS.Image).Close()
                        Catch
                        End Try

                        If (oldProjection Is Nothing OrElse oldProjection = "" OrElse Not oldProjection.StartsWith("+")) Then
                            'mapwinutility.logger.msg("One of the grids you're attempting to project does not currently have a projection. In order to reproject this file, you must use the following dialog to select the current grid projection." + vbCrLf + vbCrLf + "Please choose the current projection of: " + origFilename, MsgBoxStyle.Exclamation, "No Projection for " + System.IO.Path.GetFileName(origFilename))
                            oldProjection = m_Map.GetProjectionFromUser("The current projection of the file " + System.IO.Path.GetFileName(origFilename) + " cannot be determined." + vbCrLf + "Please select the current projection of the file.", "")

                            'Give up if they fail to provide a projection
                            If oldProjection = "" Then
                                Errors = True 'prevent 'success' message
                                GoTo skip1
                            End If
                        End If

                        Dim newFilename As String = System.IO.Path.ChangeExtension(origFilename, "")
                        If newFilename.EndsWith(".") Then newFilename = newFilename.Substring(0, newFilename.Length - 1)
                        newFilename += "_Reprojected" + System.IO.Path.GetExtension(origFilename)
                        'Dim bob As New ProjectionViewerOld
                        'bob.SetProjIn(oldProjection)
                        'bob.SetProjOut(selectedProjection)
                        'bob.ShowDialog()

                        Dim ProgressForm As New frmProgress
                        ProgressForm.Owner = g_MapWindowForm
                        ProgressForm.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
                        ProgressForm.Show()
                        ProgressForm.Taskname = "Reprojecting..."
                        ProgressForm.Filename = CType(g_Images(j), MapWinGIS.Image).Filename

                        MapWinGeoProc.SpatialReference.ProjectImage(oldProjection, selectedProjection, origFilename, newFilename, Nothing)

                        ProgressForm.CurrentStep += 1
                        ProgressForm.Close()
                        ProgressForm.Dispose()
                        ProgressForm = Nothing

                        Dim newImg As New MapWinGIS.Image
                        newImg.Open(newFilename)

                        g_Images(j) = newImg

                        If Not CType(g_Images(j), MapWinGIS.Image).Filename = "" Then
                            If System.IO.File.Exists(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt") Then
                                Errors = True
                                MapWinUtility.Logger.Msg("One or more errors occurred during reprojection. Please see the file:" + vbCrLf + System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt" + vbCrLf + "for more information.")
                            End If
                        End If

                        CType(g_Images(j), MapWinGIS.Image).Close()

                        If Not Errors And System.IO.File.Exists(newFilename) Then
                            If MapWinUtility.Logger.Msg("The reprojection of " + System.IO.Path.GetFileName(origFilename) + " has completed." + vbCrLf + vbCrLf + "The reprojected file is called " + System.IO.Path.GetFileName(newFilename) + "." + vbCrLf + vbCrLf + "Add it to the map now?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Finished! Add to map?") = MsgBoxResult.Yes Then
                                m_Map.Layers.Add(newFilename)
                            End If
                        End If
                    Catch ex As Exception
                        'If Not g_Images(j).Filename = "" Then
                        Windows.Forms.MessageBox.Show("An error occurred while reprojecting the grid(s): " + vbCrLf + vbCrLf + "The error text is:" + vbCrLf + ex.ToString())
                        'If System.IO.File.Exists(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt") Then
                        'Errors = True
                        'mapwinutility.logger.msg("One or more errors occurred during reprojection. Please see the file:" + vbCrLf + System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt" + vbCrLf + "for more information.")
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
    ' TITLE: DoRectifyToWorldfile
    ' AUTHOR: Ted Dunsford
    ' DESCRIPTION: Since MapWindow can't do skew or rotation from a world file on the fly
    ' this function creates a modified image that will match the affine skew/rotate.
    ' Depends SpatialReferencing tools in MapWinGeoProc.
    ' This also uses the new Projective class in MapWinGeoProc 
    ' and the ProjectImage function stored there
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 11/04/2006    Ted           Created initial function
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Friend Sub DoRectifyToWorldfile(ByVal m_map As MapWindow.Interfaces.IMapWin)
        Dim Errors As Boolean = False
        Dim imageFinder As New frmSelectImages
        imageFinder.ShowDialog(g_MapWindowForm)

        If g_Images Is Nothing Then Return

        If g_Images.Count > 0 Then

            For j As Integer = 0 To g_Images.Count - 1
                Errors = False
                Try

                    Try
                        If System.IO.File.Exists(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt") Then
                            System.IO.File.Delete(System.IO.File.Exists(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt"))
                        End If
                    Catch ex As Exception
                    End Try

                    Try
                        CType(g_Images(j), MapWinGIS.Image).Close()
                    Catch
                    End Try

                    Dim origFilename As String = CType(g_Images(j), MapWinGIS.Image).Filename
                    Dim newFilename As String = System.IO.Path.ChangeExtension(origFilename, "")
                    If newFilename.EndsWith(".") Then newFilename = newFilename.Substring(0, newFilename.Length - 1)
                    newFilename += "_Rect" + System.IO.Path.GetExtension(origFilename)

                    Dim ProgressForm As New frmProgress
                    ProgressForm.Owner = g_MapWindowForm
                    ProgressForm.StartPosition = Windows.Forms.FormStartPosition.CenterScreen
                    ProgressForm.Show()
                    ProgressForm.Taskname = "Rectifying..."
                    ProgressForm.Filename = CType(g_Images(j), MapWinGIS.Image).Filename

                    MapWinGeoProc.SpatialReference.RectifyToWorldFile(origFilename, newFilename, CType(ProgressForm, MapWinGIS.ICallback))

                    ProgressForm.CurrentStep += 1
                    ProgressForm.Close()
                    ProgressForm.Dispose()
                    ProgressForm = Nothing

                    Dim newImg As New MapWinGIS.Image
                    newImg.Open(newFilename)

                    g_Images(j) = newImg

                    If Not CType(g_Images(j), MapWinGIS.Image).Filename = "" Then
                        If System.IO.File.Exists(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt") Then
                            Errors = True
                            MapWinUtility.Logger.Msg("One or more errors occurred during rectification. Please see the file:" + vbCrLf + System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt" + vbCrLf + "for more information.")
                        End If
                    End If

                    CType(g_Images(j), MapWinGIS.Image).Close()

                    If Not Errors And System.IO.File.Exists(newFilename) Then
                        If MapWinUtility.Logger.Msg("The reprojection of " + System.IO.Path.GetFileName(origFilename) + " has completed." + vbCrLf + vbCrLf + "The reprojected file is called " + System.IO.Path.GetFileName(newFilename) + "." + vbCrLf + vbCrLf + "Add it to the map now?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Finished! Add to map?") = MsgBoxResult.Yes Then
                            m_Map.Layers.Add(newFilename)
                        End If
                    End If
                Catch ex As Exception
                    'If Not g_Images(j).Filename = "" Then
                    Windows.Forms.MessageBox.Show("An error occurred while reprojecting the grid(s): " + vbCrLf + vbCrLf + "The error text is:" + vbCrLf + ex.ToString())
                    'If System.IO.File.Exists(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt") Then
                    'Errors = True
                    'mapwinutility.logger.msg("One or more errors occurred during reprojection. Please see the file:" + vbCrLf + System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename) + IIf(System.IO.Path.GetDirectoryName(CType(g_Images(j), MapWinGIS.Image).Filename).EndsWith("\"), "", "\") + "ErrorLog.txt" + vbCrLf + "for more information.")
                    'End If
                    'End If
                End Try

skip1:
            Next
        End If

        Cleanup()
    End Sub
End Module
