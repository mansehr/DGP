using System;
using System.Net.Sockets;

namespace DGP_Messenger
{
	public class OnlineStatus
	{
		private char _status;
		public OnlineStatus()
		{
			_status = OFFLINE;
		}

		public OnlineStatus(char status)
		{
			this._status = status;
		}

		public char Status
		{
			get { return _status; }
			set { _status = value; }
		}

		public const char ONLINE		= 'A';
		public const char AWAY			= 'I';
		public const char BERIGTHBACK	= 'B';
		public const char BUSY			= 'D';
		public const char TALKINGINPHONE= 'E';
		public const char EATING		= 'F';
		public const char WRITINGCODE	= 'G';
		public const char HIDDEN		= 'H';
		public const char OFFLINE		= 'Z';
		public const char CHAT_USER		= 'C';


		public string GetStatusString()
		{
			string stringStatus = "";
			
			if(_status == OnlineStatus.AWAY)
			{
				stringStatus = " (Inte Vid Datorn)";
			}
			else if(_status == OnlineStatus.BERIGTHBACK)
			{
				stringStatus = " (Strax tillbaka)";
			}
			else if(_status == OnlineStatus.BUSY)
			{
				stringStatus = " (Upptagen)";
			}
			else if(_status == OnlineStatus.EATING)
			{
				stringStatus = " (Äter)";
			}
			else if(_status == OnlineStatus.TALKINGINPHONE)
			{
				stringStatus = " (Pratar I Telefon)";
			}
			else if(_status == OnlineStatus.WRITINGCODE)
			{
				stringStatus = " (Knackar Kod)";
			}
			else if(_status == OnlineStatus.HIDDEN)
			{
				stringStatus = " (Offline)";
			}
			else if(_status == OnlineStatus.OFFLINE)
			{
				stringStatus = " (Offline)";
			}
			else if(_status == OnlineStatus.ONLINE)
			{
				stringStatus = " (Online)";
			}

			return string.Copy(stringStatus);
		}
	}

	public class SocketState
	{
		public readonly string id;
		public readonly string fileTransferId;
		public readonly Socket socket;
		
		public SocketState(string id, string fileTransferId, Socket socket)
		{
			this.id = id;
			this.fileTransferId = fileTransferId;
			this.socket = socket;
		}
	}
}
