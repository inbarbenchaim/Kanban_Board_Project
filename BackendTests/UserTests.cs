using IntroSE.Backend.Fronted.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;

namespace BackendTests
{
    internal class UserTests
    {
        private readonly UserService user;

        public UserTests(UserService u)
        {
            this.user = u;
        }

        public void RunTestsRegister()
        {

            //register guy@gmail.com to the Kanban - should succeed
            //assuming that there is no user with this email in the kanban
            string res1 = user.Register("guy@gmail.com", "ABCabc123");
            Response? response1 = JsonSerializer.Deserialize<Response>(res1);
            if (response1 != null && response1.ErrorMessage != null)
            {
                Console.WriteLine(response1.ErrorMessage);
            }
            else if (response1 != null)
            {
                Console.WriteLine("Created new user successfully");
            }

            //attempt to register guy@gmail.com - should return Error
            //assuming that user with this email is already exist in the kanban
            string res2 = user.Register("guy@gmail.com", "ABCabc123");
            var response2 = JsonSerializer.Deserialize<Response>(res2);

            if (response2 != null && response2.ErrorMessage != null)
            {
                Console.WriteLine(response2.ErrorMessage);
            }
            else if (response2 != null)
            {
                Console.WriteLine("Created new user successfully");
            }

            //attempt to register with an empty username field - should return Error
            string res3 = user.Register(null, "1235");
            var response3 = JsonSerializer.Deserialize<Response>(res3);

            if (response3 != null && response3.ErrorMessage != null)
            {
                Console.WriteLine(response3.ErrorMessage);
            }
            else if (response3 != null)
            {
                Console.WriteLine("Created new user successfully");
            }


            //register ido@gmail.com to the Kanban - should succeed
            //assuming that there is no user with this email in the kanban
            string res4 = user.Register("ido@gmail.com", "ABCabc123");
            Response? response4 = JsonSerializer.Deserialize<Response>(res4);
            if (response4 != null && response4.ErrorMessage != null)
            {
                Console.WriteLine(response4.ErrorMessage);
            }
            else if (response4 != null)
            {
                Console.WriteLine("Created new user successfully");
            }
        }

        public void RunTestsLogin()
        {
            //attempt to login 'guy@gmail.com' user with the incorrect password - should return Error
            //assuming that user with this email exist in the kanban with other password
            string res4 = user.Login("guy@gmail.com", "1235");
            var response4 = JsonSerializer.Deserialize<Response>(res4);

            if (response4 != null && response4.ErrorMessage != null)
            {
                Console.WriteLine(response4.ErrorMessage);
            }
            else if (response4 != null)
            {
                Console.WriteLine("Login user successfully");
            }

            //attempt to login guy@gmail.com user with the correct password - should succced
            //assuming that user with this email and password exist in the kanban and logged out
            string res5 = user.Login("guy@gmail.com", "ABCabc123");
            var response5 = JsonSerializer.Deserialize<Response>(res5);

            if (response5 != null && response5.ErrorMessage != null)
            {
                Console.WriteLine(response5.ErrorMessage);
            }
            else if (response5 != null)
            {
                Console.WriteLine("Login user successfully");
            }

            //attempt to login guy@walla.com - should return Error
            //assuming that 'guy@walla.com' user does not exist
            string res6 = user.Login("guy@walla.com", "1235");
            var response6 = JsonSerializer.Deserialize<Response>(res6);

            if (response6 != null && response6.ErrorMessage != null)
            {
                Console.WriteLine(response6.ErrorMessage);
            }
            else if (response6 != null)
            {
                Console.WriteLine("Login user successfully");
            }

            //attempt to login with empty username field - should return Error
            string res7 = user.Login(null, "1235");
            var response7 = JsonSerializer.Deserialize<Response>(res7);

            if (response7 != null && response7.ErrorMessage != null)
            {
                Console.WriteLine(response7.ErrorMessage);
            }
            else if (response7 != null)
            {
                Console.WriteLine("Login user successfully");
            }

            //attempt to login guy@gmail.com user with the correct password - should succced
            //assuming that user with this email and password exist in the kanban and logged out
            string res8 = user.Login("ido@gmail.com", "ABCabc123");
            var response8 = JsonSerializer.Deserialize<Response>(res8);

            if (response8 != null && response8.ErrorMessage != null)
            {
                Console.WriteLine(response8.ErrorMessage);
            }
            else if (response8 != null)
            {
                Console.WriteLine("Login user successfully");
            }
        }



        public void RunTestsLogout()
        {
            //attempt to logout with 'guy@gmail.com' when the user is logged in-should succesed
            string res9 = user.Logout("guy@gmail.com");
            var response9 = JsonSerializer.Deserialize<Response>(res9);

            if (response9 != null && response9.ErrorMessage != null)
            {
                Console.WriteLine(response9.ErrorMessage);
            }
            else if (response9 != null)
            {
                Console.WriteLine("Logout user successfully");
            }

            //attempt to logout with 'guy@gmail.com' when the user is logged out - should failed
            string res10 = user.Logout("guy@gmail.com");
            var response10 = JsonSerializer.Deserialize<Response>(res10);

            if (response10 != null && response10.ErrorMessage != null)
            {
                Console.WriteLine(response10.ErrorMessage);
            }
            else if (response10 != null)
            {
                Console.WriteLine("Logout user successfully");
            }

            //attempt to logout with 'ido@gmail.com' when the user is logged in-should succesed
            string res11 = user.Logout("ido@gmail.com");
            var response11 = JsonSerializer.Deserialize<Response>(res11);

            if (response11 != null && response11.ErrorMessage != null)
            {
                Console.WriteLine(response11.ErrorMessage);
            }
            else if (response11 != null)
            {
                Console.WriteLine("Logout user successfully");
            }

        }

        public void RunTestsGetUserBoards()
        {
            //attempt to return guy@gmail.com boards - should succesed
            string res12 = user.GetUserBoards("guy@gmail.com");
            var response12 = JsonSerializer.Deserialize<Response>(res12);

            if (response12 != null && response12.ErrorMessage != null)
            {
                Console.WriteLine(response12.ErrorMessage);
            }
            else if (response12 != null)
            {
                Console.WriteLine("user  boards returned successfully");
            }
        }


    }
}
