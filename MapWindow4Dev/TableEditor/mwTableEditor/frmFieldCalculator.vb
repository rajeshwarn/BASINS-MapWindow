'Field Calculator tool. Brought in from the USU code base
'during merger by Chris M on April 21 2006.
'
'Extended 11/4/2006 by Chris M to use clsMathParser.vb, see the top of that
'file for sources and more info. This provides dozens more functions and
'logical operators.
'Update 22/12/2009 by Enrico Chiaradia to the new version of clsMathParser.vb v.4.2.1
'Added treeview control to functions list and new AddTextToComputation method
'

Imports System.Windows.Forms
Public Class frmFieldCalculator
    Inherits System.Windows.Forms.Form

#Region "Member Variables"
    Private m_shapefile As MapWinGIS.Shapefile
    Private m_Grid As DataGrid
    Private m_DestFieldName As String = ""
    Private m_DestFieldColumn As Integer
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
    Friend WithEvents lstFunctions As System.Windows.Forms.TreeView
    Private m_parser As New clsMathParser
#End Region

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
    End Sub

    <CLSCompliant(False)> _
    Public Sub New(ByVal shapefile As MapWinGIS.Shapefile, ByVal grid As DataGrid)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        m_shapefile = shapefile
        m_Grid = grid

        InitializeFieldValues()
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
    Friend WithEvents DestFieldComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents FieldsListView As System.Windows.Forms.ListView
    Friend WithEvents FieldsTitleLabel As System.Windows.Forms.Label
    Friend WithEvents DestFieldTitleLabel As System.Windows.Forms.Label
    Friend WithEvents ComputationTextBox As System.Windows.Forms.TextBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents btnSubtract As System.Windows.Forms.Button
    Friend WithEvents btnMultiply As System.Windows.Forms.Button
    Friend WithEvents btnDivide As System.Windows.Forms.Button
    Friend WithEvents btnConcat As System.Windows.Forms.Button
    Friend WithEvents FunctionTitleLabel As System.Windows.Forms.Label
    Friend WithEvents AssignmentLabel As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFieldCalculator))
        Me.DestFieldComboBox = New System.Windows.Forms.ComboBox
        Me.FieldsListView = New System.Windows.Forms.ListView
        Me.FieldsTitleLabel = New System.Windows.Forms.Label
        Me.DestFieldTitleLabel = New System.Windows.Forms.Label
        Me.ComputationTextBox = New System.Windows.Forms.TextBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnAdd = New System.Windows.Forms.Button
        Me.btnSubtract = New System.Windows.Forms.Button
        Me.btnMultiply = New System.Windows.Forms.Button
        Me.btnDivide = New System.Windows.Forms.Button
        Me.btnConcat = New System.Windows.Forms.Button
        Me.FunctionTitleLabel = New System.Windows.Forms.Label
        Me.AssignmentLabel = New System.Windows.Forms.Label
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel
        Me.lstFunctions = New System.Windows.Forms.TreeView
        Me.SuspendLayout()
        '
        'DestFieldComboBox
        '
        resources.ApplyResources(Me.DestFieldComboBox, "DestFieldComboBox")
        Me.DestFieldComboBox.Name = "DestFieldComboBox"
        '
        'FieldsListView
        '
        resources.ApplyResources(Me.FieldsListView, "FieldsListView")
        Me.FieldsListView.Name = "FieldsListView"
        Me.FieldsListView.UseCompatibleStateImageBehavior = False
        Me.FieldsListView.View = System.Windows.Forms.View.List
        '
        'FieldsTitleLabel
        '
        resources.ApplyResources(Me.FieldsTitleLabel, "FieldsTitleLabel")
        Me.FieldsTitleLabel.Name = "FieldsTitleLabel"
        '
        'DestFieldTitleLabel
        '
        resources.ApplyResources(Me.DestFieldTitleLabel, "DestFieldTitleLabel")
        Me.DestFieldTitleLabel.Name = "DestFieldTitleLabel"
        '
        'ComputationTextBox
        '
        resources.ApplyResources(Me.ComputationTextBox, "ComputationTextBox")
        Me.ComputationTextBox.Name = "ComputationTextBox"
        '
        'btnOK
        '
        resources.ApplyResources(Me.btnOK, "btnOK")
        Me.btnOK.Name = "btnOK"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        resources.ApplyResources(Me.btnCancel, "btnCancel")
        Me.btnCancel.Name = "btnCancel"
        '
        'btnAdd
        '
        resources.ApplyResources(Me.btnAdd, "btnAdd")
        Me.btnAdd.Name = "btnAdd"
        '
        'btnSubtract
        '
        resources.ApplyResources(Me.btnSubtract, "btnSubtract")
        Me.btnSubtract.Name = "btnSubtract"
        '
        'btnMultiply
        '
        resources.ApplyResources(Me.btnMultiply, "btnMultiply")
        Me.btnMultiply.Name = "btnMultiply"
        '
        'btnDivide
        '
        resources.ApplyResources(Me.btnDivide, "btnDivide")
        Me.btnDivide.Name = "btnDivide"
        '
        'btnConcat
        '
        resources.ApplyResources(Me.btnConcat, "btnConcat")
        Me.btnConcat.Name = "btnConcat"
        '
        'FunctionTitleLabel
        '
        resources.ApplyResources(Me.FunctionTitleLabel, "FunctionTitleLabel")
        Me.FunctionTitleLabel.Name = "FunctionTitleLabel"
        '
        'AssignmentLabel
        '
        resources.ApplyResources(Me.AssignmentLabel, "AssignmentLabel")
        Me.AssignmentLabel.Name = "AssignmentLabel"
        '
        'LinkLabel1
        '
        resources.ApplyResources(Me.LinkLabel1, "LinkLabel1")
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.TabStop = True
        '
        'LinkLabel2
        '
        resources.ApplyResources(Me.LinkLabel2, "LinkLabel2")
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.TabStop = True
        '
        'lstFunctions
        '
        resources.ApplyResources(Me.lstFunctions, "lstFunctions")
        Me.lstFunctions.Name = "lstFunctions"
        '
        'frmFieldCalculator
        '
        resources.ApplyResources(Me, "$this")
        Me.Controls.Add(Me.lstFunctions)
        Me.Controls.Add(Me.LinkLabel2)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.AssignmentLabel)
        Me.Controls.Add(Me.FunctionTitleLabel)
        Me.Controls.Add(Me.btnConcat)
        Me.Controls.Add(Me.btnDivide)
        Me.Controls.Add(Me.btnMultiply)
        Me.Controls.Add(Me.btnSubtract)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.ComputationTextBox)
        Me.Controls.Add(Me.DestFieldComboBox)
        Me.Controls.Add(Me.DestFieldTitleLabel)
        Me.Controls.Add(Me.FieldsTitleLabel)
        Me.Controls.Add(Me.FieldsListView)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFieldCalculator"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    <CLSCompliant(False)> _
    Public Property shapefile() As MapWinGIS.Shapefile
        Get
            Return m_shapefile
        End Get
        Set(ByVal Value As MapWinGIS.Shapefile)
            m_shapefile = Value
            InitializeFieldValues()
        End Set
    End Property

    Public Property grid() As DataGrid
        Get
            Return m_Grid
        End Get
        Set(ByVal Value As DataGrid)
            m_Grid = Value
        End Set
    End Property

    Private Sub InitializeFieldValues()
        If m_shapefile Is Nothing Then
            Exit Sub
        End If

        Dim i As Integer
        For i = 0 To m_shapefile.NumFields - 1
            FieldsListView.Items.Add(m_shapefile.Field(i).Name)
            DestFieldComboBox.Items.Add(m_shapefile.Field(i).Name)
        Next i

        If DestFieldComboBox.Items.Count > 0 Then
            DestFieldComboBox.SelectedIndex = 0
        End If

        populateTreeView()
    End Sub


    Private Sub populateTreeView()

        Dim pntShape As String = "ShapeX ShapeY ShapeZ"
        Dim polyShape As String = "ShapeXFirst ShapeYFirst ShapeZFirst ShapeXLast ShapeYLast ShapeZLast"
        Dim Oper As String = "+ - * / % \\ ^ |x| ! > >= < <= = <>"
        Dim boolOp As String = "and or not xor nand nor nxor"
        Dim gonFun As String = "atn(x) cos(x) sin(x) tan(x) acos(x) asin(x) cosh(x) sinh(x) tanh(x) acosh(x) asinh(x) atanh(x) csc(x) sec(x) cot(x) acsc(x) asec(x) acot(x) csch(x) sech(x) coth(x) acsch(x) asech(x) acoth(x) rad(x) deg(x) grad(x)"
        Dim mathFun As String = "abs(x) cbr(x) comb(n,k) dec(x) exp(x) fact(x) fix(x) gcd(a,b,...) int(x) lcm(a,b,...) ln(x) logN(x,n) mod(a,b) perm(n,k) rnd(x) root(x,n) round(x,d) sgn(x) sqr(x)"
        Dim statFun As String = "min(a,b,...) max(a,b,...) mcd(a,b,...) mcm(a,b,...) Sum(a,b,...) Mean(a,b,...) Meanq(a,b,...) Meang(a,b,...) Var(a,b,...) Varp(a,b,...) Stdev(a,b,...) Stdevp(a,b,...) Step(x,a)"
        Dim timeFun As String = "Year(d) date# DateSerial(a,m,d) Day(d) Hour(d) Minute(d) Month(d) now# Second(d) time# TimeSerial(h,m,s)"
        Dim otherFun As String = "Psi(x) AiryA(x) AiryB(x) BesselI(x,n) BesselJ(x,n) BesselK(x,n) BesselY(x,n) beta(a,b) betaI(x,a,b) CBinom(k,n,x) Ci(x) Clip(x,a,b) CNorm(x,m,d) CPoisson(x,k) DBinom(k,n,x) digamma(x) psi(x) DNorm(x,μ,σ) DPoisson(x,k) Ei(x) Ein(x,n) Elli1(x) Elli2(x) Erf(x) FresnelC(x) FresnelS(x) gamma(x) gammai(a,x) gammaln(x) HypGeom(x,a,b,c) I0(x) J0(x) K0(x) PolyCh(x,n) PolyHe(x,n) PolyLa(x,n) PolyLe(x,n) Si(x) WAM(t,fo,fm,m) WEXP(t,p,a) WEXPB(t,p,a) WFM(t,fo,fm,m) WLIN(t,p,d) WPARAB(t,p) WPULSE(t,p,d) WPULSEF(t,p,a) WRAISE(t,p) WRECT(t,p,d) WRING(t,p,a,fm) WRIPPLE(t,p,a) WSAW(t,p) WSQR(t,p) WSTEPS(t,p,n) WTRAPEZ(t,p,d) WTRI(t,p) Y0(x) zeta(x)"
        Dim constants As String = "PI# pi# pi2# pi3# pi4# e# eu# phi# g# G# R# eps# mu# c# q# me# mp# mn# K# h# A#"

        Dim cat As String() = {"Shapes", "Operators", "Booleans", "Maths", "Angles", "Statistics", "Time", "Other", "Constants"}

        Dim subs As String()
        Dim chr As Char() = {" "}
        Dim c As String
        For Each c In cat
            Select Case c
                Case "Shapes"
                    If m_shapefile.ShapefileType = MapWinGIS.ShpfileType.SHP_MULTIPOINT Or m_shapefile.ShapefileType = MapWinGIS.ShpfileType.SHP_MULTIPOINTM Or m_shapefile.ShapefileType = MapWinGIS.ShpfileType.SHP_MULTIPOINTZ Or m_shapefile.ShapefileType = MapWinGIS.ShpfileType.SHP_POINT Or m_shapefile.ShapefileType = MapWinGIS.ShpfileType.SHP_POINTZ Or m_shapefile.ShapefileType = MapWinGIS.ShpfileType.SHP_POINTM Then
                        subs = pntShape.Split(chr)
                        populateTreeView(c, subs)
                        Exit Select
                    ElseIf m_shapefile.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINE Or m_shapefile.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINEZ Or m_shapefile.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINEM Then
                        subs = polyShape.Split(chr)
                        populateTreeView(c, subs)
                        Exit Select
                    End If
                    Exit Select
                Case "Operators"
                    subs = Oper.Split(chr)
                    populateTreeView(c, subs)
                    Exit Select
                Case "Booleans"
                    subs = boolOp.Split(chr)
                    populateTreeView(c, subs)
                    Exit Select
                Case "Maths"
                    subs = mathFun.Split(chr)
                    populateTreeView(c, subs)
                    Exit Select
                Case "Angles"
                    subs = gonFun.Split(chr)
                    populateTreeView(c, subs)
                    Exit Select
                Case "Statistics"
                    subs = statFun.Split(chr)
                    populateTreeView(c, subs)
                    Exit Select
                Case "Time"
                    subs = timeFun.Split(chr)
                    populateTreeView(c, subs)
                    Exit Select
                Case "Other"
                    subs = otherFun.Split(chr)
                    populateTreeView(c, subs)
                    Exit Select
                Case "Constants"
                    subs = constants.Split(chr)
                    populateTreeView(c, subs)
                    Exit Select
            End Select
        Next
    End Sub

    Private Sub populateTreeView(ByVal cat As String, ByVal items As String())

        Dim tNode As TreeNode = Me.lstFunctions.Nodes.Add(cat)
        Dim i As String
        For Each i In items
            tNode.Nodes.Add(i)
        Next
    End Sub


    Private Sub oldAddTextToComputation(ByVal value As String)
        If ComputationTextBox.Text <> "" Then
            value = " " & value
        End If
        Dim startLength As Integer = ComputationTextBox.Text.Length
        ComputationTextBox.Text = String.Concat(ComputationTextBox.Text, value)
        ComputationTextBox.Focus()

        'Highlight the (first?) variable in parens
        Try
            If ComputationTextBox.Text.IndexOf("(", startLength) > -1 Then
                ComputationTextBox.SelectionStart = ComputationTextBox.Text.IndexOf("(", startLength) + 1
                ComputationTextBox.SelectionLength = 1
            End If
        Catch
        End Try
    End Sub

    'Modified by EAC 22/12/2009
    Private Sub AddTextToComputation(ByVal s As String)

        Dim formulaTxt As String
        Dim beforeS As String = ""
        Dim afterS As String = ""
        formulaTxt = Me.ComputationTextBox.Text

        If (Me.ComputationTextBox.SelectionLength > 0) Then

            beforeS = formulaTxt.Substring(0, Me.ComputationTextBox.SelectionStart)
            afterS = formulaTxt.Substring(Me.ComputationTextBox.SelectionStart + Me.ComputationTextBox.SelectionLength)
            formulaTxt = beforeS + s + afterS
        Else
            If formulaTxt <> "" Then
                formulaTxt = formulaTxt + " " + s
            Else
                formulaTxt = formulaTxt + s
            End If
        End If

        Me.ComputationTextBox.Text = formulaTxt
    End Sub


    Private Sub FieldsListView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles FieldsListView.DoubleClick
        Dim value As String
        If FieldsListView.SelectedItems.Count > 0 Then
            value = "[" & FieldsListView.SelectedItems.Item(0).Text & "]"
            AddTextToComputation(value)
        End If
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        AddTextToComputation("+")
    End Sub

    Private Sub btnSubtract_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubtract.Click
        AddTextToComputation("-")
    End Sub

    Private Sub btnMultiply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMultiply.Click
        AddTextToComputation("*")
    End Sub

    Private Sub btnDivide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDivide.Click
        AddTextToComputation("/")
    End Sub

    Private Sub btnConcat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConcat.Click
        AddTextToComputation("&")
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If ComputationTextBox.Text <> "" Then
            'Make sure all open parenthesis are closed to avoid stack overrun in parser
            CloseParens(ComputationTextBox.Text)
            Me.Refresh()
            Try
                Dim rt As Boolean = m_parser.StoreExpression(ComputationTextBox.Text)
                If Not rt Then
                    MapWinUtility.Logger.Msg("Could not parse computation equation: Invalid Syntax", MsgBoxStyle.Critical, "Field Calculator: Syntax Error")
                    Exit Sub
                End If
            Catch ex As Exception
                MapWinUtility.Logger.Msg("Could not parse computation equation: Invalid Syntax", MsgBoxStyle.Critical, "Field Calculator: Syntax Error")
                Exit Sub
            End Try
            If CalculateValues() Then
                If Not Me.Owner Is Nothing Then
                    CType(Me.Owner, frmTableEditor).TableEditorDataGrid.Refresh()
                    CType(Me.Owner, frmTableEditor).btnApply.Enabled = True
                    CType(Me.Owner, frmTableEditor).m_TrulyChanged = True
                End If

                If MapWinUtility.Logger.Msg("The calculation has completed. Would you like to close the Field Calculator now?", MsgBoxStyle.Information + MsgBoxStyle.YesNo, "Complete! Close window?") = MsgBoxResult.Yes Then Me.Close()
            End If
        End If
    End Sub

    Public Function CalculateValues() As Boolean
        m_DestFieldColumn = GetFieldColumn(DestFieldComboBox.SelectedItem, m_Grid)
        Dim SettingAll As Boolean = True
        Dim i As Integer

        For i = 0 To CType(m_Grid.DataSource, DataTable).Rows.Count - 1
            If m_Grid.IsSelected(i) Then
                SettingAll = False
                Exit For
            End If
        Next

        For i = 0 To CType(m_Grid.DataSource, DataTable).Rows.Count - 1
            If m_Grid.IsSelected(i) Or SettingAll Then
                Try
                    'Note index starts at 1, not zero
                    For j As Integer = 1 To m_parser.VarTop
                        For z As Integer = 0 To CType(m_Grid.DataSource, DataTable).Columns.Count - 1
                            Try
                                If CType(m_Grid.DataSource, DataTable).Columns(z).ColumnName.ToLower() = m_parser.VarName(j).ToLower() Then
                                    m_parser.VarValue(j) = Double.Parse(m_Grid.Item(i, z).ToString())
                                ElseIf m_parser.VarName(j).ToLower() = "shapex" Then
                                    m_parser.VarValue(j) = m_shapefile.Shape(Long.Parse(m_Grid.Item(i, 0).ToString())).Point(0).x
                                ElseIf m_parser.VarName(j).ToLower() = "shapey" Then
                                    m_parser.VarValue(j) = m_shapefile.Shape(Long.Parse(m_Grid.Item(i, 0).ToString())).Point(0).y
                                ElseIf m_parser.VarName(j).ToLower() = "shapez" Then
                                    m_parser.VarValue(j) = m_shapefile.Shape(Long.Parse(m_Grid.Item(i, 0).ToString())).Point(0).Z
                                ElseIf m_parser.VarName(j).ToLower() = "shapexfirst" Then
                                    m_parser.VarValue(j) = m_shapefile.Shape(Long.Parse(m_Grid.Item(i, 0).ToString())).Point(0).x
                                ElseIf m_parser.VarName(j).ToLower() = "shapeyfirst" Then
                                    m_parser.VarValue(j) = m_shapefile.Shape(Long.Parse(m_Grid.Item(i, 0).ToString())).Point(0).y
                                ElseIf m_parser.VarName(j).ToLower() = "shapezfirst" Then
                                    m_parser.VarValue(j) = m_shapefile.Shape(Long.Parse(m_Grid.Item(i, 0).ToString())).Point(0).Z
                                ElseIf m_parser.VarName(j).ToLower() = "shapexlast" Then
                                    m_parser.VarValue(j) = m_shapefile.Shape(Long.Parse(m_Grid.Item(i, 0).ToString())).Point(m_shapefile.NumShapes - 1).x
                                ElseIf m_parser.VarName(j).ToLower() = "shapeylast" Then
                                    m_parser.VarValue(j) = m_shapefile.Shape(Long.Parse(m_Grid.Item(i, 0).ToString())).Point(m_shapefile.NumShapes - 1).y
                                ElseIf m_parser.VarName(j).ToLower() = "shapezlast" Then
                                    m_parser.VarValue(j) = m_shapefile.Shape(Long.Parse(m_Grid.Item(i, 0).ToString())).Point(m_shapefile.NumShapes - 1).Z
                                End If
                            Catch ex2 As Exception
                                MapWinUtility.Logger.Dbg("DEBUG: " + ex2.ToString())
                            End Try
                        Next
                    Next

                    Dim val As String = m_parser.Eval().ToString()
                    m_Grid.Item(i, m_DestFieldColumn) = val
                Catch ex2 As Exception
                    MapWinUtility.Logger.Msg("The data is an invalid type for the destination table cell.", MsgBoxStyle.Exclamation, "Field Calculator: Cannot Store Field Value")
                    Return False
                End Try
            End If
        Next i

        Return True
    End Function

    Private Sub DestFieldComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DestFieldComboBox.SelectedIndexChanged
        Try
            Dim isNumber As Boolean = False
            m_DestFieldName = DestFieldComboBox.SelectedItem
            m_DestFieldColumn = GetFieldColumn(DestFieldComboBox.SelectedItem, m_Grid)
            If CType(m_Grid.DataSource, DataTable).Columns(m_DestFieldColumn).DataType.Name = "Int16" Then
                isNumber = True
            ElseIf CType(m_Grid.DataSource, DataTable).Columns(m_DestFieldColumn).DataType.Name = "Int32" Then
                isNumber = True
            ElseIf CType(m_Grid.DataSource, DataTable).Columns(m_DestFieldColumn).DataType.Name = "Int64" Then
                isNumber = True
            ElseIf CType(m_Grid.DataSource, DataTable).Columns(m_DestFieldColumn).DataType.Name = "Integer" Then
                isNumber = True
            ElseIf CType(m_Grid.DataSource, DataTable).Columns(m_DestFieldColumn).DataType.Name = "Double" Then
                isNumber = True
            End If
            If isNumber Then
                btnConcat.Enabled = False
            Else
                btnConcat.Enabled = True
            End If
        Catch ex As Exception
            MapWinUtility.Logger.Msg("Could not set destination field name: " & ex.Message, MsgBoxStyle.Exclamation, "Field Calculator: Error setting field name")
        End Try
    End Sub

    Private Function GetFieldColumn(ByVal fieldName As String, ByVal grid As DataGrid) As Integer
        Dim i As Integer
        For i = 0 To CType(grid.DataSource, DataTable).Columns.Count - 1
            If fieldName = CType(grid.DataSource, DataTable).Columns.Item(i).Caption Then
                Return i
            End If
        Next i
        MapWinUtility.Logger.Msg("An Invalid Field was selected in the Field Calculator Tool: " & fieldName, MsgBoxStyle.Critical, "Field Calculator: Invalid Field Selected")
        Return 0
    End Function

    Private Sub CloseParens(ByRef text As String)
        Dim opencount As Integer = 0
        Dim i As Integer
        For i = 0 To text.Length - 1
            If text.Chars(i) = "(" Then
                opencount = opencount + 1
            ElseIf text.Chars(i) = ")" Then
                opencount = opencount - 1
            End If
        Next i
        For i = 0 To opencount - 1
            text = String.Concat(text, ")")
        Next i
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start("http://www.mapwindow.org/wiki/index.php/MapWindow:TableEditorFunctions")
    End Sub

    Private Sub frmFieldCalculator_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        CType(Me.Owner, frmTableEditor).TextCalculator()
        Me.Close()
    End Sub


    Private Sub lstFunctions_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstFunctions.DoubleClick
        Dim op As String = ""
        Try
            If (lstFunctions.SelectedNode.Level > 0) Then

                op = lstFunctions.SelectedNode.Text
                AddTextToComputation(op)
            End If
        Catch
        End Try
    End Sub
End Class
