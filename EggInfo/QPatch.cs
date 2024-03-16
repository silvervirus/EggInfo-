using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using EggInfo.Loader;
using EggInfo.info;
using Debug = UnityEngine.Debug;

[BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus", BepInDependency.DependencyFlags.HardDependency)]
public class QPatch : BaseUnityPlugin
{
    public const string PLUGIN_GUID = "SN_EggInfo";
    public const string PLUGIN_NAME = "EggInfo";
    public const string PLUGIN_VERSION = "1.0.0";
   

    void Awake()
    {
        LoadEggRequirements();
    }

    private void LoadEggRequirements()
    {
        RequirementsLoader requirementsLoader = new RequirementsLoader();
        Dictionary<string, EggInfoData> eggInfoData = requirementsLoader.LoadEggInfo("egg_requirements.json");

        if (eggInfoData != null)
        {
            foreach (var eggEntry in eggInfoData)
            {
                try
                {
                    string eggName = eggEntry.Key;
                    EggInfoData eggData = eggEntry.Value;

                    // Extract egg information from eggData
                    string eggDisplayName = eggData.FriendlyName;
                    string eggDescription = eggData.Tooltip;
                    string eggInterName= eggData.InternalName;
                    // Convert eggData.TechType to TechType to get the sprite
                    TechType eggTechType = GetTechType(eggData.TechType);

                    // Create and register the custom egg prefab for each TechType
                    foreach (string internalName in eggData.InternalName.Split(','))
                    {
                        // Create and register the custom egg prefab
                        new BasicEggPrefab(eggInterName.Trim(), eggDisplayName, eggDescription, eggTechType,SpriteManager.Get(eggTechType));
                        
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error creating Custom Egg {eggEntry.Key}: {ex.Message}");
                }
            }
        }
        else
        {
            Debug.LogError("Failed to load egg requirements from JSON.");
        }
    }
    Atlas.Sprite GetSpriteFromTechType(TechType techType)
    {
        return SpriteManager.Get(techType);
    }
    private TechType GetTechType(string techTypeStr)
    {
        if (Enum.TryParse(techTypeStr, out TechType result))
        {
            return result;
        }
        // Handle error case, maybe return a default TechType
        return TechType.None;
    }
}
