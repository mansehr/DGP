using System;

namespace DGP_Messenger
{
	/// <summary>
	/// Summary description for User.
	/// </summary>
	public class User
	{
		private string _id = "";
		private string _displayName = "";

//		private string _logginName = "";
		private string _ipNumber = "";

		private OnlineStatus _status= new OnlineStatus(OnlineStatus.OFFLINE);

		public User(string id, string displayName, char status, string ipNumber)
		{
			this._id = id;
			this._displayName = displayName;
			this._status.Status = status;
//			this._logginName = logginName;
			this._ipNumber = ipNumber;
		}
		public User()
		{
			// Do Nothing
		}

		public string Id 
		{
			set { _id = value; }
			get { return string.Copy(_id); } 
		}
		public string DisplayName 
		{
			set { _displayName = value; }
			get { return string.Copy(_displayName); } 
		}
/*		public string LogginName 
		{
			set { _logginName = value; }
			get { return string.Copy(_logginName); } 
		}
*/		public string IpNumber 
		{
			set { _ipNumber = value; }
			get { return string.Copy(_ipNumber); } 
		}
		public OnlineStatus Status 
		{
			set { _status = value; }
			get { return _status; } 
		}
	}
}
