namespace Micro
{
    public class FunctionRequest
    {
        public string MethodName { get; }

        public FunctionRequest(string methodName)
        {
            MethodName = methodName;
        }
    }
}
