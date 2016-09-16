using System.Windows.Forms;
namespace MapWinGeoProc.Dialogs
{
    partial class BatchFileElement : UserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileElement));
            this.cmdHelp = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdDown = new System.Windows.Forms.Button();
            this.cmdUp = new System.Windows.Forms.Button();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblLight = new System.Windows.Forms.Label();
            this.LightList = new System.Windows.Forms.ImageList(this.components);
            this.tbFilename = new System.Windows.Forms.TextBox();
            this.cmdBrowse = new System.Windows.Forms.Button();
            this.ttLight = new System.Windows.Forms.ToolTip(this.components);
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.ttGrip = new System.Windows.Forms.ToolTip(this.components);
            this.ttBrowse = new System.Windows.Forms.ToolTip(this.components);
            this.ttDelete = new System.Windows.Forms.ToolTip(this.components);
            this.ttAdd = new System.Windows.Forms.ToolTip(this.components);
            this.ttUp = new System.Windows.Forms.ToolTip(this.components);
            this.ttDown = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmdHelp.Image = ((System.Drawing.Image)(resources.GetObject("cmdHelp.Image")));
            this.cmdHelp.Location = new System.Drawing.Point(413, 16);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(27, 21);
            this.cmdHelp.TabIndex = 0;
            this.cmdHelp.UseVisualStyleBackColor = true;
            this.cmdHelp.Click += new System.EventHandler(this.cmdHelp_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox1.Controls.Add(this.cmdDown);
            this.groupBox1.Controls.Add(this.cmdUp);
            this.groupBox1.Controls.Add(this.cmdDelete);
            this.groupBox1.Controls.Add(this.cmdAdd);
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.lblLight);
            this.groupBox1.Controls.Add(this.tbFilename);
            this.groupBox1.Controls.Add(this.cmdHelp);
            this.groupBox1.Controls.Add(this.cmdBrowse);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(457, 196);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Caption";
            this.groupBox1.Click += new System.EventHandler(this.groupBox1_Click);
            // 
            // cmdDown
            // 
            this.cmdDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDown.Image = ((System.Drawing.Image)(resources.GetObject("cmdDown.Image")));
            this.cmdDown.Location = new System.Drawing.Point(411, 157);
            this.cmdDown.Name = "cmdDown";
            this.cmdDown.Size = new System.Drawing.Size(32, 28);
            this.cmdDown.TabIndex = 8;
            this.ttUp.SetToolTip(this.cmdDown, "Move Selected File Down");
            this.cmdDown.UseVisualStyleBackColor = true;
            this.cmdDown.Click += new System.EventHandler(this.cmdDown_Click);
            // 
            // cmdUp
            // 
            this.cmdUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdUp.Image = ((System.Drawing.Image)(resources.GetObject("cmdUp.Image")));
            this.cmdUp.Location = new System.Drawing.Point(411, 123);
            this.cmdUp.Name = "cmdUp";
            this.cmdUp.Size = new System.Drawing.Size(32, 28);
            this.cmdUp.TabIndex = 7;
            this.ttUp.SetToolTip(this.cmdUp, "Move Selected File Up");
            this.cmdUp.UseVisualStyleBackColor = true;
            this.cmdUp.Click += new System.EventHandler(this.cmdUp_Click);
            // 
            // cmdDelete
            // 
            this.cmdDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDelete.Image = ((System.Drawing.Image)(resources.GetObject("cmdDelete.Image")));
            this.cmdDelete.Location = new System.Drawing.Point(411, 89);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(32, 28);
            this.cmdDelete.TabIndex = 6;
            this.ttDelete.SetToolTip(this.cmdDelete, "Delete Selected File");
            this.cmdDelete.UseVisualStyleBackColor = true;
            // 
            // cmdAdd
            // 
            this.cmdAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdAdd.Image = ((System.Drawing.Image)(resources.GetObject("cmdAdd.Image")));
            this.cmdAdd.Location = new System.Drawing.Point(411, 55);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(32, 28);
            this.cmdAdd.TabIndex = 5;
            this.ttAdd.SetToolTip(this.cmdAdd, "Add New Files");
            this.cmdAdd.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 55);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(389, 129);
            this.dataGridView1.TabIndex = 4;
            // 
            // lblLight
            // 
            this.lblLight.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblLight.ImageIndex = 0;
            this.lblLight.ImageList = this.LightList;
            this.lblLight.Location = new System.Drawing.Point(1, 16);
            this.lblLight.Name = "lblLight";
            this.lblLight.Size = new System.Drawing.Size(25, 21);
            this.lblLight.TabIndex = 1;
            // 
            // LightList
            // 
            this.LightList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("LightList.ImageStream")));
            this.LightList.TransparentColor = System.Drawing.Color.Transparent;
            this.LightList.Images.SetKeyName(0, "Yellow.ico");
            this.LightList.Images.SetKeyName(1, "Green.ico");
            this.LightList.Images.SetKeyName(2, "Red.ico");
            // 
            // tbFilename
            // 
            this.tbFilename.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tbFilename.Location = new System.Drawing.Point(32, 16);
            this.tbFilename.Name = "tbFilename";
            this.tbFilename.Size = new System.Drawing.Size(339, 20);
            this.tbFilename.TabIndex = 2;
            this.tbFilename.Enter += new System.EventHandler(this.tbFilename_Enter);
            this.tbFilename.Validating += new System.ComponentModel.CancelEventHandler(this.tbFilename_Validating);
            // 
            // cmdBrowse
            // 
            this.cmdBrowse.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmdBrowse.Image = ((System.Drawing.Image)(resources.GetObject("cmdBrowse.Image")));
            this.cmdBrowse.Location = new System.Drawing.Point(377, 16);
            this.cmdBrowse.Name = "cmdBrowse";
            this.cmdBrowse.Size = new System.Drawing.Size(29, 20);
            this.cmdBrowse.TabIndex = 3;
            this.cmdBrowse.UseVisualStyleBackColor = true;
            this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
            // 
            // ttDelete
            // 
            this.ttDelete.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // FileElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.groupBox1);
            this.Name = "BatchFileElement";
            this.Size = new System.Drawing.Size(457, 196);
            this.Load += new System.EventHandler(this.FileElement_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        void FileElement_Load(object sender, System.EventArgs e)
        {
            cmdHelp.Visible = m_HelpButtonVisible;
           
        }

        

      

        

        #endregion

        private System.Windows.Forms.Button cmdBrowse;
        private System.Windows.Forms.TextBox tbFilename;
        private System.Windows.Forms.ToolTip ttBrowse;
        private System.Windows.Forms.ImageList LightList;
        /// <summary>
        /// Help button
        /// </summary>
        protected Button cmdHelp;
        /// <summary>
        /// Status Light
        /// </summary>
        protected Label lblLight;
        /// <summary>
        /// A group box to surround individual components
        /// </summary>
        public GroupBox groupBox1;
        private ToolTip ttLight;
        private ToolTip ttHelp;
        private ToolTip ttGrip;
        private Button cmdDown;
        private Button cmdUp;
        private Button cmdDelete;
        private Button cmdAdd;
        private DataGridView dataGridView1;
        private ToolTip ttDelete;
        private ToolTip ttAdd;
        private ToolTip ttUp;
        private ToolTip ttDown;
        
    }
}
