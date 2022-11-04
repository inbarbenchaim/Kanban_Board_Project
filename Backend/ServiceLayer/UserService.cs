using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Backend.Service;
using IntroSE.Backend.Fronted.BusinessLayer;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using log4net;
using log4net.Config;

namespace IntroSE.Backend.Fronted.ServiceLayer
{
    public class UserService
    {
        private static readonly ILog log = LogManager.GetLogger("UserService");
        private readonly UserController uc;

        public UserService(UserController uc)
        {
            this.uc = uc;;
        }

        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        public string Register(string email, string password)
        {
            Response res = new Response();
            try
            {
                uc.CreateUser(email, password);
                log.Info("Register done succesfully");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        /// <summary>
        /// This method perform login for the user in the system.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="password">The user password.</param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        public string Login(string email, string password)
        {
            Response res = new Response();
            try
            {
                User user = uc.GetUser(email);
                user.Login(password);
                log.Info("Logged in succesfully");
                res.ReturnValue = email;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        /// <summary>
        /// This method perform logout from the current user in the system.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        public string Logout(string email)
        {
            Response res = new Response();
            try
            {
                if (!uc.CheckUser(email))
                {
                    log.Warn("The user is not exsist or not logged in");
                    throw new Exception("The user is not exsist or not logged in");
                }
                email = email.ToLower();
                uc.GetUser(email).Logout();
                log.Info("Lougout succesfully");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        /// <summary>
        /// This method changed the password for the user in the system.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="oldP">The user old password.</param>
        /// <param name="newP">The user new password.</param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        public string ChangePassword(string email, string oldP, string newP)
        {
            Response res = new Response();
            try
            {
                if (!uc.CheckUser(email))
                {
                    log.Warn("The user is not exsist or not logged in");
                    throw new Exception("The user is not exsist or not logged in");
                }
                uc.GetUser(email).ChangePassword(oldP, newP);
                log.Info("password changed succesfully");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        /*
        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or a list of IDs of all user's boards</returns>
        public string GetUserBoards(string email)
        {
            Response res = new Response();
            try
            {
                if (!uc.CheckUser(email))
                {
                    log.Warn("The user is not exsist or not logged in");
                    throw new Exception("The user is not exsist or not logged in");
                }
                List<int> BoardIds = uc.GetUser(email).GetUserBoards();
                res.ReturnValue = BoardIds; 
                log.Info("Board Ids of the user returned successfully");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }
        */

        ///<summary>
        /// This method receives a response, and return error if the response failed.
        /// </summary>
        /// <param name="res">The response.</param>
        ///<returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        internal string ReturnJson(Response res)
        {
            try
            {
                var options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                return JsonSerializer.Serialize(res, res.GetType(), options);
            }
            catch
            {
                log.Error("Failed to serialize Response object");
                return "Error : Failed to serialize Response object";
            }
        }

        ///<summary>This method deletes all persisted data.
        /// </summary>
        ///<returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>        
        public string DeleteData()
        {
            Response res = new Response();
            try
            {
                uc.DeleteData();
                log.Debug("Data was deleted!");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        ///<summary>
        /// This method receives a response, and return error if the response failed.
        /// </summary>
        ///<returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        public string LoadData()
        {
            Response res = new Response();
            try
            {
                uc.LoadData();
                log.Debug("Data was loaded!");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        /// <summary>
        /// This method returns a list of IDs of all user's boards.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or a list of IDs of all user's boards</returns>
        public string GetUserBoards(string email)
        {
            ResponseT<List<int>> res = new ResponseT<List<int>>();          
            try
            {
                if (!uc.CheckUser(email))
                {
                    log.Warn("The user is not exsist or not logged in");
                    throw new Exception("The user is not exsist or not logged in");
                }
                List<int> BoardIds = uc.GetUser(email).GetUserBoards();
                res.ReturnValue = BoardIds;
                log.Info("Board Ids of the user returned successfully");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            try
            {
                var options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                return JsonSerializer.Serialize(res, res.GetType(), options);
            }
            catch
            {
                log.Error("Failed to serialize Response object");
                return "Error : Failed to serialize Response object";
            }
        }
    }
}
