using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DBLuncher.Classes
{
	/// <summary>
	/// Summary description for frmDBWizard.
	/// </summary>
	public class frmDBWizard : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox dbDriver;
		private System.Windows.Forms.TextBox serverNameOrIp;
		private System.Windows.Forms.TextBox dbPort;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.TextBox password;
		private System.Windows.Forms.TextBox userName;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ComboBox comboOptions;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmDBWizard()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.dbDriver = new System.Windows.Forms.TextBox();
			this.serverNameOrIp = new System.Windows.Forms.TextBox();
			this.dbPort = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.password = new System.Windows.Forms.TextBox();
			this.userName = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label6 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.comboOptions = new System.Windows.Forms.ComboBox();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// dbDriver
			// 
			this.dbDriver.Location = new System.Drawing.Point(184, 32);
			this.dbDriver.Name = "dbDriver";
			this.dbDriver.Size = new System.Drawing.Size(160, 20);
			this.dbDriver.TabIndex = 0;
			this.dbDriver.Text = "textBox1";
			// 
			// serverNameOrIp
			// 
			this.serverNameOrIp.Location = new System.Drawing.Point(184, 72);
			this.serverNameOrIp.Name = "serverNameOrIp";
			this.serverNameOrIp.Size = new System.Drawing.Size(160, 20);
			this.serverNameOrIp.TabIndex = 1;
			this.serverNameOrIp.Text = "textBox2";
			// 
			// dbPort
			// 
			this.dbPort.Location = new System.Drawing.Point(184, 112);
			this.dbPort.Name = "dbPort";
			this.dbPort.Size = new System.Drawing.Size(160, 20);
			this.dbPort.TabIndex = 2;
			this.dbPort.Text = "textBox3";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(40, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(104, 24);
			this.label1.TabIndex = 3;
			this.label1.Text = "Database Driver";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(40, 72);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(104, 24);
			this.label2.TabIndex = 4;
			this.label2.Text = "Server";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(40, 120);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 24);
			this.label3.TabIndex = 5;
			this.label3.Text = "Port";
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(16, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(368, 160);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Connection";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(32, 240);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(104, 24);
			this.label4.TabIndex = 10;
			this.label4.Text = "Password";
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(32, 200);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(104, 24);
			this.label14.TabIndex = 9;
			this.label14.Text = "User Name";
			// 
			// password
			// 
			this.password.Location = new System.Drawing.Point(176, 240);
			this.password.Name = "password";
			this.password.Size = new System.Drawing.Size(160, 20);
			this.password.TabIndex = 8;
			this.password.Text = "textBox4";
			// 
			// userName
			// 
			this.userName.Location = new System.Drawing.Point(176, 200);
			this.userName.Name = "userName";
			this.userName.Size = new System.Drawing.Size(160, 20);
			this.userName.TabIndex = 7;
			this.userName.Text = "textBox5";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.comboOptions);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Location = new System.Drawing.Point(16, 168);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(368, 144);
			this.groupBox2.TabIndex = 11;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Authentication";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 104);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(104, 24);
			this.label6.TabIndex = 12;
			this.label6.Text = "Options";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(40, 328);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(96, 24);
			this.button1.TabIndex = 12;
			this.button1.Text = "Connect";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(224, 328);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(96, 24);
			this.button2.TabIndex = 13;
			this.button2.Text = "Test";
			// 
			// comboOptions
			// 
			this.comboOptions.Location = new System.Drawing.Point(160, 104);
			this.comboOptions.Name = "comboOptions";
			this.comboOptions.Size = new System.Drawing.Size(160, 21);
			this.comboOptions.TabIndex = 13;
			this.comboOptions.Text = "comboBox1";
			// 
			// frmDBWizard
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(400, 366);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.password);
			this.Controls.Add(this.userName);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.dbPort);
			this.Controls.Add(this.serverNameOrIp);
			this.Controls.Add(this.dbDriver);
			this.Controls.Add(this.groupBox1);
			this.Name = "frmDBWizard";
			this.Text = "frmDBWizard";
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
