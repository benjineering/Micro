namespace Micro.CodeGen.Models
{
    class Klass
    {
        public string Namespace { get; set; }

        public string Name { get; set; }

        public Method[] Methods { get; set; }
    }
}
