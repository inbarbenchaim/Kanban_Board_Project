using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;


namespace IntroSE.Backend.Fronted.BusinessLayer
{
    public class UserController
    {
        internal Dictionary<string, User> users { get; set; }

        private static readonly ILog log = LogManager.GetLogger("UserController");

        public UserController()
        {
            this.users = new Dictionary<string, User>();
        }

        /// <summary>
        /// This method check if the user exsist in the system, and if he is logged in.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <returns>boolean.</returns>
        internal bool CheckUser(string email)
        {
            if (!IsExists(email))
            {
                log.Warn("There is no user exist with this email");
                return false;
            }
            email = email.ToLower();
            if (!IsLogIn(email))
            {
                log.Warn("The user is not log in in the system");
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method create a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>void.</returns>
        internal void CreateUser(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                log.Debug("User with null email attempted register");
                throw new Exception("Email is null");
            }
            email = email.ToLower();
            if (users.ContainsKey(email))
            {
                throw new Exception($"Email {email} already exists");
            }
            try
            {
                User u = new User(email, password);
                users.Add(email, u);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// This method check if a user exists in the system.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <returns>boolean if the user exists in the system.</returns>
        internal bool IsExists(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                log.Debug("User with null email attempted to find a user");
                return false;
            }
            email = email.ToLower();
            return users.ContainsKey(email);
        }

        /// <summary>
        /// This method return the user class if he exists in the system.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <returns>User object eith this email.</returns>
        internal User GetUser(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                log.Debug("Attempted to get user with null email");
                throw new Exception($"Email is null");
            }
            email = email.ToLower();
            if (!users.ContainsKey(email))
            {
                log.Debug("There is no user exist with this email");
                throw new Exception($"Email {email} is not contain in this form");
            }
            return users[email];
        }

        /// <summary>
        /// This method return if the user is log in.
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <returns>boolean if the user is log in.</returns>
        internal bool IsLogIn(string email)
        {
            email = email.ToLower();
            if (!users[email].Status)
                log.Warn("User is not log in");
            return users[email].Status;
        }

        ///<summary>This method loads all persisted data.
        /// </summary>
        ///<returns>void, unless an error occurs</returns>
        internal void LoadData()
        {
            try
            {
                List<DTO> DTOs = new UserMapper().Select();
                foreach (UserDTO user in DTOs)
                    this.users.Add(user.Email, new User(user));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        ///<summary>This method deletes all persisted data.
        /// </summary>
        ///<returns>void, unless an error occurs</returns>
        internal void DeleteData()
        {
            try
            {
                new UserMapper().DeleteAll();
                this.users.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }
    }
}
