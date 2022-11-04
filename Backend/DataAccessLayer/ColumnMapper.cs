using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class ColumnMapper : AbstractMapper
    {
        private const string ColumnTableName = "ColumnDTO";

        private static readonly ILog log = LogManager.GetLogger("ColumnMapper");

        internal ColumnMapper() : base(ColumnTableName) { }

        /// <summary>
        /// This method insert the ColumnDTO to the column database table.
        /// </summary>
        /// <param name="column">The object of the new column</param>
        /// <returns>A bool which means whether the insert was made successfully or not</returns>
        public bool Insert(ColumnDTO column)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnTableName} ({ColumnDTO.ColumnNameColumnName} ,{ColumnDTO.LimitColumnName} ,{ColumnDTO.BoardIDColumnName}) " +
                        $"VALUES (@columnNameVal,@limitVal,@BoardIDVal);";

                    SQLiteParameter columnNameParam = new SQLiteParameter(@"columnNameVal", column.columnName);
                    SQLiteParameter limitParam = new SQLiteParameter(@"limitVal", column.limit);
                    SQLiteParameter boardIDParam = new SQLiteParameter(@"BoardIDVal", column.boardID);

                    command.Parameters.Add(columnNameParam);
                    command.Parameters.Add(limitParam);
                    command.Parameters.Add(boardIDParam);
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
        /// This method return a ColumnDTO object that represent the next column line in the table.
        /// </summary>
        /// <param name="reader">An SQLiteDataReader object that provides a way of reading from sqlite table</param>
        /// <returns>A ColumnDTO object that represent the next column line in the table</returns>
        protected override ColumnDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            ColumnDTO result = new ColumnDTO(reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2));
            return result;
        }

        /// <summary>
        /// This method return a list of a Board ColumnDTOs.
        /// </summary>
        /// <returns>A list of a ColumnDTO </returns>
        internal List<ColumnDTO> BoardColumns(int BoardID)
        {
            List<ColumnDTO> results = new List<ColumnDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {ColumnTableName} where BoardID = {BoardID}";
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