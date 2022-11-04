using System;
using System.Collections.Generic;
using System.Windows;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Backend.Fronted.ServiceLayer;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Backend.Fronted.BusinessLayer;
using System.Text.Json.Serialization;
using System.Collections.ObjectModel;
using System.Text.Json;
using Backend.Service;

namespace Frontend.Model
{
    public class BackendController
    {
        private BoardService boardService { get; set; }
        private ServiceFactory serviceFactory { get; set; }
        private UserService userService { get; set; }
        private ColumnService columnService { get; set; }

        public BackendController()
        {
            this.serviceFactory = new ServiceFactory();
            this.userService = new UserService(serviceFactory.GetUserController());
            this.boardService = new BoardService(serviceFactory.GetUserController(), serviceFactory.GetBoardController());
            this.columnService = this.serviceFactory.GetColumnService();
            userService.LoadData();
            boardService.LoadData();
        }
        /// <summary>
        /// This method continue the process of login method via the userService.
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>A UserModel </returns>*/
        public UserModel Login(string username, string password)
        {
            string login = userService.Login(username, password);
            var user = JsonSerializer.Deserialize<Response>(login);
            if (user != null && user.ErrorMessage != null)
            {
                throw new Exception(user.ErrorMessage);
            }
            return new UserModel(this, username);
        }

        /// <summary>
        /// This method continue the process of GetAllboardIDs method via the userService.
        /// </summary>
        /// <param name="email">The username of the user</param>
        /// <returns>A List of int that represent all the ID's of the boards of the user </returns>*/
        public List<int> GetAllboardIDs(string email)
        {
            string json = userService.GetUserBoards(email);
            ResponseT<List<int>> res = JsonSerializer.Deserialize<ResponseT<List<int>>>(json);

            List<int> list = res.ReturnValue;
            return list;
        }


        /// <summary>
        /// This method continue the process of GetBoardName method via the boardService.
        /// </summary>
        /// <param name="Id">The ID of the board</param>
        /// <returns>A string of the board name </returns>*/
        internal string GetBoardName(int Id)
        {            
            string json = boardService.GetBoardName(Id);
            ResponseT<string> res = JsonSerializer.Deserialize<ResponseT<string>>(json);
            return res.ReturnValue;
        }


        /// <summary>
        /// This method continue the process of Register method via the userService.
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>void</returns>*/
        internal void Register(string username, string password)
        {
            string register = userService.Register(username, password);
            var res = JsonSerializer.Deserialize<Response>(register);
            if (res != null && res.ErrorMessage != null)
            {
                throw new Exception(res.ErrorMessage);
            }
        }

        /// <summary>
        /// This method continue the process of Logout method via the userService.
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <returns>void</returns>*/
        public void Logout(string email)
        {
            string json = userService.Logout(email);
            var res = JsonSerializer.Deserialize<Response>(json);
            if ( res != null && res.ErrorMessage != null) //if logout failed
                throw new Exception(res.ErrorMessage);
        }

        /// <summary>
        /// This method continue the process of GetColumn method via the columnService.
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="BoardID">The board ID</param>
        /// <param name="name">The name of the column</param>
        /// <returns>ObservableCollection<TaskModel> object that represent the column with the tasks</returns>*/
        public ObservableCollection<TaskModel> GetColumn(string email, int BoardID, string name)
        {
            ObservableCollection<TaskModel> column = new ObservableCollection<TaskModel>();
            string json = columnService.GetColumn(email, BoardID, name);
            ResponseT<List<string>> res = JsonSerializer.Deserialize<ResponseT<List<string>>>(json);
            List<string> TaskTitles = res.ReturnValue;
            List<int> IDs = initializeID(email, BoardID, name);
            List<string> Descriptions = initializeDescription(email, BoardID, name);
            for (int i = 0; i < TaskTitles.Count; i++)
            {
                column.Add(new TaskModel(this, TaskTitles[i], IDs[i], Descriptions[i]));
            }
            return column;
        }

        /// <summary>
        /// This method convert the return value field from the json that returned from GetTasksIDs method.
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="BoardID">The board ID</param>
        /// <param name="name">The name of the column</param>
        /// <returns>List<int> that represent the taskID's list</returns>*/
        private List<int> initializeID(string email, int BoardID, string name)
        {
            string json = columnService.GetTasksIDs(email, BoardID, name);
            ResponseT<List<int>> res = JsonSerializer.Deserialize<ResponseT<List<int>>>(json);
            List<int> TaskIDs = res.ReturnValue;
            return TaskIDs;
        }

        /// <summary>
        /// This method convert the return value field from the json that returned from GetTasksDescription method.
        /// </summary>
        /// <param name="email">The email of the user</param>
        /// <param name="BoardID">The board ID</param>
        /// <param name="name">The name of the column</param>
        /// <returns>List<string> that represent the task description list</returns>*/
        private List<string> initializeDescription(string email, int BoardID, string name)
        {
            string json = columnService.GetTasksDescription(email, BoardID, name);
            ResponseT<List<string>> res = JsonSerializer.Deserialize<ResponseT<List<string>>>(json);
            List<string> TasksDesc = res.ReturnValue;
            return TasksDesc;
        }

    }

}
