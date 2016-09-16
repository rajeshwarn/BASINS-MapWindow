Imports System.Windows.Forms
Imports System.Drawing

Public Class DataGridMaskedTextColumn
    Inherits DataGridTextBoxColumn
    Private _currencyManager As System.Windows.Forms.CurrencyManager
    Private _Editing As Boolean = False
    Private MaskedBox As New MaskedTextBox
    Public Mask As String = ""

    Public Sub New()
        _currencyManager = Nothing
        AddHandler Me.TextBox.TextChanged, AddressOf MarkEdited
    End Sub

    Private Sub MarkEdited(ByVal sender As Object, ByVal e As EventArgs)
        _Editing = True
    End Sub

    Protected Overloads Overrides Sub Edit(ByVal source As CurrencyManager, ByVal rowNum As Integer, ByVal bounds As Rectangle, ByVal [readOnly] As Boolean, ByVal instantText As String, ByVal cellIsVisible As Boolean)
        MyBase.Edit(source, rowNum, bounds, [readOnly], instantText, cellIsVisible)
        Dim NewLoc As System.Drawing.Point
        NewLoc = Me.TextBox.Location
        NewLoc.X -= 3
        NewLoc.Y -= 3
        MaskedBox.Location = NewLoc
        MaskedBox.Text = Me.TextBox.Text
        MaskedBox.Parent = Me.TextBox.Parent
        MaskedBox.Size = New Size(Me.TextBox.Size.Width + 3, MaskedBox.Size.Height)
        MaskedBox.Mask = Mask

        Me.TextBox.Visible = False
        MaskedBox.Visible = True
        MaskedBox.BringToFront()
        MaskedBox.Focus()
    End Sub

    Protected Overloads Overrides Function Commit(ByVal dataSource As System.Windows.Forms.CurrencyManager, ByVal rowNum As Integer) As Boolean
        If Not _Editing Then Return True
        _Editing = False

        SetColumnValueAtRow(dataSource, rowNum, MaskedBox.Text)
        Return True
    End Function
End Class
