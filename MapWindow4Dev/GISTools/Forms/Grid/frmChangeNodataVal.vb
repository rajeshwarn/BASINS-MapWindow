Public Class frmChangeNodataVal

    Private theGrid As New MapWinGIS.Grid

    Private Sub frmChangeNodataVal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PopulateSrc1List()
    End Sub

    Private Sub PopulateSrc1List()
        Dim newlayer As MapWindow.Interfaces.Layer
        Dim i As Integer
        cmbxSrc1.Items.Clear()
        cmbxSrc1.Items.Add("(Select a raster or click the folder icon)")

        If g_MW.Layers.NumLayers > 0 Then
            For i = 0 To g_MW.Layers.NumLayers - 1
                newlayer = g_MW.Layers.Item(g_MW.Layers.GetHandle(i))
                If (newlayer.LayerType = MapWindow.Interfaces.eLayerType.Grid) Then
                    cmbxSrc1.Items.Add(newlayer.FileName)
                End If
            Next
        End If

        cmbxSrc1.SelectedIndex = 0
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        If cmbxSrc1.SelectedIndex = -1 Then
            MapWinUtility.Logger.Msg("Please choose a raster dataset first.", MsgBoxStyle.Information, "Choose a File First")
            Exit Sub
        End If

        If Not IsNumeric(txtNew.Text) Or txtNew.Text = "" Then
            MapWinUtility.Logger.Msg("Please enter only numbers for the new NoData value.", MsgBoxStyle.Exclamation, "Enter Only Numbers")
            Exit Sub
        End If

        theGrid.Header.NodataValue = Double.Parse(txtNew.Text)
        theGrid.Save()

        MapWinUtility.Logger.Msg("The nodata value has been set to: " + txtNew.Text + vbCrLf + vbCrLf + "for " + theGrid.Filename + ".", MsgBoxStyle.Information, "Success!")

        theGrid.Close()
    End Sub

    Private Sub cmbxSrc1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxSrc1.SelectedIndexChanged
        If cmbxSrc1.SelectedIndex = -1 Then Return

        If cmbxSrc1.Items(cmbxSrc1.SelectedIndex).ToString() = "(Select a raster or click the folder icon)" Then Return

        theGrid.Close()
        theGrid.Open(cmbxSrc1.Items(cmbxSrc1.SelectedIndex), MapWinGIS.GridDataType.UnknownDataType, False, MapWinGIS.GridFileType.UseExtension)
        txtOrig.Text = theGrid.Header.NodataValue
        txtNew.Text = txtOrig.Text
        txtNew.Enabled = True
        txtNew.Focus()
    End Sub

    Private Sub btnBrowseSrc1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseSrc1.Click
        Dim strPath As String

        Dim fdiagOpen As System.Windows.Forms.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
        fdiagOpen.Filter = theGrid.CdlgFilter

        If fdiagOpen.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
            strPath = fdiagOpen.FileName
            If System.IO.File.Exists(fdiagOpen.FileName) Then
                cmbxSrc1.Items.Add(fdiagOpen.FileName)
                cmbxSrc1.SelectedIndex = cmbxSrc1.Items.Count - 1
            End If
        End If
    End Sub
End Class