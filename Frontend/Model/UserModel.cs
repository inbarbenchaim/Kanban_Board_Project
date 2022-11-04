using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Frontend.Model
{
    public class UserModel : NotifiableModelObject
    {
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        public ObservableCollection<BoardModel> boards { get; set; }

        /*
        public BoardModel GetBoard()
        {
            return new BoardModel(Controller, this);
        }
        */

        public List<BoardModel> GetBoards()
        {
            return null;
        }

        //public UserModel(BackendController controller, string email) : base(controller)
        //{
        //    this.Email = email;
        //}

        private UserModel(BackendController controller, ObservableCollection<BoardModel> boards, string email) : base(controller)
        {
            //this.boards = boards;
            //this.Email = email;
            //this.boards.CollectionChanged += HandleChange;
        }

        public UserModel(BackendController controller, string email) : base(controller)
        {
            this.Email = email;
            this.boards = new ObservableCollection<BoardModel>();
            /*
            this.boards = new ObservableCollection<BoardModel>(controller.GetAllboardIDs(email).
                Select((i) => new BoardModel(controller, controller.GetBoard(i), i)).ToList());
            */
            List<int> list = controller.GetAllboardIDs(email);
            List<BoardModel> BoardsModel = new List<BoardModel>();
            foreach(int i in list)
            {
                BoardModel b = new BoardModel(controller, controller.GetBoardName(i), i, Email);
                this.boards.Add(b);
            }
            //this.boards.CollectionChanged += HandleChange;
        }
        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            //read more here: https://stackoverflow.com/questions/4279185/what-is-the-use-of-observablecollection-in-net/4279274#4279274
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (BoardModel y in e.OldItems)
                {

                    //Controller.RemoveMessage(user.Email, y.Id);
                }

            }
        }
    }
}
