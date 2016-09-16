namespace EPSG_Reference
{
    using MapWindow.Controls.Projections;

    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.btnUpdateAreas = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.projectionTreeView1 = new ProjectionTreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox1 = new ProjectionTextBox();
            this.btnSetExtents = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
            this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
            this.btnPan = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSelect = new System.Windows.Forms.ToolStripButton();
            this.btnClearExtents = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnFillCountryByArea = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblPosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblViewBounds = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSelection = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblGCS = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblPCS = new System.Windows.Forms.ToolStripStatusLabel();
            this.axMap1 = new ProjectionMap();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMap1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnUpdateAreas
            // 
            this.btnUpdateAreas.Location = new System.Drawing.Point(793, 406);
            this.btnUpdateAreas.Name = "btnUpdateAreas";
            this.btnUpdateAreas.Size = new System.Drawing.Size(90, 26);
            this.btnUpdateAreas.TabIndex = 6;
            this.btnUpdateAreas.Text = "Update Areas";
            this.btnUpdateAreas.UseVisualStyleBackColor = true;
            this.btnUpdateAreas.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 46);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.axMap1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(686, 374);
            this.splitContainer1.SplitterDistance = 493;
            this.splitContainer1.TabIndex = 14;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.projectionTreeView1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel1);
            this.splitContainer2.Size = new System.Drawing.Size(189, 374);
            this.splitContainer2.SplitterDistance = 235;
            this.splitContainer2.TabIndex = 15;
            // 
            // projectionTreeView1
            // 
            this.projectionTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.projectionTreeView1.ImageIndex = 0;
            this.projectionTreeView1.Location = new System.Drawing.Point(0, 0);
            this.projectionTreeView1.Name = "projectionTreeView1";
            this.projectionTreeView1.SelectedImageIndex = 0;
            this.projectionTreeView1.Size = new System.Drawing.Size(189, 235);
            this.projectionTreeView1.TabIndex = 0;
            this.projectionTreeView1.CoordinateSystemSelected += new ProjectionTreeView.CoordinateSystemSelectedDelegate(this.projectionTreeView1_CoordinateSystemSelected);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(189, 135);
            this.panel1.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(189, 135);
            this.panel2.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(187, 133);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "";
            // 
            // btnSetExtents
            // 
            this.btnSetExtents.Location = new System.Drawing.Point(793, 374);
            this.btnSetExtents.Name = "btnSetExtents";
            this.btnSetExtents.Size = new System.Drawing.Size(77, 26);
            this.btnSetExtents.TabIndex = 14;
            this.btnSetExtents.Text = "Limit extents";
            this.btnSetExtents.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZoomIn,
            this.btnZoomOut,
            this.btnPan,
            this.toolStripSeparator1,
            this.btnSelect,
            this.btnClearExtents,
            this.toolStripSeparator2,
            this.btnFillCountryByArea});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(686, 46);
            this.toolStrip1.TabIndex = 15;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Checked = true;
            this.btnZoomIn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnZoomIn.Image = global::EPSG_Reference.Properties.Resources.zoom_in;
            this.btnZoomIn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(56, 43);
            this.btnZoomIn.Text = "Zoom in";
            this.btnZoomIn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Image = global::EPSG_Reference.Properties.Resources.zoom_out;
            this.btnZoomOut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(64, 43);
            this.btnZoomOut.Text = "Zoom out";
            this.btnZoomOut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // btnPan
            // 
            this.btnPan.Image = global::EPSG_Reference.Properties.Resources.pan;
            this.btnPan.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnPan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPan.Name = "btnPan";
            this.btnPan.Size = new System.Drawing.Size(58, 43);
            this.btnPan.Text = "Pan map";
            this.btnPan.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 46);
            // 
            // btnSelect
            // 
            this.btnSelect.Image = global::EPSG_Reference.Properties.Resources.zoom_selection;
            this.btnSelect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(70, 43);
            this.btnSelect.Text = "Set bounds";
            this.btnSelect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // btnClearExtents
            // 
            this.btnClearExtents.Image = global::EPSG_Reference.Properties.Resources.zoom_extent;
            this.btnClearExtents.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnClearExtents.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearExtents.Name = "btnClearExtents";
            this.btnClearExtents.Size = new System.Drawing.Size(81, 43);
            this.btnClearExtents.Text = "Clear bounds";
            this.btnClearExtents.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 46);
            // 
            // btnFillCountryByArea
            // 
            this.btnFillCountryByArea.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnFillCountryByArea.Image = ((System.Drawing.Image)(resources.GetObject("btnFillCountryByArea.Image")));
            this.btnFillCountryByArea.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFillCountryByArea.Name = "btnFillCountryByArea";
            this.btnFillCountryByArea.Size = new System.Drawing.Size(115, 43);
            this.btnFillCountryByArea.Text = "Fill Area by Country";
            this.btnFillCountryByArea.Visible = false;
            this.btnFillCountryByArea.Click += new System.EventHandler(this.btnFillCountryByArea_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblPosition,
            this.lblViewBounds,
            this.lblSelection,
            this.lblGCS,
            this.lblPCS});
            this.statusStrip1.Location = new System.Drawing.Point(0, 420);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(686, 24);
            this.statusStrip1.TabIndex = 16;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblPosition
            // 
            this.lblPosition.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblPosition.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(77, 19);
            this.lblPosition.Text = "X=0.0; Y=0.0";
            // 
            // lblViewBounds
            // 
            this.lblViewBounds.Name = "lblViewBounds";
            this.lblViewBounds.Size = new System.Drawing.Size(75, 19);
            this.lblViewBounds.Text = "ViewBounds:";
            this.lblViewBounds.Visible = false;
            // 
            // lblSelection
            // 
            this.lblSelection.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblSelection.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblSelection.Name = "lblSelection";
            this.lblSelection.Size = new System.Drawing.Size(105, 19);
            this.lblSelection.Text = "Selection bounds:";
            this.lblSelection.Visible = false;
            // 
            // lblGCS
            // 
            this.lblGCS.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblGCS.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.lblGCS.Name = "lblGCS";
            this.lblGCS.Size = new System.Drawing.Size(70, 19);
            this.lblGCS.Text = "GCS count:";
            // 
            // lblPCS
            // 
            this.lblPCS.Name = "lblPCS";
            this.lblPCS.Size = new System.Drawing.Size(65, 19);
            this.lblPCS.Text = "PCS count:";
            // 
            // axMap1
            // 
            this.axMap1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMap1.Enabled = true;
            this.axMap1.Location = new System.Drawing.Point(0, 0);
            this.axMap1.Name = "axMap1";
            this.axMap1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMap1.OcxState")));
            this.axMap1.Size = new System.Drawing.Size(493, 374);
            this.axMap1.TabIndex = 1;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 444);
            this.Controls.Add(this.btnUpdateAreas);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnSetExtents);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "frmMain";
            this.Text = "EPSG Coordinate System Reference";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMap1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpdateAreas;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSetExtents;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnZoomIn;
        private System.Windows.Forms.ToolStripButton btnZoomOut;
        private System.Windows.Forms.ToolStripButton btnPan;
        private System.Windows.Forms.ToolStripButton btnSelect;
        private System.Windows.Forms.ToolStripButton btnClearExtents;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblPosition;
        private System.Windows.Forms.ToolStripStatusLabel lblGCS;
        private System.Windows.Forms.ToolStripStatusLabel lblPCS;
        private System.Windows.Forms.ToolStripStatusLabel lblViewBounds;
        private System.Windows.Forms.ToolStripStatusLabel lblSelection;
        private ProjectionTreeView projectionTreeView1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripButton btnFillCountryByArea;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private ProjectionTextBox textBox1;
        private ProjectionMap axMap1;
    }
}

