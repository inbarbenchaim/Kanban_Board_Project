using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Frontend.ViewModel

{
    public class UserViewModel : NotifiableObject
    {
        private BackendController controller;
        public UserModel user { get; private set; }

        public SolidColorBrush BackgroundColor
        {
            get
            {
                return new SolidColorBrush(user.Email.Contains("mail") ? Colors.Blue : Colors.Red);
            }
        }

        //public ObservableCollection<BoardModel> Boards { get; private set; }
        public string Title { get; private set; }
        private BoardModel _selectedBoard;
        public BoardModel SelectedBoard
        {
            get
            {
                return _selectedBoard;
            }
            set
            {
                _selectedBoard = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedBoard");
            }
        }
        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
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
        /// This method continue the process of Logout method via the BackendController .
        /// </summary>
        /// <returns>void</returns>*/
        internal void Logout()
        {
            Message = "";
            try
            {
                controller.Logout(user.Email);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
        }

        public UserViewModel(UserModel user)
        {
            this.controller = user.Controller;
            this.user = user;
            Title = "Boards of " + user.Email;
        }

        public void RemoveMessage()
        {

            try
            {
                //Board.RemoveMessage(SelectedMessage);
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot remove message. " + e.Message);
            }

        }

        /*
        public BoardModel GetBoard()
        {

        }
        */
    }
}