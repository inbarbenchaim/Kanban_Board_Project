using System;
using System.Text.Json.Serialization;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;

namespace IntroSE.Backend.Fronted.BusinessLayer
{
    public class Task
    {
        public int Id { get; private set; }

        private static readonly ILog log = LogManager.GetLogger("Task");

        public DateTime CreationTime { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime DueDate { get; private set; }
        [JsonIgnore]
        public string EmailAssignee { get; set; }
        [JsonIgnore]
        public string Column { get; private set; }
        [JsonIgnore]
        public int BoardID { get; private set; }

        private TaskDTO _taskDTO;
        internal TaskDTO taskDTO { get => _taskDTO; }


        public Task(string title,string description,DateTime dueDate, int taskID, string column, int boardID)
        {
            this.Title = title;     
            this.Description = description;
            this.DueDate = dueDate;
            this.CreationTime = DateTime.Now;
            this.Id = taskID;
            this.EmailAssignee = "";
            this.Column = column;
            this.BoardID = boardID;
            this._taskDTO = new TaskDTO(Id, CreationTime.ToString(), DueDate.ToString(), Title, Description, EmailAssignee, Column , BoardID);
            taskDTO.Save();
        }

        internal Task(TaskDTO task)
        {
            this.Title = task.Title;
            this.Description = task.Description;
            this.DueDate = DateTime.Parse(task.DueDate);
            this.CreationTime = DateTime.Parse(task.CreationDate);
            this.Id = task.TaskID;
            if (task.Assignee.Equals(""))
                this.EmailAssignee = null;
            else
                this.EmailAssignee = task.Assignee;
            this.Column = task.Column;
            this.BoardID = task.BoardID;
            this._taskDTO = task;
        }

        /// <summary>
        /// This method updates the due date of a task.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="dueDate">The new due date of the task</param>>
        /// <returns>void</returns>
        internal void UpdateTaskDueDate(string email, DateTime dueDate)
        {
            if (EmailAssignee == null || !EmailAssignee.Equals(email))
            {
                log.Warn("this user is not the assignee of this task");
                throw new ArgumentException("this user is not the assignee of this task");
            }
            if (dueDate.CompareTo(DateTime.Now) >= 0)
            {
                try
                {
                    taskDTO.DueDate = dueDate.ToString();
                    this.DueDate = dueDate;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
                throw new ArgumentException("this due date is not valid");
        }

        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="title">New title for the task</param>
        /// <returns>void</returns>*/
        internal void UpdateTaskTitle(string email, string title)
        {
            if (EmailAssignee == null || !EmailAssignee.Equals(email))
            {
                log.Warn("this user is not the assignee of this task");
                throw new ArgumentException("this user is not the assignee of this task");
            }
            if (string.IsNullOrWhiteSpace(title))
            {
                log.Warn("the title is invalid");
                throw new ArgumentException("the title is invalid");
            }
            if (title.Length > 50)
            {
                log.Warn("this title length is over 50 chars");
                throw new ArgumentException("this title length is over 50 chars");
            }
            try
            {
                taskDTO.Title = title;
                this.Title = title;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="description">New description for the task</param>
        /// <returns>void</returns>*/
        internal void UpdateTaskDescription(string email, string description)
        {
            if (EmailAssignee == null || !EmailAssignee.Equals(email))
            {
                log.Warn("this user is not the assignee of this task");
                throw new ArgumentException("this user is not the assignee of this task");
            }
            if (description == null)
            {
                log.Warn("this description can not be null");
                throw new ArgumentException("this description can not be null");
            }
            if (description != null && description.Length > 300)
            {
                log.Warn("this description length is over 300 chars");
                throw new ArgumentException("this description length is over 300 chars");
            }
            try
            {
                taskDTO.Description = description;
                this.Description = description;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This method assigns a task to a user
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>void</returns>
        internal void AssignTask(string email, string emailAssignee)
        {
            if (this.EmailAssignee != null && !this.EmailAssignee.Equals(email))
            {
                log.Warn("this user is not the assignee of this task");
                throw new ArgumentException("this user is not the assignee of this task");
            }
            try
            {
                this.taskDTO.Assignee = emailAssignee;
                this.EmailAssignee = emailAssignee;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
