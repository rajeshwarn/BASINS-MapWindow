//********************************************************************************************************
//File name: Rasterizer.cs
//Description: Internal class, provides methods for converting polyline and polygon shapefiles to grid
//             used by MapWinGeoProc.Utils.ShapefileToGrid() function
//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specific language governing rights and 
//limitations under the License. 
//
//The Original Code for rasterizing polygons (Scanline algorithm) was written by Enrico Chiaraidia.
//Jiri Kadlec implemented the polyline rasterization (Bresenham algorithm, partially base on the MapWinGIS
//ActiveX c++ open source code (LineBresenham.cpp) written by Rob Cairns
//
//For more information about the scanline algorithm, go to
//http://www.cs.rit.edu/~icss571/filling/ ('Polygon fill teaching tool', department of computer sciences,
//Rochester Institute of Technology)
//For Bresenham algorithm, see the Wikipedia (http://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm) 
//
// Authors: Enrico Chiraidia (provided the MapWindow script for rasterizing polygons in vb.net)
//          Jiri Kadlec (rewritten the code in c#, implemented Bresenham algorithm for lines
//
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
//12/21/2007 Jiri Kadlec provided the initial implementation of polygon and polyline rasterization
//algorithms.						
//********************************************************************************************************

using System;
using System.Collections;
using System.Text;

using MapWinGIS;
using Microsoft.VisualBasic;
using MapWindow.Interfaces;


namespace MapWinGeoProc.Rasterization
{
    internal class Rasterizer
    {
#region Poly2Grid
        /// <summary>
        /// This function converts a polygon shapefile to grid. It implements the line-scan algorithm.
        /// <param name="PolySf">The polygon shapefile</param>
        /// <param name="FldID">The field index</param>
        /// <param name="Newgrd">File name of the new grid</param>
        /// <param name="header">Header object of the new grid</param>
        /// <param name="grType">Grid data type</param>
        /// <param name="cback">can be used to report progress (optional)</param>
        /// <returns>true if successful</returns>
        /// </summary>
        // 
        // AUTHOR NAME: Enrico A. Chiaradia
        //       FROM: University of Milan (Italy)
        //     e.mail: enrico.chiaradia@unimi.it
        //
        // For details about the line-scan algorithm,
        // please refer to http://www.profc.udec.cl/~gabriel/tutoriales/giswb/vol1/cp3/cp3-6.htm
        //
        // for each shape in PolySf
        // 0) get the shape value (in the shapefile table)
        // 1) load all the borders in a matrix composed by x(startpoint), x(endpoint), slope and share
        // 2) define x of first scan line
        // for each scan lines
        // 3) calculate intersections with polygon border
        // 4) sort intersections from lower Y to the higther 
        // 5) convert line space between two consecutive intersection to raster
        
        public bool Poly2Grid(MapWinGIS.Shapefile PolySf, int FldID, MapWinGIS.Grid Newgrd,
            MapWinGIS.GridHeader header, MapWinGIS.GridDataType grType, 
            MapWinGIS.ICallback cback)
        {
            
            double xMin, xMax;
            double x0, x1, x2, y0, y1, y2, m, q;
            int nShps, nVs, s, nP, p, tempn, v, b;
            object val;
            bool flg;
            bord brd; //struct to keep a border
            System.Collections.ArrayList bordi2; //list of borders
            int perc = 1; //percent shapes done
            int shpsPerc;
            
            MapWinGIS.Shape PolyShp; //the polygon shape

            //count the number of shapes
            nShps = PolySf.NumShapes;
            shpsPerc = NumPercentShapes(perc, nShps); //percent of progress

            for( s = 0; s < nShps; ++s )
            {
                //for each shape, determine the extent and the number of parts

                PolyShp = PolySf.get_Shape(s);
                nP = PolyShp.NumParts;

                xMin = PolyShp.Extents.xMin;
                //yMin = PolyShp.Extents.yMin;
                xMax = PolyShp.Extents.xMax;
                //yMax = PolyShp.Extents.yMax;

                //exclude shapes which are completely outside of the grid extents
                if (!IsGridContainsShape(PolyShp, header))
                {
                    continue;
                }

                //return the shape's value
                val = GetCellValue(PolySf, FldID, s, header.NodataValue);

                //load polygon borders
                nVs = PolyShp.numPoints - 1;
                bordi2 = new System.Collections.ArrayList(nVs + 1);

                b = -1;
                tempn = 0;

                for( p = 0; p < nP; ++p )
                {
                    x0 = PolyShp.get_Point(tempn).x;
                    y0 = PolyShp.get_Point(tempn).y;

                    for( v = tempn; v < nVs; ++v )
                    {
                        x1 = PolyShp.get_Point(v).x;
                        x2 = PolyShp.get_Point(v + 1).x;
                        y1 = PolyShp.get_Point(v).y;
                        y2 = PolyShp.get_Point(v + 1).y;

                        if (x1 != x2) // no vertical lines
                        {
                            m = (y2 - y1) / (x2 - x1);
                            q = y1 - (m * x1);

                            //add border to the list
                            b = b + 1;
                            brd = new bord();
                            brd.x1 = x1;
                            brd.x2 = x2;
                            brd.m = m;
                            brd.q = q;
                            bordi2.Add(brd);     
                        }

                        //in case of multi-parts polygon
                        if( (x2 == x0) && (y2 == y0) )
                        {
                            tempn = v + 2;
                            break;
                        }
                    } //Next v
                } // next p
     
                //define the first line-scan
                double xstart;

                //    20 nov 07 added by Enrico A. Chiaradia
                //necessary if grid extentions differ from shapefile extentions
                if ( xMin >= header.XllCenter )
                {
                    xstart = FirstLineXY(xMin, header.XllCenter, header.dX, 1);
                }
                else
                {
                    xstart = FirstLineXY(xMin, header.XllCenter, header.dX, -1);
                }

                // the list of intersection y-values
                System.Collections.ArrayList y_int = new System.Collections.ArrayList();
                System.Collections.ArrayList pixels = new System.Collections.ArrayList();
                
                do
                {
                    flg = Interseca(bordi2, y_int, xstart);
                    flg = SortArray(y_int);  
                    flg = ScanLine(y_int, xstart, pixels, header, val, cback);
                    xstart = xstart + header.dX;
                }
                while ( xstart <= xMax );

                writePxList(Newgrd, grType, pixels, val, cback);

                //report the progress
                if (s >= shpsPerc)
                {
                    reportProgress(shpsPerc, nShps, "shapefile to grid", cback);
                    perc = (int)( (s * 100) / nShps );
                    shpsPerc = NumPercentShapes(perc + 1, nShps);
                }

            } // next shape

            reportProgress(100, 100, "shapefile to grid", cback); //100 % shapes done
            return true;
        }

        /// <summary>
        /// 'return the closest line referring to the grid extention
        /// </summary>
        private double FirstLineXY(double xy, double xymin, double pxsize, int fact)
        {
            double ncol, resto;
            double result = xy;
            
            ncol = Fix((xymin - xy) / pxsize);
            resto = (xymin - xy) % pxsize;

            if (resto > 0)
            {
                ncol = ncol + 1;
            }
            result = xymin + (fact * ncol * pxsize);
            return result;
        }

        private double Fix(double number)
        {
            return Math.Sign(number) * Math.Floor(Math.Abs(number));
        }

        /// <summary>
        /// determine if the shape is partially inside grid extents
        /// </summary>
        /// <param name="shp"></param>
        /// <param name="hdr"></param>
        /// <returns>false, if the shape is completely outside grid extents
        ///          true, if it's at least partially inside</returns>
        private bool IsGridContainsShape(MapWinGIS.Shape shp, MapWinGIS.GridHeader hdr)
        {
            double gridXmin = hdr.XllCenter;
            double gridYmin = hdr.YllCenter;
            double gridXmax = gridXmin + (hdr.NumberCols * hdr.dX);
            double gridYmax = gridYmin + (hdr.NumberRows * hdr.dY);

            MapWinGIS.Extents ext = shp.Extents;
            if (ext.xMin > gridXmax || ext.yMin > gridYmax ||
                ext.xMax < gridXmin || ext.yMax < gridYmin)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Creates a list of all intersections between the vertical line-scan and polygon's borders
        /// </summary>
        /// <param name="bordi2">The list of polygon borders</param>
        /// <param name="intersezioni">The list of intersections</param>
        /// <param name="xls">The line-scan x-value</param>
        private bool Interseca(ArrayList bordi2, ArrayList intersezioni, double xls)
        {
            bool result = false;
            int l, nlati, c;
            double x1, x2, m, q;

            nlati = bordi2.Count;
            intersezioni.Clear();
            c = -1;

            for ( l = 0; l < nlati; ++l)
            {
                x1 = ( (bord) bordi2[l] ).x1; 
                x2 = ( (bord) bordi2[l] ).x2;

                if ( ( ( xls >= x1 ) && ( xls < x2 ) ) || ( ( xls >= x2 ) && ( xls < x1 ) ) )
                {
                    c = c + 1;
                    m = ( (bord) bordi2[l] ).m;
                    q = ( (bord) bordi2[l] ).q;
                    //calculate Y intersection
                    intersezioni.Add( m * xls + q );
                }
            }
            result = true;
            return result;
        }

        private bool SortArray(ArrayList vettore)
        {
            bool result = false;
            vettore.Sort();
            result = true;
            return result;
        }

        /// <summary>
        /// populate all the grid cells between two consecutive intersections with shape's value
        /// </summary>
        private bool ScanLine(ArrayList vettore, double xref,
            ArrayList pixels, MapWinGIS.GridHeader header, 
            object value, MapWinGIS.ICallback cback)
        {
            bool result = false;
            bool control;
            double xll, yll, ystart, yend, ycellstart;
            double dx, dy;
            int nrows;
            int c, r, v;
            r = 0;
            GridPixel curPx;

            control = false;
            xll = header.XllCenter;
            yll = header.YllCenter;
            dx = header.dX;
            dy = header.dY;
            nrows = header.NumberRows;

            ystart = 0;
            for (v = vettore.Count - 1; v >= 0; --v)
            {
                if (control == false)
                {
                    ystart = (double)vettore[v];
                    control = true;
                }
                else
                {
                    yend = (double)vettore[v];
                    ycellstart = FirstLineXY(ystart, yll, dy, -1);

                    do
                    {
                        Globals.ProjToCell(xref, ycellstart, out c, out r, xll, yll, dx, dy, nrows);
                        curPx.col = c;
                        curPx.row = r;
                        pixels.Add(curPx);
                        ycellstart = ycellstart - dy;
                    }
                    while (ycellstart >= yend);
                    control = false;
                }
            }
            return result;
        }

#endregion

#region Line2Grid

/// <summary>
    /// converts a Line shapefile to grid using Bresenham algorithm
    /// </summary>
    /// <param name="LineSf">Polyline shapefile object</param>
    /// <param name="FldID">Field index</param>
    /// <param name="Newgrd">New grid object</param>
    /// <param name="header">Grid header</param>
    /// <param name="grType">Grid data type</param>
    /// <param name="cback">optional, for reporting progress</param>
    /// <returns>true if successful</returns>
    
    public bool Line2Grid(MapWinGIS.Shapefile LineSf, int FldID, MapWinGIS.Grid Newgrd,
    MapWinGIS.GridHeader header, MapWinGIS.GridDataType grType, MapWinGIS.ICallback cback)
    {
        int numParts, numPoints;
        int curPartStart, curPartEnd, vertexCol, vertexRow, lastCol, lastRow;
        int nShps = LineSf.NumShapes;
        int perc = 1;
        int shpsPerc = NumPercentShapes(perc, nShps); //percent of progress
        int s = 0; //shape number
        int p = 0; //part number
        int k = 0; //point number
        MapWinGIS.Shape LineShp = new MapWinGIS.Shape();
        MapWinGIS.Point curPoint;
        
        ArrayList vertices = new ArrayList(); //list of line vertex points
        ArrayList pixels = new ArrayList();   //list of calculated pixels on the line
        GridPixel px;      
        object val; //the shape's value
        
        for (s = 0; s < nShps; ++s)
        {
            LineShp = LineSf.get_Shape(s);
            numParts = LineShp.NumParts;
            numPoints = LineShp.numPoints;
            
            //exclude shapes which are completely outside of the grid extents
            if (!IsGridContainsShape(LineShp, header))
            {
                continue;
            }
                     
            //get the shape's value
            val = GetCellValue(LineSf, FldID, s, header.NodataValue);

            //process each part of the polyline
            curPartStart = 0;
            for (p = 0; p < numParts; ++p)
            {
                vertices.Clear();
                pixels.Clear();

                curPartStart = LineShp.get_Part(p);

                // check for multi-part lines
                if (p < numParts - 1)
                {
                    curPartEnd = LineShp.get_Part(p + 1) - 1;
                }
                else
                {
                    curPartEnd = numPoints - 1;
                }

                //go to next part if there's zero points
                if (numPoints <= 0)
                {
                    continue;
                }
                
                // add all points of current part to rasterization list
                // always add the first point of the part (convert its coordinates to
                // grid row and column)
                curPoint = LineShp.get_Point(curPartStart);
                Newgrd.ProjToCell(curPoint.x, curPoint.y,
                        out vertexCol, out vertexRow);
                px.col = vertexCol;
                px.row = vertexRow;
                vertices.Add(px);
                lastCol = vertexCol; lastRow = vertexRow;

                // add all other points with different grid coordinates
                for (k = curPartStart + 1; k <= curPartEnd; ++k)
                {
                    // (check if it has a different row or column than the previous point)
                    curPoint = LineShp.get_Point(k);
                    Newgrd.ProjToCell(curPoint.x, curPoint.y,
                        out vertexCol, out vertexRow);
                    if (vertexCol != lastCol || vertexRow != lastRow)
                    {
                        px.col = vertexCol;
                        px.row = vertexRow;
                        vertices.Add(px);
                        lastCol = vertexCol;
                        lastRow = vertexRow;
                    }
                }

                // convert the polyline and write pixels to grid 
                RasterizePolyline(vertices, pixels);
                writePxList(Newgrd, grType, pixels, val, cback);               
            }

            //report the progress
            if (s >= shpsPerc)
            {
                reportProgress(shpsPerc, nShps, "shapefile to grid", cback);
                perc = (int)((s * 100) / nShps);
                shpsPerc = NumPercentShapes(perc + 1, nShps);
            }
        }
        reportProgress(100, 100, "shapefile to grid", cback);
        return true;
    }

    /// <summary>
    /// implements the Bresenham's line algorithm
    /// </summary>
    /// <param name="vertices">ArrayList of points (shape2grid.pixel struct) in grid coordinates</param>
    /// <param name="gridPixels">ArrayList of pixels on the rasterized line</param>
    private void RasterizePolyline(ArrayList vertices, ArrayList gridPixels)
    {
        //declaration of local variables
        GridPixel px, pxOne, pxTwo;
        int index = 0;
        int num_vertices = vertices.Count;
        int num_edges = num_vertices - 1;
        LineEdge curEdge;
        if (num_vertices < 2)
        {
            //in this case, handle the line as a single point
            gridPixels.Add((GridPixel)vertices[0]);
            return;
        }

        pxOne = (GridPixel)vertices[0];
        pxTwo = (GridPixel)vertices[1];
        curEdge = new LineEdge(pxOne.col, pxOne.row, pxTwo.col, pxTwo.row);
        // add the first pixel
        px.col = curEdge.CurX;
        px.row = curEdge.CurY;
        gridPixels.Add(px);

        // loop through all line edges
        while (index < num_edges)
        {
            // initialize edge
            pxOne = (GridPixel) vertices[index];
            pxTwo = (GridPixel)vertices[index + 1];
            curEdge.Reset(pxOne.col, pxOne.row, pxTwo.col, pxTwo.row);

            // loop through all pixels on the edge
            while (curEdge.NextPixel())
            {
                // set the value of current pixel
                px.col = curEdge.CurX;
                px.row = curEdge.CurY;
                gridPixels.Add(px);
            }
            ++index;
        }
    }
#endregion

#region Point2Grid

    /// <summary>
    /// Converts a point shapefile to grid
    /// </summary>
    /// <param name="PointSf">Point shapefile object</param>
    /// <param name="FldID">Index of field that contains data</param>
    /// <param name="Newgrd">The new grid object</param>
    /// <param name="header">The grid header object</param>
    /// <param name="grType">The grid data type</param>
    /// <param name="cback">(optional) callback object for reporting progress</param>
    /// <returns>False when error encountered, true otherwise</returns>
    public bool Point2Grid(MapWinGIS.Shapefile PointSf, int FldID, MapWinGIS.Grid Newgrd,
    MapWinGIS.GridHeader header, MapWinGIS.GridDataType grType, MapWinGIS.ICallback cback)
    {
        //count the number of shapes
        int s;
        MapWinGIS.Shape shp;
        MapWinGIS.Point pt = new MapWinGIS.Point();
        object val;
        GridPixel px;
        int nShps = PointSf.NumShapes;
        int perc = 1;
        int shpsPerc = NumPercentShapes(perc, nShps); //percent of progress

        for (s = 0; s < nShps; ++s)
        {
            //get the point
            shp = PointSf.get_Shape(s);

            //exclude shapes which are completely outside of the grid extents
            if (!IsGridContainsShape(shp, header))
            {
                continue;
            }

            if (shp.numPoints > 0)
            {
                pt = shp.get_Point(0);
                if (pt != null)
                {
                    //return the shape's value and write the pixel
                    val = GetCellValue(PointSf, FldID, s, header.NodataValue);
                    Newgrd.ProjToCell(pt.x, pt.y, out px.col, out px.row);
                    writePx(Newgrd, grType, px, val, cback);
                }
            }

            //report the progress
            if (s >= shpsPerc)
            {
                perc = (int)((s * 100) / nShps);
                shpsPerc = NumPercentShapes(perc + 1, nShps);
                reportProgress(shpsPerc, nShps, "shapefile to grid", cback);
            }
        }
        return true;
    }
#endregion

        /// <summary>
        /// Converts a multipoint shapefile to grid
        /// </summary>
        /// <param name="MultipointSf">Multipoint shapefile object</param>
        /// <param name="FldID">Index of field that contains data</param>
        /// <param name="Newgrd">The new grid object</param>
        /// <param name="header">Grid header object</param>
        /// <param name="grType">Data type of the new grid</param>
        /// <param name="cback">optional object for reporting progress</param>
        /// <returns>False in case of error, true otherwise</returns>
#region Multipoint2grid
    public bool Multipoint2Grid(MapWinGIS.Shapefile MultipointSf, int FldID, MapWinGIS.Grid Newgrd,
    MapWinGIS.GridHeader header, MapWinGIS.GridDataType grType, MapWinGIS.ICallback cback)
    {
        //count the number of shapes
        int s, p;
        MapWinGIS.Shape shp;
        MapWinGIS.Point pt;
        ArrayList pixels = new ArrayList();
        object val;
        GridPixel px;
        int nShps = MultipointSf.NumShapes;
        int nPts = 0;
        int perc = 1;
        int shpsPerc = NumPercentShapes(perc, nShps); //percent of progress

        for (s = 0; s < nShps; ++s)
        {
            //get the shape
            shp = MultipointSf.get_Shape(s);

            //exclude shapes which are completely outside of the grid extents
            if (!IsGridContainsShape(shp, header))
            {
                continue;
            }

            //get the shape's value
            val = GetCellValue(MultipointSf, FldID, s, header.NodataValue);
            
            nPts = shp.numPoints;
            for (p = 0; p < nPts; ++p)
            {
                //write pixel values
                pt = shp.get_Point(p);
                Newgrd.ProjToCell(pt.x, pt.y, out px.col, out px.row);
                pixels.Add(px);
                writePxList(Newgrd, grType, pixels, val, cback);
            }

            //report the progress
            if (s >= shpsPerc)
            {
                reportProgress(shpsPerc, nShps, "shapefile to grid", cback);
                perc = (int)((s * 100) / nShps);
                shpsPerc = NumPercentShapes(perc + 1, nShps);
            }
        }
        return true;
    }
#endregion

#region Write Pixel
    /// <summary>
    /// writes a single grid pixel
    /// </summary>
    private void writePx(MapWinGIS.Grid gr, MapWinGIS.GridDataType grType,
        GridPixel pix, object val, MapWinGIS.ICallback cback)
    {
        switch (grType)
        {
            case GridDataType.ShortDataType:
                short v1 = Convert.ToInt16(val);
                gr.set_Value(pix.col, pix.row, v1);
                break;
            case GridDataType.LongDataType:
                int v2 = Convert.ToInt32(val);   
                gr.set_Value(pix.col, pix.row, v2);
                break;
            case GridDataType.FloatDataType:
                float v3 = Convert.ToSingle(val);
                gr.set_Value(pix.col, pix.row, v3);
                break;
            case GridDataType.DoubleDataType:
                double v4 = Convert.ToDouble(val);
                gr.set_Value(pix.col, pix.row, v4);
                break;
            default:
                reportError("the grid data type " + grType.ToString() + "is not supported.", cback);
                break;
        }
    }
        
    /// <summary>
    /// writes the pixel values from arrayList to grid
    /// </summary>
    private void writePxList(MapWinGIS.Grid gr, MapWinGIS.GridDataType grType, 
        System.Collections.ArrayList pxList, object val, MapWinGIS.ICallback cback)
    {
        switch (grType)
        {
            case GridDataType.ShortDataType:
                short v1 = Convert.ToInt16(val);
                foreach (GridPixel pix in pxList)
                {
                    gr.set_Value(pix.col, pix.row, v1);
                }
                break;
            case GridDataType.LongDataType:
                int v2 = Convert.ToInt32(val);
                foreach (GridPixel pix in pxList)
                {
                    gr.set_Value(pix.col, pix.row, v2);
                }
                break;
            case GridDataType.FloatDataType:
                float v3 = Convert.ToSingle(val);
                foreach (GridPixel pix in pxList)
                {
                    gr.set_Value(pix.col, pix.row, v3);
                }
                break;
            case GridDataType.DoubleDataType:
                double v4 = Convert.ToDouble(val);
                foreach (GridPixel pix in pxList)
                {
                    gr.set_Value(pix.col, pix.row, v4);
                }
                break;
            default:
                reportError("the grid data type " + grType.ToString() + "is not supported.", cback);
                break;
        }

        pxList.Clear();
    }

#endregion

#region Helper functions
    /// <summary>
    /// returns the shape's attribute field value
    /// </summary>
    private object GetCellValue(MapWinGIS.Shapefile sf, int fieldIndex, int shapeIndex, object noDt)
    {
        object value;
        try
        {
            value = sf.get_CellValue(fieldIndex, shapeIndex);
        }
        catch
        {
            value = noDt; //if no data is returned
        }
        return value;
    }

#endregion

#region Progress and Error reporting

    /// <summary>
    /// reports the progress
    /// </summary>
    private void reportProgress(int itemsDone, int numItems,
        string keyOfSender, MapWinGIS.ICallback cback)
    {
        if (cback != null)
        {
            int pct = (int)Math.Round(((double)itemsDone / (double)numItems) * 100);
            cback.Progress(keyOfSender, pct, "converting shapes");
        }
    }

    /// <summary>
    /// reports an error
    /// </summary>
    public void reportError(string errorMsg, MapWinGIS.ICallback cback)
    {
        if (cback != null)
        {
            cback.Error("shapefile to grid: ", errorMsg);
        }
    }

        private int NumPercentShapes(int percent, int nShps)
        {
            return (int) ( ( nShps * percent ) / 100 ) + 1;
        }

#endregion

        #region Internal structures
        //structure to keep polygon border data
        private struct bord
        {
            public double x1;
            public double x2;
            public double m;
            public double q;
        }

        //structure to keep the pixel data
        private struct GridPixel
        {
            public int col;
            public int row;
        }
        #endregion

    }

    
}