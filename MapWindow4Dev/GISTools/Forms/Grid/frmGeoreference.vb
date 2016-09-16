'********************************************************************************************************
'File Name: frmGeoreference.vb
'Description: Georeferences grid and image objects, creating associated world files and rectifying them northward. 
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source. 
'
'The Initial Developer of this version of the Original Code is Christopher Michaelis using portions created by 
'Shade1974 of the MapWindow forum.
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'11/21/2005 - Chris M - Added frmGeoreference
'********************************************************************************************************
Imports System.Drawing
Imports System.Windows.Forms

Public Class frmGeoreference
    Inherits System.Windows.Forms.Form

    Dim workingFilename As String = ""
    Dim imgheight As Double

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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents chbAutoRectify As System.Windows.Forms.CheckBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents internalMap As AxMapWinGIS.AxMap
    Friend WithEvents pt1X As System.Windows.Forms.TextBox
    Friend WithEvents pt1Y As System.Windows.Forms.TextBox
    Friend WithEvents pt1Long As System.Windows.Forms.TextBox
    Friend WithEvents pt1Lat As System.Windows.Forms.TextBox
    Friend WithEvents pt2Y As System.Windows.Forms.TextBox
    Friend WithEvents pt2Lat As System.Windows.Forms.TextBox
    Friend WithEvents pt2Long As System.Windows.Forms.TextBox
    Friend WithEvents pt2X As System.Windows.Forms.TextBox
    Friend WithEvents pt3Y As System.Windows.Forms.TextBox
    Friend WithEvents pt3Lat As System.Windows.Forms.TextBox
    Friend WithEvents pt3Long As System.Windows.Forms.TextBox
    Friend WithEvents pt3X As System.Windows.Forms.TextBox
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents rdZoomIn As System.Windows.Forms.RadioButton
    Friend WithEvents rdZoomOut As System.Windows.Forms.RadioButton
    Friend WithEvents rdPan As System.Windows.Forms.RadioButton
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents rdSelectPt As System.Windows.Forms.RadioButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGeoreference))
        Me.internalMap = New AxMapWinGIS.AxMap
        Me.pt1X = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.pt1Y = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.pt1Long = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.pt1Lat = New System.Windows.Forms.TextBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.pt2Y = New System.Windows.Forms.TextBox
        Me.pt2Lat = New System.Windows.Forms.TextBox
        Me.pt2Long = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.pt2X = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.pt3Y = New System.Windows.Forms.TextBox
        Me.pt3Lat = New System.Windows.Forms.TextBox
        Me.pt3Long = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.pt3X = New System.Windows.Forms.TextBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Button3 = New System.Windows.Forms.Button
        Me.chbAutoRectify = New System.Windows.Forms.CheckBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button4 = New System.Windows.Forms.Button
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
        Me.rdZoomIn = New System.Windows.Forms.RadioButton
        Me.rdZoomOut = New System.Windows.Forms.RadioButton
        Me.rdPan = New System.Windows.Forms.RadioButton
        Me.rdSelectPt = New System.Windows.Forms.RadioButton
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        CType(Me.internalMap, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'internalMap
        '
        resources.ApplyResources(Me.internalMap, "internalMap")
        Me.internalMap.Name = "internalMap"
        Me.internalMap.OcxState = CType(resources.GetObject("internalMap.OcxState"), System.Windows.Forms.AxHost.State)
        '
        'pt1X
        '
        resources.ApplyResources(Me.pt1X, "pt1X")
        Me.pt1X.Name = "pt1X"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'pt1Y
        '
        resources.ApplyResources(Me.pt1Y, "pt1Y")
        Me.pt1Y.Name = "pt1Y"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'pt1Long
        '
        resources.ApplyResources(Me.pt1Long, "pt1Long")
        Me.pt1Long.Name = "pt1Long"
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        '
        'pt1Lat
        '
        resources.ApplyResources(Me.pt1Lat, "pt1Lat")
        Me.pt1Lat.Name = "pt1Lat"
        '
        'GroupBox1
        '
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.pt1Y)
        Me.GroupBox1.Controls.Add(Me.pt1Lat)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.pt1Long)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.pt1X)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'GroupBox2
        '
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.pt2Y)
        Me.GroupBox2.Controls.Add(Me.pt2Lat)
        Me.GroupBox2.Controls.Add(Me.pt2Long)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.pt2X)
        Me.GroupBox2.Controls.Add(Me.Label13)
        Me.GroupBox2.Controls.Add(Me.Label16)
        Me.GroupBox2.Controls.Add(Me.Label18)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        '
        'pt2Y
        '
        resources.ApplyResources(Me.pt2Y, "pt2Y")
        Me.pt2Y.Name = "pt2Y"
        '
        'pt2Lat
        '
        resources.ApplyResources(Me.pt2Lat, "pt2Lat")
        Me.pt2Lat.Name = "pt2Lat"
        '
        'pt2Long
        '
        resources.ApplyResources(Me.pt2Long, "pt2Long")
        Me.pt2Long.Name = "pt2Long"
        '
        'Label11
        '
        resources.ApplyResources(Me.Label11, "Label11")
        Me.Label11.Name = "Label11"
        '
        'pt2X
        '
        resources.ApplyResources(Me.pt2X, "pt2X")
        Me.pt2X.Name = "pt2X"
        '
        'Label13
        '
        resources.ApplyResources(Me.Label13, "Label13")
        Me.Label13.Name = "Label13"
        '
        'Label16
        '
        resources.ApplyResources(Me.Label16, "Label16")
        Me.Label16.Name = "Label16"
        '
        'Label18
        '
        resources.ApplyResources(Me.Label18, "Label18")
        Me.Label18.Name = "Label18"
        '
        'GroupBox3
        '
        resources.ApplyResources(Me.GroupBox3, "GroupBox3")
        Me.GroupBox3.Controls.Add(Me.Label14)
        Me.GroupBox3.Controls.Add(Me.Label15)
        Me.GroupBox3.Controls.Add(Me.pt3Y)
        Me.GroupBox3.Controls.Add(Me.pt3Lat)
        Me.GroupBox3.Controls.Add(Me.pt3Long)
        Me.GroupBox3.Controls.Add(Me.Label17)
        Me.GroupBox3.Controls.Add(Me.pt3X)
        Me.GroupBox3.Controls.Add(Me.Label19)
        Me.GroupBox3.Controls.Add(Me.Label10)
        Me.GroupBox3.Controls.Add(Me.Label12)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.TabStop = False
        '
        'Label14
        '
        resources.ApplyResources(Me.Label14, "Label14")
        Me.Label14.Name = "Label14"
        '
        'Label15
        '
        resources.ApplyResources(Me.Label15, "Label15")
        Me.Label15.Name = "Label15"
        '
        'pt3Y
        '
        resources.ApplyResources(Me.pt3Y, "pt3Y")
        Me.pt3Y.Name = "pt3Y"
        '
        'pt3Lat
        '
        resources.ApplyResources(Me.pt3Lat, "pt3Lat")
        Me.pt3Lat.Name = "pt3Lat"
        '
        'pt3Long
        '
        resources.ApplyResources(Me.pt3Long, "pt3Long")
        Me.pt3Long.Name = "pt3Long"
        '
        'Label17
        '
        resources.ApplyResources(Me.Label17, "Label17")
        Me.Label17.Name = "Label17"
        '
        'pt3X
        '
        resources.ApplyResources(Me.pt3X, "pt3X")
        Me.pt3X.Name = "pt3X"
        '
        'Label19
        '
        resources.ApplyResources(Me.Label19, "Label19")
        Me.Label19.Name = "Label19"
        '
        'Label10
        '
        resources.ApplyResources(Me.Label10, "Label10")
        Me.Label10.Name = "Label10"
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.Name = "Label12"
        '
        'Button3
        '
        resources.ApplyResources(Me.Button3, "Button3")
        Me.Button3.Name = "Button3"
        '
        'chbAutoRectify
        '
        resources.ApplyResources(Me.chbAutoRectify, "chbAutoRectify")
        Me.chbAutoRectify.Checked = True
        Me.chbAutoRectify.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chbAutoRectify.Name = "chbAutoRectify"
        '
        'Button2
        '
        resources.ApplyResources(Me.Button2, "Button2")
        Me.Button2.Name = "Button2"
        '
        'Button1
        '
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.Name = "Button1"
        '
        'Button4
        '
        resources.ApplyResources(Me.Button4, "Button4")
        Me.Button4.Name = "Button4"
        '
        'LinkLabel1
        '
        resources.ApplyResources(Me.LinkLabel1, "LinkLabel1")
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.TabStop = True
        '
        'rdZoomIn
        '
        resources.ApplyResources(Me.rdZoomIn, "rdZoomIn")
        Me.rdZoomIn.Name = "rdZoomIn"
        '
        'rdZoomOut
        '
        resources.ApplyResources(Me.rdZoomOut, "rdZoomOut")
        Me.rdZoomOut.Name = "rdZoomOut"
        '
        'rdPan
        '
        resources.ApplyResources(Me.rdPan, "rdPan")
        Me.rdPan.Name = "rdPan"
        '
        'rdSelectPt
        '
        resources.ApplyResources(Me.rdSelectPt, "rdSelectPt")
        Me.rdSelectPt.Name = "rdSelectPt"
        '
        'Label20
        '
        resources.ApplyResources(Me.Label20, "Label20")
        Me.Label20.Name = "Label20"
        '
        'Label21
        '
        resources.ApplyResources(Me.Label21, "Label21")
        Me.Label21.Name = "Label21"
        '
        'Label22
        '
        resources.ApplyResources(Me.Label22, "Label22")
        Me.Label22.Name = "Label22"
        '
        'frmGeoreference
        '
        resources.ApplyResources(Me, "$this")
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.rdSelectPt)
        Me.Controls.Add(Me.rdPan)
        Me.Controls.Add(Me.rdZoomOut)
        Me.Controls.Add(Me.rdZoomIn)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.chbAutoRectify)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.internalMap)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label22)
        Me.Name = "frmGeoreference"
        CType(Me.internalMap, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Function Georeference(ByVal Point1_Img_X As Double, ByVal Point2_Img_X As Double, ByVal Point3_Img_X As Double, ByVal Point1_Img_Y As Double, ByVal Point2_Img_Y As Double, ByVal Point3_Img_Y As Double, ByVal Point1_Lat As Double, ByVal Point2_Lat As Double, ByVal Point3_Lat As Double, ByVal Point1_Long As Double, ByVal Point2_Long As Double, ByVal Point3_Long As Double) As Double()
        'This function creates the text for the world file based on the three points
        'Derived from the Affine Transform and using a little algebra
        'The result should be 6 coefficients
        'A
        'B
        'C
        'D
        'E
        'F
        'Longitude = A * X + C * Y + E
        'Latitude = B * X + D * Y + F
        'So with 3 points, we have 6 equations and 6 unknowns
        'Separate the latitude and longitude equations, and you have two systems with 3 equations and 3 unknowns

        Dim dA, dB, dC, dD, dE, dF As Double
        Dim ln1, ln2, ln3, X1, X2, X3 As Double
        Dim lat1, lat2, lat3, Y1, Y2, Y3 As Double
        Dim gamma As Double

        'You could obtain these values using any method, but my values are in textboxes
        ln1 = Val(Point1_Long)
        ln2 = Val(Point2_Long)
        ln3 = Val(Point3_Long)
        X1 = Val(Point1_Img_X)
        X2 = Val(Point2_Img_X)
        X3 = Val(Point3_Img_X)
        lat1 = Val(Point1_Lat)
        lat2 = Val(Point2_Lat)
        lat3 = Val(Point3_Lat)
        Y1 = Val(Point1_Img_Y)
        Y2 = Val(Point2_Img_Y)
        Y3 = Val(Point3_Img_Y)

        'Prevent possible division by 0

        If (Y2 - Y3) = 0 Then
            gamma = 0
        Else
            gamma = (Y1 - Y3) / (Y2 - Y3)
        End If

        'Longitude based Coefficients

        dA = (ln1 - ln3 - gamma * (ln2 - ln3)) / (X1 - X3 + gamma * (X3 - X2))

        If Y2 - Y3 = 0 Then 'Prevent division by 0
            dC = ln2 - ln3
        Else
            dC = (ln2 - ln3 + dA * (X3 - X2)) / (Y2 - Y3)
        End If

        dE = ln3 - dA * X3 - dC * Y3

        'Latitude based Coefficients

        dB = (lat1 - lat3 - gamma * (lat2 - lat3)) / (X1 - X3 + gamma * (X3 - X2))

        If Y2 - Y3 = 0 Then 'Prevent division by 0
            dD = 0
        Else
            dD = (lat2 - lat3 + dB * (X3 - X2)) / (Y2 - Y3)
        End If

        dF = lat3 - dB * X3 - dD * Y3

        Dim retVal(5) As Double

        retVal(0) = dA
        retVal(1) = dB
        retVal(2) = dC
        retVal(3) = -dD
        retVal(4) = dE
        retVal(5) = dF + (dD * CDbl(imgheight))

        Return retVal
    End Function

    Private Function RectifyNonzeroRotation(ByRef imgFilename As String, ByVal Coeffs As Double()) As Double()
        Try
            Dim img As New System.Drawing.Bitmap(imgFilename)

            If img Is Nothing Then
                mapwinutility.logger.msg("Unable to rectify the image so that north is up. This could be because it's an advanced image format, e.g. ECW or SID. Georeferencing with rotation or skew will be possible however.", MsgBoxStyle.Exclamation, "Unable to Rectify")
                Return Nothing
            End If

            If Not Coeffs.Length = 6 Then
                mapwinutility.logger.msg("Too few or too many coefficients. Should have 5.")
                Return Nothing
            End If

            Return RectifyNonzeroRotation(img, imgFilename, Coeffs(0), Coeffs(1), Coeffs(2), Coeffs(3), Coeffs(4), Coeffs(5))
        Catch ex As Exception
            mapwinutility.logger.dbg("DEBUG: " + ex.ToString())
            mapwinutility.logger.msg("Unable to rectify the image so that north is up. This could be because it's an advanced image format, e.g. ECW or SID. Georeferencing with rotation or skew will be possible however.", MsgBoxStyle.Exclamation, "Unable to Rectify")
            Return Nothing
        End Try
    End Function

    Private Function RectifyNonzeroRotation(ByRef Source As System.Drawing.Bitmap, ByVal outputFilename As String, ByVal dA As Double, ByVal dB As Double, ByVal dC As Double, ByVal dD As Double, ByVal dE As Double, ByVal dF As Double) As Double()
        'dA, dB, dC, dD, dE, dF  -- Hold the previously calculated Affine 
        'Source -- an already loaded but unrectified bitmap
        Dim dX, dY, extraX, extraY As Integer
        Dim Output As Bitmap
        Dim G As Graphics
        Dim M As Drawing2D.Matrix

        If dA = 0 Or dD = 0 Or dE = 0 Or dF = 0 Then
            MessageBox.Show("The georeferencing step does not appear to have completed successfully. Please double-check the coordinates (perhaps a flippex X/Y for Long/Lat?)", "Georeferencing FAiled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return Nothing
        End If

        'We will create a large enough bitmap to hold the entire rotated image
        extraX = Math.Abs(dC / dA * Source.Height)
        extraY = Math.Abs(dB / dD * Source.Width)
        Output = New Bitmap(Source.Width + extraX, Source.Height + extraY)
        G = Graphics.FromImage(Output)

        'If the coefficients are in alphabetical order in the world file:
        '
        'A
        'B
        'C
        'D
        'E
        'F
        '
        'X1 = Ax + Cy + E
        'Y1 = Bx + Dy + F
        '
        'As an example, if you wanted to literally draw the projected image
        'using the affine coordinates, you would use the following Matrix:
        '
        '| A     B     0 |   
        '|               |       
        '| C     D     0 |    
        '|               |      
        '| E     F     1 |    
        '
        'But to correct the image, we just need to rotate and skew,
        'specified by B and C.  However, since our B and C terms are in
        'world coordinates, we need to use A and D in order to calculate 
        'the skew/rotation in pixel coordinates
        '
        '|   1        B/D          0      |
        '|                                |
        '|   C/A       1           0      |
        '|                                |
        '|   dX       dY           1      |
        '
        '
        'Sometimes the corrected image will have some blank space at the top
        'left corner, so we need to account for that in our output image by
        'shifting the target location for the top left corner if necessary.

        'If the bottom is further left than the top left corner, shift the image right
        If dC < 0 And dA > 0 Then dX = extraX
        If dC > 0 And dA < 0 Then dX = -extraX

        'If the right edge is higher up than the top left corner, shift the image down
        If dB > 0 And dD < 0 Then dY = -extraY
        If dB < 0 And dD > 0 Then dY = extraY

        'Create the transform matrix
        M = New Drawing2D.Matrix(1, dB / dD, dC / dA, 1, dX, dY)
        G.Transform = M

        'Draw the transformed of our existing image "Source"
        G.DrawImage(Source, 0, 0, Source.Width + extraX, Source.Height + extraY)

        'Save the rectified image to the original filename with "prj" added to it
        Dim filename As String = System.IO.Path.GetFileNameWithoutExtension(outputFilename) + "_Rectified" + System.IO.Path.GetExtension(outputFilename)
        '.NET isn't smart enough to detect filetype from extension:
        If filename.ToLower().EndsWith(".bmp") Then
            Output.Save(filename, System.Drawing.Imaging.ImageFormat.Bmp)
        ElseIf filename.ToLower().EndsWith(".jpg") Then
            Output.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg)
        ElseIf filename.ToLower().EndsWith(".gif") Then
            Output.Save(filename, System.Drawing.Imaging.ImageFormat.Gif)
        ElseIf filename.ToLower().EndsWith(".png") Then
            Output.Save(filename, System.Drawing.Imaging.ImageFormat.Png)
        ElseIf filename.ToLower().EndsWith(".tif") Or filename.ToLower().EndsWith(".tiff") Then
            Output.Save(filename, System.Drawing.Imaging.ImageFormat.Tiff)
        ElseIf filename.ToLower().EndsWith(".emf") Then
            Output.Save(filename, System.Drawing.Imaging.ImageFormat.Emf)
        ElseIf filename.ToLower().EndsWith(".exif") Then
            Output.Save(filename, System.Drawing.Imaging.ImageFormat.Exif)
        ElseIf filename.ToLower().EndsWith(".wmf") Then
            Output.Save(filename, System.Drawing.Imaging.ImageFormat.Wmf)
        Else
            'Take our chances
            Output.Save(filename)
        End If

        'New rectified coordinates
        dB = 0
        dC = 0
        dE = dE - dX * dA
        dF = dF - dY * dD

        'Textbox showing coordinates
        Dim retVal(5) As Double

        retVal(0) = dA
        retVal(1) = dB
        retVal(2) = dC
        retVal(3) = dD
        retVal(4) = dE
        retVal(5) = dF

        Return retVal
    End Function

    Private Sub CreateWorldFile(ByVal imageFilename As String, ByVal Coeffs As Double())
        'Create the world file for the rectified image
        'Do this using as the extention the first letter of the original image filename,
        'then the last letter of the image, then the letter "w".
        'i.e., for .JPG, the world file will be .JGW
        'for .BMP, it will be .BPW

        Dim outputFilename As String = imageFilename.Substring(0, imageFilename.Length - 2)
        outputFilename += imageFilename.Substring(imageFilename.Length - 1)
        outputFilename += "w"

        Dim sw As System.IO.StreamWriter = System.IO.File.CreateText(outputFilename)
        sw.WriteLine(Coeffs(0).ToString)
        sw.WriteLine(Coeffs(1).ToString)
        sw.WriteLine(Coeffs(2).ToString)
        sw.WriteLine(Coeffs(3).ToString)
        sw.WriteLine(Coeffs(4).ToString)
        sw.WriteLine(Coeffs(5).ToString)
        sw.Close()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        internalMap.RemoveAllLayers()

        Dim img As New MapWinGIS.Image

        Dim [of] As New OpenFileDialog
        [of].Multiselect = False

        Try
            [of].Filter = img.CdlgFilter
        Catch
            [of].Filter = "Image Files (*.*)|*.*"
        End Try

        [of].Title = "Select Image..."
        If [of].ShowDialog(g_MapWindowForm) = System.Windows.Forms.DialogResult.OK And Not [of].FileName = "" Then
            Dim worldFilename As String = [of].FileName.Substring(0, [of].FileName.Length - 2)
            worldFilename += [of].FileName.Substring([of].FileName.Length - 1)
            worldFilename += "w"

            If System.IO.File.Exists(worldFilename) Then
                If mapwinutility.logger.msg("Warning - A world file already exists for this image! Do you wish to delete the world file and proceed to georeference the image?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "World File Exists") = MsgBoxResult.Yes Then
                    System.IO.File.Delete(worldFilename)
                Else
                    Exit Sub
                End If
            End If
            img.Open([of].FileName, MapWinGIS.ImageType.USE_FILE_EXTENSION, False)
            imgheight = img.Height
            internalMap.ZoomToLayer(internalMap.AddLayer(img, True))
            workingFilename = [of].FileName
            rdPan.Enabled = True
            rdZoomIn.Enabled = True
            rdZoomOut.Enabled = True
            rdSelectPt.Enabled = True
            rdSelectPt.Checked = True
            internalMap.MapCursor = MapWinGIS.tkCursor.crsrArrow
            internalMap.CursorMode = MapWinGIS.tkCursorMode.cmNone
            internalMap.SendMouseDown = True
            g_MW.View.MapCursor = MapWinGIS.tkCursor.crsrArrow
            g_MW.View.CursorMode = MapWinGIS.tkCursorMode.cmNone
        End If
    End Sub

    Private Sub internalMap_MouseDownEvent(ByVal sender As System.Object, ByVal e As AxMapWinGIS._DMapEvents_MouseDownEvent) Handles internalMap.MouseDownEvent
        If workingFilename = "" Then
            mapwinutility.logger.msg("Please add an image to the map first.")
            Return
        End If

        Dim x As Double = 0
        Dim y As Double = 0
        internalMap.PixelToProj(e.x, e.y, x, y)
        x = Math.Abs(x)
        y = Math.Abs(y)

        'The returned coordinate is not actually a "projected" coordinate but is rather
        'a pixel location. The e.x and e.y is a MOUSE location on the "squished" image in the
        'view - to get the actual pixel from the image, run it through pixeltoproj. Since
        'it's not georeferenced already, the current reference system is pixels, not a projected
        'system.

        If pt1X.Text = "" Then
            pt1X.Text = x
            pt1Y.Text = y
        ElseIf pt2X.Text = "" Then
            pt2X.Text = x
            pt2Y.Text = y
        ElseIf pt3X.Text = "" Then
            pt3X.Text = x
            pt3Y.Text = y
        Else
            If mapwinutility.logger.msg("You already have three points selected. Do you wish to clear the selected points and start fresh? Answer no to abort this click.", MsgBoxStyle.YesNo + MsgBoxStyle.Information, "Already Have 3 Points") = MsgBoxResult.Yes Then
                pt1X.Text = ""
                pt1Y.Text = ""
                pt2X.Text = ""
                pt2Y.Text = ""
                pt3X.Text = ""
                pt3Y.Text = ""
                pt1Lat.Text = ""
                pt1Long.Text = ""
                pt2Lat.Text = ""
                pt2Long.Text = ""
                pt3Lat.Text = ""
                pt3Long.Text = ""
                'Use their point
                pt1X.Text = x
                pt1Y.Text = y
            End If
        End If
    End Sub

    Public Sub NotifyMapClick(ByVal x As Double, ByVal y As Double)
        If workingFilename = "" Then
            Return
        End If

        If pt1Lat.Text = "" Then
            pt1Long.Text = x
            pt1Lat.Text = y
        ElseIf pt2Lat.Text = "" Then
            pt2Long.Text = x
            pt2Lat.Text = y
        ElseIf pt3Lat.Text = "" Then
            pt3Long.Text = x
            pt3Lat.Text = y
        Else
            pt1Lat.Text = ""
            pt1Long.Text = ""
            pt2Lat.Text = ""
            pt2Long.Text = ""
            pt3Lat.Text = ""
            pt3Long.Text = ""
            'Use their point
            pt1Long.Text = x
            pt1Lat.Text = y
        End If
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        pt1X.Text = ""
        pt1Y.Text = ""
        pt2X.Text = ""
        pt2Y.Text = ""
        pt3X.Text = ""
        pt3Y.Text = ""
        pt1Lat.Text = ""
        pt1Long.Text = ""
        pt2Lat.Text = ""
        pt2Long.Text = ""
        pt3Lat.Text = ""
        pt3Long.Text = ""
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If workingFilename = "" Then
            mapwinutility.logger.msg("Please add an image to the map first.")
            Return
        End If

        If pt1X.Text = "" Or pt1Y.Text = "" Or pt2X.Text = "" Or pt2Y.Text = "" Or pt3X.Text = "" Or pt3Y.Text = "" Then
            mapwinutility.logger.msg("You must have three points identified on the image in order to georeference the image. Please add a total of three points.", MsgBoxStyle.Exclamation, "Insufficient Points")
            Exit Sub
        End If

        If pt1Lat.Text = "" Or pt1Long.Text = "" Or pt2Lat.Text = "" Or pt2Long.Text = "" Or pt3Lat.Text = "" Or pt3Long.Text = "" Or Not IsNumeric(pt1Lat.Text) Or Not IsNumeric(pt1Long.Text) Or Not IsNumeric(pt2Lat.Text) Or Not IsNumeric(pt2Long.Text) Or Not IsNumeric(pt3Lat.Text) Or Not IsNumeric(pt3Long.Text) Then
            mapwinutility.logger.msg("Please enter the known coordinates (projected or lat/long) of the three points selected on the map.", MsgBoxStyle.Exclamation, "Specify Locations")
            Exit Sub
        End If

        Dim Coeffs() As Double = Georeference(Val(pt1X.Text), Val(pt2X.Text), Val(pt3X.Text), Val(pt1Y.Text), Val(pt2Y.Text), Val(pt3Y.Text), Val(pt1Lat.Text), Val(pt2Lat.Text), Val(pt3Lat.Text), Val(pt1Long.Text), Val(pt2Long.Text), Val(pt3Long.Text))
        Dim RectifiedCoeffs() As Double = Nothing

        If (Not Coeffs(1) = 0 Or Not Coeffs(2) = 0) And chbAutoRectify.Checked Then
            RectifiedCoeffs = RectifyNonzeroRotation(workingFilename, Coeffs)
        End If

        If Not RectifiedCoeffs Is Nothing Then
            CreateWorldFile(System.IO.Path.GetFileNameWithoutExtension(workingFilename) + "_Rectified" + System.IO.Path.GetExtension(workingFilename), RectifiedCoeffs)
            MapWinUtility.Logger.Msg("The image has been georeferenced and rectified, and the world file has been written.", MsgBoxStyle.Information, "Success")
        ElseIf Not Coeffs Is Nothing Then
            CreateWorldFile(workingFilename, Coeffs)
            MapWinUtility.Logger.Msg("The image has been georeferenced. Rectification was not necessary or not selected. The world file has been written.", MsgBoxStyle.Information, "Success")
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MapWinUtility.Logger.Msg("You can use this tool to georeference any image that MapWindow supports, including advanced formats like ECW and Mr. Sid." _
        + vbCrLf + vbCrLf + "However, you can only *rectify* images supported by the .NET framework, such as .JPG, .BMP, .GIF, etc." _
        + vbCrLf + vbCrLf + "Therefore, if you are using this tool to georeference advanced image formats please uncheck this box.", MsgBoxStyle.Information, "Warning")
    End Sub

    Private Sub rdSelectPt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdSelectPt.CheckedChanged
        If rdSelectPt.Checked Then
            internalMap.MapCursor = MapWinGIS.tkCursor.crsrArrow
            internalMap.CursorMode = MapWinGIS.tkCursorMode.cmNone
            internalMap.SendMouseDown = True
        End If
    End Sub

    Private Sub rdPan_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdPan.CheckedChanged
        If rdPan.Checked Then
            internalMap.MapCursor = MapWinGIS.tkCursor.crsrCross
            internalMap.CursorMode = MapWinGIS.tkCursorMode.cmPan
            internalMap.SendMouseDown = False
        End If
    End Sub

    Private Sub rdZoomIn_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdZoomIn.CheckedChanged
        If rdZoomIn.Checked Then
            internalMap.MapCursor = MapWinGIS.tkCursor.crsrCross
            internalMap.CursorMode = MapWinGIS.tkCursorMode.cmZoomIn
            internalMap.SendMouseDown = False
        End If
    End Sub

    Private Sub rdZoomOut_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdZoomOut.CheckedChanged
        If rdZoomOut.Checked Then
            internalMap.MapCursor = MapWinGIS.tkCursor.crsrCross
            internalMap.CursorMode = MapWinGIS.tkCursorMode.cmZoomOut
            internalMap.SendMouseDown = False
        End If
    End Sub

    Private Sub frmGeoreference_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim textstate As Boolean = True
        pt1X.Enabled = textstate
        pt1Y.Enabled = textstate
        pt2X.Enabled = textstate
        pt2Y.Enabled = textstate
        pt3X.Enabled = textstate
        pt3Y.Enabled = textstate
        pt1Lat.Enabled = textstate
        pt1Long.Enabled = textstate
        pt2Lat.Enabled = textstate
        pt2Long.Enabled = textstate
        pt3Lat.Enabled = textstate
        pt3Long.Enabled = textstate
    End Sub

    Private Sub internalMap_FileDropped(ByVal sender As Object, ByVal e As AxMapWinGIS._DMapEvents_FileDroppedEvent) Handles internalMap.FileDropped
        Dim g As New MapWinGIS.Grid
        Dim i As New MapWinGIS.Image

        If e.filename.ToLower().EndsWith(".shp") Then
            Dim sf As New MapWinGIS.Shapefile
            If sf.Open(e.filename) Then
                internalMap.AddLayer(sf, True)
            End If
        ElseIf Not g.CdlgFilter.ToLower().IndexOf(System.IO.Path.GetExtension(e.filename)) = -1 Then
            If g.Open(e.filename) Then
                internalMap.AddLayer(g, True)
            End If
        ElseIf Not i.CdlgFilter.ToLower().IndexOf(System.IO.Path.GetExtension(e.filename)) = -1 Then
            If i.Open(e.filename) Then
                internalMap.AddLayer(i, True)
            End If
        End If
    End Sub

    Private Sub Coords_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pt1X.TextChanged, pt1Y.TextChanged, pt1Lat.TextChanged, pt1Long.TextChanged, pt2X.TextChanged, pt2Y.TextChanged, pt2Lat.TextChanged, pt2Long.TextChanged, pt3X.TextChanged, pt3Y.TextChanged, pt3Lat.TextChanged, pt3Long.TextChanged
        If TypeOf sender Is TextBox Then
            Dim tx As TextBox = CType(sender, TextBox)
            If Not tx.Text = "" And Not tx.Text = "." And Not IsNumeric(tx.Text) Then
                MapWinUtility.Logger.Msg("Please enter only numbers for coordinates.", MsgBoxStyle.Exclamation, "Enter Only Numbers")
            End If
        End If
    End Sub
End Class
