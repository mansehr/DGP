using System;
using System.Net.Sockets;
using System.Text;

namespace DGP_Messenger_Sever
{

	public class Client
	{
		// Event fired when the client sends a message
		public event LineReceivedEventHandler LineReceived;


		// The connected client socket
		private Socket _socket;

		// Variables used to receive data from the socket
		private int		byteBufferSize;
		private byte[]	byteBuffer;
		private string	messageBuffer;

		//Client Variables
		private string _id;
		private string _email;
		private string _displayName;
		private char _status;


		public Client(Socket socket)
		{
			byteBufferSize	= 1024;
			byteBuffer		= new byte[byteBufferSize];
			messageBuffer	= "";

			_id				= "";
			_email			= "";
			_displayName	= "NONAME";
			_status			= 'Z';

			_socket = socket;
			_socket.BeginReceive(byteBuffer, 0, byteBufferSize, SocketFlags.None, new AsyncCallback(MessageRecieved), null);
		}

		public void Close()
		{
			// Make sure that we close the client socket
			if (_socket.Connected)
			{
				_socket.Shutdown(SocketShutdown.Both);
			}
			_socket.Close();
		}

		public void Send(string message)
		{
			Console.WriteLine("Sending Message: \'" + message.Replace("\r", "\n") + "\' To: " + this.Id);
			try
			{
				// Since sockets can only send arrays of bytes, we use the
				// UTF8 encoder to convert our string to an array of bytes.
				// Notice that we postfix our message with a carriage return
				// + line feed. We do this to be able to figure out when
				// a message ends. In case there will be an error with the
				// socket, we enclose the call inside a try catch.
				_socket.Send(Encoding.UTF8.GetBytes(message + "\n\r"));
			}
			catch (Exception)
			{
			}
		}



		private void MessageRecieved(IAsyncResult ar)
		{
			try
			{
				// The process of reading a message from the stream is as follows.
				// First we check how many bytes that the socket has read. After
				// that we convert these bytes to a string and append the string
				// to a temporary string. We then need to clear our buffer, so that
				// new data can be received later. We then loop through each message
				// that is available in the temp string. Remember that all messages
				// is posfixed with a carriage return + line feed. Also notice that we
				// use the UTF8 decoder to convert our byte array back to a string.
				// It's very important that you encode and decode with the same
				// encoder/decoder.

				int bytesRecieved = _socket.EndReceive(ar);
			
				messageBuffer	+= Encoding.UTF8.GetString(byteBuffer, 0, bytesRecieved);
				byteBuffer		 = new byte[byteBufferSize];

				int pos;
				while ((pos = messageBuffer.IndexOf("\n\r")) != -1)
				{
					string message = messageBuffer.Substring(0, pos);
					messageBuffer = messageBuffer.Remove(0, pos+2);

					// We received a message, fire the event
					OnLineReceived(new LineReceivedEventArgs(this, message));
				}

				// Continue to receive data asynchronously
				byteBuffer = new byte[1024];
				_socket.BeginReceive(byteBuffer, 0, byteBuffer.Length, SocketFlags.None, new AsyncCallback(MessageRecieved), null);
			}
			catch (ObjectDisposedException)
			{
				// If we shutdown the socket the EndReceive-method will throw this exception.
				// We don't need to care about this exception, but we use the try catch
				// block, just so our application wont crash.
			}
			catch (SocketException)
			{
				// When we receive a SocketException when trying to read data, it means
				// that we no longer have contact with the client, so therefore we (the server)
				// sends the LOST-command.
				OnLineReceived(new LineReceivedEventArgs(this, "LOST"));
			}
		}

		private void OnLineReceived(LineReceivedEventArgs e)
		{
			// We must do a check here to see if anyone has hooked up the event. Because
			// if none has, it's no point in raising the event.
			if (LineReceived != null)
			{
				LineReceived(this, e);
			}
		}


		public Socket ClientSocket
		{
			get { return this._socket; }
		}

		public string Id
		{
			set { _id = value; }
			get { return string.Copy(_id); }
		}

		public string Email
		{
			set { _email = value; }
			get { return string.Copy(_email); }
		}

		public char Status
		{
			get { return _status; }
			set { _status = value; }
		}

		public string DisplayName
		{
			set { _displayName = value; }
			get { return string.Copy(_displayName); }
		}

		public  byte []ByteBuffer
		{
			get { return byteBuffer; }
		}
	}


	public class ViaConnection
	{
		private Socket _socket;
		private Socket _contactSocket;

		private byte[]	byteBuffer = new byte[1024];
		private byte [] contactByteBuffer = new byte[1024];
		
		public ViaConnection(Socket socket, Socket contactSocket)
		{
			_socket = socket;
			_socket.BeginReceive(byteBuffer, 0, byteBuffer.Length, SocketFlags.None, new AsyncCallback(MessageRecieved), null);
			_socket.Send(Encoding.UTF8.GetBytes("SEND_FILE" + "\n\r"));

			_contactSocket = contactSocket;
			_contactSocket.BeginReceive(contactByteBuffer, 0, contactByteBuffer.Length, SocketFlags.None, new AsyncCallback(DGPContactMessageRecieved), null);
			_contactSocket.Send(Encoding.UTF8.GetBytes("SEND_FILE" + "\n\r"));
		}

		private void MessageRecieved(IAsyncResult ar)
		{
			int recivedBytes = _socket.EndReceive(ar);
			if(  _contactSocket.Connected == true)
			{
				Console.WriteLine(_socket.RemoteEndPoint.ToString() + " : " + recivedBytes + " : \'" + Encoding.UTF8.GetString(byteBuffer, 0, recivedBytes)+"\'");
				_contactSocket.Send(byteBuffer, 0, recivedBytes, SocketFlags.None);
			}
			byteBuffer = new byte[1024];
			_socket.BeginReceive(byteBuffer, 0, byteBuffer.Length, SocketFlags.None, new AsyncCallback(MessageRecieved), null);
		}

		private void DGPContactMessageRecieved(IAsyncResult ar)
		{
			int recivedBytes = _contactSocket.EndReceive(ar);
			if( _socket != null && _socket.Connected == true)	
			{
				Console.WriteLine(_contactSocket.RemoteEndPoint.ToString() + " : " + recivedBytes + " : \'"+ Encoding.UTF8.GetString(contactByteBuffer, 0, recivedBytes)+"\'");
				_socket.Send(contactByteBuffer, 0, recivedBytes, SocketFlags.None);
			}
			contactByteBuffer = new byte[1024];
			_contactSocket.BeginReceive(contactByteBuffer, 0, contactByteBuffer.Length, SocketFlags.None, new AsyncCallback(DGPContactMessageRecieved), null);
		}

		public Socket DGPContactSocket
		{
			get { return _contactSocket; }
		}

		public Socket Socket
		{
			get { return _socket; }
		}
	}

	public class PendingViaConnection 
	{
		private Socket _socket;
		private byte[]	byteBuffer = new byte[1024];
		private string fileTransferId;
		private string messageBuffer = "";

		public delegate void GotFileTransferId(object sender);

		public event GotFileTransferId gotFileTransferId;

		public PendingViaConnection (Socket socket)
		{
	//		Console.WriteLine("PendingViaConnection!");
			this._socket = socket;
			_socket.BeginReceive(byteBuffer, 0, byteBuffer.Length, SocketFlags.None, new AsyncCallback(MessageRecieved), null);
		}

		private void MessageRecieved(IAsyncResult ar)
		{
			int bytesRecieved = _socket.EndReceive(ar);
			messageBuffer	+= Encoding.UTF8.GetString(byteBuffer, 0, bytesRecieved);

			int pos;
			string message = "";
			while ((pos = messageBuffer.IndexOf("\n\r")) != -1)
			{
				message = messageBuffer.Substring(0, pos);
				messageBuffer = messageBuffer.Remove(0, pos+2);

				//Console.WriteLine("Message! " + message);

				if(message.StartsWith("FILE_TRANSFER_ID"))
				{

					fileTransferId = message.Substring("FILE_TRANSFER_ID".Length).Trim();

	//				Console.WriteLine("Got contact Id! " + fileTransferId);
					
					if(gotFileTransferId != null)
					{
						gotFileTransferId(this);
					}

				}
			}
			byteBuffer = new byte[1024];
			_socket.BeginReceive(byteBuffer, 0, byteBuffer.Length, SocketFlags.None, new AsyncCallback(MessageRecieved), null);
		}

		public string FileTransferId
		{
			get { return fileTransferId; }
		}

		public Socket socket
		{
			get { return this._socket; }
		}
	}
}
