using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace DGP_Messenger_Server
{
	/// <summary>
	/// Summary description for ChatSocketServer.
	/// </summary>
	public class ChatSocketServer
	{
		#region Members

		public readonly int iPort;
		public readonly int iViaPort;

		private int fileTransferId = 0;


		private Socket serverSocket;
		private Socket viaSocket;
		private ArrayList clientList			= new ArrayList();
		private ArrayList viaClientList			= new ArrayList();
		private ArrayList pendingViaConnections = new ArrayList();
		#endregion

		#region Constructor
		public ChatSocketServer(int port)
		{
			this.iPort = port;

		}
		#endregion

		#region Funtions

		public int Start()
		{
			try
			{
				serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint localEP = new IPEndPoint(IPAddress.Any, iPort);			
				serverSocket.Bind(localEP);
				serverSocket.Listen(10);
				serverSocket.BeginAccept(new AsyncCallback(ConnectionRecived), serverSocket);

				/*viaSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint viaLocalEP = new IPEndPoint(IPAddress.Any, iViaPort);			
				viaSocket.Bind(viaLocalEP);
				viaSocket.Listen(100);			
				viaSocket.BeginAccept(new AsyncCallback(ViaConnectionRecived), viaSocket);
			*/}
			catch(Exception ex)
			{
				return 0;
			}
			return 1;
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

		#region LineRecived

		private void ClientLineReceived(object sender, LineReceivedEventArgs e)
		{
			// Print the client's message in the console just so we can se what's happening
			Console.WriteLine("Message: " + e.Message);
/*
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
				}
			}
			else if(e.Message.StartsWith("SEND_FILE_VIA_SERVER"))
			{
				string [] stringBuffer = e.Message.Substring("SEND_FILE_VIA_SERVER".Length).Trim().Split('\r');
				string id = stringBuffer[0];
				string fileTransid = stringBuffer[1];
				SendMessageToClient("RECIVEFILE_VIA_SERVER" + e.Client.Id + '\r' + fileTransid, id);
			}*/
		}

		#endregion

		public int Stop()
		{
			if(serverSocket.Connected)
			{
				serverSocket.Shutdown(SocketShutdown.Both);
			}
			serverSocket.Close();
			return 1;
		}
		#endregion

		#region Properties

		public Socket socket
		{
			get {return this.serverSocket; }
		}
		#endregion

	}
}
