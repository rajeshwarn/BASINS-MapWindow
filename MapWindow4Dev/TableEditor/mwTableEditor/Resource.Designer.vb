﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System


'This class was auto-generated by the StronglyTypedResourceBuilder
'class via a tool like ResGen or Visual Studio.
'To add or remove a member, edit your .ResX file then rerun ResGen
'with the /str option, or rebuild your VS project.
'''<summary>
'''  A strongly-typed resource class, for looking up localized strings, etc.
'''</summary>
<Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"),  _
 Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
 Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
Friend Class Resource
    
    Private Shared resourceMan As Global.System.Resources.ResourceManager
    
    Private Shared resourceCulture As Global.System.Globalization.CultureInfo
    
    <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
    Friend Sub New()
        MyBase.New
    End Sub
    
    '''<summary>
    '''  Returns the cached ResourceManager instance used by this class.
    '''</summary>
    <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Friend Shared ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
        Get
            If Object.ReferenceEquals(resourceMan, Nothing) Then
                Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("mwTableEditor.Resource", GetType(Resource).Assembly)
                resourceMan = temp
            End If
            Return resourceMan
        End Get
    End Property
    
    '''<summary>
    '''  Overrides the current thread's CurrentUICulture property for all
    '''  resource lookups using this strongly typed resource class.
    '''</summary>
    <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Friend Shared Property Culture() As Global.System.Globalization.CultureInfo
        Get
            Return resourceCulture
        End Get
        Set
            resourceCulture = value
        End Set
    End Property
    
    '''<summary>
    '''  Looks up a localized string similar to Assign feature ID&apos;s to a field..
    '''</summary>
    Friend Shared ReadOnly Property AssignFeatureIDSToAField() As String
        Get
            Return ResourceManager.GetString("AssignFeatureIDSToAField", resourceCulture)
        End Get
    End Property
    
    '''<summary>
    '''  Looks up a localized string similar to The existing data in the target field will be overwritten..
    '''</summary>
    Friend Shared ReadOnly Property DataWillBeOverwritten() As String
        Get
            Return ResourceManager.GetString("DataWillBeOverwritten", resourceCulture)
        End Get
    End Property
    
    '''<summary>
    '''  Looks up a localized string similar to Enter the name of the target field..
    '''</summary>
    Friend Shared ReadOnly Property EnterTheNameOfTheTargetField() As String
        Get
            Return ResourceManager.GetString("EnterTheNameOfTheTargetField", resourceCulture)
        End Get
    End Property
    
    '''<summary>
    '''  Looks up a localized resource of type System.Drawing.Bitmap.
    '''</summary>
    Friend Shared ReadOnly Property filter() As System.Drawing.Bitmap
        Get
            Dim obj As Object = ResourceManager.GetObject("filter", resourceCulture)
            Return CType(obj,System.Drawing.Bitmap)
        End Get
    End Property
    
    '''<summary>
    '''  Looks up a localized string similar to Attribute Table Editor.
    '''</summary>
    Friend Shared ReadOnly Property TableEditorToolbarBtn_Tooltip() As String
        Get
            Return ResourceManager.GetString("TableEditorToolbarBtn.Tooltip", resourceCulture)
        End Get
    End Property
    
    '''<summary>
    '''  Looks up a localized resource of type System.Drawing.Bitmap.
    '''</summary>
    Friend Shared ReadOnly Property tableNew() As System.Drawing.Bitmap
        Get
            Dim obj As Object = ResourceManager.GetObject("tableNew", resourceCulture)
            Return CType(obj,System.Drawing.Bitmap)
        End Get
    End Property
End Class
