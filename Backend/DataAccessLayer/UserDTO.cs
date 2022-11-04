using log4net;
using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserDTO : DTO
    {
        public const string emailColumnName = "Email";
        public const string passwordColumnName = "Password";
        private static readonly ILog log = LogManager.GetLogger("UserDTO");

        private readonly string _email;
        public string Email { get => _email; }
        private string _password;
        public string Password { get => _password; set { _password = value; mapper.Update(emailColumnName, Email, passwordColumnName, Password.ToString()); } }

        internal UserDTO(string email, string password) : base(new UserMapper())
        {
            this._email = email;
            this._password = password;
        }

        /// <summary>
        /// This method save the new user in the database.
        /// </summary>
        /// <returns> void </returns>
        public void Save()
        {
            UserMapper map = new UserMapper();
            if (!map.Insert(this))
                throw new Exception("the creation of the User in the DB failed");
        }
    }
}