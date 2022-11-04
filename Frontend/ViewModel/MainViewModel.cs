using Frontend.Model;
using System;

namespace Frontend.ViewModel
{
    class MainViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        private string _userEmail;
        public string UserEmail
        {
            get => _userEmail;
            set
            {
                this._userEmail = value;
                RaisePropertyChanged("UserEmail");
            }
        }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                this._password = value;
                RaisePropertyChanged("Password");
            }
        }
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                this._message = value;
                RaisePropertyChanged("Message");
            }
        }

        /// <summary>
        /// This method continue the process of Login method via the BackendController .
        /// </summary>
        /// <returns>UserModel object if succeed, or null</returns>*/
        public UserModel Login()
        {
            Message = "";
            try
            {
                return Controller.Login(UserEmail, Password);
            }
            catch (Exception e)
            {
                Message = e.Message;
                return null;
            }
        }

        /// <summary>
        /// This method continue the process of Register method via the BackendController .
        /// </summary>
        /// <returns>void</returns>*/
        public void Register()
        {
            Message = "";
            try
            {
                Controller.Register(UserEmail, Password);
                Message = "Registered successfully";
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        public MainViewModel()
        {
            this.Controller = new BackendController();
            this.UserEmail = null;
            this.Password = null;
        }
    }
}
