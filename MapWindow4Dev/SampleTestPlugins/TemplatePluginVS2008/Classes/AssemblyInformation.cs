// ********************************************************************************************************
// <copyright file="AssemblyInformation.cs" company="Bontepaarden.nl">
//     Copyright (c) Bontepaarden.nl. All rights reserved.
// </copyright>
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http:// www.mozilla.org/MPL/ 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
// 
// The Original Code is MapWindow Open Source MeemsTools Plug-in. 
// 
// The Initial Developer of this version of the Original Code is Paul Meems.
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//  Change Log: 
//  Date           Changed By      Notes
//  13 April 2008  Paul Meems      Inital upload the MW SVN repository
//  28 August 2009 Paul Meems      Made the code StyleCop compliant
//  5  May 2011    Paul Meems      Made changes as suggested by ReSharper   
// ********************************************************************************************************

// This is a very generic class. Would be good to add it to the MapWinUtillity class
namespace TemplatePluginVS2008.Classes
{
    #region Usings

    using System.Reflection;

    #endregion

    /// <summary>
    ///   This class return Assembly info from the AssemblyInfo.cs
    /// </summary>
    public class AssemblyInformation
    {
        /// <summary>
        ///   The executing assembly
        /// </summary>
        private static readonly Assembly CurrentAssembly = Assembly.GetExecutingAssembly();

        /// <summary>
        ///   Gets the product description from the assembly info
        /// </summary>
        /// <returns>The product description</returns>
        public static string ProductDescription()
        {
            // C#3.0: Implicitly Typed Local Variables and Arrays:
            var attrType = typeof(AssemblyDescriptionAttribute);

            var attrs = CurrentAssembly.GetCustomAttributes(attrType, false);
            if (attrs.Length > 0)
            {
                //// C#3.0: Implicitly Typed Local Variables and Arrays:
                var desc = (AssemblyDescriptionAttribute)attrs[0];
                return desc.Description;
            }
            
            return string.Empty;
        }

        /// <summary>
        ///   Gets the product name from the assembly info
        /// </summary>
        /// <returns>The product name</returns>
        public static string ProductName()
        {
            // C#3.0: Implicitly Typed Local Variables and Arrays:
            var attrType = typeof(AssemblyProductAttribute);

            var attrs = CurrentAssembly.GetCustomAttributes(attrType, false);
            if (attrs.Length > 0)
            {
                //// C#3.0: Implicitly Typed Local Variables and Arrays:
                var desc = (AssemblyProductAttribute)attrs[0];
                return desc.Product;
            }
            
            return string.Empty;
        }

        /// <summary>
        ///   Gets the product title from the assembly info
        /// </summary>
        /// <returns>The product title</returns>
        public static string ProductTitle()
        {
            // C#3.0: Implicitly Typed Local Variables and Arrays:
            var attrType = typeof(AssemblyTitleAttribute);

            var attrs = CurrentAssembly.GetCustomAttributes(attrType, false);
            if (attrs.Length > 0)
            {
                //// C#3.0: Implicitly Typed Local Variables and Arrays:
                var desc = (AssemblyTitleAttribute)attrs[0];
                return desc.Title;
            }
            
            return string.Empty;
        }

        /// <summary>
        ///   Gets the product company from the assembly info
        /// </summary>
        /// <returns>The product company</returns>
        public static string ProductCompany()
        {
            // C#3.0: Implicitly Typed Local Variables and Arrays:
            var attrType = typeof(AssemblyCompanyAttribute);

            var attrs = CurrentAssembly.GetCustomAttributes(attrType, false);
            if (attrs.Length > 0)
            {
                //// C#3.0: Implicitly Typed Local Variables and Arrays:
                var desc = (AssemblyCompanyAttribute)attrs[0];
                return desc.Company;
            }
            
            return string.Empty;
        }

        /// <summary>
        ///   Gets the product copyright from the assembly info
        /// </summary>
        /// <returns>The product copyright</returns>
        public static string ProductCopyright()
        {
            // C#3.0: Implicitly Typed Local Variables and Arrays:
            var attrType = typeof(AssemblyCopyrightAttribute);

            var attrs = CurrentAssembly.GetCustomAttributes(attrType, false);
            if (attrs.Length > 0)
            {
                //// C#3.0: Implicitly Typed Local Variables and Arrays:
                var desc = (AssemblyCopyrightAttribute)attrs[0];
                return desc.Copyright;
            }
            
            return string.Empty;
        }
    }
}
