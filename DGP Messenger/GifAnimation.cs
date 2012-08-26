using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DGP_Messenger
{
	/// <summary>
	/// Summary description for GifAnimation.
	/// </summary>
	public class GifAnimation : System.Windows.Forms.UserControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GifAnimation()
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
			// GifAnimation
			// 
			this.Name = "GifAnimation";
			this.Load += new System.EventHandler(this.AnimationForm_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.AnimationForm_Paint);

		}
		#endregion

		// Load animated GIF
		Bitmap gif = new Bitmap(typeof(GifAnimation), "SAMPLE_ANIMATION_COPY.GIF");

		void AnimationForm_Load(object sender, EventArgs e) 
		{
			// Set client size to size of image
			this.SetClientSizeCore(gif.Width, gif.Height);

			// Check if image supports animation
			if( ImageAnimator.CanAnimate(gif) ) 
			{
				// Subscribe to an event indicating the next frame should be shown
				ImageAnimator.Animate(gif, new EventHandler(gif_FrameChanged));
			}
		}

		void gif_FrameChanged(object sender, EventArgs e) 
		{
			if( this.InvokeRequired ) 
			{
				// Transition from worker thread to UI thread
				this.BeginInvoke(
					new EventHandler(gif_FrameChanged),
					new object[] { sender, e });
			}
			else 
			{
				// Trigger Paint event to draw next frame
				this.Invalidate();
			}
		}

		void AnimationForm_Paint(object sender, PaintEventArgs e) 
		{
			// Update image's current frame
			ImageAnimator.UpdateFrames(gif);

			// Draw image's active frame
			Graphics g = e.Graphics;
			Rectangle rect = this.ClientRectangle;
			g.DrawImage(gif, 0, 0);

		}
	}
}
