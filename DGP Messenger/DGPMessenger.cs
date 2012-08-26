using System;
using System.Collections;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;


namespace DGP_Messenger
{
	/// <summary>
	/// Summary description for DGPMessenger.
	/// </summary>
	public class DGPMessenger
	{
		#region Constants
		
		private const int PORT = 1080;

		#endregion
		
		// Variables used to receive data from the socket
		private Socket	clientSocket;
		private int		byteBufferSize;
		private byte[]	byteBuffer;	
		private string	messageBuffer;
		
		public  User		user;
		public ArrayList Contacts = new ArrayList();

		private bool loggingIn;
		private bool _connected;

		public delegate void LogginErrorEvent(object sender, DGPLogginEventArgs e);
		public delegate void LogginComplete(object sender, EventArgs e);
		public delegate void UpdateContactEvent(object sender, ContactUpdateArgs e);
		public delegate void RecivedMessageEvent(object sender, RecivedMessageEventArgs e);
	
		public event LogginErrorEvent logginErrorEvent;
		public event LogginComplete logginComplete;
		public event UpdateContactEvent updateContact;
		public event RecivedMessageEvent recivedMessage;

		public DGPMessenger()
		{
			byteBufferSize	= 1024;
			byteBuffer		= new byte[byteBufferSize];
			messageBuffer	= "";

			user = new User();

			loggingIn = false;
			_connected = false;
		}


		public void Disconnect()
		{
			if (clientSocket != null)
			{
				if (clientSocket.Connected)
				{
					SendMessage("QUIT");
					clientSocket.Shutdown(SocketShutdown.Both);
				}
				clientSocket.Close();
			}
			_connected = false;
		}


		public void Connect(string name, string password)
		{
				Connect(name, password, "armada", 1080);
		}
		
		public void Connect(string name, string password, string adress, short port)
		{
			if(loggingIn == false)
			{
				clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

				IPEndPoint remoteEP = new IPEndPoint(Dns.Resolve(adress.Trim().ToUpper()).AddressList[0], port);

				try
				{
					clientSocket.Connect(remoteEP);
				}
				catch(SocketException)
				{
					if(logginErrorEvent != null)
					{
						logginErrorEvent(this, new DGPLogginEventArgs(DGPLogginCodes.ConnectionError));
					}
					Disconnect();
					return;
				}

				clientSocket.Send(Encoding.UTF8.GetBytes("LOGGIN "+ name + "/" + password + "\n\r"));
				clientSocket.BeginReceive(byteBuffer, 0, byteBufferSize, SocketFlags.None, new AsyncCallback(MessageRecieved), null);
				loggingIn = true;
			}
			_connected = true;
		}


		private void HandleRecivedMessage(string initMessage)
		{
			Color	messageColor;
			string	itsMessage;
			//string	contactName;

			string messageBuffer	= initMessage.Substring("MSG ".Length);
			bool returned = false;
			if(messageBuffer.StartsWith("UNDELIVERED "))
			{
				messageBuffer		= messageBuffer.Remove(0,  "UNDELIVERED ".Length);
				returned = true;
			}
			int endMessageIndex		= messageBuffer.IndexOf(" END_MESSAGE\r", 0, messageBuffer.Length);
			string message			= messageBuffer.Substring(0, endMessageIndex);
			messageBuffer			= messageBuffer.Remove(0,  endMessageIndex + " END_MESSAGE\r".Length);
			string [] messageBuffers = messageBuffer.Split('\r');
			string contactId;
			string toId;


			if(returned)
			{
				messageColor = Color.Red;
				itsMessage =	"Kunde inte leverera följande meddelande till alla som är med i konverationen: " + 
					message.Trim() + '\n';
				contactId = messageBuffers[0].Trim();
				toId =  messageBuffers[1].Trim();
			}
			else
			{
				messageColor = Color.Black;
				itsMessage = message.Trim();
				contactId = messageBuffers[1].Trim();
				toId =  messageBuffers[0].Trim();
			}

			if(recivedMessage != null)
			{
				recivedMessage(this, new RecivedMessageEventArgs(contactId, toId, itsMessage));
			}
		}


		private void MessageRecieved(IAsyncResult ar)
		{
			try
			{
				#region Comment
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

				#endregion

				int bytesRecieved = clientSocket.EndReceive(ar);
			
				messageBuffer	+= Encoding.UTF8.GetString(byteBuffer, 0, bytesRecieved);
				byteBuffer		 = new byte[byteBufferSize];

				int pos;
				string message = "";
				while ((pos = messageBuffer.IndexOf("\n\r")) != -1)
				{
					message = messageBuffer.Substring(0, pos);
					messageBuffer = messageBuffer.Remove(0, pos+2);
					
					this.ClientLineReceived( message );
				}

				// Continue to receive data asynchronously
				byteBuffer = new byte[byteBufferSize];
				clientSocket.BeginReceive(byteBuffer, 0, byteBuffer.Length, SocketFlags.None, new AsyncCallback(MessageRecieved), null);
			}
			catch (ObjectDisposedException)
			{
				#region Comment
				// If we shutdown the socket the EndReceive-method will throw this exception.
				// We don't need to care about this exception, but we use the try catch
				// block, just so our application wont crash.
				#endregion
			}
			catch (SocketException)
			{
				#region Comment
				// When we receive a SocketException when trying to read data, it means
				// that we no longer have contact with the client, so therefore we (the server)
				// sends the LOST-command.
				#endregion
				
				this.ClientLineReceived( "LOST" );
			}
		}	


		private void ClientLineReceived( string message)
		{
			if(message.StartsWith("LOGGIN"))
			{
				if(message.Substring("LOGGIN ".Length).StartsWith("TRUE"))
				{
					string [] messageBuffer = message.Substring("LOGGIN TRUE ".Length).Split('\r');
					new User(messageBuffer[1].Trim(), messageBuffer[0].Trim(),OnlineStatus.ONLINE, "");
					user.DisplayName = messageBuffer[0].Trim();
					user.Status.Status = OnlineStatus.ONLINE;
					user.Id = messageBuffer[1].Trim();
					SendMessage("GETUSERLIST");
					SendMessage("UPDATE_FRIEND_USERLISTS");

					if(logginComplete != null)
					{
						logginComplete(this, null);
					}
				}
				else
				{
					if(logginErrorEvent != null)
					{
						logginErrorEvent(this, new DGPLogginEventArgs(DGPLogginCodes.WrongPassUser));
					}
					Disconnect();
				}
			}
			else if(message.StartsWith("USERLIST"))
			{
				UpdateContactList(message.Substring("USERLIST ".Length));
			}
			else if(message.StartsWith("UPDATE_USERLIST"))
			{
				SendMessage("GETUSERLIST");
			}
			else if(message.StartsWith("UPDATE_FRIEND "))
			{
				UpdateContact(message.Substring("UPDATE_FRIEND ".Length));	
			}
			else if(message.StartsWith("MSG "))
			{
				HandleRecivedMessage(message);
			}
			else if(message.StartsWith("LOST"))
			{
				MessageBox.Show("Lost Connection With Server!", "Connection ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				//this.Show();
				//	this.BringToFront();
				//Loggout();
			}
			else if(message.StartsWith("SEND_FILE_TO_IP"))
			{
				//StringBuffer[0] : Client ID
				//StringBuffer[1] : Client IP
				//StringBuffer[2] : FileTransferId
				string [] stringBuffer = message.Substring("SEND_FILE_TO_IP".Length).Split('\r');
				/*MessageForm mf = GetMessageForm(stringBuffer[1]);
				if(stringBuffer[0].StartsWith("_ERORR"))
				{
					mf.DisplayMessage("Fel Vid försök att skicka fil!", Color.Red, "", false); 
				}
				else
				{
					//MessageBox.Show("Send File To IP: " + stringBuffer[0].Substring(0, 16));
					string [] ipString = stringBuffer[0].Split(':');	// Split to ip and port
					mf.SendFileTo(ipString[0].Trim(), stringBuffer[2]);
				}*/
			
			}
			else if(message.StartsWith("PREPARE_RECIVE_FILE "))
			{
				string [] stringBuffer = message.Substring("PREPARE_RECIVE_FILE ".Length).Trim().Split('\r');
				string id = stringBuffer[0];
				string fileTransId = stringBuffer[1];

				PrepareReciveFile(id, fileTransId);
			}
			else if(message.StartsWith("RECIVEFILE_VIA_SERVER"))
			{
				string [] stringBuffer = message.Substring("RECIVEFILE_VIA_SERVER".Length).Trim().Split('\r');
				string id = stringBuffer[0];
				string fileTransId = stringBuffer[1];

				PrepareReciveFileViaServer(id, fileTransId);
			}
		}


		public void SendMessage(string message)
		{
			if(clientSocket != null && clientSocket.Connected)
			{
				clientSocket.Send(Encoding.UTF8.GetBytes(message + "\n\r"));
			}
		}
		public void SetDisplayName(string name)
		{
			user.DisplayName = name;
			SendMessage("UPDATENAME " + name);
		}

		public void SetStatus(OnlineStatus status)
		{
			this.user.Status = status;
			SendMessage("UPDATE_ONLINE_STATUS " + status.Status);
		}

		private void UpdateContactList(string contactsString)
		{
			Contacts.Clear();
			
			string [] contactList = contactsString.Split('\n');

			if(contactList.Length > 2)
			{
				for(int i = 0; i < contactList.Length; i += 3)
				{
					DGPContact contact = new DGPContact(contactList[i], contactList[i+1], contactList[i+2][0]);
					Contacts.Add(contact);			
					updateContact(this, new ContactUpdateArgs(contact));
				}
			}
		}


		private void UpdateContact(string message)
		{	
			// messageBuffer[0] == ID
			// messageBuffer[1] == Name
			// messageBuffer[2][0] == Status
			string [] messageBuffer = message.Split('\n');
		
			DGPContact contact = new DGPContact(messageBuffer[0], messageBuffer[1], messageBuffer[2][0]);
			
			if(updateContact != null)
			{
				updateContact(this, new ContactUpdateArgs(contact));
			}
		}


		public Socket ServerSocket
		{
			get { return clientSocket; }
		}

		public bool Connected
		{
			get { return _connected; }
		}

		#region FileTransfer

		public void PrepareReciveFile(string id, string fileTransferId)
		{
			try
			{
				Socket reciveConnection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint localEP = new IPEndPoint(IPAddress.Any, 4046);
				reciveConnection.Bind(localEP);
				reciveConnection.Listen(10);
				reciveConnection.BeginAccept(new AsyncCallback(ReciveConnection), new SocketState(id, fileTransferId, reciveConnection));
	
				//TODO: MessageForm!
				/*MessageForm mf = GetMessageForm(id);
				mf.DisplayMessage("Lyssnar!", Color.SaddleBrown, "", false);
				*/
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n"+ ex.HelpLink + "\n" + ex.StackTrace + "\n"+ ex.Source, "Prepare recive");
			}
		}


		public void PrepareReciveFileViaServer(string id, string fileTransId)
		{
			try
			{
				Socket reciveConnection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint remoteEP = new IPEndPoint(Dns.Resolve("siti.mine.nu").AddressList[0], 4045);
				reciveConnection.BeginConnect(remoteEP, new AsyncCallback(ReciveConnectionConnected), new SocketState(id, fileTransId, reciveConnection));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n"+ ex.HelpLink, "Prepare recive VIA Server");
			}
		}
		

		private void ReciveConnection(IAsyncResult ar)
		{
			try
			{
				SocketState socketState = (SocketState)ar.AsyncState;

				//TODO: Reciveconnection in filetransfer doesnt have to connect to a messageform
				/*MessageForm mf = GetMessageForm(socketState.id);
				//mf.Invoke(new MessageForm.ReciveNewFileDelegate(mf.ReciveNewFile), new object [] {socketState.socket.EndAccept(ar), socketState.fileTransferId, false} );
				
				mf.ReciveNewFile(socketState.socket.EndAccept(ar), socketState.fileTransferId, false);

				socketState.socket.Close();
*/
				//this.Invoke(new DisplayMessageDelegate(this.DisplayMessage), new  object []{"Kontakt!", Color.Red, "", false});
			}
			catch (ObjectDisposedException)
			{
			}
			catch (InvalidOperationException ex)
			{
				MessageBox.Show(ex.Message +"\n"+ex.Source+"\n"+ex.InnerException+"\n"+ex.HelpLink, "InvalidOperationException");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message +"\n"+ex.Source+"\n"+ex.InnerException+"\n"+ex.HelpLink+"\n"+ex.TargetSite+"\n"+ex.StackTrace, "Exception");
			}
		}


		private void ReciveConnectionConnected(IAsyncResult ar)
		{
			try
			{
				SocketState socketState = (SocketState)ar.AsyncState;
				//TODO: Another messageform problem
				/*MessageForm mf = GetMessageForm(socketState.id);
				socketState.socket.EndConnect(ar);
				mf.ReciveNewFile(socketState.socket, socketState.fileTransferId, true);
				*/
			}
			catch (ObjectDisposedException)
			{
			}
			catch (InvalidOperationException ex)
			{
				MessageBox.Show(ex.Message);
			}
		}


		#endregion
	}
}
