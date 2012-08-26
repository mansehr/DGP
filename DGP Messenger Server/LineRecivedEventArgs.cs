using System;

namespace DGP_Messenger_Server
{
	/**********************************************************************
	*
	*  This class is derived from System.EventArgs, and it's used to pass
	*  additional data in the Client’s LineReceived event. It passes the
	*  client that sent the message, and the message itself.
	*
	**********************************************************************/
	public class LineReceivedEventArgs : System.EventArgs
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
		
		public LineReceivedEventArgs(Client client, string message)
		{
			_client		= client;
			_message	= message;
		}

		#endregion
	}
}
