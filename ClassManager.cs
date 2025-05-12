using Chronicle.Plugins.Core;

namespace Chronicle.Security.Classes
{
    public class ClassManager : IPlugable
    {
        public override string PluginName => "Class Management";

        public override string PluginDescription => "Create and Update Operator Class Templates";

        public override Version Version => new Version(1,0,0,0);

        public override int Execute()
        {
            (new Classes()).Show();
            return 0;
        }
    }
}
