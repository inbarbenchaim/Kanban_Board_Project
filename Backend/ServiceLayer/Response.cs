namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Response
    {
        public string ErrorMessage { get; set;}

        public object ReturnValue { get; set;}

        public Response() { }

        public Response(string msg, object returnValue)
        {
            ErrorMessage = msg;
            this.ReturnValue = returnValue;
        }
    }
}