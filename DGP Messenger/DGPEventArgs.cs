using System;

namespace DGP_Messenger
{
	public enum DGPLogginCodes
	{
		WrongPassUser = 1,
		ConnectionError	
	}
	/// <summary>
	/// Summary description for DGPMessengerEvents.
	/// </summary>
	public class DGPLogginEventArgs : EventArgs
	{
		public readonly DGPLogginCodes ErrorCode;
		public DGPLogginEventArgs(DGPLogginCodes init)
		{
            ErrorCode = init;
		}
	}


	public class ContactUpdateArgs : EventArgs
	{
		public readonly DGPContact contact;
		public ContactUpdateArgs(DGPContact init)
		{
			contact = init;
		}
	}


	public class RecivedMessageEventArgs : EventArgs
	{
		private string _contactId;
		private string _message;
		private string _toId;

		public RecivedMessageEventArgs(string fromId, string toId, string message)
		{
			_contactId	= fromId;
			_toId		= toId;
			_message	= message;
		}

		public string Message
		{
			get { return string.Copy(_message); }
		}

		public string ContactId
		{
			get { return string.Copy(_contactId); }
		}
		
		public string ToId
		{
			get { return string.Copy(_toId); }
		}
	}
}
