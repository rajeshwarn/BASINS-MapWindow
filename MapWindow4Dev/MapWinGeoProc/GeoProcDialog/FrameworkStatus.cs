using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MapWinGeoProc.Pitfill;

namespace MapWinGeoProc.Dialogs
{
    /// <summary>
    /// This is a user control to show the status of differnet grid portions of a grid being analyzed.
    /// </summary>
    public partial class FrameworkStatus : UserControl
    {
        /// <summary>
        /// A convenient link to the specific framework where an image is being analyzed
        /// </summary>
        public MapWinGeoProc.Pitfill.Framework myFramework;
        /// <summary>
        /// This initializes this user control.
        /// </summary>
        public FrameworkStatus()
        {
            InitializeComponent();
        }

        private void FrameworkStatus_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// After the control has been updated, call this function to allow it to recreate the status colors.
        /// </summary>
        public void Redraw()
        {
            Pitfill.Frame CurFrame;
            if (myFramework == null) return;
            if (myFramework.NumFramesTall == 0 || myFramework.NumFramesWide == 0) return;
            if (myFramework.HasFrames == false) return;
            Graphics g = this.CreateGraphics();
            float W, H;
            W = (float)this.Width / (float)myFramework.NumFramesWide;
            H = (float)this.Height / (float)myFramework.NumFramesTall;
            SolidBrush green = new SolidBrush(Color.Green);
            SolidBrush yellow = new SolidBrush(Color.Yellow);
            SolidBrush white = new SolidBrush(Color.White);
            SolidBrush blue = new SolidBrush(Color.Blue);
            SolidBrush turquoise = new SolidBrush(Color.Turquoise);
            Pen black = new Pen(Color.Black);
            for (int Y = 0; Y < myFramework.NumFramesTall; Y++)
            {
                for (int X = 0; X < myFramework.NumFramesWide; X++)
                {
                    CurFrame = myFramework.get_Frame(X, Y);

                    Rectangle rec = CurFrame.Rectangle;
                    rec.Width = (int)(W * (float)rec.Width/myFramework.FrameWidth);
                    rec.X = (int)(W * (float)CurFrame.X);
                    rec.Height = (int)(H * (float)rec.Height/myFramework.FrameHeight);
                    rec.Y = (int)(H * (float)CurFrame.Y);
                    switch(CurFrame.Status)
                    {
                        case Frame.StatusType.tkNotEvaluated:
                            g.FillRectangle(white, rec);
                            break;
                        case Frame.StatusType.tkEvaluatedNoDependencies:
                            g.FillRectangle(green, rec);
                            break;
                        case Frame.StatusType.tkEvaluatedNeedsRevisiting:
                            g.FillRectangle(yellow, rec);
                            break;
                        case Frame.StatusType.tkBeingEvaluated1:
                            g.FillRectangle(blue, rec);
                            break;
                    }
                    g.DrawRectangle(black, rec);
                }
            }
        }
    }

}
