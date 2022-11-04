using IntroSE.Backend.Fronted.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kanban.BackendTests.Program
{
    internal class DalTests
    {
        private readonly BoardService board;
        private readonly UserService user;

        public DalTests(BoardService board, UserService user)
        {
            this.board = board;
            this.user = user;
        }

        public void RunTestsLoadData()
        {
            //attempt to load data of users - should succeed
            string res1 = user.LoadData();
            var response1 = JsonSerializer.Deserialize<Response>(res1);

            if (response1 != null && response1.ErrorMessage != null)
            {
                Console.WriteLine(response1.ErrorMessage);
            }
            else if (response1 != null)
            {
                Console.WriteLine("loaded users data successfully");
            }

            //attempt to load data of boards - should succeed

            /*
            string res2 = board.LoadData();
            var response2 = JsonSerializer.Deserialize<Response>(res2);

            if (response2 != null && response2.ErrorMessage != null)
            {
                Console.WriteLine(response2.ErrorMessage);
            }
            else if (response2 != null)
            {
                Console.WriteLine("loaded boards data successfully");
            }
            */
        }

        public void RunTestsDeleteData()
        {
            //attempt to load data of users - should succeed
            string res1 = user.DeleteData();
            var response1 = JsonSerializer.Deserialize<Response>(res1);

            if (response1 != null && response1.ErrorMessage != null)
            {
                Console.WriteLine(response1.ErrorMessage);
            }
            else if (response1 != null)
            {
                Console.WriteLine("deleted users data successfully");
            }

            //attempt to load data of boards - should succeed

            string res2 = board.DeleteData();
            var response2 = JsonSerializer.Deserialize<Response>(res2);

            if (response2 != null && response2.ErrorMessage != null)
            {
                Console.WriteLine(response2.ErrorMessage);
            }
            else if (response2 != null)
            {
                Console.WriteLine("deleted boards data successfully");
            }
        }
    }
}
