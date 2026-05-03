using AsusFanControl.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AsusFanControl.Services
{
    public class ProfileManager
    {
        private readonly string _profilesDir;

        public ProfileManager()
        {
            _profilesDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AsusFanControl",
                "profiles"
            );

            if (!Directory.Exists(_profilesDir))
                Directory.CreateDirectory(_profilesDir);
        }

        public string ProfilesDirectory => _profilesDir;

        /// <summary>
        /// Loads all profiles from disk. Creates default profiles if none exist.
        /// </summary>
        public List<FanProfile> LoadAll()
        {
            var profiles = new List<FanProfile>();

            if (!Directory.Exists(_profilesDir))
                Directory.CreateDirectory(_profilesDir);

            var files = Directory.GetFiles(_profilesDir, "*.json");

            foreach (var file in files)
            {
                try
                {
                    var json = File.ReadAllText(file);
                    var profile = JsonConvert.DeserializeObject<FanProfile>(json);
                    if (profile != null)
                        profiles.Add(profile);
                }
                catch
                {
                    // Skip corrupted profile files
                }
            }

            // Create default profiles if none exist
            if (profiles.Count == 0)
            {
                var defaults = new[]
                {
                    FanProfile.CreateDefault(),
                    FanProfile.CreateSilent(),
                    FanProfile.CreatePerformance()
                };

                foreach (var profile in defaults)
                {
                    Save(profile);
                    profiles.Add(profile);
                }
            }

            return profiles.OrderBy(p => p.Name).ToList();
        }

        /// <summary>
        /// Saves a profile to disk as a JSON file.
        /// </summary>
        public void Save(FanProfile profile)
        {
            if (string.IsNullOrWhiteSpace(profile.Name))
                throw new ArgumentException("Profile name cannot be empty.");

            var fileName = SanitizeFileName(profile.Name) + ".json";
            var filePath = Path.Combine(_profilesDir, fileName);

            var json = JsonConvert.SerializeObject(profile, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Deletes a profile from disk.
        /// </summary>
        public bool Delete(string profileName)
        {
            var fileName = SanitizeFileName(profileName) + ".json";
            var filePath = Path.Combine(_profilesDir, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Renames a profile (deletes old file, saves with new name).
        /// </summary>
        public void Rename(FanProfile profile, string newName)
        {
            var oldName = profile.Name;
            Delete(oldName);
            profile.Name = newName;
            Save(profile);
        }

        /// <summary>
        /// Loads a specific profile by name.
        /// </summary>
        public FanProfile Load(string profileName)
        {
            var fileName = SanitizeFileName(profileName) + ".json";
            var filePath = Path.Combine(_profilesDir, fileName);

            if (!File.Exists(filePath))
                return null;

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<FanProfile>(json);
        }

        private string SanitizeFileName(string name)
        {
            var invalid = Path.GetInvalidFileNameChars();
            return new string(name.Select(c => invalid.Contains(c) ? '_' : c).ToArray());
        }
    }
}
