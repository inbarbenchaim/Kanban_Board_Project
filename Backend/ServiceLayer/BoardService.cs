using IntroSE.Backend.Fronted.BusinessLayer;
using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Text.Json;
using log4net;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using log4net.Config;
using System.Reflection;
using System.IO;
using Backend.Service;

namespace IntroSE.Backend.Fronted.ServiceLayer
{
    public class BoardService
    {
        private static readonly ILog log = LogManager.GetLogger("BoardService");
        private readonly UserController uc;
        internal readonly BoardController bc;

        public BoardService(UserController uc, BoardController bc)
        {
            this.uc = uc;
            this.bc = bc;
        }

        /// <summary>
        /// This method add a boardID to the list of the users boards ID.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        public string AddBoard(string email, string boardName)
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
                var user = uc.GetUser(email);
                bc.AddBoard(user, boardName);
                log.Debug("new Board created with the name: " + boardName);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        ///<summary>
        /// This method receives a boardname, and remove the boardID of the board with this name from the user's boards.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="boardName">The name of the board</param>
        ///<returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        public string RemoveBoard(string email, string boardName)
        {
            Response res = new Response();
            try
            {
                if (!uc.CheckUser(email))
                {
                    log.Warn("The user is not exsist or not logged in");
                    throw new Exception("The user is not exsist or not logged in");
                }
                var user = uc.GetUser(email);
                bc.RemoveBoard(user, boardName);
                log.Debug("Board " + boardName + " deleted");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        /// <summary>
        /// This method adds a user as member to an existing board.
        /// </summary>
        /// <param name="email">The email of the user that joins the board. Must be logged in</param>
        /// <param name="boardID">The board's ID</param>
        ///<returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        public string JoinBoard(string email, int boardID)
        {
            Response res = new Response();
            try
            {
                if (!uc.CheckUser(email))
                {
                    log.Warn("The user is not exsist or not logged in");
                    throw new Exception("The user is not exsits or not logged in");
                }
                bc.JoinBoard(email, boardID);
                log.Info("Board added to user Boards succesfully");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }


        ///<summary>
        /// This method receives a user email and board ID, and delete the user from the board's user list.
        /// </summary>
        /// <param name="email">The user email address.</param>
        /// <param name="boardID">The ID of the board</param>
        ///<returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        public string LeaveBoard(string email, int boardID)
        {
            if (!uc.CheckUser(email))
            {
                log.Warn("The user is not exsist or not logged in");
                throw new Exception("The user is not exsist or not logged in");
            }
            Response res = new Response();
            try
            {
            email = email.ToLower();
                bc.LeaveBoard(email, boardID);
                log.Info("Board removed from user Boards succesfully");
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
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        ///<returns>The string in json format that represent a Response object with an
        /// error messeage or the board name</returns>        
        public string GetBoardName(int boardId)
        {
            Response res = new Response();
            try
            {
                res.ReturnValue = bc.GetBoardName(boardId);
                log.Info("Board name returned succesfully");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }
        */
        /// <summary>
        /// This method returns a board's name
        /// </summary>
        /// <param name="boardId">The board's ID</param>
        ///<returns>The string in json format that represent a Response object with an
        /// error messeage or the board name</returns>        
        public string GetBoardName(int boardId)
        {
            ResponseT<string> res = new ResponseT<string>();
            try
            {
                res.ReturnValue = bc.GetBoardName(boardId);
                log.Info("Board name returned succesfully");
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

        /// <summary>
        /// This method transfer ownership of the board from one user to another
        /// </summary>
        /// <param name="currentOwnerEmail">The current owner user email</param>
        /// <param name="newOwnerEmail">The new owner user email</param>
        /// <param name="boardID">The board ID</param>
        ///<returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, int boardID)
        {
            if (!uc.CheckUser(currentOwnerEmail))
            {
                log.Warn("A user with ownerEmail email is not exsist or not logged in");
                throw new Exception("A user with ownerEmail email is not exsits or not logged in");
            }
            Response res = new Response();
            try
            {
                bc.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardID);
                log.Info("Board name returned succesfully");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        ///<summary>
        /// This method Loads the Boards data from the DB.
        /// </summary>
        ///<returns> string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        public string LoadData()
        {
            Response res = new Response();
            try
            {
                bc.LoadData();
                log.Debug("Data was loaded!");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

        ///<summary>
        /// This method delete the Boards data from the DB.
        /// </summary>
        ///<returns> string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        public string DeleteData()
        {
            Response res = new Response();
            try
            {
                bc.DeleteData();
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
        /// <param name="res">The response.</param>
        ///<returns>The string in json format that represent a Response object with an
        /// error messeage or empty response</returns>
        /// 
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

        public string GetBoard(int id)
        {
            Response res = new Response();
            try
            {
                Board b = bc.GetBoard(id);
                res.ReturnValue = b;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res.ErrorMessage = ex.Message;
            }
            return ReturnJson(res);
        }

    }
}
