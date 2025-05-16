namespace Micro.Config
{
    public class MicroConfig
    {
        public UiProject UiProject { get; set; } = new UiProject();
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
