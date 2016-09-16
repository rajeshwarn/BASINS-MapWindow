namespace MapWinGeoProc.Dialogs
{
    partial class GeoProcDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GeoProcDialog));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelElementContainer = new System.Windows.Forms.Panel();
            this.panelOkCancel = new System.Windows.Forms.Panel();
            this.cmdToggleHelp = new System.Windows.Forms.Button();
            this.cmdCancle = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.lblHelpImage = new System.Windows.Forms.Label();
            this.panelHelpTitle = new System.Windows.Forms.Panel();
            this.rtbHelpTitle = new System.Windows.Forms.RichTextBox();
            this.PanelWiki = new System.Windows.Forms.Panel();
            this.cmdShrink = new System.Windows.Forms.Button();
            this.cmdEnlarge = new System.Windows.Forms.Button();
            this.cmdWiki = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelOkCancel.SuspendLayout();
            this.panelHelpTitle.SuspendLayout();
            this.PanelWiki.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelElementContainer);
            this.splitContainer1.Panel1.Controls.Add(this.panelOkCancel);
            this.splitContainer1.Panel1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Panel1_MouseWheel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.AutoScrollMinSize = new System.Drawing.Size(85, 0);
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.lblHelpImage);
            this.splitContainer1.Panel2.Controls.Add(this.panelHelpTitle);
            this.splitContainer1.Panel2.Controls.Add(this.PanelWiki);
            this.splitContainer1.Panel2.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.HelpPanel_MouseWheel);
            this.splitContainer1.Size = new System.Drawing.Size(664, 373);
            this.splitContainer1.SplitterDistance = 408;
            this.splitContainer1.TabIndex = 2;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // panelElementContainer
            // 
            this.panelElementContainer.AutoScroll = true;
            this.panelElementContainer.AutoScrollMinSize = new System.Drawing.Size(250, 0);
            this.panelElementContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelElementContainer.Location = new System.Drawing.Point(0, 0);
            this.panelElementContainer.Name = "panelElementContainer";
            this.panelElementContainer.Size = new System.Drawing.Size(408, 325);
            this.panelElementContainer.TabIndex = 2;
            this.panelElementContainer.Click += new System.EventHandler(this.panelElementContainer_Click);
            // 
            // panelOkCancel
            // 
            this.panelOkCancel.Controls.Add(this.cmdToggleHelp);
            this.panelOkCancel.Controls.Add(this.cmdCancle);
            this.panelOkCancel.Controls.Add(this.cmdOK);
            this.panelOkCancel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOkCancel.Location = new System.Drawing.Point(0, 325);
            this.panelOkCancel.Name = "panelOkCancel";
            this.panelOkCancel.Size = new System.Drawing.Size(408, 48);
            this.panelOkCancel.TabIndex = 1;
            this.panelOkCancel.Click += new System.EventHandler(this.panelElementContainer_Click);
            // 
            // cmdToggleHelp
            // 
            this.cmdToggleHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdToggleHelp.Location = new System.Drawing.Point(310, 15);
            this.cmdToggleHelp.Name = "cmdToggleHelp";
            this.cmdToggleHelp.Size = new System.Drawing.Size(95, 21);
            this.cmdToggleHelp.TabIndex = 2;
            this.cmdToggleHelp.Text = "<< Hide Help";
            this.cmdToggleHelp.UseVisualStyleBackColor = true;
            this.cmdToggleHelp.Click += new System.EventHandler(this.cmdToggleHelp_Click);
            // 
            // cmdCancle
            // 
            this.cmdCancle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancle.Location = new System.Drawing.Point(229, 15);
            this.cmdCancle.Name = "cmdCancle";
            this.cmdCancle.Size = new System.Drawing.Size(75, 23);
            this.cmdCancle.TabIndex = 1;
            this.cmdCancle.Text = "Cancel";
            this.cmdCancle.UseVisualStyleBackColor = true;
            this.cmdCancle.Click += new System.EventHandler(this.cmdCancle_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(153, 15);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(70, 23);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // lblHelpImage
            // 
            this.lblHelpImage.Location = new System.Drawing.Point(14, 246);
            this.lblHelpImage.Name = "lblHelpImage";
            this.lblHelpImage.Size = new System.Drawing.Size(71, 36);
            this.lblHelpImage.TabIndex = 4;
            this.lblHelpImage.Visible = false;
            this.lblHelpImage.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.HelpPanel_MouseWheel);
            // 
            // panelHelpTitle
            // 
            this.panelHelpTitle.Controls.Add(this.rtbHelpTitle);
            this.panelHelpTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHelpTitle.Location = new System.Drawing.Point(0, 32);
            this.panelHelpTitle.Name = "panelHelpTitle";
            this.panelHelpTitle.Size = new System.Drawing.Size(252, 205);
            this.panelHelpTitle.TabIndex = 5;
            // 
            // rtbHelpTitle
            // 
            this.rtbHelpTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbHelpTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbHelpTitle.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbHelpTitle.Location = new System.Drawing.Point(0, 0);
            this.rtbHelpTitle.Name = "rtbHelpTitle";
            this.rtbHelpTitle.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtbHelpTitle.Size = new System.Drawing.Size(252, 205);
            this.rtbHelpTitle.TabIndex = 2;
            this.rtbHelpTitle.Text = "Enter A Title";
            this.rtbHelpTitle.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.HelpPanel_MouseWheel);
            // 
            // PanelWiki
            // 
            this.PanelWiki.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelWiki.Controls.Add(this.cmdShrink);
            this.PanelWiki.Controls.Add(this.cmdEnlarge);
            this.PanelWiki.Controls.Add(this.cmdWiki);
            this.PanelWiki.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelWiki.Location = new System.Drawing.Point(0, 0);
            this.PanelWiki.Name = "PanelWiki";
            this.PanelWiki.Size = new System.Drawing.Size(252, 32);
            this.PanelWiki.TabIndex = 2;
            this.PanelWiki.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.HelpPanel_MouseWheel);
            this.PanelWiki.Click += new System.EventHandler(this.PanelWiki_Click);
            // 
            // cmdShrink
            // 
            this.cmdShrink.Image = ((System.Drawing.Image)(resources.GetObject("cmdShrink.Image")));
            this.cmdShrink.Location = new System.Drawing.Point(119, 3);
            this.cmdShrink.Name = "cmdShrink";
            this.cmdShrink.Size = new System.Drawing.Size(24, 25);
            this.cmdShrink.TabIndex = 2;
            this.cmdShrink.UseVisualStyleBackColor = true;
            this.cmdShrink.Click += new System.EventHandler(this.cmdShrink_Click);
            // 
            // cmdEnlarge
            // 
            this.cmdEnlarge.Image = ((System.Drawing.Image)(resources.GetObject("cmdEnlarge.Image")));
            this.cmdEnlarge.Location = new System.Drawing.Point(90, 3);
            this.cmdEnlarge.Name = "cmdEnlarge";
            this.cmdEnlarge.Size = new System.Drawing.Size(23, 25);
            this.cmdEnlarge.TabIndex = 1;
            this.cmdEnlarge.UseVisualStyleBackColor = true;
            this.cmdEnlarge.Click += new System.EventHandler(this.cmdEnlarge_Click);
            // 
            // cmdWiki
            // 
            this.cmdWiki.Image = ((System.Drawing.Image)(resources.GetObject("cmdWiki.Image")));
            this.cmdWiki.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdWiki.Location = new System.Drawing.Point(6, 3);
            this.cmdWiki.Name = "cmdWiki";
            this.cmdWiki.Size = new System.Drawing.Size(78, 25);
            this.cmdWiki.TabIndex = 0;
            this.cmdWiki.Text = "Wiki Help";
            this.cmdWiki.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdWiki.UseVisualStyleBackColor = true;
            this.cmdWiki.Click += new System.EventHandler(this.cmdWiki_Click);
            // 
            // GeoProcDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 373);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GeoProcDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Example";
            this.Resize += new System.EventHandler(this.frmCustomDialog_Resize);
            this.Shown += new System.EventHandler(this.frmCustomDialog_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panelOkCancel.ResumeLayout(false);
            this.panelHelpTitle.ResumeLayout(false);
            this.PanelWiki.ResumeLayout(false);
            this.ResumeLayout(false);

        }

       

        
       
        #endregion

       // private OpenFileElement openFileElement1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelOkCancel;
        private System.Windows.Forms.Button cmdToggleHelp;
        private System.Windows.Forms.Button cmdCancle;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Panel panelElementContainer;
        private System.Windows.Forms.Panel PanelWiki;
        private System.Windows.Forms.Button cmdWiki;
        private System.Windows.Forms.Label lblHelpImage;
        private System.Windows.Forms.Panel panelHelpTitle;
        private System.Windows.Forms.RichTextBox rtbHelpTitle;
        private System.Windows.Forms.Button cmdEnlarge;
        private System.Windows.Forms.Button cmdShrink;




    }
}

