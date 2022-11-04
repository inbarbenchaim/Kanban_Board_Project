using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntroSE.Backend.Fronted.BusinessLayer;


namespace Frontend.Model
{
    public class TaskModel : NotifiableModelObject
    {
        /*
        private DateTime creationTime;
        public DateTime CreationTime
        {
            get
            {
                return creationTime;
            }
            set
            {
                creationTime = value;
                RaisePropertyChanged("CreationTime");
            }
        }
        private DateTime dueDate;
        public DateTime DueDate
        {
            get
            {
                return dueDate;
            }
            set
            {
                dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }
        */
        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }
        /*
        private string emailCreator;
        public string EmailCreator
        {
            get
            {
                return emailCreator;
            }
            set
            {
                emailCreator = value;
                RaisePropertyChanged("EmailCreator");
            }
        }
        */
        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                RaisePropertyChanged("Description");
            }
        }
        /*
        private string emailAssignee;
        public string EmailAssignee
        {
            get
            {
                return emailAssignee;
            }
            set
            {
                emailAssignee = value;
                RaisePropertyChanged("EmailAssignee");
            }
        }
        */
        private int taskId;
        public int TaskId
        {
            get { return taskId; }
            set
            {
                this.taskId = value;
                RaisePropertyChanged("TaskId");
            }
        }
        
        /*
        public TaskModel(BackendController controller, string emailAssignee, DateTime creationTime, DateTime dueDate, string title, string description, int taskId) : base(controller)
        {
            this.EmailAssignee = emailAssignee;
            this.CreationTime = creationTime;
            this.DueDate = dueDate;
            this.Title = title;
            this.TaskId = taskId;
            this.Description = description;
        }


        public TaskModel(BackendController controller, Task task) : this(controller, task.EmailAssignee, task.CreationTime, task.DueDate, task.Title, task.Description, task.Id)
        {

        }
        */
        public TaskModel(BackendController controller, string title, int taskId, string description) : base(controller)
        {
            this.title = title;
            this.taskId = taskId;
            this.description = description;
        }
    }
}