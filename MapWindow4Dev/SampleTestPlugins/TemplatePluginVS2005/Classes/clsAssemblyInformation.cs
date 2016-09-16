//********************************************************************************************************
//File Name: clsAsemblyInformation.cs
//Description: This class return Assembly info from the AssemblyInfo.cs
//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
//limitations under the License. 
//
//The Original Code is MapWindow Open Source MeemsTools Plug-in. 
//
//The Initial Developer of this version of the Original Code is Paul Meems.
//
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Change Log: 
// Date          Changed By      Notes
// 8 April 2008  Paul Meems      Inital upload the MW SVN repository
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace TemplatePluginVS2005.Classes
{
    class AssemblyInformation
    {
        public static string ProductDescription()
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            Type attrType = typeof(AssemblyDescriptionAttribute);

            object[] attrs = currentAssembly.GetCustomAttributes(attrType,
            false);
            if (attrs.Length > 0)
            {
                AssemblyDescriptionAttribute desc =
                (AssemblyDescriptionAttribute)attrs[0];
                return desc.Description;
            }
            else
            {
                return "";
            }
        }


        public static string ProductName()
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            Type attrType = typeof(AssemblyProductAttribute);

            object[] attrs = currentAssembly.GetCustomAttributes(attrType,
            false);
            if (attrs.Length > 0)
            {
                AssemblyProductAttribute desc =
                (AssemblyProductAttribute)attrs[0];
                return desc.Product;
            }
            else
            {
                return "";
            }
        }

        public static string ProductTitle()
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            Type attrType = typeof(AssemblyTitleAttribute);

            object[] attrs = currentAssembly.GetCustomAttributes(attrType,
            false);
            if (attrs.Length > 0)
            {
                AssemblyTitleAttribute desc =
                (AssemblyTitleAttribute)attrs[0];
                return desc.Title;
            }
            else
            {
                return "";
            }
        }

        public static string ProductCompany()
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            Type attrType = typeof(AssemblyCompanyAttribute);

            object[] attrs = currentAssembly.GetCustomAttributes(attrType,
            false);
            if (attrs.Length > 0)
            {
                AssemblyCompanyAttribute desc =
                (AssemblyCompanyAttribute)attrs[0];
                return desc.Company;
            }
            else
            {
                return "";
            }
        }

        public static string ProductCopyright()
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            Type attrType = typeof(AssemblyCopyrightAttribute);

            object[] attrs = currentAssembly.GetCustomAttributes(attrType,
            false);
            if (attrs.Length > 0)
            {
                AssemblyCopyrightAttribute desc =
                (AssemblyCopyrightAttribute)attrs[0];
                return desc.Copyright;
            }
            else
            {
                return "";
            }
        }
    }
}
