'********************************************************************************************************
'File Name: frmStatistics.vb
'Description: (for future use)
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

Public Class frmStatistics
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
    Friend WithEvents Statistics_tb As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStatistics))
        Me.Statistics_tb = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'Statistics_tb
        '
        Me.Statistics_tb.AcceptsReturn = True
        Me.Statistics_tb.AccessibleDescription = Nothing
        Me.Statistics_tb.AccessibleName = Nothing
        resources.ApplyResources(Me.Statistics_tb, "Statistics_tb")
        Me.Statistics_tb.BackColor = System.Drawing.SystemColors.Window
        Me.Statistics_tb.BackgroundImage = Nothing
        Me.Statistics_tb.Font = Nothing
        Me.Statistics_tb.Name = "Statistics_tb"
        Me.Statistics_tb.ReadOnly = True
        '
        'frmStatistics
        '
        Me.AccessibleDescription = Nothing
        Me.AccessibleName = Nothing
        resources.ApplyResources(Me, "$this")
        Me.BackgroundImage = Nothing
        Me.Controls.Add(Me.Statistics_tb)
        Me.Font = Nothing
        Me.Name = "frmStatistics"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    '***Added by Nathan Eaton, CALM 02/05***'
End Class
