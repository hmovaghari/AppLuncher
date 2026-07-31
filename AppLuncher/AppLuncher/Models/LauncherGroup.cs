using System;
using System.Collections.Generic;

namespace AppLuncher.Models
{
    public sealed class LauncherGroup
    {
        public LauncherGroup()
        {
            Id = Guid.NewGuid();
            Name = "New Group";
            ChildGroups = new List<LauncherGroup>();
            Items = new List<LauncherItem>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<LauncherGroup> ChildGroups { get; set; }

        public List<LauncherItem> Items { get; set; }
    }
}
