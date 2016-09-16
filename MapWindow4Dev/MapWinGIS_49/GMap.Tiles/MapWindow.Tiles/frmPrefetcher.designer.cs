namespace GMap.NET
{
    partial class frmPrefetcher
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
         if(disposing && (components != null))
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
          this.progressBar1 = new System.Windows.Forms.ProgressBar();
          this.label1 = new System.Windows.Forms.Label();
          this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
          this.listView1 = new System.Windows.Forms.ListView();
          this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
          this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
          this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
          this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
          this.Ok = new System.Windows.Forms.Button();
          this.btnCancel = new System.Windows.Forms.Button();
          this.label2 = new System.Windows.Forms.Label();
          this.cboProvider = new System.Windows.Forms.ComboBox();
          this.label3 = new System.Windows.Forms.Label();
          this.txtExtents = new System.Windows.Forms.TextBox();
          this.groupBox1 = new System.Windows.Forms.GroupBox();
          this.chkSelectAll = new System.Windows.Forms.CheckBox();
          this.treeView1 = new System.Windows.Forms.TreeView();
          this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
          this.tableLayoutPanel1.SuspendLayout();
          this.groupBox1.SuspendLayout();
          this.SuspendLayout();
          // 
          // progressBar1
          // 
          this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
          this.progressBar1.Location = new System.Drawing.Point(3, 16);
          this.progressBar1.Name = "progressBar1";
          this.progressBar1.Size = new System.Drawing.Size(305, 11);
          this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
          this.progressBar1.TabIndex = 0;
          // 
          // label1
          // 
          this.label1.AutoSize = true;
          this.label1.Location = new System.Drawing.Point(3, 0);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(48, 13);
          this.label1.TabIndex = 1;
          this.label1.Text = "Progress";
          // 
          // tableLayoutPanel1
          // 
          this.tableLayoutPanel1.ColumnCount = 1;
          this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
          this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
          this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 1);
          this.tableLayoutPanel1.Location = new System.Drawing.Point(581, 7);
          this.tableLayoutPanel1.Name = "tableLayoutPanel1";
          this.tableLayoutPanel1.RowCount = 2;
          this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
          this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
          this.tableLayoutPanel1.Size = new System.Drawing.Size(311, 30);
          this.tableLayoutPanel1.TabIndex = 2;
          // 
          // listView1
          // 
          this.listView1.CheckBoxes = true;
          this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader2});
          this.listView1.FullRowSelect = true;
          this.listView1.GridLines = true;
          this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
          this.listView1.HideSelection = false;
          this.listView1.Location = new System.Drawing.Point(228, 50);
          this.listView1.Name = "listView1";
          this.listView1.Size = new System.Drawing.Size(319, 251);
          this.listView1.TabIndex = 3;
          this.listView1.UseCompatibleStateImageBehavior = false;
          this.listView1.View = System.Windows.Forms.View.Details;
          // 
          // columnHeader1
          // 
          this.columnHeader1.Text = "";
          this.columnHeader1.Width = 30;
          // 
          // columnHeader5
          // 
          this.columnHeader5.Text = "Scale";
          this.columnHeader5.Width = 50;
          // 
          // columnHeader6
          // 
          this.columnHeader6.Text = "Number of tiles";
          this.columnHeader6.Width = 90;
          // 
          // columnHeader7
          // 
          this.columnHeader7.Text = "Appr. size, MB";
          this.columnHeader7.Width = 80;
          // 
          // Ok
          // 
          this.Ok.Location = new System.Drawing.Point(364, 307);
          this.Ok.Name = "Ok";
          this.Ok.Size = new System.Drawing.Size(88, 28);
          this.Ok.TabIndex = 4;
          this.Ok.Text = "Start";
          this.Ok.UseVisualStyleBackColor = true;
          this.Ok.Click += new System.EventHandler(this.Ok_Click);
          // 
          // btnCancel
          // 
          this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
          this.btnCancel.Location = new System.Drawing.Point(458, 307);
          this.btnCancel.Name = "btnCancel";
          this.btnCancel.Size = new System.Drawing.Size(88, 28);
          this.btnCancel.TabIndex = 5;
          this.btnCancel.Text = "Close";
          this.btnCancel.UseVisualStyleBackColor = true;
          // 
          // label2
          // 
          this.label2.AutoSize = true;
          this.label2.Location = new System.Drawing.Point(18, 30);
          this.label2.Name = "label2";
          this.label2.Size = new System.Drawing.Size(46, 13);
          this.label2.TabIndex = 6;
          this.label2.Text = "Provider";
          // 
          // cboProvider
          // 
          this.cboProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
          this.cboProvider.FormattingEnabled = true;
          this.cboProvider.Location = new System.Drawing.Point(70, 27);
          this.cboProvider.Name = "cboProvider";
          this.cboProvider.Size = new System.Drawing.Size(293, 21);
          this.cboProvider.TabIndex = 7;
          // 
          // label3
          // 
          this.label3.AutoSize = true;
          this.label3.Location = new System.Drawing.Point(225, 7);
          this.label3.Name = "label3";
          this.label3.Size = new System.Drawing.Size(42, 13);
          this.label3.TabIndex = 8;
          this.label3.Text = "Extents";
          // 
          // txtExtents
          // 
          this.txtExtents.Location = new System.Drawing.Point(228, 23);
          this.txtExtents.Name = "txtExtents";
          this.txtExtents.ReadOnly = true;
          this.txtExtents.Size = new System.Drawing.Size(318, 20);
          this.txtExtents.TabIndex = 9;
          // 
          // groupBox1
          // 
          this.groupBox1.Controls.Add(this.label2);
          this.groupBox1.Controls.Add(this.cboProvider);
          this.groupBox1.Location = new System.Drawing.Point(570, 84);
          this.groupBox1.Name = "groupBox1";
          this.groupBox1.Size = new System.Drawing.Size(378, 110);
          this.groupBox1.TabIndex = 10;
          this.groupBox1.TabStop = false;
          this.groupBox1.Text = "Caching";
          // 
          // chkSelectAll
          // 
          this.chkSelectAll.AutoSize = true;
          this.chkSelectAll.Location = new System.Drawing.Point(24, 307);
          this.chkSelectAll.Name = "chkSelectAll";
          this.chkSelectAll.Size = new System.Drawing.Size(98, 17);
          this.chkSelectAll.TabIndex = 11;
          this.chkSelectAll.Text = "Select all/none";
          this.chkSelectAll.UseVisualStyleBackColor = true;
          this.chkSelectAll.CheckedChanged += new System.EventHandler(this.chkSelectAll_CheckedChanged);
          // 
          // treeView1
          // 
          this.treeView1.HideSelection = false;
          this.treeView1.Location = new System.Drawing.Point(11, 7);
          this.treeView1.Name = "treeView1";
          this.treeView1.Size = new System.Drawing.Size(208, 293);
          this.treeView1.TabIndex = 12;
          this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
          // 
          // columnHeader2
          // 
          this.columnHeader2.Text = "Status";
          // 
          // frmPrefetcher
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.BackColor = System.Drawing.SystemColors.Control;
          this.ClientSize = new System.Drawing.Size(554, 342);
          this.Controls.Add(this.treeView1);
          this.Controls.Add(this.txtExtents);
          this.Controls.Add(this.chkSelectAll);
          this.Controls.Add(this.label3);
          this.Controls.Add(this.groupBox1);
          this.Controls.Add(this.btnCancel);
          this.Controls.Add(this.tableLayoutPanel1);
          this.Controls.Add(this.Ok);
          this.Controls.Add(this.listView1);
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
          this.KeyPreview = true;
          this.MaximizeBox = false;
          this.MinimizeBox = false;
          this.Name = "frmPrefetcher";
          this.Padding = new System.Windows.Forms.Padding(4);
          this.ShowIcon = false;
          this.ShowInTaskbar = false;
          this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
          this.Text = "Tiles prefetcher";
          this.tableLayoutPanel1.ResumeLayout(false);
          this.tableLayoutPanel1.PerformLayout();
          this.groupBox1.ResumeLayout(false);
          this.groupBox1.PerformLayout();
          this.ResumeLayout(false);
          this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ProgressBar progressBar1;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.ListView listView1;
      private System.Windows.Forms.ColumnHeader columnHeader1;
      private System.Windows.Forms.Button Ok;
      private System.Windows.Forms.Button btnCancel;
      private System.Windows.Forms.ColumnHeader columnHeader5;
      private System.Windows.Forms.ColumnHeader columnHeader6;
      private System.Windows.Forms.ColumnHeader columnHeader7;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.ComboBox cboProvider;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox txtExtents;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.CheckBox chkSelectAll;
      private System.Windows.Forms.TreeView treeView1;
      private System.Windows.Forms.ColumnHeader columnHeader2;
   }
}