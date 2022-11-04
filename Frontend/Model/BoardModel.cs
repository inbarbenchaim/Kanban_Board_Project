using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Frontend.Model
{
    public class BoardModel : NotifiableModelObject
    {
        //private readonly UserModel user;
        //private readonly BackendController backendController;
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                this._name = value;
                RaisePropertyChanged("Name");
            }
        }
        private int _id;
        public int ID
        {
            get => _id;
            set
            {
                this._id = value;
                RaisePropertyChanged("ID");
                //Controller.UpdateBody(UserEmail, Id, value);
            }
        }

        public ObservableCollection<TaskModel> backlog { get; set; }
        public ObservableCollection<TaskModel> inProgress { get; set; }
        public ObservableCollection<TaskModel> done { get; set; }

        public BoardModel(BackendController controller, string name, int id, string email) : base(controller)
        {
            this._name = name;
            this._id = id;
            this.backlog = controller.GetColumn(email, id,"backlog");
            this.inProgress = controller.GetColumn(email, id, "in progress");
            this.done = controller.GetColumn(email, id, "done");            
        }


        /*
        public ObservableCollection<MessageModel> Messages { get; set; }
        private ObservableCollection<ColumnModel> columns;
        public ObservableCollection<ColumnModel> Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
                RaisePropertyChanged("Columns");
            }
        }
        private int ID;
        private string Name;
        */

        /*
        public BoardModel(BackendController controller, UserModel user) : base(controller)
        {
            this.user = user;
            backendController = controller;
        }
        */


        /*
        internal BoardModel(BackendController controller, int id, string name) : base(controller)
        {            
            this.ID = id;
            this.Name = name;
            //Messages.CollectionChanged += HandleChange;
        }
        public BoardModel(BackendController controller, UserModel user) : base(controller)
        {
            this.user = user;                        
            Messages = new ObservableCollection<MessageModel>(controller.GetAllBoards(user.Email).
                Select((c, i) => new MessageModel(controller, controller.GetMessage(user.Email,i),user)).ToList());             
            Messages.CollectionChanged += HandleChange;
        }
        */


        public void RemoveMessage(MessageModel t)
        {

            //Messages.Remove(t);

        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            /*
            //read more here: https://stackoverflow.com/questions/4279185/what-is-the-use-of-observablecollection-in-net/4279274#4279274
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (MessageModel y in e.OldItems)
                {

                    Controller.RemoveMessage(user.Email, y.Id);                    
                }

            }
        }

        public ColumnModel getColumn(int ordinal)
        {
            if (ordinal >= 0 & ordinal < Columns.Count)
            {
                foreach (ColumnModel item in Columns)
                {
                    if (ordinal == item.ColumnOrdinal)
                        return item;
                }

            }
            return null;
        }

        public ColumnModel getColumn(string columnName)
        {
            foreach (var item in Columns)
            {
                if (item.ColumnName.Equals(columnName))
                {
                    return item;
                }
            }
            return null; //If column was not found
        }
            */
        }
    }
}
