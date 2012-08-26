using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using DotMSN;


namespace DGP_Messenger
{
	/// <summary>
	/// Summary description for DGPContact.
	/// </summary>
	public class DGPContact
	{
		DotMSN.Contact _msnContact = null;

		private string _name = "";
		private string _id = "";
		private OnlineStatus _onlineStatus = new OnlineStatus(OnlineStatus.OFFLINE);

		

		public DGPContact(string id, string name, char onlineStatus)
		{
			this._id = id;
			this._name = name;
			this._onlineStatus.Status = onlineStatus;
		}

		public DGPContact(DotMSN.Contact msnContact)
		{
			this._msnContact = msnContact;
		}

		public bool IsMSNContact
		{
			get 
			{
				if(_msnContact != null)
					return true;
				else 
					return false;
			}
		}

		public bool IsChatContact
		{
			get
			{
				if(this.IsMSNContact == false && this.DGPStatus.Status == OnlineStatus.CHAT_USER)
					return true;
				else
					return false;
			}
		}

		public DotMSN.Contact MSNContact
		{
			get { return _msnContact; }
			set { this._msnContact = value; }
		}

		public string Id 
		{
			set { _id = value; }
			get 
			{ 
				if(_id != null)
					return string.Copy(_id);
				else
					return "";
			} 
		}
	
		public OnlineStatus DGPStatus
		{
			set { _onlineStatus = value; }
			get { return _onlineStatus; } 
		}

		public string Name
		{
			set 
			{
				if(IsMSNContact == false)
				{
					_name = value;
				}
			}
			get 
			{
				if(IsMSNContact == false)
				{
					if(_name != null)
					{
						return string.Copy(_name);
					}
					else
					{
						return "";
					}
				}
				else
				{
					return this._msnContact.Name;
				}
			} 
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
