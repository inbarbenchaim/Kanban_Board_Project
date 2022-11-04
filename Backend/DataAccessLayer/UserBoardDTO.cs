using log4net;
using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserBoardDTO : DTO
    {
        public const string userColumnName = "User";
        public const string BoardColumnName = "BoardID";
        private static readonly ILog log = LogManager.GetLogger("UserBoardDTO");

        private readonly int _boardID;
        private readonly string _userEmail;
        public int boardID { get => _boardID; }
        public string userEmail { get => _userEmail; }


        internal UserBoardDTO(string userEmail, int BoardID) : base(new UserBoardMapper())
        {
            this._boardID = BoardID;
            this._userEmail = userEmail;
        }

        /// <summary>
        /// This method save the new board and user connection in the database.
        /// </summary>
        /// <returns> void </returns>
        public void Save()
        {
            UserBoardMapper map = new UserBoardMapper();
            if (!map.Insert(this))
                throw new Exception("the creation of the connection between the user and the board in the DB failed");
        }

        /// <summary>
        /// This method delete the connection fot this user and board from the database.
        /// </summary>
        /// <returns> void </returns>
        public void Delete()
        {
            if (!mapper.Delete(userColumnName, BoardColumnName, this.userEmail, this.boardID))
                throw new Exception("the deletion of the connection between the user and the board in the DB failed");
        }
    }
}