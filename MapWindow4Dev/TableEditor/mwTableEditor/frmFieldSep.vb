
Public Class frmFieldSep
#Region "Variables"
    'Dim newFields As ArrayList = New ArrayList                          ' New fields
    Private m_sf As MapWinGIS.Shapefile                                 ' Opened sheip file
    Private m_tableValue As DataTable                                   ' Grid's data
    Private m_cellValue As System.Windows.Forms.DataGridCell            ' Selected cell
    Private m_NewValues As ArrayList = New ArrayList()                  ' New values for
    Private resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFieldSep))
#End Region

#Region "Properties"
    <CLSCompliant(False)> _
    Public WriteOnly Property SfProperty() As MapWinGIS.Shapefile
        Set(ByVal value As MapWinGIS.Shapefile)
            m_sf = value
        End Set
    End Property
    Public WriteOnly Property CellValueProperty() As System.Windows.Forms.DataGridCell
        Set(ByVal value As System.Windows.Forms.DataGridCell)
            m_cellValue = value
        End Set
    End Property
    Public WriteOnly Property TableValueProperty() As System.Data.DataTable
        Set(ByVal value As System.Data.DataTable)
            m_tableValue = value
        End Set
    End Property
    Public ReadOnly Property NewValuesProperty() As System.Collections.ArrayList
        Get
            Return m_NewValues
        End Get
    End Property
#End Region

#Region "Button clicks"
    Private Sub rdBtTab_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        tBoxOther.Enabled = False
    End Sub

    Private Sub rdBtSpace_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdBtSpace.CheckedChanged
        tBoxOther.Enabled = False
    End Sub

    Private Sub rdBtColon_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdBtColon.CheckedChanged
        tBoxOther.Enabled = False
    End Sub

    Private Sub rdBtOther_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdBtOther.CheckedChanged
        tBoxOther.Enabled = True

    End Sub
    Private Sub frmFieldSap_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim columnInd As Integer
        Dim columnValue As DataColumn
        Dim strName As String = ""
        Dim count As Integer = 0
        Dim i As Integer = 1
        Dim index As Integer
        Dim strField As String = resources.GetString("msgTheField")

        tBoxOther.Enabled = False
        lstFields.Items.Clear()

        columnInd = m_cellValue.ColumnNumber
        columnValue = m_tableValue.Columns.Item(columnInd)

        lblField.Text = columnValue.ColumnName.ToString()

        While count < 2
            strName = String.Format("{0}{1}", strField, i)
            If nameNotExist(strName) Then
                lstFields.Items.Add(strName)
                count = count + 1
            End If
            i = i + 1
        End While


        If rdBtOther.Checked Then
            tBoxOther.Enabled = True
        End If

        If tBoxNField.Text.Length > 0 Then
            btnAdd.Enabled = True
        Else
            btnAdd.Enabled = False
        End If

        index = lstFields.SelectedIndex
        If index < 0 Then
            btnRemove.Enabled = False
        Else
            btnRemove.Enabled = True
        End If
    End Sub
    Private Function nameNotExist(ByVal strName As String) As Boolean
        Dim exist As Boolean = False

        For index As Integer = 0 To m_tableValue.Columns.Count - 1
            If strName.Equals(m_tableValue.Columns.Item(index).ColumnName.ToString, System.StringComparison.OrdinalIgnoreCase) Then
                exist = True
                Exit For
            End If
        Next

        If exist Then
            Return False
        Else
            Return True
        End If
    End Function
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Dim index As Integer
        If lstFields.Items.Count > 2 Then
            index = lstFields.SelectedIndex
            If index <> -1 Then
                lstFields.Items.RemoveAt(index)
            End If
        End If
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click

        Dim fldName As String
        Dim index As Integer
        If tBoxNField.Text.Length > 0 Then
            fldName = tBoxNField.Text
            index = lstFields.FindStringExact(fldName)
            If index = Windows.Forms.ListBox.NoMatches Then
                If nameNotExist(fldName) Then
                    lstFields.Items.Add(fldName)
                Else
                    MapWinUtility.Logger.Msg(resources.GetString("msgTheField") & " " & fldName & " " & resources.GetString("msgExists"), MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, resources.GetString("msgError"))

                End If
            Else
                MapWinUtility.Logger.Msg(resources.GetString("msgTheField") & " " & fldName & " " & resources.GetString("msgExists"), MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, resources.GetString("msgError"))
            End If
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Dim fieldValue As String
        Dim separator As String
        Dim oneValue As ArrayList
        separator = ""
        If rdBtSpace.Checked Then
            separator = " "
        ElseIf rdBtColon.Checked Then
            separator = ":"
        ElseIf rdBtOther.Checked Then
            If tBoxOther.Text.Length <> 0 Then
                separator = tBoxOther.Text.ToString()
            End If
        End If

        m_NewValues.Clear()
        If separator.Length <> 0 Then

            For index As Integer = 0 To m_tableValue.Rows.Count - 1
                fieldValue = m_tableValue.Rows.Item(index).Item(m_cellValue.ColumnNumber).ToString
                oneValue = saperateString(fieldValue, separator, lstFields.Items.Count)
                m_NewValues.Add(oneValue)
            Next

            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Hide()
        Else
            MapWinUtility.Logger.Msg(resources.GetString("msgNoSaperator"), MsgBoxStyle.OkOnly Or MsgBoxStyle.Information, resources.GetString("msgError"))
        End If
    End Sub
#End Region

    Private Function saperateString(ByVal strField As String, ByVal separator As String, ByVal countF As Integer) As ArrayList
        Dim retStr As ArrayList = New ArrayList()
        Dim oneField As String
        Dim index As Integer
        index = 1

        If strField.Length <> 0 Then
            While index >= 0 And countF > 1
                index = strField.IndexOf(separator)
                If index = 0 Then

                    If separator.Length = 1 Then
                        strField = strField.TrimStart(separator)
                    Else
                        strField = strField.Substring(separator.Length)
                    End If
                ElseIf index > 0 Then
                    oneField = strField.Substring(0, index)
                    If oneField.Length <> 0 Then
                        retStr.Add(oneField)
                    End If

                    strField = strField.Substring(index + separator.Length)
                    countF = countF - 1
                End If

            End While
            If separator.Length = 1 Then
                strField = strField.TrimStart(separator)
            End If

            If strField.Length > 0 Then
                retStr.Add(strField)
            End If

        End If

        Return retStr
    End Function

    Private Sub tBoxNField_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tBoxNField.TextChanged

        If tBoxNField.Text.Length > 0 Then
            btnAdd.Enabled = True
        Else
            btnAdd.Enabled = False
        End If

    End Sub

    Private Sub lstFields_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstFields.SelectedIndexChanged
        Dim index As Integer
        index = lstFields.SelectedIndex
        If index < 0 Then
            btnRemove.Enabled = False
        Else
            btnRemove.Enabled = True
        End If
    End Sub
End Class