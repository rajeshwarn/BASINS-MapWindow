Imports System.Windows.Forms

Public Class frm3Dto2D
    Inherits System.Windows.Forms.Form

    Public Sub New()
        InitializeComponent()
    End Sub

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnBrowseInput As System.Windows.Forms.Button
    Friend WithEvents txtBoxOutput As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowseOutput As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtBoxInput As System.Windows.Forms.TextBox

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm3Dto2D))
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnBrowseInput = New System.Windows.Forms.Button
        Me.txtBoxOutput = New System.Windows.Forms.TextBox
        Me.btnBrowseOutput = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtBoxInput = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(338, 122)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Input Shapefile Z"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnBrowseInput
        '
        Me.btnBrowseInput.Image = CType(resources.GetObject("btnBrowseInput.Image"), System.Drawing.Image)
        Me.btnBrowseInput.Location = New System.Drawing.Point(390, 30)
        Me.btnBrowseInput.Name = "btnBrowseInput"
        Me.btnBrowseInput.Size = New System.Drawing.Size(23, 21)
        Me.btnBrowseInput.TabIndex = 3
        Me.btnBrowseInput.UseVisualStyleBackColor = True
        '
        'txtBoxOutput
        '
        Me.txtBoxOutput.Location = New System.Drawing.Point(16, 87)
        Me.txtBoxOutput.Name = "txtBoxOutput"
        Me.txtBoxOutput.ReadOnly = True
        Me.txtBoxOutput.Size = New System.Drawing.Size(368, 20)
        Me.txtBoxOutput.TabIndex = 4
        '
        'btnBrowseOutput
        '
        Me.btnBrowseOutput.Image = CType(resources.GetObject("btnBrowseOutput.Image"), System.Drawing.Image)
        Me.btnBrowseOutput.Location = New System.Drawing.Point(390, 86)
        Me.btnBrowseOutput.Name = "btnBrowseOutput"
        Me.btnBrowseOutput.Size = New System.Drawing.Size(23, 21)
        Me.btnBrowseOutput.TabIndex = 5
        Me.btnBrowseOutput.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Output Shapefile"
        '
        'txtBoxInput
        '
        Me.txtBoxInput.Location = New System.Drawing.Point(16, 31)
        Me.txtBoxInput.Name = "txtBoxInput"
        Me.txtBoxInput.ReadOnly = True
        Me.txtBoxInput.Size = New System.Drawing.Size(368, 20)
        Me.txtBoxInput.TabIndex = 7
        '
        'frm3Dto2D
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(425, 158)
        Me.Controls.Add(Me.txtBoxInput)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnBrowseOutput)
        Me.Controls.Add(Me.txtBoxOutput)
        Me.Controls.Add(Me.btnBrowseInput)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button1)
        Me.Name = "frm3Dto2D"
        Me.Text = "ShapefileZM to Shapefile"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private Sub btnBrowseInput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseInput.Click

        Dim ofd As New System.Windows.Forms.OpenFileDialog()
        ofd.Filter = "Shapefile (*.shp)|*.shp"
        ofd.FilterIndex = 0
        ofd.Multiselect = False
        ofd.AddExtension = True
        ofd.CheckFileExists = True
        If (ofd.ShowDialog(Me) = Windows.Forms.DialogResult.OK) Then
            Dim inputSF As New MapWinGIS.Shapefile()
            If (inputSF.Open(ofd.FileName, Nothing) = False) Then
                System.Windows.Forms.MessageBox.Show("The shapefile failed to open, ensure that the file specified is a valid shapefile Z", "Invalid shapefile", MessageBoxButtons.OK)
                Return
            ElseIf (inputSF.ShapefileType <> MapWinGIS.ShpfileType.SHP_MULTIPOINTZ And inputSF.ShapefileType <> MapWinGIS.ShpfileType.SHP_POINTZ And inputSF.ShapefileType <> MapWinGIS.ShpfileType.SHP_POLYGONZ And inputSF.ShapefileType <> MapWinGIS.ShpfileType.SHP_POLYLINEZ) Then
                System.Windows.Forms.MessageBox.Show("The shapefile failed to open, ensure that the file specified is a valid shapefile Z", "Invalid shapefile", MessageBoxButtons.OK)
                inputSF.Close()
                Return
            Else
                txtBoxInput.Text = ofd.FileName
                txtBoxOutput.Text = System.IO.Path.GetDirectoryName(ofd.FileName) + System.IO.Path.GetFileNameWithoutExtension(ofd.FileName) + "_flat" + System.IO.Path.GetExtension(ofd.FileName)
            End If
        End If

    End Sub

    Private Sub btnBrowseOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseOutput.Click
        Dim sfd As New System.Windows.Forms.SaveFileDialog()
        sfd.Filter = "Shapefile (*.shp)|*.shp"
        sfd.FilterIndex = 0
        sfd.AddExtension = True
        sfd.CheckFileExists = True
        If (sfd.ShowDialog(Me) = Windows.Forms.DialogResult.OK) Then
            txtBoxOutput.Text = sfd.FileName
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim inputFilename As String = txtBoxInput.Text
        Dim outputFilename As String = txtBoxOutput.Text
        Dim converter As New MapWinGeoProc.ShapefileXYZtoXY()

        Try
            If (converter.Convert(inputFilename, outputFilename)) Then
                MessageBox.Show("Conversion successful.")
            End If
        Catch ex As Exception
            MessageBox.Show("Error converting shapefile: " + inputFilename + vbCrLf + "Error: " + ex.ToString(), "Error", MessageBoxButtons.OK)
        End Try

    End Sub
End Class