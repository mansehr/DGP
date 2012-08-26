using System;
using System.Data.Odbc;
using System.Collections;


namespace DGP_Messenger_Sever
{
	/// <summary>
	/// Summary description for Database.
	/// </summary>
	public class Database
	{
		private OdbcConnection	itsConnection;
		private OdbcCommand		itsCommand;
		private OdbcDataReader	itsDataReader;

		public Database(string server, string database, string uid, string password)
		{
			try
			{
				//Connection string for Connector/ODBC 3.51
				string ConString = "DRIVER={MySQL ODBC 3.51 Driver};" + 
					"SERVER=" + server + ";" +
					"DATABASE=" + database + ";" +
					"UID=" + uid + ";" +
					"PASSWORD=" + password + ";" +
					"OPTION=3";

				//Connect to MySQL using Connector/ODBC
				itsConnection = new OdbcConnection(ConString);
				itsConnection.Open();

				//Create a sample table
				//itsCommand = new OdbcCommand("",itsConnection);
			}
			catch (OdbcException MyOdbcException)//Catch any ODBC exception ..
			{
				this.handleException(MyOdbcException);
			}
		}

		public string read( string sqlCommand)
		{
			try
			{
				string returnString = "";
				itsCommand = new OdbcCommand("", itsConnection);
				itsCommand.CommandText = sqlCommand;
				itsDataReader = itsCommand.ExecuteReader();
				while(itsDataReader.Read())
				{
					if(returnString != "")
					{
						returnString += "\n";
					}
					returnString += itsDataReader.GetString(0);
				}
				itsCommand.Dispose();
				itsDataReader.Close();
				return string.Copy(returnString);
			}
			catch (OdbcException MyOdbcException)//Catch any ODBC exception ..
			{
				this.handleException(MyOdbcException);
			}
			return "";
		}

		public int readInt( string sqlCommand)
		{
			try
			{
				int returnInt = 0;
				itsCommand = new OdbcCommand("", itsConnection);
				itsCommand.CommandText = sqlCommand;
				itsDataReader = itsCommand.ExecuteReader();
				while(itsDataReader.Read())
				{
					returnInt = itsDataReader.GetInt32(0);
				}
				itsCommand.Dispose();
				itsDataReader.Close();
				return returnInt;
			}
			catch (OdbcException MyOdbcException)//Catch any ODBC exception ..
			{
				this.handleException(MyOdbcException);
			}
			return 0;
		}

		public ArrayList getDGPContacts(string userId)
		{
			//Console.WriteLine("UserID: " + userId);
			try
			{
				ArrayList friendList = new ArrayList();
				itsCommand = new OdbcCommand("SELECT user FROM friends WHERE friend=\'" + userId + "\'",itsConnection);
				itsDataReader = itsCommand.ExecuteReader();
				for(int i = 0; itsDataReader.Read(); i++)
				{
			//		Console.WriteLine("Add friend: " + itsDataReader.GetString(0));
					friendList.Add(itsDataReader.GetString(0));
				}
				itsCommand.Dispose();
				itsDataReader.Close();
				return friendList;
			}
			

			catch(OdbcException MyOdbcException)  //Catch any ODBC exception ..
			{
				this.handleException(MyOdbcException);
			}
			return null;
		}

		/*public DGPContact getDGPContactStats(string friendId)
		{
			string name = read("SELECT displayName FROM users WHERE id="+friendId);
			string ipNumber = read("SELECT ipNumber FROM users WHERE id="+friendId);
			int id = readInt("SELECT id FROM users WHERE id="+friendId);
			
			return new DGPContact(id, name, ipNumber);
		}*/

		public string getDisplayName(string id)
		{
			return read("SELECT displayName FROM users WHERE userID=\'" + id + "\'");
		}

		public bool testNameAndPassword(string name, string password, Client client)
		{
			try
			{
				int itsInloggId;
				itsCommand = new OdbcCommand("",itsConnection);
				itsCommand.CommandText = "SELECT userID FROM users WHERE email=\'"+ name + "\'";
				itsDataReader =  itsCommand.ExecuteReader();
				if(itsDataReader.Read())
				{
					itsInloggId = itsDataReader.GetInt32(0);
					itsDataReader.Close();
				}
				else
				{
					itsCommand.Dispose();
					itsDataReader.Close();
					return false;
				}


				itsCommand.CommandText = "SELECT password FROM users WHERE userID=\'"+ itsInloggId.ToString() + "\'";
				itsDataReader =  itsCommand.ExecuteReader();
				itsDataReader.Read();
				string databasePassword = itsDataReader.GetString(0);
				if(string.Compare(databasePassword, password, true) == 0)
				{
					itsDataReader.Close();

					itsCommand.CommandText = "SELECT * FROM users WHERE userID=\'"+ itsInloggId.ToString() + "\'";
					itsDataReader =  itsCommand.ExecuteReader();
					itsDataReader.Read();

					client.Id = itsDataReader.GetString(0);
					client.Email = name;
					//client.IpNumber = itsDataReader.GetString(3);
					client.DisplayName = itsDataReader.GetString(4);

					itsCommand.Dispose();
					itsDataReader.Close();
					return true;
				}
				else
				{
					itsCommand.Dispose();
					itsDataReader.Close();
					return false;
				}
			}
			catch (OdbcException MyOdbcException)//Catch any ODBC exception ..
			{
				this.handleException(MyOdbcException);
			}
			return false;
		}

		public void setUserOnlineStatus(char onlineStatus, string itsId)
		{
			try
			{
				itsCommand = new OdbcCommand("",itsConnection);
				if(itsCommand.Connection.State != 0)
				{
					itsCommand.CommandText = "UPDATE users SET status=\'" + onlineStatus + "\' WHERE userID=\'" + itsId + "\'";
					itsCommand.ExecuteNonQuery();
					itsCommand.Dispose();
				}
			}
			catch(OdbcException MyOdbcException)  //Catch any ODBC exception ..
			{
				this.handleException(MyOdbcException);
			}
		}

		public void setUserDisplayName(string id, string displayName)
		{
			try
			{
				itsCommand = new OdbcCommand("",itsConnection);
				if(itsCommand.Connection.State != 0)
				{
					itsCommand.CommandText = "UPDATE users SET displayName=\'" + displayName + "\' WHERE userID=\'" + id + "\'";
					itsCommand.ExecuteNonQuery();
					itsCommand.Dispose();
				}
			}
			catch(OdbcException MyOdbcException)  //Catch any ODBC exception ..
			{
				this.handleException(MyOdbcException);
			}
		}

		public bool getUserLogginStatus(int itsId)
		{
			try
			{
				char itsChar;
				itsCommand = new OdbcCommand("",itsConnection);
				itsCommand.CommandText = "SELECT status FROM users WHERE userID=\'" + itsId + "\'";
				itsDataReader =  itsCommand.ExecuteReader();
				if(itsDataReader.Read())
				{
					itsChar = itsDataReader.GetChar(0);
					if(itsChar == 'Y')
					{
						itsCommand.Dispose();
						itsDataReader.Close();
						return true;
					}
					else
					{
						itsCommand.Dispose();
						itsDataReader.Close();
						return false;
					}
				}
				else
				{
					itsDataReader.Close();
					itsCommand.Dispose();
					return false;
				}
			}
			catch(OdbcException MyOdbcException)  //Catch any ODBC exception ..
			{
				this.handleException(MyOdbcException);
			}
			return false;
		}

		public char getUserOnlineStatus(string id)
		{
			try
			{
				char itsChar;
				itsCommand = new OdbcCommand("",itsConnection);
				itsCommand.CommandText = "SELECT status FROM users WHERE userID=\'" + id + "\'";;
				itsDataReader =  itsCommand.ExecuteReader();
				if(itsDataReader.Read())
				{
					itsChar = itsDataReader.GetChar(0);
					itsDataReader.Close();
					itsCommand.Dispose();
					return itsChar;
				}
				else
				{
					itsDataReader.Close();
					itsCommand.Dispose();
					return 'Z';
				}
			}
			catch(OdbcException MyOdbcException)  //Catch any ODBC exception ..
			{
				this.handleException(MyOdbcException);
			}
			return 'Z';
		}

		public void close()
		{
			try
			{
				//Close all resources
				if(itsDataReader != null)
					itsDataReader.Close();
				if(itsConnection != null)
					itsConnection.Close();
			}
			catch(OdbcException MyOdbcException)  //Catch any ODBC exception ..
			{
				this.handleException(MyOdbcException);
			}
		}

		public void cleanOnlineStatus()
		{
			try
			{
				itsCommand = new OdbcCommand("",itsConnection);
				if(itsCommand.Connection.State != 0)
				{
					itsCommand.CommandText = "UPDATE users SET status=\'Z\'";
					itsCommand.ExecuteNonQuery();
					itsCommand.Dispose();
				}
			}
			catch(OdbcException MyOdbcException)  //Catch any ODBC exception ..
			{
				this.handleException(MyOdbcException);
			}
		}

		private void handleException(OdbcException MyOdbcException)
		{
			for (int i=0; i < MyOdbcException.Errors.Count; i++)
			{
				Console.Write("SQL ERROR #" + i + "\n" +
					"Message: " + MyOdbcException.Errors[i].Message + "\n" +
					"Native: " + MyOdbcException.Errors[i].NativeError.ToString() + "\n" +
					"Source: " + MyOdbcException.Errors[i].Source + "\n" +
					"SQL: " + MyOdbcException.Errors[i].SQLState + "\n");
			}
		}
	}
}
