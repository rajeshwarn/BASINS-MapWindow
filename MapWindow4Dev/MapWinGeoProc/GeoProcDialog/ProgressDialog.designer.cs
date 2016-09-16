namespace MapWinGeoProc.Dialogs
{
    /// <summary>
    /// A class allowing progress messages to be displayed while long algorithms are taking place.
    /// </summary>
    partial class ProgressDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressDialog));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.lblTimeLabel = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lblSource = new System.Windows.Forms.Label();
            this.lblDest = new System.Windows.Forms.Label();
            this.lblSourcelbl = new System.Windows.Forms.Label();
            this.lblDestlbl = new System.Windows.Forms.Label();
            this.cmdCancle = new System.Windows.Forms.Button();
            this.Pause = new System.Windows.Forms.Button();
            this.frameworkStatus1 = new MapWinGeoProc.Dialogs.FrameworkStatus();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(2, 307);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(516, 12);
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Value = 100;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(334, 208);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // lblTimeLabel
            // 
            this.lblTimeLabel.AutoSize = true;
            this.lblTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeLabel.Location = new System.Drawing.Point(440, 10);
            this.lblTimeLabel.Name = "lblTimeLabel";
            this.lblTimeLabel.Size = new System.Drawing.Size(42, 16);
            this.lblTimeLabel.TabIndex = 3;
            this.lblTimeLabel.Text = "Time:";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.Location = new System.Drawing.Point(362, 37);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(45, 16);
            this.lblTime.TabIndex = 4;
            this.lblTime.Text = "label1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(2, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblSource);
            this.splitContainer1.Panel2.Controls.Add(this.lblDest);
            this.splitContainer1.Panel2.Controls.Add(this.lblSourcelbl);
            this.splitContainer1.Panel2.Controls.Add(this.lblTimeLabel);
            this.splitContainer1.Panel2.Controls.Add(this.lblDestlbl);
            this.splitContainer1.Panel2.Controls.Add(this.lblTime);
            this.splitContainer1.Size = new System.Drawing.Size(705, 296);
            this.splitContainer1.SplitterDistance = 208;
            this.splitContainer1.TabIndex = 6;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.richTextBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.frameworkStatus1);
            this.splitContainer2.Size = new System.Drawing.Size(705, 208);
            this.splitContainer2.SplitterDistance = 334;
            this.splitContainer2.TabIndex = 0;
            // 
            // lblSource
            // 
            this.lblSource.AutoSize = true;
            this.lblSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSource.Location = new System.Drawing.Point(67, 10);
            this.lblSource.Name = "lblSource";
            this.lblSource.Size = new System.Drawing.Size(17, 16);
            this.lblSource.TabIndex = 6;
            this.lblSource.Text = "...";
            // 
            // lblDest
            // 
            this.lblDest.AutoSize = true;
            this.lblDest.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDest.Location = new System.Drawing.Point(67, 51);
            this.lblDest.Name = "lblDest";
            this.lblDest.Size = new System.Drawing.Size(17, 16);
            this.lblDest.TabIndex = 8;
            this.lblDest.Text = "...";
            // 
            // lblSourcelbl
            // 
            this.lblSourcelbl.AutoSize = true;
            this.lblSourcelbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSourcelbl.Location = new System.Drawing.Point(10, 10);
            this.lblSourcelbl.Name = "lblSourcelbl";
            this.lblSourcelbl.Size = new System.Drawing.Size(51, 16);
            this.lblSourcelbl.TabIndex = 5;
            this.lblSourcelbl.Text = "Source";
            // 
            // lblDestlbl
            // 
            this.lblDestlbl.AutoSize = true;
            this.lblDestlbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDestlbl.Location = new System.Drawing.Point(10, 51);
            this.lblDestlbl.Name = "lblDestlbl";
            this.lblDestlbl.Size = new System.Drawing.Size(36, 16);
            this.lblDestlbl.TabIndex = 7;
            this.lblDestlbl.Text = "Dest";
            // 
            // cmdCancle
            // 
            this.cmdCancle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancle.Location = new System.Drawing.Point(633, 303);
            this.cmdCancle.Name = "cmdCancle";
            this.cmdCancle.Size = new System.Drawing.Size(75, 23);
            this.cmdCancle.TabIndex = 7;
            this.cmdCancle.Text = "Cancel";
            this.cmdCancle.UseVisualStyleBackColor = true;
            this.cmdCancle.Click += new System.EventHandler(this.cmdCancle_Click);
            // 
            // Pause
            // 
            this.Pause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Pause.Location = new System.Drawing.Point(548, 303);
            this.Pause.Name = "Pause";
            this.Pause.Size = new System.Drawing.Size(79, 23);
            this.Pause.TabIndex = 8;
            this.Pause.Text = "Pause";
            this.Pause.UseVisualStyleBackColor = true;
            this.Pause.Click += new System.EventHandler(this.Pause_Click);
            // 
            // frameworkStatus1
            // 
            this.frameworkStatus1.BackColor = System.Drawing.Color.White;
            this.frameworkStatus1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.frameworkStatus1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.frameworkStatus1.Location = new System.Drawing.Point(0, 0);
            this.frameworkStatus1.Name = "frameworkStatus1";
            this.frameworkStatus1.Size = new System.Drawing.Size(367, 208);
            this.frameworkStatus1.TabIndex = 5;
            // 
            // ProgressDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 331);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Pause);
            this.Controls.Add(this.cmdCancle);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProgressDialog";
            this.Text = "Fill Progress";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label lblTimeLabel;
        private System.Windows.Forms.Label lblTime;
        private MapWinGeoProc.Dialogs.FrameworkStatus frameworkStatus1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Label lblDest;
        private System.Windows.Forms.Label lblDestlbl;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.Label lblSourcelbl;
        private System.Windows.Forms.Button cmdCancle;
        private System.Windows.Forms.Button Pause;
    }
}