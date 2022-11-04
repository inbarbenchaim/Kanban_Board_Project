using System.Text.Json;
using IntroSE.Backend.Fronted.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace BackendTests
{
    internal class ColumnTests
    {
        private readonly ColumnService column;

        public ColumnTests(ColumnService column)
        {
            this.column = column;
        }

        public void RunTestsLimit()
        {
            //attempt to limit column 0 with invalid limit - should return Error
            //assuming that column with this ID exist
            //assuming the limit must be bigger than 0  
            string res1 = column.LimitColumn("guy@gmail.com", 0,0, -2);
            Response? response1 = JsonSerializer.Deserialize<Response>(res1);

            if (response1 != null && response1.ErrorMessage != null)
            {
                Console.WriteLine(response1.ErrorMessage);
            }
            else if (response1 != null)
            {
                Console.WriteLine("limit column applied successfully");
            }
            

            //attempt to limit column 0 with invalid limit - should return Error
            //assuming that column with this ID exist
            //assuming the limit must be bigger than 1
            string res2 = column.LimitColumn("guy@gmail.com", 0, 0, 0);
            Response? response2 = JsonSerializer.Deserialize<Response>(res2);

            if (response2 != null && response2.ErrorMessage != null)
            {
                Console.WriteLine(response2.ErrorMessage);
            }
            else if (response2 != null)
            {
                Console.WriteLine("limit column applied successfully");
            }

            //attempt to limit column 0 to 20 tasks - should succeed
            //assuming that column with this ID exist
            string res3 = column.LimitColumn("guy@gmail.com", 0, 0, 20);
            Response? response3 = JsonSerializer.Deserialize<Response>(res3);

            if (response3 != null && response3.ErrorMessage != null)
            {
                Console.WriteLine(response3.ErrorMessage);
            }
            else if (response3 != null)
            {
                Console.WriteLine("limit column applied successfully");
            }

            //attempt to limit column 0 to 1 tasks - should return Error
            //assuming that column with this ID exist and contains more tasks than the new limit(1)
            string res4 = column.LimitColumn("guy@gmail.com", 0, 0, 1);
            Response? response4 = JsonSerializer.Deserialize<Response>(res4);

            if (response4 != null && response4.ErrorMessage != null)
            {
                Console.WriteLine(response4.ErrorMessage);
            }
            else if (response4 != null)
            {
                Console.WriteLine("limit column applied successfully");
            }
        }

        public void RunTestsGetLimit()
        {
            //attempt to recieve the column limit of column 0 - should succeed
            //assunming that the limit of column 0 is 1
            string res5 = column.GetColumnLimit("guy@gmail.com", 0, 0);
            Response? response5 = JsonSerializer.Deserialize<Response>(res5);

            if (response5 != null && response5.ErrorMessage != null)
            {
                Console.WriteLine(response5.ErrorMessage);
            }
            else
            {
                Console.WriteLine("limit column returned successfully");
            }
        }

        public void RunTestsGetName()
        {
            //attempt to recieve the name of column 0 - should succeed
            //assuming that the name of column 0 is 'BackLog'
            string res6 = column.GetColumnName("guy@gmail.com", 0, 0);
            Response? response6 = JsonSerializer.Deserialize<Response>(res6);

            if (response6 != null && response6.ErrorMessage != null)
            {
                Console.WriteLine(response6.ErrorMessage);
            }
            else
            {
                Console.WriteLine("column name returned successfully");
            }
        }

        public void RunGetColumnTests()
        {
            //attempt to recieve the column - should succeed
            //assuming that the name of column 0 is 'backlog'
            string res7 = column.GetColumn("guy@gmail.com", 0, "backlog");
            Response ? response7 = JsonSerializer.Deserialize<Response>(res7);

            if (response7 != null && response7.ErrorMessage != null && (string)response7.ReturnValue != "backlog")
            {
                Console.WriteLine(response7.ErrorMessage);
            }
            else
            {
                Console.WriteLine("column tasklist returned successfully");
            }
        }
    }
}