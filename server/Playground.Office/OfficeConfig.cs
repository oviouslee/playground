using Playground.Core.Extensions;

namespace Playground.Office
{
    public class OfficeConfig
    {
        public string directory;

        public string Directory
        {
            get => directory;
            set
            {
                directory = value;
                Directory.EnsureDirectoryExists();
            }
        }
    }
}