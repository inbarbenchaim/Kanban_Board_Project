using IntroSE.Backend.Fronted.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using IntroSE.Kanban.Backend.BusinessLayer;
using System.Collections.Generic;
using log4net;
using System.Reflection;
using log4net.Config;
using System.IO;

namespace IntroSE.Backend.Fronted.ServiceLayer
{
    public class TaskService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("TaskService");
        private readonly UserController uc;
        private readonly BoardController bc;
        public const int backlog = 0;
        public const int inProgress = 1;
        public const int done = 2;

        public TaskService(UserController uc, BoardController bc)
        {
            this.uc = uc;
            this.bc = bc;
        }

        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardID">The ID of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        public string AddTask(string email, int boardID, string title, string description, DateTime dueDate) //update function signature
        {
            if (!uc.CheckUser(email))
            {
                log.Warn("The user is not exsist or not logged in");
                throw new Exception("The user is not exsist or not logged in");
            }
            email = email.ToLower();
            if (!bc.CheckBoardUser(email, boardID))
            {
                log.Warn("The user does not have a board with this name");
                throw new Exception("The user is not exsist or not logged in");
            }
            Response res = new Response();
            try
            {
                Board b = bc.GetBoard(boardID);
                b.AddTask(title, description, dueDate);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardID">The ID of the board</param>
        /// <param name="columnOrdinal">The column id</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        public string AdvanceTask(string email, int boardID, int columnOrdinal, int taskId)
        {
            if (!uc.CheckUser(email))
            {
                log.Warn("The user is not exsist or not logged in");
                throw new Exception("The user is not exsist or not logged in");
            }
            if (!bc.CheckBoardUser(email, boardID))
            {
                log.Warn("The user does not have a board with this name");
                throw new Exception("The user does not have a board with this name");
            }
            Response res = new Response();
            try
            {
                Board b = bc.GetBoard(boardID);
                b.AdvanceTask(email, columnOrdinal, taskId);
                log.Debug("task " + taskId + " in column " + columnOrdinal + " advanced to the next column");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        /// <summary>
        /// This method updates the due date of a task.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardID">The ID of the board</param>
        /// <param name="dueDate">The new due date of the task</param>
        /// <param name="columnName">The column name</param>
        /// <param name="taskID">The task to be updated identified task ID</param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        public string UpdateTaskDueDate(string email, int boardID, DateTime dueDate, string columnName, int taskID)
        {
            if (!uc.CheckUser(email))
            {
                log.Warn("The user is not exsist or not logged in");
                throw new Exception("The user is not exsist or not logged in");
            }
            if (!bc.CheckBoardUser(email, boardID))
            {
                log.Warn("The user does not have a board with this name");
                throw new Exception("The user does not have a board with this name");
            }
            Response res = new Response();
            try
            {
                Board b = bc.GetBoard(boardID);
                b.UpdateTaskDueDate(email, dueDate, columnName, taskID);
                log.Debug("The task due date is updated to: " + dueDate.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardID">The ID of the board</param>
        /// <param name="columnName">The column name</param>
        /// <param name="taskID">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        public string UpdateTaskTitle(string email, int boardID, string columnName, int taskID, string title)
        {
            if (!uc.CheckUser(email))
            {
                log.Warn("The user is not exsist or not logged in");
                throw new Exception("The user is not exsist or not logged in");
            }
            if (!bc.CheckBoardUser(email, boardID))
            {
                log.Warn("The user does not have a board with this name");
                throw new Exception("The user does not have a board with this name");
            }
            Response res = new Response();
            try
            {
                Board b = bc.GetBoard(boardID);
                b.UpdateTaskTitle(email, columnName, taskID, title);
                log.Debug("The task title is updated to: " + title);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardID">The ID of the board</param>
        /// <param name="columnName">The column name</param>
        /// <param name="taskID">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        public string UpdateTaskDescription(string email, int boardID, string columnName, int taskID, string description)
        {
            if (!uc.CheckUser(email))
            {
                log.Warn("The user is not exsist or not logged in");
                throw new Exception("The user is not exsist or not logged in");
            }
            if (!bc.CheckBoardUser(email, boardID))
            {
                log.Warn("The user does not have a board with this name");
                throw new Exception("The user does not have a board with this name");
            }
            Response res = new Response();
            try
            {
                Board b = bc.GetBoard(boardID);
                b.UpdateTaskDescription(email, columnName, taskID, description);
                log.Debug("The task description is updated to: " + description);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardID">The ID of the board</param>
        /// <param name="columnName">The column name.</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>        
        public string AssignTask(string email, int boardID, string columnName, int taskID, string emailAssignee)
        {
            Response res = new Response();
            try
            {
                if (!uc.CheckUser(email))
                {
                    log.Warn("The user is not exsist or not logged in");
                    throw new Exception("The user is not exsist or not logged in");
                }
                if (!bc.CheckBoardUser(email, boardID))
                {
                    log.Warn("The user does not have a board with this name");
                    throw new Exception("The user does not have a board with this name");
                }
                bc.GetBoard(boardID).AssignTask(email, columnName, taskID, emailAssignee);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        /// <summary>
        /// This method returns all the In progress tasks of the user.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <returns> The string in json format that represent a Response object with an
        /// error messeage or list of all in progress tasks of the user</returns>*/
        public string InProgressTasks(string email)
        {
            if (!uc.CheckUser(email))
            {
                log.Warn("The user is not exsist or not logged in");
                throw new Exception("The user is not exsist or not logged in");
            }
            Response res = new Response();
            try
            {
                email = email.ToLower();
                List<Task> tasks = new List<Task>();
                tasks = bc.InProgressTasks(email);
                res.ReturnValue = tasks;
                log.Info("return in progress tasks succesfully");
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
    }
}
