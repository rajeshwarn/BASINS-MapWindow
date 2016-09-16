
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Linq
Imports MapWinUtility
Imports MapWindow.Interfaces
Imports MapWinGIS
Imports MapWindow.Controls.Projections

Module ProjectionTools

    ''' <summary>
    ''' Assigns projection to the shapefile (no reproject is done)
    ''' </summary>
    ''' <param name="m_Map">Reference to MapWindow</param>
    ''' <remarks></remarks>
    Friend Sub DoApplySFProj(ByVal m_Map As MapWindow.Interfaces.IMapWin)
        Dim OLD_CODE As Boolean = False

        If OLD_CODE Then
            Dim Errors As Boolean = False
            Dim shpfileFinder As New Windows.Forms.OpenFileDialog
            shpfileFinder.Filter = "Shapefiles (*.shp)|*.shp"
            shpfileFinder.Multiselect = True
            shpfileFinder.ShowDialog(g_MapWindowForm)

            If shpfileFinder.FileNames.Length > 0 Then

                Dim defaultProjection As String = ""
                'Open the first SF and get it's projection, just to try to give
                'the user the current projection.
                Try
                    If (System.IO.File.Exists(shpfileFinder.FileNames(0))) Then
                        Dim gp_sf As New MapWinGIS.Shapefile
                        gp_sf.Open(shpfileFinder.FileNames(0))
                        defaultProjection = gp_sf.Projection
                        gp_sf.Close()
                    End If
                Catch
                    'Noncritical; this is for user friendliness
                End Try


                Dim selectedProjection As String = m_Map.GetProjectionFromUser("Please select the projection to be applied.", defaultProjection)
                If Not selectedProjection = "" Then
                    Dim filen As String
                    For Each filen In shpfileFinder.FileNames
                        If (System.IO.File.Exists(filen)) Then
                            Try
                                Dim sf As New MapWinGIS.Shapefile
                                sf.Open(filen, Nothing)
                                sf.Projection = selectedProjection
                                sf.Close()
                            Catch ex As Exception
                                Errors = True
                                Windows.Forms.MessageBox.Show("An error occurred while applying the specified projection to: " + vbCrLf + filen + vbCrLf + vbCrLf + "The error text is:" + vbCrLf + ex.ToString())
                            End Try
                        End If
                    Next

                    If Not Errors Then MapWinUtility.Logger.Msg("The projection(s) have been applied.", MsgBoxStyle.Information, "Finished")
                End If

            End If

            Cleanup()
        Else
            'Dim layers As IEnumerable(Of Layer) = m_Map.Layers.Where(Function(lyr) _
            '                                         lyr.LayerType = eLayerType.LineShapefile Or _
            '                                         lyr.LayerType = eLayerType.PointShapefile Or _
            '                                         lyr.LayerType = eLayerType.PolygonShapefile)

            'Dim list As New ArrayList
            'list.Add("Open dialog: w/o projection")
            'list.Add("Open dialog: all")

            'If layers.Count > 0 Then
            '    ' suggest to add from project only if there are shapefiles there
            '    list.Add("From project: w/o projection")
            '    list.Add("From project: all")
            'End If

            'Dim filenames As New List(Of String)

            'Dim index As Integer = MapWindow.Controls.Dialogs.ChooseOptions(list, 1, _
            '                       "Choose the way to select shapefiles", "Choose files")
            'If index = -1 Then
            '    Return
            'Else
            '    If index = 0 Or index = 1 Then
            '        Dim dlg As New OpenFileDialog
            '        dlg.Filter = (New MapWinGIS.Shapefile()).CdlgFilter
            '        dlg.Multiselect = True
            '        If dlg.ShowDialog(g_MapWindowForm) = DialogResult.OK Then
            '            If index = 1 Then
            '                ' DIALOG: all
            '                filenames = dlg.FileNames.ToList()
            '            Else
            '                ' DIALOG: without projection
            '                Dim sf As New MapWinGIS.Shapefile
            '                For Each name As String In filenames
            '                    If sf.Open(name, Nothing) Then
            '                        If sf.GeoProjection.IsEmpty Then
            '                            filenames.Add(name)
            '                        End If
            '                        sf.Close()
            '                    End If
            '                Next
            '            End If
            '        Else
            '            Return
            '        End If
            '    Else
            '        ' copying from layers
            '        For Each layer As Layer In layers
            '            Dim sf As Shapefile = DirectCast(layer.GetObject(), MapWinGIS.Shapefile)
            '            If Not sf Is Nothing AndAlso sf.SourceType = tkShapefileSourceType.sstDiskBased Then
            '                If (index = 2 AndAlso sf.GeoProjection.IsEmpty) Or index = 3 Then
            '                    filenames.Add(sf.Filename)
            '                End If
            '            End If
            '        Next
            '    End If

            '    If (filenames.Count = 0) Then
            '        MessageBox.Show("No shapefiles without projection were found", m_Map.ApplicationInfo.ApplicationName, _
            '                        MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    Else
            '        Dim arr() As String = filenames.ToArray()

            Dim form As frmAssignProjection = New MapWindow.Controls.Projections.frmAssignProjection(m_Map)
            If (form.ShowDialog(g_MapWindowForm) = Windows.Forms.DialogResult.OK) Then
                ' do something
            End If
            'End If
            'End If
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: DoSFReproject
    ' AUTHOR: Chris Michaelis
    ' DESCRIPTION: Method to Reproject shapefile
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 11/03/2005    ARA             Added Header
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Friend Sub DoSFReproject(ByVal m_map As MapWindow.Interfaces.IMapWin)
        Dim Errors As Boolean = False
        Dim shpfileFinder As New Windows.Forms.OpenFileDialog
        shpfileFinder.Filter = "Shapefiles (*.shp)|*.shp"
        shpfileFinder.Multiselect = True
        shpfileFinder.ShowDialog(g_MapWindowForm)

        If shpfileFinder.FileNames.Length > 0 Then
            Dim defaultPRojection As String = ""
            'Open the first SF and get it's projection, just to try to give
            'the user the current projection.
            Try
                If (System.IO.File.Exists(shpfileFinder.FileNames(0))) Then
                    Dim gp_sf As New MapWinGIS.Shapefile
                    gp_sf.Open(shpfileFinder.FileNames(0))
                    Try
                        defaultPRojection = gp_sf.Projection
                    Catch
                        defaultPRojection = ""
                    End Try
                    gp_sf.Close()
                End If
            Catch
                'Noncritical; this is for user friendliness
            End Try

            Dim selectedProjection As String = m_map.GetProjectionFromUser("Please select the projection to be applied.", defaultPRojection)

            If Not selectedProjection = "" Then
                Dim filen As String
                For Each filen In shpfileFinder.FileNames
                    Errors = False

                    Try
                        If System.IO.File.Exists(System.IO.Path.GetDirectoryName(filen) + IIf(System.IO.Path.GetDirectoryName(filen).EndsWith("\"), "", "\") + "ErrorLog.txt") Then
                            System.IO.File.Delete(System.IO.File.Exists(System.IO.Path.GetDirectoryName(filen) + IIf(System.IO.Path.GetDirectoryName(filen).EndsWith("\"), "", "\") + "ErrorLog.txt"))
                        End If
                    Catch ex As Exception
                    End Try

                    If (System.IO.File.Exists(filen)) Then
                        Try
                            Dim sf As New MapWinGIS.Shapefile
                            sf.Open(filen)

                            Dim oldProjection As String = ""
                            Try
                                oldProjection = sf.Projection
                            Catch
                                oldProjection = ""
                            End Try

                            sf.Close()


                            If (oldProjection Is Nothing OrElse oldProjection = "" OrElse Not oldProjection.StartsWith("+")) Then
                                'mapwinutility.logger.msg("One of the grids you're attempting to project does not currently have a projection. In order to reproject this file, you must use the following dialog to select the current grid projection." + vbCrLf + vbCrLf + "Please choose the current projection of: " + origFilename, MsgBoxStyle.Exclamation, "No Projection for " + System.IO.Path.GetFileName(origFilename))
                                oldProjection = m_map.GetProjectionFromUser("The current projection of the file " + System.IO.Path.GetFileName(filen) + " cannot be determined." + vbCrLf + "Please select the current projection of the file.", "")

                                'Give up if they fail to provide a projection
                                If oldProjection = "" Then
                                    Errors = True 'prevent 'success' message
                                    GoTo skip1
                                End If
                            End If

                            Dim newFilename As String = System.IO.Path.ChangeExtension(filen, "")
                            If newFilename.EndsWith(".") Then newFilename = newFilename.Substring(0, newFilename.Length - 1)
                            newFilename += "_Reprojected" + System.IO.Path.GetExtension(filen)
                            MapWinGeoProc.SpatialReference.ProjectShapefile(oldProjection, selectedProjection, filen, newFilename)

                            Dim newsf As New MapWinGIS.Shapefile
                            newsf.Open(newFilename)
                            newsf.Projection = selectedProjection

                            newsf.Close()

                            If Not filen = "" Then
                                If System.IO.File.Exists(System.IO.Path.GetDirectoryName(filen) + IIf(System.IO.Path.GetDirectoryName(filen).EndsWith("\"), "", "\") + "ErrorLog.txt") Then
                                    Errors = True
                                    MapWinUtility.Logger.Msg("One or more errors occurred during reprojection. Please see the file:" + vbCrLf + System.IO.Path.GetDirectoryName(filen) + IIf(System.IO.Path.GetDirectoryName(filen).EndsWith("\"), "", "\") + "ErrorLog.txt" + vbCrLf + "for more information.")
                                End If
                            End If

                            If Not Errors And System.IO.File.Exists(newFilename) Then
                                If MapWinUtility.Logger.Msg("The reprojection of " + System.IO.Path.GetFileName(filen) + " has completed." + vbCrLf + vbCrLf + "The reprojected file is called " + System.IO.Path.GetFileName(newFilename) + "." + vbCrLf + vbCrLf + "Add it to the map now?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Finished! Add to map?") = MsgBoxResult.Yes Then
                                    m_map.Layers.Add(newFilename)
                                End If
                            End If
                        Catch ex As Exception
                            'Windows.Forms.MessageBox.Show("An error occurred while reprojecting the shapefile: " + vbCrLf + filen + vbCrLf + vbCrLf + "The error text is:" + vbCrLf + ex.ToString())
                            If Not filen = "" Then
                                If System.IO.File.Exists(System.IO.Path.GetDirectoryName(filen) + IIf(System.IO.Path.GetDirectoryName(filen).EndsWith("\"), "", "\") + "ErrorLog.txt") Then
                                    Errors = True
                                    MapWinUtility.Logger.Msg("One or more errors occurred during reprojection. Please see the file:" + vbCrLf + System.IO.Path.GetDirectoryName(filen) + IIf(System.IO.Path.GetDirectoryName(filen).EndsWith("\"), "", "\") + "ErrorLog.txt" + vbCrLf + "for more information.")
                                End If
                            End If
                        End Try
                    End If
skip1:
                Next
            End If
        End If

        Cleanup()
    End Sub
End Module
