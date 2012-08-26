using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DGP_Messenger
{
	/// <summary>
	/// Summary description for LogginDialog.
	/// </summary>
	public class LogginDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox msnNameField;
		private System.Windows.Forms.ComboBox msnDrpLoggin;
		private System.Windows.Forms.Label NameLabel2;
		private System.Windows.Forms.Label PasswordLabel2;
		private System.Windows.Forms.TextBox msnPasswordField;
		private System.Windows.Forms.TextBox dgpNameField;
		private System.Windows.Forms.ComboBox dgpDrpLoggin;
		private System.Windows.Forms.Label NameLabel1;
		private System.Windows.Forms.Label PasswordLabel1;
		private System.Windows.Forms.TextBox dgpPasswordField;
		private System.Windows.Forms.CheckBox msnChbox;
		private System.Windows.Forms.CheckBox dgpChbox;
		private System.Windows.Forms.PictureBox LogginButton;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.CheckBox chbChat;
		private System.Windows.Forms.TextBox tbChatServer;
		private System.Windows.Forms.TextBox tbChatName;
		private System.Windows.Forms.Label lbChatFtp;
		private System.Windows.Forms.Label lbChatName;
		private System.Windows.Forms.ComboBox drpChat;
		private System.ComponentModel.IContainer components;

		public LogginDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();


			//this.NameField.Text = "andreas.sehr@dgp.se";
			this.dgpDrpLoggin.Items.Add("okand@dgp.se");
			this.dgpDrpLoggin.Items.Add("andreas.sehr@dgp.se");
			this.dgpDrpLoggin.Items.Add("david@dgp.se");

			UpdateEnabled();
		}

		public void UpdateEnabled()
		{
			if(dgpChbox.Checked == true)
			{
				this.dgpDrpLoggin.Enabled = true;
				this.dgpNameField.Enabled = true;
				this.dgpPasswordField.Enabled = true;
			}
			else
			{
				this.dgpDrpLoggin.Enabled = false;
				this.dgpNameField.Enabled = false;
				this.dgpPasswordField.Enabled = false;
			}

			if(msnChbox.Checked == true)
			{
				this.msnDrpLoggin.Enabled = true;
				this.msnNameField.Enabled = true;
				this.msnPasswordField.Enabled = true;
			}
			else
			{
				this.msnDrpLoggin.Enabled = false;
				this.msnNameField.Enabled = false;
				this.msnPasswordField.Enabled = false;
			}

			if(chbChat.Checked == true)
			{
				this.tbChatName.Enabled = true;
				this.tbChatServer.Enabled = true;
				this.drpChat.Enabled = true;
			}
			else
			{
				this.tbChatName.Enabled = false;
				this.tbChatServer.Enabled = false;
				this.drpChat.Enabled = false;
			}
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(LogginDialog));
			this.msnNameField = new System.Windows.Forms.TextBox();
			this.msnDrpLoggin = new System.Windows.Forms.ComboBox();
			this.NameLabel2 = new System.Windows.Forms.Label();
			this.PasswordLabel2 = new System.Windows.Forms.Label();
			this.msnPasswordField = new System.Windows.Forms.TextBox();
			this.dgpNameField = new System.Windows.Forms.TextBox();
			this.dgpDrpLoggin = new System.Windows.Forms.ComboBox();
			this.NameLabel1 = new System.Windows.Forms.Label();
			this.PasswordLabel1 = new System.Windows.Forms.Label();
			this.dgpPasswordField = new System.Windows.Forms.TextBox();
			this.msnChbox = new System.Windows.Forms.CheckBox();
			this.dgpChbox = new System.Windows.Forms.CheckBox();
			this.LogginButton = new System.Windows.Forms.PictureBox();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.chbChat = new System.Windows.Forms.CheckBox();
			this.tbChatServer = new System.Windows.Forms.TextBox();
			this.tbChatName = new System.Windows.Forms.TextBox();
			this.drpChat = new System.Windows.Forms.ComboBox();
			this.lbChatFtp = new System.Windows.Forms.Label();
			this.lbChatName = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// msnNameField
			// 
			this.msnNameField.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.msnNameField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.msnNameField.Enabled = false;
			this.msnNameField.Location = new System.Drawing.Point(120, 232);
			this.msnNameField.Name = "msnNameField";
			this.msnNameField.Size = new System.Drawing.Size(160, 20);
			this.msnNameField.TabIndex = 27;
			this.msnNameField.Text = "dgpmessenger@hotmail.com";
			// 
			// msnDrpLoggin
			// 
			this.msnDrpLoggin.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.msnDrpLoggin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.msnDrpLoggin.Enabled = false;
			this.msnDrpLoggin.Location = new System.Drawing.Point(120, 232);
			this.msnDrpLoggin.Name = "msnDrpLoggin";
			this.msnDrpLoggin.Size = new System.Drawing.Size(178, 21);
			this.msnDrpLoggin.TabIndex = 31;
			// 
			// NameLabel2
			// 
			this.NameLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.NameLabel2.BackColor = System.Drawing.Color.Transparent;
			this.NameLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.NameLabel2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.NameLabel2.Location = new System.Drawing.Point(120, 216);
			this.NameLabel2.Name = "NameLabel2";
			this.NameLabel2.Size = new System.Drawing.Size(40, 16);
			this.NameLabel2.TabIndex = 29;
			this.NameLabel2.Text = "Namn:";
			// 
			// PasswordLabel2
			// 
			this.PasswordLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.PasswordLabel2.BackColor = System.Drawing.Color.Transparent;
			this.PasswordLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.PasswordLabel2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.PasswordLabel2.Location = new System.Drawing.Point(120, 256);
			this.PasswordLabel2.Name = "PasswordLabel2";
			this.PasswordLabel2.Size = new System.Drawing.Size(64, 16);
			this.PasswordLabel2.TabIndex = 30;
			this.PasswordLabel2.Text = "Lösenord:";
			// 
			// msnPasswordField
			// 
			this.msnPasswordField.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.msnPasswordField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.msnPasswordField.Enabled = false;
			this.msnPasswordField.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.msnPasswordField.Location = new System.Drawing.Point(120, 272);
			this.msnPasswordField.Name = "msnPasswordField";
			this.msnPasswordField.PasswordChar = '*';
			this.msnPasswordField.Size = new System.Drawing.Size(160, 20);
			this.msnPasswordField.TabIndex = 28;
			this.msnPasswordField.Text = "123456";
			// 
			// dgpNameField
			// 
			this.dgpNameField.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.dgpNameField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.dgpNameField.Location = new System.Drawing.Point(120, 136);
			this.dgpNameField.Name = "dgpNameField";
			this.dgpNameField.Size = new System.Drawing.Size(160, 20);
			this.dgpNameField.TabIndex = 32;
			this.dgpNameField.Text = "okand@dgp.se";
			// 
			// dgpDrpLoggin
			// 
			this.dgpDrpLoggin.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.dgpDrpLoggin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.dgpDrpLoggin.Location = new System.Drawing.Point(120, 136);
			this.dgpDrpLoggin.Name = "dgpDrpLoggin";
			this.dgpDrpLoggin.Size = new System.Drawing.Size(178, 21);
			this.dgpDrpLoggin.TabIndex = 36;
			// 
			// NameLabel1
			// 
			this.NameLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.NameLabel1.BackColor = System.Drawing.Color.Transparent;
			this.NameLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.NameLabel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.NameLabel1.Location = new System.Drawing.Point(120, 120);
			this.NameLabel1.Name = "NameLabel1";
			this.NameLabel1.Size = new System.Drawing.Size(40, 16);
			this.NameLabel1.TabIndex = 34;
			this.NameLabel1.Text = "Namn:";
			// 
			// PasswordLabel1
			// 
			this.PasswordLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.PasswordLabel1.BackColor = System.Drawing.Color.Transparent;
			this.PasswordLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.PasswordLabel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.PasswordLabel1.Location = new System.Drawing.Point(120, 160);
			this.PasswordLabel1.Name = "PasswordLabel1";
			this.PasswordLabel1.Size = new System.Drawing.Size(64, 16);
			this.PasswordLabel1.TabIndex = 35;
			this.PasswordLabel1.Text = "Lösenord:";
			// 
			// dgpPasswordField
			// 
			this.dgpPasswordField.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.dgpPasswordField.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.dgpPasswordField.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.dgpPasswordField.Location = new System.Drawing.Point(120, 176);
			this.dgpPasswordField.Name = "dgpPasswordField";
			this.dgpPasswordField.PasswordChar = '*';
			this.dgpPasswordField.Size = new System.Drawing.Size(160, 20);
			this.dgpPasswordField.TabIndex = 33;
			this.dgpPasswordField.Text = "12345";
			// 
			// msnChbox
			// 
			this.msnChbox.BackColor = System.Drawing.Color.Transparent;
			this.msnChbox.Location = new System.Drawing.Point(48, 248);
			this.msnChbox.Name = "msnChbox";
			this.msnChbox.Size = new System.Drawing.Size(56, 24);
			this.msnChbox.TabIndex = 37;
			this.msnChbox.Text = "MSN";
			this.msnChbox.CheckedChanged += new System.EventHandler(this.msnChbox_CheckedChanged);
			// 
			// dgpChbox
			// 
			this.dgpChbox.BackColor = System.Drawing.Color.Transparent;
			this.dgpChbox.Location = new System.Drawing.Point(48, 152);
			this.dgpChbox.Name = "dgpChbox";
			this.dgpChbox.Size = new System.Drawing.Size(56, 24);
			this.dgpChbox.TabIndex = 38;
			this.dgpChbox.Text = "DGP";
			this.dgpChbox.CheckedChanged += new System.EventHandler(this.dgpChbox_CheckedChanged);
			// 
			// LogginButton
			// 
			this.LogginButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.LogginButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.LogginButton.Image = ((System.Drawing.Image)(resources.GetObject("LogginButton.Image")));
			this.LogginButton.Location = new System.Drawing.Point(128, 320);
			this.LogginButton.Name = "LogginButton";
			this.LogginButton.Size = new System.Drawing.Size(110, 45);
			this.LogginButton.TabIndex = 39;
			this.LogginButton.TabStop = false;
			this.LogginButton.Click += new System.EventHandler(this.LogginButton_Click);
			this.LogginButton.MouseEnter += new System.EventHandler(this.LogginButton_MouseEnter);
			this.LogginButton.MouseLeave += new System.EventHandler(this.LogginButton_MouseLeave);
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(110, 45);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// chbChat
			// 
			this.chbChat.BackColor = System.Drawing.Color.Transparent;
			this.chbChat.Location = new System.Drawing.Point(48, 56);
			this.chbChat.Name = "chbChat";
			this.chbChat.Size = new System.Drawing.Size(56, 24);
			this.chbChat.TabIndex = 45;
			this.chbChat.Text = "Chat";
			this.chbChat.CheckedChanged += new System.EventHandler(this.chbChat_CheckedChanged);
			// 
			// tbChatServer
			// 
			this.tbChatServer.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.tbChatServer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tbChatServer.Location = new System.Drawing.Point(120, 40);
			this.tbChatServer.Name = "tbChatServer";
			this.tbChatServer.Size = new System.Drawing.Size(160, 20);
			this.tbChatServer.TabIndex = 40;
			this.tbChatServer.Text = "armada";
			// 
			// tbChatName
			// 
			this.tbChatName.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.tbChatName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tbChatName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.tbChatName.Location = new System.Drawing.Point(120, 80);
			this.tbChatName.Name = "tbChatName";
			this.tbChatName.Size = new System.Drawing.Size(160, 20);
			this.tbChatName.TabIndex = 41;
			this.tbChatName.Text = "SiTi";
			// 
			// drpChat
			// 
			this.drpChat.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.drpChat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.drpChat.Location = new System.Drawing.Point(120, 40);
			this.drpChat.Name = "drpChat";
			this.drpChat.Size = new System.Drawing.Size(178, 21);
			this.drpChat.TabIndex = 44;
			// 
			// lbChatFtp
			// 
			this.lbChatFtp.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lbChatFtp.BackColor = System.Drawing.Color.Transparent;
			this.lbChatFtp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbChatFtp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lbChatFtp.Location = new System.Drawing.Point(120, 24);
			this.lbChatFtp.Name = "lbChatFtp";
			this.lbChatFtp.Size = new System.Drawing.Size(40, 16);
			this.lbChatFtp.TabIndex = 42;
			this.lbChatFtp.Text = "Server:";
			// 
			// lbChatName
			// 
			this.lbChatName.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lbChatName.BackColor = System.Drawing.Color.Transparent;
			this.lbChatName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbChatName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lbChatName.Location = new System.Drawing.Point(120, 64);
			this.lbChatName.Name = "lbChatName";
			this.lbChatName.Size = new System.Drawing.Size(64, 16);
			this.lbChatName.TabIndex = 43;
			this.lbChatName.Text = "Namn:";
			// 
			// LogginDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(338, 391);
			this.Controls.Add(this.chbChat);
			this.Controls.Add(this.tbChatServer);
			this.Controls.Add(this.tbChatName);
			this.Controls.Add(this.dgpNameField);
			this.Controls.Add(this.dgpPasswordField);
			this.Controls.Add(this.msnNameField);
			this.Controls.Add(this.msnPasswordField);
			this.Controls.Add(this.drpChat);
			this.Controls.Add(this.lbChatFtp);
			this.Controls.Add(this.lbChatName);
			this.Controls.Add(this.LogginButton);
			this.Controls.Add(this.dgpChbox);
			this.Controls.Add(this.msnChbox);
			this.Controls.Add(this.dgpDrpLoggin);
			this.Controls.Add(this.NameLabel1);
			this.Controls.Add(this.PasswordLabel1);
			this.Controls.Add(this.msnDrpLoggin);
			this.Controls.Add(this.NameLabel2);
			this.Controls.Add(this.PasswordLabel2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LogginDialog";
			this.ShowInTaskbar = false;
			this.Text = "Loggin";
			this.Load += new System.EventHandler(this.LogginDialog_Load);
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties

		public bool MsnLoggin
		{
			get { return this.msnChbox.Checked; }
		}
		public string MsnName
		{
			get { return string.Copy(this.msnNameField.Text); }
		}
		public string MsnPassword
		{
			get { return string.Copy(this.msnPasswordField.Text); }
		}

		public bool DgpLoggin
		{
			get { return this.dgpChbox.Checked; }
		}

		public string DgpName
		{
			get { return string.Copy(this.dgpNameField.Text); }
		}
		public string DgpPassword
		{
			get { return string.Copy(this.dgpPasswordField.Text); }
		}

		public bool ChatLoggin
		{
			get { return this.chbChat.Checked; }
		}

		public string ChatServer
		{
			get { return string.Copy(this.tbChatServer.Text); }
		}
		public string ChatName
		{
			get { return string.Copy(this.tbChatName.Text); }
		}

		#endregion

		private void dgpChbox_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateEnabled();
		}

		private void msnChbox_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateEnabled();
		}

		private void chbChat_CheckedChanged(object sender, System.EventArgs e)
		{
			UpdateEnabled();
		}

		private void drpLoggin_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			dgpNameField.Text = dgpDrpLoggin.SelectedItem.ToString();
			if(dgpDrpLoggin.SelectedIndex == 0)
			{
				dgpPasswordField.Text = "12345";
			}
			else
			{
				dgpPasswordField.Text = "";
			}
		}

		private void LogginButton_MouseEnter(object sender, System.EventArgs e)
		{
			LogginButton.Image = imageList1.Images[0];
			LogginButton.Refresh();
		}

		private void LogginButton_MouseLeave(object sender, System.EventArgs e)
		{
			LogginButton.Image = imageList1.Images[1];
			LogginButton.Refresh();
		}

		private void LogginButton_Click(object sender, System.EventArgs e)
		{
			bool closeOk = false;
			if(this.DgpLoggin )
			{
				if(this.DgpName != "" && this.DgpPassword != "")
				{
					closeOk = true;
				}
			}
			if(this.MsnLoggin )
			{
				if(this.MsnName != "" && this.MsnPassword != "")
				{
					closeOk = true;
				}
			}
			if(this.ChatLoggin )
			{
				if(this.ChatName != "" && this.ChatServer != "")
				{
					closeOk = true;
				}
			}

			if(closeOk)
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
			else
			{
				MessageBox.Show(this, "Alla fält är inte korrekt ifyllda!", "Kunde Inte Logga In", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void LogginDialog_Load(object sender, System.EventArgs e)
		{
			//Autologgin
			//this.DialogResult = DialogResult.OK;
			//this.Close();
		}
	}
}
