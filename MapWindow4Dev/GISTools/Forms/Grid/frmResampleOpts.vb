Public Class frmResampleOpts
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents lstGrids As System.Windows.Forms.ListView
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblMin As System.Windows.Forms.Label
    Friend WithEvents lblMax As System.Windows.Forms.Label
    Friend WithEvents lblAvg As System.Windows.Forms.Label
    Friend WithEvents lblRecommend As System.Windows.Forms.Label
    Friend WithEvents lblCellSize As System.Windows.Forms.Label
    Friend WithEvents txtCellSize As System.Windows.Forms.TextBox
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmResampleOpts))
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnNext = New System.Windows.Forms.Button
        Me.lstGrids = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblRecommend = New System.Windows.Forms.Label
        Me.lblAvg = New System.Windows.Forms.Label
        Me.lblMax = New System.Windows.Forms.Label
        Me.lblMin = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblCellSize = New System.Windows.Forms.Label
        Me.txtCellSize = New System.Windows.Forms.TextBox
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
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
        '
        'lstGrids
        '
        Me.lstGrids.AccessibleDescription = Nothing
        Me.lstGrids.AccessibleName = Nothing
        resources.ApplyResources(Me.lstGrids, "lstGrids")
        Me.lstGrids.BackgroundImage = Nothing
        Me.lstGrids.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lstGrids.Font = Nothing
        Me.lstGrids.FullRowSelect = True
        Me.lstGrids.GridLines = True
        Me.lstGrids.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lstGrids.MultiSelect = False
        Me.lstGrids.Name = "lstGrids"
        Me.lstGrids.TabStop = False
        Me.lstGrids.UseCompatibleStateImageBehavior = False
        Me.lstGrids.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        resources.ApplyResources(Me.ColumnHeader1, "ColumnHeader1")
        '
        'ColumnHeader2
        '
        resources.ApplyResources(Me.ColumnHeader2, "ColumnHeader2")
        '
        'GroupBox1
        '
        Me.GroupBox1.AccessibleDescription = Nothing
        Me.GroupBox1.AccessibleName = Nothing
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.BackgroundImage = Nothing
        Me.GroupBox1.Controls.Add(Me.lblRecommend)
        Me.GroupBox1.Controls.Add(Me.lblAvg)
        Me.GroupBox1.Controls.Add(Me.lblMax)
        Me.GroupBox1.Controls.Add(Me.lblMin)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Font = Nothing
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'lblRecommend
        '
        Me.lblRecommend.AccessibleDescription = Nothing
        Me.lblRecommend.AccessibleName = Nothing
        resources.ApplyResources(Me.lblRecommend, "lblRecommend")
        Me.lblRecommend.Font = Nothing
        Me.lblRecommend.Name = "lblRecommend"
        '
        'lblAvg
        '
        Me.lblAvg.AccessibleDescription = Nothing
        Me.lblAvg.AccessibleName = Nothing
        resources.ApplyResources(Me.lblAvg, "lblAvg")
        Me.lblAvg.Font = Nothing
        Me.lblAvg.Name = "lblAvg"
        '
        'lblMax
        '
        Me.lblMax.AccessibleDescription = Nothing
        Me.lblMax.AccessibleName = Nothing
        resources.ApplyResources(Me.lblMax, "lblMax")
        Me.lblMax.Font = Nothing
        Me.lblMax.Name = "lblMax"
        '
        'lblMin
        '
        Me.lblMin.AccessibleDescription = Nothing
        Me.lblMin.AccessibleName = Nothing
        resources.ApplyResources(Me.lblMin, "lblMin")
        Me.lblMin.Font = Nothing
        Me.lblMin.Name = "lblMin"
        '
        'Label3
        '
        Me.Label3.AccessibleDescription = Nothing
        Me.Label3.AccessibleName = Nothing
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Font = Nothing
        Me.Label3.Name = "Label3"
        '
        'Label4
        '
        Me.Label4.AccessibleDescription = Nothing
        Me.Label4.AccessibleName = Nothing
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Font = Nothing
        Me.Label4.Name = "Label4"
        '
        'Label2
        '
        Me.Label2.AccessibleDescription = Nothing
        Me.Label2.AccessibleName = Nothing
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Font = Nothing
        Me.Label2.Name = "Label2"
        '
        'Label1
        '
        Me.Label1.AccessibleDescription = Nothing
        Me.Label1.AccessibleName = Nothing
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Font = Nothing
        Me.Label1.Name = "Label1"
        '
        'lblCellSize
        '
        Me.lblCellSize.AccessibleDescription = Nothing
        Me.lblCellSize.AccessibleName = Nothing
        resources.ApplyResources(Me.lblCellSize, "lblCellSize")
        Me.lblCellSize.Font = Nothing
        Me.lblCellSize.Name = "lblCellSize"
        '
        'txtCellSize
        '
        Me.txtCellSize.AccessibleDescription = Nothing
        Me.txtCellSize.AccessibleName = Nothing
        resources.ApplyResources(Me.txtCellSize, "txtCellSize")
        Me.txtCellSize.BackgroundImage = Nothing
        Me.txtCellSize.Font = Nothing
        Me.txtCellSize.Name = "txtCellSize"
        '
        'frmResampleOpts
        '
        Me.AcceptButton = Me.btnNext
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.CancelButton = Me.btnCancel
        Me.Controls.Add(Me.txtCellSize)
        Me.Controls.Add(Me.lblCellSize)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lstGrids)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnNext)
        Me.Font = Nothing
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmResampleOpts"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private m_factors As ArrayList

    Private Sub Initialize()
        Dim grd As MapWinGIS.Grid
        Dim avg, min, max As Double
        Dim firstRun As Boolean = True

        m_factors = New ArrayList
        lstGrids.Items.Clear()

        For Each grd In g_Grids
            With grd.Header
                If firstRun Then
                    min = .dX
                    max = .dX
                    firstRun = False
                Else
                    If .dX < min Then
                        min = .dX
                    ElseIf .dX > max Then
                        max = .dX
                    End If
                End If
                m_factors.Add(FindFactors(.dX))
                avg += .dX
            End With
            lstGrids.Items.Add(grd.Filename).SubItems.Add(grd.Header.dX)
        Next

        avg /= g_Grids.Count

        Const ddCellSize As Double = 0.0002777777778
        lblAvg.Text = Math.Round(avg, 14)
        lblMax.Text = max
        lblMin.Text = min
        If lblMax.Text > 1 AndAlso lblMin.Text > 1 Then
            lblRecommend.Text = FindGCF()
        Else
            If Math.Round(CDbl(lblMax.Text), 13) = ddCellSize Then
                lblRecommend.Text = ddCellSize
            Else
                lblRecommend.Text = lblAvg.Text
            End If
        End If
        txtCellSize.Text = lblRecommend.Text
    End Sub

    Private Function FindFactors(ByVal CellSize As Integer) As Hashtable
        Dim retval As New Hashtable
        Dim i As Integer

        For i = 2 To CellSize / 2
            If CellSize Mod i = 0 Then
                retval.Add(i, i)
            End If
        Next
        retval.Add(CellSize, CellSize)

        Return retval
    End Function

    Private Function FindGCF() As Integer
        Dim curFactor As Integer = 1
        Dim i, j As Integer, ht As Hashtable
        Dim keys As Integer()
        Dim isCommon As Boolean = True

        If Not m_factors Is Nothing AndAlso m_factors.Count > 0 Then
            ht = m_factors(0)
            ReDim keys(ht.Count - 1)
            ht.Keys.CopyTo(keys, 0)
            For i = 0 To keys.Length - 1
                isCommon = True
                For j = 1 To m_factors.Count - 1
                    ht = m_factors(j)
                    If ht.ContainsKey(keys(i)) = False Then
                        isCommon = False
                    End If
                Next
                If isCommon AndAlso keys(i) > curFactor Then
                    curFactor = keys(i)
                End If
            Next
        End If

        Return curFactor
    End Function

    Private Sub ResampleForm_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        Me.Initialize()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Cleanup()
        Me.Close()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        If txtCellSize.Text Is Nothing OrElse txtCellSize.Text.Length < 1 OrElse (Not IsNumeric(txtCellSize.Text)) Then Exit Sub

        ' calculate the number of cells that will be in the new grid.  If this gets too big, then warn the user.
        Dim l, r, t, b As Double
        Dim grd As MapWinGIS.Grid, first As Boolean = True
        For Each grd In g_Grids
            If first Then
                l = grd.Header.XllCenter - grd.Header.dX / 2
                b = grd.Header.YllCenter - grd.Header.dY / 2
                r = l + grd.Header.dX * grd.Header.NumberCols
                t = b + grd.Header.dY * grd.Header.NumberRows
                first = False
            Else
                l = Math.Min(l, grd.Header.XllCenter - grd.Header.dX / 2)
                b = Math.Min(b, grd.Header.YllCenter - grd.Header.dY / 2)
                r = Math.Max(r, (grd.Header.XllCenter - grd.Header.dX / 2) + grd.Header.dX * grd.Header.NumberCols)
                t = Math.Max(t, (grd.Header.YllCenter - grd.Header.dY / 2) + grd.Header.dY * grd.Header.NumberRows)
            End If
        Next
        Dim numCells As Long = (Math.Abs(r - l) / txtCellSize.Text) * (Math.Abs(t - b) / txtCellSize.Text)
        Dim objWMI As New clsWMI
        Dim maxMemory As Double = objWMI.TotalPhysicalMemory
        objWMI = Nothing
        If numCells * 8 > maxMemory Then
            If MapWinUtility.Logger.Msg("WARNING!  The new grid will have " & Format(numCells, "#,###") & " cells which could create a file of " & Format(Math.Round(numCells * 8) / (2 ^ 20), "#,###") & " MB (or more) which is larger than your physical memory of " & Format(Math.Round(maxMemory / (2 ^ 20)), "#,###") & " MB.  This may cause problems due to insufficient memory.  Do you wish to continue?", MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation, "Grid Wizard") = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        g_NewCellSize = txtCellSize.Text

        Me.Close()
    End Sub

    Private Sub txtCellSize_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtCellSize.Validating
        If txtCellSize.Text < 0 Then
            MapWinUtility.Logger.Msg("Please enter a positive value for the Cell Size.", "Grid Wizard")
            e.Cancel = True
            Exit Sub
        End If

        Dim i As Integer
        For i = 0 To g_Grids.Count - 1
            If txtCellSize.Text > CType(g_Grids(i), MapWinGIS.Grid).Header.NumberRows * CType(g_Grids(i), MapWinGIS.Grid).Header.dX Then
                MapWinUtility.Logger.Msg("Please enter a cell size that is not larger than any of the loaded grids.", "Grid Wizard")
                e.Cancel = True
                Exit Sub
            End If
        Next
    End Sub

End Class
