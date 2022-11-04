using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Backend.Fronted.BusinessLayer;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Backend.Fronted.ServiceLayer;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ServiceFactory
    {
        private static readonly ILog log = LogManager.GetLogger("ServiceFactory");
        private UserController userController;
        private BoardController boardController;
        private UserService userService;
        private BoardService boardService;
        private ColumnService columnService;
        private TaskService taskService;

        public ServiceFactory()
        { 
            userController = new UserController();
            boardController = new BoardController(userController);
            userService = new UserService(userController);
            boardService = new BoardService(userController, boardController);
            columnService = new ColumnService(userController, boardController);
            taskService = new TaskService(userController,boardController);
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        public UserController GetUserController()
        {
            return userController;
        }

        public BoardController GetBoardController()
        {
            return boardController; 
        }

        public UserService GetUserService()
        {
            return userService;
        }

        public BoardService GetBoardService()
        {
            return boardService;
        }

        public ColumnService GetColumnService()
        {
            return columnService;
        }

        public TaskService GetTaskService()
        {
            return taskService;
        }
    }
}
