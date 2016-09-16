namespace MapWindow.Tiles
{
    partial class TilesControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        //private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLat = new System.Windows.Forms.TextBox();
            this.txtLong = new System.Windows.Forms.TextBox();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboProvider = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblRamCache = new System.Windows.Forms.Label();
            this.lblDatabaseCache = new System.Windows.Forms.Label();
            this.btnTilesSettings = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkUseDatabase = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelExtents = new System.Windows.Forms.GroupBox();
            this.panelBounds = new System.Windows.Forms.Panel();
            this.txtMaxLon = new System.Windows.Forms.TextBox();
            this.txtMinLon = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMaxLat = new System.Windows.Forms.TextBox();
            this.txtMinLat = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnDoCaching = new System.Windows.Forms.Button();
            this.optCustom = new System.Windows.Forms.RadioButton();
            this.optLayers = new System.Windows.Forms.RadioButton();
            this.cboLayers = new System.Windows.Forms.ComboBox();
            this.btnChooseExtents = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.chkServer = new System.Windows.Forms.CheckBox();
            this.chkUseRam = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnBrowseCache = new System.Windows.Forms.Button();
            this.chkExtents = new System.Windows.Forms.CheckBox();
            this.optAll = new System.Windows.Forms.RadioButton();
            this.optExtents = new System.Windows.Forms.RadioButton();
            this.btnFillMemory = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelExtents.SuspendLayout();
            this.panelBounds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Latitude";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Longitude";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Find";
            // 
            // txtLat
            // 
            this.txtLat.Location = new System.Drawing.Point(81, 26);
            this.txtLat.Name = "txtLat";
            this.txtLat.Size = new System.Drawing.Size(87, 20);
            this.txtLat.TabIndex = 3;
            this.txtLat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLat_KeyPress);
            // 
            // txtLong
            // 
            this.txtLong.Location = new System.Drawing.Point(81, 56);
            this.txtLong.Name = "txtLong";
            this.txtLong.Size = new System.Drawing.Size(87, 20);
            this.txtLong.TabIndex = 4;
            // 
            // txtFind
            // 
            this.txtFind.Location = new System.Drawing.Point(68, 96);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(145, 20);
            this.txtFind.TabIndex = 5;
            this.txtFind.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFind_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Provider";
            // 
            // cboProvider
            // 
            this.cboProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProvider.FormattingEnabled = true;
            this.cboProvider.Location = new System.Drawing.Point(68, 135);
            this.cboProvider.Name = "cboProvider";
            this.cboProvider.Size = new System.Drawing.Size(145, 21);
            this.cboProvider.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkServer);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cboProvider);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtFind);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtLong);
            this.groupBox1.Controls.Add(this.txtLat);
            this.groupBox1.Location = new System.Drawing.Point(51, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(233, 189);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tiles";
            // 
            // lblRamCache
            // 
            this.lblRamCache.AutoSize = true;
            this.lblRamCache.Location = new System.Drawing.Point(140, 57);
            this.lblRamCache.Name = "lblRamCache";
            this.lblRamCache.Size = new System.Drawing.Size(67, 13);
            this.lblRamCache.TabIndex = 30;
            this.lblRamCache.Text = "0.0/20.0 MB";
            // 
            // lblDatabaseCache
            // 
            this.lblDatabaseCache.AutoSize = true;
            this.lblDatabaseCache.Location = new System.Drawing.Point(140, 32);
            this.lblDatabaseCache.Name = "lblDatabaseCache";
            this.lblDatabaseCache.Size = new System.Drawing.Size(67, 13);
            this.lblDatabaseCache.TabIndex = 31;
            this.lblDatabaseCache.Text = "0.0/20.0 MB";
            // 
            // btnTilesSettings
            // 
            this.btnTilesSettings.Location = new System.Drawing.Point(141, 82);
            this.btnTilesSettings.Name = "btnTilesSettings";
            this.btnTilesSettings.Size = new System.Drawing.Size(86, 21);
            this.btnTilesSettings.TabIndex = 29;
            this.btnTilesSettings.Text = "Settings...";
            this.btnTilesSettings.UseVisualStyleBackColor = true;
            this.btnTilesSettings.Click += new System.EventHandler(this.btnTilesSettings_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(185, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "deg.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(185, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "deg.";
            // 
            // chkUseDatabase
            // 
            this.chkUseDatabase.AutoSize = true;
            this.chkUseDatabase.Location = new System.Drawing.Point(24, 31);
            this.chkUseDatabase.Name = "chkUseDatabase";
            this.chkUseDatabase.Size = new System.Drawing.Size(72, 17);
            this.chkUseDatabase.TabIndex = 24;
            this.chkUseDatabase.Text = "Database";
            this.chkUseDatabase.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboLayers);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.panelExtents);
            this.panel1.Controls.Add(this.optLayers);
            this.panel1.Controls.Add(this.chkExtents);
            this.panel1.Controls.Add(this.trackBar1);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(296, 501);
            this.panel1.TabIndex = 9;
            // 
            // panelExtents
            // 
            this.panelExtents.Controls.Add(this.btnFillMemory);
            this.panelExtents.Controls.Add(this.optExtents);
            this.panelExtents.Controls.Add(this.optAll);
            this.panelExtents.Controls.Add(this.chkUseRam);
            this.panelExtents.Controls.Add(this.btnDoCaching);
            this.panelExtents.Controls.Add(this.btnChooseExtents);
            this.panelExtents.Controls.Add(this.panelBounds);
            this.panelExtents.Controls.Add(this.lblRamCache);
            this.panelExtents.Controls.Add(this.lblDatabaseCache);
            this.panelExtents.Controls.Add(this.btnBrowseCache);
            this.panelExtents.Controls.Add(this.btnTilesSettings);
            this.panelExtents.Controls.Add(this.chkUseDatabase);
            this.panelExtents.Location = new System.Drawing.Point(51, 198);
            this.panelExtents.Name = "panelExtents";
            this.panelExtents.Size = new System.Drawing.Size(233, 300);
            this.panelExtents.TabIndex = 10;
            this.panelExtents.TabStop = false;
            this.panelExtents.Text = "Caching";
            // 
            // panelBounds
            // 
            this.panelBounds.Controls.Add(this.txtMaxLon);
            this.panelBounds.Controls.Add(this.txtMinLon);
            this.panelBounds.Controls.Add(this.label12);
            this.panelBounds.Controls.Add(this.label11);
            this.panelBounds.Controls.Add(this.txtMaxLat);
            this.panelBounds.Controls.Add(this.txtMinLat);
            this.panelBounds.Controls.Add(this.label10);
            this.panelBounds.Controls.Add(this.label8);
            this.panelBounds.Location = new System.Drawing.Point(24, 166);
            this.panelBounds.Name = "panelBounds";
            this.panelBounds.Size = new System.Drawing.Size(205, 68);
            this.panelBounds.TabIndex = 31;
            // 
            // txtMaxLon
            // 
            this.txtMaxLon.Location = new System.Drawing.Point(148, 42);
            this.txtMaxLon.Name = "txtMaxLon";
            this.txtMaxLon.Size = new System.Drawing.Size(45, 20);
            this.txtMaxLon.TabIndex = 18;
            this.txtMaxLon.Text = "180.0";
            // 
            // txtMinLon
            // 
            this.txtMinLon.Location = new System.Drawing.Point(75, 42);
            this.txtMinLon.Name = "txtMinLon";
            this.txtMinLon.Size = new System.Drawing.Size(45, 20);
            this.txtMinLon.TabIndex = 17;
            this.txtMinLon.Text = "-180.0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(126, 45);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(16, 13);
            this.label12.TabIndex = 16;
            this.label12.Text = "to";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 45);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "Longitude:";
            // 
            // txtMaxLat
            // 
            this.txtMaxLat.Location = new System.Drawing.Point(148, 7);
            this.txtMaxLat.Name = "txtMaxLat";
            this.txtMaxLat.Size = new System.Drawing.Size(45, 20);
            this.txtMaxLat.TabIndex = 13;
            this.txtMaxLat.Text = "90.0";
            // 
            // txtMinLat
            // 
            this.txtMinLat.Location = new System.Drawing.Point(75, 7);
            this.txtMinLat.Name = "txtMinLat";
            this.txtMinLat.Size = new System.Drawing.Size(45, 20);
            this.txtMinLat.TabIndex = 12;
            this.txtMinLat.Text = "-90.0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(126, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(16, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "to";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Latitude:";
            // 
            // btnDoCaching
            // 
            this.btnDoCaching.Location = new System.Drawing.Point(141, 240);
            this.btnDoCaching.Name = "btnDoCaching";
            this.btnDoCaching.Size = new System.Drawing.Size(86, 21);
            this.btnDoCaching.TabIndex = 30;
            this.btnDoCaching.Text = "Prefetch...";
            this.btnDoCaching.UseVisualStyleBackColor = true;
            this.btnDoCaching.Click += new System.EventHandler(this.btnDoCaching_Click);
            // 
            // optCustom
            // 
            this.optCustom.AutoSize = true;
            this.optCustom.Location = new System.Drawing.Point(18, 93);
            this.optCustom.Name = "optCustom";
            this.optCustom.Size = new System.Drawing.Size(97, 17);
            this.optCustom.TabIndex = 28;
            this.optCustom.Text = "Custom extents";
            this.optCustom.UseVisualStyleBackColor = true;
            // 
            // optLayers
            // 
            this.optLayers.AutoSize = true;
            this.optLayers.Checked = true;
            this.optLayers.Location = new System.Drawing.Point(337, 392);
            this.optLayers.Name = "optLayers";
            this.optLayers.Size = new System.Drawing.Size(138, 17);
            this.optLayers.TabIndex = 27;
            this.optLayers.TabStop = true;
            this.optLayers.Text = "Tiles in the layer extents";
            this.optLayers.UseVisualStyleBackColor = true;
            // 
            // cboLayers
            // 
            this.cboLayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLayers.FormattingEnabled = true;
            this.cboLayers.Location = new System.Drawing.Point(337, 420);
            this.cboLayers.Name = "cboLayers";
            this.cboLayers.Size = new System.Drawing.Size(179, 21);
            this.cboLayers.TabIndex = 25;
            // 
            // btnChooseExtents
            // 
            this.btnChooseExtents.Location = new System.Drawing.Point(55, 240);
            this.btnChooseExtents.Name = "btnChooseExtents";
            this.btnChooseExtents.Size = new System.Drawing.Size(80, 21);
            this.btnChooseExtents.TabIndex = 23;
            this.btnChooseExtents.Text = "Set current";
            this.btnChooseExtents.UseVisualStyleBackColor = true;
            this.btnChooseExtents.Click += new System.EventHandler(this.btnChooseExtents_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(0, 0);
            this.trackBar1.Maximum = 15;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar1.Size = new System.Drawing.Size(45, 501);
            this.trackBar1.TabIndex = 0;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // chkServer
            // 
            this.chkServer.AutoSize = true;
            this.chkServer.Location = new System.Drawing.Point(68, 162);
            this.chkServer.Name = "chkServer";
            this.chkServer.Size = new System.Drawing.Size(77, 17);
            this.chkServer.TabIndex = 32;
            this.chkServer.Text = "Run offline";
            this.chkServer.UseVisualStyleBackColor = true;
            this.chkServer.CheckedChanged += new System.EventHandler(this.chkServer_CheckedChanged);
            // 
            // chkUseRam
            // 
            this.chkUseRam.AutoSize = true;
            this.chkUseRam.Location = new System.Drawing.Point(24, 56);
            this.chkUseRam.Name = "chkUseRam";
            this.chkUseRam.Size = new System.Drawing.Size(50, 17);
            this.chkUseRam.TabIndex = 35;
            this.chkUseRam.Text = "RAM";
            this.chkUseRam.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.optCustom);
            this.groupBox2.Location = new System.Drawing.Point(337, 225);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(250, 128);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Caching extents";
            // 
            // btnBrowseCache
            // 
            this.btnBrowseCache.Location = new System.Drawing.Point(55, 82);
            this.btnBrowseCache.Name = "btnBrowseCache";
            this.btnBrowseCache.Size = new System.Drawing.Size(80, 21);
            this.btnBrowseCache.TabIndex = 36;
            this.btnBrowseCache.Text = "Browse...";
            this.btnBrowseCache.UseVisualStyleBackColor = true;
            this.btnBrowseCache.Click += new System.EventHandler(this.btnBrowseCache_Click);
            // 
            // chkExtents
            // 
            this.chkExtents.AutoSize = true;
            this.chkExtents.Location = new System.Drawing.Point(337, 369);
            this.chkExtents.Name = "chkExtents";
            this.chkExtents.Size = new System.Drawing.Size(120, 17);
            this.chkExtents.TabIndex = 36;
            this.chkExtents.Text = "Extents for caching:";
            this.chkExtents.UseVisualStyleBackColor = true;
            // 
            // optAll
            // 
            this.optAll.AutoSize = true;
            this.optAll.Checked = true;
            this.optAll.Location = new System.Drawing.Point(24, 120);
            this.optAll.Name = "optAll";
            this.optAll.Size = new System.Drawing.Size(57, 17);
            this.optAll.TabIndex = 37;
            this.optAll.TabStop = true;
            this.optAll.Text = "All tiles";
            this.optAll.UseVisualStyleBackColor = true;
            // 
            // optExtents
            // 
            this.optExtents.AutoSize = true;
            this.optExtents.Location = new System.Drawing.Point(24, 143);
            this.optExtents.Name = "optExtents";
            this.optExtents.Size = new System.Drawing.Size(160, 17);
            this.optExtents.TabIndex = 38;
            this.optExtents.Text = "Tiles in the following extents:";
            this.optExtents.UseVisualStyleBackColor = true;
            // 
            // btnFillMemory
            // 
            this.btnFillMemory.Location = new System.Drawing.Point(55, 267);
            this.btnFillMemory.Name = "btnFillMemory";
            this.btnFillMemory.Size = new System.Drawing.Size(172, 21);
            this.btnFillMemory.TabIndex = 39;
            this.btnFillMemory.Text = "Fill memory from database";
            this.btnFillMemory.UseVisualStyleBackColor = true;
            this.btnFillMemory.Click += new System.EventHandler(this.btnFillMemory_Click);
            // 
            // TilesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "TilesControl";
            this.Size = new System.Drawing.Size(296, 501);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelExtents.ResumeLayout(false);
            this.panelExtents.PerformLayout();
            this.panelBounds.ResumeLayout(false);
            this.panelBounds.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLat;
        private System.Windows.Forms.TextBox txtLong;
        private System.Windows.Forms.TextBox txtFind;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboProvider;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox panelExtents;
        private System.Windows.Forms.CheckBox chkUseDatabase;
        private System.Windows.Forms.TextBox txtMaxLon;
        private System.Windows.Forms.TextBox txtMinLon;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtMaxLat;
        private System.Windows.Forms.TextBox txtMinLat;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton optCustom;
        private System.Windows.Forms.RadioButton optLayers;
        private System.Windows.Forms.ComboBox cboLayers;
        private System.Windows.Forms.Button btnChooseExtents;
        private System.Windows.Forms.Button btnDoCaching;
        private System.Windows.Forms.Button btnTilesSettings;
        private System.Windows.Forms.Label lblDatabaseCache;
        private System.Windows.Forms.Label lblRamCache;
        private System.Windows.Forms.Panel panelBounds;
        private System.Windows.Forms.CheckBox chkServer;
        private System.Windows.Forms.CheckBox chkUseRam;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnBrowseCache;
        private System.Windows.Forms.CheckBox chkExtents;
        private System.Windows.Forms.RadioButton optExtents;
        private System.Windows.Forms.RadioButton optAll;
        private System.Windows.Forms.Button btnFillMemory;
    }
}