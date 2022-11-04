using IntroSE.Backend.Fronted.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;

namespace BackendTests
{
    internal class BoardTests
    {
        private readonly BoardService board;
        private readonly ColumnService column;

        public BoardTests(BoardService board,ColumnService column)
        {
            this.board = board;
            this.column = column;
        }

        public void RunAddBoardTests()
        {
            //attempt to add new Board with title 'testBoard' - should succeed
            //assuming that there is no Board exist with this title
            string res1 = board.AddBoard("guy@gmail.com", "testBoard");
            var response1 = JsonSerializer.Deserialize<Response>(res1);
            if (response1 != null && response1.ErrorMessage != null)
            {
                Console.WriteLine(response1.ErrorMessage);
            }
            else if (response1 != null)
            {
                Console.WriteLine("Board added successfully");
            }

            //attempt to add new Board with title that already exist in the kanban - should return Error
            //assuming that Board with this title already exist 
            string res2 = board.AddBoard("guy@gmail.com", "testBoard");
            var response2 = JsonSerializer.Deserialize<Response>(res2);
            if (response2 != null && response2.ErrorMessage != null)
            {
                Console.WriteLine(response2.ErrorMessage);
            }
            else if (response2 != null)
            {
                Console.WriteLine("Board added successfully");
            }

            //attempt to add new Board with an empty title - should return Error
            string res3 = board.AddBoard("guy@gmail.com", "");
            var response3 = JsonSerializer.Deserialize<Response>(res3);
            if (response3 != null && response3.ErrorMessage != null)
            {
                Console.WriteLine(response3.ErrorMessage);
            }
            else if (response3 != null)
            {
                Console.WriteLine("Board added successfully");
            }
        }

        public void RunRemoveBoardsTests()
        {

            //attempt to remove board with user that is not the owner - should return error
            //assuming that Board with this title does exist 
            string res53 = board.RemoveBoard("ido@gmail.com", "testBoard");
            var response53 = JsonSerializer.Deserialize<Response>(res53);
            if (response53 != null && response53.ErrorMessage != null)
            {
                Console.WriteLine(response53.ErrorMessage);
            }
            else if (response53 != null)
            {
                Console.WriteLine("Board removed successfully");
            }

            //attempt to remove board with user that is the owner- should succeed
            //assuming that Board with this title does exist 
            string res13 = board.RemoveBoard("guy@gmail.com", "testBoard");
            var response13 = JsonSerializer.Deserialize<Response>(res13);
            if (response13 != null && response13.ErrorMessage != null)
            {
                Console.WriteLine(response13.ErrorMessage);
            }
            else if (response13 != null)
            {
                Console.WriteLine("Board removed successfully");
            }

            //attempt to remove board - should return Error
            //assuming that Board with this title does not exist 
            string res14 = board.RemoveBoard("guy@gmail.com", "testBoard");
            var response14 = JsonSerializer.Deserialize<Response>(res14);
            if (response14 != null && response14.ErrorMessage != null)
            {
                Console.WriteLine(response14.ErrorMessage);
            }
            else if (response14 != null)
            {
                Console.WriteLine("Board removed successfully");
            }

            //attempt to remove board when given empty title - should return Error
            string res15 = board.RemoveBoard("guy@gmail.com", "");
            var response15 = JsonSerializer.Deserialize<Response>(res15);
            if (response15 != null && response15.ErrorMessage != null)
            {
                Console.WriteLine(response15.ErrorMessage);
            }
            else if (response15 != null)
            {
                Console.WriteLine("Board removed successfully");
            }
        }

   
        
        public void RunGetColumnTests()
        {
            //attempt to recieve the name of a specefic column that does not exist - should return Error
            //assuming that column with this name exist
            string res11 = column.GetColumn("guy@gmail.com", 0, "backlog");
            var response11 = JsonSerializer.Deserialize<Response>(res11);
            if (response11 != null && response11.ErrorMessage != null)
            {
                Console.WriteLine(response11.ErrorMessage);
            }
            else if (response11 != null)
            {
                Console.WriteLine("column returned successfully");
            }

            //attempt to recieve the name of a specefic column that does not exist - should return Error
            //assuming that column with this name does not exist
            string res12 = column.GetColumn("guy@gmail.com",0, "testColumn");
            var response12 = JsonSerializer.Deserialize<Response>(res12);
            if (response12 != null && response12.ErrorMessage != null)
            {
                Console.WriteLine(response12.ErrorMessage);
            }
            else if (response12 != null)
            {
                Console.WriteLine("column returned successfully");
            }
        }
        
        public void RunTestsGetBoardName()
        {
            //attempt to recieve the name of boardID 0 - should succeed
            //assuming that board with this id is exists
            string res13 = board.GetBoardName(0);
            var response13 = JsonSerializer.Deserialize<Response>(res13);
            if (response13 != null && response13.ErrorMessage != null)
            {
                Console.WriteLine(response13.ErrorMessage);
            }
            else if (response13 != null)
            {
                Console.WriteLine("board name returned successfully");
            }

            //attempt to recieve the name of boardID 100 - should returned error
            //assuming that column with this Id does not exist
            string res14 = board.GetBoardName(100);
            var response14 = JsonSerializer.Deserialize<Response>(res14);
            if (response14 != null && response14.ErrorMessage != null)
            {
                Console.WriteLine(response14.ErrorMessage);
            }
            else if (response14 != null)
            {
                Console.WriteLine("board name returned successfully");
            }
        }

        public void RunTestsJoinBoard()
        {
            //attempt to join Board of another user - should succeed
            string res1 = board.JoinBoard("ido@gmail.com", 0);
            var response1 = JsonSerializer.Deserialize<Response>(res1);
            if (response1 != null && response1.ErrorMessage != null)
            {
                Console.WriteLine(response1.ErrorMessage);
            }
            else if (response1 != null)
            {
                Console.WriteLine("joined board successfully");
            }
        }

        public void RunTestsLeaveBoard()
        {
            //attempt to leave Board of another user - should succeed
            string res1 = board.LeaveBoard("guy@gmail.com", 0);
            var response1 = JsonSerializer.Deserialize<Response>(res1);
            if (response1 != null && response1.ErrorMessage != null)
            {
                Console.WriteLine(response1.ErrorMessage);
            }
            else if (response1 != null)
            {
                Console.WriteLine("left board successfully");
            }
        }

        public void RunTestsTransferOwnership()
        {
            //attempt to change Board owner to another user - should succeed
            string res1 = board.TransferOwnership("guy@gmail.com", "ido@gmail.com", 0);
            var response1 = JsonSerializer.Deserialize<Response>(res1);
            if (response1 != null && response1.ErrorMessage != null)
            {
                Console.WriteLine(response1.ErrorMessage);
            }
            else if (response1 != null)
            {
                Console.WriteLine("board  ownership transfered successfully");
            }
        }
    }
}
