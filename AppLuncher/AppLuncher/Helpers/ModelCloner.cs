using System;
using AppLuncher.Models;
using Newtonsoft.Json;

namespace AppLuncher.Helpers
{
    public static class ModelCloner
    {
        public static T Clone<T>(T source)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }

        public static void CopyLauncherItem(LauncherItem source, LauncherItem destination)
        {
            destination.Name = source.Name;
            destination.IconPath = source.IconPath;
            destination.IconIndex = source.IconIndex;
            destination.RunAsAdministrator = source.RunAsAdministrator;
            destination.Actions = source.Actions;
        }

        public static LauncherGroup DuplicateGroup(LauncherGroup source)
        {
            LauncherGroup copy = Clone(source);
            RegenerateGroupIds(copy);
            copy.Name = CreateCopyName(copy.Name);
            return copy;
        }

        public static LauncherItem DuplicateLauncherItem(LauncherItem source)
        {
            LauncherItem copy = Clone(source);
            RegenerateLauncherItemIds(copy);
            copy.Name = CreateCopyName(copy.Name);
            return copy;
        }

        private static void RegenerateGroupIds(LauncherGroup group)
        {
            group.Id = Guid.NewGuid();

            foreach (LauncherItem item in group.Items)
            {
                RegenerateLauncherItemIds(item);
            }

            foreach (LauncherGroup childGroup in group.ChildGroups)
            {
                RegenerateGroupIds(childGroup);
            }
        }

        private static void RegenerateLauncherItemIds(LauncherItem item)
        {
            item.Id = Guid.NewGuid();
            foreach (LaunchAction action in item.Actions)
            {
                action.Id = Guid.NewGuid();
            }
        }

        private static string CreateCopyName(string name)
        {
            return (string.IsNullOrWhiteSpace(name) ? "Unnamed" : name) + " - Copy";
        }
    }
}
