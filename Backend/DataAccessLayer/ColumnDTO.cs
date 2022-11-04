using log4net;
using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class ColumnDTO : DTO
    {
        public const string ColumnNameColumnName = "ColumnName";
        public const string LimitColumnName = "ColumnLimit";
        public const string BoardIDColumnName = "BoardID";

        private static readonly ILog log = LogManager.GetLogger("ColumnDTO");

        private readonly int _boardID;
        public int boardID { get => _boardID; }

        private readonly string _columnName;
        public string columnName { get => _columnName; }
        private int _limit;
        public int limit { get => _limit; set { _limit = value; mapper.Update(boardID, LimitColumnName, value, ColumnNameColumnName, columnName); } }

        internal ColumnDTO(string columnName, int limit, int BoardID) : base(new ColumnMapper())
        {
            _columnName = columnName;
            _limit = limit;
            _boardID = BoardID;
        }

        /// <summary>
        /// This method save this column in the database.
        /// </summary>
        /// <returns> void </returns>
        public void Save()
        {
            ColumnMapper map = new ColumnMapper();
            map.Insert(this);
        }

        /// <summary>
        /// This method delete this column from the database.
        /// </summary>
        /// <returns> void </returns>
        public void Delete()
        {            
            if (!mapper.Delete(ColumnNameColumnName, BoardIDColumnName, columnName, this.boardID))
                throw new Exception("the deletion of the Board in the DB failed");            
        }
    }
}