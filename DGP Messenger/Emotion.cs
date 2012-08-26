using System;
using System.Drawing;

namespace DGP_Messenger
{
	/// <summary>
	/// Summary description for Emotion.
	/// </summary>
	public class Emotions: object
	{
		public Emotions(Image img)
		{
			this.active = false;
			this.image = img;
			this.number = 0;
			this.itsString = "";
			this.position = new Point(0);
			this.size = new Size(0,0);
		}

		public bool active;
		public Image image;
		public int number;
		public string itsString;
		public Point position;
		public Size size;

		public int X
		{
			get { return position.X; }
		}
		public int Y
		{
			get { return position.Y; }
		}
		public int Width
		{
			get { return this.size.Width; }
		}
		public int Height 
		{
			get { return this.size.Height; }
		}

		public int Left
		{
			get { return this.position.X; }
		}
		public int Top
		{
			get { return this.position.Y; }
		}
		public int Right
		{
			get { return this.position.X + this.size.Width; }
		}
		public int Bottom
		{
			get { return this.position.Y + this.size.Height; }
		}

		public bool pointInside(int x, int y)
		{
			if( x >= this.Left && x <= this.Right &&
				y >= this.Top  && y <= this.Bottom)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public Rectangle Rect
		{
			get { return new Rectangle( this.position, this.size); }
		}
	}
}
