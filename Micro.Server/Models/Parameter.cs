namespace Micro.Server.Models
{
    public class Parameter
    {
        public string Name { get; set; }

        public TypeName Type { get; set; }

        public Parameter(string name, TypeName type)
        {
            Name = name;
            Type = type;
        }
    }
}
