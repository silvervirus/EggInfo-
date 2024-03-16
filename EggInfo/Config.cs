using EggInfo.Loader;
using Nautilus.Json;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace EggInfo.config
{
    public class MyConfig : ConfigFile
    {
        // Define properties for storing tooltips for each egg
        public Dictionary<string, string> EggTooltips { get; set; } = new Dictionary<string, string>();

        // Define properties for storing required plants for each egg
        public Dictionary<string, List<string>> RequiredPlantsPerEgg { get; set; } = new Dictionary<string, List<string>>();

        // Singleton instance for easy access
        private static MyConfig instance;
        public static MyConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MyConfig();
                    instance.Load(); // Optionally load the configuration here
                }
                return instance;
            }
        }

        // Define a TryGet method to retrieve tooltip values
        public bool TryGet(string eggName, out string tooltip)
        {
            return EggTooltips.TryGetValue(eggName, out tooltip);
        }

        public static void PatchMethod()
        {
            Instance.Load();

            // Load egg tooltips and requirements from JSON file
            var requirementsLoader = new RequirementsLoader();
            var eggInfoData = requirementsLoader.LoadEggInfo("egg_requirements.json");

            if (eggInfoData == null)
            {
                Debug.LogError("Failed to load configuration data.");
                return;
            }

            // Assign loaded egg tooltips to the EggTooltips dictionary
            foreach (var pair in eggInfoData)
            {
                Instance.EggTooltips[pair.Key] = pair.Value.Tooltip;
            }

            // Print egg tooltips for debugging purposes
            foreach (var pair in Instance.EggTooltips)
            {
                string eggName = pair.Key;
                string eggTooltip = pair.Value;
                Debug.Log($"{eggName} tooltip: {eggTooltip}");
            }
        }
    }
}
