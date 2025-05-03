namespace Micro.CodeGen.Models
{
    class Class
    {
        public string Namespace { get; set; }

        public string Name { get; set; }

        public Method[] Methods { get; set; }
    }
}
