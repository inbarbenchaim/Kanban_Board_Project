using System;
using System.Collections.Generic;
using log4net;
using IntroSE.Backend.Fronted.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class BoardController
    {
        private Dictionary<int, Board> _boards; //Contains all boards by IDboard key
        public Dictionary<int, Board> boards { get => _boards; private set { _boards = value;} }
        private readonly UserController uc;
        private static readonly ILog log = LogManager.GetLogger("BoardController");
        private static int counterIDboards = 0;

        public BoardController(UserController uc)
        {
            this.uc = uc;
            this._boards = new Dictionary<int, Board>();
        }

        /// <summary>
        /// This method add a board to the user's boards.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="nameBoard">The name of the board that we want to add.</param>
        /// <returns>void, unless an error occurs</returns>
        internal void AddBoard(User user, string nameBoard)
        {
            try
            {                           
                if (string.IsNullOrWhiteSpace(nameBoard))
                {
                    log.Warn("board with this empty name cannot be created");
                    throw new ArgumentException("board with this empty name cannot be created");
                }
                if (IsBoardNameExists(user.UserEmail, nameBoard))
                {
                    log.Warn("The name is already in the system");
                    throw new ArgumentException("board with this name is already exist");
                }
                Board newBoard = new Board(nameBoard, user.UserEmail, counterIDboards);
                UserBoardDTO user_boardDTO = new UserBoardDTO(user.UserEmail, counterIDboards);
                user_boardDTO.Save();
                boards.Add(newBoard.boardID, newBoard);
                newBoard.AddUser(user);
                user.AddBoard(counterIDboards);
                counterIDboards++;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        /// <returns>The board's name, unless an error occurs</returns>
        internal string GetBoardName(int boardId)
        {
            if (!boards.ContainsKey(boardId))
            {
                log.Warn("there is no Board with this ID");
                throw new ArgumentException("there is no Board with this ID");
            }
            return boards[boardId].name;
        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        /// <returns>void, unless an error occurs</returns>
        internal void JoinBoard(string email, int boardID)
        {
            try
            {
                foreach (int boardsID in uc.GetUser(email).BoardIDs)
                {
                    if (GetBoard(boardsID).name == GetBoard(boardID).name)
                    {
                        log.Warn("This user already has board with the same board name");
                        throw new Exception("This user already has board with the same board name");
                    }
                }
                boards[boardID].JoinBoard(uc.GetUser(email));
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
        internal void LeaveBoard(string email, int boardID)
        {
            if(!CheckBoardUser(email, boardID))
            {
                log.Warn("This user does not connected to the board with this ID");
                throw new Exception ("This user does not connected to the board with this ID");
            }
            try
            {
                boards[boardID].LeaveBoard(uc.GetUser(email));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This method remove the board from the user's boards .
        /// </summary>
        /// <param name="user">The email of the user that joins the board. Must be logged in</param>
        /// <param name="Boardname">The board name</param>
        /// <returns>void, unless an error occurs</returns>
        internal void RemoveBoard(User user, string Boardname)
        {
            try
            {
                Board b = GetBoard(user.UserEmail, Boardname);
                if (b.owner != user.UserEmail)
                {
                    log.Warn(user.UserEmail + " is not the owner of the board");
                    throw new ArgumentException(user.UserEmail + " is not the owner of the board");
                }
                foreach (User u in uc.users.Values)
                {
                    if (u.BoardIDs.Contains(b.boardID))
                    {
                        UserBoardDTO user_boardDTO = new UserBoardDTO(u.UserEmail, b.boardID);
                        user_boardDTO.Delete();
                        u.BoardIDs.Remove(b.boardID);
                    }
                }
                foreach (Column c in b.columns.Values)
                {
                    foreach(Task t in c.Tasks.Values)
                    {
                        t.taskDTO.Delete();
                    }
                    c.columnDTO.Delete();
                    c.Tasks.Clear();
                }
                b.boardDTO.Delete();
                b.columns.Clear();
                boards.Remove(b.boardID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        /// <summary>
        /// This method transfer the owner of the board from one user to another.
        /// </summary>
        /// <param name="currentOwnerEmail">The current owner user email</param>
        /// <param name="newOwnerEmail">The email of the new owner of the board</param>
        /// <param name="boardID">The board ID</param>
        /// <returns>void, unless an error occurs</returns>
        internal void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, int boardID)
        {
            if (!CheckBoardUser(currentOwnerEmail, boardID))
            {
                log.Warn("The current user is not assign to the board");
                throw new Exception("The current user is not assign to the board");
            }
            if (!uc.IsExists(newOwnerEmail))
                throw new ArgumentException("The email of the new owner does not exists in the system");
            if (!CheckBoardUser(newOwnerEmail, boardID))
            {
                log.Warn("The new owner user is not assign to the board");
                throw new Exception("The new owner user is not assign to the board");
            }
            if (!boards[boardID].owner.Equals(currentOwnerEmail))
                throw new ArgumentException(currentOwnerEmail + " is not the owner of the board");
            try
            {
                Board board = GetBoard(boardID);
                board.boardDTO.Owner = newOwnerEmail;
                board.owner = newOwnerEmail;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This method recive a user email and an ID board and check if the user connected to this board.
        /// </summary>
        /// <param name="email">The user email</param>
        /// <param name="boardID">The board ID</param>
        /// <returns>Boolean if the user connected to this board.</returns>
        internal bool CheckBoardUser(string email, int boardID)
        {
            User user = uc.GetUser(email);
            if (user.BoardIDs.Contains(boardID))
                return true;
            log.Warn("This user does not connected to the board with this ID");
            return false;
        }

        /// <summary>
        /// This method return a Board according to the user email and board name.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="nameBoard">The board name.</param>
        /// <returns>A board with this board name and user email.</returns>
        internal Board GetBoard(string email, string nameBoard)
        {
            try
            {                
                foreach (var b in uc.GetUser(email).BoardIDs)
                {
                    if (boards[b].name.Equals(nameBoard))
                        return boards[b];
                }
                log.Warn("there is no Board with this ID for this User");
                throw new Exception("there is no Board with this ID for this User");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This method return a Board according to the user email and board name.
        /// </summary>
        /// <param name="boardID">The board ID.</param>
        /// <returns>A board with this ID.</returns>
        internal Board GetBoard(int boardID)
        {
            if (!boards.ContainsKey(boardID))
            {
                log.Warn("there is no Board with this ID in the system");
                throw new Exception("there is no Board with this ID in the system");
            }
            return boards[boardID];
        }

        /// <summary>
        /// This method check is the user have a board with this name already.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <param name="nameBoard">The board name.</param>
        /// <returns>Boolean if there is a board with this name, or not.</returns>
        internal bool IsBoardNameExists(string email, string nameBoard)
        {
            try
            {               
                foreach (var b in uc.GetUser(email).BoardIDs)
                {
                    if (boards[b].name.Equals(nameBoard))
                        return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This method return a list of the in progress list column in this board.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>A list of the in progress tasks, unless an error occurs</returns>
        internal List<Task> InProgressTasks(string email)
        {
            try
            {
                List<Task> ListTasks = new List<Task>();
                User user = uc.GetUser(email);
                foreach (var b in user.BoardIDs)
                {
                    Board board = GetBoard(b);
                    List<Task> ListTaskBoard = board.InProgressTasks(email);
                    if (ListTaskBoard.Count > 0)
                    {
                        foreach (var t in ListTaskBoard)
                            ListTasks.Add(t);
                    }
                }
                return ListTasks;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This method loads the boards data from the DAL to the BL.
        /// </summary>
        /// <returns>void</returns>
        internal void LoadData()
        {
            try
            {
                List<DTO> DTOs = new BoardMapper().Select();
                foreach (BoardDTO board in DTOs)
                {
                    List<UserBoardDTO> userBoardDTOs = new UserBoardMapper().GetUsers(board.boardID);
                    List<User> users = UBtoUser(userBoardDTOs);
                    this._boards.Add(board.boardID, new Board(board, users));
                }
                counterIDboards = MaxID() + 1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        /// <summary>
        /// This method find the max ID of the boards.
        /// </summary>
        /// <returns>the max ID</returns>
        private int MaxID()
        {
            if (_boards == null || _boards.Count == 0)
                return 0;
            int max = 0;
            foreach (Board b in _boards.Values)
            {
                if (b.boardID > max) 
                    max = b.boardID;   
            }
            return max;
        }

        private List<User> UBtoUser(List<UserBoardDTO> userBoardDTOs)
        {
            List<User> users = new List<User>();
            foreach (UserBoardDTO ub in userBoardDTOs)
            {
                User u = uc.GetUser(ub.userEmail);
                u.BoardIDs.Add(ub.boardID);
                users.Add(u);
            }
                
            return users;
        }

        /// <summary>
        /// This method delate all the existing data.
        /// </summary>
        /// <returns>void</returns>
        internal void DeleteData()
        {
            try
            {
                new BoardMapper().DeleteAll();
                new ColumnMapper().DeleteAll();
                new TaskMapper().DeleteAll();
                new UserBoardMapper().DeleteAll();
                boards.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
