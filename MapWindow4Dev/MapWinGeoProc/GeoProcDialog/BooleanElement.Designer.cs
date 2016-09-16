using System.Windows.Forms;
namespace MapWinGeoProc.Dialogs
{
    partial class BooleanElement : UserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BooleanElement));
            this.cmdHelp = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LightList = new System.Windows.Forms.ImageList(this.components);
            this.cbValue = new System.Windows.Forms.CheckBox();
            this.lblGrip = new System.Windows.Forms.Label();
            this.ttLight = new System.Windows.Forms.ToolTip(this.components);
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.ttGrip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cmdHelp.Image = ((System.Drawing.Image)(resources.GetObject("cmdHelp.Image")));
            this.cmdHelp.Location = new System.Drawing.Point(414, 18);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(27, 21);
            this.cmdHelp.TabIndex = 0;
            this.cmdHelp.UseVisualStyleBackColor = true;
            this.cmdHelp.Click += new System.EventHandler(this.cmdHelp_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox1.Controls.Add(this.cmdHelp);
            this.groupBox1.Controls.Add(this.cbValue);
            this.groupBox1.Controls.Add(this.lblGrip);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(460, 45);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Click += new System.EventHandler(this.groupBox1_Click);
            // 
            // LightList
            // 
            this.LightList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("LightList.ImageStream")));
            this.LightList.TransparentColor = System.Drawing.Color.Transparent;
            this.LightList.Images.SetKeyName(0, "Yellow.ico");
            this.LightList.Images.SetKeyName(1, "Green.ico");
            this.LightList.Images.SetKeyName(2, "Red.ico");
            // 
            // cbValue
            // 
            this.cbValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbValue.AutoSize = true;
            this.cbValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbValue.Location = new System.Drawing.Point(6, 16);
            this.cbValue.Name = "cbValue";
            this.cbValue.Size = new System.Drawing.Size(114, 20);
            this.cbValue.TabIndex = 4;
            this.cbValue.Text = "A Check Box";
            this.cbValue.UseVisualStyleBackColor = true;
            // 
            // lblGrip
            // 
            this.lblGrip.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.lblGrip.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblGrip.Image = ((System.Drawing.Image)(resources.GetObject("lblGrip.Image")));
            this.lblGrip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblGrip.Location = new System.Drawing.Point(447, 16);
            this.lblGrip.Name = "lblGrip";
            this.lblGrip.Size = new System.Drawing.Size(10, 26);
            this.lblGrip.TabIndex = 2;
            this.lblGrip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblGrip_MouseDown);
            this.lblGrip.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblGrip_MouseMove);
            this.lblGrip.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblGrip_MouseUp);
            // 
            // BooleanElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.groupBox1);
            this.Name = "BooleanElement";
            this.Size = new System.Drawing.Size(460, 45);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

     

        #endregion

        private System.Windows.Forms.CheckBox cbValue;
        private System.Windows.Forms.ImageList LightList;
        /// <summary>
        /// Help button
        /// </summary>
        protected Button cmdHelp;
        /// <summary>
        /// A group box to surround individual components
        /// </summary>
        public GroupBox groupBox1;
        private Label lblGrip;
        private ToolTip ttLight;
        private ToolTip ttHelp;
        private ToolTip ttGrip;
    }
}
