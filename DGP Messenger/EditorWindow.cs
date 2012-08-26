using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace DGP_Messenger
{
	/// <summary>
	/// Summary description for EditorWindow.
	/// </summary>
	public class EditorWindow : System.Windows.Forms.Form
	{
		private Color textColor = Color.Black;

		//private int ticks = 0; 
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.Button open;
		private System.Windows.Forms.Button save;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.StatusBar statusBar;
		private System.ComponentModel.IContainer components;

		public EditorWindow()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(EditorWindow));
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.open = new System.Windows.Forms.Button();
			this.save = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.button1 = new System.Windows.Forms.Button();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.SuspendLayout();
			// 
			// richTextBox1
			// 
			this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.richTextBox1.Location = new System.Drawing.Point(8, 48);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(448, 368);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.Text = "";
			this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
			// 
			// open
			// 
			this.open.Location = new System.Drawing.Point(8, 16);
			this.open.Name = "open";
			this.open.TabIndex = 1;
			this.open.Text = "Öppna";
			this.open.Click += new System.EventHandler(this.open_Click);
			// 
			// save
			// 
			this.save.Location = new System.Drawing.Point(96, 16);
			this.save.Name = "save";
			this.save.TabIndex = 2;
			this.save.Text = "Spara";
			this.save.Click += new System.EventHandler(this.save_Click);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Title = "Öpnna...";
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.Title = "Spara...";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(184, 16);
			this.button1.Name = "button1";
			this.button1.TabIndex = 3;
			this.button1.Text = "Röd!";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(95, 72);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 431);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(464, 22);
			this.statusBar.TabIndex = 4;
			this.statusBar.Text = "Coolo";
			// 
			// EditorWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.AliceBlue;
			this.ClientSize = new System.Drawing.Size(464, 453);
			this.Controls.Add(this.statusBar);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.save);
			this.Controls.Add(this.open);
			this.Controls.Add(this.richTextBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "EditorWindow";
			this.Text = "DGP Editor";
			this.ResumeLayout(false);

		}
		#endregion

		private void open_Click(object sender, System.EventArgs e)
		{	
			if(openFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					if(openFileDialog.FileName.Substring(openFileDialog.FileName.Length-3).ToLower() == "rtf")
					{
						richTextBox1.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.RichText);
					}
					else
					{
						richTextBox1.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.PlainText);
					}
				}
				catch(IOException ex)
				{
					MessageBox.Show("Fel när programmet försökte läsa från filen!\n"+ex.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}
				catch(Exception ex)
				{
					MessageBox.Show("Fel när programmet försökte läsa från filen!\n"+ex.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}
			}
			ColorCode();
		}

		private void save_Click(object sender, System.EventArgs e)
		{
			if(saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				try
				{
					if(saveFileDialog.FileName.Substring(saveFileDialog.FileName.Length-3).ToLower() == "rtf")
					{
						richTextBox1.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.RichText);
					}
					else
					{
						richTextBox1.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
					}
				}
				catch(IOException ex)
				{
					MessageBox.Show("Fel när programmet försökte skriva till filen!\n"+ex.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}
				catch(Exception ex)
				{
					MessageBox.Show("Fel när programmet försökte skriva till filen!\n"+ex.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				}
			}
		}

		private void richTextBox1_TextChanged(object sender, System.EventArgs e)
		{
			ColorCode();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			richTextBox1.SelectionColor = Color.Red;
		}

		private void ColorCode()
		{
			RichTextBox textBox = richTextBox1;
			
			int selectionStart = textBox.SelectionStart;
			
			string searchWord = "if";
			/*int startIndex = searchWord.Length+2;
			if(selectionStart < startIndex)
			{
				startIndex = textBox.Text.Length;
				MessageBox.Show(startIndex.ToString() + "   " + selectionStart);
			}
			string word = textBox.Text.Substring(selectionStart - startIndex, startIndex);
			*/

			int startIndex = 0;
			string word = textBox.Text;


			int wordIndex = 0;
			do
			{
				char charBefore = ' ';
				char charAfter = ' ';

				wordIndex = word.IndexOf(searchWord, wordIndex, word.Length - wordIndex);
				if(wordIndex >= 0)
				{
					if(selectionStart - startIndex + wordIndex-1 >= 0)
					{
						//charBefore = textBox.Text[selectionStart - startIndex + wordIndex-1];
					}

					if(selectionStart - startIndex + wordIndex + searchWord.Length-1 < textBox.Text.Length)
					{
						//textBox.Select(selectionStart - startIndex + wordIndex + searchWord.Length, 1);
						charAfter = textBox.Text[selectionStart - startIndex + wordIndex + searchWord.Length-1];
					}

					if( !((charBefore >= 'a' && charBefore <= 'z')	|| 
						(charBefore >= 'A' && charBefore <= 'Z'))	)
					{
						textBox.SuspendLayout();
						textBox.Select(selectionStart - startIndex + wordIndex, searchWord.Length);
						textBox.SelectionColor = Color.Blue;
						textBox.Select(selectionStart, 0);
						textBox.ResumeLayout();	
					}

					if(  charAfter == ' '							|| 
						!((charAfter >= 'a' && charAfter <= 'z')	|| 
						(charAfter >= 'A' && charAfter <= 'Z'))		)
					{
						textBox.SuspendLayout();
						textBox.Select(selectionStart - startIndex + wordIndex, searchWord.Length+1);
						textBox.SelectionColor = textColor;
						textBox.Select(selectionStart, 0);
						textBox.ResumeLayout();	
					}
					
					wordIndex += searchWord.Length;
				}
				statusBar.Text = "StartIndex: " + startIndex + "    SelectionStart: " 
					+ selectionStart + "  Word: " + word + "     WordIndex: " + wordIndex + "    CharBefore: '" + charBefore + "'   CharAfter: '" + charAfter + "'";

			} while(wordIndex >= 0 && word.Length > wordIndex);

			textBox.SelectionColor = textColor;
		}
	}
}
