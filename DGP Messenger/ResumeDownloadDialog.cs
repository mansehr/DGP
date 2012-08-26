using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DGP_Messenger
{
	/// <summary>
	/// Summary description for ResumeDownloadDialog.
	/// </summary>
	public class ResumeDownloadDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnAbort;
		private System.Windows.Forms.Button btnRename;
		private System.Windows.Forms.Button btnResume;
		private System.Windows.Forms.PictureBox pictureBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ResumeDownloadDialog(string fileName,  bool canContinue)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			label1.Text = "Filen " + fileName + " finns redan!"; 
			if(canContinue)
			{
				label3.Text = "Eller vill du försöka fortsätta ladda ner den?"; 
			}
			else
			{
				label3.Visible = false;
				btnResume.Enabled = false;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ResumeDownloadDialog));
			this.btnAbort = new System.Windows.Forms.Button();
			this.btnRename = new System.Windows.Forms.Button();
			this.btnResume = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// btnAbort
			// 
			this.btnAbort.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.btnAbort.Location = new System.Drawing.Point(280, 120);
			this.btnAbort.Name = "btnAbort";
			this.btnAbort.TabIndex = 1;
			this.btnAbort.Text = "&Skriv Över";
			// 
			// btnRename
			// 
			this.btnRename.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnRename.Location = new System.Drawing.Point(96, 120);
			this.btnRename.Name = "btnRename";
			this.btnRename.TabIndex = 0;
			this.btnRename.Text = "Byt &Namn";
			// 
			// btnResume
			// 
			this.btnResume.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnResume.Location = new System.Drawing.Point(184, 120);
			this.btnResume.Name = "btnResume";
			this.btnResume.Size = new System.Drawing.Size(80, 24);
			this.btnResume.TabIndex = 2;
			this.btnResume.Text = "&Fortsätt";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(96, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(256, 24);
			this.label3.TabIndex = 3;
			this.label3.Text = "Eller vill du försöka fortsätta ladda ner filen?";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(96, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(256, 24);
			this.label2.TabIndex = 4;
			this.label2.Text = "Vill du byta namn eller skriva över filen?";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(96, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(256, 32);
			this.label1.TabIndex = 5;
			this.label1.Text = "Filen + file + finns redan!";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 32);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(80, 72);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 6;
			this.pictureBox1.TabStop = false;
			// 
			// ResumeDownloadDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(362, 151);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnResume);
			this.Controls.Add(this.btnRename);
			this.Controls.Add(this.btnAbort);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "ResumeDownloadDialog";
			this.Text = "Fortsätt Nerladdning?";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
