using System;

namespace DGP_Messenger_Server
{
	/// <summary>
	/// Summary description for ChatServer.
	/// </summary>
	public class ChatServer : SocketServer
	{

		public ChatServer(int port) : base(port)
		{
			this.ClientRecived += new ClientRecivedEventHandler(ChatServer_ClientRecived); 
		}

		private void ChatServer_ClientRecived(object sender, ClientRecivedEventArgs e)
		{
			e.Client.LineRecived += new LineRecivedEventHandler(ClientLineRecived);
		}

		private void ClientLineRecived(object sender, LineRecivedEventArgs e)
		{
			// Print the client's message in the console just so we can se what's happening
			//Logg("Message: " + e.Message);

			// Check the message prefix to see what kind of message the client sent
			if (e.Message.StartsWith("LOGGIN"))
			{
				string messageBuffer = e.Message.Substring("LOGGIN ".Length);
				int pos = messageBuffer.IndexOf("/");
				if(pos != -1)
				{
					string name		= messageBuffer.Substring(0, pos);
					string password = messageBuffer.Remove(0, pos+1);

					if(password != "")
					{
						e.Client.Send("LOGGIN FALSE");
						Logg("LOGGIN ERROR: Ej chatbehhörig");
					}

					
					e.Client.DisplayName = name;
					e.Client.Id = getNewClientId().ToString();
					e.Client.Status = 'C';
					e.Client.Send("LOGGIN TRUE " + e.Client.DisplayName + "\r" + e.Client.Id);

					Logg("Client Id: \'" + e.Client.Id + "\' -  Name: \'" + name + "\'  -   Password: \'password\'  Loggin TRUE");
					
					UpdateUserLists();
				}
				else
				{
					e.Client.Send("LOGGIN FALSE");
					Logg("LOGGIN ERROR: Inget Namn");
				}
			}
			else if (e.Message.StartsWith("GETUSERLIST"))
			{
				string message = "USERLIST ";
				
				message += "0\nAll\nC";	// Server User, Send messages to everybody

				for( int i = 0; i < this.clientList.Count; i++)
				{
					message += "\n";
					Client client = (Client)clientList[i];
					message += client.Id;
					message += "\n";
					message += client.DisplayName;
					message += "\n";
					message += client.Status;
				}
			//	Logg("GetUserList  -  Id: " + e.Client.Id);
				e.Client.Send(message);
			}
			else if (e.Message.StartsWith("LOST") || e.Message.StartsWith("QUIT"))
			{
				e.Client.Close();
				clientList.Remove(e.Client);
				Logg("Client Removed: " + e.Client.DisplayName + "  -  Clients Left: " + clientList.Count);
				UpdateUserLists();
			}
			else if (e.Message.StartsWith("MSG"))
			{
				string messageBuffer	= e.Message.Substring("MSG ".Length);
				int endMessageIndex		= messageBuffer.IndexOf(" END_MESSAGE\r", 0, messageBuffer.Length);
				string message			= messageBuffer.Substring(0, endMessageIndex);
				messageBuffer			= messageBuffer.Remove(0,  endMessageIndex + " END_MESSAGE\r".Length);
				string [] messageBuffers = messageBuffer.Split('\r');
				string contactId			= messageBuffers[0].Trim();


			//	Logg("\'Contact ID: " + messageBuffer + "\'");
				
				if(SendMessageToClient(e.Message, contactId) == false)
				{	// If not delivered
					// Return the message
					SendMessageToClient("MSG UNDELIVERED " + e.Message.Substring("MSG ".Length), e.Client.Id);
				}
			}
			else if(e.Message.StartsWith("UPDATENAME"))
			{
				string messageBuffer = string.Copy(e.Message.Substring("UPDATENAME ".Length));
				e.Client.DisplayName = string.Copy(messageBuffer);
				UpdateUserLists();
			}
			/*else if(e.Message.StartsWith("SEND_FILE_TO"))
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
				if(SendMessageToClient(e.Message, id) != true)
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

		private void UpdateUserLists()
		{
			foreach(Client client in clientList)
			{
				client.Send("UPDATE_USERLIST");
			}
		}

		private Client getClient(int id)
		{
			for( int i = 0; i < this.clientList.Count; i++)
			{
				Client client = (Client)clientList[i];
				if(client.Id.Length > 0 && int.Parse(client.Id) == id)
				{
					return client;
				}
			}
			return null;
		}

		private int getNewClientId()
		{
			for( int i = 1; i < this.clientList.Count+1; i++)
			{
				if(getClient(i) == null)
				{
					return i;
				}	
			}
			return this.clientList.Count+1;
		}

		private bool SendMessageToClient(string message, string id)
		{
			foreach(Client client in clientList)
			{
				if(client.Id == id || id == "0")		//If id == 0 send to everybody
				{
					client.Send(message);
					return true;
				}
			}
			return false;
		}
	}
}
