using System;
using System.Collections.Generic;

namespace AppLuncher.Models
{
    public sealed class LauncherItem
    {
        public LauncherItem()
        {
            Id = Guid.NewGuid();
            Name = "New Launcher";
            Actions = new List<LaunchAction>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string IconPath { get; set; }

        public List<LaunchAction> Actions { get; set; }
    }
}
