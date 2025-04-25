namespace Micro.Common.Config
{
    public class MicroConfig
    {
        public UiProject UiProject { get; set; } = new UiProject();
    }

    public class UiProject
    {
    #if DEBUG
        private const string _defaultUiProjectRoot = "../../../../../dev-svelte-app";
    #else
        private const string _defaultUiProjectRoot = "./ui";
    #endif

        public string UiProjectRoot { get; set; } = "";
    }
}
