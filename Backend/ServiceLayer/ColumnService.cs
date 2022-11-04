using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Text.Json;
using log4net;
using IntroSE.Backend.Fronted.BusinessLayer;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using log4net.Config;
using System.IO;
using System.Reflection;
using Backend.Service;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ColumnService
    {
        private static readonly ILog log = LogManager.GetLogger("ColumnService");
        private readonly UserController uc;
        private readonly BoardController bc;


        public ColumnService(UserController uc, BoardController bc)
        {
            this.uc = uc;
            this.bc = bc;
        }

        /*
        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardID">The ID of the board</param>
        /// <param name="columnName">The column name</param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or Column object</returns>
        public string GetColumn(string email, int boardID, string columnName)
        {
            if (!uc.CheckUser(email))
            {
                log.Warn("The user is not exsist or not logged in");
                throw new Exception("The user is not exsist or not logged in");
            }
            Response res = new Response();
            try
            {
                Board b = bc.GetBoard(boardID);
                res.ReturnValue = b.GetColumn(columnName).Tasks.Values.ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }
        */
        public string GetColumn(string email, int boardID, string columnName)
        {
            Column c;
            if (!uc.CheckUser(email))
            {
                log.Warn("The user is not exsist or not logged in");
                throw new Exception("The user is not exsist or not logged in");
            }
            ResponseT<List<string>> res = new ResponseT<List<string>>();
            try
            {
                List<string> TasksNames = new List<string>();
                Board b = bc.GetBoard(boardID);
                c = b.GetColumn(columnName);
                foreach (Task t in c.Tasks.Values)
                {
                    TasksNames.Add(t.Title);
                }
                res.ReturnValue = TasksNames;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            try
            {
                var options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                return JsonSerializer.Serialize(res, res.GetType(), options);
            }
            catch
            {
                log.Error("Failed to serialize Response object");
                return "Error : Failed to serialize Response object";
            }
        }

        /// <summary>
        /// This method limit the the number of tasks in the column.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardID">The ID of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The limit number of the tasks in the column c</param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>*/
        public string LimitColumn(string email, int boardID, int columnOrdinal, int limit)
        {
            Response res = new Response();
            try
            {
                if (!uc.CheckUser(email))
                {
                    log.Warn("The user is not exsist or not logged in");
                    throw new Exception("The user is not exsist or not logged in");
                }
                else
                {
                    Column c = bc.GetBoard(boardID).GetColumn(columnOrdinal);
                    c.LimitColumn(limit);
                    log.Debug("column limit updated to " + limit);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardID">The ID of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or column name</returns>
        public string GetColumnName(string email, int boardID, int columnOrdinal)
        {
            if (!uc.CheckUser(email))
            {
                log.Warn("The user is not exsist or not logged in");
                throw new Exception("The user is not exsist or not logged in");
            }
            Response res = new Response();
            try
            {
                Column c = bc.GetBoard(boardID).GetColumn(columnOrdinal);
                res.ReturnValue = c.columnName;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardID">The ID of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or column name</returns>
        public string GetColumnNameStr(string email, int boardID, int columnOrdinal)
        {
            if (!uc.CheckUser(email))
            {
                log.Warn("The user is not exsist or not logged in");
                throw new Exception("The user is not exsist or not logged in");
            }
            try
            {
                Column c = bc.GetBoard(boardID).GetColumn(columnOrdinal);
                return c.columnName;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }



        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardID">The ID of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or column limit</returns>
        public string GetColumnLimit(string email, int boardID, int columnOrdinal)
        {
            Response res = new Response();
            try
            {
                if (!uc.CheckUser(email))/*the user has to be logged in?*/
                {
                    log.Warn("The user is not exsist or not logged in");
                    throw new Exception("The user is not exsist or not logged in");
                }
                else
                {
                    Column c = bc.GetBoard(boardID).GetColumn(columnOrdinal);

                    res.ReturnValue = c.limit;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        ///<summary>
        /// This method receives a response, and return error if the response failed.
        /// </summary>
        /// <param name="res">The response.</param>
        ///<returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        /// 
        internal string ReturnJson(Response res)
        {
            try
            {
                var options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                return JsonSerializer.Serialize(res, res.GetType(), options);
            }
            catch
            {
                log.Error("Failed to serialize Response object");
                return "Error : Failed to serialize Response object";
            }
        }

        public string GetTasksIDs(string email, int boardID, string columnName)
        {
            Column c;
            if (!uc.CheckUser(email))
            {
                log.Warn("The user is not exsist or not logged in");
                throw new Exception("The user is not exsist or not logged in");
            }
            ResponseT<List<int>> res = new ResponseT<List<int>>();
            try
            {
                List<int> TaskIDs = new List<int>();
                Board b = bc.GetBoard(boardID);
                c = b.GetColumn(columnName);
                foreach (Task t in c.Tasks.Values)
                {
                    TaskIDs.Add(t.Id);
                }
                res.ReturnValue = TaskIDs;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            try
            {
                var options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                return JsonSerializer.Serialize(res, res.GetType(), options);
            }
            catch
            {
                log.Error("Failed to serialize Response object");
                return "Error : Failed to serialize Response object";
            }
        }

        public string GetTasksDescription(string email, int boardID, string columnName)
        {
            Column c;
            if (!uc.CheckUser(email))
            {
                log.Warn("The user is not exsist or not logged in");
                throw new Exception("The user is not exsist or not logged in");
            }
            ResponseT<List<string>> res = new ResponseT<List<string>>();
            try
            {
                List<string> TaskIDs = new List<string>();
                Board b = bc.GetBoard(boardID);
                c = b.GetColumn(columnName);
                foreach (Task t in c.Tasks.Values)
                {
                    TaskIDs.Add(t.Description);
                }
                res.ReturnValue = TaskIDs;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            try
            {
                var options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                return JsonSerializer.Serialize(res, res.GetType(), options);
            }
            catch
            {
                log.Error("Failed to serialize Response object");
                return "Error : Failed to serialize Response object";
            }
        }
    }
}