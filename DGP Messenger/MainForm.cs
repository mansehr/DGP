using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using DotMSN;

using Color = System.Drawing.Color;

namespace DGP_Messenger
{

	enum Protokoll { DGP = 0, MSN, CHAT, ALL };
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		#region Delegates
		// Delegates

		private delegate MessageForm CreateNewMessageFormDelegate(User user, string contactId, Socket socket, bool activate);
		private delegate void ClientMessageRecivedDelegate(string message);

		private delegate void ShowLoggingInLayoutDelegate();
		private delegate void ShowLoggedInLayoutDelegate();
		private delegate void LogginDelegate(string name, string password);
		private delegate void LogginChatDelegate(string name, string password, string adress);
		private delegate void UpdateContactDelegate(Contact contact);
		private delegate MessageForm NewMsnMessageFormDelegate(Conversation conv, string mail);
		private delegate MessageForm NewMessageFormDelegate(string mail);

		private delegate void VoidDelegate();


		#endregion

		#region Class Members
	
		private ArrayList messageForms	= new ArrayList();	// Array with all the msnMessengerforms open	
		private ArrayList Contacts = new ArrayList();

		private LogginDialog logginDialog = null;

		private DataTransferForm dataTransferForm;

		private DotMSN.Messenger			msnMessenger;
		private DGP_Messenger.DGPMessenger	dgpMessenger;
		private DGP_Messenger.DGPMessenger	chatMessenger;
		private bool loggingInMSN = false;
		private bool loggingInDGP = false;
		private bool loggingInChat = false;
		private MessageForm publicChatForm;
//		private Thread logginMSNThread;

		private bool forcedClosed = false;

		MainFormSettingsValues _settings;

		//Windows form stuff
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.MenuItem menuItem5;

		
		private System.Windows.Forms.NotifyIcon NotifyIcon;
		private System.Windows.Forms.ContextMenu NotifyIconMenu;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem menuItem18;
		private System.Windows.Forms.MenuItem menuItem19;
		private System.Windows.Forms.MenuItem menuItem20;
		private System.Windows.Forms.MenuItem menuItem21;
		private System.Windows.Forms.MenuItem menuItem22;
		private System.Windows.Forms.MenuItem menuItem23;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox pictureBox3;
		private System.Windows.Forms.PictureBox pictureBox5;
		private System.Windows.Forms.PictureBox LogginButton;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.MenuItem menuItem25;
		private System.Windows.Forms.MenuItem menuItem26;
		private System.Windows.Forms.MenuItem menuItem27;
		private System.Windows.Forms.PictureBox pictureBox4;
		private System.Windows.Forms.MenuItem setHidden;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem24;
		private System.Windows.Forms.MenuItem menuItem28;
		private System.Windows.Forms.MenuItem menuItem29;
		private System.Windows.Forms.MenuItem menuItem30;
		private System.Windows.Forms.MenuItem menuItem31;
		private DGP_Messenger.RoundedcornerPanel roundedcornerPanel1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.MenuItem addMsnFriendMenuItem;
		private System.Windows.Forms.MenuItem menuItemLoggOfMSN;
		private System.Windows.Forms.MenuItem menuItemLoggOfDGP;
		private System.Windows.Forms.MenuItem menuItem32;
		private System.Windows.Forms.MenuItem menuItem33;
		private System.Windows.Forms.PictureBox pictureBox6;
		private System.Windows.Forms.PictureBox pictureBox7;
		private System.Windows.Forms.Label label3;
		private System.ComponentModel.IContainer components;

		#endregion

		#region Constructor and Dispose

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);

			//this.PasswordField.Text = "tjollahopp23";
			this.LogginButton.Focus();

			//this.BackgroundImage.Flags;

			this.dataTransferForm = new DataTransferForm();
			//this.dataTransferForm.Visible = false;

//			msnMessenger.ErrorReceived +=new DotMSN.Messenger.ErrorReceivedHandler(msnMessenger_ErrorReceived);
//			msnMessenger.ConnectionFailure += new DotMSN.Messenger.ConnectionFailureHandler(msnMessenger_ConnectionFailure);
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			Loggout(Protokoll.ALL);
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.label1 = new System.Windows.Forms.Label();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem17 = new System.Windows.Forms.MenuItem();
			this.menuItem18 = new System.Windows.Forms.MenuItem();
			this.menuItem19 = new System.Windows.Forms.MenuItem();
			this.menuItem20 = new System.Windows.Forms.MenuItem();
			this.menuItem21 = new System.Windows.Forms.MenuItem();
			this.menuItem22 = new System.Windows.Forms.MenuItem();
			this.menuItem23 = new System.Windows.Forms.MenuItem();
			this.setHidden = new System.Windows.Forms.MenuItem();
			this.menuItem29 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItemLoggOfDGP = new System.Windows.Forms.MenuItem();
			this.menuItemLoggOfMSN = new System.Windows.Forms.MenuItem();
			this.menuItem30 = new System.Windows.Forms.MenuItem();
			this.menuItem25 = new System.Windows.Forms.MenuItem();
			this.menuItem26 = new System.Windows.Forms.MenuItem();
			this.menuItem27 = new System.Windows.Forms.MenuItem();
			this.addMsnFriendMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem16 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.NotifyIconMenu = new System.Windows.Forms.ContextMenu();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem24 = new System.Windows.Forms.MenuItem();
			this.menuItem31 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this.menuItem15 = new System.Windows.Forms.MenuItem();
			this.menuItem28 = new System.Windows.Forms.MenuItem();
			this.menuItem32 = new System.Windows.Forms.MenuItem();
			this.menuItem33 = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.label2 = new System.Windows.Forms.Label();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			this.pictureBox5 = new System.Windows.Forms.PictureBox();
			this.LogginButton = new System.Windows.Forms.PictureBox();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.pictureBox4 = new System.Windows.Forms.PictureBox();
			this.roundedcornerPanel1 = new DGP_Messenger.RoundedcornerPanel();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.pictureBox6 = new System.Windows.Forms.PictureBox();
			this.pictureBox7 = new System.Windows.Forms.PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.DimGray;
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(16, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(264, 48);
			this.label1.TabIndex = 1;
			this.label1.Text = "Inloggad: ";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.label1.Visible = false;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1,
																					  this.menuItem25,
																					  this.menuItem6});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem5,
																					  this.menuItem17,
																					  this.menuItem29,
																					  this.menuItem2,
																					  this.menuItem30});
			this.menuItem1.Text = "Meny";
			// 
			// menuItem5
			// 
			this.menuItem5.Enabled = false;
			this.menuItem5.Index = 0;
			this.menuItem5.Text = "Settings";
			this.menuItem5.Click += new System.EventHandler(this.menuItemSettings_Click);
			// 
			// menuItem17
			// 
			this.menuItem17.Enabled = false;
			this.menuItem17.Index = 1;
			this.menuItem17.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuItem18,
																					   this.menuItem19,
																					   this.menuItem20,
																					   this.menuItem21,
																					   this.menuItem22,
																					   this.menuItem23,
																					   this.setHidden});
			this.menuItem17.Text = "Status";
			// 
			// menuItem18
			// 
			this.menuItem18.Index = 0;
			this.menuItem18.Text = "Online";
			this.menuItem18.Click += new System.EventHandler(this.setOnline_Click);
			// 
			// menuItem19
			// 
			this.menuItem19.Index = 1;
			this.menuItem19.Text = "Strax Tillbaka";
			this.menuItem19.Click += new System.EventHandler(this.setBrb_Click);
			// 
			// menuItem20
			// 
			this.menuItem20.Index = 2;
			this.menuItem20.Text = "Inte Vid Datorn";
			this.menuItem20.Click += new System.EventHandler(this.setAway_Click);
			// 
			// menuItem21
			// 
			this.menuItem21.Index = 3;
			this.menuItem21.Text = "Äter Mat";
			this.menuItem21.Click += new System.EventHandler(this.setEating_Click);
			// 
			// menuItem22
			// 
			this.menuItem22.Index = 4;
			this.menuItem22.Text = "Prata I Telefon";
			this.menuItem22.Click += new System.EventHandler(this.setPhone_Click);
			// 
			// menuItem23
			// 
			this.menuItem23.Index = 5;
			this.menuItem23.Text = "Upptagen";
			this.menuItem23.Click += new System.EventHandler(this.setBusy_Click);
			// 
			// setHidden
			// 
			this.setHidden.Index = 6;
			this.setHidden.Text = "Göm Mig!";
			this.setHidden.Click += new System.EventHandler(this.setHidden_Click);
			// 
			// menuItem29
			// 
			this.menuItem29.Index = 2;
			this.menuItem29.Text = "-";
			// 
			// menuItem2
			// 
			this.menuItem2.Enabled = false;
			this.menuItem2.Index = 3;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItemLoggOfDGP,
																					  this.menuItemLoggOfMSN});
			this.menuItem2.Text = "Logga Ut";
			this.menuItem2.Click += new System.EventHandler(this.menuItemLoggOut_Click);
			// 
			// menuItemLoggOfDGP
			// 
			this.menuItemLoggOfDGP.Enabled = false;
			this.menuItemLoggOfDGP.Index = 0;
			this.menuItemLoggOfDGP.Text = "DGP";
			this.menuItemLoggOfDGP.Click += new System.EventHandler(this.menuItemLoggOfDGP_Click);
			// 
			// menuItemLoggOfMSN
			// 
			this.menuItemLoggOfMSN.Enabled = false;
			this.menuItemLoggOfMSN.Index = 1;
			this.menuItemLoggOfMSN.Text = "MSN";
			this.menuItemLoggOfMSN.Click += new System.EventHandler(this.menuItemLoggOfMSN_Click);
			// 
			// menuItem30
			// 
			this.menuItem30.Index = 4;
			this.menuItem30.Text = "Avsluta";
			this.menuItem30.Click += new System.EventHandler(this.menuItemExit_Click);
			// 
			// menuItem25
			// 
			this.menuItem25.Enabled = false;
			this.menuItem25.Index = 1;
			this.menuItem25.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuItem26,
																					   this.menuItem27,
																					   this.addMsnFriendMenuItem});
			this.menuItem25.Text = "Extra";
			// 
			// menuItem26
			// 
			this.menuItem26.Index = 0;
			this.menuItem26.Text = "Editor";
			this.menuItem26.Click += new System.EventHandler(this.menuItem26_Click);
			// 
			// menuItem27
			// 
			this.menuItem27.Index = 1;
			this.menuItem27.Text = "Data Transfer";
			this.menuItem27.Click += new System.EventHandler(this.menuItemShowDataTransfer_Click);
			// 
			// addMsnFriendMenuItem
			// 
			this.addMsnFriendMenuItem.Index = 2;
			this.addMsnFriendMenuItem.Text = "Add MSN Friend";
			this.addMsnFriendMenuItem.Click += new System.EventHandler(this.addMsnFriendMenuItem_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 2;
			this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem16});
			this.menuItem6.Text = "Hjälp";
			// 
			// menuItem16
			// 
			this.menuItem16.Index = 0;
			this.menuItem16.Text = "Om DGP Messenger";
			this.menuItem16.Click += new System.EventHandler(this.menuItemAbout_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 4;
			this.menuItem4.Text = "-";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 6;
			this.menuItem3.Text = "Avsluta";
			this.menuItem3.Click += new System.EventHandler(this.menuItemExit_Click);
			// 
			// listBox1
			// 
			this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.listBox1.BackColor = System.Drawing.Color.White;
			this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.listBox1.ForeColor = System.Drawing.Color.Transparent;
			this.listBox1.ItemHeight = 15;
			this.listBox1.Location = new System.Drawing.Point(32, 128);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(232, 208);
			this.listBox1.TabIndex = 2;
			this.listBox1.Visible = false;
			this.listBox1.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
			this.listBox1.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.listBox1_MeasureItem);
			this.listBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox1_DrawItem);
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(0, 0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(303, 40);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBox1.TabIndex = 19;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
			// 
			// NotifyIcon
			// 
			this.NotifyIcon.ContextMenu = this.NotifyIconMenu;
			this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
			this.NotifyIcon.Text = "DGP Messenger";
			this.NotifyIcon.DoubleClick += new System.EventHandler(this.NotifyIcon_DoubleClick);
			// 
			// NotifyIconMenu
			// 
			this.NotifyIconMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.menuItem9,
																						   this.menuItem24,
																						   this.menuItem31,
																						   this.menuItem7,
																						   this.menuItem4,
																						   this.menuItem28,
																						   this.menuItem3});
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 0;
			this.menuItem9.Text = "Visa Fönstret";
			this.menuItem9.Click += new System.EventHandler(this.NotifyIcon_DoubleClick);
			// 
			// menuItem24
			// 
			this.menuItem24.Index = 1;
			this.menuItem24.Text = "-";
			// 
			// menuItem31
			// 
			this.menuItem31.Enabled = false;
			this.menuItem31.Index = 2;
			this.menuItem31.Text = "Settings";
			this.menuItem31.Click += new System.EventHandler(this.menuItemSettings_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Enabled = false;
			this.menuItem7.Index = 3;
			this.menuItem7.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem10,
																					  this.menuItem11,
																					  this.menuItem12,
																					  this.menuItem13,
																					  this.menuItem14,
																					  this.menuItem15});
			this.menuItem7.Text = "Status";
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 0;
			this.menuItem10.Text = "Online";
			this.menuItem10.Click += new System.EventHandler(this.setOnline_Click);
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 1;
			this.menuItem11.Text = "Strax Tillbaka";
			this.menuItem11.Click += new System.EventHandler(this.setBrb_Click);
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 2;
			this.menuItem12.Text = "Inte Vid Datorn";
			this.menuItem12.Click += new System.EventHandler(this.setAway_Click);
			// 
			// menuItem13
			// 
			this.menuItem13.Index = 3;
			this.menuItem13.Text = "Äter Mat";
			this.menuItem13.Click += new System.EventHandler(this.setEating_Click);
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 4;
			this.menuItem14.Text = "Prata I Telefon";
			this.menuItem14.Click += new System.EventHandler(this.setPhone_Click);
			// 
			// menuItem15
			// 
			this.menuItem15.Index = 5;
			this.menuItem15.Text = "Upptagen";
			this.menuItem15.Click += new System.EventHandler(this.setBusy_Click);
			// 
			// menuItem28
			// 
			this.menuItem28.Enabled = false;
			this.menuItem28.Index = 5;
			this.menuItem28.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuItem32,
																					   this.menuItem33});
			this.menuItem28.Text = "Logga Ut";
			this.menuItem28.Click += new System.EventHandler(this.menuItemLoggOut_Click);
			// 
			// menuItem32
			// 
			this.menuItem32.Index = 0;
			this.menuItem32.Text = "DGP";
			this.menuItem32.Click += new System.EventHandler(this.menuItemLoggOfDGP_Click);
			// 
			// menuItem33
			// 
			this.menuItem33.Index = 1;
			this.menuItem33.Text = "MSN";
			this.menuItem33.Click += new System.EventHandler(this.menuItemLoggOfMSN_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = -1;
			this.menuItem8.Text = "-";
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label2.BackColor = System.Drawing.Color.Lavender;
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label2.Font = new System.Drawing.Font("Verdana", 16F);
			this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.label2.Location = new System.Drawing.Point(40, 104);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(224, 88);
			this.label2.TabIndex = 20;
			this.label2.Text = "Loggar In På: MSN DGP ";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label2.Visible = false;
			// 
			// pictureBox3
			// 
			this.pictureBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox3.BackColor = System.Drawing.Color.White;
			this.pictureBox3.Location = new System.Drawing.Point(0, 40);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(304, 4);
			this.pictureBox3.TabIndex = 22;
			this.pictureBox3.TabStop = false;
			// 
			// pictureBox5
			// 
			this.pictureBox5.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.pictureBox5.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
			this.pictureBox5.Location = new System.Drawing.Point(8, 160);
			this.pictureBox5.Name = "pictureBox5";
			this.pictureBox5.Size = new System.Drawing.Size(112, 80);
			this.pictureBox5.TabIndex = 24;
			this.pictureBox5.TabStop = false;
			// 
			// LogginButton
			// 
			this.LogginButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.LogginButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.LogginButton.Image = ((System.Drawing.Image)(resources.GetObject("LogginButton.Image")));
			this.LogginButton.Location = new System.Drawing.Point(88, 264);
			this.LogginButton.Name = "LogginButton";
			this.LogginButton.Size = new System.Drawing.Size(110, 45);
			this.LogginButton.TabIndex = 25;
			this.LogginButton.TabStop = false;
			this.LogginButton.Click += new System.EventHandler(this.LoginButton_Click);
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
			// pictureBox4
			// 
			this.pictureBox4.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox4.BackgroundImage")));
			this.pictureBox4.Location = new System.Drawing.Point(0, 0);
			this.pictureBox4.Name = "pictureBox4";
			this.pictureBox4.Size = new System.Drawing.Size(304, 440);
			this.pictureBox4.TabIndex = 23;
			this.pictureBox4.TabStop = false;
			// 
			// roundedcornerPanel1
			// 
			this.roundedcornerPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.roundedcornerPanel1.BackColor = System.Drawing.Color.Transparent;
			this.roundedcornerPanel1.ForeColor = System.Drawing.Color.White;
			this.roundedcornerPanel1.Location = new System.Drawing.Point(15, 107);
			this.roundedcornerPanel1.Name = "roundedcornerPanel1";
			this.roundedcornerPanel1.Size = new System.Drawing.Size(267, 243);
			this.roundedcornerPanel1.TabIndex = 26;
			this.roundedcornerPanel1.Visible = false;
			// 
			// pictureBox6
			// 
			this.pictureBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox6.BackColor = System.Drawing.Color.White;
			this.pictureBox6.Location = new System.Drawing.Point(0, 248);
			this.pictureBox6.Name = "pictureBox6";
			this.pictureBox6.Size = new System.Drawing.Size(304, 4);
			this.pictureBox6.TabIndex = 27;
			this.pictureBox6.TabStop = false;
			// 
			// pictureBox7
			// 
			this.pictureBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox7.BackColor = System.Drawing.Color.White;
			this.pictureBox7.Location = new System.Drawing.Point(0, 144);
			this.pictureBox7.Name = "pictureBox7";
			this.pictureBox7.Size = new System.Drawing.Size(304, 4);
			this.pictureBox7.TabIndex = 28;
			this.pictureBox7.TabStop = false;
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(176, 184);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 32);
			this.label3.TabIndex = 29;
			this.label3.Text = "Dgp Messenger!     The only there is.";
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(246)), ((System.Byte)(246)), ((System.Byte)(247)));
			this.ClientSize = new System.Drawing.Size(296, 409);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.LogginButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.pictureBox7);
			this.Controls.Add(this.pictureBox6);
			this.Controls.Add(this.pictureBox5);
			this.Controls.Add(this.pictureBox3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.roundedcornerPanel1);
			this.Controls.Add(this.pictureBox4);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu1;
			this.Name = "MainForm";
			this.Text = "DGP Messenger";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties

		public MainFormSettingsValues Settings
		{
			get { return _settings; }
		}

		#endregion

		#region Private Methods		//////////////////////////

		private void Loggin()
		{
			logginDialog = new LogginDialog();

			if(logginDialog.ShowDialog(this) == DialogResult.OK)
			{
				loggingInDGP = logginDialog.DgpLoggin;
				loggingInMSN = logginDialog.MsnLoggin;
				loggingInChat	 = logginDialog.ChatLoggin;

				this.ShowLoggingInLayout();
				//this.Invoke(new ShowLoggingInLayoutDelegate(this.ShowLoggingInLayout), null );
				
				if(loggingInDGP)
				{
					dgpMessenger = new DGPMessenger();
					loggingInDGP = true;
					this.LogginDgp(logginDialog.DgpName, logginDialog.DgpPassword);
					//this.Invoke(new LogginDelegate(LogginDgp), new object [] {logginDialog.DgpName, logginDialog.DgpPassword});
				}
				if(loggingInMSN)
				{	
					msnMessenger = new Messenger();
					loggingInMSN = true;
					this.LogginMsn(logginDialog.MsnName, logginDialog.MsnPassword);
					//this.Invoke(new LogginDelegate(LogginMsn), new object [] {logginDialog.MsnName, logginDialog.MsnPassword});
				}
				if(loggingInChat)
				{
					chatMessenger = new DGPMessenger();
					loggingInChat = true;
					//this.Invoke(new LogginChatDelegate(LogginChat), new object [] {logginDialog.ChatName, "", logginDialog.ChatServer});
					this.LogginChat(logginDialog.ChatName, "", logginDialog.ChatServer, 1080);
				}
			}
			logginDialog = null;
		}

		private void LogginDgp(string name, string password)
		{
			menuItemLoggOfDGP.Enabled = true;
			dgpMessenger = new DGPMessenger();
			try
			{
				dgpMessenger.logginErrorEvent += new DGP_Messenger.DGPMessenger.LogginErrorEvent(dgpMessenger_logginErrorEvent);
				dgpMessenger.logginComplete += new DGP_Messenger.DGPMessenger.LogginComplete(dgpMessenger_logginComplete);
				dgpMessenger.updateContact += new DGP_Messenger.DGPMessenger.UpdateContactEvent(dgpMessenger_updateContact);
				dgpMessenger.recivedMessage += new DGP_Messenger.DGPMessenger.RecivedMessageEvent(dgpMessenger_recivedMessage);
				dgpMessenger.Connect(name, password);
			}
			catch(Exception)
			{
				MessageBox.Show("Något gick fel vi inloggninen till DGP!");
				Loggout(Protokoll.DGP);
			}
		}

		private void LogginMsn(string name, string password)
		{
			menuItemLoggOfMSN.Enabled = true;
			msnMessenger = new Messenger();
			try
			{	
				msnMessenger.ConnectionFailure += new DotMSN.Messenger.ConnectionFailureHandler(msnMessenger_ConnectionFailure);
				msnMessenger.MailboxStatus += new DotMSN.Messenger.MailboxStatusHandler(msnMessenger_MailboxStatus);
				msnMessenger.ErrorReceived += new DotMSN.Messenger.ErrorReceivedHandler(msnMessenger_ErrorReceived);
				msnMessenger.ListReceived += new DotMSN.Messenger.ListReceivedHandler(msnMessenger_ListReceived);
				// Event när contact har lagts till..
				msnMessenger.ContactAdded += new DotMSN.Messenger.ContactAddedHandler(msnMessenger_ContactAdded);
				// setup the callbacks
				// we log when someone goes online
				msnMessenger.ContactOnline += new Messenger.ContactOnlineHandler(ContactOnline);

				msnMessenger.ContactOffline += new DotMSN.Messenger.ContactOfflineHandler(ContactOffline);

				// we want to do something when we have a conversation
				msnMessenger.ConversationCreated += new DotMSN.Messenger.ConversationCreatedHandler(msnMessenger_ConversationCreated);

				// notify us when synchronization is completed
				msnMessenger.SynchronizationCompleted += new Messenger.SynchronizationCompletedHandler(OnSynchronizationCompleted);
				
				
				
				DotMSN.Owner owner = new DotMSN.Owner(logginDialog.MsnName, logginDialog.MsnPassword);
				
				owner.ScreenNameChanged += new DotMSN.Contact.ScreenNameChangedHandler(Owner_ScreenNameChanged);
				owner.StatusChanged		+= new DotMSN.Contact.StatusChangedHandler(Owner_StatusChanged);

				DotMSN.Connection connection = new DotMSN.Connection("64.4.13.58", 1863);

				// everything is setup, now connect to the msnMessenger service
				msnMessenger.Connect(connection, owner);	
				

				msnMessenger.InitialStatus = MSNStatus.Online;

				
				//Log.Text += "Connected!\r\n";
				
				// synchronize the whole list.
				// remember you can only do this once per session!
				// after synchronizing the initial status will be set.
				msnMessenger.SynchronizeList();
					

				/* uncomment this when you want to automatically add
					  people who have added you to their contactlist on your own
					  contactlist. (remember the pop-up dialog in MSN Messenger client when someone adds you, this is the 'automatic' method)					 
				*/	foreach(Contact contact in
						msnMessenger.GetListEnumerator(MSNList.ReverseList))
					{						
						msnMessenger.AddContact(contact.Mail);
					}
					
			}
			catch(MSNException)
			{
				MessageBox.Show(this, "Problem vid MSN inloggingen!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
				Loggout(Protokoll.ALL);
				// in case of an error, report this to the user (or developer)
				//MessageBox.Show(this, "Connecting failed: " + e.ToString());
			}
		}

		private void LogginChat(string name, string password, string adress, short port)
		{
			chatMessenger = new DGPMessenger();
			try
			{
				chatMessenger.logginErrorEvent += new DGP_Messenger.DGPMessenger.LogginErrorEvent(chatMessenger_logginErrorEvent);
				chatMessenger.logginComplete += new DGP_Messenger.DGPMessenger.LogginComplete(chatMessenger_logginComplete);
				chatMessenger.updateContact += new DGP_Messenger.DGPMessenger.UpdateContactEvent(chatMessenger_updateContact);
				chatMessenger.recivedMessage += new DGP_Messenger.DGPMessenger.RecivedMessageEvent(chatMessenger_recivedMessage);
				chatMessenger.Connect(name, password, adress, port);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Något gick fel vi inloggninen till Chatten! \n Felmeddelande: " + ex.Message);
				Loggout(Protokoll.CHAT);
			}
		}

		private void Loggout(Protokoll protokoll)
		{
			bool disconnectAll = false;

			if(protokoll == Protokoll.ALL)
			{
				disconnectAll = true;
				menuItem2.Enabled = false;
			}

			if(dgpMessenger != null && (disconnectAll || protokoll == Protokoll.DGP))
			{
				dgpMessenger.Disconnect();
				//Contacts.Clear();
				loggingInDGP = false;
				menuItemLoggOfDGP.Enabled = false;
				for(int i = 0; i < Contacts.Count; i++)
				{
					DGPContact con = Contacts[i] as DGPContact;
					if(con.IsMSNContact == false)
					{
						Contacts.Remove(con);
						i--;
					}	
				}
			}

			if(chatMessenger != null && (disconnectAll || protokoll == Protokoll.CHAT))
			{
				chatMessenger.Disconnect();
				loggingInChat = false;
				//menuItemLoggOfDGP.Enabled = false;
				/*for(int i = 0; i < Contacts.Count; i++)
				{
					DGPContact con = Contacts[i] as DGPContact;
					if(con.IsMSNContact == false)
					{
						Contacts.Remove(con);
						i--;
					}	
				}*/
			}

			if(msnMessenger != null && (disconnectAll || protokoll == Protokoll.MSN))
			{
				msnMessenger.CloseConnection();
				loggingInMSN = false;
				menuItemLoggOfMSN.Enabled = false;
				for(int i = 0; i < Contacts.Count; i++)
				{
					DGPContact con = Contacts[i] as DGPContact;
					if(con.IsMSNContact)
					{
						Contacts.Remove(con);
						i--;
					}	
				}
			}
				
			foreach(MessageForm mf in messageForms)
			{
				mf.SocketDisconnect();
			}


			if( disconnectAll	||	
				((msnMessenger == null	|| msnMessenger.Connected == false)		&&
				(dgpMessenger == null	|| dgpMessenger.Connected == false)		&&
				(chatMessenger == null	|| chatMessenger.Connected == false)		&& 
				loggingInMSN == false && loggingInDGP == false	&& loggingInChat == false))
			{
				Contacts.Clear();
				this.SuspendLayout();
				label1.Hide();
				label2.Hide();
				listBox1.Hide();
				roundedcornerPanel1.Hide();
				listBox1.Items.Clear();
				LogginButton.Show();
				LogginButton.BringToFront();
				label3.Show();
				label3.BringToFront();
				this.ResumeLayout();

				mainMenu1.MenuItems[0].MenuItems[0].Enabled = false;
				mainMenu1.MenuItems[0].MenuItems[1].Enabled = false;
				mainMenu1.MenuItems[1].Enabled = false;
				mainMenu1.MenuItems[2].Enabled = false;

				//NotifyIconConntextMenu
				menuItem7.Enabled = false;
				menuItem28.Enabled = false;
				menuItem31.Enabled = false;

			
				//messageForms.Clear();	// Is this really good ?
			}
			this.SortListBox();
			this.UpdateLogginLabel();
		}


		// Messageform
		private MessageForm GetMessageForm(string id)
		{
			MessageForm mf = null;
			// Do we have the form in the list?
			for(int i= 0; i < messageForms.Count; i++)
			{
				mf = messageForms[i] as MessageForm;
				if(id == mf.GetId())
				{
					return mf;
				}
			}
			
			// We didnt find the speciall messageform, Then we make a new one..
			mf = (MessageForm)this.Invoke(new NewMessageFormDelegate(this.NewMessageForm), new object [] {id} );
			
			return mf;
		}
		private void		NewMessageFormCreated(MessageForm messageForm)
		{
			messageForms.Add(messageForm);

			messageForm.MessageFormClosedEvent += new MessageFormClosedEventHandler(MessageFormClosed);
			messageForm.ShowDataTransferWindowEvent += new EventHandler(messageForm_ShowDataTransferWindow);
			messageForm.FontChangedEvent += new EventHandler(messageForm_FontChangedEvent);
			messageForm.NewFileTransfer += new DGP_Messenger.MessageForm.NewFileTransferEventHandler(messageForm_NewFileTransfer);
			messageForm.Visible		= true;
			messageForm.Active		= true;

			messageForm.TextFont = Settings.MessageFormTextFont;
			messageForm.TextColor = Settings.MessageFormTextColor;
		}
		private void		MessageFormClosed(object sender, MessageFormClosedEventArgs e)
		{
			messageForms.Remove(e.messageForm);
		}

		private MessageForm NewMessageForm(User user, string contactId, Socket socket, bool activate)
		{
			DGPContact contact = new DGPContact(contactId, contactId, OnlineStatus.OFFLINE);
			foreach(DGPContact fr in listBox1.Items)
			{
				if(fr.Id == contactId)
				{
					contact = fr;
				}
			}

			MessageForm messageForm = new MessageForm(user, contact, socket);
			
			NewMessageFormCreated(messageForm);

			messageForm.Active		= activate;

			if(activate)
			{
				messageForm.BringToFront();
			}
			return messageForm;
		}

		private MessageForm NewMessageForm(string contactId)
		{
			MessageForm messageForm = new MessageForm(contactId);
			
			NewMessageFormCreated(messageForm);

			messageForm.Active		= false;

			return messageForm;
		}

		private MessageForm NewMsnMessageForm(Conversation conv, string mail)
		{
			MessageForm messageForm = new MessageForm(conv, mail);
			
			NewMessageFormCreated(messageForm);

			messageForm.Active		= false;

			return messageForm;
		}


		private void ShowLoggedInLayout()
		{
			//If reconnected!
		/*	foreach(MessageForm mf in messageForms)
			{
				mf.SocketConnect(clientSocket);
			}
*/
			UpdateLogginLabel();

			label2.Hide();
			label1.Show();
			roundedcornerPanel1.Show();
			roundedcornerPanel1.BringToFront();
			listBox1.Show();
			listBox1.BringToFront();
			pictureBox3.Show();

			mainMenu1.MenuItems[0].MenuItems[0].Enabled = true;
			mainMenu1.MenuItems[0].MenuItems[1].Enabled = true;
			mainMenu1.MenuItems[1].Enabled = true;
			mainMenu1.MenuItems[2].Enabled = true;

			//NotifyIconConntextMenu
			menuItem7.Enabled = true;
			menuItem28.Enabled = true;
			menuItem31.Enabled = true;

			if(this.loggingInChat)
			{
				publicChatForm = NewMessageForm(chatMessenger.user, "0", chatMessenger.ServerSocket, true);
				publicChatForm.SetNameAndStatusString("All (Online)");
				publicChatForm.SetDGPContactName("All");
				publicChatForm.Show();
			}
		}

		private void ShowLoggingInLayout()
		{
			LogginButton.Hide();
			label3.Hide();

			string msnString = "";
			string dpgString = "";

			if(loggingInMSN)
			{
				msnString = "\tMSN\n";
			}

			if(loggingInDGP)
			{
				dpgString = "\tDGP\n";
			}

			label2.Text = "Loggar in på: \n" + msnString + dpgString; 
			label2.Show();

			menuItem2.Enabled = true;
		}


		private void SortListBox()
		{
			listBox1.SuspendLayout();

			//ArrayList contacts = new ArrayList(D);
			ArrayList onlineContacts = new ArrayList();
			ArrayList offlineContacts = new ArrayList();

			DGPContact onlineGroupHeader = new DGPContact("-1", "Online", OnlineStatus.ONLINE);
			DGPContact offlineGroupHeader = new DGPContact("-2", "Offline", OnlineStatus.ONLINE);

			foreach(DGPContact con in Contacts)
			{
				if(con.IsMSNContact)
				{
					if(con.MSNContact.Status != MSNStatus.Offline)
					{
						onlineContacts.Add(con);
					}
					else
					{
						offlineContacts.Add(con);
					}
				}
				else
				{
					if(con.DGPStatus.Status != OnlineStatus.OFFLINE)
					{
						onlineContacts.Add(con);
					}
					else
					{
						offlineContacts.Add(con);
					}
				}
			}

			foreach(DGPContact con in listBox1.Items)
			{
				if(con.Id != "")
				{
					int id = int.Parse(con.Id);
					if(id < 0)
					{
						switch(id)
						{
							case -1: onlineGroupHeader = con; break;	
							case -2: offlineGroupHeader = con; break;
						}
					}
				}
			}

			listBox1.Items.Clear();

			onlineGroupHeader.Name = "Online (" + onlineContacts.Count.ToString()+")";
			listBox1.Items.Add(onlineGroupHeader);
			if(onlineGroupHeader.DGPStatus.Status == OnlineStatus.ONLINE)
			{
				if(onlineContacts.Count == 0)
				{
					listBox1.Items.Add(new DGPContact("-3", "Ingen", OnlineStatus.ONLINE));
				}
				else
				{
					listBox1.Items.AddRange((DGPContact []) onlineContacts.ToArray(typeof(DGPContact)));
				
				}
			}

			offlineGroupHeader.Name = "Offline (" + offlineContacts.Count.ToString()+")";
			listBox1.Items.Add(offlineGroupHeader);
			if(offlineGroupHeader.DGPStatus.Status == OnlineStatus.ONLINE)
			{
				if(offlineContacts.Count == 0)
				{
					listBox1.Items.Add(new DGPContact("-3", "Ingen", OnlineStatus.ONLINE));
				}
				else
				{
					listBox1.Items.AddRange((DGPContact []) offlineContacts.ToArray(typeof(DGPContact)));
				}
			}

			listBox1.ResumeLayout(true);
		}


		private void UpdateContact(Contact contact)
		{
			bool found = false;
			for(int i = 0; i < this.Contacts.Count && !found; i++)
			{
				DGPContact con = (DGPContact)Contacts[i];
				if(con.MSNContact == contact)
				{
					Contacts.Remove(con);
					Contacts.Add(new DGPContact(contact));
					found = true;
				}

			}
			if(!found)
			{
				Contacts.Add(new DGPContact(contact));
			}

			SortListBox();
		}

		private void UpdateContact(DGPContact contact)
		{
			bool found = false;
			for(int i = 0; i < this.Contacts.Count && !found; i++)
			{
				DGPContact listContact = (DGPContact)Contacts[i];
				if(contact.IsMSNContact == listContact.IsMSNContact)
				{
					if(contact.IsMSNContact)
					{
						if(contact.MSNContact.Mail == listContact.MSNContact.Mail)
						{
							listContact = contact;
							found = true;
						}
					}
					else
					{
						if(contact.Id == listContact.Id)
						{
							listContact = contact;
							found = true;
						}
					}
				}
			}

			if(!found)
			{
				Contacts.Add(contact);
			}

			SortListBox();
		}

/*		private void UpdateMessageForms()
		{
			foreach(DGPContact fr in listBox1.Items)
			{
				foreach(MessageForm mf in messageForms)
				{
					if(fr.Id == mf.contact.Id)
					{
						mf.SetDGPContactName(fr.Name);
						mf.SetDGPContactOnlineStatus(fr.Status.Status);
					}
				}
			}
		}
		*/
		private void UpdateOnlineStatus(OnlineStatus status)
		{
			if(dgpMessenger.Connected)
			{
				dgpMessenger.SetStatus(status);
			}
			
			if(msnMessenger.Connected == true)
			{
				MSNStatus msnStatus;
				switch(status.Status)
				{
					case OnlineStatus.AWAY: msnStatus = MSNStatus.Away; break;
					case OnlineStatus.BERIGTHBACK: msnStatus = MSNStatus.BRB; break;
					case OnlineStatus.BUSY: msnStatus = MSNStatus.Busy; break;
					case OnlineStatus.EATING: msnStatus = MSNStatus.Lunch; break;
					case OnlineStatus.OFFLINE: msnStatus = MSNStatus.Offline; break;
					case OnlineStatus.ONLINE: msnStatus = MSNStatus.Online; break;
					case OnlineStatus.TALKINGINPHONE: msnStatus = MSNStatus.Phone; break;
					case OnlineStatus.HIDDEN: msnStatus = MSNStatus.Hidden; break;
					case OnlineStatus.WRITINGCODE: msnStatus = MSNStatus.Unknown; break;
					default : msnStatus = MSNStatus.Unknown; break;
				}
				msnMessenger.SetStatus(msnStatus);
				msnMessenger.Owner.Status = msnStatus;
				MessageBox.Show(msnMessenger.Owner.Status.ToString() +"\n"+msnStatus.ToString());
			}

			UpdateLogginLabel();
		}

		private void UpdateLogginLabel()
		{
			string labelString = "Inloggad : \n\t";
			string loggingInString = "";

			if(loggingInMSN || loggingInDGP)
			{
				loggingInString = "Loggar in på ";
			}

			if(dgpMessenger != null && dgpMessenger.Connected)
			{
				labelString += "DGP " + dgpMessenger.user.DisplayName + dgpMessenger.user.Status.GetStatusString() + "\n\t"; 
			}
			else if(loggingInDGP)
			{
				loggingInString += "DGP ";
			}

			if(chatMessenger != null && chatMessenger.Connected)
			{
				labelString += "Chat " + chatMessenger.user.DisplayName + chatMessenger.user.Status.GetStatusString() + "\n\t"; 
			}
			else if(loggingInDGP)
			{
				loggingInString += "Chat";
			}

			if(msnMessenger != null && msnMessenger.Connected)
			{				
				labelString += "MSN " + msnMessenger.Owner.Name + " (" + msnMessenger.Owner.Status + ")\n\t";
			}
			else if(loggingInMSN)
			{
				loggingInString += "MSN";
			}

			label1.Text = labelString + loggingInString;
		}


		#endregion					//////////////////////////

		#region Events				//////////////////////////

		#region LogginButton		//////////////////////////

		private void LoginButton_Click(object sender, System.EventArgs e)
		{
			if(logginDialog != null)
			{
				logginDialog.BringToFront();

			//	this.FormBorderStyle = FormBorderStyle.Sizable;
			//	this.Menu = this.mainMenu1;
			}
			else
			{
				//logginMSNThread = new Thread(new ThreadStart(this.Loggin));
				//logginMSNThread.Start();

				this.Loggin();
			//	this.FormBorderStyle = FormBorderStyle.None;
			//	this.Menu = null;
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


		#endregion					//////////////////////////
	
		#region MenuItems			//////////////////////////

		private void menuItemExit_Click(object sender, System.EventArgs e)
		{
			this.forcedClosed = true;
			this.Close();	
		}


		private void menuItemLoggOut_Click(object sender, System.EventArgs e)
		{
			if(MessageBox.Show(this, "Vill du logga ut från ALLA nätverk?", "Logga Ut?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				Loggout(Protokoll.ALL);
			}
		}


		private void menuItemLoggOfDGP_Click(object sender, System.EventArgs e)
		{
			if(MessageBox.Show(this, "Vill du logga ut från DGP nätverket?", "Logga Ut?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				Loggout(Protokoll.DGP);
			}
		}


		private void menuItemLoggOfMSN_Click(object sender, System.EventArgs e)
		{
			if(MessageBox.Show(this, "Vill du logga ut från MSN nätverket?", "Logga Ut?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				Loggout(Protokoll.MSN);
			}
		}


		private void menuItemSettings_Click(object sender, System.EventArgs e)
		{
			string dgpName = "", msnName = "";
			if(dgpMessenger.user != null)
			{
				
				dgpName = dgpMessenger.user.DisplayName;
			}
			if(msnMessenger.Owner != null)
			{
				
				msnName = msnMessenger.Owner.Name;
			}

			SettingsForm settingsForm = new SettingsForm();
			settingsForm.dgpName = dgpName;
			settingsForm.msnName = msnName;

			if(settingsForm.ShowDialog() == DialogResult.OK)
			{
				if(dgpMessenger.user != null)
				{
					if(settingsForm.dgpName != dgpMessenger.user.DisplayName)
					{
						dgpMessenger.SetDisplayName(settingsForm.dgpName);
						this.UpdateLogginLabel();
					}
				}
				if(msnMessenger != null)
				{
					if(msnMessenger.Connected && msnMessenger.Owner.Name != settingsForm.msnName)
					{
						msnMessenger.Owner.Name = settingsForm.msnName;
						this.UpdateLogginLabel();
					}
				}
			}
		}


		private void menuItemAbout_Click(object sender, System.EventArgs e)
		{
			AboutDialog aboutDialog = new AboutDialog();
			aboutDialog.ShowDialog(this);
		}


		private void menuItem26_Click(object sender, System.EventArgs e)
		{
			EditorWindow editWin = new EditorWindow();
			editWin.Visible = true;
		}


		private void menuItemShowDataTransfer_Click(object sender, System.EventArgs e)
		{
			this.dataTransferForm.Visible = true;
			this.dataTransferForm.BringToFront();
		}


		#region SetStatuses			//////////////////////////
		
		private void setOnline_Click(object sender, System.EventArgs e)
		{
			UpdateOnlineStatus(new OnlineStatus(OnlineStatus.ONLINE));
		}	


		private void setBrb_Click(object sender, System.EventArgs e)
		{
			UpdateOnlineStatus(new OnlineStatus(OnlineStatus.BERIGTHBACK));
		}


		private void setHidden_Click(object sender, System.EventArgs e)
		{
			UpdateOnlineStatus(new OnlineStatus(OnlineStatus.HIDDEN));
		}


		private void setAway_Click(object sender, System.EventArgs e)
		{
			UpdateOnlineStatus(new OnlineStatus(OnlineStatus.AWAY));
		}


		private void setEating_Click(object sender, System.EventArgs e)
		{
			UpdateOnlineStatus(new OnlineStatus(OnlineStatus.EATING));
		}


		private void setPhone_Click(object sender, System.EventArgs e)
		{
			UpdateOnlineStatus(new OnlineStatus(OnlineStatus.TALKINGINPHONE));
		}	


		private void setBusy_Click(object sender, System.EventArgs e)
		{
			UpdateOnlineStatus(new OnlineStatus(OnlineStatus.BUSY));
		}


		#endregion					//////////////////////////

		#endregion					//////////////////////////

		#region ListBox				//////////////////////////

		private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			listBox1.Refresh();
		}


		private void listBox1_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			listBox1.SuspendLayout();

			if(e.Index >= 0)
			{
				e.DrawFocusRectangle();
				e.DrawBackground();

				DGPContact contact = (DGPContact)listBox1.Items[e.Index];
			
				Color stringColor;
				string statusString = "";
				int tab = 15;
				Font font = listBox1.Font;

				if(contact.Id == "-3")
				{
					stringColor = Color.Gray;
				}
				else if(contact.Id != "" && int.Parse(contact.Id) < 0)
				{
					stringColor = Color.DarkBlue;
					font = new Font(listBox1.Font, FontStyle.Bold);
					tab = 0;
				}
				else
				{
					bool offline = false;;
					if(contact.IsMSNContact)
					{
						statusString = " ( " + contact.MSNContact.Status.ToString() + " )";
						if(contact.MSNContact.Status == MSNStatus.Offline)
						{
							offline = true;
						}
					}
					else if(contact.IsChatContact) 
					{
						// ( ) är redan inkluderade i dgpstatus strängen!
						statusString = "";

						stringColor = Color.Gainsboro;
					}
					else if(contact.IsChatContact) 
					{
						// ( ) är redan inkluderade i dgpstatus strängen!
						statusString = contact.DGPStatus.GetStatusString();

						if(contact.DGPStatus.Status == OnlineStatus.OFFLINE)
						{
							offline = true;
						}
					}
					
					if(offline && !contact.IsChatContact)
					{
						stringColor = Color.Gray;
					}
					else
					{
						stringColor = Color.DarkSlateGray;
					}
				}


	
				if(listBox1.GetSelected(e.Index))
				{
					stringColor = Color.White;
				}


				e.Graphics.DrawString(contact.Name + statusString, font, new SolidBrush(stringColor), e.Bounds.Left + tab, e.Bounds.Top + 1);
			}

			listBox1.ResumeLayout(true);
		}


		private void listBox1_MeasureItem(object sender, System.Windows.Forms.MeasureItemEventArgs e)
		{
			e.ItemHeight = listBox1.Font.Height + 4;
		}


		private void listBox1_DoubleClick(object sender, System.EventArgs e)
		{
			DGPContact contact = (DGPContact)listBox1.SelectedItem;
			if(contact != null)
			{
				if(contact.IsMSNContact)
				{
					if(contact.MSNContact.Status != MSNStatus.Offline /*&& 
						this.msnMessenger.Owner.Status != MSNStatus.Offline*/ )
					{
						//this.NewMessageForm(contact.MSNContact.Mail);
						MessageForm mf = GetMessageForm(contact.MSNContact.Mail);
						mf.Show();
						if(mf.conversation == null || mf.conversation.Connected != true)
						{
							//MessageBox.Show("REQUEST New conversation: " + contact.MSNContact.Name);
							msnMessenger.RequestConversation(contact.MSNContact.Mail, contact.MSNContact.Mail);
						}
					}
				}
				else if(int.Parse(contact.Id) < 0)
				{
					if(contact.DGPStatus.Status == OnlineStatus.ONLINE)
					{
						contact.DGPStatus.Status = OnlineStatus.OFFLINE;
					}
					else
					{
						contact.DGPStatus.Status = OnlineStatus.ONLINE;
					}
					this.SortListBox();
				}
				else if(contact.DGPStatus.Status != OnlineStatus.OFFLINE)
				{
					bool allreadyOpen = false;
					int i = 0;
					foreach(MessageForm mf in messageForms)
					{
						if(mf.contact.Id == contact.Id
							&& mf.Visible != false)
						{
							allreadyOpen = true;
							mf.contact = contact;
							break;
						}
						i++;
					}
					if(allreadyOpen == false)
					{
						if(contact.IsChatContact)
						{
							if(contact.Id == "0")
							{
								publicChatForm.Show();
							}
							else
							{
								NewMessageForm(chatMessenger.user, contact.Id, chatMessenger.ServerSocket, true);
							}
						}
						else
						{
							NewMessageForm(dgpMessenger.user, contact.Id, dgpMessenger.ServerSocket, true);
						}
					}
					else
					{
						MessageForm message = (MessageForm)messageForms[i];
						message.BringToFront();
					}
				}
			}
		}


		#endregion					//////////////////////////

		#region MainFormEvents		//////////////////////////

		private void MainForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Rectangle rect = this.ClientRectangle;
			rect.Height = 440;

			LinearGradientBrush brush = new LinearGradientBrush(rect,
				Color.FromArgb(230, 233,240),
				Color.FromArgb(246, 246, 247),
				LinearGradientMode.Vertical);

			Graphics g = e.Graphics;
			g.FillRectangle(brush, rect);

			//Image b = this.BackgroundImage;
			//g.DrawImage(b, 0,0);

		//	e.Dispose();

		}


		private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(forcedClosed == true) // Exit from the menu
			{
				// Save Settins
				Settings.desktopLocation = this.DesktopLocation;
				Settings.size = this.Size;
				//Settings.logginDialog = logginDialog;
				MainFormSettings mainFormSettings = new MainFormSettings();
				mainFormSettings.Save(this, "Settings.dgp");
			}
			else
			{	// Close only window and show in taskbar
				e.Cancel = true;
				this.ShowInTaskbar = false;
				this.Hide();
				NotifyIcon.Visible = true;
			}
		}

		
		private void MainForm_Load(object sender, System.EventArgs e)
		{
			MainFormSettings mainFormSettings = new MainFormSettings();
			this._settings = mainFormSettings.Load(this, "Settings.dgp");

			this.Location = Settings.desktopLocation;
			this.Size = Settings.size;
			//logginDialog = new LogginDialog();//Settings.logginDialog;

	//Automatisk loggin
	//Loggin();
		}


		private void addMsnFriendMenuItem_Click(object sender, System.EventArgs e)
		{
			AddContactDialog contactAdd = new AddContactDialog();
			contactAdd.ShowDialog(this);
			this.msnMessenger.AddContact(contactAdd.Email);
			this.msnMessenger.SendForwardListRequest();
		}

		
		#endregion					//////////////////////////

		#region MessaggeForm		//////////////////////////				
		
		private void messageForm_ShowDataTransferWindow(object sender, EventArgs e)
		{
			this.dataTransferForm.Show();
			this.dataTransferForm.BringToFront();
		}

		private void messageForm_FontChangedEvent(object sender, EventArgs e)
		{
			MessageForm messageForm = sender as MessageForm;
			this.Settings.MessageFormTextFont = messageForm.TextFont;
			this.Settings.MessageFormTextColor = messageForm.TextColor;

			foreach(MessageForm mf in messageForms)
			{
				mf.TextFont = Settings.MessageFormTextFont;
				mf.TextColor = Settings.MessageFormTextColor;
			}
		}

		private void messageForm_NewFileTransfer(object sender, NewFileTransferEventArgs e)
		{
			this.dataTransferForm.AddNewTransfer(new Activity(e.fileTransfer));
		}

	
		#endregion					//////////////////////////
 		
		// MSN
		#region Msn Events			//////////////////////////

		// Create
		private void OnSynchronizationCompleted(Messenger sender, EventArgs e)
		{
			loggingInMSN = false;
			/*	foreach(DGPContact contact in msnMessenger.GetListEnumerator(MSNList.ForwardList))
				{
					// if the contact is not offline we can send messages and we want to show
					// it in the contactlistview
					if(contact.Status != MSNStatus.Offline)
					{
						// add this contact to the listview,
						// we 'tag' the listitem with the contact object
						//ListItem item = new ListItem(contact.Name);
						//item.Tag		  = contact;
						Log.Text += contact.Name + "\r\n";
					}
				}

	*/		this.Invoke(new ShowLoggedInLayoutDelegate(this.ShowLoggedInLayout), null);

			//		Thread thread = new Thread(new ThreadStart(this.ShowLoggedInLayout));
			//		thread.Start();


			//		this.ShowLoggedInLayout();
			// first show all people who are on our forwardlist. This is the 'main' contactlist
			// a normal person would see when logging in.
			// if you want to get all 'online' people enumerate trough this list and extract
			// all contacts with the right DotMSN.MSNStatus  (eg online/away/busy)

			ArrayList groups = new ArrayList(msnMessenger.ContactGroups.Values);
			string groupsStr = "";
			foreach(Contact contact in msnMessenger.GetListEnumerator(MSNList.ForwardList))
			{		
				this.Invoke(new UpdateContactDelegate(this.UpdateContact), new object [] { contact });
				bool found = false;
				foreach(ContactGroup cg in groups)
				{
					if(contact.ContactGroup != null && contact.ContactGroup.ID == cg.ID)
					{
						groupsStr += cg.Name + ":"+ contact.Name+"\n";
						found = true;
					}
				}
				if(!found)
				{
					//groups.Add(contact.ContactGroup);
					//groupsStr += groups.Count.ToString() + ":"+ contact.ContactGroup.Name+"\n";
				}
				//this.Log.Text += "FL > " + contact.Name + " (" + contact.Status + ")\r\n";
				//FillListview();
			}
			/*	foreach(ContactGroup cg in msnMessenger.ContactGroups)
				{
					//groupsStr += cg.ID.ToString() + ":"+ cg.Name+"\n";
				}
			*/	

			////Testar Lite med ContacGroups!!!!
			/*DotMSN.ContactGroup cg1 = (DotMSN.ContactGroup)groups[0];
			MessageBox.Show(cg1.Name +"  "+ cg1.ID);
			MessageBox.Show(groupsStr);

			groupsStr = "";
			foreach(ContactGroup cg in groups)
			{
				groupsStr += cg.Name + "\n";
			}
			MessageBox.Show(groupsStr);
*/

			/*	// now get the reverse list. This list shows all people who have you on their
			// contactlist.
			foreach(DGPContact contact in msnMessenger.ReverseList)
			{
				this.Log.Text += "RL > " + contact.Name + " (" + contact.Status + ")\r\n";
			}

			// we follow with the blocked list. this shows all the people who are blocked
			// by you
			foreach(DGPContact contact in msnMessenger.BlockedList)
			{
				this.Log.Text += "BL > " + contact.Name + " (" + contact.Status + ")\r\n";
			}

			// when the privacy of the client is set to MSNPrivacy.NoneButAllowed then only
			// the contacts on the allowedlist are able to see your status
			foreach(DGPContact contact in msnMessenger.AllowedList)
			{
				this.Log.Text += "AL > " + contact.Name + " (" + contact.Status + ")\r\n";
			}
*/
			// now set our initial status !
			// we must set an initial status otherwise 
			msnMessenger.SetStatus(MSNStatus.Online);
			
			//msnMessenger.
			//Log.Text += "Status set to online!\r\n";


			/* the alllist just enumerates all possible contacts. it is not very usefull in
				 * this sample so i've commented it out.
				foreach(DGPContact contact in msnMessenger.AllList)
				{
					this.Log.Text += "AL > " + contact.Name + " (" + contact.Status + ")";
				} */
		}

		private void ContactOnline(Messenger sender, ContactEventArgs e)
		{
			this.Invoke(new UpdateContactDelegate(this.UpdateContact), new object [] { e.Contact });
		}
		
		private void ContactOffline(Messenger sender, ContactEventArgs e)
		{
			this.Invoke(new UpdateContactDelegate(this.UpdateContact), new object [] { e.Contact });
		}


		// Main
		private void msnMessenger_ErrorReceived(Messenger sender, MSNErrorEventArgs e)
		{
			MessageBox.Show(e.Error.ToString(), "Error");
		}

		private void msnMessenger_ConnectionFailure(Messenger sender, ConnectionErrorEventArgs e)
		{
			MessageBox.Show(e.Error.ErrorCode.ToString(),"Connection Error");
		}

		private void msnMessenger_ContactAdded(Messenger sender, ListMutateEventArgs e)
		{
			MessageBox.Show(this, e.Subject.Name + " har lagts till din lista!", "Contact Added");
		}

		private void msnMessenger_MailboxStatus(Messenger sender, MailboxStatusEventArgs e)
		{
			MessageBox.Show(this,	"InboxURL: " + e.InboxURL + 
				"\n FoldersUnread: " + e.FoldersUnread +
				"\n FoldersURL: " + e.FoldersURL + 
				"\n InboxUnread: " + e.InboxUnread +
				"\n PostURL: " + e.PostURL);
		}

		private void msnMessenger_ListReceived(Messenger sender, ListReceivedEventArgs e)
		{
			MessageBox.Show(this, "msnMessenger_ListReceived");
		}

		private void msnMessenger_ConversationCreated(Messenger sender, ConversationEventArgs e)
		{
			//MessageBox.Show("New conversation: " + e.Conversation.Messenger.Owner.Name);

			// remember there are not yet users in the conversation (except ourselves)
			// they will join _after_ this event. We create another callback to handle this.
			// When user(s) have joined we can start sending messages.
			//e.Conversation.ContactJoin +=new DotMSN.Conversation.ContactJoinHandler(Conversation_ContactJoin);			

			// log the event when the two clients are connected
			//e.Conversation.ConnectionEstablished += new DotMSN.Conversation.ConnectionEstablishedHandler(Conversation_ConnectionEstablished); 
			

			if(e.Conversation.ClientData != null)
			{
				string mail = e.Conversation.ClientData as string;
				//MessageBox.Show(mail);
				this.GetMessageForm(mail).SetConversation(e.Conversation);
			}
			else
			{
				e.Conversation.MessageReceived += new DotMSN.Conversation.MessageReceivedHandler(Conversation_MessageReceived);
			}

			//this.Invoke(new NewMsnMessageFormDelegate(this.NewMsnMessageForm), new object [] {e.Conversation, null} );
			// notify us when the other contact is typing something
			//e.Conversation.UserTyping  +=		

			// we want to be accept filetransfer invitations
			//e.Conversation.FileTransferHandler.InvitationReceived +=

		}


		// Owner
		private void Owner_ScreenNameChanged(Contact sender, EventArgs e)
		{
			UpdateLogginLabel();
		}

		private void Owner_StatusChanged(Contact sender, StatusChangeEventArgs e)
		{
			UpdateLogginLabel();
		}

				
		// Conversation
		private void Conversation_MessageReceived(Conversation sender, MessageEventArgs e)
		{
			if(sender.ClientData == null)
			{
				sender.ClientData = e.Sender.Mail;
				MessageForm mf = this.GetMessageForm(sender.ClientData as string);
				mf.SetConversation(sender);
				mf.UpdateMsnContactName();
				mf.conv_MessageReceived(sender, e);
			}
		}

	/*	private void Conversation_ConnectionEstablished(Conversation sender, EventArgs e)
		{
			//string mail = sender.ClientData as string;
			//this.GetMessageForm(mail).SetConversation(sender);
			//MessageBox.Show("ConnectionEstablished ");
			//this.GetMessageForm(sender)
			//this.Invoke(new NewMsnMessageFormDelegate(this.NewMsnMessageForm), new object [] {sender} ); //NewMessageForm(sender);
		}*/

		#endregion					//////////////////////////

		#region DGP Events			//////////////////////////

		private void dgpMessenger_logginErrorEvent(object sender, DGPLogginEventArgs e)
		{
			if(e.ErrorCode == DGP_Messenger.DGPLogginCodes.ConnectionError)
			{
				MessageBox.Show("Kunde inte kontakta DGP servern!", 
					"Kunde Inte Logga In", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else if(e.ErrorCode == DGP_Messenger.DGPLogginCodes.WrongPassUser)
			{
				MessageBox.Show("Fel användarnamn/lösenord!", 
					"Kunde Inte Logga In", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			Loggout(Protokoll.DGP);
		}

		private void dgpMessenger_logginComplete(object sender, EventArgs e)
		{
			Contacts.AddRange(dgpMessenger.Contacts);

			SortListBox();
			//ShowLoggedInLayout();
			this.Invoke(new ShowLoggedInLayoutDelegate(this.ShowLoggedInLayout), null);
		}

		private void dgpMessenger_updateContact(object sender, ContactUpdateArgs e)
		{
			this.UpdateContact(e.contact);
		}

		private void dgpMessenger_recivedMessage(object sender, RecivedMessageEventArgs e)
		{
			string itsMessage = e.Message;
			string contactId = e.ContactId;
			string contactName = "";
			Color messageColor = Color.Blue;

			MessageForm messageForm;

			int i = 0;
			bool allreadyOpen = false;
			foreach(MessageForm mf in messageForms)
			{
				if( mf.contact.Id == contactId
					&& mf.Visible == true)
				{
					allreadyOpen = true;
					break;
				}
				i++;
			}
			if(allreadyOpen == false)
			{
				NewMessageForm(dgpMessenger.user, contactId, dgpMessenger.ServerSocket, false);
			}

			messageForm = (MessageForm)messageForms[i];
			
			contactName = messageForm.contact.Name;
		
			messageForm.DisplayMessage(itsMessage, messageColor, contactName, true);

			if(messageForm.Active == false)
			{
				messageForm.AlertMessage();
			}
		}

		
		#endregion					//////////////////////////
		
		#region Chat Events			//////////////////////////

		private void chatMessenger_logginErrorEvent(object sender, DGPLogginEventArgs e)
		{
			if(e.ErrorCode == DGP_Messenger.DGPLogginCodes.ConnectionError)
			{
				MessageBox.Show("Kunde inte kontakta DGP servern!", 
					"Kunde Inte Logga In", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else if(e.ErrorCode == DGP_Messenger.DGPLogginCodes.WrongPassUser)
			{
				MessageBox.Show("Fel användarnamn/lösenord!", 
					"Kunde Inte Logga In", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			Loggout(Protokoll.CHAT);
		}

		private void chatMessenger_logginComplete(object sender, EventArgs e)
		{
			Contacts.AddRange(chatMessenger.Contacts);

			SortListBox();
			//ShowLoggedInLayout();
			this.Invoke(new ShowLoggedInLayoutDelegate(this.ShowLoggedInLayout), null);
		}

		private void chatMessenger_updateContact(object sender, ContactUpdateArgs e)
		{
			this.UpdateContact(e.contact);
		}

		private void chatMessenger_recivedMessage(object sender, RecivedMessageEventArgs e)
		{
			string itsMessage = e.Message;
			string contactId = e.ContactId;

			string contactName = "Skumt";
			Color messageColor = Color.Blue;

			MessageForm messageForm;
			
			if(e.ToId == "0")
			{
				messageForm = publicChatForm;
				foreach(DGPContact con in this.listBox1.Items)
				{
					if(con.Id == e.ContactId)
					{
						contactName = con.Name;
						publicChatForm.DisplayMessage(itsMessage, messageColor, contactName, true);
						continue;
					}
				}				
			}
			else
			{
				int i = 0;
				bool allreadyOpen = false;
				foreach(MessageForm mf in messageForms)
				{
					if( mf.contact.Id == contactId
						&& mf.Visible == true)
					{
						allreadyOpen = true;
						break;
					}
					i++;
				}
				if(allreadyOpen == false)
				{
					this.Invoke(new CreateNewMessageFormDelegate(this.NewMessageForm), new object [] {chatMessenger.user, contactId, chatMessenger.ServerSocket, false });
				}

			
				messageForm = (MessageForm)messageForms[i];
			
				contactName = messageForm.contact.Name;
				messageForm.DisplayMessage(itsMessage, messageColor, contactName, true);
			}

			

			if(messageForm.Active == false)
			{
				messageForm.AlertMessage();
			}
		}

		#endregion

		private void NotifyIcon_DoubleClick(object sender, System.EventArgs e)
		{
			this.ShowInTaskbar = true;
			this.Show();
			NotifyIcon.Visible = false;
		}

		private void NotifyIcon_Click(object sender, System.EventArgs e)
		{
			//NotifyIcon.le
		}

		private void pictureBox1_Click(object sender, System.EventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.dgpmessenger.se");
		}
	

		#endregion					//////////////////////////

		#region EntryPoint

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			
			Process[] RunningProcesses = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
   
			//MessageBox.Show(RunningProcesses.Length.ToString());
			//if(RunningProcesses.Length == 1)
			{
				//Application.Run(new LogginDialog());
				//Application.Run(new EditorWindow());	
				
				Application.Run(new MainForm());
				
				//Application.Run(new DataTransferForm());
				//ResumeDownloadDialog rs = new ResumeDownloadDialog("Coool.Mp3", "David", false);
				//rs.ShowDialog();
				//Application.Run(new MessageForm(new User("Adde", "MR Cool", 'A', ""), new DGPContact("Adde1", "Andreas", 'B'), null, new DataTransferForm()));
			}
			/*else
			{
				MessageBox.Show("Du kör redan en instans av DGP Messenger!", "Process ERROR",MessageBoxButtons.OK, MessageBoxIcon.Stop);
				foreach(Process process in RunningProcesses)
				{
					clsUser32Dll.ShowWindowAsync(process.MainWindowHandle, (int)clsUser32Dll.ShowWindowConstants.SW_SHOWMINIMIZED);
					clsUser32Dll.ShowWindowAsync(process.MainWindowHandle, (int)clsUser32Dll.ShowWindowConstants.SW_RESTORE);
				}
			}*/
		}

		#endregion
	}
}
