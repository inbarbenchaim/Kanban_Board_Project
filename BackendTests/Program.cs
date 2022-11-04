using BackendTests;
using IntroSE.Backend.Fronted.BusinessLayer;
using IntroSE.Backend.Fronted.ServiceLayer;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using Kanban.BackendTests.Program;

namespace IntroSE.ForumSystem.Frontend
{
    class Program
    {
        public static void Main(string[] args)
        {

            UserController uc = new UserController();
            BoardController bc = new BoardController(uc);
            UserService user = new UserService(uc);
            BoardService board = new BoardService(uc, bc);
            TaskService task = new TaskService(uc, bc);
            ColumnService column = new ColumnService(uc, bc);

            UserTests userTests = new UserTests(user);
            BoardTests boardTests = new BoardTests(board, column);
            TaskTests taskTests = new TaskTests(task);
            ColumnTests columnTests = new ColumnTests(column);
            DalTests datTests = new DalTests(board, user);

            Console.WriteLine("Tests for Delete Data:");
            datTests.RunTestsDeleteData();
            Console.WriteLine("---------------------------------------------------------------------------");

            /*
            Console.WriteLine("Tests for Register:");
            userTests.RunTestsRegister();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Logout:");
            userTests.RunTestsLogout();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Login:");
            userTests.RunTestsLogin();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Add Board:");
            boardTests.RunAddBoardTests();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Get User Boards:");
            userTests.RunTestsGetUserBoards();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Get Board Name:");
            boardTests.RunTestsGetBoardName();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Join Board:");
            boardTests.RunTestsJoinBoard();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Add Task:");
            taskTests.RunAddTaskTests();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Assign Task:");
            taskTests.RunTestsAssignTask();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Advance Task:");
            taskTests.RunAdvanceTaskTests();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Get Column:");
            boardTests.RunGetColumnTests();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for In Progress Tasks:");
            taskTests.RunTestsInProgressTasks();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Get Column:");
            boardTests.RunGetColumnTests();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Limit Column:");
            columnTests.RunTestsLimit();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Get Limit:");
            columnTests.RunTestsGetLimit();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Get Name:");
            columnTests.RunTestsGetName();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Update Task Due Date:");
            taskTests.RunTestsUpdateTaskDueDate();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Update Task Title:");
            taskTests.RunTestsUpdateTaskTitle();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Update Task Description:");
            taskTests.RunTestsUpdateTaskDescription();
            Console.WriteLine("---------------------------------------------------------------------------"); 

            Console.WriteLine("Tests for Transfer Ownership:");
            boardTests.RunTestsTransferOwnership();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Leave Board:");
            boardTests.RunTestsLeaveBoard();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Remove Boards:");
            boardTests.RunRemoveBoardsTests();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Delete Data:");
            datTests.RunTestsDeleteData();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine("Tests for Load Data:");
            datTests.RunTestsLoadData();
            Console.WriteLine("---------------------------------------------------------------------------");
            */
            user.Register("mail@mail.com", "Password1");
            board.AddBoard("mail@mail.com", "board1");
            board.AddBoard("mail@mail.com", "board2");
            task.AddTask("mail@mail.com", 0, "title1", "description1", new DateTime(2023, 07, 27, 23, 59, 59));
            task.AddTask("mail@mail.com", 0, "title2", "description2", new DateTime(2023, 07, 27, 23, 59, 59));
            task.AddTask("mail@mail.com", 0, "title3", "description3", new DateTime(2023, 07, 27, 23, 59, 59));
        }
    }
}
