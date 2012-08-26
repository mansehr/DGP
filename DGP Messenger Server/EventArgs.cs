using System;

namespace DGP_Messenger_Server
{
	/**********************************************************************
	*
	*  This class is derived from System.EventArgs, and it's used to pass
	*  additional data in the Client’s LineRecived event. It passes the
	*  client that sent the message, and the message itself.
	*
	**********************************************************************/

	public delegate void LineRecivedEventHandler(object sender, LineRecivedEventArgs e);
	public delegate void LoggEventHandler(object sender, LoggEventArgs e);
	public delegate void ClientRecivedEventHandler(object sender, ClientRecivedEventArgs e);

	public class LineRecivedEventArgs : System.EventArgs
	{
		#region Class members

		/**********************************************************************
		*
		*  Class members
		*
		**********************************************************************/

		private Client	_client;
		private string	_message;

		#endregion

		#region Properties

		/**********************************************************************
		*
		*  Properties
		*
		**********************************************************************/

		public Client Client
		{
			get { return _client; }
		}

		public string Message
		{
			get { return _message; }
		}

		#endregion

		#region Constructors

		/**********************************************************************
		*
		*  Constructors
		*
		**********************************************************************/
		
		public LineRecivedEventArgs(Client client, string message)
		{
			_client		= client;
			_message	= message;
		}

		#endregion
	}

	public class ClientRecivedEventArgs : System.EventArgs
	{
		#region Class members

		/**********************************************************************
		*
		*  Class members
		*
		**********************************************************************/

		private Client	_client;

		#endregion

		#region Properties

		/**********************************************************************
		*
		*  Properties
		*
		**********************************************************************/

		public Client Client
		{
			get { return _client; }
		}

		#endregion

		#region Constructors

		/**********************************************************************
		*
		*  Constructors
		*
		**********************************************************************/
		
		public ClientRecivedEventArgs(Client client)
		{
			_client		= client;
		}

		#endregion
	}

	public class LoggEventArgs
	{
		#region Class members

		/**********************************************************************
		*
		*  Class members
		*
		**********************************************************************/

		private string	_message;

		#endregion

		#region Properties

		/**********************************************************************
		*
		*  Properties
		*
		**********************************************************************/

		public string Message
		{
			get { return _message; }
		}

		#endregion

		#region Constructors

		/**********************************************************************
		*
		*  Constructors
		*
		**********************************************************************/

		public LoggEventArgs(string message)
		{
			_message	= message;	
		}

		#endregion
	}
}
