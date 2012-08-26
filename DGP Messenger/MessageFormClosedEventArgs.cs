using System;

namespace DGP_Messenger
{
	/// <summary>
	/// Summary description for MessageFormClosedEventArgs.
	/// </summary>
	public class MessageFormClosedEventArgs
	{
		public MessageFormClosedEventArgs(MessageForm messageForm)
		{
			_messageForm = messageForm;
		}

		#region Class members

		/**********************************************************************
		*
		*  Class members
		*
		**********************************************************************/

		private MessageForm	_messageForm;

		#endregion

		#region Properties

		/**********************************************************************
		*
		*  Properties
		*
		**********************************************************************/

		public MessageForm messageForm
		{
			get { return _messageForm; }
		}

		#endregion

	}
}
