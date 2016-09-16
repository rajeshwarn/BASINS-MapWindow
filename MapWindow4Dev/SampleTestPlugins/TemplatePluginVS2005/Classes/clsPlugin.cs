//********************************************************************************************************
//File Name: clsPlugin.cs
//Description: This class holds all code used in this plug-in
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
using MapWindow.Interfaces;

namespace TemplatePluginVS2005.Classes
{
    public class pluginCode
    {
        private MapWindow.Interfaces.IMapWin _mapWin;

        /// <summary>
        /// Constructor
        /// </summary>
        public pluginCode(MapWindow.Interfaces.IMapWin MapWin)
        {
            _mapWin = MapWin;
        }

        public void fillVisibleLayers(TemplatePluginVS2005.Forms.frmLayers frm)
        {
            int selectedIndex = 0;
            int counter = 0;
            frm.cboLayers.Items.Clear();

            //Add every visible layer to the combobox:
            for (int i = 0; i < _mapWin.Layers.NumLayers; i++)
            {
                int layerHandle = _mapWin.Layers.GetHandle(i);
                if (_mapWin.Layers[layerHandle].Visible)
                {
                    string layerName = _mapWin.Layers[layerHandle].Name;
                    eLayerType layerType = _mapWin.Layers[layerHandle].LayerType;
                    MyLayersList myListItem = new MyLayersList(layerName, layerHandle, layerType);
                    frm.cboLayers.Items.Add(myListItem);
                    if (_mapWin.Layers.CurrentLayer == layerHandle)
                        selectedIndex = counter;
                    counter++;
                }                
            }
            if (selectedIndex > 0)
                frm.cboLayers.SelectedIndex = selectedIndex;
        }
    }

    /// <summary>
    /// Used to fill the layers combobox
    /// </summary>
    public class MyLayersList
    {
        private string _name;
        private int _layerHandle;
        private eLayerType _layerType;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="layerHandle"></param>
        /// <param name="layerType"></param>
        /// <param name="mapUnits"></param>
        public MyLayersList(string name, int layerHandle, eLayerType layerType)
        {
            _name = name;
            _layerHandle = layerHandle;
            _layerType = layerType;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int LayerHandle
        {
            get { return _layerHandle; }
            set { _layerHandle = value; }
        }

        public eLayerType LayerType
        {
            get { return _layerType; }
            set { _layerType = value; }
        }

        // This is neccessary because the ListBox and ComboBox rely 
        // on this method when determining the text to display. 
        public override string ToString()
        {
            return _name;
        }

    }
}