using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
//using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using DotMSN;

namespace DGP_Messenger
{
	/// <summary>
	/// Summary description for MessengerForm.
	/// </summary>
	public class MessageForm : System.Windows.Forms.Form
	{

		#region Delegate and Event

		public delegate void DisplayMessageDelegate(string message, Color color, string displayName, bool rtf);

		public delegate void DisplayMSNMessageDelegate(DotMSN.Message message, string displayName);
		
		public delegate void UpdateSendActivityDelegate(FileTransfer fileTransfer, SendFileEvent sfe);
		
		public delegate void UpdateReciveActivityDelegate(FileTransfer fileTransfer, SendFileEvent sfe);

//		public delegate void ReciveNewFileDelegate(Socket reciveConnection, string fileTransferId, bool viaServer);

		public delegate void NewActivityDelegate(FileTransfer fileTransfer);
		
		public delegate void SetNameAndStatusStringDelegate(string nameAndStatus);

		private delegate void StartClientWritingTimerDelegate(string clientName);

		public delegate void NewFileTransferEventHandler(object sender, NewFileTransferEventArgs e);

		public event NewFileTransferEventHandler NewFileTransfer;

		public event MessageFormClosedEventHandler MessageFormClosedEvent;
		
		public event EventHandler ShowDataTransferWindowEvent;

		public event EventHandler FontChangedEvent;

		#endregion

		#region Class Members

		private const int antEmotions = 9;//(int)EmotionImgs.Images.Count;
		private Emotions [] emotions = new Emotions[antEmotions];
		private ArrayList fileActivity = new ArrayList(); 
		//	private int startDrawActivity = 0;
		//	public readonly DataTransferForm dataTransferForm;

		private Conversation _conversation = null;
		private bool clientLeft = false;

		private ArrayList messageStack = new ArrayList();

		private User user;
		private DGPContact _contact;
		private bool _active;
		
		// Variables used to receive data from the socket
		private Socket	_clientSocket;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RichTextBox RecivedTextBox;
		private ExRichTextBox SendTextBox;
		private System.Windows.Forms.Timer flashTimer;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.PictureBox greyLine;
		private System.Windows.Forms.ImageList SendButtonImgs;
		private System.Windows.Forms.ImageList EmotionImgs;
		private System.Windows.Forms.FontDialog FontDialog;
		private System.Windows.Forms.ImageList KanterImgs;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.Button btnSendFile;
		private System.Windows.Forms.Button btnPic;
		private System.Windows.Forms.Button btnFont;
		private System.Windows.Forms.Panel emotionPan;
		private System.Windows.Forms.Button btnTransfers;
		private System.Windows.Forms.PictureBox btnSend;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.Timer typingTimer;
		private System.Windows.Forms.Timer clientWritingTimer;
		private DGP_Messenger.RoundedcornerPanel roundedcornerPanel1;
		private DGP_Messenger.RoundedcornerPanel roundedcornerPanel2;
		private System.Windows.Forms.StatusBarPanel statusBarPanel;
		private System.ComponentModel.IContainer components;

		#endregion

		#region Constructor and Dispose

		public MessageForm(User user, DGPContact contact, Socket socket)
		{
			initData();

			this.user			= user;
			this._contact		= contact;
			this._clientSocket	= socket;
			this.SetDGPContactName( _contact.Name);

			//this.DisplayMessage("Till : " + contact.Name + "\n", Color.Purple, "");			
		}

		public MessageForm(string contactId)
		{
			initData();

			this.user.DisplayName = "NULL";
			this._contact		= new DGPContact(contactId, "NULL", 'Z');

			this.SetDGPContactName(contactId);		
		}

		public MessageForm(Conversation conv, string mail)
		{
			initData();

			this.SetConversation( conv );
			this._contact		= new DGPContact(mail, "", 'Z');

			this.UpdateMsnContactName();

			//this.DisplayMessage("Till : " + contact.Name + "\n", Color.Purple, "");			
		}


		/// <summary>
		/// Set variables that are set for all constructors
		/// </summary>
		private void initData()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);

			this._active		= false;
			this._clientSocket	= null;
			this.user			= new User();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			foreach(Activity ac in fileActivity)
			{
				if(ac.fileTransfer != null)
					ac.fileTransfer.close();
			}

			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );

			if(MessageFormClosedEvent != null)
			{
				MessageFormClosedEvent(this, new MessageFormClosedEventArgs(this));
			}

		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MessageForm));
			this.label1 = new System.Windows.Forms.Label();
			this.RecivedTextBox = new System.Windows.Forms.RichTextBox();
			this.SendTextBox = new DGP_Messenger.ExRichTextBox();
			this.flashTimer = new System.Windows.Forms.Timer(this.components);
			this.SendButtonImgs = new System.Windows.Forms.ImageList(this.components);
			this.btnSend = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.greyLine = new System.Windows.Forms.PictureBox();
			this.btnPic = new System.Windows.Forms.Button();
			this.EmotionImgs = new System.Windows.Forms.ImageList(this.components);
			this.btnFont = new System.Windows.Forms.Button();
			this.FontDialog = new System.Windows.Forms.FontDialog();
			this.KanterImgs = new System.Windows.Forms.ImageList(this.components);
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.btnSendFile = new System.Windows.Forms.Button();
			this.btnTransfers = new System.Windows.Forms.Button();
			this.emotionPan = new System.Windows.Forms.Panel();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.statusBarPanel = new System.Windows.Forms.StatusBarPanel();
			this.typingTimer = new System.Windows.Forms.Timer(this.components);
			this.clientWritingTimer = new System.Windows.Forms.Timer(this.components);
			this.roundedcornerPanel1 = new DGP_Messenger.RoundedcornerPanel();
			this.roundedcornerPanel2 = new DGP_Messenger.RoundedcornerPanel();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.CausesValidation = false;
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(32, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(424, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Konversation Med ";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// RecivedTextBox
			// 
			this.RecivedTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.RecivedTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.RecivedTextBox.HideSelection = false;
			this.RecivedTextBox.Location = new System.Drawing.Point(32, 96);
			this.RecivedTextBox.Name = "RecivedTextBox";
			this.RecivedTextBox.ReadOnly = true;
			this.RecivedTextBox.Size = new System.Drawing.Size(544, 280);
			this.RecivedTextBox.TabIndex = 3;
			this.RecivedTextBox.Text = "";
			this.RecivedTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.This_LinkClicked);
			// 
			// SendTextBox
			// 
			this.SendTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.SendTextBox.BackColor = System.Drawing.Color.White;
			this.SendTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.SendTextBox.HiglightColor = DGP_Messenger.RtfColor.White;
			this.SendTextBox.Location = new System.Drawing.Point(32, 432);
			this.SendTextBox.MaxLength = 1024;
			this.SendTextBox.Name = "SendTextBox";
			this.SendTextBox.Size = new System.Drawing.Size(448, 64);
			this.SendTextBox.TabIndex = 4;
			this.SendTextBox.Text = "";
			this.SendTextBox.TextColor = DGP_Messenger.RtfColor.Black;
			this.SendTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTextBox_KeyDown);
			this.SendTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SendTextBox_KeyPress);
			this.SendTextBox.TextChanged += new System.EventHandler(this.SendTextBox_TextChanged);
			this.SendTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.This_LinkClicked);
			// 
			// flashTimer
			// 
			this.flashTimer.Enabled = true;
			this.flashTimer.Interval = 6000;
			this.flashTimer.Tick += new System.EventHandler(this.flashTimer_Tick);
			// 
			// SendButtonImgs
			// 
			this.SendButtonImgs.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
			this.SendButtonImgs.ImageSize = new System.Drawing.Size(95, 72);
			this.SendButtonImgs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("SendButtonImgs.ImageStream")));
			this.SendButtonImgs.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// btnSend
			// 
			this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSend.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnSend.Enabled = false;
			this.btnSend.Image = ((System.Drawing.Image)(resources.GetObject("btnSend.Image")));
			this.btnSend.Location = new System.Drawing.Point(496, 424);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(95, 72);
			this.btnSend.TabIndex = 5;
			this.btnSend.TabStop = false;
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			this.btnSend.MouseEnter += new System.EventHandler(this.btnSend_MouseEnter);
			this.btnSend.MouseLeave += new System.EventHandler(this.btnSend_MouseLeave);
			// 
			// pictureBox2
			// 
			this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Top;
			this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
			this.pictureBox2.Location = new System.Drawing.Point(0, 0);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(600, 32);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox2.TabIndex = 7;
			this.pictureBox2.TabStop = false;
			// 
			// greyLine
			// 
			this.greyLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.greyLine.BackColor = System.Drawing.Color.LightGray;
			this.greyLine.Location = new System.Drawing.Point(24, 80);
			this.greyLine.Name = "greyLine";
			this.greyLine.Size = new System.Drawing.Size(552, 3);
			this.greyLine.TabIndex = 10;
			this.greyLine.TabStop = false;
			// 
			// btnPic
			// 
			this.btnPic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnPic.Location = new System.Drawing.Point(24, 392);
			this.btnPic.Name = "btnPic";
			this.btnPic.Size = new System.Drawing.Size(56, 24);
			this.btnPic.TabIndex = 11;
			this.btnPic.Text = "Bild";
			this.btnPic.Click += new System.EventHandler(this.btnPic_Click);
			// 
			// EmotionImgs
			// 
			this.EmotionImgs.ImageSize = new System.Drawing.Size(19, 19);
			this.EmotionImgs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("EmotionImgs.ImageStream")));
			this.EmotionImgs.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// btnFont
			// 
			this.btnFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnFont.Location = new System.Drawing.Point(88, 392);
			this.btnFont.Name = "btnFont";
			this.btnFont.Size = new System.Drawing.Size(56, 24);
			this.btnFont.TabIndex = 12;
			this.btnFont.Text = "Font";
			this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
			// 
			// FontDialog
			// 
			this.FontDialog.FontMustExist = true;
			this.FontDialog.MaxSize = 20;
			this.FontDialog.MinSize = 8;
			this.FontDialog.ShowColor = true;
			// 
			// KanterImgs
			// 
			this.KanterImgs.ImageSize = new System.Drawing.Size(10, 10);
			this.KanterImgs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("KanterImgs.ImageStream")));
			this.KanterImgs.TransparentColor = System.Drawing.Color.White;
			// 
			// openFileDialog
			// 
			this.openFileDialog.Title = "Välj fil som du vill skicka";
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.OverwritePrompt = false;
			this.saveFileDialog.Title = "Spara filen";
			// 
			// btnSendFile
			// 
			this.btnSendFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSendFile.Location = new System.Drawing.Point(152, 392);
			this.btnSendFile.Name = "btnSendFile";
			this.btnSendFile.Size = new System.Drawing.Size(72, 23);
			this.btnSendFile.TabIndex = 0;
			this.btnSendFile.Text = "Skicka Fil";
			this.btnSendFile.Click += new System.EventHandler(this.sendFile_Click);
			// 
			// btnTransfers
			// 
			this.btnTransfers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnTransfers.Location = new System.Drawing.Point(232, 392);
			this.btnTransfers.Name = "btnTransfers";
			this.btnTransfers.Size = new System.Drawing.Size(64, 23);
			this.btnTransfers.TabIndex = 22;
			this.btnTransfers.Text = "Transfers";
			this.btnTransfers.Click += new System.EventHandler(this.btnTransfers_Click);
			// 
			// emotionPan
			// 
			this.emotionPan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.emotionPan.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(255)), ((System.Byte)(192)));
			this.emotionPan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.emotionPan.Cursor = System.Windows.Forms.Cursors.Hand;
			this.emotionPan.Location = new System.Drawing.Point(24, 424);
			this.emotionPan.Name = "emotionPan";
			this.emotionPan.Size = new System.Drawing.Size(112, 40);
			this.emotionPan.TabIndex = 25;
			this.emotionPan.Visible = false;
			this.emotionPan.Click += new System.EventHandler(this.emotionPan_Click);
			this.emotionPan.VisibleChanged += new System.EventHandler(this.emotionPan_VisibleChanged);
			this.emotionPan.Paint += new System.Windows.Forms.PaintEventHandler(this.emotionPan_Paint);
			this.emotionPan.MouseMove += new System.Windows.Forms.MouseEventHandler(this.emotionPan_MouseMove);
			this.emotionPan.MouseLeave += new System.EventHandler(this.emotionPan_MouseLeave);
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 511);
			this.statusBar.Name = "statusBar";
			this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						 this.statusBarPanel});
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(600, 22);
			this.statusBar.TabIndex = 27;
			// 
			// statusBarPanel
			// 
			this.statusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
			this.statusBarPanel.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
			this.statusBarPanel.Icon = ((System.Drawing.Icon)(resources.GetObject("statusBarPanel.Icon")));
			this.statusBarPanel.Text = "Chat";
			this.statusBarPanel.Width = 59;
			// 
			// typingTimer
			// 
			this.typingTimer.Interval = 5000;
			this.typingTimer.Tick += new System.EventHandler(this.typingTimer_Tick);
			// 
			// clientWritingTimer
			// 
			this.clientWritingTimer.Interval = 10000;
			this.clientWritingTimer.Tick += new System.EventHandler(this.clientWriting_Tick);
			// 
			// roundedcornerPanel1
			// 
			this.roundedcornerPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.roundedcornerPanel1.BackColor = System.Drawing.Color.Transparent;
			this.roundedcornerPanel1.ForeColor = System.Drawing.Color.White;
			this.roundedcornerPanel1.Location = new System.Drawing.Point(24, 88);
			this.roundedcornerPanel1.Name = "roundedcornerPanel1";
			this.roundedcornerPanel1.Size = new System.Drawing.Size(560, 296);
			this.roundedcornerPanel1.TabIndex = 28;
			// 
			// roundedcornerPanel2
			// 
			this.roundedcornerPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.roundedcornerPanel2.BackColor = System.Drawing.Color.Transparent;
			this.roundedcornerPanel2.ForeColor = System.Drawing.Color.White;
			this.roundedcornerPanel2.Location = new System.Drawing.Point(24, 424);
			this.roundedcornerPanel2.Name = "roundedcornerPanel2";
			this.roundedcornerPanel2.Size = new System.Drawing.Size(464, 80);
			this.roundedcornerPanel2.TabIndex = 29;
			// 
			// MessageForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(246)), ((System.Byte)(246)), ((System.Byte)(247)));
			this.CausesValidation = false;
			this.ClientSize = new System.Drawing.Size(600, 533);
			this.Controls.Add(this.emotionPan);
			this.Controls.Add(this.statusBar);
			this.Controls.Add(this.SendTextBox);
			this.Controls.Add(this.RecivedTextBox);
			this.Controls.Add(this.btnTransfers);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.greyLine);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.btnSendFile);
			this.Controls.Add(this.btnFont);
			this.Controls.Add(this.btnPic);
			this.Controls.Add(this.btnSend);
			this.Controls.Add(this.roundedcornerPanel1);
			this.Controls.Add(this.roundedcornerPanel2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MessageForm";
			this.Text = "MessengerForm";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.MessageForm_Closing);
			this.Load += new System.EventHandler(this.MessageForm_Load);
			this.Activated += new System.EventHandler(this.MessageForm_Activated);
			this.Deactivate += new System.EventHandler(this.MessageForm_Deactivate);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties

		public DGPContact contact
		{
			set { _contact = value; }
			get { return _contact; }
		}

		public Socket socket
		{
			get { return _clientSocket; }
		}

		public bool Active
		{
			get { return _active; }
			set { _active = value; }
		}

		public Conversation conversation
		{
			get { return _conversation; }
		}

		public Font TextFont
		{
			get { return SendTextBox.Font; }
			set 
			{
				SendTextBox.Font = value; 
			
				ExRichTextBox rtfConv = new ExRichTextBox();
				rtfConv.Rtf = RecivedTextBox.Rtf;
				rtfConv.SelectAll();
				if(rtfConv.SelectionFont != null)
				{
					rtfConv.SelectionFont = new Font(rtfConv.SelectionFont.FontFamily, SendTextBox.Font.Size, SendTextBox.Font.Style);
				}
				else
				{
					rtfConv.SelectionFont = new Font("Verdana", SendTextBox.Font.Size, SendTextBox.Font.Style);
				}

				RecivedTextBox.Select(RecivedTextBox.Text.Length, 0);
				RecivedTextBox.Rtf = rtfConv.Rtf;
			}
		}

		public Color TextColor
		{
			get { return SendTextBox.ForeColor; }
			set { SendTextBox.ForeColor = value; }
		}

		#endregion

		#region Methods

		public void AlertMessage()
		{
			Sound blip = new Sound();
			blip.Play("blip.wav", blip.SND_ASYNC);

			clsUser32Dll.FlashWindowOnce(this);

			flashTimer.Start();
		}


		public void StopFlashing()
		{
			clsUser32Dll.FlashWindowStop(this);
		}


		#region DisplayMessageFunctions

		public void DisplayMessage(string message)
		{
			this.Invoke(new DisplayMessageDelegate(this.DisplayMessage), new object [] {message, Color.Black, "", false});
		}

		public void DisplayMessage(DotMSN.Message message, string displayName)
		{
			FontStyle fontStyle;
			
			switch(message.Decorations)
			{
				case DotMSN.MSNTextDecorations.Bold:		fontStyle = FontStyle.Bold; break;
				case DotMSN.MSNTextDecorations.Italic:		fontStyle = FontStyle.Italic; break;
				case DotMSN.MSNTextDecorations.Strike:		fontStyle = FontStyle.Strikeout; break;
				case DotMSN.MSNTextDecorations.Underline:	fontStyle = FontStyle.Underline; break;
				case DotMSN.MSNTextDecorations.None: 
				default: fontStyle = FontStyle.Regular; break;
			}

			Font font = new Font(message.Font, TextFont.Size, fontStyle);

			DisplayMessage(message.Text, message.Color, displayName, false, font); 
		}

		public void DisplayMessage(string message, Color color, string displayName, bool rtf)
		{
			DisplayMessage(message, color, displayName, rtf, null);
		}

		public void DisplayMessage(string message, Color color, string displayName, bool rtf, Font font)
		{	
			int index = RecivedTextBox.Text.Length;
			if(displayName != "")
			{
				RecivedTextBox.Select(RecivedTextBox.Text.Length, 0);
				RecivedTextBox.SelectionIndent = 5;
				RecivedTextBox.SelectionColor = Color.Red;
				RecivedTextBox.SelectionFont = new Font("Verdana", SendTextBox.Font.Size);
				RecivedTextBox.AppendText(displayName);
				RecivedTextBox.SelectionColor = Color.FromArgb(46, 56, 84);
				RecivedTextBox.AppendText(" säger:\n");
			}

			if(rtf == true)
			{
				ExRichTextBox rtfConv = new ExRichTextBox();
				rtfConv.Rtf = message;
				rtfConv.SelectAll();
				rtfConv.SelectionIndent = 20;
				if(rtfConv.SelectionFont != null)
				{
					rtfConv.SelectionFont = new Font(rtfConv.SelectionFont.FontFamily, SendTextBox.Font.Size, SendTextBox.Font.Style);
				}
				else
				{
					rtfConv.SelectionFont = new Font("Verdana", SendTextBox.Font.Size, SendTextBox.Font.Style);
				}

				RecivedTextBox.Select(RecivedTextBox.Text.Length, 0);
				RecivedTextBox.SelectedRtf = rtfConv.Rtf;
			}
			else
			{
				RecivedTextBox.SelectionIndent = 20;
				RecivedTextBox.SelectionColor = color;
				if(font != null)
				{
					RecivedTextBox.SelectionFont = font;
				}
				else
				{
					RecivedTextBox.SelectionFont = new Font("Verdana", TextFont.Size);
				}
				message = message.Replace('\r', ' ');
				message = message.Replace('\n', ' ');
				RecivedTextBox.AppendText(message.Trim() + "\n");
			}

			//Check for smileys!
			int length = RecivedTextBox.Text.Length;
			//int index = RecivedTextBox.Text.Length - length;
			string compString;

			for(int stringIndex = index; stringIndex < length-1; stringIndex++)
			{
				//MessageBox.Show(RecivedTextBox.Text.Length + " - " + length.ToString() + " = " + index.ToString() + " ++" + stringIndex.ToString());
				compString = RecivedTextBox.Text[stringIndex].ToString() + RecivedTextBox.Text[stringIndex+1].ToString();
			
				for(int iconIndex = 0; iconIndex < antEmotions; iconIndex++)
				{
					if( compString.ToLower() == emotions[iconIndex].itsString)
					{
						RecivedTextBox.Select(stringIndex, 2);

						ExRichTextBox picToRich = new ExRichTextBox();
						picToRich.InsertImage(EmotionImgs.Images[emotions[iconIndex].number]);
						RecivedTextBox.SelectedRtf = picToRich.Rtf;
					}
				}
			}
			if(this.Active == true)
			{
				RecivedTextBox.Focus();
				RecivedTextBox.ScrollToCaret();
				SendTextBox.Focus();
				SendTextBox.Select(SendTextBox.TextLength, 0);
			}
			else
			{
				AlertMessage();
			}
		}


		#endregion

		public string GetId()
		{
			if(_contact.IsMSNContact == true)
			{
				return _contact.MSNContact.Mail;
			}
			else
			{
				return _contact.Id;
			}
		}

		private void SendMSNFile(string fileName)
		{
			if(this._conversation.Connected)
			{
				DotMSN.Contact sender = this._conversation.Messenger.Owner;
				
				//foreach(string reciverMail in this._conversation.Users)
				{
					FileStream			fileStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite); 
					DotMSN.FileTransfer fileTransfer = new DotMSN.FileTransfer(this._conversation, sender, this._contact.MSNContact, fileName, fileStream);

					MSNFileTransfer msnFileTransfer = new MSNFileTransfer(fileTransfer, this);

					this._conversation.FileTransferHandler.TransferFile(fileTransfer);

					if(NewFileTransfer != null)
					{
						NewFileTransfer(this, new NewFileTransferEventArgs(msnFileTransfer));
					}

				}
				//this._conversation.FileTransferHandler.TransferFile(this._contact.Id, fileName);
			}
		}

		private void SendMessage(string message)
		{
			try
			{
				if(this._conversation  != null)
				{
					if(clientLeft == true)
					{
						MSNHeaderStringBuilder header = new MSNHeaderStringBuilder(SendTextBox.SelectionFont, SendTextBox.SelectionColor);
						_conversation.SendMessage(message, header.Header);
						//MessageBox.Show(header.Header);
					}
					else
					{
						_conversation.Messenger.RequestConversation(GetId(), GetId());
						messageStack.Add(message);
					}
				}
				else
				{
					_clientSocket.Send(Encoding.UTF8.GetBytes(message + "\n\r"));
				}
			}
			catch (Exception e)
			{
				MessageBox.Show("MSG: "+e.Message + "\nTargetSite:" + e.TargetSite +"\nInnerException: "+ e.InnerException, "Send Message From MessageForm!" );
			}
		}

		public void SetConversation(DotMSN.Conversation conv)
		{
			ShowConnected();
			this._conversation = conv;
		
			if(conv != null)
			{
				conv.AllContactsLeft += new DotMSN.Conversation.AllContactsLeftHandler(conv_AllContactsLeft);
				conv.ConnectionClosed += new DotMSN.Conversation.ConnectionClosedHandler(conv_ConnectionClosed);
				conv.ContactJoin += new DotMSN.Conversation.ContactJoinHandler(conv_ContactJoin);
				conv.ContactLeave += new DotMSN.Conversation.ContactLeaveHandler(conv_ContactLeave);
				conv.MessageReceived += new DotMSN.Conversation.MessageReceivedHandler(conv_MessageReceived);	
				conv.UserTyping += new DotMSN.Conversation.UserTypingHandler(conv_UserTyping);
				conv.FileTransferHandler.InvitationReceived += new DotMSN.FileTransferHandler.FileTransferInvitationHandler(FileTransferHandler_InvitationReceived);
				
				this.user.DisplayName = conv.Messenger.Owner.Mail;
				clientLeft = true;
			}
			else
			{
				this.user.DisplayName = "NULL";
				clientLeft = false;
			}
		}
		
		public void SetDGPContactName(string name)
		{
			contact.Name = name;

			string connected;
			connected = name + " " + contact.DGPStatus.GetStatusString();

			if(this.Created)
			{
				this.Invoke(new SetNameAndStatusStringDelegate(this.SetNameAndStatusString), new object [] { connected });
			}
			else
			{
				this.SetNameAndStatusString(connected);
			}
		}

		public void UpdateMsnContactName()
		{
			if(this._conversation != null)
			{
				string connected = "";
				int i = 0;
				foreach(Contact con in this._conversation.Users.Values)
				{
					if(i++ > 0)
					{
						connected += ", ";
					}
					connected += con.Name + " (" + con.Status+")";
				}

				if(this.Created)
				{
					this.Invoke(new SetNameAndStatusStringDelegate(this.SetNameAndStatusString), new object [] { connected });
				}
				else
				{
					this.SetNameAndStatusString(connected);
				}		
			}
		}

		public void SetNameAndStatusString(string nameAndStatus)
		{
			this.Text			= nameAndStatus + " - Meddelandefönster ";
			this.label1.Text	= "Konversation Med: " + nameAndStatus;
		}

		public void SocketConnect(Socket socket)
		{
			_clientSocket = socket;

			ShowConnected();
		}

		private void ShowConnected()
		{
			SendTextBox.Enabled = true;
			if(this.SendTextBox.Text.Length > 0)
				btnSend.Enabled = true;
		}

		public void SocketDisconnect()
		{
			_clientSocket = null;

			ShowDisconnected();
		}

		private void ShowDisconnected()
		{
			SendTextBox.Enabled = false;
			btnSend.Enabled = false;
		}

		private void StartClientWritingTimer(string clientName)
		{
			this.statusBarPanel.Text = clientName + " is typing";

			//Restart the timer so we dont get a tick wich would stop the timer
			clientWritingTimer.Stop();
			clientWritingTimer.Start();
		}

		private void SendWritingMessage()
		{
			if(this._conversation != null)
			{
				try
				{
					string theString = "MIME-Version: 1.0\r\n" + "Content-Type: text/x-msmsgscontrol\r\n" + "TypingUser: " + this._conversation.Messenger.Owner.Mail + "\r\n";
			
					this._conversation.SendMessage("", theString);
				}
				catch(DotMSN.MSNException)
				{
					//MessageBox.Show(ex.Message + "\n" + this._conversation.Connected, "MSNException");
					_conversation.Messenger.RequestConversation(GetId(), GetId());
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message, "Exception");
				}
			}
		}

		private void WritingMessage()
		{
			if(typingTimer.Enabled != true)
			{
				SendWritingMessage();

				// Start the timer (that will call the SendWritingMessage() function every 5 seconds ) 
				typingTimer.Start();
			}
		}


		#endregion

		#region Events

		#region conversation events

		public void conv_MessageReceived(Conversation sender, MessageEventArgs e)
		{
			//MessageBox.Show(e.Message.Header);
			
			// Reset the statusbar text
			this.statusBarPanel.Text = "";
			typingTimer.Stop();
			
			this.Invoke(new DisplayMSNMessageDelegate(this.DisplayMessage), new object [] { e.Message, e.Sender.Name} );

			if(this.Visible != true)
			{
				this.Show();
			}
		}

		private void conv_AllContactsLeft(Conversation sender, EventArgs e)
		{
			clientLeft = false;
		}

		private void conv_ContactJoin(Conversation sender, ContactEventArgs e)
		{
			this._contact.MSNContact = e.Contact;
			this.DisplayMessage("\t\t\t\t\t\tContact joined! " + e.Contact.Name);
			this.UpdateMsnContactName();

			foreach( string message in this.messageStack)
			{
				this.SendMessage(message);
			}
			messageStack.Clear();
		}

		private void conv_ContactLeave(Conversation sender, ContactEventArgs e)
		{
			this.DisplayMessage("\t\t\t\t\t\tContact left! " + e.Contact.Name);
			if(sender.Users.Count > 0)
			{
				this.UpdateMsnContactName();
			}
		}

		private void conv_UserTyping(Conversation sender, ContactEventArgs e)
		{
			this.Invoke(new StartClientWritingTimerDelegate(this.StartClientWritingTimer), new object [] { e.Contact.Name } );
		}

		private void conv_ConnectionClosed(Conversation sender, EventArgs e)
		{
			DisplayMessage("\t\t\t\t\t\tCoversation Connection Closed!");
			clientLeft = false;
		}	


		#endregion

		#region Button clicks

		private void btnSend_Click(object sender, System.EventArgs e)
		{
			_active = true;

			if(SendTextBox.Text != "")
			{
				if(this._conversation  != null)		// Send via msn
				{
					SendMessage(SendTextBox.Text);
					DisplayMessage(SendTextBox.Rtf, SendTextBox.ForeColor, user.DisplayName, true);
				}
				else
				{
					SendMessage("MSG " + SendTextBox.Rtf + " END_MESSAGE\r" + contact.Id + "\r" + user.Id);
					DisplayMessage(SendTextBox.Rtf, SendTextBox.ForeColor, user.DisplayName, true);
				}

				//Reset the text and Stop writing timer
				SendTextBox.Text = "";
				SendTextBox.Focus();
				statusBar.Text = "";
				typingTimer.Stop();
			}
		}

		private void btnPic_Click(object sender, System.EventArgs e)
		{
			this.emotionPan.Show();
			this.emotionPan.BringToFront();
		}

		private void btnFont_Click(object sender, System.EventArgs e)
		{
			FontDialog.Font = this.TextFont;
			FontDialog.Color = this.TextColor;

			if(FontDialog.ShowDialog() == DialogResult.OK)
			{
				this.TextFont = FontDialog.Font;
				this.TextColor = FontDialog.Color;

				if(FontChangedEvent != null)
				{
					FontChangedEvent(this, new EventArgs());
				}		
			}
		}

		private void sendFile_Click(object sender, System.EventArgs e)
		{
			if(openFileDialog.ShowDialog() == DialogResult.OK)
			{
				DisplayMessage("Tries to send '" + openFileDialog.FileName + "' to " + this._contact.Name);
				if(this._conversation != null)
				{
					SendMSNFile(openFileDialog.FileName);
				}
				else
				{
					this.SendMessage("SEND_FILE_TO " + this.contact.Id);
				}
				//Show dataTransferWindow
				if(ShowDataTransferWindowEvent != null)
				{
					ShowDataTransferWindowEvent(this, new EventArgs());
				}
			}
		}

		private void btnTransfers_Click(object sender, System.EventArgs e)
		{
			if(ShowDataTransferWindowEvent != null)
			{
				ShowDataTransferWindowEvent(this, new EventArgs());
			}
		}

		#endregion

		#region SendTextBox Events

		private void SendTextBox_TextChanged(object sender, System.EventArgs e)
		{
			if(SendTextBox.Text.Length == 0)
			{
				btnSend.Enabled = false;		
			}
			else if((this.socket != null || this._conversation != null) )
			{
				btnSend.Enabled = true;
			}
		}

		private void SendTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(SendTextBox.Text.Length > 1023)
			{
				e.Handled = false;	
			}
		}

		private void SendTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyValue == (char)13)
			{
				e.Handled = true;
				if(SendTextBox.Text.Length != 0)
				{
					btnSend_Click(sender, e);
				}
			}
			else
			{
				SendWritingMessage();
			}
		}


		#endregion

		#region EmotionPan Events

		private void emotionPan_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			int itemWidth = 24;
			int itemHeight = 24;

			int iconsEachRow = 4;
			int rows = (int)(EmotionImgs.Images.Count / iconsEachRow)+1;
			emotionPan.Size = new Size(iconsEachRow * itemWidth + 2, rows * itemHeight + 2);

			for(int i = 0; i < emotions.Length; i++)
			{
				if(emotions[i].active)
				{
					Rectangle rc = new Rectangle(	emotions[i].Left+1, emotions[i].Top+1, 
						emotions[i].Width-2, emotions[i].Height-2);

					e.Graphics.FillRectangle(new SolidBrush(Color.LightBlue), rc);
					e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.DarkBlue), 1), rc);
 
				}
				e.Graphics.DrawImageUnscaled(emotions[i].image, emotions[i].X+2, emotions[i].Y+2);
			}
		}


		private void emotionPan_VisibleChanged(object sender, System.EventArgs e)
		{
			emotionPan.Top = btnPic.Bottom + 1;
			emotionPan.Left = btnPic.Left;
		}


		private void emotionPan_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			bool update = false;
			for(int i = 0; i < emotions.Length; i++)
			{
				if(emotions[i].pointInside(e.X, e.Y))
				{
					if(emotions[i].active != true)
					{
						update = true;
					}
				}
				else
				{ 
					if(emotions[i].active == true)
					{
						update = true;
					}
				}

				if(update)
				{
					emotions[i].active = !emotions[i].active;

					emotionPan.Invalidate(emotions[i].Rect);
					emotionPan.Update();

					update = false;
				}
			}
		}


		private void emotionPan_Click(object sender, System.EventArgs e)
		{
			for(int i = 0; i < emotions.Length; i++)
			{
				if(emotions[i].active)
				{
					//SendTextBox.InsertImage(emotions[i].image);
					SendTextBox.InsertTextAsRtf(emotions[i].itsString);
					emotions[i].active = false;
				}
			}
			SendTextBox.Focus();
			emotionPan.Hide();
		}

		private void emotionPan_MouseLeave(object sender, System.EventArgs e)
		{
			emotionPan.Hide();
			for(int i = 0; i < emotions.Length; i++)
			{
				emotions[i].active = false;
			}
		}

		#endregion

		//Click on a link will open an website
		private void This_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start(e.LinkText);
			}
			catch(Exception)
			{
			}
		}

		
		//Active and not
		private void MessageForm_Activated(object sender, System.EventArgs e)
		{
			StopFlashing();

			RecivedTextBox.Focus();
			RecivedTextBox.ScrollToCaret();
			SendTextBox.Focus();
			SendTextBox.Select(SendTextBox.TextLength, 0);

			_active = true;
		}

		private void MessageForm_Deactivate(object sender, System.EventArgs e)
		{
			_active = false;
		}

		
		//button picturechanging
		private void btnSend_MouseEnter(object sender, System.EventArgs e)
		{
			btnSend.Image = SendButtonImgs.Images[0];
			btnSend.Refresh();
		}

		private void btnSend_MouseLeave(object sender, System.EventArgs e)
		{
			btnSend.Image = SendButtonImgs.Images[1];
			btnSend.Refresh();
		}

		
		//Load and close
		private void MessageForm_Load(object sender, System.EventArgs e)
		{
			int iconsEachRow = 4;

			int itemWidth = 24;
			int itemHeight = 24;

			for(int i = 0; i < antEmotions; i++)
			{
				int row = (i/iconsEachRow);
				int y = row*itemHeight;
				int x = itemWidth*i - (row*iconsEachRow*itemWidth);

				emotions[i] = new Emotions(EmotionImgs.Images[i]);
				emotions[i].number = i;
				emotions[i].size = new Size(itemWidth, itemHeight);

				emotions[i].position = new Point(x,y);
			}

			emotions[0].itsString = ":)";
			emotions[1].itsString = ":d";
			emotions[2].itsString = ":$";
			emotions[3].itsString = ":s";
			emotions[4].itsString = ":(";
			emotions[5].itsString = ":?";
			emotions[6].itsString = ";)";
			emotions[7].itsString = ":p";
			emotions[8].itsString = ":/";
		}

		private void MessageForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
		}


		//Timer ticks
		private void typingTimer_Tick(object sender, System.EventArgs e)
		{
			SendWritingMessage();
		}

		private void clientWriting_Tick(object sender, System.EventArgs e)
		{
			// Reset the statusbar text
			this.statusBarPanel.Text = "";
			typingTimer.Stop();
		}
		
		private void flashTimer_Tick(object sender, System.EventArgs e)
		{
			StopFlashing();
		}


		#endregion

		#region (DGP) File activities and old code

		/*	public void SetDGPContactOnlineStatus(char status)
		{
			//contact.Status.Status = status;
			//SetDGPContactName(contact.Name);
		}
*/
		

		public void NewActivity(FileTransfer fileTransfer)
		{
			fileTransfer.TransferCompleteEvent += new DGP_Messenger.FileTransfer.TransferComplete(fileTransfer_TransferDoneEvent);
			Activity activity = new Activity(fileTransfer);
			activity.linkLabel = new LinkLabel();
			//activity.label = new Label();
			//activity.image = new PictureBox();
			//activity.Top = 0;

			// label
			/*activity.label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			activity.label.Location = new System.Drawing.Point(pictureBox5.Right + 4, pictureBox5.Bottom - 80);
			activity.label.Name = "label2";
			activity.label.Size = new System.Drawing.Size(120, 80);
			activity.label.TabIndex = 15;
			activity.label.Text = labelText;
			activity.label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			activity.label.SendToBack();
			*/
			// linkLabel
			activity.linkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			//activity.linkLabel.Location = new System.Drawing.Point(pictureBox5.Right + 4, pictureBox5.Bottom - 80 );
			activity.linkLabel.Name = "linkLabel2";
			activity.linkLabel.Size = new System.Drawing.Size(120, 80);
			activity.linkLabel.TabIndex = 17;
			activity.linkLabel.TabStop = true;
			activity.linkLabel.Text = "";
			activity.linkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			//		activity.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
	

			/*activity.image.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			activity.image.Location = new System.Drawing.Point(pictureBox5.Right + 110, pictureBox5.Bottom - 75);
			activity.image.Name = "image1";
			activity.image.TabStop = false;
			activity.image.SizeMode = PictureBoxSizeMode.Normal;
			activity.image.Size = new System.Drawing.Size(10, 10);
			activity.image.Image = close.Image;
			activity.image.Click += new System.EventHandler(this.close_Click);
			activity.image.Cursor = System.Windows.Forms.Cursors.Hand;

*/
			//this.Controls.Add(activity.label);
			//this.Controls.Add(activity.linkLabel);
			//this.Controls.Add(activity.image);

			//activity.linkLabel.BringToFront();

			//fileActivity.Add(activity);

			//this.dataTransferForm.AddNewTransfer(activity);
			//this.dataTransferForm.Show();

			//linkLabel1.Top = 296;
			//linkLabel1.BringToFront();
			//activityPosition.Start();

			//this.dataTransferForm.AddNewTransfer(activity);
		}


		public void ReciveNewFile(Socket reciveConnection, string fileTransferId, bool viaServer)
		{
			if(viaServer)
			{
				byte [] byteBuffer = Encoding.UTF8.GetBytes("FILE_TRANSFER_ID " + fileTransferId + "\n\r");
				reciveConnection.Send(byteBuffer, 0, byteBuffer.Length, SocketFlags.None); 
			}

			ReciveFile reciveFile = new ReciveFile(reciveConnection, fileTransferId);	
			reciveFile.SendFileChangeEvent += new DGP_Messenger.ReciveFile.SendFileStatus(reciveFile_SendFileChangeEvent);
			
			this.Invoke(new NewActivityDelegate(this.NewActivity), new object [] { reciveFile});
			//	this.NewActivity(reciveFile);
		}
	
		public void SendFileTo(string ip, string fileTransferId)
		{
			Socket sendFileSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			bool viaServer = false;	

			try
			{
				//throw new SocketException(0);
				//DisplayMessage(IPAddress.Parse(ip).ToString() + '\n', Color.Red, "", false);
				IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(ip), 4046);
				DisplayMessage("Försöker kontakta klienten på ip: " + ip, Color.Red, "", false);
				sendFileSocket.Connect(remoteEP);
			}
			catch(SocketException)
			{
				DisplayMessage("Försöket Misslyckades! \n Skicka filen via servern!", Color.Red, "", false);

				IPEndPoint remoteEP = new IPEndPoint(Dns.Resolve("siti.mine.nu").AddressList[0], 4045);
				sendFileSocket.Connect(remoteEP);

				byte [] byteBuffer = Encoding.UTF8.GetBytes("FILE_TRANSFER_ID " + fileTransferId + "\n\r");
				sendFileSocket.Send(byteBuffer);//, 0, byteBuffer.Length, SocketFlags.None, null, null); 

				this.SendMessage("SEND_FILE_VIA_SERVER" + this.contact.Id + '\r' + fileTransferId);
				viaServer = true;
			}

			//sendFileSocket.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendFileSocketDataRecived), null);
			//sendFileSocket.Send(Encoding.UTF8.GetBytes("SEND_FILE" + "\n\r"));
			DisplayMessage("Kontakt med klienten! \nVäntar på att han/hon ska acceptera!", Color.Red, "", false);
			SendFile sendFile = new SendFile(sendFileSocket, string.Copy(openFileDialog.FileName), fileTransferId, viaServer);
			sendFile.SendFileChangeEvent += new DGP_Messenger.SendFile.SendFileStatus(sendFile_SendFileChangeEvent);

			NewActivity(sendFile);
		}


		/*		private void UpdateFiletransferData(FileTransfer transFile)
				{
					string sendReciveState;
					if(transFile.TransferStatus == TransferStatus.FILE_SENDING)
					{
						sendReciveState = "Sending ";
					}
					else if(transFile.TransferStatus == TransferStatus.FILE_RECIVING)
					{
						sendReciveState = "Reciving ";
					}
					else
					{
						sendReciveState = "";
					}
					long readBytes = transFile.TransferedBytes / 1000;
					long allBytes =  transFile.FileSize / 1000;
					short percentDone = (short)(((decimal)transFile.TransferedBytes/(decimal)transFile.FileSize)*100);
					for(int i = 0; i < fileActivity.Count; i++)
					{
						Activity ac = (Activity)fileActivity[i];
						if(transFile.Equals(ac.fileTransfer))
						{
							ac.Text = sendReciveState + "File: " + transFile.FileName + "\n"+ readBytes + "kB / " + allBytes + "kB\n "	+ percentDone + "%";
						}
					}

					dataTransferForm.UpdateFileTransfer(transFile);
				}
				*/

	
		private void reciveFile_SendFileChangeEvent(object sender, SendFileEvent sfe)
		{
			this.Invoke(new UpdateReciveActivityDelegate(this.UpdateReciveActivity), new object[] { (FileTransfer)sender, sfe}); 
		}

		private void sendFile_SendFileChangeEvent(object sender, SendFileEvent sfe)
		{
			this.Invoke(new UpdateSendActivityDelegate(this.UpdateSendActivity), new object[] { (FileTransfer)sender, sfe}); 		
		}

		private void UpdateReciveActivity(FileTransfer fileTransfer, SendFileEvent sfe)
		{	
			ReciveFile reciveFile  = (ReciveFile)fileTransfer;
			if(sfe.fileSendStatus == TransferStatus.CONFIRM_RECIVE)
			{
				//UpdateFiletransferData(fileTransfer);

				for(int i = 0; i < fileActivity.Count; i++)
				{
					Activity ac = (Activity)fileActivity[i];
					if(fileTransfer.Equals(ac.fileTransfer))
					{
						ac.linkLabel.Text = "Spara filen!";
					}
				}

				DisplayMessage(this.contact.Name + " Vill skicka filen \"" + reciveFile.FileName.Trim() + "\" till dig. " + 
							"Klicka på transfersknappen för att visa fildelningsfönstret där du kan välja att spara filen.", Color.SkyBlue, "", false);
			}
			else if(sfe.fileSendStatus == TransferStatus.FILE_RECIVING)
			{	
			//	UpdateFiletransferData(fileTransfer);

				DisplayMessage("recived bytes" + fileTransfer.TransferedBytes, Color.SaddleBrown, "", false);

				//this.Invoke(new DisplayMessageDelegate(this.DisplayMessage), new  object []{"Recived Bytes: " + reciveFile.RecivedBytes.ToString() + "/" + reciveFile.FileSize, Color.SaddleBrown, "", false});
			}
			else if(sfe.fileSendStatus == TransferStatus.RECIVE_OK)
			{
				DisplayMessage("Recive OK", Color.SaddleBrown, "", false);
				reciveFile.close();

			//	UpdateFiletransferData(fileTransfer);
			}
			else if(sfe.fileSendStatus == TransferStatus.SOCKET_EXCEPTION)
			{
			}
			//		else
			{
				DisplayMessage("Recive event: " + sfe.fileSendStatus.ToString() + '\n', Color.SaddleBrown, "", false);
			}
		}

		public void UpdateSendActivity(FileTransfer fileTransfer, SendFileEvent sfe)
		{
			SendFile sendFile = (SendFile)fileTransfer;
			if(sfe.fileSendStatus == TransferStatus.START_SEND)
			{
				DisplayMessage("Sending file: ", Color.SaddleBrown, "", false);
				
			//	UpdateFiletransferData(fileTransfer);
			}
			else if(sfe.fileSendStatus == TransferStatus.FILE_SENDING)
			{
				//string tempString = "Sending Bytes: " + sendFile.ReadBytes.ToString() + "/" + sendFile.FileSize.ToString();
			//	UpdateFiletransferData(fileTransfer);
				
				//this.Invoke(new DisplayMessageDelegate(this.DisplayMessage), new  object []{tempString, Color.SaddleBrown, "", false});
			}
			else if(sfe.fileSendStatus == TransferStatus.SEND_OK)
			{
				DisplayMessage("Send OK!", Color.SaddleBrown, "", false);
				
			//	UpdateFiletransferData(fileTransfer);
				//label3.Text = "Sending Bytes: Done!";
				//sendFile.close();
			}
			else if(sfe.fileSendStatus == TransferStatus.SOCKET_EXCEPTION)
			{
			}
			//	else
			{
				DisplayMessage("Send event: " + sfe.fileSendStatus.ToString() + '\n', Color.SaddleBrown, "", false);
			}
		}


		private void fileTransfer_TransferDoneEvent(object sender, EventArgs sfe)
		{
			FileTransfer transfer = sender as FileTransfer;
			DisplayMessage("\nÖverföringen av " + transfer.FileName + " är nu färdig.\n"); 

		}



		/* private void activityPosition_Tick(object sender, System.EventArgs e)
		{
			int maxTop = 96;
			bool endDraw = false;
			for(int i = 0; i < fileActivity.Count; i++)
			{
				Activity ac = (Activity)fileActivity[i];

				int posChange = 7;
				if(ac.Top > (maxTop+posChange))
				{
					//DisplayMessage("Tick: " + ac.linkLabel.Top + "    : " + ac.linkLabel.Text, Color.Red, "", false);
					ac.Top = ac.Top - posChange;
					
					//activityPosition.Interval = activityPosition.Interval + 1;
				}
				else
				{
					ac.Top = maxTop;
					//activityPosition.Stop();
					//activityPosition.Interval = 20;
				}

				if(endDraw || i < startDrawActivity)
				{
					ac.linkLabel.SendToBack();
					ac.label.SendToBack();
					ac.image.SendToBack();
				}
				else
				{
					if(ac.linkLabel.Top > pictureBox5.Bottom - 80)
					{
						ac.linkLabel.SendToBack();
						ac.image.SendToBack();
					}
					else
					{
						ac.label.Top = ac.Top;
						ac.linkLabel.Top = ac.Top;
						ac.image.Top = ac.Top + 2;
						ac.image.Left = ac.linkLabel.Right - 12;

						if(ac.fileTransfer.TransferStatus == TransferStatus.CONFIRM_RECIVE)
						{
							ac.label.SendToBack();
							ac.linkLabel.BringToFront();
						}
						else
						{
							ac.linkLabel.SendToBack();
							ac.label.BringToFront();
						}
						

						ac.image.BringToFront();
					}

					ac.linkLabel.Refresh();
					ac.label.Refresh();
					ac.image.Refresh();

					maxTop = maxTop + ac.linkLabel.Height + 4;
					if(maxTop + ac.linkLabel.Height > pictureBox3.Bottom)
					{
						endDraw = true;
					}
				}
			}
		}

		private void linkLabel2_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			if(startDrawActivity < fileActivity.Count-1)
			{
				startDrawActivity++;
			}
		}

		private void linkLabel3_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			if(startDrawActivity > 0)
			{
				startDrawActivity--;
			}
		}

		private void close_Click(object sender, System.EventArgs e)
		{
			//DisplayMessage("Click: " + sender.ToString(), Color.Red, "", false);
			bool breakIt = false;
			for(int i = 0; i < fileActivity.Count && !breakIt; i++)
			{
				Activity ac = (Activity)fileActivity[i];
				if(sender.Equals(ac.image))
				{
					fileActivity.RemoveAt(i);
					ac.linkLabel.Dispose();
					//ac.label.Dispose();
					//ac.image.Dispose();
					ac.fileTransfer.close();
					breakIt = true;
				}
			}
		}
*/


		#endregion

		private void FileTransferHandler_InvitationReceived(FileTransferHandler sender, FileTransferInvitationEventArgs e)
		{
			//e.FileTransfer.
			MessageBox.Show(e.FileTransfer.Sender.Name + " vill Skicka filen: " + e.FileTransfer.FileName + "! \nStorlek: " + e.FileTransfer.FileSize.ToString() + "B");
			//e.FileTransfer.
		}
	}

	public delegate void MessageFormClosedEventHandler(object sender, MessageFormClosedEventArgs e);
}
