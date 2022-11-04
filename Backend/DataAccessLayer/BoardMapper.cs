using System;
﻿using log4net;
using System.Data.SQLite;


namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardMapper : AbstractMapper
    {
        private const string BoardTableName = "BoardDTO";

        private static readonly ILog log = LogManager.GetLogger("BoardMapper");


        internal BoardMapper() : base(BoardTableName) { }

        /// <summary>
        /// This method insert the BoardDTO to the board database table.
        /// </summary>
        /// <param name="board">The object of the new board</param>
        /// <returns>A bool which means whether the insert was made successfully or not</returns>
        public bool Insert(BoardDTO board)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardTableName} ({DTO.IDColumnName} ,{BoardDTO.BoardNameColumnName} ,{BoardDTO.BoardOwnerColumnName}, {BoardDTO.counterIDtaskColumnName}) " +
                        $"VALUES (@idVal,@nameVal,@ownerVal,@counterVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", board.boardID);
                    SQLiteParameter titleParam = new SQLiteParameter(@"nameVal", board.Name);
                    SQLiteParameter ownerParam = new SQLiteParameter(@"ownerVal", board.Owner);
                    SQLiteParameter counterParam = new SQLiteParameter(@"counterVal", board.counterIDtask);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(ownerParam);
                    command.Parameters.Add(counterParam);
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
        /// This method return a DTO object that represent the next column line in the table.
        /// </summary>
        /// <param name="reader">An SQLiteDataReader object that provides a way of reading from sqlite table</param>
        /// <returns>A DTO object that represent the next column line in the table</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardDTO result = new BoardDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3));
            return result;
        }
    }
}