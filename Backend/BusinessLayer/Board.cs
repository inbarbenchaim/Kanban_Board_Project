using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using log4net;


namespace IntroSE.Backend.Fronted.BusinessLayer
{
    public class Board
    {
        public string name { get; private set; }
        private static readonly ILog log = LogManager.GetLogger("Board");

        public int boardID { get; private set; }
        private List<User> _users;
        internal List<User> users { get { return _users; } set { _users = value; } }

        public string owner { get; internal set; }

        public int counterIDtask { get; private set; }

        public const int backlog = 0;
        public const int inProgress = 1;
        public const int done = 2;

        internal Dictionary<string, Column> columns { get; set; }
        internal BoardDTO boardDTO { get; private set; }


        public Board(string name, string ownerEmail, int counterIDboards)
        {
            this.name = name;
            columns = new Dictionary<string, Column>();
            columns.Add("backlog", new Column("backlog", counterIDboards));
            columns.Add("in progress", new Column("in progress", counterIDboards));
            columns.Add("done", new Column("done", counterIDboards));
            this.owner = ownerEmail;
            this.boardID = counterIDboards;
            this.counterIDtask = 0;
            this.boardDTO = new BoardDTO(boardID, name, ownerEmail, counterIDtask);
            users = new List<User>();
            boardDTO.Save();
        }

        internal Board(BoardDTO board, List<User> users)
        {
            this.name = board.Name;
            this.owner = board.Owner;
            this.boardID = board.boardID;
            this.counterIDtask = board.counterIDtask;
            this.boardDTO = board;
            List<ColumnDTO> columnDTOs = new ColumnMapper().BoardColumns(this.boardID);
            columns = new Dictionary<string, Column>();
            foreach (ColumnDTO c in columnDTOs)
                columns.Add(c.columnName, new Column(c, this.boardID));
            this.users = users;
        }

        /// <summary>
        /// This method add a user to the board.
        /// </summary>
        /// <param name="user">The user to add to the board</param>
        /// <returns>void</returns>*/
        internal void AddUser(User user)
        {
            if (!users.Contains(user))
                users.Add(user);
        }

        /// <summary>
        /// This method edit the name of the board.
        /// </summary>
        /// <param name="name">.the name of the board</param>
        /// <returns>void</returns>*/
        internal void EditName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    log.Warn("the name of the board that received is invalid");
                    throw new ArgumentException("the name received is invalid");
                }
                this.name = name;
                boardDTO.Name = name;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This method move task from a list to another list.
        /// </summary>
        /// <param name="useremail">The user email address.</param>
        /// <param name="columnOrdinal">The column number.backlog = 0, inprogress = 1, done = 2</param>
        /// <param name="taskId">The id of the task that advanced</param>
        /// <returns>void</returns>*/
        internal void AdvanceTask(string useremail, int columnOrdinal, int taskId)
        {
            try
            {
                if (columnOrdinal == done)
                {
                    log.Warn("the task is in the last column and cannot continue to move forward");
                    throw new Exception("the task is in the last column and cannot continue to move forward");
                }
                if (columnOrdinal > done || columnOrdinal < backlog)
                {
                    log.Warn("there is no column exist with this Column ordinal");
                    throw new IndexOutOfRangeException("there is no column exist with this Column ordinal");
                }
                Column currentColumn = GetColumn(columnOrdinal);
                Column nextColumn = GetColumn(columnOrdinal + 1);
                if (!currentColumn.IsExist(taskId))
                {
                    log.Warn("there is no task exist with this task ID in this column ID");
                    throw new ArgumentException("there is no task exist with this task ID in this column ID");
                }
                if (nextColumn.Tasks.Count == nextColumn.limit)
                {
                    log.Warn("the task can not be advanced due limition on tasks number");
                    throw new ArgumentException("the task can not be advanced due limition on tasks number");
                }
                Task task = currentColumn.GetTask(taskId);
                if (task.EmailAssignee != null && task.EmailAssignee != useremail)
                {
                    log.Warn("this user cannot advance this task");
                    throw new ArgumentException("this user cannot advance this task");
                }
                TaskDTO taskDTO = task.taskDTO;
                taskDTO.Column = nextColumn.columnName;
                nextColumn.AddTask(taskId, task);
                currentColumn.Remove(taskId);
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
        /// <returns>void</returns>*/
        internal void AddTask(string title, string description, DateTime dueDate)
        {
            if (string.IsNullOrWhiteSpace(title) || title.Length > 50)
            {
                log.Warn("the title is invalid");
                throw new ArgumentException("the title is invalid");
            }
            if (description.Length > 300)
            {
                log.Warn("the description is to long");
                throw new ArgumentException("the description is to long");
            }
            if (columns["backlog"].limit == columns["backlog"].Tasks.Count)
            {
                log.Warn("the backLog column is full");
                throw new ArgumentException("the backLog column is full");
            }
            if (dueDate.CompareTo(DateTime.Now) <= 0)
            {
                log.Warn("the due date time have to be in the future");
                throw new ArgumentException("the due date time have to be in the future");
            }
            try
            {
                Column c = GetColumn("backlog");
                c.AddTask(title, description, dueDate, counterIDtask, boardID);
                this.boardDTO.counterIDtask = counterIDtask+1;
                counterIDtask++;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This method returns a requested column.
        /// </summary>
        /// <param name="columnOrdinal">The column ID.backlog = 0, inprogress = 1, done = 2</param>
        /// <returns>column, unless an error occurs </returns>
        internal Column GetColumn(int columnOrdinal)
        {
            if (columnOrdinal < backlog || columnOrdinal > done)
            {
                log.Warn("there is no Column with this Column ID");
                throw new IndexOutOfRangeException("there is no Column with this Column ID");
            }
            if (columnOrdinal == 0)
                return columns["backlog"];
            else if (columnOrdinal == 1)
                return columns["in progress"];
            else
                return columns["done"];
        }

        /// <summary>
        /// This method returns a requested column.
        /// </summary>
        /// <param name="name">The column name.</param>
        /// <returns>column, unless an error occurs </returns>
        internal Column GetColumn(string name)
        {
            if (!columns.ContainsKey(name))
            {
                log.Warn("there is no Column with this Column ID");
                throw new IndexOutOfRangeException("There is no Column with this Column Name");
            }
            return columns[name];

        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>void, unless an error occurs</returns>
        internal void JoinBoard(User u)
        {
            try
            {
                UserBoardDTO user_boardDTO = new UserBoardDTO(u.UserEmail, boardID);
                user_boardDTO.Save();
                u.BoardIDs.Add(boardID);
                users.Add(u);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This method removes a user from the members list of a board.
        /// </summary>
        /// <param name="email">The email of the user. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>void, unless an error occurs</returns>
        internal void LeaveBoard(User u)
        {
            try
            {
                if (this.owner == u.UserEmail)
                {
                    log.Warn("An owner of board can not leave the board");
                    throw new ArgumentException("An owner of board can not leave the board");
                }
                UserBoardDTO user_boardDTO = new UserBoardDTO(u.UserEmail, boardID);
                user_boardDTO.Delete();
                users.Remove(u);
                u.BoardIDs.Remove(boardID);
                foreach(Column c in columns.Values)
                {
                    if (!c.columnName.Equals("done"))
                    {
                        foreach(Task t in c.Tasks.Values)
                        {
                            if (u.UserEmail.Equals(t.EmailAssignee))
                            {
                                t.taskDTO.Assignee = null;
                                t.EmailAssignee = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        ///<summary>
        /// This method receives 2 emails, and changes the owner of this board.
        /// </summary>
        /// <param name="ownerEmail">The user email address that owns now the board.</param>
        /// <param name="newOwnerEmail">The user email address that will owns the board.</param>
        /// <returns>void, unless an error occurs</returns>
        internal void ChangeOwner(User owner, User newOwner)
        {
            try
            {
                if (this.owner != owner.UserEmail)
                {
                    log.Warn(owner.UserEmail + " is not the owner of the board");
                    throw new ArgumentException(owner.UserEmail + " is not the owner of the board");
                }
                if (users.Contains(newOwner))
                {
                    log.Warn(newOwner.UserEmail + " is not connected of the board");
                    throw new ArgumentException(newOwner.UserEmail + " is not connected of the board");
                }
                boardDTO.Owner = newOwner.UserEmail;
                this.owner = newOwner.UserEmail;
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
        /// <param name="columnName">The column name.</param>
        /// <param name="taskID">The task to be updated identified a task ID</param>        
        /// <param name="emailAssignee">Email of the asignee user</param>
        /// <returns>void, unless an error occurs</returns>
        internal void AssignTask(string email, string columnName, int taskID, string emailAssignee)
        {
            if (columnName.Equals("done"))
            {
                log.Warn("There is no option to update task that is in 'done' column");
                throw new Exception("There is no option to update task that is in 'done' column");
            }
            try
            {
                GetColumn(columnName).AssignTask(email, taskID, emailAssignee);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardID">The ID of the board</param>
        /// <param name="columnName">The column name</param>
        /// <param name="taskID">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>void, unless an error occurs</returns>
        internal void UpdateTaskDescription(string email, string columnName, int taskID, string description)
        {
            if (columnName.Equals("done"))
            {
                log.Warn("There is no option to update task that is in 'done' column");
                throw new Exception("There is no option to update task that is in 'done' column");
            }
            try
            {
                GetColumn(columnName).UpdateTaskDescription(email, taskID, description);
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
        /// <param name="columnName">The column name</param>
        /// <param name="taskID">The task to be updated identified task ID</param>
        /// <returns>void, unless an error occurs</returns>
        internal void UpdateTaskDueDate(string email, DateTime dueDate, string columnName, int taskID)
        {
            if (columnName.Equals("done"))
            {
                log.Warn("There is no option to update task that is in 'done' column");
                throw new Exception("There is no option to update task that is in 'done' column");
            }
            try
            {
                GetColumn(columnName).UpdateTaskDueDate(email, dueDate, taskID);
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
        /// <param name="columnName">The column name</param>
        /// <param name="taskID">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>void, unless an error occurs</returns>
        internal void UpdateTaskTitle(string email, string columnName, int taskID, string title)
        {
            if (columnName.Equals("done"))
            {
                log.Warn("There is no option to update task that is in 'done' column");
                throw new Exception("There is no option to update task that is in 'done' column");
            }
            try
            {
                GetColumn(columnName).UpdateTaskTitle(email, taskID, title);
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
        internal List<Task> InProgressTasks(string email)
        {
            try
            {
                Column c = GetColumn("in progress");
                List<Task> tasks = new List<Task>();
                foreach(Task t in c.Tasks.Values)
                {
                    if (t.EmailAssignee != null && t.EmailAssignee.Equals(email))
                        tasks.Add(t);
                }
                return tasks;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

