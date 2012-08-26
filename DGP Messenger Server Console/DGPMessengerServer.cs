using System;
using System.Net;
//using System.Data;
using System.Text;
using System.Net.Sockets;
using System.Collections;



namespace DGP_Messenger_Sever
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class DGPMessengerServer
	{
		// Web Database
		/*private const string DB_SERVER		= "mysql.web10.se";
		private const string DB_DATABASE	= "wse15655";
		private const string DB_USERNAME	= "wse15655";
		private const string DB_PASSWORD	= "197qw47131";
*/
		private const string DB_SERVER		= "mysql13.dgp.se";
		private const string DB_DATABASE	= "dgp_se";
		private const string DB_USERNAME	= "access@d1014";
		private const string DB_PASSWORD	= "uiop1122";


		private const int PORT = 1080;
		private const int VIAPORT = 4045;

		private int fileTransferId = 0;

		Socket serverSocket;
		Socket viaSocket;
		ArrayList clientList = new ArrayList();
		ArrayList viaClientList = new ArrayList();
		ArrayList pendingViaConnections = new ArrayList();

		public DGPMessengerServer()
		{
			if(StartServer() != 0)
			{
				while (true)
				{
					string read = Console.ReadLine();
					if (read.ToLower().Equals("exit"))
					{
						break;
					}
					else if(read.StartsWith("MSG "))
					{
						Console.WriteLine("Tar hand om msg");
						int pos = read.IndexOf("TO");
						string message = read.Substring("MSG ".Length, pos - "MSG ".Length).Trim();
						string contact = read.Substring(pos + "TO ".Length).Trim();
						foreach(Client client in clientList)
						{
							//Console.WriteLine("Tar hand om msg");
							if(client.Id == contact)
							{
								client.Send("MSG " + message + " END_MESSAGE\r" + contact + "\r" + "SERVER");
								Console.WriteLine("MSG " + message + " END_MESSAGE\n" + contact + "\n" + "SERVER");
							}
						}
					}
				}
				StopServer();
			}
			else
			{
				Console.WriteLine("Press ENTER key to exit!");
				Console.Read();
			}
		}

		private int StartServer()
		{
			Console.WriteLine("_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _");
			Console.WriteLine("_-_-_-_-_-_-_-_-_-_-_-_-_-_-_ DGP Messenger Server -_-_-_-_-_-_-_-_-_-_-_-_-_-_");
			Console.WriteLine("_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_");
			Console.WriteLine("_-_-_-_-_-_-_-_-_-_-_- By: Sehr & the DGP Team Sweden _-_-_-_-_-_-_-_-_-_-_-_-_");
			Console.WriteLine("_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_");
			Console.WriteLine("_-_-_-_-_ Type \'exit\' and then press enter to end this application -_-_-_-_-_-_");
			Console.WriteLine("_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_");
			Console.WriteLine("");
			try
			{
				serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint localEP = new IPEndPoint(IPAddress.Any, PORT);			
				serverSocket.Bind(localEP);
				serverSocket.Listen(10);
				serverSocket.BeginAccept(new AsyncCallback(ConnectionRecived), serverSocket);

				viaSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint viaLocalEP = new IPEndPoint(IPAddress.Any, VIAPORT);			
				viaSocket.Bind(viaLocalEP);
				viaSocket.Listen(100);			
				viaSocket.BeginAccept(new AsyncCallback(ViaConnectionRecived), viaSocket);

				Console.WriteLine("Listening for clients to connect on ip: " + serverSocket.LocalEndPoint.ToString());
			}
			catch(Exception)
			{
				Console.WriteLine("Failed to set upp Socket");
				return 0;
			}

			Database db = new Database(DB_SERVER, DB_DATABASE, DB_USERNAME, DB_PASSWORD);
			db.cleanOnlineStatus();
			db.close();

			return 1;
		}

		private void StopServer()
		{
			if(serverSocket.Connected)
			{
				serverSocket.Shutdown(SocketShutdown.Both);
			}
			serverSocket.Close();
			
		}

		private void ConnectionRecived(IAsyncResult ar)
		{
			try
			{
				Client client = new Client(serverSocket.EndAccept(ar));

				client.LineReceived += new LineReceivedEventHandler(ClientLineReceived);

				clientList.Add(client);

				Console.WriteLine("Client Connected From Ip: " + client.ClientSocket.RemoteEndPoint.ToString());

				serverSocket.BeginAccept(new AsyncCallback(ConnectionRecived), this);
			}
			catch (ObjectDisposedException)
			{
			}
		}

		private void ViaConnectionRecived(IAsyncResult ar)
		{
			try
			{
				PendingViaConnection pendingConnection = new PendingViaConnection(viaSocket.EndAccept(ar));
				pendingConnection.gotFileTransferId += new DGP_Messenger_Sever.PendingViaConnection.GotFileTransferId(pendingConnection_gotFileTransferId);
				Console.WriteLine("Via server Astablished From Ip: " + pendingConnection.socket.RemoteEndPoint.ToString());
				

				/*foreach(Client client in clientList)
				{
					if(viaClient.Socket.RemoteEndPoint.ToString() == client.ClientSocket.RemoteEndPoint.ToString())
					{
						foreach(ViaClient via in viaClientList)
						{
							if(via.DGPContactId == client.Id)
							{
								ViaClient viaClient = new ViaClient(client.socket, contact.socket);
								//via.DGPContactSocket = viaClient.Socket;
								Console.WriteLine("Two client connected!");
								found = true;
							}
						}
					}
				}

				if(found == false)
				{
					viaClientList.Add(viaClient);
					Console.WriteLine("New viaClient created!");
				}*/
				viaSocket.BeginAccept(new AsyncCallback(ViaConnectionRecived), this);
			}
			catch (ObjectDisposedException)
			{
			}
		}

		private void ClientLineReceived(object sender, LineReceivedEventArgs e)
		{
			// Print the client's message in the console just so we can se what's happening
			 Console.WriteLine("Message: " + e.Message);

			// Check the message prefix to see what kind of message the client sent
			if (e.Message.StartsWith("LOGGIN"))
			{
				string messageBuffer = e.Message.Substring("LOGGIN ".Length);
				int pos = messageBuffer.IndexOf("/");
				if(pos != -1)
				{
					string name		= messageBuffer.Substring(0, pos);
					string password = messageBuffer.Remove(0, pos+1);

					Console.WriteLine("Name: \'" + name + "\'     Password: \'password\'");
					Database db		= new Database(DB_SERVER, DB_DATABASE, DB_USERNAME, DB_PASSWORD);
					if(db.testNameAndPassword(name, password, e.Client))
					{
						SetUserOnlineStatus('A', e.Client);
						e.Client.Send("LOGGIN TRUE " + e.Client.DisplayName + "\r" + e.Client.Id);
						Console.WriteLine("Loggin TRUE");
					}
					else
					{
						e.Client.Send("LOGGIN FALSE");
						Console.WriteLine("Loggin FALSE");
					}
					db.close();
				}
				else
				{
					e.Client.Send("LOGGIN FALSE");
					Console.WriteLine("LOGGIN ERROR");
				}
			}
			else if (e.Message.StartsWith("GETUSERLIST"))
			{
				Database db		= new Database(DB_SERVER, DB_DATABASE, DB_USERNAME, DB_PASSWORD);
				string [] contactIds = (string []) db.getDGPContacts(e.Client.Id).ToArray(typeof(string));

				string message = "USERLIST ";
				
				for( int i = 0; i < contactIds.Length; i++)
				{
					if(i != 0)
					{
						message += "\n";
					}
					message += contactIds[i].ToString();
					message += "\n";
					message += db.getDisplayName(contactIds[i].ToString());
					message += "\n";
					
					message += db.getUserOnlineStatus(contactIds[i]);
				}
				e.Client.Send(message);
				db.close();
			}
			else if (e.Message.StartsWith("LOST") || e.Message.StartsWith("QUIT"))
			{
				SetUserOnlineStatus('Z', e.Client);
				
				e.Client.Close();
				clientList.Remove(e.Client);
				Console.WriteLine("Client Removed: " + e.Client.DisplayName);
				Console.WriteLine("Clients Left: " + clientList.Count);

				UpdateDGPContactsUserList(e.Client);
			}
			else if (e.Message.StartsWith("MSG"))
			{
				string messageBuffer	= e.Message.Substring("MSG ".Length);
				int endMessageIndex		= messageBuffer.IndexOf(" END_MESSAGE\r", 0, messageBuffer.Length);
				string message			= messageBuffer.Substring(0, endMessageIndex);
				messageBuffer			= messageBuffer.Remove(0,  endMessageIndex + " END_MESSAGE\r".Length);
				string [] messageBuffers = messageBuffer.Split('\r');
				string contactId			= messageBuffers[0].Trim();

				
				if(SendMessageToClient(e.Message, contactId) == false)
				{	// If not delivered
					// Return the message
					SendMessageToClient("MSG UNDELIVERED " + e.Message.Substring("MSG ".Length), e.Client.Id);
				}
			}
			else if(e.Message.StartsWith("UPDATENAME"))
			{
				string messageBuffer = string.Copy(e.Message.Substring("UPDATENAME ".Length));
				Database db		= new Database(DB_SERVER, DB_DATABASE, DB_USERNAME, DB_PASSWORD);
				db.setUserDisplayName(e.Client.Id.ToString(), messageBuffer);
				e.Client.DisplayName = string.Copy(messageBuffer);
				db.close();
				UpdateDGPContactsUserList(e.Client);
			}
			else if(e.Message.StartsWith("UPDATE_FRIEND_USERLISTS"))
			{
				UpdateDGPContactsUserList(e.Client);
			}
			else if(e.Message.StartsWith("UPDATE_ONLINE_STATUS "))
			{
				string stringBuffer = e.Message.Substring("UPDATE_ONLINE_STATUS ".Length).Trim();
				char onlineStatus = stringBuffer[0];
				SetUserOnlineStatus(onlineStatus, e.Client);
				UpdateDGPContactsUserList(e.Client);
			}
			else if(e.Message.StartsWith("SEND_FILE_TO"))
			{
				string id = e.Message.Substring("SEND_FILE_TO".Length).Trim();
				string clientIp = GetClientIp(id);
				if(clientIp != "")
				{
					SendMessageToClient("PREPARE_RECIVE_FILE " + e.Client.Id + '\r' + fileTransferId, id);
					SendMessageToClient("SEND_FILE_TO_IP " + clientIp + '\r' + id + '\r' + fileTransferId, e.Client.Id);
					fileTransferId++;
				}
				else
				{
					SendMessageToClient("SEND_FILE_TO_IP_ERROR " + '\r' + id, e.Client.Id);
				}
				/*if(SendMessageToClient(e.Message, id) != true)
				{
					SendMessageToClient("MSG UNDELIVERED Kunde inte skicka inbjudan om fildelning", e.Client.Id);
				}*/
			}
			else if(e.Message.StartsWith("SEND_FILE_VIA_SERVER"))
			{
				string [] stringBuffer = e.Message.Substring("SEND_FILE_VIA_SERVER".Length).Trim().Split('\r');
				string id = stringBuffer[0];
				string fileTransid = stringBuffer[1];
				SendMessageToClient("RECIVEFILE_VIA_SERVER" + e.Client.Id + '\r' + fileTransid, id);
			}
		}

		private void UpdateDGPContactsUserList(Client user)
		{
			Database db		= new Database(DB_SERVER, DB_DATABASE, DB_USERNAME, DB_PASSWORD);
			string [] contactIds = (string []) db.getDGPContacts(user.Id).ToArray(typeof(string));
				
			for( int i = 0; i < contactIds.Length; i++)
			{
				foreach(Client client in clientList)
				{
					if(client.Id == contactIds[i])
					{
						string message =	"UPDATE_FRIEND " + user.Id + '\n' + 
											user.DisplayName + '\n' + user.Status;

						client.Send(message);
					}
				}
			}
			db.close();
		}

		private void SetUserOnlineStatus(char status, Client client)
		{
			client.Status = status;
			Database db		= new Database(DB_SERVER, DB_DATABASE, DB_USERNAME, DB_PASSWORD);
			db.setUserOnlineStatus(status, client.Id);
			db.close();
		}

		private bool SendMessageToClient(string message, string id)
		{
			foreach(Client client in clientList)
			{
				if(client.Id == id)
				{
					client.Send(message);
					//Console.WriteLine("Message: \'" + message + "\'   To: " + id);
					return true;
				}
			}
			return false;
		}

		private string GetClientIp(string id)
		{
			foreach(Client client in clientList)
			{
				if(client.Id == id)
				{
					return client.ClientSocket.RemoteEndPoint.ToString();
				}
			}
			return "";
		}
			


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			new DGPMessengerServer();
			//Console.Read();
		}

		private void pendingConnection_gotFileTransferId(object sender)
		{
			PendingViaConnection eventPvc = (PendingViaConnection)sender;
			Console.WriteLine("Pending Event! " + eventPvc.socket.RemoteEndPoint.ToString());
			int i = 0;
			bool found = false;
			for( ; i < pendingViaConnections.Count && !found; i++)
			{
				PendingViaConnection pvc = (PendingViaConnection)pendingViaConnections[i];
				if(eventPvc.FileTransferId == pvc.FileTransferId)
				{
					ViaConnection tempVia = new ViaConnection(pvc.socket, eventPvc.socket);
					Console.WriteLine("Via Connection Established! \t" + eventPvc.socket.RemoteEndPoint.ToString() + '\t' + pvc.socket.RemoteEndPoint.ToString());
					pendingViaConnections.RemoveAt(i);
					found = true;
				}
			}
			if(found)
			{
			//	pendingViaConnections.RemoveAt(i);
			}
			else
			{
				pendingViaConnections.Add(eventPvc);
			}
		}
	}
}
