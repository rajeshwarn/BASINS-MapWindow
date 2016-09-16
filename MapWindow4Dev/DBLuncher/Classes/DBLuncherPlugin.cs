//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
//limitations under the License. 
//
//The Original Code is MapWindow Labler Plug-in. 
//
//The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
//Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
//public domain in March 2004.  
//
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
//2/1/2005 - jlk - allow toolbar to be added to MapWindow main toolbar
//9/29/2005 - dpa - separated the labeler plugin from the identifier plugin to ease improvement of each.
//********************************************************************************************************

using System;
using System.Windows.Forms;	
using MapWinGIS;

namespace DBLuncher
{

	public class DBLuncherPlugin : MapWindow.Interfaces.IPlugin
	{	
		private System.Resources.ResourceManager resMan = new System.Resources.ResourceManager("DBLuncher.Resource", System.Reflection.Assembly.GetExecutingAssembly());

		public MapWindow.Interfaces.IMapWin m_MapWin;
		public bool m_Activated;	
		public MapWinGIS.tkCursorMode m_PreviousCursorMode;
		public MapWinGIS.tkCursor m_PreviousCursor;
		public bool m_MouseDown;
		public int m_hDraw;
		public int m_ParentHandle;
		private System.Drawing.Color YELLOW = System.Drawing.Color.Yellow;
		private System.Drawing.Color RED = System.Drawing.Color.Red;
		private string m_ToolbarName = "";
		
		public DBLuncherPlugin()
		{
		}

		public string Author
		{
			get
			{
				return "MapWindow Open Source Team";
			}
		}

		public string BuildDate
		{
			get
			{
				return System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location).ToString();
			}
		}

		public string Description
		{
			get
			{
				return "Contains a tools for adding labels to features in a vector (shapefile) layer.";
			}
		}

		public string Name
		{
			get
			{
				return resMan.GetString("DBLuncher.Name");
			}
		}

		public string SerialNumber
		{
			get
			{
				return "";//This function is deprecated in the open source code.
			}
		}

		public string Version
		{
			get
			{
				string major = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileMajorPart.ToString();
				string minor = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileMinorPart.ToString();
				string build = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileBuildPart.ToString();
				return major + "." + minor + "." + build;
			}
		}

		public void Initialize(MapWindow.Interfaces.IMapWin MapWin, int ParentHandle)
		{
			try
			{
				m_MapWin = MapWin;
				m_ParentHandle = ParentHandle;

				MapWindow.Interfaces.ToolbarButton button;
						
				//Add the tool bar
				if (m_ToolbarName.Length > 0)  
					m_MapWin.Toolbar.AddToolbar(m_ToolbarName);
				
				//add buttons and the tool tip
				button = m_MapWin.Toolbar.AddButton("DB Luncher",m_ToolbarName,null,null);
				button.Tooltip = "DB Luncher";
				//button.Picture = labelIcon;

			}
			catch(System.Exception ex)
			{
				ShowErrorBox("Initialize()",ex.Message);
			}
		}

		public void Terminate()
		{
			//unload the buttons
			m_MapWin.Toolbar.RemoveButton("DB Luncher");

			//unload the toolbar
			if (m_ToolbarName.Length > 0)  
				m_MapWin.Toolbar.RemoveToolbar(m_ToolbarName);

			m_MapWin = null;
		}

		public void ItemClicked(string ItemName, ref bool Handled)
		{
			try
			{
				//check to see if it was the Feature Identifier pressed
				if(ItemName == "DB Luncher")
				{
	

					//we handled this event
					Handled = true;
				}
			}
			catch(System.Exception ex)
			{
				ShowErrorBox("ItemClicked()",ex.Message);
			}
		}

		public void LayerRemoved(int Handle)
		{
		
		}

		public void LayersCleared()
		{
			
		}

		public void LayerSelected(int Handle)
		{ 
		}
		public void MapMouseDown(int Button, int Shift, int x, int y, ref bool Handled)
		{

		}

		public void MapMouseUp(int Button, int Shift, int x, int y, ref bool Handled)
		{
		}

		public void MapMouseMove(int ScreenX, int ScreenY, ref bool Handled)
		{

		}
		
		public void MapDragFinished(System.Drawing.Rectangle Bounds, ref bool Handled)
		{
		
		}

		public void LegendDoubleClick(int Handle, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)
		{
		
		}

		public void LegendMouseDown(int Handle, int Button, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)
		{
		
		}

		public void LegendMouseUp(int Handle, int Button, MapWindow.Interfaces.ClickLocation Location, ref bool Handled)
		{
		
		}

		public void MapExtentsChanged()
		{
			
		}

		public void Message(string msg, ref bool Handled)
		{
			
		}

		public void ProjectLoading(string ProjectFile, string SettingsString)
		{

		}

		public void ProjectSaving(string ProjectFile, ref string SettingsString)
		{

		}

		public void ShapesSelected(int Handle, MapWindow.Interfaces.SelectInfo SelectInfo)
		{
			
		}

		private void LoadLayer()
		{

		}

		private void ShowErrorBox(string functionName,string errorMsg)
		{
			MessageBox.Show("Error in " + functionName + ", Message: " + errorMsg,"Feature Identifier",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Error);
		}

		public void LayersAdded(MapWindow.Interfaces.Layer[] Layers)
		{

		}
	
	}
	

}
