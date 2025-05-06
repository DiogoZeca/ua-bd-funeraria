using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
using System.Net;
using System.Xml.Linq;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace funeraria.Entities
{
    public sealed class Database
    {
        private static Database databaseInstance = null;
        private static readonly object lockObject = new object();

        // This is for SQL Server Authentication when using a username and password instead of Windows Authentication
        private static string serverAddress = "tcp:mednat.ieeta.pt\\SQLSERVER,8101";
        private static string databaseName = "p2g7";
        private static string databaseUsername = "p2g7";
        private static string databasePassword = "Zeca_Duarte_2025";
        private static string connectionString = "data source=" + serverAddress + ";initial catalog=" + databaseName + ";uid=" + databaseUsername + ";password=" + databasePassword;

        //private static string serverAddress = "(localdb)\\MSSQLLocalDB";
        //private static string databaseName = "FuneralServiceDB";
        //private static string connectionString = "data source=" + serverAddress + ";initial catalog=" + databaseName + ";Integrated Security=True";


        private Database() { }

        public static Database GetDatabase()
        {
            if (databaseInstance == null)
            {
                lock (lockObject)
                {
                    if (databaseInstance == null)
                    {
                        databaseInstance = new Database();
                    }
                }
            }
            return databaseInstance;
        }

        private static SqlConnection ConnectDB()
        {
            return new SqlConnection(connectionString);
        }

        //
        // USER METHODS
        //
        public bool AuthenticateUser(string username, string password)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {

                    command.CommandText = "AuthenticateUser";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    SqlParameter status = new SqlParameter("@Status", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(status);

                    SqlParameter message = new SqlParameter("@Message", SqlDbType.NVarChar, 255)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(message);

                    SqlParameter userId = new SqlParameter("@UserID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(userId);

                    try
                    {

                        command.ExecuteNonQuery();
                        bool isAuthenticated = (bool)command.Parameters["@Status"].Value;
                        String msgReturn = (String)command.Parameters["@Message"].Value;

                        if (!isAuthenticated)
                        {
                            MessageBox.Show(msgReturn, "Authentication", MessageBoxButtons.OK);
                            return false;
                        }
                        else
                        {
                            LoginForm.userId = (int)command.Parameters["@UserID"].Value;
                            return true;
                        }

                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("SQL Error: " + ex.Message, "Authentication Error", MessageBoxButtons.OK);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Authentication Error", MessageBoxButtons.OK);
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public DataRow GetUserbyId(int id)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE id = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", id);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        connection.Close();
                        return dt.Rows[0];
                    }
                }
            }
        }
        public static void DeleteAccount(int id)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM Users WHERE id = @UserID", connection))
                {
                    command.Parameters.AddWithValue("@UserID", id);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("SQL error \r\n" + ex.Message, "Delete Account Error", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error \r\n" + ex.Message, "Delete Account Error", MessageBoxButtons.OK);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public bool RegisterUser(string username, string email, string password, string name, byte[] img)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "RegisterUser";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@ProfilePicture", img);

                    SqlParameter status = new SqlParameter("@Status", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(status);
                    SqlParameter message = new SqlParameter("@Message", SqlDbType.NVarChar, 255)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(message);

                    try
                    {
                        command.ExecuteNonQuery();
                        bool success = (bool)command.Parameters["@Status"].Value;
                        String msg = (String)command.Parameters["@Message"].Value;
                        MessageBox.Show(msg, "Registration", MessageBoxButtons.OK);
                        return success;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Failed to register user due to the SQL error \r\n" + ex.Message, "Register User Error", MessageBoxButtons.OK);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to register user due to the error \r\n" + ex.Message, "Register User Error", MessageBoxButtons.OK);
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public bool UpdateUser(int? id, string name, string email, string username, string password, byte[] img)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_updateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", id);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@ProfilePicture", img);


                    SqlParameter status = new SqlParameter("@StatusOut", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(status);
                    SqlParameter message = new SqlParameter("@Message", SqlDbType.NVarChar, 255)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(message);

                    try
                    {
                        command.ExecuteNonQuery();
                        bool success = (bool)command.Parameters["@StatusOut"].Value;
                        String msg = (String)command.Parameters["@Message"].Value;
                        MessageBox.Show(msg, "Update User", MessageBoxButtons.OK);
                        return success;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Failed to update user due to the SQL error \r\n" + ex.Message, "Update User Error", MessageBoxButtons.OK);
                        return false;
                    }
                }
            }
        }

        //
        // CREMATORIES METHODS
        //
        public bool AddCrematory(string location, string contact, decimal price)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_addCrematory", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Location", location);
                    command.Parameters.AddWithValue("@Contact", contact);
                    command.Parameters.AddWithValue("@Price", price);

                    SqlParameter status = new SqlParameter("@Status", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(status);

                    SqlParameter message = new SqlParameter("@Message", SqlDbType.VarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(message);

                    try
                    {
                        command.ExecuteNonQuery();
                        bool success = (bool)command.Parameters["@Status"].Value;
                        String msg = (String)command.Parameters["@Message"].Value;
                        MessageBox.Show(msg, "Add Crematory", MessageBoxButtons.OK);
                        connection.Close();
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("SQL error \r\n" + ex.Message, "Add Crematory Error", MessageBoxButtons.OK);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error \r\n" + ex.Message, "Add Crematory Error", MessageBoxButtons.OK);
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public static void DeleteCrematory(int id)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM Crematory WHERE id = @CrematoryID", connection))
                {
                    command.Parameters.AddWithValue("@CrematoryID", id);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("SQL error \r\n" + ex.Message, "Delete Crematory Error", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error \r\n" + ex.Message, "Delete Crematory Error", MessageBoxButtons.OK);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public DataTable GetAllCrematoryList()
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                String command = "SELECT * FROM Crematory";
                using (SqlCommand cmd = new SqlCommand(command, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        connection.Close();
                        return table;
                    }
                }
            }
        }
        public DataTable GetCrematoryById(int id)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                String command = "SELECT * FROM Crematory WHERE id = @crematoryID";
                using (SqlCommand cmd = new SqlCommand(command, connection))
                {
                    cmd.Parameters.AddWithValue("@crematoryID", id);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        connection.Close();
                        return table;
                    }
                }
            }
        }
        public bool UpdateCrematory(int id, string location, string contact, decimal price)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_updateCrematory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CrematoryID", id);
                    command.Parameters.AddWithValue("@Location", location);
                    command.Parameters.AddWithValue("@Contact", contact);
                    command.Parameters.AddWithValue("@Price", price);

                    SqlParameter status = new SqlParameter("@Status", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(status);

                    SqlParameter message = new SqlParameter("@Message", SqlDbType.VarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(message);

                    try
                    {
                        command.ExecuteNonQuery();
                        bool success = (bool)command.Parameters["@Status"].Value;
                        String msg = (String)command.Parameters["@Message"].Value;
                        MessageBox.Show(msg, "Update Crematory", MessageBoxButtons.OK);
                        connection.Close();
                        return success;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Failed to update Crematory due to the error \r\n" + ex.Message, "Update Crematory Error", MessageBoxButtons.OK);
                        connection.Close();
                        return false;
                    }
                }
            }
        }

        //
        // CEMETERY METHODS
        //
        public bool AddCemetery(string location, string contact, decimal price)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_addCemetery", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Location", location);
                    command.Parameters.AddWithValue("@Contact", contact);
                    command.Parameters.AddWithValue("@Price", price);

                    SqlParameter status = new SqlParameter("@Status", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(status);

                    SqlParameter message = new SqlParameter("@Message", SqlDbType.VarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(message);

                    try
                    {
                        command.ExecuteNonQuery();
                        bool success = (bool)command.Parameters["@Status"].Value;
                        String msg = (String)command.Parameters["@Message"].Value;
                        MessageBox.Show(msg, "Add Cemetery", MessageBoxButtons.OK);
                        connection.Close();
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("SQL error \r\n" + ex.Message, "Add Cemetery Error", MessageBoxButtons.OK);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error \r\n" + ex.Message, "Add Cemetery Error", MessageBoxButtons.OK);
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public static void DeleteCemetery(int id)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM Cemetery WHERE id = @CemeteryID", connection))
                {
                    command.Parameters.AddWithValue("@CemeteryID", id);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("SQL error \r\n" + ex.Message, "Delete Cemetery Error", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error \r\n" + ex.Message, "Delete Cemetery Error", MessageBoxButtons.OK);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public DataTable GetAllCemeteryList()
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                String command = "SELECT * FROM Cemetery";
                using (SqlCommand cmd = new SqlCommand(command, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        connection.Close();
                        return table;
                    }
                }
            }
        }
        public DataTable GetCemeteryById(int id)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                String command = "SELECT * FROM Cemetery WHERE id = @cemeteryID";
                using (SqlCommand cmd = new SqlCommand(command, connection))
                {
                    cmd.Parameters.AddWithValue("@cemeteryID", id);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        connection.Close();
                        return table;
                    }
                }
            }
        }
        public bool UpdateCemetery(int id, string location, string contact, decimal price)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_updateCemetery", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CemeteryID", id);
                    command.Parameters.AddWithValue("@Location", location);
                    command.Parameters.AddWithValue("@Contact", contact);
                    command.Parameters.AddWithValue("@Price", price);

                    SqlParameter status = new SqlParameter("@Status", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(status);

                    SqlParameter message = new SqlParameter("@Message", SqlDbType.VarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(message);

                    try
                    {
                        command.ExecuteNonQuery();
                        bool success = (bool)command.Parameters["@Status"].Value;
                        String msg = (String)command.Parameters["@Message"].Value;
                        MessageBox.Show(msg, "Update Cemetery", MessageBoxButtons.OK);
                        connection.Close();
                        return success;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Failed to update Cemetery due to the error \r\n" + ex.Message, "Update Cemetery Error", MessageBoxButtons.OK);
                        connection.Close();
                        return false;
                    }
                }
            }
        }

        //
        // CHURCH METHODS
        //
        public bool AddChurch(string location, string name)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_addChurch", connection))
                {
                    command.Parameters.Add(new SqlParameter("@Location", location));
                    command.Parameters.Add(new SqlParameter("@Name", name));

                    SqlParameter status = new SqlParameter("@Status", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(status);

                    SqlParameter message = new SqlParameter("@Message", SqlDbType.VarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(message);

                    try
                    {
                        command.ExecuteNonQuery();
                        bool success = (bool)command.Parameters["@Status"].Value;
                        String msg = (String)command.Parameters["@Message"].Value;
                        MessageBox.Show(msg, "Add Church", MessageBoxButtons.OK);
                        connection.Close();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error \r\n" + ex.Message, "Add Church Error", MessageBoxButtons.OK);
                        connection.Close();
                        return false;
                    }
                }
            }
        }
        public static void DeleteChurch(int id)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_deleteChurch", connection))
                {
                    command.Parameters.AddWithValue("@ChurchID", id);
                    SqlParameter status = new SqlParameter("@Status", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(status);

                    SqlParameter message = new SqlParameter("@Message", SqlDbType.VarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(message);

                    try
                    {
                        command.ExecuteNonQuery();
                        bool success = (bool)command.Parameters["@Status"].Value;
                        String msg = (String)command.Parameters["@Message"].Value;
                        MessageBox.Show(msg, "Delete Church", MessageBoxButtons.OK);
                        connection.Close();
                    } 
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error \r\n" + ex.Message, "Delete Church Error", MessageBoxButtons.OK);
                        connection.Close();
                    }
                }
            }
        }
        public DataTable GetAllChurcList()
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                String command = "SELECT * FROM Church";
                using (SqlCommand cmd = new SqlCommand(command, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        connection.Close();
                        return table;
                    }
                }
            }
        }
        public DataTable GetChurchById(int id)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                String command = "SELECT * FROM Church WHERE id = @churchID";
                using (SqlCommand cmd = new SqlCommand(command, connection))
                {
                    cmd.Parameters.AddWithValue("@churchID", id);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        connection.Close();
                        return table;
                    }
                }
            }
        }
        public bool UpdateChurch(int id, string location, string name)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("sp_updateChurch", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CemeteryID", id);
                    command.Parameters.AddWithValue("@Location", location);
                    command.Parameters.AddWithValue("@Name", name);

                    SqlParameter status = new SqlParameter("@Status", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(status);

                    SqlParameter message = new SqlParameter("@Message", SqlDbType.VarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(message);

                    try
                    {
                        command.ExecuteNonQuery();
                        bool success = (bool)command.Parameters["@Status"].Value;
                        String msg = (String)command.Parameters["@Message"].Value;
                        MessageBox.Show(msg, "Church Cemetery", MessageBoxButtons.OK);
                        connection.Close();
                        return success;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Failed to update Church due to the error \r\n" + ex.Message, "Update Church Error", MessageBoxButtons.OK);
                        connection.Close();
                        return false;
                    }
                }
            }
        }

        //
        // PRIEST METHODS
        //
        public bool AddPriest(string bi, string name, decimal price, string contact, string title)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    // Insert into Person
                    SqlCommand cmd1 = new SqlCommand("INSERT INTO Person (bi, name) VALUES (@bi, @name)", connection, transaction);
                    cmd1.Parameters.AddWithValue("@bi", bi);
                    cmd1.Parameters.AddWithValue("@name", name);
                    cmd1.ExecuteNonQuery();

                    // Insert into Representative
                    SqlCommand cmd2 = new SqlCommand("INSERT INTO Representative (person_bi, contact) VALUES (@bi, @contact)", connection, transaction);
                    cmd2.Parameters.AddWithValue("@bi", bi);
                    cmd2.Parameters.AddWithValue("@contact", contact);
                    cmd2.ExecuteNonQuery();

                    // Insert into Priest
                    SqlCommand cmd3 = new SqlCommand("INSERT INTO Priest (representative_bi, price, title) VALUES (@bi, @price, @title)", connection, transaction);
                    cmd3.Parameters.AddWithValue("@bi", bi);
                    cmd3.Parameters.AddWithValue("@price", price);
                    cmd3.Parameters.AddWithValue("@title", title);
                    cmd3.ExecuteNonQuery();

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public static void DeletePriest(string bi)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Primeiro, remove da tabela Have
                    using (SqlCommand cmdDeleteHave = new SqlCommand("DELETE FROM Have WHERE priest_bi = @PriestBi", connection, transaction))
                    {
                        cmdDeleteHave.Parameters.AddWithValue("@PriestBi", bi);
                        cmdDeleteHave.ExecuteNonQuery();
                    }

                    // Depois, remove da tabela Priest
                    using (SqlCommand cmdDeletePriest = new SqlCommand("DELETE FROM Priest WHERE representative_bi = @PriestBi", connection, transaction))
                    {
                        cmdDeletePriest.Parameters.AddWithValue("@PriestBi", bi);
                        cmdDeletePriest.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("SQL error \r\n" + ex.Message, "Delete Priest Error", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error \r\n" + ex.Message, "Delete Priest Error", MessageBoxButtons.OK);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public DataTable GetAllPriestList()
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                String command = "SELECT * FROM Priest";
                using (SqlCommand cmd = new SqlCommand(command, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        connection.Close();
                        return table;
                    }
                }
            }
        }
        public DataTable GetPriestById(string bi)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                String command = "SELECT * FROM vw_PriestDetails WHERE BI = @priestID";
                using (SqlCommand cmd = new SqlCommand(command, connection))
                {
                    cmd.Parameters.AddWithValue("@priestID", bi);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        connection.Close();
                        return table;
                    }
                }
            }
        }
        public String GetPriestNameByPriestBi(string bi)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT name FROM vw_PriestWithPerson WHERE representative_bi = @priestBi", connection))
                {
                    cmd.Parameters.AddWithValue("@priestBi", bi);

                    object result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }
        public decimal GetPriestContactByPriestBi(string bi)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT contact FROM vw_PriestWithPerson WHERE representative_bi = @priestBi", connection))
                {
                    cmd.Parameters.AddWithValue("@priestBi", bi);
                    object result = cmd.ExecuteScalar();
                    return decimal.TryParse(result?.ToString(), out var contact) ? contact : 0;

                }
            }
        }
        public bool UpdatePriest(string bi, string name, decimal price, string contact, string title)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();

                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    // Update Person
                    SqlCommand cmd1 = new SqlCommand("UPDATE Person SET name = @name WHERE bi = @bi", connection, transaction);
                    cmd1.Parameters.AddWithValue("@bi", bi);
                    cmd1.Parameters.AddWithValue("@name", name);
                    cmd1.ExecuteNonQuery();

                    // Update Representative
                    SqlCommand cmd2 = new SqlCommand("UPDATE Representative SET contact = @contact WHERE person_bi = @bi", connection, transaction);
                    cmd2.Parameters.AddWithValue("@bi", bi);
                    cmd2.Parameters.AddWithValue("@contact", contact);
                    cmd2.ExecuteNonQuery();

                    // Update Priest
                    SqlCommand cmd3 = new SqlCommand("UPDATE Priest SET price = @price, title = @title WHERE representative_bi = @bi", connection, transaction);
                    cmd3.Parameters.AddWithValue("@bi", bi);
                    cmd3.Parameters.AddWithValue("@price", price);
                    cmd3.Parameters.AddWithValue("@title", title);
                    cmd3.ExecuteNonQuery();

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public bool PriestExists(string bi)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT dbo.findBiExists(@bi)", connection))
                {
                    cmd.Parameters.AddWithValue("@bi", bi);
                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int count))
                    {
                        return count > 0;
                    }
                    return false;
                }
            }
        }

        //
        // FLORIST METHODS
        //
        public bool AddFlorist(int nif, string name, string contact, string address)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO dbo.Florist(nif, name, contact, address) VALUES(@Nif, @Name, @Contact, @Address)", connection))
                {
                    command.Parameters.Add(new SqlParameter("@Nif", SqlDbType.Int) { Value = nif });
                    command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar) { Value = name });
                    command.Parameters.Add(new SqlParameter("@Contact", SqlDbType.NVarChar) { Value = contact });
                    command.Parameters.Add(new SqlParameter("@Address", SqlDbType.NVarChar) { Value = address });
                    try
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("SQL error \r\n" + ex.Message, "Add Florist Error", MessageBoxButtons.OK);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error \r\n" + ex.Message, "Add Florist Error", MessageBoxButtons.OK);
                        return false;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public DataTable GetAllFloristList()
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                String command = "SELECT * FROM Florist";
                using (SqlCommand cmd = new SqlCommand(command, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        connection.Close();
                        return table;
                    }
                }
            }
        }
        public DataTable GetFloristByNif(int nif)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                String command = "SELECT * FROM Florist WHERE nif = @floristNif";
                using (SqlCommand cmd = new SqlCommand(command, connection))
                {
                    cmd.Parameters.AddWithValue("@floristNif", nif);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        connection.Close();
                        return table;
                    }
                }
            }
        }
        public static void DeleteFlorist(int nif)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    using (SqlCommand cmdDeleteFlorist = new SqlCommand("DELETE FROM Florist WHERE nif = @FloristNif", connection, transaction))
                    {
                        cmdDeleteFlorist.Parameters.AddWithValue("@FloristNif", nif);
                        cmdDeleteFlorist.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("SQL error \r\n" + ex.Message, "Delete Florist Error", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error \r\n" + ex.Message, "Delete Florist Error", MessageBoxButtons.OK);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public bool UpdateFlorist(int nif, string name, string contact, string address)
        {
            using (SqlConnection connection = ConnectDB())
            {
                string query = "UPDATE Florist SET name = @Name, contact = @Contact, address = @Address WHERE nif = @Nif";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Contact", contact);
                cmd.Parameters.AddWithValue("@Address", address);
                cmd.Parameters.AddWithValue("@Nif", nif);

                connection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool FloristExists(int nif)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT dbo.findNifExists(@nif)", connection))
                {
                    cmd.Parameters.AddWithValue("@nif", nif);
                    object result = cmd.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int count))
                    {
                        return count > 0;
                    }
                    return false;
                }
            }
        }

        //
        // PROCESS METHODS
        //

        public DataTable GetAllProcessList()
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                String command = "SELECT * FROM Process";
                using (SqlCommand cmd = new SqlCommand(command, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        connection.Close();
                        return table;
                    }
                }
            }
        }

        public static void DeleteProcess(int id)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM Process WHERE num_process = @ProcessID", connection))
                {
                    command.Parameters.AddWithValue("@ProcessID", id);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("SQL error \r\n" + ex.Message, "Delete Process Error", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error \r\n" + ex.Message, "Delete Process Error", MessageBoxButtons.OK);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }


        public DataTable GetAllProductId()
        {
            using (SqlConnection connection = ConnectDB())
            {
                string query = "SELECT * FROM Products";
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        connection.Close();
                        return table;
                    }
                }
            }
        }

        public string GetProductTypeById(decimal id)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                String command = "SELECT Tipo FROM vw_AllProducts WHERE id = @productID";
                using (SqlCommand cmd = new SqlCommand(command, connection))
                {
                    cmd.Parameters.AddWithValue("@productID", id);
                    object result = cmd.ExecuteScalar();
                    MessageBox.Show(result.ToString());
                    return result != null ? result.ToString() : null;
                }
            }
        }

        public String GetDeceasedNameByProcessId(decimal processId)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT name FROM vw_ProcessesWithDeceased WHERE num_process = @processId", connection))
                {
                    cmd.Parameters.AddWithValue("@processId", processId);

                    object result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }

        public DateTime GetFuneralDateByProcessId(decimal processId)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT funeral_date FROM vw_ProcessesWithFuneral WHERE num_process = @processId", connection))
                {
                    cmd.Parameters.AddWithValue("@processId", processId);

                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToDateTime(result) : DateTime.MinValue;
                }
            }
        }

        public String GetLocalDeathByProcessId(decimal processId)
        {
            using (SqlConnection connection = ConnectDB())
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT location FROM vw_ProcessesWithFuneral WHERE num_process = @processId", connection))
                {
                    cmd.Parameters.AddWithValue("@processId", processId);

                    object result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }

    }
}
