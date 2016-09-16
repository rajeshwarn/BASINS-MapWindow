'********************************************************************************************************
'File Name: frmColumn_Expression.vb
'Description: This form is used to build a column query with the Data Query tool.
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
'The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
'Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
'public domain in March 2004.  
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'Sept 01 2005: Chris Michaelis cmichaelis@happysquirrel.com - 
'              Replaced the Public Domain table editor with an enhanced version that was
'              contributed by Nathan Eaton at CALM Western Australia. This is released
'              as open source with his permission.


Public Class frmTextCalculator
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
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Close_bn As System.Windows.Forms.Button
    Friend WithEvents Apply As System.Windows.Forms.Button
    Friend WithEvents query_text_tb As System.Windows.Forms.TextBox
    Friend WithEvents Fields_lb As System.Windows.Forms.ListBox
    Friend WithEvents functions_lb As System.Windows.Forms.ListBox
    Friend WithEvents AssignmentLabel As System.Windows.Forms.Label
    Friend WithEvents DestFieldComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents DestFieldTitleLabel As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTextCalculator))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Close_bn = New System.Windows.Forms.Button
        Me.Apply = New System.Windows.Forms.Button
        Me.query_text_tb = New System.Windows.Forms.TextBox
        Me.Fields_lb = New System.Windows.Forms.ListBox
        Me.functions_lb = New System.Windows.Forms.ListBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.AssignmentLabel = New System.Windows.Forms.Label
        Me.DestFieldComboBox = New System.Windows.Forms.ComboBox
        Me.DestFieldTitleLabel = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Close_bn
        '
        Me.Close_bn.DialogResult = System.Windows.Forms.DialogResult.Cancel
        resources.ApplyResources(Me.Close_bn, "Close_bn")
        Me.Close_bn.Name = "Close_bn"
        '
        'Apply
        '
        resources.ApplyResources(Me.Apply, "Apply")
        Me.Apply.Name = "Apply"
        '
        'query_text_tb
        '
        resources.ApplyResources(Me.query_text_tb, "query_text_tb")
        Me.query_text_tb.Name = "query_text_tb"
        '
        'Fields_lb
        '
        Me.Fields_lb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.Fields_lb, "Fields_lb")
        Me.Fields_lb.Name = "Fields_lb"
        Me.Fields_lb.Sorted = True
        '
        'functions_lb
        '
        Me.functions_lb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.functions_lb.Items.AddRange(New Object() {resources.GetString("functions_lb.Items"), resources.GetString("functions_lb.Items1"), resources.GetString("functions_lb.Items2"), resources.GetString("functions_lb.Items3"), resources.GetString("functions_lb.Items4"), resources.GetString("functions_lb.Items5")})
        resources.ApplyResources(Me.functions_lb, "functions_lb")
        Me.functions_lb.Name = "functions_lb"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'AssignmentLabel
        '
        resources.ApplyResources(Me.AssignmentLabel, "AssignmentLabel")
        Me.AssignmentLabel.Name = "AssignmentLabel"
        '
        'DestFieldComboBox
        '
        resources.ApplyResources(Me.DestFieldComboBox, "DestFieldComboBox")
        Me.DestFieldComboBox.Name = "DestFieldComboBox"
        '
        'DestFieldTitleLabel
        '
        resources.ApplyResources(Me.DestFieldTitleLabel, "DestFieldTitleLabel")
        Me.DestFieldTitleLabel.Name = "DestFieldTitleLabel"
        '
        'frmTextCalculator
        '
        resources.ApplyResources(Me, "$this")
        Me.CancelButton = Me.Close_bn
        Me.ControlBox = False
        Me.Controls.Add(Me.AssignmentLabel)
        Me.Controls.Add(Me.DestFieldComboBox)
        Me.Controls.Add(Me.DestFieldTitleLabel)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.functions_lb)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Close_bn)
        Me.Controls.Add(Me.Apply)
        Me.Controls.Add(Me.query_text_tb)
        Me.Controls.Add(Me.Fields_lb)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmTextCalculator"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    '***Added By Nathan Eaton, CALM 02/05
    'this form allows the user to specify a column expression for a column

    Private Sub Close_bn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Close_bn.Click
        Me.Close()
    End Sub

    Private Sub Apply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Apply.Click
        'Test first to see if using a column expression -- if so, make sure there are no circular references
        If functions_lb.Text = "+" Or functions_lb.Text = "Trim()" Or functions_lb.Text = "Substring()" Then
            If query_text_tb.Text.ToLower().Contains(DestFieldComboBox.Text.ToLower().Trim()) Then
                mapwinutility.logger.msg("The +, Trim(), and Substring() functions cannot operate on the same field as you are setting.", MsgBoxStyle.Exclamation, "Circular Reference")
                Return
            End If
        End If

        'send the query from the form to the routine that will perform the column expression
        TableEditor.Column_Expression_Calculate(Me.DestFieldComboBox.Text, Me.query_text_tb.Text)
    End Sub

    Private Sub Fields_lb_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Fields_lb.DoubleClick
        'add the fieldname to the query string textbox
        If Me.query_text_tb.Text = "" Then
            Me.query_text_tb.Text = Me.query_text_tb.Text & Me.Fields_lb.Items.Item(Me.Fields_lb.SelectedIndex)
        Else
            Me.query_text_tb.Text = Me.query_text_tb.Text & " " & Me.Fields_lb.Items.Item(Me.Fields_lb.SelectedIndex)
        End If

        'Pass the focus back to the query textbox
        Me.query_text_tb.Focus()
        Me.query_text_tb.AppendText("")
    End Sub

    Private Sub functions_lb_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles functions_lb.DoubleClick
        If Me.query_text_tb.Text = "" Then
            Me.query_text_tb.Text = Me.query_text_tb.Text & Me.functions_lb.Items.Item(Me.functions_lb.SelectedIndex) & " "
        Else
            Me.query_text_tb.Text = Me.query_text_tb.Text & " " & Me.functions_lb.Items.Item(Me.functions_lb.SelectedIndex) & " "
        End If

        'Pass the focus back to the query textbox
        Me.query_text_tb.Focus()
        Me.query_text_tb.AppendText("")
    End Sub
End Class
