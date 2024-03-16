using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using EggInfo.config;

namespace EggInfo.Loader
{
    public class RequirementsLoader
    {
        private readonly string _pluginDirectory;

        public RequirementsLoader()
        {
            _pluginDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public Dictionary<string, EggInfoData> LoadEggInfo(string fileName)
        {
            try
            {
                string filePath = Path.Combine(_pluginDirectory, fileName);
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Dictionary<string, EggInfoData>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading {fileName}: {ex.Message}");
                return null;
            }
        }
    }

    public class EggInfoData
    {
        public string Tooltip { get; set; }
        public string FriendlyName { get; set; }
        public string InternalName { get; set; }
        public string TechType { get; set; }
    }
}
