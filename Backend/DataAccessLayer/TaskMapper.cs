using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class TaskMapper : AbstractMapper
    {
        private const string TaskTableName = "TaskDTO";
        private static readonly ILog log = LogManager.GetLogger("TaskMapper");

        internal TaskMapper() : base(TaskTableName) { }

        /// <summary>
        /// This method insert the TaskDTO to the task database table.
        /// </summary>
        /// <param name="task">The object of the new task</param>
        /// <returns>A bool which means whether the insert was made successfully or not</returns>
        public bool Insert(TaskDTO task)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskTableName} ({DTO.IDColumnName} ,{TaskDTO.assigneeColumnName} , {TaskDTO.titleColumnName} , " +
                        $"{TaskDTO.descriptionColumnName} ,{TaskDTO.creationDateColumnName} ,{TaskDTO.dueDateColumnName} ,{TaskDTO.ColumnColumnName} ,{TaskDTO.boardIDColumnName}) " +
                        $"VALUES (@idVal,@assigneeVal,@titleVal,@desriptionVal,@creationVal,@dueVal,@columnVal,@boardIDVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", task.TaskID);
                    SQLiteParameter assigneeParam = new SQLiteParameter(@"assigneeVal", task.Assignee);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", task.Title);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"desriptionVal", task.Description);
                    SQLiteParameter creationParam = new SQLiteParameter(@"creationVal", task.CreationDate);
                    SQLiteParameter dueParam = new SQLiteParameter(@"dueVal", task.DueDate);
                    SQLiteParameter columnParam = new SQLiteParameter(@"columnVal", task.Column);
                    SQLiteParameter boardIDParam = new SQLiteParameter(@"boardIDVal", task.BoardID);


                    command.Parameters.Add(idParam);
                    command.Parameters.Add(assigneeParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(creationParam);
                    command.Parameters.Add(dueParam);
                    command.Parameters.Add(columnParam);
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
        /// This method return a TaskDTO object that represent the next task line in the table.
        /// </summary>
        /// <param name="reader">An SQLiteDataReader object that provides a way of reading from sqlite table</param>
        /// <returns>A TaskDTO object that represent the next task line in the table</returns>
        protected override TaskDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            TaskDTO result = new TaskDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetInt32(7));
            return result;

        }

        /// <summary>
        /// This method return a list of a Board ColumnDTOs.
        /// </summary>
        /// <returns>A list of a ColumnDTO </returns>
        internal List<TaskDTO> ColumnTasks(string columnName,int BoardID)
        {
            List<TaskDTO> results = new List<TaskDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TaskTableName} where {TaskDTO.ColumnColumnName} = \"{columnName}\" and {TaskDTO.boardIDColumnName} = {BoardID}";
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