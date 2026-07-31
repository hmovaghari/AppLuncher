using System.Collections.Generic;

namespace AppLuncher.Models
{
    public sealed class AppDatabase
    {
        public AppDatabase()
        {
            RootGroups = new List<LauncherGroup>();
        }

        public List<LauncherGroup> RootGroups { get; set; }
    }
}
