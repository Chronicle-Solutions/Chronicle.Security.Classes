using Chronicle.Plugins.Core;

namespace Chronicle.Security.Classes
{
    public class ClassManager : IPlugable
    {
        public string PluginName => "Class Management";

        public string PluginDescription => "Create and Update Operator Class Templates";

        public Version Version => new Version(1,0,0,0);

        public int Execute()
        {
            (new Classes()).Show();
            return 0;
        }
    }
}
