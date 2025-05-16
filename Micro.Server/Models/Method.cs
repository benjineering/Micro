namespace Micro.Server.Models
{
    public class Method
    {
        public string Name { get; set; }

        public Parameter[] Parameters { get; set; }

        public TypeName ReturnType { get; set; }

        public Method(string name, Parameter[] parameters, TypeName returnType)
        {
            Name = name;
            Parameters = parameters;
            ReturnType = returnType;
        }
    }
}
