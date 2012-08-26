using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

namespace DGP_Messenger
{
	/// <summary>
	/// Summary description for settings.
	/// </summary>

	[SerializableAttribute]
	public class MainFormSettingsValues
	{
		public Point	desktopLocation			= new Point(100, 100);
		public Size		size					= new Size(304, 443);
		public Color	MessageFormTextColor	= Color.Black;
		public Font		MessageFormTextFont		= new Font("Microsoft Sans Serif", 8.25F);
		//public LogginDialog		logginDialog	= new LogginDialog();	
	}


	public class MainFormSettings
	{
		public void Save(MainForm form, string fileName)
		{
			MainFormSettingsValues values = form.Settings;

			using( Stream stream = new FileStream(@fileName, FileMode.Create) ) 
			{
				// Write to the stream
				IFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, values);
			}
		}

		public MainFormSettingsValues Load(MainForm form, string fileName)
		{
			MainFormSettingsValues values;
			try
			{
				using( Stream stream = new FileStream(@fileName, FileMode.OpenOrCreate) ) 
				{
					IFormatter formatter = new BinaryFormatter();
					values = (MainFormSettingsValues)formatter.Deserialize(stream);
				}
			}
			catch(Exception e)
			{
				MessageBox.Show("Error: " + e.Message, "Loadsettings exception");
				values = new MainFormSettingsValues();
			}
			
			form.StartPosition = FormStartPosition.Manual;

			return values;
		}
	}
}
