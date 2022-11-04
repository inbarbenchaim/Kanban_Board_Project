using log4net;
using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardDTO : DTO
    {
        //private readonly int _boardId;

        public const string BoardNameColumnName = "BoardName";
        public const string BoardOwnerColumnName = "Owner";
        public const string counterIDtaskColumnName = "counterIDtask";
        private static readonly ILog log = LogManager.GetLogger("BoardDTO");


        private int _boardID;
        public int boardID { get => _boardID; }

        private string _name;
        public string Name { get => _name; set { _name = value; mapper.Update(DTO.IDColumnName, boardID.ToString(), BoardNameColumnName, value); } }
        private string _owner;
        public string Owner { get => _owner; set { _name = value; mapper.Update(DTO.IDColumnName, boardID.ToString(), BoardOwnerColumnName, value); } }
        private int _counterIDtask;
        public int counterIDtask { get => _counterIDtask; set { _counterIDtask = value; mapper.Update(DTO.IDColumnName, boardID.ToString(), counterIDtaskColumnName, value.ToString()); } }

        internal BoardDTO(int boardID, string name, string owner, int counterIDtask) : base(new BoardMapper())
        {
            this._boardID = boardID;
            this._name = name;
            this._owner = owner;
            this._counterIDtask = counterIDtask;
        }

        /// <summary>
        /// This method save the new board in the database.
        /// </summary>
        /// <returns> void </returns>
        public void Save()
        {
            BoardMapper map = new BoardMapper();
            if (!map.Insert(this))
                throw new Exception("the creation of the Board in the DB failed");
            
        }

        /// <summary>
        /// This method delete this board from the database.
        /// </summary>
        /// <returns> void </returns>
        public void Delete()
        {
            if (!mapper.Delete(IDColumnName, this.boardID.ToString()))
                throw new Exception("the deletion of the Board in the DB failed");
        }
    }
}