'********************************************************************************************************
'File Name: frmClipOperation.vb
'Description: performs ovelay operation based on 2 shapefiles
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source GIS Tools Plug-in. 
'
'Contributor(s): (Open source contributors should list themselves and their modifications here).
'03/06/11 - Sergei Leschinski - created original tool
'********************************************************************************************************
Imports System.IO
Imports System.Threading
Imports System.Windows.Forms
Imports MapWindow.Interfaces
Imports MapWinGIS
Imports MapWinUtility

Public Class frmClipOperation
    Inherits clsToolBase

    ' the particalu clipping operation requested by user
    Private _operation As MapWinGIS.tkClipOperation

    ' the full name of the first input shapefile
    Private _name1 As String

    ' the full name of the second input shapefile
    Private _name2 As String

    ''' <summary>
    ''' Creates a new instance of the frmClipOperation class
    ''' </summary>
    Public Sub New(ByVal operation As MapWinGIS.tkClipOperation)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        _operation = operation

        ' Add any initialization after the InitializeComponent() call.
        ShapefileTools.PopulateShapefileList(cboLayer1)
        ShapefileTools.PopulateShapefileList(cboLayer2)

        Select Case operation
            Case MapWinGIS.tkClipOperation.clClip
                Me.Text = "Shapefile Clipping"
                Label1.Text = "Subject shapefile"
                Label2.Text = "Clipping shapefile"
            Case MapWinGIS.tkClipOperation.clDifference
                Me.Text = "Shapefiles Difference"
                Label1.Text = "Subject shapefile"
                Label2.Text = "Clipping shapefile"
            Case MapWinGIS.tkClipOperation.clIntersection
                Me.Text = "Shapefiles Intersection"
                Label1.Text = "Shapefile 1"
                Label2.Text = "Shapefile 2"
            Case MapWinGIS.tkClipOperation.clSymDifference
                Me.Text = "Shapefiles Symmetrical Difference"
                Label1.Text = "Shapefile 1"
                Label2.Text = "Shapefile 2"
            Case MapWinGIS.tkClipOperation.clUnion
                Me.Text = "Shapefiles Union"
                Label1.Text = "Shapefile 1"
                Label2.Text = "Shapefile 2"
        End Select


    End Sub

    ''' <summary>
    ''' Fill the list of fields for the layer
    ''' </summary>
    Private Sub cboLayer_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLayer2.SelectedIndexChanged, cboLayer1.SelectedIndexChanged

        Dim combo As ComboBox = cboLayer1
        Dim lbl As System.Windows.Forms.Label = lblSelected1
        Dim chk As CheckBox = chkSelectedOnly1

        If CType(sender, ComboBox) Is cboLayer2 Then
            combo = cboLayer2
            lbl = lblSelected2
            chk = chkSelectedOnly2
        End If

        lbl.Text = "Number of selected: 0"

        Dim postfix As String = ""
        Select Case (_operation)
            Case MapWinGIS.tkClipOperation.clClip : postfix = "clip"
            Case MapWinGIS.tkClipOperation.clDifference : postfix = "diff"
            Case MapWinGIS.tkClipOperation.clIntersection : postfix = "intsct"
            Case MapWinGIS.tkClipOperation.clSymDifference : postfix = "symdiff"
            Case MapWinGIS.tkClipOperation.clUnion : postfix = "union"
            Case Else : postfix = "clip"
        End Select

        If (combo.Items.Count <= 0) Then Return
        Dim name As String = combo.Text
        Dim sf As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(name)
        If Not sf Is Nothing Then
            lbl.Text = "Number of selected: " & sf.NumSelected
            chk.Enabled = sf.NumSelected > 0
            If combo Is cboLayer1 Then
                txtFilename.Text = ShapefileTools.GetAvailibleFileName(sf.Filename, postfix)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Chooses the file to save the results to
    ''' </summary>
    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        ShapefileTools.ChooseShapefileName(txtFilename)
    End Sub

    ''' <summary>
    ''' Runs th dissolve routine
    ''' </summary>
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If cboLayer1.Text = String.Empty Then
            Logger.Msg("First layer is not selected")
        ElseIf cboLayer2.Text = String.Empty Then
            Logger.Msg("Second layer is not selected")
        ElseIf txtFilename.Text = String.Empty Then
            Logger.Msg("Output filename is not specified")
        ElseIf File.Exists(txtFilename.Text) Then
            Logger.Msg("The filename selected already exists")
        ElseIf cboLayer1.SelectedIndex = cboLayer2.SelectedIndex Then
            Logger.Msg("Two different shapefiles should be set as input")
        Else
            Dim sf1 As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(cboLayer1.Text)
            Dim sf2 As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(cboLayer2.Text)
            If Not sf1 Is Nothing And Not sf2 Is Nothing Then

                _name1 = sf1.Filename
                _name2 = sf2.Filename

                Me.Hide() ': Application.DoEvents()
                'Thread.Sleep(500) ': Application.DoEvents()

                _progress.Text = Me.Text
                _progress.lblSubject.Text = "Subject: " + cboLayer1.Text
                _progress.Show(g_MapWindowForm) : Application.DoEvents()

                _thread = New Thread(AddressOf Me.Execute)
                _thread.SetApartmentState(ApartmentState.STA)
                _thread.IsBackground = True

                _thread.Start()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Executes the operation in the back thread
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub Execute()

        Dim sf1 As New MapWinGIS.Shapefile
        Dim sf2 As New MapWinGIS.Shapefile
        Dim result As MapWinGIS.Shapefile = Nothing

        Dim callback As New Callback(_progress)

        sf1.Open(_name1, callback)
        sf2.Open(_name2, Nothing)

        ' copying selection
        Dim sfSource1 As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(cboLayer1.Text)
        Dim sfSource2 As MapWinGIS.Shapefile = ShapefileTools.ShapefileByLayerName(cboLayer2.Text)

        If sf1.NumShapes = sfSource1.NumShapes Then
            For i As Integer = 0 To sfSource1.NumShapes - 1
                sf1.ShapeSelected(i) = sfSource1.ShapeSelected(i)
            Next
        End If

        If sf2.NumShapes = sfSource2.NumShapes Then
            For i As Integer = 0 To sfSource2.NumShapes - 1
                sf2.ShapeSelected(i) = sfSource2.ShapeSelected(i)
            Next
        End If

        If Not (sf1.SourceType = tkShapefileSourceType.sstDiskBased And sf2.SourceType = tkShapefileSourceType.sstDiskBased) Then Return

        sf1.GeometryEngine = IIf(chkUseClipper.Checked, MapWinGIS.tkGeometryEngine.engineClipper, MapWinGIS.tkGeometryEngine.engineGeos)

        Dim selected1 As Boolean = chkSelectedOnly1.Checked And chkSelectedOnly1.Enabled
        Dim selected2 As Boolean = chkSelectedOnly2.Checked And chkSelectedOnly2.Enabled

        If _operation = tkClipOperation.clClip Then
            result = sf1.Clip(selected1, sf2, selected2)
        ElseIf _operation = tkClipOperation.clDifference Then
            result = sf1.Difference(selected1, sf2, selected2)
        ElseIf _operation = tkClipOperation.clIntersection Then
            result = sf1.GetIntersection(selected1, sf2, selected2, MapWinGIS.ShpfileType.SHP_NULLSHAPE)
        ElseIf _operation = tkClipOperation.clSymDifference Then
            result = sf1.SymmDifference(selected1, sf2, selected2)
        ElseIf _operation = tkClipOperation.clUnion Then
            result = sf1.Union(selected1, sf2, selected2)
        End If

        sf1.Close()
        sf2.Close()

        Dim args(1) As Object
        args(0) = False   ' failure

        If callback.Canceled Then
            args(1) = "The operation was canceled by user"
        Else
            If Not result Is Nothing Then
                If Not result.SaveAs(txtFilename.Text, Nothing) Then
                    args(1) = "An error while writing the resulting file has occured" + vbNewLine + result.ErrorMsg(result.LastErrorCode)   ' error message
                Else
                    args(0) = True   ' success
                End If
                result.Close()
            Else
                args(1) = "No result was returned" + vbNewLine + sf1.ErrorMsg(sf1.LastErrorCode)
            End If
        End If

        _progress.BeginInvoke(New FinishedDelegate(AddressOf Me.TaskFinished), args)

    End Sub

    ''' <summary>
    ''' Handles the finishing of execution
    ''' </summary>
    Protected Overrides Sub TaskFinished(ByVal success As Boolean, ByVal errorMessage As String)
        If Not _progress Is Nothing Then _progress.Close()
        Application.DoEvents()

        If success Then
            If MsgBox("Do you want to add the new layer to the map?", MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                Dim newLayer As Layer = g_MW.Layers.Add(txtFilename.Text)
            End If
        Else
            Logger.Msg(errorMessage)
        End If

        _thread = Nothing : GC.Collect()

        Me.Close()
    End Sub

    ''' <summary>
    ''' Handles the closing of the progress window, hides the current window
    ''' </summary>
    Protected Overrides Sub DoClose(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs)
        ' Whether the the resources of the derived class will be released?
        Me.Close()
    End Sub
End Class