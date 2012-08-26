using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace DGP_Messenger_Server
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnStartServer;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox chkMsql;
		private System.Windows.Forms.TextBox tbServer;
		private System.Windows.Forms.TextBox tbDatabase;
		private System.Windows.Forms.TextBox tbUsername;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Label lbServer;
		private System.Windows.Forms.Label lbDatabase;
		private System.Windows.Forms.Label lbUsername;
		private System.Windows.Forms.Label lbPassword;
		private System.Windows.Forms.Label lbPort;
		private System.Windows.Forms.TextBox tbPort;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RichTextBox tbLog;
		private System.Windows.Forms.Button btnStopServer;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private SocketServer server;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			tbLog.Text += "_ _ _ _ _ _  _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _\n";
			tbLog.Text += "_-_-_-_-_-_-_-_-_-_-_-_-_ DGP Messenger Server _-_-_-_-_-_-_-_-_-_-_-_\n";
			tbLog.Text += "_-_-_-_-_-_-__-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_\n";
			tbLog.Text += "_-_-_-_-_-_-__-_-_- By: Sehr & the DGP Team Sweden _-_-_-_-_-_-_-_-_\n";
			tbLog.Text += "_-_-_-_-_-_-__-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_\n\n";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.btnStartServer = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lbPassword = new System.Windows.Forms.Label();
			this.lbUsername = new System.Windows.Forms.Label();
			this.lbDatabase = new System.Windows.Forms.Label();
			this.lbServer = new System.Windows.Forms.Label();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.tbUsername = new System.Windows.Forms.TextBox();
			this.tbDatabase = new System.Windows.Forms.TextBox();
			this.tbServer = new System.Windows.Forms.TextBox();
			this.chkMsql = new System.Windows.Forms.CheckBox();
			this.lbPort = new System.Windows.Forms.Label();
			this.tbPort = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tbLog = new System.Windows.Forms.RichTextBox();
			this.btnStopServer = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnStartServer
			// 
			this.btnStartServer.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(128)), ((System.Byte)(255)), ((System.Byte)(128)));
			this.btnStartServer.Location = new System.Drawing.Point(56, 280);
			this.btnStartServer.Name = "btnStartServer";
			this.btnStartServer.Size = new System.Drawing.Size(88, 23);
			this.btnStartServer.TabIndex = 1;
			this.btnStartServer.Text = "START Server";
			this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lbPassword);
			this.groupBox1.Controls.Add(this.lbUsername);
			this.groupBox1.Controls.Add(this.lbDatabase);
			this.groupBox1.Controls.Add(this.lbServer);
			this.groupBox1.Controls.Add(this.tbPassword);
			this.groupBox1.Controls.Add(this.tbUsername);
			this.groupBox1.Controls.Add(this.tbDatabase);
			this.groupBox1.Controls.Add(this.tbServer);
			this.groupBox1.Enabled = false;
			this.groupBox1.Location = new System.Drawing.Point(24, 136);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(280, 120);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "MySQL Options";
			// 
			// lbPassword
			// 
			this.lbPassword.Location = new System.Drawing.Point(8, 88);
			this.lbPassword.Name = "lbPassword";
			this.lbPassword.Size = new System.Drawing.Size(88, 23);
			this.lbPassword.TabIndex = 8;
			this.lbPassword.Text = "Password:";
			this.lbPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbUsername
			// 
			this.lbUsername.Location = new System.Drawing.Point(8, 64);
			this.lbUsername.Name = "lbUsername";
			this.lbUsername.Size = new System.Drawing.Size(88, 23);
			this.lbUsername.TabIndex = 7;
			this.lbUsername.Text = "Username:";
			this.lbUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbDatabase
			// 
			this.lbDatabase.Location = new System.Drawing.Point(8, 40);
			this.lbDatabase.Name = "lbDatabase";
			this.lbDatabase.Size = new System.Drawing.Size(88, 23);
			this.lbDatabase.TabIndex = 6;
			this.lbDatabase.Text = "Database:";
			this.lbDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbServer
			// 
			this.lbServer.Location = new System.Drawing.Point(8, 16);
			this.lbServer.Name = "lbServer";
			this.lbServer.Size = new System.Drawing.Size(88, 23);
			this.lbServer.TabIndex = 5;
			this.lbServer.Text = "Server:";
			this.lbServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tbPassword
			// 
			this.tbPassword.Location = new System.Drawing.Point(96, 88);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.PasswordChar = '*';
			this.tbPassword.Size = new System.Drawing.Size(144, 20);
			this.tbPassword.TabIndex = 4;
			this.tbPassword.Text = "uiop1122";
			// 
			// tbUsername
			// 
			this.tbUsername.Location = new System.Drawing.Point(96, 64);
			this.tbUsername.Name = "tbUsername";
			this.tbUsername.Size = new System.Drawing.Size(144, 20);
			this.tbUsername.TabIndex = 3;
			this.tbUsername.Text = "access@d1014";
			// 
			// tbDatabase
			// 
			this.tbDatabase.Location = new System.Drawing.Point(96, 40);
			this.tbDatabase.Name = "tbDatabase";
			this.tbDatabase.Size = new System.Drawing.Size(144, 20);
			this.tbDatabase.TabIndex = 2;
			this.tbDatabase.Text = "dgp_se";
			// 
			// tbServer
			// 
			this.tbServer.Location = new System.Drawing.Point(96, 16);
			this.tbServer.Name = "tbServer";
			this.tbServer.Size = new System.Drawing.Size(144, 20);
			this.tbServer.TabIndex = 1;
			this.tbServer.Text = "mysql13.dgp.se";
			// 
			// chkMsql
			// 
			this.chkMsql.Location = new System.Drawing.Point(32, 104);
			this.chkMsql.Name = "chkMsql";
			this.chkMsql.Size = new System.Drawing.Size(176, 24);
			this.chkMsql.TabIndex = 0;
			this.chkMsql.Text = "Run with MySQL sever";
			this.chkMsql.CheckedChanged += new System.EventHandler(this.chkMsql_CheckedChanged);
			// 
			// lbPort
			// 
			this.lbPort.Location = new System.Drawing.Point(32, 72);
			this.lbPort.Name = "lbPort";
			this.lbPort.Size = new System.Drawing.Size(80, 23);
			this.lbPort.TabIndex = 3;
			this.lbPort.Text = "Port:";
			this.lbPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tbPort
			// 
			this.tbPort.Location = new System.Drawing.Point(120, 72);
			this.tbPort.MaxLength = 5;
			this.tbPort.Name = "tbPort";
			this.tbPort.Size = new System.Drawing.Size(144, 20);
			this.tbPort.TabIndex = 4;
			this.tbPort.Text = "1080";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Georgia", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Black;
			this.label1.Location = new System.Drawing.Point(32, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(280, 32);
			this.label1.TabIndex = 5;
			this.label1.Text = "DGP Server";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tbLog
			// 
			this.tbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tbLog.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.tbLog.Location = new System.Drawing.Point(328, 8);
			this.tbLog.Name = "tbLog";
			this.tbLog.ReadOnly = true;
			this.tbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.tbLog.Size = new System.Drawing.Size(344, 320);
			this.tbLog.TabIndex = 6;
			this.tbLog.Text = "";
			// 
			// btnStopServer
			// 
			this.btnStopServer.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(128)), ((System.Byte)(128)));
			this.btnStopServer.Enabled = false;
			this.btnStopServer.Location = new System.Drawing.Point(176, 280);
			this.btnStopServer.Name = "btnStopServer";
			this.btnStopServer.Size = new System.Drawing.Size(88, 23);
			this.btnStopServer.TabIndex = 7;
			this.btnStopServer.Text = "STOP Server";
			this.btnStopServer.Click += new System.EventHandler(this.btnStopServer_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(246)), ((System.Byte)(246)), ((System.Byte)(247)));
			this.ClientSize = new System.Drawing.Size(680, 334);
			this.Controls.Add(this.btnStopServer);
			this.Controls.Add(this.tbLog);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbPort);
			this.Controls.Add(this.lbPort);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnStartServer);
			this.Controls.Add(this.chkMsql);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "DGP Server";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void chkMsql_CheckedChanged(object sender, System.EventArgs e)
		{
			this.groupBox1.Enabled = chkMsql.Checked;
		}

		private void btnStartServer_Click(object sender, System.EventArgs e)
		{
			if(chkMsql.Checked)
			{
				server = new DgpServer(int.Parse(this.tbPort.Text));
				tbLog.Text += "Start server with MySQL DB Under construktion\n";
			}
			else
			{
				server = new ChatServer(int.Parse(this.tbPort.Text));
				tbLog.Text += "Start server whitout MySQL DB.\n";
			}
			
			server.LoggEvent += new LoggEventHandler(server_LoggEvent);
			if(SetServerStatus(true) == 1)
			{
				tbLog.Text += "Listening for clients to connect on ip: " + server.socket.LocalEndPoint.ToString() + "\n";
			}
			else
			{
				tbLog.Text += "Failed to set upp Socket. \n";
				SetServerStatus(false);
			}
		}

		private void btnStopServer_Click(object sender, System.EventArgs e)
		{
			if(MessageBox.Show(this, "Are you sure you want to stop the server?", "Stop server?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				SetServerStatus(false);
			}
		}

		private int SetServerStatus(bool start)
		{
			if(start)
			{
				btnStartServer.Enabled	= false;
				btnStopServer.Enabled	= true;
				tbPort.Enabled			= false;
				chkMsql.Enabled			= false;
				return server.Start();
			}
			else
			{
				btnStartServer.Enabled	= true;
				btnStopServer.Enabled	= false;
				tbPort.Enabled			= true;
				chkMsql.Enabled			= true;
				tbLog.Text += "Server Stopped ! ! ! \n\n";
				return server.Stop();
			}
		}

		private void server_LoggEvent(object sender, LoggEventArgs e)
		{
			tbLog.Text += e.Message + "\n";
		}
	}
}
