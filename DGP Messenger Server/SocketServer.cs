using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace DGP_Messenger_Server
{
	/// <summary>
	/// Summary description for SocketServer.
	/// </summary>
	public class SocketServer
	{
		#region Members				//////////////////////

		public readonly int iPort;
		public readonly int iViaPort;
		public event LoggEventHandler LoggEvent;
		public event ClientRecivedEventHandler ClientRecived;

		private Socket serverSocket;
		
		protected ArrayList clientList			= new ArrayList();
		
		//private ArrayList viaClientList			= new ArrayList();
		//	private int fileTransferId = 0;		
		//	private Socket viaSocket;
		//	private ArrayList pendingViaConnections = new ArrayList();

		#endregion					//////////////////////

		#region Constructor			//////////////////////
		
		public SocketServer(int port)
		{
			this.iPort = port;
		}
		
		#endregion					//////////////////////

		#region Funktions

		public int Start()
		{
			try
			{
				serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint localEP = new IPEndPoint(Dns.Resolve(Dns.GetHostName()).AddressList[0], iPort);			
				serverSocket.Bind(localEP);
				serverSocket.Listen(10);
				serverSocket.BeginAccept(new AsyncCallback(ConnectionRecived), serverSocket);
//
				/*viaSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPEndPoint viaLocalEP = new IPEndPoint(IPAddress.Any, iViaPort);			
				viaSocket.Bind(viaLocalEP);
				viaSocket.Listen(100);			
				viaSocket.BeginAccept(new AsyncCallback(ViaConnectionRecived), viaSocket);
			*/
			}
			catch(Exception)
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

				if (ClientRecived != null)
				{
					ClientRecived(this, new ClientRecivedEventArgs(client));
				}

				clientList.Add(client);

				Logg("Client Connected From Ip: " + client.ClientSocket.RemoteEndPoint.ToString());

				serverSocket.BeginAccept(new AsyncCallback(ConnectionRecived), this);
			}
			catch (ObjectDisposedException)
			{
			}
		}

		public int Stop()
		{
			if(serverSocket.Connected)
			{
				serverSocket.Shutdown(SocketShutdown.Both);
			}
			serverSocket.Close();
			return 1;
		}

		protected void Logg(string s)
		{
			if (LoggEvent != null)
			{
				LoggEvent(this, new LoggEventArgs(s));
			}		
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
