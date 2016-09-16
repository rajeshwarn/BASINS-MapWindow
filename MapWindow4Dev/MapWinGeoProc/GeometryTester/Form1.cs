using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;
using System.IO;
using MapWinGIS;
using MapWinGeoProc;
using AxMapWinGIS;
using System.Text;

namespace GeometryTester
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnClipPolygonsWithPolygon;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label lblNumber;
		private System.Windows.Forms.Button btnClipPointsWithPolygon;
		private System.Windows.Forms.Button btnClipLinesWithPolygon;
		private System.Windows.Forms.Button btnClipPolygonWithLine;
		private System.Windows.Forms.Button btnClipGridWithPolygon;
		private System.Windows.Forms.Button btnConvertPt;
		private System.Windows.Forms.Button btnProjectGrid;
		private System.Windows.Forms.Button btnReProjectGrid;
		private System.Windows.Forms.Button btnClipShapesWithPolygon;
		private System.Windows.Forms.Button btnMultiPart;
		private System.Windows.Forms.Button btnGetArea;
		private System.Windows.Forms.Button btnClipPolysWithLines;
		private System.Windows.Forms.Button btnSelectWithPolygon;
		private System.Windows.Forms.Button btnCentroid;
		private System.Windows.Forms.Button btnShapeMerge;
		private AxMapWinGIS.AxMap axMap1;
		private System.Windows.Forms.Button ErasePolySF;
		private System.Windows.Forms.Button btnBufferPoints;
		MapWinGIS.Image image = new MapWinGIS.ImageClass();

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.label1 = new System.Windows.Forms.Label();
			this.btnClipPolygonsWithPolygon = new System.Windows.Forms.Button();
			this.lblNumber = new System.Windows.Forms.Label();
			this.btnClipPointsWithPolygon = new System.Windows.Forms.Button();
			this.btnClipLinesWithPolygon = new System.Windows.Forms.Button();
			this.btnClipPolygonWithLine = new System.Windows.Forms.Button();
			this.btnClipGridWithPolygon = new System.Windows.Forms.Button();
			this.btnConvertPt = new System.Windows.Forms.Button();
			this.btnProjectGrid = new System.Windows.Forms.Button();
			this.btnReProjectGrid = new System.Windows.Forms.Button();
			this.btnClipShapesWithPolygon = new System.Windows.Forms.Button();
			this.btnMultiPart = new System.Windows.Forms.Button();
			this.btnGetArea = new System.Windows.Forms.Button();
			this.btnClipPolysWithLines = new System.Windows.Forms.Button();
			this.btnSelectWithPolygon = new System.Windows.Forms.Button();
			this.btnCentroid = new System.Windows.Forms.Button();
			this.btnShapeMerge = new System.Windows.Forms.Button();
			this.axMap1 = new AxMapWinGIS.AxMap();
			this.ErasePolySF = new System.Windows.Forms.Button();
			this.btnBufferPoints = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.axMap1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(56, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(152, 40);
			this.label1.TabIndex = 1;
			this.label1.Text = "Testing Program for MapWinGeoProc functions";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnClipPolygonsWithPolygon
			// 
			this.btnClipPolygonsWithPolygon.Location = new System.Drawing.Point(56, 72);
			this.btnClipPolygonsWithPolygon.Name = "btnClipPolygonsWithPolygon";
			this.btnClipPolygonsWithPolygon.Size = new System.Drawing.Size(152, 32);
			this.btnClipPolygonsWithPolygon.TabIndex = 2;
			this.btnClipPolygonsWithPolygon.Text = "ClipPolygonsWithPolygon()";
			this.btnClipPolygonsWithPolygon.Click += new System.EventHandler(this.btnClipPolygonsWithPolygon_Click);
			// 
			// lblNumber
			// 
			this.lblNumber.Location = new System.Drawing.Point(80, 352);
			this.lblNumber.Name = "lblNumber";
			this.lblNumber.Size = new System.Drawing.Size(256, 64);
			this.lblNumber.TabIndex = 4;
			this.lblNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnClipPointsWithPolygon
			// 
			this.btnClipPointsWithPolygon.Location = new System.Drawing.Point(56, 112);
			this.btnClipPointsWithPolygon.Name = "btnClipPointsWithPolygon";
			this.btnClipPointsWithPolygon.Size = new System.Drawing.Size(152, 32);
			this.btnClipPointsWithPolygon.TabIndex = 5;
			this.btnClipPointsWithPolygon.Text = "ClipPointsWithPolygon()";
			this.btnClipPointsWithPolygon.Click += new System.EventHandler(this.btnClipPointsWithPolygon_Click);
			// 
			// btnClipLinesWithPolygon
			// 
			this.btnClipLinesWithPolygon.Location = new System.Drawing.Point(56, 152);
			this.btnClipLinesWithPolygon.Name = "btnClipLinesWithPolygon";
			this.btnClipLinesWithPolygon.Size = new System.Drawing.Size(152, 32);
			this.btnClipLinesWithPolygon.TabIndex = 6;
			this.btnClipLinesWithPolygon.Text = "ClipLinesWithPolygon()";
			this.btnClipLinesWithPolygon.Click += new System.EventHandler(this.btnClipLinesWithPolygon_Click);
			// 
			// btnClipPolygonWithLine
			// 
			this.btnClipPolygonWithLine.Location = new System.Drawing.Point(56, 192);
			this.btnClipPolygonWithLine.Name = "btnClipPolygonWithLine";
			this.btnClipPolygonWithLine.Size = new System.Drawing.Size(152, 32);
			this.btnClipPolygonWithLine.TabIndex = 9;
			this.btnClipPolygonWithLine.Text = "ClipPolygonWithLine()";
			this.btnClipPolygonWithLine.Click += new System.EventHandler(this.btnClipPolygonWithLine_Click);
			// 
			// btnClipGridWithPolygon
			// 
			this.btnClipGridWithPolygon.Location = new System.Drawing.Point(56, 232);
			this.btnClipGridWithPolygon.Name = "btnClipGridWithPolygon";
			this.btnClipGridWithPolygon.Size = new System.Drawing.Size(152, 32);
			this.btnClipGridWithPolygon.TabIndex = 10;
			this.btnClipGridWithPolygon.Text = "ClipGridWithPolygon()";
			this.btnClipGridWithPolygon.Click += new System.EventHandler(this.btnClipGridWithPolygon_Click);
			// 
			// btnConvertPt
			// 
			this.btnConvertPt.Location = new System.Drawing.Point(56, 272);
			this.btnConvertPt.Name = "btnConvertPt";
			this.btnConvertPt.Size = new System.Drawing.Size(152, 32);
			this.btnConvertPt.TabIndex = 11;
			this.btnConvertPt.Text = "ProjectShapefile";
			this.btnConvertPt.Click += new System.EventHandler(this.btnConvertPt_Click);
			// 
			// btnProjectGrid
			// 
			this.btnProjectGrid.Location = new System.Drawing.Point(56, 312);
			this.btnProjectGrid.Name = "btnProjectGrid";
			this.btnProjectGrid.Size = new System.Drawing.Size(152, 32);
			this.btnProjectGrid.TabIndex = 12;
			this.btnProjectGrid.Text = "ProjectGrid";
			this.btnProjectGrid.Click += new System.EventHandler(this.btnProjectGrid_Click);
			// 
			// btnReProjectGrid
			// 
			this.btnReProjectGrid.Location = new System.Drawing.Point(224, 312);
			this.btnReProjectGrid.Name = "btnReProjectGrid";
			this.btnReProjectGrid.Size = new System.Drawing.Size(120, 32);
			this.btnReProjectGrid.TabIndex = 13;
			this.btnReProjectGrid.Text = "ReProjectGrid";
			this.btnReProjectGrid.Click += new System.EventHandler(this.btnReProjectGrid_Click);
			// 
			// btnClipShapesWithPolygon
			// 
			this.btnClipShapesWithPolygon.Location = new System.Drawing.Point(224, 72);
			this.btnClipShapesWithPolygon.Name = "btnClipShapesWithPolygon";
			this.btnClipShapesWithPolygon.Size = new System.Drawing.Size(120, 32);
			this.btnClipShapesWithPolygon.TabIndex = 16;
			this.btnClipShapesWithPolygon.Text = "ClipShapesWith Polygon()";
			this.btnClipShapesWithPolygon.Click += new System.EventHandler(this.btnClipShapesWithPolygon_Click);
			// 
			// btnMultiPart
			// 
			this.btnMultiPart.Location = new System.Drawing.Point(224, 120);
			this.btnMultiPart.Name = "btnMultiPart";
			this.btnMultiPart.Size = new System.Drawing.Size(120, 32);
			this.btnMultiPart.TabIndex = 17;
			this.btnMultiPart.Text = "MultiPartShapes";
			this.btnMultiPart.Click += new System.EventHandler(this.btnMultiPart_Click);
			// 
			// btnGetArea
			// 
			this.btnGetArea.Location = new System.Drawing.Point(224, 168);
			this.btnGetArea.Name = "btnGetArea";
			this.btnGetArea.Size = new System.Drawing.Size(120, 32);
			this.btnGetArea.TabIndex = 18;
			this.btnGetArea.Text = "get_Area()";
			this.btnGetArea.Click += new System.EventHandler(this.btnGetArea_Click);
			// 
			// btnClipPolysWithLines
			// 
			this.btnClipPolysWithLines.Location = new System.Drawing.Point(224, 216);
			this.btnClipPolysWithLines.Name = "btnClipPolysWithLines";
			this.btnClipPolysWithLines.Size = new System.Drawing.Size(120, 32);
			this.btnClipPolysWithLines.TabIndex = 19;
			this.btnClipPolysWithLines.Text = "ClipPolysWithLines()";
			this.btnClipPolysWithLines.Click += new System.EventHandler(this.btnClipPolysWithLines_Click);
			// 
			// btnSelectWithPolygon
			// 
			this.btnSelectWithPolygon.Location = new System.Drawing.Point(224, 264);
			this.btnSelectWithPolygon.Name = "btnSelectWithPolygon";
			this.btnSelectWithPolygon.Size = new System.Drawing.Size(120, 32);
			this.btnSelectWithPolygon.TabIndex = 20;
			this.btnSelectWithPolygon.Text = "SelectWithPolygon()";
			this.btnSelectWithPolygon.Click += new System.EventHandler(this.btnSelectWithPolygon_Click);
			// 
			// btnCentroid
			// 
			this.btnCentroid.Location = new System.Drawing.Point(224, 24);
			this.btnCentroid.Name = "btnCentroid";
			this.btnCentroid.Size = new System.Drawing.Size(120, 32);
			this.btnCentroid.TabIndex = 21;
			this.btnCentroid.Text = "Centroid()";
			this.btnCentroid.Click += new System.EventHandler(this.btnCentroid_Click);
			// 
			// btnShapeMerge
			// 
			this.btnShapeMerge.Location = new System.Drawing.Point(368, 24);
			this.btnShapeMerge.Name = "btnShapeMerge";
			this.btnShapeMerge.Size = new System.Drawing.Size(120, 32);
			this.btnShapeMerge.TabIndex = 22;
			this.btnShapeMerge.Text = "ShapeMerge()";
			this.btnShapeMerge.Click += new System.EventHandler(this.btnShapeMerge_Click);
			// 
			// axMap1
			// 
			this.axMap1.Enabled = true;
			this.axMap1.Location = new System.Drawing.Point(416, 256);
			this.axMap1.Name = "axMap1";
			this.axMap1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMap1.OcxState")));
			this.axMap1.Size = new System.Drawing.Size(216, 160);
			this.axMap1.TabIndex = 23;
			// 
			// ErasePolySF
			// 
			this.ErasePolySF.Location = new System.Drawing.Point(368, 72);
			this.ErasePolySF.Name = "ErasePolySF";
			this.ErasePolySF.Size = new System.Drawing.Size(120, 32);
			this.ErasePolySF.TabIndex = 24;
			this.ErasePolySF.Text = "ErasePolySF()";
			this.ErasePolySF.Click += new System.EventHandler(this.ErasePolySF_Click);
			// 
			// btnBufferPoints
			// 
			this.btnBufferPoints.Location = new System.Drawing.Point(368, 120);
			this.btnBufferPoints.Name = "btnBufferPoints";
			this.btnBufferPoints.Size = new System.Drawing.Size(120, 32);
			this.btnBufferPoints.TabIndex = 25;
			this.btnBufferPoints.Text = "BufferPoints()";
			this.btnBufferPoints.Click += new System.EventHandler(this.btnBufferPoints_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(688, 430);
			this.Controls.Add(this.btnBufferPoints);
			this.Controls.Add(this.ErasePolySF);
			this.Controls.Add(this.axMap1);
			this.Controls.Add(this.btnShapeMerge);
			this.Controls.Add(this.btnCentroid);
			this.Controls.Add(this.btnSelectWithPolygon);
			this.Controls.Add(this.btnClipPolysWithLines);
			this.Controls.Add(this.btnGetArea);
			this.Controls.Add(this.btnMultiPart);
			this.Controls.Add(this.btnClipShapesWithPolygon);
			this.Controls.Add(this.btnReProjectGrid);
			this.Controls.Add(this.btnProjectGrid);
			this.Controls.Add(this.btnConvertPt);
			this.Controls.Add(this.btnClipGridWithPolygon);
			this.Controls.Add(this.btnClipPolygonWithLine);
			this.Controls.Add(this.btnClipLinesWithPolygon);
			this.Controls.Add(this.btnClipPointsWithPolygon);
			this.Controls.Add(this.lblNumber);
			this.Controls.Add(this.btnClipPolygonsWithPolygon);
			this.Controls.Add(this.label1);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.axMap1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		#region DeleteShapeFile()
		//this function is used to delete temporary shapefiles that were
		//saved to disk. There are three files for every one shapefile, so
		//all three must be deleted.
		private void DeleteShapeFile(string shapeFilePath)
		{
			System.IO.File.Delete(shapeFilePath + ".shp");
			System.IO.File.Delete(shapeFilePath + ".dbf");
			System.IO.File.Delete(shapeFilePath + ".shx");
		}
		#endregion

		#region ClipPolygonsWithPolygon() test
		private void btnClipPolygonsWithPolygon_Click(object sender, System.EventArgs e)
		{
			string polySFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\counties.shp";
			string resultSFPath = @"C:\resultSF.shp";
			MapWinGIS.Shapefile clipperPolySF = new MapWinGIS.ShapefileClass();
			clipperPolySF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\ClipperPolygonSF.shp", null);
			int numClipperShapes = clipperPolySF.NumShapes;
			MapWinGIS.Shape shp = clipperPolySF.get_Shape(0);
			bool status;

			status = MapWinGeoProc.SpatialOperations.ClipShapesWithPolygon(ref polySFPath, ref shp, ref resultSFPath, false);
			
			clipperPolySF.Close();
			Debug.WriteLine("Done, function returned " + status);
		}
		#endregion

		#region ClipPointsWithPolygon() test
		private void btnClipPointsWithPolygon_Click(object sender, System.EventArgs e)
		{
			string pointSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\pointSF.shp";
			string resultSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\resultSF.shp";
			MapWinGIS.Shapefile polySF = new MapWinGIS.ShapefileClass();
			polySF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\ClipperPolygonSF.shp", null);
			int numClipperShapes = polySF.NumShapes;
			MapWinGIS.Shape polygon = polySF.get_Shape(5);
			bool status;

			//******************** save-to-disk versions ***************
			status = MapWinGeoProc.SpatialOperations.ClipShapesWithPolygon(ref pointSFPath, ref polygon, ref resultSFPath, false);
			//status = MapWinGeoProc.SpatialOperations.ClipShapesWithPolygon(ref pointSFPath, ref polygon, ref resultSFPath);
			polySF.Close();
			Debug.WriteLine("Done, function returned " + status);

			//*********** save-to-memory versions *********************
//			DeleteShapeFile(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\resultSF");
//			MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
//			resultSF.CreateNew(resultSFPath, MapWinGIS.ShpfileType.SHP_POINT);
//			MapWinGIS.Shapefile pointSF = new MapWinGIS.ShapefileClass();
//			pointSF.Open(pointSFPath, null);
//			status = MapWinGeoProc.SpatialOperations.ClipShapesWithPolygon(ref pointSF, ref polygon, out resultSF, false);
//			//status = MapWinGeoProc.SpatialOperations.ClipShapesWithPolygon(ref pointSF, ref polygon, out resutlSF);
//			resultSF.StopEditingShapes(true, true, false);
//			resultSF.SaveAs(resultSFPath, null);
//			pointSF.Close();
//			polySF.Close();
//			Debug.WriteLine("Done, function returned " + status);		
		}
		#endregion

		#region ClipLinesWithPolygon() test
		private void btnClipLinesWithPolygon_Click(object sender, System.EventArgs e)
		{
			//*** Watershed example ***
//			string streamPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\loriv2.shp";
//			string watershedPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\lo.shp";
//			string resultSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\result.shp";
//			MapWinGIS.Shapefile polySF = new MapWinGIS.ShapefileClass();
//			polySF.Open(watershedPath, null);
//			int numPolyShapes = polySF.NumShapes;
//			MapWinGIS.Shape polygon = new MapWinGIS.ShapeClass();
//			polygon = polySF.get_Shape(6);
//			bool status = MapWinGeoProc.SpatialOperations.ClipShapesWithPolygon(ref streamPath, ref polygon, ref resultSFPath, true);			
//			polySF.Close();	
//			Debug.WriteLine("Done, function returned " + status);
			
			//*** Simple Shape example ***
			string lineSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\lineSF.shp";
			string polySFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\ClipperPolygonSF.shp"; 
			string resultSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\resultSF.shp";
			MapWinGIS.Shapefile polySF = new MapWinGIS.ShapefileClass();
			polySF.Open(polySFPath, null);
			int numPolyShapes = polySF.NumShapes;
			MapWinGIS.Shape polygon = new MapWinGIS.ShapeClass();
//			polygon = polySF.get_Shape(5);
//			polygon = polySF.get_Shape(4);
//			polygon = polySF.get_Shape(3);
//			polygon = polySF.get_Shape(2);
//			polygon = polySF.get_Shape(1);
			polygon = polySF.get_Shape(0);
			bool status = MapWinGeoProc.SpatialOperations.ClipShapesWithPolygon(ref lineSFPath, ref polygon, ref resultSFPath, false);
			polySF.Close();	
			Debug.WriteLine("Done, function returned " + status);
		}
		#endregion

		#region ClipPolygonWithLine() test
		private void btnClipPolygonWithLine_Click(object sender, System.EventArgs e)
		{
			bool status;
			string resultSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\resultSF.shp";			
			MapWinGIS.Shapefile lineSF = new MapWinGIS.ShapefileClass();
			MapWinGIS.Shapefile polygonSF = new MapWinGIS.ShapefileClass();

			//SPEED OPTIMIZED TESTS
//			lineSF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\lineSF.shp", null);
//				MapWinGIS.Shape line = lineSF.get_Shape(2);// 2 sections
////			MapWinGIS.Shape line = lineSF.get_Shape(6);// 0 sections
//			polygonSF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\ClipperPolygonSF.shp", null);
//			MapWinGIS.Shape polygon = polygonSF.get_Shape(0); //diamond shape
//			status = MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(ref polygon, ref line, ref resultSFPath, true);

//			lineSF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\lineSF.shp", null);
//			MapWinGIS.Shape line = lineSF.get_Shape(0);// 3 sections
//			polygonSF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\ClipperPolygonSF.shp", null);
//			MapWinGIS.Shape polygon = polygonSF.get_Shape(1); //vert rectangle shape
//			status = MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(ref polygon, ref line, ref resultSFPath, true);

//			lineSF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\lineSF.shp", null);
////			MapWinGIS.Shape line = lineSF.get_Shape(1);// 0 sections
//				MapWinGIS.Shape line = lineSF.get_Shape(4);// 2 sections
////			MapWinGIS.Shape line = lineSF.get_Shape(5);// 0 sections
//			polygonSF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\ClipperPolygonSF.shp", null);
//			MapWinGIS.Shape polygon = polygonSF.get_Shape(3); //horiz rectangle shape
//			status = MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(ref polygon, ref line, ref resultSFPath, true);

//			lineSF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\lineSF.shp", null);
//			MapWinGIS.Shape line = lineSF.get_Shape(16);// 6 sections
//			polygonSF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\ClipperPolygonSF.shp", null);
//			MapWinGIS.Shape polygon = polygonSF.get_Shape(2); //triangle shape
//			status = MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(ref polygon, ref line, ref resultSFPath, true);


			//ACCURACY TESTS
//			lineSF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\ClipperLineSF.shp", null);
//			MapWinGIS.Shape line = lineSF.get_Shape(0);
//			polygonSF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\ClipperPolygonSF.shp", null);
//			MapWinGIS.Shape polygon = polygonSF.get_Shape(1);
//			status = MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(ref polygon, ref line, ref resultSFPath, false);

//			lineSF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\countyCutter.shp", null);
//			MapWinGIS.Shape line = lineSF.get_Shape(0); //use with countyCutter
//			polygonSF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\counties.shp", null);
//			MapWinGIS.Shape polygon = polygonSF.get_Shape(2);
//			status = MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(ref polygon, ref line, ref resultSFPath, false);
			
//			lineSF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\lineSF.shp", null);
//	//			MapWinGIS.Shape line = lineSF.get_Shape(8); //out->out, 3 sections
//	//			MapWinGIS.Shape line = lineSF.get_Shape(9); //in->in, 2 sections
//				MapWinGIS.Shape line = lineSF.get_Shape(10); //squiggle, 5 sections
//	//			MapWinGIS.Shape line = lineSF.get_Shape(11); //squiggle, 3 sections
//	//			MapWinGIS.Shape line = lineSF.get_Shape(12); //squiggle, 5 sections
//			polygonSF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\ClipperPolygonSF.shp", null);
//			MapWinGIS.Shape polygon = polygonSF.get_Shape(4);
//			status = MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(ref polygon, ref line, ref resultSFPath, false);
			
			lineSF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\lineSF.shp", null);
//				MapWinGIS.Shape line = lineSF.get_Shape(13); //in->in, 4 sections
//				MapWinGIS.Shape line = lineSF.get_Shape(14); //out->out, 4 sections
				MapWinGIS.Shape line = lineSF.get_Shape(15); //squiggle, 19 sections
			polygonSF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\ClipperPolygonSF.shp", null);
			MapWinGIS.Shape polygon = polygonSF.get_Shape(5);					
			status = MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(ref polygon, ref line, ref resultSFPath, false);
			
			lineSF.Close();
			polygonSF.Close();
			
			MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
			resultSF.Open(resultSFPath, null);
			
			int numResultShapes = resultSF.NumShapes;
			string numPts = "";
			string pts = "";
			for(int i= 0; i<= numResultShapes-1; i++)
			{
				numPts = "shp" + i + " has " + resultSF.get_Shape(i).numPoints + " points.";
				for(int j = 0; j <= resultSF.get_Shape(i).numPoints-1; j++)
				{
					pts += "(" + resultSF.get_Shape(i).get_Point(j).x + ", " + resultSF.get_Shape(i).get_Point(j).y + ") ";
				}
				Debug.WriteLine(numPts);
				Debug.WriteLine(pts);
				pts = "";
			}
			lblNumber.Text = " numResultShapes: " + numResultShapes;
			resultSF.Close();
			Debug.WriteLine("Done");
		}
		#endregion

		#region ClipGridWithPolygon Test
		private void btnClipGridWithPolygon_Click(object sender, System.EventArgs e)
		{
			string gridFile = @"C:\Dev\MapWinGeoProc\GeometryTester\GridFiles\clippeddem.bgd";
			string polyFile = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\gridClipper.shp";
			string resultPath = @"C:\Dev\MapWinGeoProc\GeometryTester\GridFiles\";
			MapWinGIS.Shapefile polygons = new MapWinGIS.ShapefileClass();
			MapWinGIS.Shape polygon = new MapWinGIS.ShapeClass();
			polygons.Open(polyFile, null);
			int numPolys = polygons.NumShapes;
			bool success = true;

			for(int i =0; i <=  numPolys-1; i++)
			{
				string resultFile = resultPath + "rGrid_" + i + ".bgd";
				polygon = polygons.get_Shape(i);

				if(i == 0)//diamond
				{	
					//Trim result
					if(MapWinGeoProc.SpatialOperations.ClipGridWithPolygon(ref gridFile, ref polygon, ref resultFile, true) == false)
					{
						success = false;
						break;
					}
				}
				if(i == 1)//star shape
				{	
					//Do NOT trim result
					if(MapWinGeoProc.SpatialOperations.ClipGridWithPolygon(ref gridFile, ref polygon, ref resultFile, false) ==false)
					{
						success = false;
						break;
					}
					//Now trim result for comparison
					resultFile = resultPath + "rGrid_1Trimmed.bgd";
					if(MapWinGeoProc.SpatialOperations.ClipGridWithPolygon(ref gridFile, ref polygon, ref resultFile, true) == false)
					{
						success = false;
						break;
					}
				}
				else //blob and crescent shape
				{	
					//Result trimmed by default
					if(MapWinGeoProc.SpatialOperations.ClipGridWithPolygon(ref gridFile, ref polygon, ref resultFile) == false)
					{
						success = false;
						break;
					}
				}
			}
			if(success == false)
			{
				string errorMsg = MapWinGeoProc.Error.GetLastErrorMsg();
				Debug.WriteLine("ErrorMsg: " + errorMsg);
			}
			
			Debug.WriteLine("Done");			
		}
		#endregion
		
		#region ProjectShapefile() test
		//actually tests ProjectShapefile()
		private void btnConvertPt_Click(object sender, System.EventArgs e)
		{
			DataLog dl = new DataLog();
			StringBuilder sb = new StringBuilder();
			string inputSF = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\counties.shp";
			string resultSF = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\PROJcounties.shp";
			//			MapWinGIS.Shapefile sf = new MapWinGIS.ShapefileClass();
			//			sf.Open(pointSF, null);
			//			int numPoints = sf.NumShapes;
			bool status = true;
			
			//# NAD_1983_UTM_Zone_12N <26912>
			string sourceProj = "+proj=utm +zone=12 +ellps=GRS80 +datum=NAD83 +units=m";
			//# USA_Contiguous_Albers_Equal_Area_Conic <102003>
			string destProj = "+proj=aea +lat_1=29.500000000 +lat_2=45.500000000 +lat_0=37.500000000 +lon_0=-96.000000000 +x_0=0.000 +y_0=0.000 +ellps=GRS80 +datum=NAD83 +units=m";
			//# World_Robinson <54030>
			//string destProj = "+proj=robin +lon_0=0.000000000 +x_0=0.000 +y_0=0.000 +ellps=WGS84 +datum=WGS84 +units=m";
			//# GCS_WGS_1984 <4326> 
			//string destProj = "+proj=longlat +ellps=WGS84 +datum=WGS84";

			//status = MapWinGeoProc.SpatialReference.ProjectShapefile(ref sourceProj, ref destProj, ref inputSF, ref resultSF);
			//to test overwrite version:
			//			MapWinGIS.Shapefile sf = new MapWinGIS.ShapefileClass();
			//			sf.Open(inputSF, null);
			//			status = MapWinGeoProc.SpatialReference.ProjectShapefile(ref sourceProj, ref destProj, ref sf);
			//			sf.Close();

			//to test in-memory version:
			MapWinGIS.Shapefile sf = new MapWinGIS.ShapefileClass();
			sf.Open(inputSF, null);
			MapWinGIS.Shapefile result = new MapWinGIS.ShapefileClass();
			status = MapWinGeoProc.SpatialReference.ProjectShapefile(ref sourceProj, ref destProj, ref sf, out result);
			result.SaveAs(resultSF, null);

			//			if(status == true)
			//			{
			//				MapWinGIS.Shapefile result = new MapWinGIS.ShapefileClass();
			//				MapWinGIS.Point pt, resultPt;
			//				result.Open(resultSF, null);
			//				int numShapes = result.NumShapes;
			//				for(int i = 0; i <= numShapes-1; i++)
			//				{
			//					pt = new MapWinGIS.PointClass();
			//					resultPt = new MapWinGIS.PointClass();
			//					pt = sf.get_Shape(i).get_Point(0);
			//					resultPt = result.get_Shape(i).get_Point(0);
			//					sb.Append( i + "(" + pt.x + ", " + pt.y + ") converted to: (" + resultPt.x + ", " + resultPt.y + ")/n");
			//				}
			//				dl.LogData(sb.ToString());	
			//				result.Close();
			//			}
			//			sf.Close();
		
			Debug.WriteLine("done");
		}
		#endregion

		#region ProjectGrid() test
		private void btnProjectGrid_Click(object sender, System.EventArgs e)
		{
			bool status;

			//nasty, big grid
			string sourceProj = "+proj=longlat +datum=NAD83";
			//# USA_Contiguous_Albers_Equal_Area_Conic <102003>
			string destProj = "+proj=aea +lat_1=29.500000000 +lat_2=45.500000000 +lat_0=37.500000000 +lon_0=-96.000000000 +x_0=0.000 +y_0=0.000 +ellps=GRS80 +datum=NAD83 +units=m";
			string inputGrid = @"C:\Documents and Settings\Angela\Desktop\02060006ned\02060006ned.tif";	//in longlat
			string resultGrid = @"C:\Documents and Settings\Angela\Desktop\02060006ned\PROJned.tif";
			System.TimeSpan startTime = new TimeSpan(System.DateTime.Now.Ticks);
			//Debug.WriteLine("Start time: " + System.DateTime.Now.TimeOfDay.ToString());
			status = MapWinGeoProc.SpatialReference.ProjectGrid(ref sourceProj, ref destProj, ref inputGrid, ref resultGrid, true);
			//Debug.WriteLine("End time: " + System.DateTime.Now.TimeOfDay.ToString());
			System.TimeSpan endTime = new TimeSpan(System.DateTime.Now.Ticks);
			System.TimeSpan elapsedTime = new TimeSpan((endTime.Ticks - startTime.Ticks));
			Debug.WriteLine("Total minutes: " + elapsedTime.TotalMinutes.ToString() + " minutes");
			Debug.WriteLine("Total seconds: " + elapsedTime.TotalSeconds.ToString() + " seconds");

//			//Aquaterra source projection
//			//string sourceProj = "+proj=longlat +ellps=sphere +lon_0=0 +lat_0=0 +h=0 +datum=NAD83";
//			//string sourceProj = "+proj=longlat +datum=NAD83";	//equivalent to above
//			//Aquaterra destination projection
//			//string destProj = "+proj=aea +ellps=clrk66 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m";
//			//# NAD_1983_UTM_Zone_12N <26912>
//			
//			
//			string sourceProj = "+proj=utm +zone=12 +ellps=GRS80 +datum=NAD83 +units=m";			
//			//# NAD_1983_UTM_Zone_11N
//			//string destProj = "+proj=utm +zone=11 +ellps=GRS80 +datum=NAD83 +units=m";
//			//# USA_Contiguous_Albers_Equal_Area_Conic <102003>
//			//string destProj = "+proj=aea +lat_1=29.500000000 +lat_2=45.500000000 +lat_0=37.500000000 +lon_0=-96.000000000 +x_0=0.000 +y_0=0.000 +ellps=GRS80 +datum=NAD83 +units=m";
//			//# World_Robinson <54030>
//			//string destProj = "+proj=robin +lon_0=0.000000000 +x_0=0.000 +y_0=0.000 +ellps=WGS84 +datum=WGS84 +units=m";
//			//# GCS_WGS_1984 <4326> 
//			string destProj = "+proj=longlat +ellps=WGS84 +datum=WGS84";
//			
//
//			
//			//TIFF destination projection
//			//string destProj = "+proj=utm +zone=12 +ellps=GRS80 +datum=NAD83 +units=m";
//
//			//			string inputGrid = @"C:\Temp\testGrids\clipped.asc";
//			//			string resultGrid = @"C:\Temp\testGrids\WGSgrid.asc";
//			//Aquaterra Grid
//			//string inputGrid = @"C:\Documents and Settings\Angela\Desktop\probGrid\01070001.bgd";	//in longlat
//			//string resultGrid = @"C:\Documents and Settings\Angela\Desktop\probGrid\resultGrid.bgd";
//
//			//string inputGrid = @"C:\Documents and Settings\Angela\Desktop\grid.bgd";
//			//Tiff grids
//			string inputGrid = @"C:\Documents and Settings\Angela\Desktop\grid.tif";		//in utm zone 12N
//			//string inputGrid = @"C:\Documents and Settings\Angela\Desktop\42907573.tif";	//in longlat
//			//string inputGrid = @"C:\Documents and Settings\Angela\Desktop\temp.tif";		//in longlat		
//			//string inputGrid = @"C:\Documents and Settings\Angela\Desktop\sample.tif";	//in longlat
//			string resultGrid = @"C:\Documents and Settings\Angela\Desktop\resultGrid.tif";
//			
//			//use the "save to file" version:
//			status = MapWinGeoProc.SpatialReference.ProjectGrid(ref sourceProj, ref destProj, ref inputGrid, ref resultGrid, true);
//			//use to test "in memory" version:
//			//			MapWinGIS.Grid iGrid = new MapWinGIS.GridClass();
//			//			iGrid.Open(inputGrid, MapWinGIS.GridDataType.UnknownDataType, true, MapWinGIS.GridFileType.UseExtension, null);
//			//			MapWinGIS.Grid rGrid = new MapWinGIS.GridClass();
//			//			status = MapWinGeoProc.SpatialReference.ProjectGrid(ref sourceProj, ref destProj, ref iGrid, out rGrid, true);
//			//			rGrid.Save(resultGrid, MapWinGIS.GridFileType.UseExtension, null);
//			//			Debug.WriteLine("Status = " + status);
//
//			MapWinGIS.Grid iGrid = new MapWinGIS.GridClass();
//			iGrid.Open(inputGrid, MapWinGIS.GridDataType.UnknownDataType, true, MapWinGIS.GridFileType.UseExtension, null);
//			double dX, dY, Xll, Yll;
//			int numCols, numRows;
//			dX = iGrid.Header.dX;
//			dY = iGrid.Header.dY;
//			Xll = iGrid.Header.XllCenter;
//			Yll = iGrid.Header.YllCenter;
//			numCols = iGrid.Header.NumberCols;
//			numRows = iGrid.Header.NumberRows;
//			Debug.WriteLine("Original Grid info:");
//			Debug.WriteLine("(XllCenter, YllCenter): (" + Xll + ", " + Yll + ")");
//			Debug.WriteLine("Cell size = " + dX + "(dX) " + dY + "(dY)");
//			Debug.WriteLine("NumCols = " + numCols + " NumRows = " + numRows);
//			iGrid.Close();
			string errorMsg = MapWinGeoProc.Error.GetLastErrorMsg();
			Debug.WriteLine("Done. " + errorMsg);
		}

		private void btnReProjectGrid_Click(object sender, System.EventArgs e)
		{
			bool status;
			//# USA_Contiguous_Albers_Equal_Area_Conic <102003>
			//string sourceProj = "+proj=aea +lat_1=29.500000000 +lat_2=45.500000000 +lat_0=37.500000000 +lon_0=-96.000000000 +x_0=0.000 +y_0=0.000 +ellps=GRS80 +datum=NAD83 +units=m";
			//# World_Robinson <54030>
			//string sourceProj = "+proj=robin +lon_0=0.000000000 +x_0=0.000 +y_0=0.000 +ellps=WGS84 +datum=WGS84 +units=m";
			//# GCS_WGS_1984 <4326> 
			//string sourceProj = "+proj=longlat +ellps=WGS84 +datum=WGS84";
			//# NAD_1983_UTM_Zone_12N <26912>
			//Aquaterra source
			string sourceProj = "+proj=aea +ellps=clrk66 +lon_0=-96 +lat_0=23.0 +lat_1=29.5 +lat_2=45.5 +x_0=0 +y_0=0 +datum=NAD83 +units=m";
			//Aquaterra destination
			//string destProj = "+proj=longlat +ellps=sphere +lon_0=0 +lat_0=0 +h=0 +datum=NAD83";
			//USA_Contiguous_Equidistant_Conic
			string destProj = "+proj=eqdc +lat_0=0.000000000 +lon_0=0.000000000 +lat_1=33.000000000 +lat_2=45.000000000 +x_0=0.000 +y_0=0.000 +ellps=GRS80 +datum=NAD83 +units=m";
			//Aquaterra grids
			string inputGrid = @"C:\Documents and Settings\Angela\Desktop\probGrid\resultGrid.bgd";
			string resultGrid = @"C:\Documents and Settings\Angela\Desktop\probGrid\ReProjected.bgd";
			//string destProj = "+proj=utm +zone=12 +ellps=GRS80 +datum=NAD83 +units=m";
			//			string inputGrid = @"C:\Temp\testGrids\WGSgrid.asc";
			//			string resultGrid = @"C:\Temp\testGrids\ReWGSgrid.asc";
			status = MapWinGeoProc.SpatialReference.ProjectGrid(ref sourceProj, ref destProj, ref inputGrid, ref resultGrid, true);
			Debug.WriteLine("Status = " + status);
			//get grid info for output
			MapWinGIS.Grid iGrid = new MapWinGIS.GridClass();
			iGrid.Open(inputGrid, MapWinGIS.GridDataType.UnknownDataType, true, MapWinGIS.GridFileType.UseExtension, null);
			double dX, dY, Xll, Yll;
			int numCols, numRows;
			dX = iGrid.Header.dX;
			dY = iGrid.Header.dY;
			Xll = iGrid.Header.XllCenter;
			Yll = iGrid.Header.YllCenter;
			numCols = iGrid.Header.NumberCols;
			numRows = iGrid.Header.NumberRows;
			Debug.WriteLine("Projected Grid info:");
			Debug.WriteLine("(XllCenter, YllCenter): (" + Xll + ", " + Yll + ")");
			Debug.WriteLine("Cell size = " + dX + "(dX) " + dY + "(dY)");
			Debug.WriteLine("NumCols = " + numCols + " NumRows = " + numRows);
			iGrid.Close();
			
			iGrid.Open(resultGrid, MapWinGIS.GridDataType.UnknownDataType, true, MapWinGIS.GridFileType.UseExtension, null);
			dX = iGrid.Header.dX;
			dY = iGrid.Header.dY;
			Xll = iGrid.Header.XllCenter;
			Yll = iGrid.Header.YllCenter;
			numCols = iGrid.Header.NumberCols;
			numRows = iGrid.Header.NumberRows;
			Debug.WriteLine("ReProjected Grid info:");
			Debug.WriteLine("(XllCenter, YllCenter): (" + Xll + ", " + Yll + ")");
			Debug.WriteLine("Cell size = " + dX + "(dX) " + dY + "(dY)");
			Debug.WriteLine("NumCols = " + numCols + " NumRows = " + numRows);
			iGrid.Close();

			Debug.WriteLine("Done.");
		}
		#endregion

		#region ClipShapesWithPolygon Test
		private void btnClipShapesWithPolygon_Click(object sender, System.EventArgs e)
		{
			bool status = true;
			string streamPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\loriv2.shp";
			string watershedPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\lo.shp";
			string tempResultPath = @"C:\Temp\tempResult.shp";
			string resultPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\resultSF.shp";
			
			MapWinGIS.Shapefile streamSF = new MapWinGIS.ShapefileClass();
			streamSF.Open(streamPath, null);
			int numStreams = streamSF.NumShapes;
			MapWinGIS.Shapefile watershedSF = new MapWinGIS.ShapefileClass();
			watershedSF.Open(watershedPath, null);
			int numWatersheds = watershedSF.NumShapes;
			Debug.WriteLine("number of streams = " + numStreams);
			Debug.WriteLine("number of watersheds = " + numWatersheds);

			MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();

			//delete any result shapeFile of the same name then re-create it.
			if(System.IO.File.Exists(resultPath))
			{	
				int length = resultPath.IndexOf(".", 0);
				string shapeFileName = resultPath.Substring(0, length);
				DeleteShapeFile(shapeFileName);
			}
			//create the result shapeFile
			if(resultSF.CreateNew(resultPath, MapWinGIS.ShpfileType.SHP_POLYLINE)==false)
			{
				string errorMsg = "Problem creating the result shapeFile: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
				Debug.WriteLine(errorMsg);
				status = false;
			}
			
			int shpPosition = 0;
			int numTempResults = 0;

			for(int i = 0; i <= numWatersheds-1; i++)
			{
				MapWinGIS.Shapefile tempResultSF = new MapWinGIS.ShapefileClass();
				//delete any result shapeFile of the same name then re-create it.
				if(System.IO.File.Exists(tempResultPath))
				{	
					int length = resultPath.IndexOf(".", 0);
					string shapeFileName = tempResultPath.Substring(0, length);
					DeleteShapeFile(shapeFileName);
				}
				//create the result shapeFile
				if(tempResultSF.CreateNew(tempResultPath, MapWinGIS.ShpfileType.SHP_POLYLINE)==false)
				{
					string errorMsg = "Problem creating the result shapefile: " + tempResultSF.get_ErrorMsg(tempResultSF.LastErrorCode);
					Debug.WriteLine(errorMsg);
					status = false;
				}
				MapWinGIS.Shape watershed = new MapWinGIS.ShapeClass();
				watershed = watershedSF.get_Shape(i);
				int numPoints = watershed.numPoints;
				Debug.WriteLine("Watershed " + i + " has " + numPoints + " points.");

				System.DateTime time1, time2;
				System.TimeSpan elapsed;
				time1 = DateTime.Now;
				status = MapWinGeoProc.SpatialOperations.ClipShapesWithPolygon(ref streamSF, ref watershed, out tempResultSF, true);
				//status = MapWinGeoProc.SpatialOperations.ClipShapesWithPolygon(ref streamSF, ref watershed, out tempResultSF, false);
				time2 = DateTime.Now;
				elapsed = time2-time1;
				Debug.WriteLine("Elapsed time: " + elapsed.Minutes + ":" + elapsed.Seconds);
		
				if(status == true)
				{
					numTempResults = tempResultSF.NumShapes;
					int numStreamPts = 0;
					for(int j = 0; j <= numTempResults-1; j++)
					{
						MapWinGIS.Shape tempShp = new MapWinGIS.ShapeClass();
						tempShp = tempResultSF.get_Shape(j);
						numStreamPts += tempShp.numPoints;
						shpPosition = resultSF.NumShapes;
						if(resultSF.EditInsertShape(tempShp, ref shpPosition)==false)
						{
							Debug.WriteLine("Problem inserting shape into result file.");
							status = false;
							break;
						}
					}
					Debug.WriteLine(numStreamPts);
				}
			}

			resultSF.StopEditingShapes(true, true, null);
			streamSF.Close();
			watershedSF.Close();
			resultSF.Close();

			Debug.WriteLine("Done, function returned " + status);	
		}
		#endregion

		#region Test Multi-Part Polygons
		private void btnMultiPart_Click(object sender, System.EventArgs e)
		{
			MapWinGIS.Utils utils = new MapWinGIS.UtilsClass();
			double area = 0;
			MapWinGIS.Shape shape = new MapWinGIS.ShapeClass();
			MapWinGIS.Shape aShp = new MapWinGIS.ShapeClass();
			MapWinGIS.Point[] point = new MapWinGIS.Point[5];
			MapWinGIS.Point[] point2 = new MapWinGIS.Point[5];
			int partIndex = 0;
			int pointIndex = 0;
			bool success;
			
			//Create two arrays of point objects
			for(int i = 0; i <= 4; i++)
			{
				point[i] = new MapWinGIS.PointClass();
				point2[i] = new MapWinGIS.PointClass();
			}
			//Create a new polygon shape object
			success = shape.Create(ShpfileType.SHP_POLYGON);
			success = aShp.Create(ShpfileType.SHP_POLYGON);

			//Set the x and y coordinates for the first part's points
			//Note: These points are arranged in a clockwise order.
			//As a result, these points specify the part of the shape
			//that will be filled.
			point[0].x = 100;
			point[0].y = 200;
			point[1].x = 200;
			point[1].y = 200;
			point[2].x = 200;
			point[2].y = 100;
			point[3].x = 100;
			point[3].y = 100;
			point[4].x = 100;
			point[4].y = 200;

			//Insert the first part into the shape with points starting at point index 0
			success = shape.InsertPart(0, ref partIndex);
			//Increment the part index
			partIndex++;
			//Insert each point in the point array into the first part
			int aPtIndex = 0;
			for(int i = 0; i <= 4; i++)
			{
				success = shape.InsertPoint(point[i], ref pointIndex);
				pointIndex++;

				success = aShp.InsertPoint(point[i], ref aPtIndex);
				aPtIndex++;
			}

			//area = utils.get_Area(aShp);
			area = MapWinGeoProc.Statistics.Area(ref aShp);
			Debug.WriteLine("aShp is made of " + aShp.NumParts + " parts.");
			Debug.WriteLine("Outside shape has " + aShp.numPoints + " points.");
			Debug.WriteLine("Area of outer, clockwise shape: " + area);
			
			aShp = new MapWinGIS.ShapeClass();
			aShp.ShapeType = ShpfileType.SHP_POLYGON;
			//Set the x and y coordinates for the second part's points
			//Note: These points are arranged in a counter-clockwise order.
			//As a result, these points specify the part to 
			//be cut out of the shape.
			point2[0].x = 120;
			point2[0].y = 120;
			point2[1].x = 150;
			point2[1].y = 120;
			point2[2].x = 150;
			point2[2].y = 150;
			point2[3].x = 120;
			point2[3].y = 150;
			point2[4].x = 120;
			point2[4].y = 120;

			//Insert the second part using the points from the next point index and on
			success = shape.InsertPart(pointIndex, ref partIndex);
			//Increment the part index
			partIndex++;
			//Insert each point in the point2 array into the shape in the second part
			aPtIndex = 0;
			for(int i = 0; i <= 4; i++)
			{
				success = shape.InsertPoint(point2[i], ref pointIndex);
				pointIndex++;

				aShp.InsertPoint(point2[i], ref aPtIndex);
				aPtIndex++;
			}
			//get area of second shape (the hole)
			//area = utils.get_Area(aShp);
			area = MapWinGeoProc.Statistics.Area(ref aShp);
			Debug.WriteLine("Number of inside points: " + aShp.numPoints);
			Debug.WriteLine("Area of inside part (hole): " + area);
			
			//get area of multi-part shape (second part is a hole)
			//area = utils.get_Area(shape);
			area = MapWinGeoProc.Statistics.Area(ref shape);

			Debug.WriteLine("Shape1 has " + shape.NumParts + " parts.");
			Debug.WriteLine("Part1 is from point " + shape.get_Part(0) + " to point " + shape.get_Part(1));
			Debug.WriteLine("Part2 is from point " + shape.get_Part(1) + " to point " + shape.numPoints);
			Debug.WriteLine("Shape1 area = " + area);
			Debug.WriteLine("Shape1 has " + shape.numPoints + " points.");

			//create a multi-part shape, add to a shapefile, then project it into a new coordinate system
			int numPoints;
			pointIndex = 0;
			partIndex = 0;
			string countySF = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\counties.shp";
			string inputPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\multiPart";
			string inputSF = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\multiPart.shp";
			string resultSF = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\PROJmultiPart.shp";

			MapWinGIS.Shapefile sf = new MapWinGIS.ShapefileClass();
			if(File.Exists(inputSF))
			{
				DeleteShapeFile(inputPath);
			}
			sf.CreateNew(inputSF, ShpfileType.SHP_POLYGON);
			
			MapWinGIS.Shape mpShape = new MapWinGIS.ShapeClass();
			mpShape.ShapeType = ShpfileType.SHP_POLYGON;
			mpShape.InsertPart(pointIndex, ref partIndex);
			partIndex++;

			MapWinGIS.Shapefile cSF = new MapWinGIS.ShapefileClass();
			cSF.Open(countySF, null);
			//add first "part"
			MapWinGIS.Shape county = new MapWinGIS.ShapeClass();
			county = cSF.get_Shape(0);
			numPoints = county.numPoints;
			for(int i=0; i<= numPoints-1; i++)
			{
				mpShape.InsertPoint(county.get_Point(i), ref pointIndex);
				pointIndex++;
			}
			//add second "part"
			mpShape.InsertPart(pointIndex, ref partIndex);
			county = cSF.get_Shape(3);
			numPoints = county.numPoints;
			for(int i = 0; i<= numPoints-1; i++)
			{
				mpShape.InsertPoint(county.get_Point(i), ref pointIndex);
				pointIndex++;
			}
			//add the two-part county to result file
			int shpIndex = 0;
			sf.EditInsertShape(mpShape, ref shpIndex);
			shpIndex++;

			//add the other two shapes just for fun
			sf.EditInsertShape(cSF.get_Shape(1), ref shpIndex);
			shpIndex++;
			sf.EditInsertShape(cSF.get_Shape(2), ref shpIndex);
			
			sf.StopEditingShapes(true, true, null);
			
			//now project the created SF into a new coordinate system:
			string sourceProj = "+proj=utm +zone=12 +ellps=GRS80 +datum=NAD83 +units=m";
			//# USA_Contiguous_Albers_Equal_Area_Conic <102003>
			string destProj = "+proj=aea +lat_1=29.500000000 +lat_2=45.500000000 +lat_0=37.500000000 +lon_0=-96.000000000 +x_0=0.000 +y_0=0.000 +ellps=GRS80 +datum=NAD83 +units=m";

			//use save-to-file version:
			//sf.Close();
			//bool status = MapWinGeoProc.SpatialReference.ProjectShapefile(ref sourceProj, ref destProj, ref inputSF, ref resultSF);

			//use over-write existing file version:
			//bool status = MapWinGeoProc.SpatialReference.ProjectShapefile(ref sourceProj, ref destProj, ref sf);
			//sf.Close();

			//use in-memory version:
			MapWinGIS.Shapefile result = new MapWinGIS.ShapefileClass();
			result.CreateNew(resultSF, ShpfileType.SHP_POLYGON);
			bool status = MapWinGeoProc.SpatialReference.ProjectShapefile(ref sourceProj, ref destProj, ref sf, out result);
			result.StopEditingShapes(true, true, null);
			sf.Close();

			Debug.WriteLine("Done. Status = " + status);			
		}
		#endregion

		#region getArea test
		private void btnGetArea_Click(object sender, System.EventArgs e)
		{
			MapWinGIS.Utils utils = new MapWinGIS.UtilsClass();
			int ptIndex = 0;
			MapWinGIS.Shape clockPoly = new MapWinGIS.ShapeClass();
			clockPoly.ShapeType = MapWinGIS.ShpfileType.SHP_POLYGON;
			MapWinGIS.Point point = new MapWinGIS.PointClass();
			point.x = 10;
			point.y = 10;
			clockPoly.InsertPoint(point, ref ptIndex);
			ptIndex++;
		
			point = new MapWinGIS.PointClass();
			point.x = 20;
			point.y = 10;
			clockPoly.InsertPoint(point, ref ptIndex);
			ptIndex++;

			point = new MapWinGIS.PointClass();
			point.x = 30;
			point.y = 10;
			clockPoly.InsertPoint(point, ref ptIndex);
			ptIndex++;

			point = new MapWinGIS.PointClass();
			point.x = 30;
			point.y = 20;
			clockPoly.InsertPoint(point, ref ptIndex);
			ptIndex++;

			point = new MapWinGIS.PointClass();
			point.x = 20;
			point.y = 20;
			clockPoly.InsertPoint(point, ref ptIndex);
			ptIndex++;

			point = new MapWinGIS.PointClass();
			point.x = 20;
			point.y = 30;
			clockPoly.InsertPoint(point, ref ptIndex);
			ptIndex++;

			point = new MapWinGIS.PointClass();
			point.x = 30;
			point.y = 30;
			clockPoly.InsertPoint(point, ref ptIndex);
			ptIndex++;

			point = new MapWinGIS.PointClass();
			point.x = 40;
			point.y = 30;
			clockPoly.InsertPoint(point, ref ptIndex);
			ptIndex++;

			point = new MapWinGIS.PointClass();
			point.x = 40;
			point.y = 0;
			clockPoly.InsertPoint(point, ref ptIndex);
			ptIndex++;

			point = new MapWinGIS.PointClass();
			point.x = 35;
			point.y = 5;
			clockPoly.InsertPoint(point, ref ptIndex);
			ptIndex++;

			point = new MapWinGIS.PointClass();
			point.x = 25;
			point.y = 0;
			clockPoly.InsertPoint(point, ref ptIndex);
			ptIndex++;

			point = new MapWinGIS.PointClass();
			point.x = 10;
			point.y = 10;
			clockPoly.InsertPoint(point, ref ptIndex);
			ptIndex++;

			double area = MapWinGeoProc.Statistics.Area(ref clockPoly);
			Debug.WriteLine(area);

			MapWinGIS.Shape antiPoly = new MapWinGIS.ShapeClass();
			antiPoly.ShapeType = MapWinGIS.ShpfileType.SHP_POLYGON;

			int numPoints = clockPoly.numPoints;
			ptIndex = 0;
			for(int i = numPoints-1; i >= 0; i--)
			{
                antiPoly.InsertPoint(clockPoly.get_Point(i), ref ptIndex);
				ptIndex++;
			}

			area = MapWinGeoProc.Statistics.Area(ref antiPoly);
			Debug.WriteLine("MapWinGeoProc area of anitPoly: " + area);
			area = utils.get_Area(antiPoly);
			Debug.WriteLine("MapWinGIS area: " + area);

			MapWinGIS.Shapefile sf = new MapWinGIS.ShapefileClass();
			//in areaPolygonSF, shape0 is clockwise and contains shape1 which is counterclockwise
			//shape2 is counterclockwise and is inside of shape3.
			sf.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\areaPolygonSF.shp", null);
			int numShapes = sf.NumShapes;
			for(int i = 0; i <= numShapes-1; i++)
			{
				MapWinGIS.Shape shape = new MapWinGIS.ShapeClass();
				shape = sf.get_Shape(i);
				area = MapWinGeoProc.Statistics.Area(ref shape);
				Debug.WriteLine("Area of shape " + i + " = " + area);				
			}

			//create multi-part polygons
			MapWinGIS.Shape multi_1 = new MapWinGIS.ShapeClass();
			multi_1.ShapeType = MapWinGIS.ShpfileType.SHP_POLYGON;
			//create first part
			int partIndex = 0;
			multi_1.InsertPart(0, ref partIndex);
			partIndex++;
            int numPolyPts = sf.get_Shape(0).numPoints;
			ptIndex = 0;
			for(int i= 0; i<= numPolyPts-1; i++)
			{
				multi_1.InsertPoint(sf.get_Shape(0).get_Point(i), ref ptIndex);
				ptIndex++;
			}
			//create second part
			multi_1.InsertPart(ptIndex, ref partIndex);
			partIndex++;
			numPolyPts = sf.get_Shape(1).numPoints;
			for(int i=0; i<= numPolyPts-1; i++)
			{
				multi_1.InsertPoint(sf.get_Shape(1).get_Point(i), ref ptIndex);
				ptIndex++;
			}

			//calculate area
			area = MapWinGeoProc.Statistics.Area(ref multi_1);
			Debug.WriteLine("Area of Shape0 with Shape1 removed: " + area);
			area = utils.get_Area(multi_1);
			Debug.WriteLine("MapWinGIS area: " + area);

			//create a new multi-part polygon
			MapWinGIS.Shape multi_2 = new MapWinGIS.ShapeClass();
			multi_2.ShapeType = MapWinGIS.ShpfileType.SHP_POLYGON;
			//create first part (outer area)
			partIndex = 0;
			multi_2.InsertPart(0, ref partIndex);
			partIndex++;
			numPolyPts = sf.get_Shape(3).numPoints;
			ptIndex = 0;
			for(int i= 0; i<= numPolyPts-1; i++)
			{
				multi_2.InsertPoint(sf.get_Shape(3).get_Point(i), ref ptIndex);
				ptIndex++;
			}
			//create second part (hole)
			multi_2.InsertPart(ptIndex, ref partIndex);
			partIndex++;
			numPolyPts = sf.get_Shape(2).numPoints;
			for(int i=0; i<= numPolyPts-1; i++)
			{
				multi_2.InsertPoint(sf.get_Shape(2).get_Point(i), ref ptIndex);
				ptIndex++;
			}

			//calculate area
			area = MapWinGeoProc.Statistics.Area(ref multi_2);
			Debug.WriteLine("Area of Shape3 with Shape2 removed: " + area);
			area = utils.get_Area(multi_2);
			Debug.WriteLine("MapWinGIS area: " + area);			

			sf.Close();
		}
		#endregion

		#region ClipPolygonsWithLines test
		private void btnClipPolysWithLines_Click(object sender, System.EventArgs e)
		{
			MapWinGIS.Shapefile polySF = new MapWinGIS.ShapefileClass();
			MapWinGIS.Shapefile lineSF = new MapWinGIS.ShapefileClass();
			MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
			string polySFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\ClipperPolygonSF.shp";
			string lineSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\lineSF.shp";
			string resultSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\resultSF";
			if(File.Exists(resultSFPath + ".shp"))
			{
				DeleteShapeFile(resultSFPath);
			}
			polySF.Open(polySFPath, null);
			lineSF.Open(lineSFPath, null);
			MapWinGIS.ShpfileType shpType = polySF.ShapefileType;
			resultSF.CreateNew(resultSFPath + ".shp", shpType);
			int numPolys = polySF.NumShapes;
			int numLines = lineSF.NumShapes;
			MapWinGIS.Shape polygon, line;
			int shpIndex = 0;

			MapWinGIS.Shapefile polygons = new MapWinGIS.ShapefileClass();
			polygons.CreateNew(@"C:\Temp\tempPolyFile.shp", shpType);
			polygon = new MapWinGIS.ShapeClass();
			polygon.Create(shpType);
			polygon = polySF.get_Shape(3);
			polygons.EditInsertShape(polySF.get_Shape(3), ref shpIndex);
		
			MapWinGIS.Shapefile lines = new MapWinGIS.ShapefileClass();
			lines.CreateNew(@"C:\Temp\tempLineFile.shp", lineSF.ShapefileType);
			line = new MapWinGIS.ShapeClass();
			line.Create(lineSF.ShapefileType);
			line = lineSF.get_Shape(4);
			shpIndex = 0;
			lines.EditInsertShape(line, ref shpIndex);
			shpIndex++;
			MapWinGIS.Shape line2 = new MapWinGIS.ShapeClass();
			line2.Create(lineSF.ShapefileType);
			line2 = lineSF.get_Shape(7);
			lines.EditInsertShape(line2, ref shpIndex);
			Debug.WriteLine("line1 xMax:" + lines.get_Shape(0).Extents.xMax);
			Debug.WriteLine("line2 xMax:" + lines.get_Shape(1).Extents.xMax);

			bool success = MapWinGeoProc.SpatialOperations.ClipPolygonSFWithLineSF(ref polygons, ref lines, out resultSF);



//			for(int i = 0; i<= numPolys-1; i++)
//			{
//				polygon = new MapWinGIS.ShapeClass();
//				polygon = polySF.get_Shape(i);
//
//				string trialSFPath = @"C:\temp\trial_SF";
//				if(File.Exists(trialSFPath + ".shp"))
//				{
//					DeleteShapeFile(trialSFPath);
//				}
//				MapWinGIS.Shapefile trialSF = new MapWinGIS.ShapefileClass();
//				trialSF.CreateNew(trialSFPath + ".shp", shpType);
//
//				for(int j = 0; j<= numLines-1; j++)
//				{
//					string tempSFPath = @"C:\temp\temp_SF";
//					if(File.Exists(tempSFPath + ".shp"))
//					{
//						DeleteShapeFile(tempSFPath);
//					}
//					MapWinGIS.Shapefile tempSF = new MapWinGIS.ShapefileClass();
//					trialSF.CreateNew(tempSFPath + ".shp", shpType);
//					
//					line = new MapWinGIS.ShapeClass();
//					line = lineSF.get_Shape(j);
//					
//					if( i == 3 && (j == 4))
//					{
//						MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(ref polygon, ref line, out trialSF, false);
//					}
//					if( i == 3 && (j == 7))
//					{
//						int numTrials = trialSF.NumShapes;
//						for(int k = 0; k <= numTrials-1; k++)
//						{
//							MapWinGIS.Shape poly = new MapWinGIS.ShapeClass();
//							poly = trialSF.get_Shape(k);
//							MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(ref poly, ref line, out tempSF, false);
//
//							int numShapes = tempSF.NumShapes;
//							if(numShapes > 0)
//							{
//								trialSF.EditDeleteShape(i);
//								int shpIndex = trialSF.NumShapes;
//								for(int num = 0; num <= numShapes-1; num++)
//								{
//									trialSF.EditInsertShape(tempSF.get_Shape(num), ref shpIndex);
//									shpIndex++;
//								}
//							}
//						}						
//					}//end of handling second line
//
//				}//end of looping through lines
//				int numResults = trialSF.NumShapes;
//				int index = trialSF.NumShapes;
//				for(int k = 0; k <= numResults; k++)
//				{
//					resultSF.EditInsertShape(trialSF.get_Shape(k), ref index);
//					index++;
//				}
//
//			}//end of looping through polygons
			Debug.WriteLine("Number of result shapes: " + resultSF.NumShapes);
			if(resultSF.NumShapes > 0)
			{
				resultSF.StopEditingShapes(true, true, null);
				resultSF.SaveAs(resultSFPath + ".shp", null);
				resultSF.Close();
			}
			polySF.Close();
			lineSF.Close();	

			Debug.WriteLine("Done. Success = " + success);
		}
		#endregion

		#region SelectWithPolygon test
		private void btnSelectWithPolygon_Click(object sender, System.EventArgs e)
		{
			string pointSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\pointSF.shp";
			string pointResultSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\pointResultSF.shp";
			MapWinGIS.Shapefile polySF = new MapWinGIS.ShapefileClass();
			polySF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\ClipperPolygonSF.shp", null);
			int numClipperShapes = polySF.NumShapes;
			MapWinGIS.Shape polygon = polySF.get_Shape(5);
			bool status;
			status = MapWinGeoProc.SpatialOperations.SelectWithPolygon(ref pointSFPath, ref polygon, ref pointResultSFPath);
			Debug.WriteLine("Done with points. Status = " + status);
			
			string lineSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\lineSF.shp";
			string lineResultSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\lineResultSF.shp";
			status = MapWinGeoProc.SpatialOperations.SelectWithPolygon(ref lineSFPath, ref polygon, ref lineResultSFPath);
			Debug.WriteLine("Done with lines. Status = " + status);
			
			polygon = polySF.get_Shape(1);
			string polySFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\gridClipper.shp";
			string polyResultSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\polyResultSF.shp";
			status = MapWinGeoProc.SpatialOperations.SelectWithPolygon(ref polySFPath, ref polygon, ref polyResultSFPath);
			Debug.WriteLine("Done with polygons. Status = " + status);

			polySF.Close();

		}
		#endregion

		#region Centroid test
		private void btnCentroid_Click(object sender, System.EventArgs e)
		{
			int shpIndex = 0;
			MapWinGIS.Shapefile polySF = new MapWinGIS.ShapefileClass();
			polySF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\ClipperPolygonSF.shp", null);
			MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
			string resultSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\resultSF";
			if(File.Exists(resultSFPath + ".shp"))
			{
				DeleteShapeFile(resultSFPath);
			}
			resultSF.CreateNew(resultSFPath + ".shp", MapWinGIS.ShpfileType.SHP_POINT);
			
			int numPolygons = polySF.NumShapes;
			for(int i = 0; i <= numPolygons-1; i++)
			{
				MapWinGIS.Shape polygon = new MapWinGIS.ShapeClass();
				polygon = polySF.get_Shape(i);
				MapWinGIS.Point centroid = new MapWinGIS.PointClass();
				centroid = MapWinGeoProc.Statistics.Centroid(ref polygon);
				Debug.WriteLine("poly " + i + " centroid: (" + centroid.x + ", " + centroid.y + ")");

				MapWinGIS.Shape ptShape = new MapWinGIS.ShapeClass();
				ptShape.ShapeType = MapWinGIS.ShpfileType.SHP_POINT;
				int ptIndex = 0;
				ptShape.InsertPoint(centroid, ref ptIndex);

				shpIndex = resultSF.NumShapes;
				resultSF.EditInsertShape(ptShape, ref shpIndex);			
			}

			resultSF.StopEditingShapes(true, true, null);
            resultSF.Close();
			polySF.Close();
			Debug.WriteLine("Done computing centroids.");		
		}
		#endregion

		#region ShapeMerge test
		private void btnShapeMerge_Click(object sender, System.EventArgs e)
		{
			//test merging lines
			MapWinGIS.ShpfileType sfType = MapWinGIS.ShpfileType.SHP_POLYLINE;
			MapWinGIS.Shapefile sf = new MapWinGIS.ShapefileClass();
			sf.CreateNew(@"C:\temp\tempSF.shp", sfType);
			MapWinGIS.Shape line1 = new MapWinGIS.ShapeClass();
			line1.Create(sfType);
			int ptIndex = 0;
			MapWinGIS.Point pt = new MapWinGIS.PointClass();
			pt.x = 10.0;
			pt.y = 10.0;
			line1.InsertPoint(pt, ref ptIndex);
			ptIndex++;
			pt = new MapWinGIS.PointClass();
			pt.x = 12.0;
			pt.y = 10.0;
			line1.InsertPoint(pt, ref ptIndex);
			ptIndex++;
			pt = new MapWinGIS.PointClass();
			pt.x = 14.0;
			pt.y = 10.0;
			line1.InsertPoint(pt, ref ptIndex);
			int shpIndex = 0;
			sf.EditInsertShape(line1, ref shpIndex);
			//create line2, such that firstPt of line2 == firstPt of line1
			MapWinGIS.Shape line2 = new MapWinGIS.ShapeClass();
			line2.Create(sfType);
			pt = new MapWinGIS.PointClass();
			pt.x = 10.0;
			pt.y = 10.0;
			ptIndex = 0;
			line2.InsertPoint(pt, ref ptIndex);
			ptIndex++;
			pt = new MapWinGIS.PointClass();
			pt.x = 12.0;
			pt.y = 15.0;
			line2.InsertPoint(pt, ref ptIndex);
			shpIndex = 1;
			sf.EditInsertShape(line2, ref shpIndex);
			//create line3, such that the first pt == last pt of line2
			MapWinGIS.Shape line3 = new MapWinGIS.ShapeClass();
			line3.Create(sfType);
			pt = new MapWinGIS.PointClass();
			pt.x = 12.0;
			pt.y = 15.0;
			ptIndex = 0;
			line3.InsertPoint(pt, ref ptIndex);
			ptIndex++;
			pt = new MapWinGIS.PointClass();
			pt.x = 16.0;
			pt.y = 15.0;
			line3.InsertPoint(pt, ref ptIndex);
			shpIndex = 2;
			sf.EditInsertShape(line3, ref shpIndex);
			//create line4, such that lastPt == firstPt of line3
			MapWinGIS.Shape line4 = new MapWinGIS.ShapeClass();
			line4.Create(sfType);
			pt = new MapWinGIS.PointClass();
			pt.x = 20.0;
			pt.y = 18.0;
			ptIndex = 0;
			line4.InsertPoint(pt, ref ptIndex);
			ptIndex++;
			pt = new MapWinGIS.PointClass();
			pt.x = 12.0;
			pt.y = 15.0;
			line4.InsertPoint(pt, ref ptIndex);
			shpIndex = 3;
			sf.EditInsertShape(line4, ref shpIndex);
			//create line5, such that lastPt == lastPt of line4
			MapWinGIS.Shape line5 = new MapWinGIS.ShapeClass();
			line5.Create(sfType);
			pt = new MapWinGIS.PointClass();
			pt.x = 17.0;
			pt.y = 15.0;
			ptIndex = 0;
			line5.InsertPoint(pt, ref ptIndex);
			ptIndex++;
			pt = new MapWinGIS.PointClass();
			pt.x = 12.0;
			pt.y = 15.0;
			line5.InsertPoint(pt, ref ptIndex);
			shpIndex = 4;
			sf.EditInsertShape(line5, ref shpIndex);
			//now test all of the combinations
			for(int j = 0; j <= sf.NumShapes-2; j++)
			{
				MapWinGIS.Shape resultShp = new MapWinGIS.ShapeClass();
				resultShp.Create(sfType);
				Debug.WriteLine("Merging lines " + j + " and " + (j+1));
				bool status = MapWinGeoProc.SpatialOperations.MergeShapes(ref sf, j, j+1, out resultShp);
				Debug.WriteLine("Status = " + status);
				for(int i = 0; i<= resultShp.numPoints-1; i++)
				{
					Debug.WriteLine("(" + resultShp.get_Point(i).x + ", " + resultShp.get_Point(i).y + ")");
				}
			}
			Debug.WriteLine("Done with lines.");
			//test merging polygons
			string polySFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\counties.shp";
			MapWinGIS.Shapefile polySF = new MapWinGIS.ShapefileClass();
			polySF.Open(polySFPath, null);
			MapWinGIS.Shape returnShp = new MapWinGIS.ShapeClass();
			returnShp.Create(polySF.ShapefileType);
			Debug.WriteLine("Merging polygons.");
			bool returnVal = MapWinGeoProc.SpatialOperations.MergeShapes(ref polySF, 0, 1, out returnShp);
			//axMap1.AddLayer(returnShp, true);
			string resultSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\resultSF";
			if(File.Exists(resultSFPath + ".shp"))
			{
               DeleteShapeFile(resultSFPath);
			}
			MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
			resultSF.CreateNew(resultSFPath + ".shp", polySF.ShapefileType);
			shpIndex = 0;
			if(returnShp.numPoints > 0)
			{
				resultSF.EditInsertShape(returnShp, ref shpIndex);
				shpIndex++;
			}
			//now merg shapes 2 & 3
			returnShp = new MapWinGIS.ShapeClass();
			returnVal = MapWinGeoProc.SpatialOperations.MergeShapes(ref polySF, 2, 3, out returnShp);
			if(returnShp.numPoints > 0)
			{
				resultSF.EditInsertShape(returnShp, ref shpIndex);
			}
			resultSF.StopEditingShapes(true, true, null);
			resultSF.Close();
			polySF.Close();
			Debug.WriteLine("Done with polygons.");
			Debug.WriteLine("Starting to merge lines in loriv2.shp");
			string lineSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\loriv2.shp";
			MapWinGIS.Shapefile lineSF = new MapWinGIS.ShapefileClass();
			lineSF.Open(lineSFPath, null);
			MapWinGIS.Shapefile resultLineSF = new MapWinGIS.ShapefileClass();
			string resultLineSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\resultLineSF";
			if(File.Exists(resultLineSFPath + ".shp"))
			{
				DeleteShapeFile(resultLineSFPath);
			}
			resultLineSF.CreateNew(resultLineSFPath + ".shp", lineSF.ShapefileType);
			for(int i = 125; i <= 130; i+=2)
			{
				MapWinGIS.Shape shp1 = new MapWinGIS.ShapeClass();
				shp1 = lineSF.get_Shape(i);
				MapWinGIS.Shape shp2 = new MapWinGIS.ShapeClass();
				shp2 = lineSF.get_Shape(i+1);
				MapWinGIS.Shape result = new MapWinGIS.ShapeClass();
				returnVal = MapWinGeoProc.SpatialOperations.MergeShapes(ref shp1, ref shp2, out result);
				if(result.numPoints > 0)
				{
					shpIndex = resultLineSF.NumShapes;
                    resultLineSF.EditInsertShape(result, ref shpIndex);
				}				
			}
			resultLineSF.StopEditingShapes(true, true, null);
			resultLineSF.Close();
			lineSF.Close();
			Debug.WriteLine("Done merging lines from loriv2.shp");
		}
		#endregion

		#region ErasePolySF() test
		private void ErasePolySF_Click(object sender, System.EventArgs e)
		{
			string polySFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\counties.shp";
			string resultSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\resultSF.shp";
			MapWinGIS.Shapefile erasePolySF = new MapWinGIS.ShapefileClass();
			erasePolySF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\ClipperPolygonSF.shp", null);
			erasePolySF.Open(@"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\gridClipper.shp", null);
			int numEraseShapes = erasePolySF.NumShapes;
			MapWinGIS.Shape shp = erasePolySF.get_Shape(2);
			bool status;

			status = MapWinGeoProc.SpatialOperations.Erase(ref polySFPath, ref shp, ref resultSFPath);
			
			erasePolySF.Close();
			Debug.WriteLine("Done, function returned " + status);
		}
		#endregion

		#region BufferPoints() test
		private void btnBufferPoints_Click(object sender, System.EventArgs e)
		{
			bool status = false;
			string pointSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\pointSF.shp";
			string resultSFPath = @"C:\Dev\MapWinGeoProc\GeometryTester\ShapeFiles\resultSF";
			MapWinGIS.Shapefile pointSF = new MapWinGIS.ShapefileClass();
			pointSF.Open(pointSFPath, null);

			if(File.Exists(resultSFPath + ".shp"))
			{
				File.Delete(resultSFPath + ".shp");
				File.Delete(resultSFPath + ".dbf");
				File.Delete(resultSFPath + ".shx");
			}
			MapWinGIS.Shapefile resultSF = new MapWinGIS.ShapefileClass();
			resultSF.CreateNew(resultSFPath + ".shp", MapWinGIS.ShpfileType.SHP_POLYGON);

			int numPoints = pointSF.NumShapes;
			int shpIndex = 0;
			for(int i = 0; i <= numPoints-1; i+=3)
			{
				//buffer every third point and add result shape to resultSF
				MapWinGIS.Shape currPt = new MapWinGIS.ShapeClass();
				currPt = pointSF.get_Shape(i);
				MapWinGIS.Shape resultShp = new MapWinGIS.ShapeClass();
				status = MapWinGeoProc.SpatialOperations.BufferShape(ref currPt, 1000, out resultShp);
				if(status == true)
				{
					if(resultShp.numPoints > 0)
					{
						shpIndex = resultSF.NumShapes;
						resultSF.EditInsertShape(resultShp, ref shpIndex);
					}
				}
				else
				{
					break;
				}				
			}

			resultSF.StopEditingShapes(true, true, null);
			resultSF.Close();
			pointSF.Close();
			Debug.WriteLine("Done, function returned " + status);		
		}
		#endregion


	}//end of class Form1
}//end of namespace GeometryTester
