using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppLuncher.Models;
using Newtonsoft.Json;

namespace AppLuncher.Services
{
    public sealed class JsonDatabaseService
    {
        private readonly JsonSerializerSettings serializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Include
        };

        public AppDatabase LoadOrCreate(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("A database file path is required.", "filePath");
            }

            if (!File.Exists(filePath))
            {
                AppDatabase newDatabase = new AppDatabase();
                Save(filePath, newDatabase);
                return newDatabase;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    AppDatabase emptyDatabase = new AppDatabase();
                    Save(filePath, emptyDatabase);
                    return emptyDatabase;
                }

                AppDatabase database = JsonConvert.DeserializeObject<AppDatabase>(json, serializerSettings);
                if (database == null)
                {
                    throw new InvalidDataException("The JSON database does not contain a valid object.");
                }

                Normalize(database);
                return database;
            }
            catch (JsonException exception)
            {
                throw new InvalidDataException("The JSON database is corrupt or has an invalid structure.", exception);
            }
            catch (UnauthorizedAccessException exception)
            {
                throw new IOException("The JSON database cannot be accessed.", exception);
            }
        }

        public void Save(string filePath, AppDatabase database)
        {
            if (database == null)
            {
                throw new ArgumentNullException("database");
            }

            string directory = Path.GetDirectoryName(Path.GetFullPath(filePath));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string temporaryPath = filePath + ".tmp";
            string backupPath = filePath + ".bak";
            string json = JsonConvert.SerializeObject(database, serializerSettings);

            try
            {
                File.WriteAllText(temporaryPath, json);

                if (File.Exists(filePath))
                {
                    File.Replace(temporaryPath, filePath, backupPath, true);
                }
                else
                {
                    File.Move(temporaryPath, filePath);
                }
            }
            finally
            {
                if (File.Exists(temporaryPath))
                {
                    File.Delete(temporaryPath);
                }
            }
        }

        private static void Normalize(AppDatabase database)
        {
            if (database.RootGroups == null)
            {
                database.RootGroups = new List<LauncherGroup>();
            }

            foreach (LauncherGroup group in database.RootGroups)
            {
                NormalizeGroup(group);
            }
        }

        private static void NormalizeGroup(LauncherGroup group)
        {
            if (group.Id == Guid.Empty)
            {
                group.Id = Guid.NewGuid();
            }

            group.Name = string.IsNullOrWhiteSpace(group.Name) ? "Unnamed Group" : group.Name.Trim();
            group.ChildGroups = group.ChildGroups ?? new List<LauncherGroup>();
            group.Items = group.Items ?? new List<LauncherItem>();

            foreach (LauncherItem item in group.Items)
            {
                if (item.Id == Guid.Empty)
                {
                    item.Id = Guid.NewGuid();
                }

                item.Name = string.IsNullOrWhiteSpace(item.Name) ? "Unnamed Launcher" : item.Name.Trim();
                item.Actions = (item.Actions ?? new List<LaunchAction>())
                    .OrderBy(action => action.Order)
                    .ToList();

                for (int index = 0; index < item.Actions.Count; index++)
                {
                    LaunchAction action = item.Actions[index];
                    if (action.Id == Guid.Empty)
                    {
                        action.Id = Guid.NewGuid();
                    }

                    action.DelayAfterMs = Math.Max(0, action.DelayAfterMs);
                    action.Order = index + 1;
                }
            }

            foreach (LauncherGroup childGroup in group.ChildGroups)
            {
                NormalizeGroup(childGroup);
            }
        }
    }
}
