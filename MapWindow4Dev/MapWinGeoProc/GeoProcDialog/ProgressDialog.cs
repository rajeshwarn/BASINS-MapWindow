using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MapWinGeoProc.Dialogs
{
    public partial class ProgressDialog : Form
    {
        DateTime m_StartTime;
        TimeSpan m_ExecutionTime;
        MapWinGIS.ICallback m_ICallBack;
        bool m_Canceled = false;
        bool m_Paused = false;
        int m_Progress;
        /// <summary>
        /// Initializer for a Progress Dialog
        /// </summary>
        public ProgressDialog()
        {
            InitializeComponent();
        }
        /// <summary>
        /// A MapWinGIS.ICallback for messags etc.  Messages will be forwarded to this ICallBack.
        /// </summary>
        public MapWinGIS.ICallback ICallBack
        {
            get
            {
                return m_ICallBack;
            }
        }
        
        /// <summary>
        /// Sets values to be displayed for this object.
        /// </summary>
        /// <param name="Source">A String specifying the source file</param>
        /// <param name="Dest">A String specifying the destination file</param>
        /// <param name="Target">The Framework that has the status information</param>
        public void init(string Source, string Dest, MapWinGeoProc.Pitfill.Framework Target)
        {
            frameworkStatus1.myFramework = Target;
            lblSource.Text = Source;
            lblDest.Text = Dest;
            m_StartTime = DateTime.Now;
        }
        /// <summary>
        /// This is an optional callback object to report progress to.
        /// </summary>
        public MapWinGIS.ICallback CallBack
        {
            get
            {
                return m_ICallBack;
            }
            set
            {
                m_ICallBack = value;
            }
        }
       
        /// <summary>
        /// Writes a message to this dialog, and echos the message to the CallBack interface if one is specified.
        /// </summary>
        /// <param name="Message">A String message to be written to the dialog or callback</param>
        public void WriteMessage(string Message)
        {
            if (this.Visible == true)
            {
                richTextBox1.AppendText(Message);
                richTextBox1.SelectionStart = richTextBox1.Text.Length;
                Application.DoEvents();
            }
            if (m_ICallBack != null)
            {
                m_ICallBack.Progress("Status", 0, Message);
            }
        }
        /// <summary>
        /// Shows the progress of the dialog
        /// </summary>
        public int Progress
        {
            get
            {
                return m_Progress;
            }
            set
            {
                if (this.Visible == false) return;
                m_Progress = value;
                if (m_Progress > 100) m_Progress = 100;
                progressBar1.Value = m_Progress;
            }
           
        }
        /// <summary>
        /// If we are showing this dialog, redraw the graphical progress display
        /// </summary>
        public void ReDraw()
        {
            if (this.Visible == false) return;
            m_ExecutionTime = DateTime.Now - m_StartTime;
            lblTime.Text = m_ExecutionTime.Hours.ToString() + "Hours, " + m_ExecutionTime.Minutes.ToString() + " Minutes, " + m_ExecutionTime.Seconds.ToString() + " Seconds";
            frameworkStatus1.Redraw();
            
            Application.DoEvents();
        }
        /// <summary>
        /// Causes the progress dialog to allow itself to be redrawn
        /// </summary>
        public void DoEvents()
        {
            Application.DoEvents();
        }
        
        // Cancels the fill process
        private void cmdCancle_Click(object sender, EventArgs e)
        {
            m_Canceled = true;
            this.Hide();
        }
        /// <summary>
        /// Tests whether the cancel button has been pressed or not.
        /// </summary>
        /// <returns>True if the cancel button has been pressed.</returns>
        public bool IsCanceled
        {
            get
            {
                return m_Canceled;
            }
        }
      
        private void Pause_Click(object sender, EventArgs e)
        {
            m_Paused = !m_Paused;
            if (m_Paused)
            {
                Pause.Text = "Resume";
            }
            else
            {
                Pause.Text = "Pause";
            }
            Application.DoEvents();
        }
        /// <summary>
        /// Boolean.  Returns true if the program should pause, and false otherwise.
        /// </summary>
        public bool IsPaused
        {
            get
            {
                return m_Paused;
            }
        }
        

        
    }
}