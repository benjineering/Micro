namespace Micro.CodeGen.Config
{
    public class MicroConfig
    {
        public ServerGeneratorType ServerGeneratorType { get; set; }
        public UiProject UiProject { get; set; } = new UiProject();
    }

    public enum ServerGeneratorType
    {
        Http,
        Lambda
    }

    public class UiProject
    {
    #if DEBUG
        private const string _defaultOutputDir = "../../../../../dev-micro-ui-app/micro/generated";
    #else
        private const string _defaultOutputDir = "./ui/micro";
    #endif

        public string OutputDir { get; set; } = _defaultOutputDir;
    }
}
