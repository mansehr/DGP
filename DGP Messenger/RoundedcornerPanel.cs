using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DGP_Messenger
{
	/// <summary>
	/// Summary description for RoundedcornerPanel.
	/// </summary>
	public class RoundedcornerPanel : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public RoundedcornerPanel()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true); 
			this.BackColor = Color.Transparent;
			this.SetStyle(ControlStyles.ResizeRedraw, true);
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// RoundedcornerPanel
			// 
			this.ForeColor = System.Drawing.Color.White;
			this.Name = "RoundedcornerPanel";
			this.Size = new System.Drawing.Size(136, 112);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.RoundedcornerPanel_Paint);

		}
		#endregion

		private void RoundedcornerPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Rectangle rect = this.ClientRectangle;
			int cornerSize = 20;

			if(cornerSize*2 > rect.Width)
			{
				cornerSize = rect.Width/2;
			}
			if(cornerSize*2 > rect.Height)
			{
				cornerSize = rect.Height/2;
			}

			if(cornerSize <= 0)
			{
				cornerSize = 1;
			}

			Brush brush = new SolidBrush(this.ForeColor);
			Pen pen = new Pen(new SolidBrush(Color.Gray), 1);

			e.Graphics.FillRectangle(brush, 
				rect.Left,
				rect.Top + cornerSize,
				rect.Width,
				rect.Height -(2*cornerSize)); 

			e.Graphics.FillRectangle(brush, 
				rect.Left + cornerSize,
				rect.Top,
				rect.Width - (2*cornerSize),
				rect.Height); 


			e.Graphics.FillPie(brush,
				rect.Left, 
				rect.Top,
				cornerSize*2,
				cornerSize*2,
				180, 90);

			e.Graphics.DrawArc(pen,
				rect.Left, 
				rect.Top,
				cornerSize*2,
				cornerSize*2,
				180, 90);

			e.Graphics.FillPie(brush,
				rect.Right - cornerSize*2 -1, 
				rect.Top,
				cornerSize*2,
				cornerSize*2,
				270, 90);

			e.Graphics.DrawArc(pen,
				rect.Right - cornerSize*2 -1, 
				rect.Top,
				cornerSize*2,
				cornerSize*2,
				270, 90);

			e.Graphics.FillPie(brush,
				rect.Right - cornerSize*2 -1, 
				rect.Bottom - cornerSize*2-1,
				cornerSize*2,
				cornerSize*2,
				0, 90);

			e.Graphics.DrawArc(pen,
				rect.Right - cornerSize*2 -1, 
				rect.Bottom - cornerSize*2 -1,
				cornerSize*2,
				cornerSize*2,
				0, 90);

			e.Graphics.FillPie(brush,
				rect.Left, 
				rect.Bottom - cornerSize*2 -1,
				cornerSize*2,
				cornerSize*2,
				90, 90);

			e.Graphics.DrawArc(pen,
				rect.Left, 
				rect.Bottom - cornerSize*2 -1,
				cornerSize*2,
				cornerSize*2,
				90, 90);



			Point [] leftLine = new Point [] {	new Point(rect.Left,	rect.Top + cornerSize),
												 new Point(rect.Left,	rect.Bottom - cornerSize) };
			Point [] rightLine = new Point [] { new Point(rect.Right-1,	rect.Top + cornerSize),
												  new Point(rect.Right-1,	rect.Bottom - cornerSize) }; 
			Point [] topLine = new Point [] {	new Point(rect.Right - cornerSize,	rect.Top),
												new Point(rect.Left + cornerSize,	rect.Top) }; 
			Point [] bottomLine = new Point [] {new Point(rect.Right - cornerSize,	rect.Bottom-1),
												   new Point(rect.Left + cornerSize,	rect.Bottom-1) }; 



			e.Graphics.DrawLines(pen, leftLine);
			e.Graphics.DrawLines(pen, rightLine);
			e.Graphics.DrawLines(pen, topLine);
			e.Graphics.DrawLines(pen, bottomLine);
		} 
	}
}
