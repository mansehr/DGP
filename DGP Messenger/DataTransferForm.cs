using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace DGP_Messenger
{
	/// <summary>
	/// Summary description for DataTransferForm.
	/// </summary>
	public class DataTransferForm : System.Windows.Forms.Form
	{
		//private ArrayList transfers = new ArrayList();
		private System.Windows.Forms.ListBox listBox1;
		private System.Drawing.Font listItemFont;
		private System.Windows.Forms.ImageList transferBarImg;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.ComponentModel.IContainer components;



		public DataTransferForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.Visible = false;
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);

			listItemFont = (Font)this.Font.Clone();

			/*AddNewTransfer(new Activity1("Hello", "On", "YOU"));
			AddNewTransfer(new Activity1("Tss", "123432", "Downloading"));
			AddNewTransfer(new Activity1("Hej.txt", "1234", "Sending"));*/
			/*ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem();
			ListViewItem lvi = new ListViewItem();
			listView1.Items.Add(*/

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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DataTransferForm));
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.transferBarImg = new System.Windows.Forms.ImageList(this.components);
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.listBox1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(246)), ((System.Byte)(246)), ((System.Byte)(247)));
			this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.listBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.listBox1.HorizontalExtent = 475;
			this.listBox1.HorizontalScrollbar = true;
			this.listBox1.ItemHeight = 80;
			this.listBox1.Location = new System.Drawing.Point(16, 24);
			this.listBox1.Name = "listBox1";
			this.listBox1.ScrollAlwaysVisible = true;
			this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.listBox1.Size = new System.Drawing.Size(496, 344);
			this.listBox1.TabIndex = 0;
			this.listBox1.SizeChanged += new System.EventHandler(this.listBox1_SizeChanged);
			this.listBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox1_DrawItem);
			// 
			// transferBarImg
			// 
			this.transferBarImg.ImageSize = new System.Drawing.Size(6, 17);
			this.transferBarImg.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("transferBarImg.ImageStream")));
			this.transferBarImg.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.OverwritePrompt = false;
			this.saveFileDialog.Title = "Spara filen";
			// 
			// DataTransferForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(246)), ((System.Byte)(246)), ((System.Byte)(247)));
			this.ClientSize = new System.Drawing.Size(528, 381);
			this.Controls.Add(this.listBox1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DataTransferForm";
			this.Text = "DGP Filhämtaren";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.DataTransferForm_Closing);
			this.ResumeLayout(false);

		}
		#endregion


		public void AddNewTransfer(Activity activity)
		{
			activity.linkLabel = new LinkLabel();

			activity.linkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			
			activity.linkLabel.Name = "linkLabel";
			activity.linkLabel.Size = new System.Drawing.Size(80, 70);
			activity.linkLabel.TabIndex = 17;
			activity.linkLabel.TabStop = true;
			if(activity.fileTransfer.TransferStatus == TransferStatus.CONFIRM_RECIVE)
			{
				activity.linkLabel.Text = "Spara";
			}
			else
			{
				activity.linkLabel.Text = "Avbryt";
			}
			activity.linkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			activity.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_LinkClicked);
	
			listBox1.Items.Add(activity);
			listBox1.Controls.Add(activity.linkLabel);

			activity.linkLabel.Location = getLinkLabelPoint(listBox1.Items.Count);

			activity.fileTransfer.DataUpdateEvent += new DGP_Messenger.FileTransfer.DataUpdate(fileTransfer_dataUpdate);
			activity.fileTransfer.StartTransferEvent += new DGP_Messenger.FileTransfer.TransferStarts(fileTransfer_StartTransferEvent);

			/*ListViewItem lvi = new ListViewItem();
			lvi.Text = activity.fileTransfer.FileName;
			lvi.SubItems.Add(activity.fileTransfer.FileSize.ToString());
			lvi.SubItems.Add(activity.fileTransfer.TransferStatus.ToString());
			//listView1.Items.Add(lvi);*/

		}

		public void UpdateFileTransfer(FileTransfer fileTransfer)
		{
			/*for(int i = 0; i < transfers.Count; i++)
			{
				Activity ac = (Activity)transfers[i];
				if(ac.fileTransfer == fileTransfer)
				{
					SetFileInfo(ac.fileTransfer, i);
				}
			}*/
		}


		/*	private void SetFileInfo(FileTransfer fileTransfer, int index)
			{
				listView1.BeginUpdate();

				long readBytes = fileTransfer.TransferedBytes / 1000;
				long allBytes =  fileTransfer.FileSize / 1000;
				short percentDone = (short)(((decimal)fileTransfer.TransferedBytes/(decimal)fileTransfer.FileSize)*100);

				label1.Text = "FileName: " + fileTransfer.FileName;
				label2.Text = "File size: " + allBytes + "kB";
				label3.Text = "Recived Bytes: " + readBytes + "kB";
				label4.Text = "Percent: " + percentDone + "%";

				listView1.Items[index].SubItems[0].Text = fileTransfer.FileName;
				listView1.Items[index].SubItems[1].Text = percentDone.ToString() + "%";
				listView1.Items[index].SubItems[2].Text = fileTransfer.TransferStatus.ToString();

				listView1.EndUpdate();
			}*/

		private void DataTransferForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Visible = false;
			e.Cancel = true;
		}

		private Point getLinkLabelPoint(int index)
		{
			int linkLabelLeft;
			int linkLabelTop;
			if(this.listBox1.Size.Width < 475)
			{
				//this.listBox1.
				linkLabelLeft = 415;
			}
			else
			{
				linkLabelLeft = listBox1.Right - 85;
			}
			
			if(index >= 0 && listBox1.Items.Count > index)
			{
				linkLabelTop = listBox1.GetItemRectangle(index).Top + 5;
			}
			else
			{
				linkLabelTop = (80 * index) + 5;
			}

			return new Point(linkLabelLeft, linkLabelTop);
		}

		private void listBox1_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			//listBox1.SuspendLayout();

			if(e.Index >= 0)
			{
				Activity ac = (Activity)listBox1.Items[e.Index];

				ac.linkLabel.Location = getLinkLabelPoint(e.Index);
				//ac.linkLabel.Text = e.Index.ToString() + ": " + ac.linkLabel.Location.Y.ToString();
				ac.linkLabel.Refresh();

				//e.Graphics.DrawString(this.listBox1.GetItemRectangle(e.Index).Top.ToString(), listItemFont, new SolidBrush(Color.Red), 80, e.Bounds.Top + 10);
				//e.Graphics.DrawString(e.Index.ToString(), listItemFont, new SolidBrush(Color.Red), 80, e.Bounds.Top + 30);
				e.Graphics.DrawString(ac.ToString(), listItemFont, new SolidBrush(Color.Red), 80, e.Bounds.Top + 10);
				e.Graphics.DrawString(ac.TransferState.ToString(), listItemFont, new SolidBrush(Color.Blue), 80, e.Bounds.Top + 50 );
				
				Point drawPoint = new Point(8, e.Bounds.Top + 8);
				Size drawSize = new Size(64,64);

				if(ac.image.Width < drawSize.Width)
				{
					drawPoint.X += (drawSize.Width - ac.image.Width) / 2;
					drawSize.Width = ac.image.Width;
				}

				if(ac.image.Height < drawSize.Height)
				{
					drawPoint.Y += (drawSize.Height - ac.image.Height) / 2; 
					drawSize.Height = ac.image.Height;
				}

				Rectangle bmpRect = new Rectangle(drawPoint, drawSize); 
				e.Graphics.DrawImage(ac.image, bmpRect, 0, 0, ac.image.Width, ac.image.Height, GraphicsUnit.Pixel); 



				int dlWidth = e.Bounds.Width - 275; //200;
				Point dlPoint = new Point(140, e.Bounds.Top + 30);

				int doneWidth;
				float percentDone, transferSpeed;
				/*if( ac.TransferState == TransferStatus.FILE_RECIVING || 
					ac.TransferState == TransferStatus.FILE_SENDING)
				{*/
				percentDone = ac.getPercent();
				transferSpeed = ac.getTransferSpeed()/1000F;
				/*}
				else
				{
					percentDone = 0;
					transferSpeed = 0;
				}*/
				doneWidth = (int)(dlWidth * percentDone) / 100;
		

				Bitmap dlMiddle = new Bitmap(transferBarImg.Images[1]);
				bmpRect = new Rectangle(dlPoint.X + doneWidth, dlPoint.Y, dlWidth - doneWidth, dlMiddle.Height);
				ImageAttributes imageAttr = new ImageAttributes();
				imageAttr.SetWrapMode(WrapMode.TileFlipX);
				e.Graphics.DrawImage(dlMiddle, bmpRect, 0, 0, dlMiddle.Width, dlMiddle.Height, GraphicsUnit.Pixel, imageAttr); 

				Bitmap dlLeft = new Bitmap(transferBarImg.Images[3]);
				bmpRect = new Rectangle(dlPoint.X, dlPoint.Y, dlLeft.Width, dlLeft.Height);
				e.Graphics.DrawImage(dlLeft, bmpRect, 0, 0, dlLeft.Width, dlLeft.Height, GraphicsUnit.Pixel); 
			
				Bitmap dlRight = new Bitmap(transferBarImg.Images[2]);
				bmpRect = new Rectangle(dlPoint.X + dlWidth - dlRight.Width, dlPoint.Y, dlRight.Width, dlRight.Height);
				e.Graphics.DrawImage(dlRight, bmpRect, 0, 0, dlRight.Width, dlRight.Height, GraphicsUnit.Pixel); 

				Bitmap dlDone = new Bitmap(transferBarImg.Images[0]);
				bmpRect = new Rectangle(dlPoint.X, dlPoint.Y, doneWidth, dlDone.Height);
				e.Graphics.DrawImage(dlDone, bmpRect, 0, 0, dlDone.Width, dlDone.Height, GraphicsUnit.Pixel, imageAttr);

				// Transferpercent String
				e.Graphics.DrawString(percentDone+" %", listItemFont, new SolidBrush(Color.Black), dlPoint.X + (dlWidth/2), dlPoint.Y );
				
				// Transferspeed String
				e.Graphics.DrawString(transferSpeed+" KB/s", listItemFont, new SolidBrush(Color.Black), dlPoint.X + dlWidth + 2, dlPoint.Y );
				
				// Data transfered
				decimal divide = 1;
				string byteType = "";
				bool stop = false;
				for(int i = 1; !stop && i < 6; i++)
				{
					int byteSize = (int)Math.Pow(1000, i+1);
					if(ac.FileSize > byteSize)
					{
						divide = byteSize;
					}
					else
					{
						stop = true;
					}
					if(i == 1)
					{
						byteType = "K";
					}
					else if(i == 2)
					{
						byteType = "M";
					}
					else if(i == 3)
					{
						byteType = "G";
					}
					else if(i == 4)
					{
						byteType = "T";
					}
					else if(i == 5)
					{
						byteType = "P";
					}
				}

				// Räkna och skriv ut hur många (M, G, T)bytes som har blivit nerladdade tot
				decimal transferedBytes = (ac.TransferedBytes/divide);
				decimal fileSize = (ac.FileSize/divide);
				string dataTransfered = transferedBytes.ToString() + " " + byteType + "B / " + fileSize.ToString() + " " + byteType + "B";
				e.Graphics.DrawString(dataTransfered, listItemFont, new SolidBrush(Color.Black), dlPoint.X + (dlWidth/2) - 50, dlPoint.Y + 20 );

				/*Font linkFont = new Font(listItemFont, FontStyle.Underline | FontStyle.Bold);
				e.Graphics.DrawString("Avbryt", linkFont, new SolidBrush(Color.Blue), e.Bounds.Right-50, dlPoint.Y );
	*/

				//Nedersta linjen
				e.Graphics.DrawLine(new Pen(new SolidBrush(Color.Gray), 1), new Point(4, e.Bounds.Bottom-1), new Point(e.Bounds.Right-4, e.Bounds.Bottom-1));
				//e.Graphics.DrawLine(new Pen(new SolidBrush(Color.Gray), 1), new Point(e.Bounds.Left+4, e.Bounds.Top), new Point(e.Bounds.Right-4, e.Bounds.Top));			
			
			}
			else if(listBox1.Items.Count == 0)
			{
				e.Graphics.DrawString("Inga Nedladdningar Pågår", listItemFont, new SolidBrush(Color.DarkGray), 80, 40 );
			}

			//listBox1.ResumeLayout(true);
		}

		private void listBox1_SizeChanged(object sender, System.EventArgs e)
		{
			/*Activity ac = (Activity)listBox1.Items[0];
			MessageBox.Show(ac.getTransferSpeed().ToString());*/
			listBox1.Refresh();
		}

		private void linkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			for(int i = 0; i < listBox1.Items.Count; i++)
			{
				LinkLabel lb = sender as LinkLabel;
				Activity ac = listBox1.Items[i] as Activity;
		
				if( LinkLabel.ReferenceEquals(lb, ac.linkLabel))
				{
					if(ac.TransferState == TransferStatus.CONFIRM_RECIVE)
					{	
						FileInfo fi = null;
						try
						{
							bool gotFile = true;
							do
							{
								saveFileDialog.FileName = ac.fileTransfer.FileName;
								DialogResult fileDialogResult = saveFileDialog.ShowDialog(this);

								if(fileDialogResult == DialogResult.OK)
								{
									fi = new FileInfo(saveFileDialog.FileName);
									if(fi.Exists)
									{
										bool canContinue;
										if(fi.Length < ac.fileTransfer.FileSize)
										{
											canContinue = true;
										}
										else
										{
											canContinue = false;
										}

										ResumeDownloadDialog rs = new ResumeDownloadDialog(fi.Name, canContinue);
										DialogResult result = rs.ShowDialog();
									
										/*MessageBox.Show("Filen " + fi.Name + " finns redan. Men har inte blivit helt nerladdad!\n"+ 
											"Vill du försöka fortsätta nerladdningen där den slutade sist? \n" +
											"Om inte så kommer nerladdningen börja om från början och filen kommer skrivas över!", 
											"File Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);*/

										if(result == DialogResult.OK)
										{
											ReciveFile rc = (ReciveFile)ac.fileTransfer;
											rc.ResumeTransfer(fi.FullName, fi.Length );
										}
										else if(result == DialogResult.Abort)
										{
											fi.Delete();
											MessageBox.Show("File Deleted");

											ReciveFile rc = (ReciveFile)ac.fileTransfer;
											rc.StartTransfer(fi.FullName);
										}
										else if(result == DialogResult.Cancel)
										{
											//MessageBox.Show("Leta ny fil");
											gotFile = false;
										}
									}
									else
									{
										ReciveFile rc = (ReciveFile)ac.fileTransfer;
										rc.StartTransfer(saveFileDialog.FileName);
									}
								}
								else if(fileDialogResult == DialogResult.Cancel)
								{
									/// Didnt get the file, but do nothing
									gotFile = true;
								}			
							}while(gotFile != true);
						}
						catch(Exception ex)
						{
							MessageBox.Show(ex.Message, "Transfer Error");
							if(ac != null)
							{
								ac.fileTransfer.close();
							}
						}
						/*finally
						{
							
						}*/
					}
					else if(ac.TransferState != TransferStatus.FILE_RECIVING)
					{
						listBox1.Items.Remove(ac);
						listBox1.Controls.Remove(lb);
					}
					else
					{
						ac.fileTransfer.close();
						lb.Text = "Ta Bort";
					}
					listBox1.Refresh();
				}
			}
		}

		private void fileTransfer_dataUpdate(object sender, EventArgs ev)
		{
			listBox1.Refresh();
			/*for(int i = 0; i < listBox1.Items.Count; i++)
			{
				Activity ac = (Activity)listBox1.Items[i];
				if(ac.fileTransfer == sender)
				{
					listBox1.Refresh();
				}
			}*/
		}

		private void fileTransfer_StartTransferEvent(object sender, EventArgs sfe)
		{
			listBox1.Refresh();
			for(int i = 0; i < listBox1.Items.Count; i++)
			{
				Activity ac = (Activity)listBox1.Items[i];
				if(ac.fileTransfer == sender)
				{
					listBox1.Refresh();
				}
			}
		}
	}


	public class Activity
	{
		private Bitmap bmp;
		private string _text;
		public LinkLabel linkLabel;
		public readonly FileTransfer fileTransfer;

		public Activity(FileTransfer fileTransfer)
		{
			this.fileTransfer = fileTransfer;
			bmp = ExtractIcon.GetIcon(fileTransfer.FileName, false).ToBitmap();
		}

		public Bitmap image
		{
			get { return bmp; }
		}

		public string FileName
		{
			get { return fileTransfer.FileName; }
		}

		public TransferStatus TransferState
		{
			get { return this.fileTransfer.TransferStatus; }
		}

		public long FileSize
		{
			get { return fileTransfer.FileSize; }
		}

		public string Text
		{
			get { return string.Copy(_text); }
			set { _text = value; }
		}

		public long TransferedBytes
		{
			get { return fileTransfer.TransferedBytes; }
		}

		public override string ToString()
		{
			return string.Copy(FileName);
		}

		public float getPercent()
		{
			float percentDone = 0;

			if(FileSize != 0)
			{
				percentDone = ((TransferedBytes*100)/FileSize);
			}

			return percentDone;
		}

		public long getTransferSpeed()
		{
			if(fileTransfer.TransferStatus != TransferStatus.WAITING)
			{
				return this.fileTransfer.TransferSpeed;
			}
			else
			{
				return /*(fileTransfer.TransferedBytes);// / */(fileTransfer.TimeUsed);
			}
		}
	}
}
