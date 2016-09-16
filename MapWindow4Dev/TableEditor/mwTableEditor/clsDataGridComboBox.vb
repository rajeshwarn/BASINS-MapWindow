Imports System.Windows.Forms
Imports System.Drawing

Public Class DataGridComboBoxColumn
    Inherits DataGridTextBoxColumn
    Public ComboBox As FocusShiftCombo
    Private _currencyManager As System.Windows.Forms.CurrencyManager
    Private _rowNum As Integer
    Private _Editing As Boolean

    Public Sub New()
        _currencyManager = Nothing
        _Editing = False

        ComboBox = New FocusShiftCombo()
        ComboBox.DropDownStyle = ComboBoxStyle.DropDownList
        AddHandler ComboBox.Leave, AddressOf LeaveComboBox

        AddHandler ComboBox.SelectionChangeCommitted, AddressOf SelectionChangeCommit
    End Sub

    Private Sub HandleScroll(ByVal sender As Object, ByVal e As EventArgs)
        If ComboBox.Visible Then
            ComboBox.Hide()
        End If
    End Sub

    Private Sub SelectionChangeCommit(ByVal sender As Object, ByVal e As EventArgs)
        _Editing = True
        MyBase.ColumnStartedEditing(DirectCast(sender, System.Windows.Forms.Control))
    End Sub

    Private Sub LeaveComboBox(ByVal sender As Object, ByVal e As EventArgs)
        If _Editing Then
            SetColumnValueAtRow(_currencyManager, _rowNum, ComboBox.Text)
            _Editing = False

            Invalidate()
        End If

        ComboBox.Hide()
        AddHandler Me.DataGridTableStyle.DataGrid.Scroll, AddressOf HandleScroll
    End Sub

    Protected Overloads Overrides Sub Edit(ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)
        MyBase.Edit(source, rowNum, bounds, [readOnly], instantText, cellIsVisible)

        _rowNum = rowNum
        _currencyManager = source

        Dim NewLoc As System.Drawing.Point
        NewLoc = Me.TextBox.Location
        NewLoc.X -= 3
        NewLoc.Y -= 3
        ComboBox.Location = NewLoc
        ComboBox.Text = Me.TextBox.Text
        ComboBox.Parent = Me.TextBox.Parent
        ComboBox.Size = New Size(Me.TextBox.Size.Width + 3, ComboBox.Size.Height)
        ComboBox.SelectedIndex = ComboBox.FindStringExact(Me.TextBox.Text)

        Me.TextBox.Visible = False
        ComboBox.Visible = True
        ComboBox.BringToFront()
        ComboBox.Focus()
        AddHandler Me.DataGridTableStyle.DataGrid.Scroll, AddressOf HandleScroll
    End Sub

    Protected Overloads Overrides Function Commit(ByVal dataSource As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer) As Boolean
        If _Editing Then
            _Editing = False
            SetColumnValueAtRow(dataSource, rowNum, ComboBox.Text)
        End If
        Return True
    End Function
End Class

Public Class FocusShiftCombo
    Inherits System.Windows.Forms.ComboBox
    Private Const WM_KEYUP As Integer = 257

    Protected Overloads Overrides Sub WndProc(ByRef theMessage As System.Windows.Forms.Message)
        If theMessage.Msg = WM_KEYUP Then
            Return
        Else
            MyBase.WndProc(theMessage)
        End If
    End Sub
End Class