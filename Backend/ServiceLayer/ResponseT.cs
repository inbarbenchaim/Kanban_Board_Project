namespace Backend.Service
{
    ///<summary>This class extends <c>Response</c> and represents the result of a call to a non-void function. 
    ///In addition to the behavior of <c>Response</c>, the class holds the value of the returned value in the variable <c>Value</c>.</summary>
    ///<typeparam name="T">The type of the returned value of the function, stored by the list.</typeparam>
    public class ResponseT<T>
    {
        public string ErrorMessage { get; set; }

        public T ReturnValue { get; set; }
        public ResponseT() { }

        public ResponseT(string msg, T returnValue)
        {
            ErrorMessage = msg;
            this.ReturnValue = returnValue;
        }
    }
}
