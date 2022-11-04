using IntroSE.Backend.Fronted.BusinessLayer;
using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using System.Linq;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Column
    {
        public int limit { get; private set; }

        private static readonly ILog log = LogManager.GetLogger("Column");

        public string columnName { get; private set; }

        private Dictionary<int, Task> _Tasks;
        internal Dictionary<int, Task> Tasks { get => _Tasks; set { _Tasks = value; } }

        private ColumnDTO _columnDTO;
        internal ColumnDTO columnDTO { get => _columnDTO; set { _columnDTO = value; } }
        public Column(string name, int boardID)
        {
            this.columnName = name;
            limit = -1;
            this._columnDTO = new ColumnDTO(columnName, limit, boardID);
            Tasks = new Dictionary<int, Task>();
            columnDTO.Save();
        }
        public Column(int limit, string name)
        {
            LimitColumn(limit);
            this.columnName = name;
            Tasks = new Dictionary<int, Task>();
        }

        internal Column(ColumnDTO column, int BoardID)
        {
            this.limit = column.limit;
            this.columnName = column.columnName;
            this._columnDTO = column;
            List<TaskDTO> TaskDTOs = new TaskMapper().ColumnTasks(this.columnName, BoardID);
            Tasks = new Dictionary<int, Task>();
            foreach (TaskDTO t in TaskDTOs)
                Tasks.Add(t.TaskID, new Task(t));
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>void</returns>
        internal void LimitColumn(int limit)
        {
            if (limit <= 0)
            {
                log.Warn("Limit have to be a positive number");
                throw new ArgumentException("Limit have to be a positive number");
            }

            if (limit <= this.limit && limit < Tasks.Count)
            {
                log.Warn("the new limit has to be bigger then number of tasks in the column");
                throw new ArgumentException("the new limit has to be bigger then number of tasks in the column");
            }
            this.limit = limit;
            this.columnDTO.limit = limit;
            log.Info("column limit updated successfully");
        }

        /// <summary>
        /// Method for getting a required task from the column  
        /// </summary>
        /// <param name="taskId">The requested taskID</param>
        /// <returns>Task object if task with this ID exist in the column</returns>*/
        internal Task GetTask(int taskId)
        {
            if (!Tasks.ContainsKey(taskId))
            {
                log.Warn("the new limit has to be bigger then number of tasks in the column");
                throw new ArgumentException("There is no task exist with this ID in this column");
            }
            return Tasks[taskId];
        }

        /// <summary>
        /// This method checks if the TaskID exist in this column 
        /// </summary>
        /// <param name="taskID">The taskID for the search</param>
        /// <returns>True if task with this ID exist in the column,False otherwise</returns>*/
        internal bool IsExist(int taskID)
        {
            return Tasks.ContainsKey(taskID);
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>void, unless an error occurs</returns>
        internal void AssignTask(string email, int taskID, string emailAssignee)
        {
            try
            {
                GetTask(taskID).AssignTask(email, emailAssignee);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <param name="counterIDtask">The counter of the new task</param>
        /// <param name="boardID">The ID board</param>
        /// <returns>void</returns>
        internal void AddTask(string title, string description, DateTime dueDate, int counterIDtask, int boardID)
        {
            Task t = new Task(title, description, dueDate, counterIDtask, "backlog", boardID);
            Tasks.Add(t.Id, t);
            log.Info("task added successfully to the column");
        }

        /// <summary>
        /// This method adds a new task to the tasks list.
        /// </summary>
        /// <param name="taskID">ID of the new task</param>
        /// <param name="task">The mew task to add</param>
        /// <returns>void</returns>
        internal void AddTask(int taskID, Task task)
        {
            Tasks.Add(taskID, task);
            log.Info("task added successfully to the column");
        }

        /// <summary>
        /// This method remove a task from the column.
        /// </summary>
        /// <param name="taskID">ID of the task to remove</param>
        /// <returns>void</returns>
        internal void Remove(int taskID)
        {
            Tasks.Remove(taskID);
            log.Info("task removed successfully from the column");
        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="taskID">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>void, unless an error occurs</returns>
        internal void UpdateTaskDescription(string email, int taskID, string description)
        {
            try
            {
                GetTask(taskID).UpdateTaskDescription(email, description);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// This method updates the due date of a task.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="dueDate">The new due date of the task</param>
        /// <param name="taskID">The task to be updated identified task ID</param>
        /// <returns>void, unless an error occurs</returns>
        internal void UpdateTaskDueDate(string email, DateTime dueDate, int taskID)
        {
            try
            {
                GetTask(taskID).UpdateTaskDueDate(email, dueDate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="taskID">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>void, unless an error occurs</returns>
        internal void UpdateTaskTitle(string email, int taskID, string title)
        {
            try
            {
                GetTask(taskID).UpdateTaskTitle(email, title);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This method return a list of the in progress list column in this board.
        /// </summary>
        /// <returns>A list of the in progress tasks, unless an error occurs</returns>
        internal List<Task> InProgressTasks()
        {
            List<Task> ListInProgressTasks = new List<Task>();
            foreach (var t in Tasks)
            {
                ListInProgressTasks.Add(t.Value);
            }
            log.Info("InProgressTasks list created successfully");
            return ListInProgressTasks;
        }

        public List<Task> GetTasks()
        {
            return Tasks.Values.ToList();
        }
    }
}