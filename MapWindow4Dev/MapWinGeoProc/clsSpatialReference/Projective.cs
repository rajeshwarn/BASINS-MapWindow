using System;
using System.Collections.Generic;
using System.Text;

namespace MapWinGeoProc.Transforms
{
    /// <summary>
    /// This class contains the functions necessary to project an image.
    /// If you have a specific desired output cellwidth/height, call Specify_CellSize first
    /// When you call Derive_Coefficients, send the projected coordinates,
    /// and the function will set up an output image.  
    /// The Project_Point_Backward works in pixel coordinates, not in projected coordinates.
    /// 
    /// </summary>
    public class Projective
    {
        #region Class Variables

        double a, b, c, d, e, f, g, h; // Projective Transform coefficients
        double Xll, Yll, Xul, Yul, Xur, Yur, Xlr, Ylr;
        bool Defined = false;
        bool DefinedForVector = false;

        // These are used internally and externally to keep track of the number of pixels 
        // in both the original and projected images.
        int m_OutputHeight;
        int m_OutputWidth;
        int m_InputHeight;
        int m_InputWidth;

        bool m_CellSizeSpecified;
        bool m_AffineWillWork;

        // This information isn't encoded by pixel locations, but rather in a world file later.
        // We make these publicly accessible for that reason.
        double m_OutputCellWidth;
        double m_OutputCellHeight;
        double m_XllCenter;
        double m_YllCenter;         

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the integer height in pixels of the projected image.
        /// </summary>
        public int OutputHeight
        {
            get
            {
                return m_OutputHeight;
            }
        }
        /// <summary>
        /// Gets the integer width in pixels of the projected image
        /// </summary>
        public int OutputWidth
        {
            get
            {
                return m_OutputWidth;
            }
        }
        /// <summary>
        /// Gets the horizontal size of a pixel in the output image in the projected coordinates
        /// </summary>
        public double dX
        {
            get
            {
                return m_OutputCellWidth;
            }
        }
        /// <summary>
        /// Gets the vertical size of a pixel in the output image in the projected coordinates
        /// </summary>
        public double dY
        {
            get
            {
                return m_OutputCellHeight;
            }
        }
        /// <summary>
        /// Gets a double value representing the horizontal location of the lower left pixel of the 
        /// output image in projected coordinates.
        /// </summary>
        public double XllCenter
        {
            get
            {
                return m_XllCenter;
            }
        }
        /// <summary>
        /// Gets a double value representing the vertical location of the lower left pixel of the 
        /// output image in projected coordinates.
        /// </summary>
        public double YLLCenter
        {
            get
            {
                return m_YllCenter;
            }
        }
        /// <summary>
        /// Solving the four points will also check to see if the projection can be solved using
        /// the Affine transform (only using 3 points).  If it can, it is recommended that you
        /// call the Project_Affine function.  Otherwise you will still be able to call the 
        /// Project_Affine function, but it probably won't be a good fit.
        /// </summary>
        public bool AffineWillWork
        {
            get
            {
                return m_AffineWillWork;
            }
        }
        #endregion

        #region Public Methods
        //  ---------------------------- Specify_CellSize ------------------------------------------
        /// <summary>
        /// If this function is called the output image will be sized so that the pixels represent these dimensions.
        /// If this function is not specified, a cellwidth will be chosen to be as small as possible while retaining data.
        /// This is done by ensuring that no edge will contain fewer pixels than the edge in the source image.
        /// </summary>
        /// <param name="CellHeight">CellHeight is the height in the projected coordinates of each pixel in the output image</param>
        /// <param name="CellWidth">CellWidth is the width in the projected coordinates of each pixel in the output image</param>
        /// <remarks>Call this function first if you are going to call functions</remarks>
        public void Specify_CellSize(double CellHeight, double CellWidth)
        {
            m_OutputCellWidth = CellWidth;
            m_OutputCellHeight = CellHeight;
            m_CellSizeSpecified = true;
        }

        //---------------------------- Derive_Coefficients------------------------------------------
        /// <summary>
        /// Initializes the projective coefficients.  Requires that the input is
        /// a rectangular image, and the output points are the projected output
        /// values corresponding to the input image corners in the following positions: 
        /// pt2 - pt3
        ///  |     |
        /// pt1 - pt4
        /// // X' = (aX + bY + c) / (gX + hY + 1)
        /// // Y' = (dX + eY + f) / (gX + hY + 1)
        /// </summary>
        /// <param name="ImageWidth">The width of the entire original image in pixels</param>
        /// <param name="ImageHeight">The height of the entire original image in pixels</param>
        /// <param name="ptLowerLeft">The projected location of the lower left corner of the input image</param>
        /// <param name="ptUpperLeft">The projected location of the upper left corner of the input image</param>
        /// <param name="ptUpperRight">The projected location of the upper right corner of the input image</param>
        /// <param name="ptLowerRight">The projected location of the lower right corner of the input image</param>
        public void Derive_Coefficients(int ImageWidth, int ImageHeight,
            MapWinGIS.Point ptLowerLeft, MapWinGIS.Point ptUpperLeft, MapWinGIS.Point ptUpperRight, MapWinGIS.Point ptLowerRight)
        {
            double x, y;
           
            m_InputHeight = ImageHeight;
            m_InputWidth = ImageWidth;
            
            Size_Coordinates(ImageWidth, ImageHeight, ptLowerLeft, ptUpperLeft, ptUpperRight, ptLowerRight);
                
            x = (double)ImageWidth;
            y = (double)ImageHeight;

            a = -(-Xul * Yll * Xlr + Xur * Yll * Xlr - Xur * Yul * Xlr + Xul * Yur * Xlr + Xll * Yul * Xur - Xul * Yur * Xll - Xur * Ylr * Xll + Xul * Ylr * Xll) / x / (Yur * Xlr + Yul * Xur - Yul * Xlr - Yur * Xul - Ylr * Xur + Ylr * Xul);
            b = (Xul * Yll * Xur - Xul * Yll * Xlr + Xul * Yur * Xlr - Xur * Ylr * Xul - Xll * Yur * Xlr - Xll * Yul * Xur + Xll * Yul * Xlr + Xur * Ylr * Xll) / y / (Yur * Xlr + Yul * Xur - Yul * Xlr - Yur * Xul - Ylr * Xur + Ylr * Xul);
            c = Xll;
            d = -(-Ylr * Yur * Xll + Ylr * Xll * Yul - Ylr * Yul * Xur + Ylr * Yur * Xul + Yll * Yur * Xlr + Yll * Yul * Xur - Yll * Yul * Xlr - Yll * Yur * Xul) / x / (Yur * Xlr + Yul * Xur - Yul * Xlr - Yur * Xul - Ylr * Xur + Ylr * Xul);
            e = (Ylr * Xll * Yul - Yul * Yur * Xll + Yul * Yur * Xlr - Ylr * Yul * Xur - Yll * Yur * Xlr + Yll * Yur * Xul + Ylr * Yll * Xur - Ylr * Xul * Yll) / y / (Yur * Xlr + Yul * Xur - Yul * Xlr - Yur * Xul - Ylr * Xur + Ylr * Xul);
            f = Yll;
            g = -(Yur * Xlr - Yul * Xlr - Xul * Yll - Yur * Xll - Ylr * Xur + Ylr * Xul + Xll * Yul + Yll * Xur) / (Yur * Xlr + Yul * Xur - Yul * Xlr - Yur * Xul - Ylr * Xur + Ylr * Xul) / x;
            h = (Ylr * Xll + Yll * Xur - Yll * Xlr - Yul * Xur + Yul * Xlr - Yur * Xll - Ylr * Xul + Yur * Xul) / y / (Yur * Xlr + Yul * Xur - Yul * Xlr - Yur * Xul - Ylr * Xur + Ylr * Xul);

            
            // Establish if our affine is good enough
           // evaluate_Affine();

            Defined = true;
            return;
        }

        /// <summary>
        /// This routine is from a vector standpoint and doesn't care about aligning the projected image within a rectangular
        /// image framework. 
        /// </summary>
        /// <param name="OriginalExtents">An Extents object for the original shapefile or shape or vector.</param>
        /// <param name="ptLowerLeft">The projected location of the lower left corner of the original extents</param>
        /// <param name="ptUpperLeft">The projected location of the upper left corner of the original extents</param>
        /// <param name="ptUpperRight">The projected location of the upper right corner of the original extents</param>
        /// <param name="ptLowerRight">The projected location of the lower right corner of the original extents</param>
        public void Derive_Coefficients(MapWinGIS.Extents OriginalExtents,
            MapWinGIS.Point ptLowerLeft, MapWinGIS.Point ptUpperLeft, MapWinGIS.Point ptUpperRight, MapWinGIS.Point ptLowerRight)
        {
            double x, y;
            Size_Coordinates(m_InputWidth, m_InputHeight, ptLowerLeft, ptUpperLeft, ptUpperRight, ptLowerRight);

            x = m_InputWidth;
            y = m_InputHeight;

          
            Xll = ptLowerLeft.x;
            Yll = ptLowerLeft.y;
            Xul = ptUpperLeft.x;
            Yul = ptUpperLeft.y;
            Xur = ptUpperRight.x;
            Yur = ptUpperRight.y;
            Xlr = ptLowerRight.x;
            Ylr = ptLowerRight.y;

            a = -(-Xul * Yll * Xlr + Xur * Yll * Xlr - Xur * Yul * Xlr + Xul * Yur * Xlr + Xll * Yul * Xur - Xul * Yur * Xll - Xur * Ylr * Xll + Xul * Ylr * Xll) / x / (Yur * Xlr + Yul * Xur - Yul * Xlr - Yur * Xul - Ylr * Xur + Ylr * Xul);
            b = (Xul * Yll * Xur - Xul * Yll * Xlr + Xul * Yur * Xlr - Xur * Ylr * Xul - Xll * Yur * Xlr - Xll * Yul * Xur + Xll * Yul * Xlr + Xur * Ylr * Xll) / y / (Yur * Xlr + Yul * Xur - Yul * Xlr - Yur * Xul - Ylr * Xur + Ylr * Xul);
            c = Xll;
            d = -(-Ylr * Yur * Xll + Ylr * Xll * Yul - Ylr * Yul * Xur + Ylr * Yur * Xul + Yll * Yur * Xlr + Yll * Yul * Xur - Yll * Yul * Xlr - Yll * Yur * Xul) / x / (Yur * Xlr + Yul * Xur - Yul * Xlr - Yur * Xul - Ylr * Xur + Ylr * Xul);
            e = (Ylr * Xll * Yul - Yul * Yur * Xll + Yul * Yur * Xlr - Ylr * Yul * Xur - Yll * Yur * Xlr + Yll * Yur * Xul + Ylr * Yll * Xur - Ylr * Xul * Yll) / y / (Yur * Xlr + Yul * Xur - Yul * Xlr - Yur * Xul - Ylr * Xur + Ylr * Xul);
            f = Yll;
            g = -(Yur * Xlr - Yul * Xlr - Xul * Yll - Yur * Xll - Ylr * Xur + Ylr * Xul + Xll * Yul + Yll * Xur) / (Yur * Xlr + Yul * Xur - Yul * Xlr - Yur * Xul - Ylr * Xur + Ylr * Xul) / x;
            h = (Ylr * Xll + Yll * Xur - Yll * Xlr - Yul * Xur + Yul * Xlr - Yur * Xll - Ylr * Xul + Yur * Xul) / y / (Yur * Xlr + Yul * Xur - Yul * Xlr - Yur * Xul - Ylr * Xur + Ylr * Xul);


            // Establish if our affine is good enough
            // evaluate_Affine();

            DefinedForVector = true;
            return;
        }
       

        //---------------------------- Project_Point_Backward (Int) ------------------------------
        /// <summary>
        /// Takes output coordinates and determines what input coordinates to use.
        /// This assumes that you have already solved for the coefficients by 
        /// </summary>
        /// <param name="Xproj">Integer representing the X coordinate of the output image to back-project</param>
        /// <param name="Yproj">Integer representing the Y coordinate of the output image to back-project</param>
        /// <param name="Xoriginal">Integer representing the back-projected location in the input image</param>
        /// <param name="Yoriginal">Integer representing the back-projected location of the input image</param>
        /// <returns>false if the location was outside of the input image dimensions</returns>
        public bool Project_Point_Backward(int Xproj, int Yproj, out int Xoriginal, out int Yoriginal)
        {
            double X, Y;
            double X1, Y1;
            Xoriginal = Yoriginal = 0;
            if (Defined == false)
            {
                throw new ApplicationException("You first have to define the coefficients by calling Derive_Coefficients.");
            }
            
            X1 = (double)Xproj;
            Y1 = (double)Yproj;
            X = -(b * Y1 - Y1 * h * c + e * c - X1 * e + f * X1 * h - f * b) / (X1 * h * d + b * Y1 * g - b * d - X1 * g * e + a * e - a * Y1 * h);
            Y = -(c * Y1 * g + X1 * d - a * Y1 - X1 * g * f - c * d + a * f) / (X1 * h * d + b * Y1 * g - b * d - X1 * g * e + a * e - a * Y1 * h);
           
            double TestX = (a * X + b * Y + c) / (g * X + h * Y + 1);
            double TestY = (d * X + e * Y + f) / (g * X + h * Y + 1);
            if (X < 0 || Y < 0 || X > m_InputWidth || Y > m_InputHeight) return false;
            // using nearest neighbors
            Xoriginal = rnd(X);
            Yoriginal = rnd(Y);
            return true;
        }

        /// <summary>
        /// This is for more of a vector handling of the problem.  Instead of returning an integer,
        /// this will project an actual point.
        ///  X' = (aX + bY + c) / (gX + hY + 1)
        ///  Y' = (dX + eY + f) / (gX + hY + 1)
        /// </summary>
        /// <param name="Original">The starting point</param>
        /// <param name="Projected">The projected point</param>
        /// <returns></returns>
        public bool Project_Point_Foreward(MapWinGIS.Point Original, out MapWinGIS.Point Projected)
        {
            double X, Y;
            double X1, Y1;
            Projected = new MapWinGIS.Point();
            if (Defined == false  && DefinedForVector == false)
            {
                throw new ApplicationException("You must first call Derive_Coefficients"); 
            }
            X = Original.x;
            Y = Original.y;

            X1 = (a * X + b * Y + c) / (g * X + h * Y + 1);
            Y1 = (d * X + e * Y + f) / (g * X + h * Y + 1);
            Projected.x = X1;
            Projected.y = Y1;
            return true;
        }

        /// <summary>
        /// This is for more of a vector handling of the problem.  Instead of returning an integer,
        /// this will back-project an actual point from the projected image to its source in the original
        /// </summary>
        /// <param name="Projected"></param>
        /// <param name="Original"></param>
        /// <returns></returns>
        public bool Project_Point_Backward(MapWinGIS.Point Projected, out MapWinGIS.Point Original)
        {
            double X, Y;
            double X1, Y1;
            Original = new MapWinGIS.Point();
            if (Defined == false && DefinedForVector == false)
            {
                throw new ApplicationException("You must first call Derive_Coefficients");
            }

            X1 = Projected.x;
            Y1 = Projected.y;
            X = -(b * Y1 - Y1 * h * c + e * c - X1 * e + f * X1 * h - f * b) / (X1 * h * d + b * Y1 * g - b * d - X1 * g * e + a * e - a * Y1 * h);
            Y = -(c * Y1 * g + X1 * d - a * Y1 - X1 * g * f - c * d + a * f) / (X1 * h * d + b * Y1 * g - b * d - X1 * g * e + a * e - a * Y1 * h);
            Original.x = X;
            Original.y = Y;
            return true;
        }

        /// <summary>
        /// Reprojects an image using the currently defined projective transform.
        /// Be sure to call Derive_Coefficients first.  This loops through point by point
        /// so won't be very fast.
        /// </summary>
        /// <param name="SourceImage">A MapWinGIS.Image object to be transformed</param>
        /// <param name="resultImage">A string representing the destination filename</param>
        /// <param name="ICallBack">A MapWinGIS.ICallback interface for progress messages</param>
        /// <remarks>ArgumentExceptions should be trapped for user error, but other types should be reported as bugs</remarks>
        public void ProjectImage(MapWinGIS.Image SourceImage, string resultImage, MapWinGIS.ICallback ICallBack)
        {
            bool res;
            if (SourceImage == null) throw new ArgumentException("Source Image cannot be null.");
            if (resultImage == null) resultImage = System.IO.Path.ChangeExtension(SourceImage.Filename, "_Projected" + System.IO.Path.GetExtension(SourceImage.Filename));
            if (Defined == false)
            {
                throw new ApplicationException("You first have to define the coefficients by calling Derive_Coefficients.");
            }
            MapWinGIS.Image DestImage = new MapWinGIS.Image();
            try
            {
                res = DestImage.CreateNew(m_OutputWidth, m_OutputHeight);
            }
            catch
            {
                throw new ApplicationException("The current Image object crashes with images too large to fit in memory.");
            }
            if(res == false)
            {
                throw new ApplicationException("Application Exception when Creating New: " + DestImage.get_ErrorMsg(DestImage.LastErrorCode));
            }
            if (res == false) throw new ApplicationException("Image object is having trouble creating a new image.");
            for (int Yprj = 0; Yprj < m_OutputHeight; Yprj++)
            {
                for (int Xprj = 0; Xprj < m_OutputWidth; Xprj++)
                {
                   
                    double X, Y;
                    double X1, Y1;
                    int Xorig = 0;
                    int Yorig = 0;

                    X1 = (double)Xprj;
                    Y1 = (double)Yprj;
                    X = -(b * Y1 - Y1 * h * c + e * c - X1 * e + f * X1 * h - f * b) / (X1 * h * d + b * Y1 * g - b * d - X1 * g * e + a * e - a * Y1 * h);
                    Y = -(c * Y1 * g + X1 * d - a * Y1 - X1 * g * f - c * d + a * f) / (X1 * h * d + b * Y1 * g - b * d - X1 * g * e + a * e - a * Y1 * h);

                    if (X < 0 || Y < 0 || X > m_InputWidth || Y > m_InputHeight) continue;
                    // using nearest neighbors
                    Xorig = rnd(X);
                    Yorig = rnd(Y);
                    
                    int Rowo = (m_InputHeight - 1) - Yorig;
                    int pVal = SourceImage.get_Value(Rowo, Xorig);

                    int row = (m_OutputHeight - 1) - Yprj;
                    DestImage.set_Value(row, Xprj, pVal);
                    
                }
                if (ICallBack != null) ICallBack.Progress("Status", (Yprj * 100) / m_OutputHeight, "Row: " + Yprj.ToString());
                
            }

            //DestImage.dX = m_OutputCellWidth;
            //DestImage.dY = m_OutputCellHeight;
            //DestImage.XllCenter = m_XllCenter;
            //DestImage.YllCenter = m_YllCenter;

            DestImage.OriginalDX = m_OutputCellWidth;
            DestImage.OriginalDY = m_OutputCellHeight;
            DestImage.OriginalXllCenter = m_XllCenter;
            DestImage.OriginalYllCenter = m_YllCenter;

            string dir = System.IO.Path.GetDirectoryName(resultImage);
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            if (System.IO.Path.GetExtension(resultImage) == ".jpg")
            {
                resultImage = System.IO.Path.ChangeExtension(resultImage, ".bmp");
            }
            
            DestImage.SetProjection(SourceImage.GetProjection());
            res = DestImage.Save(resultImage, true, MapWinGIS.ImageType.USE_FILE_EXTENSION, ICallBack);
            DestImage.Close();

            if (!res)
                throw new ApplicationException(DestImage.get_ErrorMsg(DestImage.LastErrorCode));

            if (ICallBack != null) ICallBack.Progress("Status", 0, "Saved output as " + DestImage.Filename);
        }

        #endregion

        #region Private Functions

        // Takes the projected coordinates and figures out the pixel coordinates, width and height given a specific cell sizing
        private void Size_Coordinates(int Width, int Height, MapWinGIS.Point outLL, MapWinGIS.Point outUL, MapWinGIS.Point outUR, MapWinGIS.Point outLR)
        {
            double Xmin = outLL.x; // We assume that X increases from left to right
            double Ymin = outLL.y; // We assume that Y increases from bottom to top
            double Xmax = outLL.x;
            double Ymax = outLL.y;
            if (outUL.x < Xmin) Xmin = outUL.x;
            if (outUL.x > Xmax) Xmax = outUL.x;
            if (outUL.y < Ymin) Ymin = outUL.y;
            if (outUL.y > Ymax) Ymax = outUL.y;
            if (outUR.x < Xmin) Xmin = outUR.x;
            if (outUR.x > Xmax) Xmax = outUR.x;
            if (outUR.y < Ymin) Ymin = outUR.y;
            if (outUR.y > Ymax) Ymax = outUR.y;
            if (outLR.x < Xmin) Xmin = outLR.x;
            if (outLR.x > Xmax) Xmax = outLR.x;
            if (outLR.y < Ymin) Ymin = outLR.y;
            if (outLR.y > Ymax) Ymax = outLR.y;
            m_YllCenter = Ymin;
            m_XllCenter = Xmin;
            if (m_CellSizeSpecified == false)
            {
               double scale = Autosize_Cells((double)Width, (double)Height, outLL, outUL, outUR, outLR);
               m_OutputCellHeight = scale;
               m_OutputCellWidth = scale;
            }
            m_OutputHeight =(int)((Ymax - Ymin)/ m_OutputCellHeight);
            m_OutputWidth = (int)( (Xmax - Xmin)/m_OutputCellWidth);
            Xll = (outLL.x - Xmin) / m_OutputCellWidth;
            Yll = (outLL.y - Ymin) / m_OutputCellHeight;
            Xul = (outUL.x - Xmin) / m_OutputCellWidth;
            Yul = (outUL.y - Ymin) / m_OutputCellHeight;
            Xur = (outUR.x - Xmin) / m_OutputCellWidth;
            Yur = (outUR.y - Ymin) / m_OutputCellHeight;
            Xlr = (outLR.x - Xmin) / m_OutputCellWidth;
            Ylr = (outLR.y - Ymin) / m_OutputCellHeight;
        }

        /// <summary>
        /// This function will calculate a "scale" factor that should be the output pixel size.  If none is specified,
        /// this function is called internally.  This should prevent data loss, but may have more data than needed.
        /// </summary>
        /// <param name="Width">The width of the source image </param>
        /// <param name="Height">The height of the source image</param>
        /// <param name="outLL">The projected location of the lower left point</param>
        /// <param name="outUL">The projected location of the upper left point</param>
        /// <param name="outUR">The projected location of the upper right point</param>
        /// <param name="outLR">The projected location of the lower right point</param>
        /// <returns>A double value representing the cell-height and cell-width.</returns>
        public static double Autosize_Cells(double Width, double Height, MapWinGIS.Point outLL, MapWinGIS.Point outUL, MapWinGIS.Point outUR, MapWinGIS.Point outLR)
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

            // RIGHT
            dx = outLR.x - outUR.x;
            dy = outLR.y - outUR.y;
            dist = Math.Sqrt(dx * dx + dy * dy);
            if ((dist / Height) < scale) scale = (dist / Height);

            // BOTTOM
            dx = outLL.x - outLR.x;
            dy = outLL.y - outLR.y;
            dist = Math.Sqrt(dx * dx + dy * dy);
            if ((dist / Width) < scale) scale = (dist / Width);
            
            return scale;
        }

        // If plugging 3 points into the affine gets us point 4, use affine instead
        private void evaluate_Affine()
        {
            // X' = a*X + b*Y + C
            // Y' = d*X + e*Y + f
            double a, b, c, d, e, f;
            // For this to work with image coordinates, our upper left becomes 0, 0
            // Upper Left (X = 0, Y = 0)
            c = Xul;
            f = Yul;
            // Upper Right, X = Width-1, Y = 0
            a = (Xur - c) / (m_InputWidth - 1);
            d = (Yur - f) / (m_InputWidth - 1);
            // Lower Left, X = 0, Y = Height-1
            b = (Xll - c) / (m_InputHeight - 1);
            e = (Yll - f) / (m_InputHeight - 1);


            double testX, testY, dist, dx, dy;
            testX = a * m_InputWidth + b * m_InputHeight + c;
            testY = d * m_InputWidth + e * m_InputHeight + f;
            dx = testX - Xlr;
            dy = testY - Ylr;
            dist = Math.Sqrt(dx * dx + dy * dy);
            // setting g and h to 0 simplifies the projective transform to the affine transform
           
            if (dist > 2)
            {
                // the affine projection isn't good enough, so we will have to use the projective transform
                m_AffineWillWork = false;
            }
            else
            {
                // We prefer to use the affine transform because it is a much MUCH faster transform
                m_AffineWillWork = true;
            }
        }
        
        
        // Math.Round has too much junk in my opinion.  This should be faster.
        private int rnd(double val)
        {
            if (val > 0) return (int)(val + .5);
            return (int)(val - .5);
        }


        #endregion


    }
}
