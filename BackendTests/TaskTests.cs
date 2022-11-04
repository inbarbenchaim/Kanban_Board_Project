using IntroSE.Backend.Fronted.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;

namespace BackendTests
{
    internal class TaskTests
    {
        private readonly TaskService t;

        public TaskTests(TaskService t)
        {
            this.t = t;
        }

        public void RunAdvanceTaskTests()
        {
            //attempt to advance task to the next column - should succeeed
            //assuming that the task and the columns ID are exist(the next column as well)
            string res9 = t.AdvanceTask("ido@gmail.com", 0, 0, 1);
            var response9 = JsonSerializer.Deserialize<Response>(res9);
            if (response9 != null && response9.ErrorMessage != null)
            {
                Console.WriteLine(response9.ErrorMessage);
            }
            else if (response9 != null)
            {
                Console.WriteLine("Task advanced successfully");
            }

            //attempt to advance task to the next column - should succeeed
            //assuming that the task and the columns ID are exist(the next column as well)
            string res10 = t.AdvanceTask("guy@gmail.com", 0, 0, 2);
            var response10 = JsonSerializer.Deserialize<Response>(res10);
            if (response10 != null && response10.ErrorMessage != null)
            {
                Console.WriteLine(response10.ErrorMessage);
            }
            else if (response10 != null)
            {
                Console.WriteLine("Task advanced successfully");
            }

            //attempt to recieve the name of a specefic column (according to ID) - should succeed
            //assuming that column with this Id does exist
        }
        public void RunAddTaskTests()
        {
            //attempt to add new task with title 'titleTest' - should succeed
            //assuming that the numbers of tasks in the column we using did not reach his limit
            string res4 = t.AddTask("guy@gmail.com", 0, "titleTest", "tests", new DateTime(2022, 07, 27, 23, 59, 59));
            var response4 = JsonSerializer.Deserialize<Response>(res4);
            if (response4 != null && response4.ErrorMessage != null)
            {
                Console.WriteLine(response4.ErrorMessage);
            }
            else if (response4 != null)
            {
                Console.WriteLine("Task added successfully");
            }

            //attempt to add new task with title 'titleTest'(already exist) - should secceed
            //assuming that task with the title 'titleTest' already exist in the column and it is ok
            string res5 = t.AddTask("guy@gmail.com", 0, "titleTest", "tests", new DateTime(2022, 07, 27, 23, 59, 59));
            var response5 = JsonSerializer.Deserialize<Response>(res5);
            if (response5 != null && response5.ErrorMessage != null)
            {
                Console.WriteLine(response5.ErrorMessage);
            }
            else if (response5 != null)
            {
                Console.WriteLine("Task added successfully");
            }

            //attempt to add new task with empty title - should return Error
            //assuming that it is illeagl to add new task without title
            string res6 = t.AddTask("guy@gmail.com", 0, "", "tests", new DateTime(2022, 07, 27, 23, 59, 59));
            var response6 = JsonSerializer.Deserialize<Response>(res6);
            if (response6 != null && response6.ErrorMessage != null)
            {
                Console.WriteLine(response6.ErrorMessage);
            }
            else if (response6 != null)
            {
                Console.WriteLine("Task added successfully");
            }

            //attempt to add new Task with empty description - should succeed
            //assuming that the numbers of tasks in the column we using did not reach his limit
            string res7 = t.AddTask("guy@gmail.com", 0, "titleTest2", "", new DateTime(2022, 07, 27, 23, 59, 59));
            var response7 = JsonSerializer.Deserialize<Response>(res7);
            if (response7 != null && response7.ErrorMessage != null)
            {
                Console.WriteLine(response7.ErrorMessage);
            }
            else if (response7 != null)
            {
                Console.WriteLine("Task added successfully");
            }
            //attempt to add new Task with empty due date - should return error
            //assuming that the numbers of tasks in the column we using did not reach his limit
            string res8 = t.AddTask("guy@gmail.com", 0, "titleTest3", "tests", new DateTime());
            var response8 = JsonSerializer.Deserialize<Response>(res8);
            if (response8 != null && response8.ErrorMessage != null)
            {
                Console.WriteLine(response8.ErrorMessage);
            }
            else if (response8 != null)
            {
                Console.WriteLine("Task added successfully");
            }
            //attempt to add new Task - should succeed
            //assuming that the numbers of tasks in the column we using did not reach his limit
            string res88 = t.AddTask("guy@gmail.com", 0, "titleTest3", "tests", new DateTime(2022, 07, 27, 23, 59, 59));
            var response88 = JsonSerializer.Deserialize<Response>(res88);
            if (response88 != null && response88.ErrorMessage != null)
            {
                Console.WriteLine(response88.ErrorMessage);
            }
            else if (response88 != null)
            {
                Console.WriteLine("Task added successfully");
            }

        }
        public void RunTestsUpdateTaskDueDate()
        {
            //attempt to update task due date with a user that is not the assignee- should failed
            //assuming that the column and task ID are exist
            string res26 = t.UpdateTaskDueDate("guy@gmail.com", 0, new DateTime(2024, 04, 27, 23, 59, 59), "in progress",1);
            var response26 = JsonSerializer.Deserialize<Response>(res26);

            if (response26 != null && response26.ErrorMessage != null)
            {
                Console.WriteLine(response26.ErrorMessage);
            }
            else if (response26 != null)
            {
                Console.WriteLine("Task due date updated successfully");
            }

            //attempt to update task due date - should succeed
            //assuming that the column and task ID are exist
            string res36 = t.UpdateTaskDueDate("ido@gmail.com", 0, new DateTime(2024, 04, 27, 23, 59, 59), "in progress", 1);
            var response36 = JsonSerializer.Deserialize<Response>(res36);

            if (response36 != null && response36.ErrorMessage != null)
            {
                Console.WriteLine(response36.ErrorMessage);
            }
            else if (response36 != null)
            {
                Console.WriteLine("Task due date updated successfully");
            }

            //attempt to update task due date to a date that already passed - should return Error
            //assuming that the column and task ID are exist
            string res27 = t.UpdateTaskDueDate("ido@gmail.com", 0, new DateTime(2020, 04, 27, 23, 59, 59), "in progress", 1);
            var response27 = JsonSerializer.Deserialize<Response>(res27);

            if (response27 != null && response27.ErrorMessage != null)
            {
                Console.WriteLine(response27.ErrorMessage);
            }
            else if (response27 != null)
            {
                Console.WriteLine("Task due date updated successfully");
            }

            //attempt to update task due date to an empty due date (without due date) - should return Error
            //cannot update the dueDate to null
            string res28 = t.UpdateTaskDueDate("ido@gmail.com", 0, new DateTime(), "in progress", 1);
            var response28 = JsonSerializer.Deserialize<Response>(res28);

            if (response28 != null && response28.ErrorMessage != null)
            {
                Console.WriteLine(response28.ErrorMessage);
            }
            else if (response28 != null)
            {
                Console.WriteLine("Task due date updated successfully");
            }
        }
        public void RunTestsUpdateTaskTitle()
        {
            //attempt to update Task title to a new title with a user that is not the assignee - should return Error
            //assuming that the column and task ID are exist
            string res29 = t.UpdateTaskTitle("guy@gmail.com", 0, "backlog", 0,"newTitle");
            var response29 = JsonSerializer.Deserialize<Response>(res29);

            if (response29 != null && response29.ErrorMessage != null)
            {
                Console.WriteLine(response29.ErrorMessage);
            }
            else if (response29 != null)
            {
                Console.WriteLine("Task title updated successfully");
            }

            //attempt to update Task title to a new title with a user that is the assignee - should succeed
            //assuming that the column and task ID are exist
            string res39 = t.UpdateTaskTitle("ido@gmail.com", 0, "in progress", 1, "newTitle");
            var response39 = JsonSerializer.Deserialize<Response>(res39);

            if (response39 != null && response39.ErrorMessage != null)
            {
                Console.WriteLine(response39.ErrorMessage);
            }
            else if (response39 != null)
            {
                Console.WriteLine("Task title updated successfully");
            }

            //attempt to update Task title to an empty title - should return Error
            //assuming that the column and task ID are exist
            string res30 = t.UpdateTaskTitle("ido@gmail.com", 0, "in progress", 1,"");
            var response30 = JsonSerializer.Deserialize<Response>(res30);

            if (response30 != null && response30.ErrorMessage != null)
            {
                Console.WriteLine(response30.ErrorMessage);
            }
            else if (response30 != null)
            {
                Console.WriteLine("Task title updated successfully");
            }
        }
        public void RunTestsUpdateTaskDescription()
        {
            //attempt to update Task description with user that is not the assignee - should return error
            //assuming that the column and task ID are exist
            string res31 = t.UpdateTaskDescription("guy@gmail.com", 0, "backlog", 0, "new description");
            var response31 = JsonSerializer.Deserialize<Response>(res31);

            if (response31 != null && response31.ErrorMessage != null)
            {
                Console.WriteLine(response31.ErrorMessage);
            }
            else if (response31 != null)
            {
                Console.WriteLine("Task description updated successfully");
            }

            //attempt to update Task description with user that is the assignee - should succeed
            //assuming that the column and task ID are exist
            string res41 = t.UpdateTaskDescription("ido@gmail.com", 0, "in progress", 1, "new description");
            var response41 = JsonSerializer.Deserialize<Response>(res41);

            if (response41 != null && response41.ErrorMessage != null)
            {
                Console.WriteLine(response41.ErrorMessage);
            }
            else if (response41 != null)
            {
                Console.WriteLine("Task description updated successfully");
            }

            //attempt to update Task description to an empty description - should succeed
            //assuming that the column and task ID are exist
            string res32 = t.UpdateTaskDescription("ido@gmail.com", 0, "in progress", 1, "");
            var response32 = JsonSerializer.Deserialize<Response>(res32);

            if (response32 != null && response32.ErrorMessage != null)
            {
                Console.WriteLine(response32.ErrorMessage);
            }
            else if (response32 != null)
            {
                Console.WriteLine("Task description updated successfully");
            }
        }

        public void RunTestsAssignTask()
        {
            //attempt to assign Task 1 to another user - should succeed

            string res1 = t.AssignTask("guy@gmail.com", 0, "backlog", 1, "ido@gmail.com");
            var response1 = JsonSerializer.Deserialize<Response>(res1);

            if (response1 != null && response1.ErrorMessage != null)
            {
                Console.WriteLine(response1.ErrorMessage);
            }
            else if (response1 != null)
            {
                Console.WriteLine("Assign task successfully");
            }
        }

        public void RunTestsInProgressTasks()
        {
            //attempt to create a list of all tasks from 'InProgress' column from all boards - should succeed
            //assuming that 'guy@gmail.com' user exist
            string res8 = t.InProgressTasks("ido@gmail.com");
            var response8 = JsonSerializer.Deserialize<Response>(res8);
            if (response8 != null && response8.ErrorMessage != null)
            {
                Console.WriteLine(response8.ErrorMessage);
            }
            else if (response8 != null)
            {
                Console.WriteLine("InProgressTasks List created successfully");
            }
        }
    }
}