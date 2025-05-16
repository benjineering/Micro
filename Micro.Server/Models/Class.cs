namespace Micro.Server.Models
{
    public class Class
    {
        public TypeName Name { get; }

        public Method[] Methods { get; }

        public Class(TypeName name, Method[] methods)
        {
            Name = name;
            Methods = methods;
        }
    }
}
