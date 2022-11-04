
namespace Frontend.Model
{
    public class MessageModel : NotifiableModelObject
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                this._id = value;
                RaisePropertyChanged("Id");
            }
        }
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                this._title = value;
                RaisePropertyChanged("Title");
            }
        }
        private string _body;
        public string Body
        {
            get => _body;
            set
            {
                this._body = value;
                RaisePropertyChanged("Body");
                //Controller.UpdateBody(UserEmail, Id, value);
            }
        }
        private string UserEmail; //storing this user here is an hack becuase static & singletone are not allowed.
        public MessageModel(BackendController controller, int id, string title, string body, string user_email) : base(controller)
        {
            Id = id;
            Title = title;
            Body = body;
            UserEmail = user_email;
        }

        public MessageModel(BackendController controller, (int Id, string Title, string Body) message, UserModel user) : this(controller, message.Id, message.Title, message.Body, user.Email) { }

    }
}
