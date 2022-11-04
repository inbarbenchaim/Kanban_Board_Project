using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal abstract class AbstractMapper
    {
        protected readonly string _connectionString;
        private readonly string _tableName;
        private static readonly ILog log = LogManager.GetLogger("AbstractMapper");

        public AbstractMapper(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = tableName;
        }
        /// <summary>
        /// This method update the table according to the values obtained.
        /// </summary>
        /// <param name="filterName">The filter name, according to its value, the values in the table are classified</param>
        /// <param name="filter">The value by which the values in the table are classified</param>
        /// <param name="attributeName">The name of the attribute that we want to update</param>
        /// <param name="attributeValue">The value of the attribute that we want to update</param>
        /// <returns>A bool which means whether the insert was made successfully or not</returns>
        public bool Update(string filterName, string filter, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command;
                if (int.TryParse(filter, out _))
                {
                    command = new SQLiteCommand
                    {
                        Connection = connection,
                        CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where {filterName}={Int32.Parse(filter)}"
                    };
                }
                else
                {
                    command = new SQLiteCommand
                    {
                        Connection = connection,
                        CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where {filterName}={filter}"
                    };
                }
                try
                {
                    if (int.TryParse(attributeValue, out _))
                        command.Parameters.Add(new SQLiteParameter(attributeName, Int32.Parse(attributeValue)));
                    else
                        command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
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

            }
            return res > 0;
        }

        /// <summary>
        /// This method update the table according to the values obtained - receives two filters.
        /// </summary>
        /// <param name="filterName">The filter name, according to its value, the values in the table are classified</param>
        /// <param name="filter">The value by which the values in the table are classified</param>
        /// <param name="attributeName">The name of the attribute that we want to update</param>
        /// <param name="attributeValue">The value of the attribute that we want to update</param>
        /// <param name="id">The ID of the object that we want to update</param>
        /// <returns>A bool which means whether the insert was made successfully or not</returns>
        public bool Update(int id, string attributeName, string attributeValue, string filterName, int filter)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where id={id} and {filterName}={filter}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    command.ExecuteNonQuery();
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

            }
            return res > 0;
        }

        /// <summary>
        /// This method update the table according to the values obtained - receives two filters.
        /// </summary>
        /// <param name="filterName">The filter name, according to its value, the values in the table are classified</param>
        /// <param name="filter">The value by which the values in the table are classified</param>
        /// <param name="attributeName">The name of the attribute that we want to update</param>
        /// <param name="attributeValue">The value of the attribute that we want to update</param>
        /// <param name="id">The ID of the object that we want to update</param>
        /// <returns>A bool which means whether the insert was made successfully or not</returns>
        public bool Update(int id, string attributeName, int attributeValue, string filterName, string filter)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where BoardID={id} and {filterName}=\"{filter}\""
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    command.ExecuteNonQuery();
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

            }
            return res > 0;
        }

        /// <summary>
        /// This method return a list of a specific DTO.
        /// </summary>
        /// <returns>A list of a DTO </returns>
        internal List<DTO> Select()
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName};";
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

        protected abstract DTO ConvertReaderToObject(SQLiteDataReader reader);

        /// <summary>
        /// This method delete specific data by the filter and filtername arguments.
        /// </summary>
        /// <param name="filterName">The filter name, according to its value, the values in the table are classified</param>
        /// <param name="filter">The value by which the values in the table are classified</param>
        /// <returns>A bool which means whether the deletion was made successfully or not</returns>
        public bool Delete(string filterName, string filter)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command;
                if (int.TryParse(filter, out _))
                {
                    command = new SQLiteCommand
                    {
                        Connection = connection,
                        CommandText = $"delete from {_tableName} where {filterName}={Int32.Parse(filter)}"
                    };
                }
                else
                {
                    command = new SQLiteCommand
                    {
                        Connection = connection,
                        CommandText = $"delete from {_tableName} where {filterName}={filter}"
                    };
                }
                try
                {
                    connection.Open();
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

            }
            return res > 0;
        }

        /// <summary>
        /// This method delete specific data.
        /// </summary>
        /// <param name="filterName1">A filter name, according to its value, the values in the table are classified</param>
        /// <param name="filterName2">A filter name, according to its value, the values in the table are classified</param>
        /// <param name="filter1">A value by which the values in the table are classified</param>
        /// <param name="filter2">A value by which the values in the table are classified</param>

        /// <returns>A bool which means whether the deletion was made successfully or not</returns>
        public bool Delete(string filterName1, string filterName2, string filter1, int filter2)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command;
                if (int.TryParse(filter1, out _))
                {
                    command = new SQLiteCommand
                    {
                        Connection = connection,
                        CommandText = $"delete from {_tableName} where {filterName1}={Int32.Parse(filter1)} and {filterName2}={filter2}"
                    };
                }
                else
                {
                    command = new SQLiteCommand
                    {
                        Connection = connection,
                        CommandText = $"delete from {_tableName} where {filterName1}=\"{filter1}\" and {filterName2}={filter2}"
                    };
                }
                try
                {
                    connection.Open();
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

            }
            return res > 0;
        }
        /// <summary>
        /// This method delete all the data.
        /// </summary>
        /// <returns>A bool which means whether the deletion was made successfully or not</returns>
        public bool DeleteAll()
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"DELETE FROM {_tableName}";
                try
                {
                    connection.Open();
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
            }
            return res > 0;
        }
    }
}