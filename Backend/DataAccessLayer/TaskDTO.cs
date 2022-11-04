using log4net;
using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class TaskDTO : DTO
    {
        public const string titleColumnName = "Title";
        public const string descriptionColumnName = "Description";
        public const string assigneeColumnName = "Assignee";
        public const string creationDateColumnName = "CreationDate";
        public const string dueDateColumnName = "DueDate";
        public const string ColumnColumnName = "Column";
        public const string boardIDColumnName = "BoardID";
        private static readonly ILog log = LogManager.GetLogger("TaskDTO");



        private readonly int _taskID;
        public int TaskID { get => _taskID; }
        private string _creationDate;
        public string CreationDate { get => _creationDate; }
        private string _dueDate;
        public string DueDate { get => _dueDate; set { _dueDate = value; mapper.Update(TaskID, dueDateColumnName, value , boardIDColumnName, BoardID); } }
        private string _title;
        public string Title { get => _title; set { _title = value; mapper.Update(TaskID, titleColumnName, value, boardIDColumnName, BoardID); } }
        private string _description;
        public string Description { get => _description; set { _description = value; mapper.Update(TaskID, descriptionColumnName, value, boardIDColumnName, BoardID); } }
        private string _assignee;
        public string Assignee { get => _assignee; set { _assignee = value; mapper.Update(TaskID, assigneeColumnName, value, boardIDColumnName, BoardID); } }
        private string _column;
        public string Column { get => _column; set { _column = value; mapper.Update(TaskID, ColumnColumnName, value, boardIDColumnName, BoardID); } }
        private int _boardID;
        public int BoardID { get => _boardID; }

        internal TaskDTO(int taskID, string assignee, string title, string description, string creationDate, string dueDate
            , string column, int BoardID) : base(new TaskMapper())
        {
            this._taskID = taskID;
            this._creationDate = creationDate;
            this._dueDate = dueDate;
            this._title = title;
            this._description = description;
            this._assignee = assignee;
            this._column = column;
            this._boardID = BoardID;
        }
        
        /// <summary>
        /// This method save the new task in the database.
        /// </summary>
        /// <returns> void </returns>
        public void Save()
        {
            TaskMapper map = new TaskMapper();
            if (!map.Insert(this))
                throw new Exception("the creation of the new Task in the DB failed");
        }

        /// <summary>
        /// This method delete this task from the database.
        /// </summary>
        /// <returns> void </returns>
        public void Delete()
        {
            if (!mapper.Delete(IDColumnName, boardIDColumnName, this.TaskID.ToString(), this.BoardID))
                throw new Exception("the deletion of the Task in the DB failed");
        }
    }
}