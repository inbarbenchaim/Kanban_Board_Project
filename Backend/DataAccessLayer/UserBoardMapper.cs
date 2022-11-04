using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserBoardMapper : AbstractMapper
    {
        private const string UserBoardTableName = "UserBoardDTO";
        private static readonly ILog log = LogManager.GetLogger("UserBoardMapper");

        internal UserBoardMapper() : base(UserBoardTableName) { }

        /// <summary>
        /// This method insert the UserBoardDTO to the user-board database table.
        /// </summary>
        /// <param name="userBoard">The object of the new user-board</param>
        /// <returns>A bool which means whether the insert was made successfully or not</returns>
        public bool Insert(UserBoardDTO userBoard)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {UserBoardTableName} ({UserBoardDTO.userColumnName} ,{UserBoardDTO.BoardColumnName}) " +
                        $"VALUES (@userVal,@boardVal);";

                    SQLiteParameter userParam = new SQLiteParameter(@"userVal", userBoard.userEmail);
                    SQLiteParameter BoardParam = new SQLiteParameter(@"boardVal", userBoard.boardID);

                    command.Parameters.Add(userParam);
                    command.Parameters.Add(BoardParam);
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
        /// This method return a UserBoardDTO object that represent the next user-board line in the table.
        /// </summary>
        /// <param name="reader">An SQLiteDataReader object that provides a way of reading from sqlite table</param>
        /// <returns>A UserBoardDTO object that represent the next user-board line in the table</returns>
        protected override UserBoardDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserBoardDTO result = new UserBoardDTO(reader.GetString(0), reader.GetInt32(1));
            return result;
        }

        /// <summary>
        /// This method return a list of a Board ColumnDTOs.
        /// </summary>
        /// <returns>A list of a UserBoardDTO </returns>
        internal List<UserBoardDTO> GetUsers(int BoardID)
        {
            List<UserBoardDTO> results = new List<UserBoardDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {UserBoardTableName} where BoardID = {BoardID}";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }
                }
                catch (Exception ex)
                {
                    log.Debug(ex.Message);
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }
    }
}