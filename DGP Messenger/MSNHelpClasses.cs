using System;
using System.Drawing;

namespace DGP_Messenger
{

	/// <summary>
	/// Summary description for MSNHeaderStringBuilder.
	/// </summary>
	public class MSNHeaderStringBuilder
	{
		private readonly string headerString = "MIME-Version: 1.0\r\n" + "Content-Type: text/plain; charset=UTF-8\r\n";
		private readonly string formatString = "";

		public MSNHeaderStringBuilder(Font font, Color color)
		{

			string fontString = font.Name.Replace(" ", "%20");

			string effectString = "";
			if(font.Underline)
			{
				effectString += "U";
			}
			if(font.Italic)
			{
				effectString += "I";
			}
			if(font.Strikeout)
			{
				effectString += "S";
			}
			if(font.Bold)
			{
				effectString += "B";
			}
			string colorString = ReverseString(ColorToHex(color));

			formatString = "X-MMS-IM-Format: FN=" + fontString + "; EF=" + effectString + "; CO=" + colorString + "; CS=0; PF=22";
		}

		public string Header
		{
			get { return (headerString + formatString); }
		}


		#region Some internet code for the color to hex converting (minor modifications)

		public static String IntToHex(int hexint)
			//  This method converts a integer into a hexadecimal string representing the
			//  int value. The returned string will look like this: 55FF. Note that there is
			//  no leading '#' in the returned string! 
		{
			int counter,reminder;
			String hexstr;

			counter=1;
			hexstr="";
			while (hexint+15>Math.Pow(16,counter-1))
			{
				reminder=(int)(hexint%Math.Pow(16,counter));
				reminder=(int)(reminder/Math.Pow(16,counter-1));
				if (reminder<=9)
				{
					hexstr=hexstr+(char)(reminder+48);
				}
				else
				{
					hexstr=hexstr+(char)(reminder+55);
				}
				hexint-=reminder;
				counter++;
			}
			//return ReverseString(hexstr);
			return hexstr;
		}

		public static String IntToHex(int hexint,int length)
			//  This version of the IntToHex method returns a hexadecimal string representing the
			//  int value in the given minimum length. If the hexadecimal string is shorter then the
			//  length parameter the missing characters will be filled up with leading zeroes.
			//  Note that the returend string though is not truncated if the value exeeds the length!
		{
			String hexstr,ret;
			int counter;
			hexstr=IntToHex(hexint);
			ret="";
			if (hexstr.Length<length)
			{
				for (counter=0;counter<(length-hexstr.Length);counter++)
				{
					ret=ret+"0";
				}
			}
			return ret+hexstr;
		}

		public static String ReverseString(String inStr)
			//  Helper Method that reverses a String.
		{
			String outStr;
			int counter;
			outStr="";
			for (counter=inStr.Length-1;counter>=0;counter--)
			{
				outStr=outStr+inStr[counter];
			}
			return outStr;
		}

		public static String ColorToHex(Color actColor)
			//  Translates a .NET Framework Color into a string containing the html hexadecimal 
			//  representation of a color. The string has a leading '#' character that is followed 
			//  by 6 hexadecimal digits. 
		{
			return ""+IntToHex(actColor.R,2)+IntToHex(actColor.G,2)+IntToHex(actColor.B,2);
		}

		#endregion
	}
}