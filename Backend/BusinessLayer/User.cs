using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Text.RegularExpressions;

namespace IntroSE.Backend.Fronted.BusinessLayer
{
    internal class User
    {
        private static readonly ILog log = LogManager.GetLogger("User");

        private List<int> _BoardIDs;
        internal List<int> BoardIDs { get => _BoardIDs; set { _BoardIDs = value; } }

        public const int backlog = 0;
        public const int inProgress = 1;
        public const int done = 2;

        public string UserEmail { get; private set; }
        public string Password { get; private set; }
        public bool Status { get; private set; }

        private UserDTO _userDTO;
        internal UserDTO userDTO { get => _userDTO; }


        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="useremail">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>void.</returns>
        public User(string useremail, string password)
        {
            if (!IsValidEmail(useremail))
            {
                log.Debug("User with invalid email attempted register");
                throw new Exception("Email is invalid");
            }
            if (!IsValidPassword(password))
            {
                log.Debug("User with invalid password attempted register");
                throw new Exception("Email is password");
            }
            this._BoardIDs = new List<int>();
            this.UserEmail = useremail;
            this.Password = password;
            this.Status = true;
            try
            {
                this._userDTO = new UserDTO(useremail, password);
                userDTO.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public User(UserDTO user)
        {
            this.UserEmail=user.Email;
            this.Password = user.Password;
            this.Status = false;
            this._userDTO = user;
            this._BoardIDs = new List<int>();
        }

        /// <summary>
        /// This method check if the password is valid.
        /// </summary>
        /// <param name="password">The password that need to be checked.</param>
        /// <returns>boolean if the password is valid.</returns>
        internal bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) //if password is null
            {
                log.Warn("Password cannot be null");
                return false;
            }

            bool existUpper = false;
            bool existSmall = false;
            bool existNumber = false;
            if (password.Length < 6 | password.Length > 20) //Length check
            {
                log.Warn("Invalid password length");
                return false;
            }
            for (int i = 0; i < password.Length; i++) //Contains capital letter, small letter, and a digit
            {
                if ((password[i] <= 'Z') & (password[i]) >= 'A')
                    existUpper = true;
                if ((password[i] <= 'z') & (password[i] >= 'a'))
                    existSmall = true;
                if ((password[i] <= '9') & (password[i] >= '0'))
                    existNumber = true;
            }
            if (!existUpper) //Missing capital letter
            {
                log.Warn("Invalid password : must contains a uppercase letter");
                return false;
            }
            if (!existSmall) //Missing small letter
            {
                log.Warn("Invalid password : must contains a small character");
                return false;
            }
            if (!existNumber) //Missing digit
            {
                log.Warn("Invalid password : must contains a digit");
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method check if the email is valid.
        /// </summary>
        /// <param name="email">The email that need to be checked.</param>
        /// <returns>boolean if the email is valid.</returns>
        internal bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                log.Warn("Email string cannot be null or contain white space");
                return false;
            }
            try
            {
                email = email.ToLower();
                string pattern = "^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$";
                var regex = new Regex(pattern);
                return regex.IsMatch(email);
            }
            catch (RegexMatchTimeoutException)
            {
                log.Debug("Email address is incorrent");
                return false;
            }
            catch (ArgumentException)
            {

                log.Debug("Email address is incorrent");
                return false;
            }
        }

        /// <summary>
        /// This method perform login for the user in the system.
        /// </summary>
        /// <param name="useremail">The user email address.</param>
        /// <param name="password">The user password.</param>
        /// <returns>void.</returns>
        internal void Login(string password)
        {
            if (this.Password != password)
            {
                log.Debug("User with wrong password attempted login");
                throw new Exception($"Password {password} is wrong");
            }
            this.Status = true;
        }

        /// <summary>
        /// This method perform logout from the current user in the system.
        /// </summary>
        /// <param name="useremail">The user email address.</param>
        /// <returns>void.</returns>
        internal void Logout()
        {
            if (Status == false)  //If user is already logged out
            {
                log.Warn("User is not logged in");
                throw new Exception("User is not logged in");
            }
            this.Status = false;
        }

        /// <summary>
        /// This method changed the password for the user in the system.
        /// </summary>
        /// <param name="oldP">The user old password.</param>
        /// <param name="newP">The user new password.</param>
        /// <returns>void.</returns>
        internal void ChangePassword(string oldP, string newP)
        {
            if (Status == false)
            {
                log.Warn("User is not logged in");
                throw new Exception("User is not logged in");
            }
            if (!IsValidPassword(newP))
            {
                log.Warn("User with invalid new password attempted to change password");
                throw new Exception("User with invalid new password attempted to change password");
            }
            if (oldP != Password) {
                log.Debug("User with incorrect password attempted to change password");
                throw new Exception($"Password {oldP} is not matching to the current one");
            }
            this.Password = newP; // change password successfully
        }

        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>The BoardIDs list, unless an error occurs (see <see cref="GradingService"/>)</returns>
        internal List<int> GetUserBoards()
        {
            if (Status == false)
            {
                log.Warn("User is not logged in");
                throw new Exception("User is not logged in");
            }
            return BoardIDs;
        }

        /// <summary>
        /// This method add a board to the user's boards.
        /// </summary>
        /// <param name="boardID">The ID of the board that we want to add.</param>
        /// <returns>void, unless an error occurs</returns>
        internal void AddBoard(int boardID)
        {
            BoardIDs.Add(boardID);
        }
    }
}
