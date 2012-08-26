using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace DGP_Messenger
{
	public enum TransferStatus 
	{ 
		ERROR = 0,
		NULLDATA_ERROR,
		CLOSE,
		WAITING,
		//CONFIRM_SEND,
		CONFIRM_RECIVE, 
		START_RECIVE, 
		START_SEND, 
		FILE_RECIVING, 
		RECIVE_OK, 
		SOCKET_EXCEPTION,
		FILE_SENDING,
		FILE_ERROR,
		SEND_OK,
		SOCKET_SHUTDOWN
	} ;

	/// <summary>
	/// An abstract headclass for all filetransferclasses
	/// </summary>
	public abstract class FileTransfer
	{
		//Delegates
		public delegate void ConfirmRecive(object sender, EventArgs sfe);
		public delegate void SendFileStatus(object sender, SendFileEvent sfe);
		public delegate void TransferStarts(object sender, EventArgs sfe);
		public delegate void TransferComplete(object sender, EventArgs sfe);
		public delegate void TransferError(object sender, EventArgs sfe);
		public delegate void DataUpdate(object sender, EventArgs ev);

		// Events
		public event ConfirmRecive	ConfirmReciveEvent;
		public event TransferStarts StartTransferEvent;
		public event TransferComplete	TransferCompleteEvent;
		public event TransferError	TransferErrorEvent;
		public event SendFileStatus SendFileChangeEvent;
		public event DataUpdate		DataUpdateEvent;


		protected Timer			timer;
		protected long			timerTicks = 0;
		protected long			transferspeed = 0;
		protected long			recivedLastTick = 0;
		protected TransferStatus _transferStatus;



		public FileTransfer()
		{
			timer = new Timer();
			timer.Interval = 50;
			timer.Tick += new EventHandler(timer_Tick);
			_transferStatus = TransferStatus.WAITING;
		}
		
		public abstract void close();

		protected void SetSendFileStatus(TransferStatus fss)
		{
			if(SendFileChangeEvent != null)
			{
				_transferStatus = fss;
				SendFileChangeEvent(this, new SendFileEvent(fss));
			}

			if(	fss == TransferStatus.CONFIRM_RECIVE && 
				ConfirmReciveEvent != null)
			{
				ConfirmReciveEvent(this, new EventArgs());
			}
			else if((fss == TransferStatus.START_RECIVE || fss == TransferStatus.START_SEND) && 
				StartTransferEvent != null)
			{
				StartTransferEvent(this, new EventArgs());
			}
			else if((fss == TransferStatus.SEND_OK || fss == TransferStatus.RECIVE_OK) && 
				TransferCompleteEvent != null)
			{
				TransferCompleteEvent(this, new EventArgs());
			}
			else if((fss == TransferStatus.ERROR || fss == TransferStatus.FILE_ERROR || 
				fss == TransferStatus.SOCKET_EXCEPTION) && 
				TransferErrorEvent != null)
			{
				TransferErrorEvent(this, new EventArgs());
			}
		}

		protected void UpdateDataEvent()
		{
			if(DataUpdateEvent != null)
			{
				DataUpdateEvent(this, null);
			}
		}

		#region Properties

		public abstract String FileName
		{
			get ;
		}

		public abstract long TransferedBytes
		{
			get ;
		}

		public abstract  long FileSize
		{
			get ;
		}

		public long TransferSpeed
		{
			get { return transferspeed; }
		}

		public TransferStatus TransferStatus
		{
			get { return _transferStatus; }
		}

		public long TimeUsed
		{
			get { return timerTicks/*/20f*/; }
		}
		#endregion

		private void timer_Tick(object sender, EventArgs e)
		{
			timerTicks += 1;
		}
	}


	#region MSN FileTranfer class

	public class MSNFileTransfer : FileTransfer
	{
		DotMSN.FileTransfer msnFileTransfer;
		MessageForm		messageFormOwner;
		
		
		public MSNFileTransfer(DotMSN.FileTransfer initFileTransfer, MessageForm initForm)
		{
			msnFileTransfer = initFileTransfer;
								
			msnFileTransfer.Accepted			+= new DotMSN.FileTransfer.FileTransferHandler(fileTransfer_Accepted);
			msnFileTransfer.Completed			+= new DotMSN.FileTransfer.FileTransferHandler(fileTransfer_Completed);
			msnFileTransfer.InvitationCancelled += new DotMSN.FileTransfer.FileTransferInvitationCancelledHandler(fileTransfer_InvitationCancelled);
			msnFileTransfer.Progressing			+= new DotMSN.FileTransfer.FileTransferHandler(fileTransfer_Progressing);
			msnFileTransfer.Started				+= new DotMSN.FileTransfer.FileTransferHandler(fileTransfer_Started);
			msnFileTransfer.TransferCancelled	+= new DotMSN.FileTransfer.FileTransferCancelledHandler(fileTransfer_TransferCancelled);

			messageFormOwner = initForm;
		}

		public override void close()
		{
			this.msnFileTransfer.Cancel();
		}


		#region Events

		private void fileTransfer_Accepted(DotMSN.FileTransfer sender, EventArgs e)
		{
			messageFormOwner.DisplayMessage("File Accepted");
			UpdateDataEvent();
		}

		private void fileTransfer_Completed(DotMSN.FileTransfer sender, EventArgs e)
		{
			messageFormOwner.DisplayMessage("File Completed");
			timer.Stop();
			UpdateDataEvent();
		}

		private void fileTransfer_InvitationCancelled(DotMSN.FileTransfer sender, DotMSN.FileTransferInvitationCancelledEventArgs e)
		{
			messageFormOwner.DisplayMessage("fileTransfer_InvitationCancelled: " + e.CancelCode);
			UpdateDataEvent();
		}

		private void fileTransfer_Progressing(DotMSN.FileTransfer sender, EventArgs e)
		{
			if((this.recivedLastTick++ % 1000) == 0)
			{
				UpdateDataEvent();
				//messageFormOwner.DisplayMessage("fileTransfer_Progressing: " + sender.BytesProcessed);
			}
		}

		private void fileTransfer_Started(DotMSN.FileTransfer sender, EventArgs e)
		{
			messageFormOwner.DisplayMessage("fileTransfer_Started: " + sender.FileSize);
			timer.Start();
			UpdateDataEvent();
		}

		private void fileTransfer_TransferCancelled(DotMSN.FileTransfer sender, DotMSN.FileTransferCancelledEventArgs e)
		{
			messageFormOwner.DisplayMessage("fileTransfer_TransferCancelled: " + e.CancelCode);
			timer.Stop();
			UpdateDataEvent();
		}

		#endregion

		#region Methods


		#endregion

		#region Properties

		public override String FileName
		{
			get { return this.msnFileTransfer.FileName ; }
		}

		public override long TransferedBytes
		{
			get { long ret = this.msnFileTransfer.BytesProcessed; return ret; }
		}

		public override long FileSize
		{
			get { return this.msnFileTransfer.FileSize; }
		}

		#endregion
	}

	#endregion

	#region DGP Filetransfer Class

	public abstract class DGPFileTransfer : FileTransfer
	{
		protected Socket		socket;
		protected byte []		data	= new byte[1024];
		protected string		messageBuffer = "";
		protected bool			viaServer;
		protected bool			transferingFile = false;
		protected FileStream	fs;
		protected string		_fileTransferId;
		protected bool			isClosed = false;

		public DGPFileTransfer()
		{
			_transferStatus = TransferStatus.WAITING;
		}

		protected void baseClose()
		{
			SetSendFileStatus(TransferStatus.CLOSE);
			if(socket != null)
			{
				if(socket.Connected)
				{
					socket.Shutdown(SocketShutdown.Both);
				}
				socket.Close();
				if(socket.Connected)
				{
					MessageBox.Show("Winsock error: " + Convert.ToString(System.Runtime.InteropServices.Marshal.GetLastWin32Error()) );
				}
			}

			if(fs != null)
			{
				fs.Close();
			}

			timer.Stop();

			this.isClosed = true;
		}

		public string FileTransferId
		{
			get { return string.Copy(_fileTransferId); }
		}
	}

	public class SendFile : DGPFileTransfer
	{
		private long	readBytes = 0;
		private FileInfo fileInfo;
		private BinaryReader binRead;
		private long startSendAt;

		public override void close()
		{
			if(binRead != null)
			{
				binRead.Close();
			}
			base.baseClose();
		}

		public SendFile(Socket socket, string fileName, string fileTransferId, bool viaServer)
		{
			this.socket = socket;
			socket.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(DataRecived), socket);
			if(viaServer != true)
			{
				socket.Send(Encoding.UTF8.GetBytes("SEND_FILE " + fileTransferId + "\n\r"));
			}

			fileInfo = new FileInfo(string.Copy(fileName));

			this._fileTransferId = fileTransferId;
			this.viaServer = viaServer;

			SetSendFileStatus(TransferStatus.CONFIRM_RECIVE);

			timer.Tick += new EventHandler(timer_Tick);
		}

		public override long TransferedBytes
		{
			get { return readBytes; }
		}

		public override string FileName
		{
			get { return fileInfo.Name; }
		}

		public override long FileSize
		{
			get 
			{
				if(fileInfo != null)
				{
					return fileInfo.Length;
				}
				return -1;
			}
		}

		private void DataRecived(IAsyncResult ar)
		{
			try
			{
				int bytesRecived = socket.EndReceive(ar);
				if(bytesRecived == 0)
				{
					SetSendFileStatus(TransferStatus.NULLDATA_ERROR);
					this.close();
				}
			
				if(transferingFile != true)
				{
					messageBuffer	+= Encoding.UTF8.GetString(data, 0, bytesRecived);
					//sendByteBuffer		 = new byte[data.Length];

					int pos;
					string message = "";
					while ((pos = messageBuffer.IndexOf("\n\r")) != -1)
					{
						message = messageBuffer.Substring(0, pos);
						messageBuffer = messageBuffer.Remove(0, pos+2);
					
						//MessageBox.Show("Send: " + message);
				
						if(message.StartsWith("GET_FILE_INFO"))
						{	
							Send("FILE_INFO " + fileInfo.Name + "\r" + fileInfo.Length);
						}
						else if(message.StartsWith("START_SENDING"))
						{
							string startAt = message.Substring("START_SENDING".Length).Trim();
							startSendAt = long.Parse(startAt);
					
							SetSendFileStatus(TransferStatus.START_SEND);
							transferingFile = true;
							//SendTheFile();
	
							readBytes = 0;
							fs = new FileStream(fileInfo.FullName, FileMode.Open);
							binRead = new BinaryReader(fs);			
							binRead.BaseStream.Seek(startSendAt, SeekOrigin.Current);
				
							SetSendFileStatus(TransferStatus.FILE_SENDING);

							timer.Start();

							do
							{
								//SetSendFileStatus(TransferStatus.FILE_SENDING);

								socket.Send(binRead.ReadBytes(1024));
								readBytes = binRead.BaseStream.Position;

							}while(binRead.BaseStream.Position < binRead.BaseStream.Length);

							SetSendFileStatus(TransferStatus.SEND_OK);
							this.close();
						}
						else 
						{
							SetSendFileStatus(TransferStatus.ERROR);
						}
					}
				}

				// Continue to receive data asynchronously
				data = new byte[1024];
				socket.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(DataRecived), socket);
			}
			catch(IOException)
			{
				SetSendFileStatus(TransferStatus.FILE_ERROR);
				this.close();
			}
			catch (ObjectDisposedException)
			{
				SetSendFileStatus(TransferStatus.SOCKET_EXCEPTION);
			}
			catch (SocketException)
			{			
				SetSendFileStatus(TransferStatus.SOCKET_EXCEPTION);	
				this.close();
			}
			catch (Exception)
			{			
				SetSendFileStatus(TransferStatus.CLOSE);
				this.close();
			}
		}

		private void Send(string sendString)
		{
			socket.Send(Encoding.UTF8.GetBytes(sendString + "\n\r"));
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			transferspeed = readBytes - recivedLastTick;
			recivedLastTick = readBytes;
		}
	}

	public class ReciveFile : DGPFileTransfer
	{
		private string	_fileName;
		private long	_fileSize;

		private long recivedBufferSize;
		private BinaryWriter binWrt;

		public ReciveFile(Socket socket, string fileTransferId)
		{
			this.socket = socket;
			this.socket.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(DataRecived), socket);
			this._fileTransferId = fileTransferId;
			timer.Tick += new EventHandler(timer_Tick);

			_fileName = "";
			_fileSize = 0;
		}

		public override void close()
		{
			if(binWrt != null)
			{
				binWrt.Close();
			}
			base.baseClose();
		}

		public override String FileName
		{
			get { return _fileName; }
		}

		public override long TransferedBytes
		{
			get { return recivedBufferSize; }
		}

		public override long FileSize
		{
			get { return _fileSize; }
		}

		private int nulldatarecived = 0;
		private void DataRecived(IAsyncResult ar)
		{

			try
			{
				int bytesRecived = socket.EndReceive(ar);

				if(bytesRecived == 0)
				{
					if(nulldatarecived == 10000)
					{
						SetSendFileStatus(TransferStatus.NULLDATA_ERROR);
						this.close();
					}
					else
					{
						nulldatarecived++;
					}
				}

				//MessageBox.Show("Bytes" + bytesRecieved);
			
				if(transferingFile != true)
				{
					messageBuffer	+= Encoding.UTF8.GetString(data, 0, bytesRecived);
					//DisplayMessage(messageBuffer, Color.SaddleBrown, "", false);
					//byteBuffer		 = new byte[data.Length];

				

					int pos;
					string message = "";
					while ((pos = messageBuffer.IndexOf("\n\r")) != -1)
					{
						message = messageBuffer.Substring(0, pos);
						messageBuffer = messageBuffer.Remove(0, pos+2);
					
						//MessageBox.Show("Size: " + bytesRecived.ToString() + "\nRecive: '" + message + "'");

						if(message.StartsWith("SEND_FILE"))
						{
						
							//SetSendFileStatus(TransferStatus.ERROR);
							//MessageBox.Show(message);
							socket.Send(Encoding.UTF8.GetBytes("GET_FILE_INFO" + "\n\r"));
							//DisplayMessage("SEND GET_FILE_INFO", Color.SaddleBrown, "", false);

						}
						else if(message.StartsWith("FILE_INFO"))
						{
							//MessageBox.Show(message);
							string [] stringBuffer = Encoding.UTF8.GetString(data).Substring("FILE_INFO".Length).Split('\r');
							_fileName = stringBuffer[0];
							_fileSize = int.Parse(stringBuffer[1]);

							SetSendFileStatus(TransferStatus.CONFIRM_RECIVE);			
						}
						else 
						{
							SetSendFileStatus(TransferStatus.ERROR);
						}
					}
				}
				else
				{
					//recivedBufferSize = bytesRecived;
					//SetSendFileStatus(TransferStatus.FILE_RECIVING);
					if(recivedBufferSize < _fileSize)
					{
						binWrt.Write(data, 0, bytesRecived);
						binWrt.Flush();
						recivedBufferSize += bytesRecived;

						//SetSendFileStatus(TransferStatus.FILE_RECIVING);
					}
					if(recivedBufferSize == _fileSize)
					{
						SetSendFileStatus(TransferStatus.RECIVE_OK);
						this.close();
					}
					else if(recivedBufferSize > _fileSize)
					{
						SetSendFileStatus(TransferStatus.ERROR);
						this.close();
					}
				}

				// Continue to receive data asynchronously
				data = new byte[1024];
				socket.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(DataRecived), socket);
			}
			catch (ObjectDisposedException)
			{
				SetSendFileStatus(TransferStatus.SOCKET_EXCEPTION);
			}
			catch (SocketException)
			{			
				this.close();
				SetSendFileStatus(TransferStatus.SOCKET_EXCEPTION);
			}
			catch(Exception)
			{
				this.close();
				SetSendFileStatus(TransferStatus.ERROR);
			}
		}
	
		public void StartTransfer(string fileName)
		{
			ResumeTransfer(fileName, 0);
		}

		public void ResumeTransfer(string fileName, long startAt)
		{
			timer.Start();

			socket.Send(Encoding.UTF8.GetBytes("START_SENDING " + startAt.ToString() + "\n\r"));
			transferingFile = true;

			recivedBufferSize = startAt;
			fs = new FileStream(fileName, FileMode.OpenOrCreate);
			binWrt = new BinaryWriter(fs);
			binWrt.BaseStream.Seek(0, SeekOrigin.End);
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			transferspeed = recivedBufferSize - recivedLastTick;
			recivedLastTick = recivedBufferSize;
		}
	}

	#endregion

	public class SendFileEvent : EventArgs
	{
		public readonly TransferStatus fileSendStatus;
		public SendFileEvent(TransferStatus init)
		{
			fileSendStatus = init;
		}
	}

	public class NewFileTransferEventArgs : EventArgs
	{
		public readonly FileTransfer fileTransfer;
		public NewFileTransferEventArgs(FileTransfer init)
		{
			fileTransfer = init;
		}
	}
}