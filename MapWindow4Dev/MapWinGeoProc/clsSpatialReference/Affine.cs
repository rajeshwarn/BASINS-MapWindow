using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Transforms
{
    /// <summary>
    /// A class supporting the affine transform.
    /// </summary>
    class Affine
    {
        #region Class Level Variables
        bool IsDefined;
        // X' = a * X + b * Y + c
        // Y' = d * X + e * Y + f
        float a, b, c, d, e, f; // affine transform coefficients
        float Xul, Yul; // Output Image pixel coordinates for the Upper Left point
        float Xll, Yll; // Output Image pixel coordinates for the Lower Left point
        float Xur, Yur; // Output Image pixel coordinates for the Upper Right point
        float Xlr, Ylr; // Output Image pixel coordinates for the Lower Right point
        // Output image specifications.  These will be needed for the world file construction.
        int m_OutputHeight;
        int m_OutputWidth;
        float m_OutputCellWidth;
        float m_OutputCellHeight;
        float m_XllCenter;
        float m_YllCenter;

        // Input image width and height in pixel coordinates.
        int m_InputHeight;
        int m_InputWidth;

        
        bool m_CellSizeSpecified;

        #endregion

        //  ---------------------------- Specify_CellSize ------------------------------------------
        /// <summary>
        /// If this function is called the output image will be sized so that the pixels represent these dimensions.
        /// If this function is not specified, a cellwidth will be chosen to be as small as possible while retaining data.
        /// This is done by ensuring that no edge will contain fewer pixels than the edge in the source image.
        /// </summary>
        /// <param name="CellHeight">CellHeight is the height in the projected coordinates of each pixel in the output image</param>
        /// <param name="CellWidth">CellWidth is the width in the projected coordinates of each pixel in the output image</param>
        /// <remarks>Call this function first if you are going to call functions</remarks>
        public void Specify_CellSize(float CellHeight, float CellWidth)
        {
            m_OutputCellWidth = CellWidth;
            m_OutputCellHeight = CellHeight;
            m_CellSizeSpecified = true;
        }

        //  ---------------------------- Derive_Coefficients ---------------------------------------
        /// <summary>
        /// Calculates coefficients that will be stored in the instantiated object for further transforms.
        /// </summary>
        /// <param name="Width">The pixel width of the original rectangular image</param>
        /// <param name="Height">The pixel height of the original rectangular image</param>
        /// <param name="UpperLeft">The projected MapWinGIS.Point of the upper left corner of the image</param>
        /// <param name="UpperRight">The projected MapWinGIS.Point of the upper right corner of the image</param>
        /// <param name="LowerLeft">The projected MapWinGIS.Point of the lower left corner of the image</param>
        /// <param name="LowerRight">The projected MapWinGIS.Point fo the lower right corner of the image</param>
        public void Derive_Coefficients(int Width, int Height, MapWinGIS.Point UpperLeft, MapWinGIS.Point UpperRight, MapWinGIS.Point LowerLeft, MapWinGIS.Point LowerRight)
        {
            // X' = a*X + b*Y + C
            // Y' = d*X + e*Y + f
            m_InputHeight = Height;
            m_InputWidth = Width;
            Size_Coordinates(Width, Height, LowerLeft, UpperLeft, UpperRight, LowerRight);

            // Use our Top Left as 0, 0 and determine from there
            c = Xul;
            f = Yul;
            // Upper Right, X = Width-1, Y = 0
            a = (Xur - c) / (m_InputWidth - 1);
            d = (Yur - f) / (m_InputWidth - 1);
            // Lower Left, X = 0, Y = Height-1
            b = (Xll - c) / (m_InputHeight - 1);
            e = (Yll - f) / (m_InputHeight - 1);

            IsDefined = true;
        }
       
        //  ---------------------------- Error -----------------------------------------------------
        /// <summary>
        /// If you have all four corners, and you want to ensure that the affine transform that you have specified
        /// describes the result image adequately, use this function to estimate the erroroneous distance in pixels
        /// (Any value less than about 2 is generally good enough.)
        /// </summary>
        /// <returns>Double.  The pythagorean difference between the predicted and actual location
        /// for the lower right point in pixel coordinates.</returns>
        public double Error()
        {
            if (IsDefined == false) throw new ApplicationException("First call Derive_Coefficients, then check the error with this function.");
            
           
            double testX, testY, dist, dx, dy;
            // transform the coordinates corresponding to the lower right
            testX = a * m_InputWidth + b * m_InputHeight + c;
            testY = d * m_InputWidth + e * m_InputHeight + f;
            // convert textX, textY from pixel to projected coordinates
            dx = testX - Xlr;
            dy = testY - Ylr;
            dist = Math.Sqrt(dx * dx + dy * dy);
            return dist;
        }

        public void TransformImage(string InputBitmap, string OutputFile, bool WriteWorldFile)
        {
            // the coefficents we created can be represented as a matrix like this:

            //       | a     d     0 |   
            //       |               |       
            //       | b     e     0 |    
            //       |               |      
            //       | c     f     1 | 

            // the same matrix is represented here using the terms that the system drawing matrix uses

            //      |   m11      m12    0  |
            //      |                      |
            //      |   m21      m22    0  |
            //      |                      |
            //      |   dx       dy     1  |

            System.Drawing.Image Source = new System.Drawing.Bitmap(InputBitmap);
            System.Drawing.Image Dest = new System.Drawing.Bitmap(m_OutputWidth, m_OutputHeight);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(Dest);
            g.Transform = new System.Drawing.Drawing2D.Matrix(a, d, b, e, c, f);
            System.Drawing.Imaging.ImageFormat outf;
            string WorldFileExt = ".bpw";
            string DotNetFilename = OutputFile;
            bool TryMapWinGIS = false;
            // If we can get around using the GDAL Image object, we will do so here
            switch(System.IO.Path.GetExtension(OutputFile))
            {
                case ".bmp":
                    outf = System.Drawing.Imaging.ImageFormat.Bmp;
                    WorldFileExt = ".bpw";
                    break;
                case ".gif":
                    outf = System.Drawing.Imaging.ImageFormat.Gif;
                    WorldFileExt = ".gfw";
                    break;
                case ".jpg":
                    outf = System.Drawing.Imaging.ImageFormat.Jpeg;
                    WorldFileExt = ".jgw";
                    break;
                case ".png":
                    outf = System.Drawing.Imaging.ImageFormat.Png;
                    WorldFileExt = ".pgw";
                    break;
                case ".tif":
                    outf = System.Drawing.Imaging.ImageFormat.Tiff;
                    WorldFileExt = ".tfw";
                    break;
                default:
                    //we shouldn't get here 

                    throw new ApplicationException("File format not currently supported by Affine");         
            }
            Dest.Save(OutputFile, outf);
            if (TryMapWinGIS == true)
            {
                // This won't work 
                //MapWinGIS.Image ConvImage = new MapWinGIS.Image();
                //ConvImage.Open(DotNetFilename, MapWinGIS.ImageType.BITMAP_FILE, true, null);
                //ConvImage.Save(OutputFile, true, MapWinGIS.ImageType.USE_FILE_EXTENSION, null);
            }
            else
            {
                if (WriteWorldFile == true)
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(System.IO.Path.ChangeExtension(OutputFile, WorldFileExt));
                    sw.WriteLine(m_OutputCellWidth);
                    sw.WriteLine(0.ToString());
                    sw.WriteLine(0.ToString());
                    sw.WriteLine(m_OutputCellHeight);
                    sw.WriteLine(m_XllCenter);
                    sw.WriteLine(m_YllCenter);
                    sw.Close();
                }
            }
        }

        //  ---------------------------- Autosize_Cells --------------------------------------------
        /// <summary>
        /// This function will calculate a "scale" factor that should be the output pixel size.  If none is specified,
        /// this function is called internally.  This should prevent data loss, but may have more data than needed.
        /// </summary>
        /// <param name="Width">The width of the source image </param>
        /// <param name="Height">The height of the source image</param>
        /// <param name="outLL">The projected location of the lower left point</param>
        /// <param name="outUL">The projected location of the upper left point</param>
        /// <param name="outUR">The projected location of the upper right point</param>
        /// <returns>A float value representing the cell-height and cell-width.</returns>
        public static double Autosize_Cells(float Width, float Height, MapWinGIS.Point outLL, MapWinGIS.Point outUL, MapWinGIS.Point outUR)
        {

            double scale;
            double dx, dy, dist;

            // ---------- preserve information by sizing against the side that would lose information first
            // LEFT
            dx = outUL.x - outLL.x;
            dy = outUL.y - outLL.y;
            dist = Math.Sqrt(dx * dx + dy * dy);
            scale = dist / Height;

            // TOP
            dx = outUR.x - outUL.x;
            dy = outUR.y - outUL.y;
            dist = Math.Sqrt(dx * dx + dy * dy);
            if ((dist / Width) < scale) scale = (dist / Width);

            // For the affine transform, the right and bottom distances will be the same as the top and left
            return scale;
        }

        // Takes the projected coordinates and figures out the pixel coordinates, width and height given a specific cell sizing
        private void Size_Coordinates(int Width, int Height, MapWinGIS.Point outLL, MapWinGIS.Point outUL, MapWinGIS.Point outUR, MapWinGIS.Point outLR)
        {
            float Xmin = (float)outLL.x; // We assume that X increases from left to right
            float Ymin = (float)outLL.y; // To work with 2D image, Y is 0 at the top and increases as you move down
            float Xmax = (float)outLL.x; // however we are aware that in projected coords, ymin will be below ymax
            float Ymax = (float)outLL.y;
            if (outUL.x < Xmin) Xmin = (float)outUL.x;
            if (outUL.x > Xmax) Xmax = (float)outUL.x;
            if (outUL.y < Ymin) Ymin = (float)outUL.y;
            if (outUL.y > Ymax) Ymax = (float)outUL.y;
            if (outUR.x < Xmin) Xmin = (float)outUR.x;
            if (outUR.x > Xmax) Xmax = (float)outUR.x;
            if (outUR.y < Ymin) Ymin = (float)outUR.y;
            if (outUR.y > Ymax) Ymax = (float)outUR.y;
            if (outLR.x < Xmin) Xmin = (float)outLR.x;
            if (outLR.x > Xmax) Xmax = (float)outLR.x;
            if (outLR.y < Ymin) Ymin = (float)outLR.y;
            if (outLR.y > Ymax) Ymax = (float)outLR.y;
            m_YllCenter = Ymin;
            m_XllCenter = Xmin;
            if (m_CellSizeSpecified == false)
            {
                float scale = (float)Autosize_Cells((float)Width, (float)Height, outLL, outUL, outUR);
                m_OutputCellHeight = scale;
                m_OutputCellWidth = scale;
            }
            m_OutputHeight = (int)((Ymax - Ymin) / m_OutputCellHeight);
            m_OutputWidth = (int)((Xmax - Xmin) / m_OutputCellWidth);
            Xul = (float)(outUL.x - Xmin) / m_OutputCellWidth;
            Yul = (float)(Ymax - outUL.y) / m_OutputCellHeight;
            Xur = (float)(outUR.x - Xmin) / m_OutputCellWidth;
            Yur = (float)(Ymax - outUR.y) / m_OutputCellHeight;
            Xll = (float)(outLL.x - Xmin) / m_OutputCellWidth;
            Yll = (float)(Ymax - outLL.y) / m_OutputCellHeight;
            Xlr = (float)(outLR.x - Xmin) / m_OutputCellWidth;
            Ylr = (float)(Ymax - outLR.y) / m_OutputCellHeight;
           
           // X4 = (outLR.x - Xmin) / m_OutputCellWidth;
           // Y4 = (outLR.y - Ymin) / m_OutputCellHeight;
        }
       
    }
}
