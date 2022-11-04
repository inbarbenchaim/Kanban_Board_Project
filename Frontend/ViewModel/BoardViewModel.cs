using Frontend.Model;
using System;
using System.Windows;
using System.Windows.Media;

namespace Frontend.ViewModel

{
    public class BoardViewModel : NotifiableObject
    {
        private BackendController controller;

        public UserModel user { get; private set; }
        public BoardModel board { get; private set; }
        public string Title { get; private set; }

        private MessageModel _selectedMessage;
        public MessageModel SelectedMessage
        {
            get
            {
                return _selectedMessage;
            }
            set
            {
                _selectedMessage = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedTask");
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

        /*
        public void Logout()
        {
            controller.Logout(user.Email);
        }
        /*

        /*
        public BoardViewModel(UserModel user)
        {
            this.controller = user.Controller;
            this.user = user;
            Title = "Board for " + user.Email;
            board = user.GetBoard();
        }
        */

        public BoardViewModel(BoardModel board, UserModel user)
        {
            this.user = user;
            this.board = board;
            this.controller = board.Controller;
            Title = this.board.Name;
            //this.SetTasksBorderColors();
        }
        /*
        public void SetTasksBorderColors() //Colors the task's border
        {
            foreach (ColumnModel column in board.Columns)
            {
                foreach (TaskModel task in column.Tasks)
                {
                    if (task.EmailAssignee.Equals(user.Email))
                        task.BackGroundColor = new SolidColorBrush(Colors.Blue);
                    else
                        task.BackGroundColor = new SolidColorBrush(Colors.White);
                }
            }
        }
        */

        public void RemoveMessage()
        {

            try
            {
                board.RemoveMessage(SelectedMessage);
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot remove message. " + e.Message);
            }

        }
    }
}