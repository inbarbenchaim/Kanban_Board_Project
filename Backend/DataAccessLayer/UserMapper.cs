using log4net;
using System;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserMapper : AbstractMapper
    {
        private const string UserTableName = "UserDTO";
        private static readonly ILog log = LogManager.GetLogger("UserMapper");

        internal UserMapper() : base(UserTableName) { }

        /// <summary>
        /// This method insert the UserDTO to the user database table.
        /// </summary>
        /// <param name="user">The object of the new user</param>
        /// <returns>A bool which means whether the insert was made successfully or not</returns>
        public bool Insert(UserDTO user)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {UserTableName} ({UserDTO.emailColumnName} ,{UserDTO.passwordColumnName}) " +
                        $"VALUES (@emailVal,@passwordVal)";

                    SQLiteParameter emailParam = new SQLiteParameter("@emailVal", user.Email);
                    SQLiteParameter passwordParam = new SQLiteParameter("@passwordVal", user.Password);

                    command.Parameters.Add(passwordParam);
                    command.Parameters.Add(emailParam);

                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Debug(ex.Message);
                    throw new Exception(ex.Message);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        /// <summary>
        /// This method return a DTO object that represent the next user line in the table.
        /// </summary>
        /// <param name="reader">An SQLiteDataReader object that provides a way of reading from sqlite table</param>
        /// <returns>A DTO object that represent the next user line in the table</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserDTO result = new UserDTO(reader.GetString(0), reader.GetString(1));
            return result;
        }
    }
}