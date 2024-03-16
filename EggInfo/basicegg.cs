using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;
using EggInfo.Loader;
using EggInfo.info;
using EggInfo.config;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using System.Diagnostics.CodeAnalysis;
using Nautilus.Crafting;
using Nautilus.Handlers;
using Newtonsoft.Json;
using static HandReticle;

namespace EggInfo.info
{
    public class BasicEggPrefab : CustomPrefab
    {
        [SetsRequiredMembers]
        public BasicEggPrefab(string internalName, string displayName, string tooltip, TechType techType ,Atlas.Sprite sprite)
            : base(internalName, displayName, tooltip, SpriteManager.Get(techType))
        {
            this.SetPdaGroupCategory(TechGroup.Resources, TechCategory.BasicMaterials);
            this.SetUnlock(techType);
            this.Register();
        }
    }

    public class QPatch : MonoBehaviour
    {
        private void Awake()
        {
            LoadEggRequirements();
            CreateCustomGroups();
        }

        private void LoadEggRequirements()
        {
            RequirementsLoader requirementsLoader = new RequirementsLoader();
            Dictionary<string, EggInfoData> eggInfoData = requirementsLoader.LoadEggInfo("egg_requirements.json");

            if (eggInfoData != null)
            {
                foreach (var kvp in eggInfoData)
                {
                    string eggName = kvp.Key;
                    EggInfoData eggData = kvp.Value;

                    // Create custom egg prefab
                    TechType techType = GetTechType(eggData.TechType);
                    new BasicEggPrefab(eggData.InternalName, eggData.FriendlyName, eggData.Tooltip, techType,SpriteManager.Get(techType));
                }
            }
            else
            {
                Debug.LogError("Failed to load egg requirements from JSON.");
            }
        }

        private TechType GetTechType(string techTypeStr)
        {
            if (Enum.TryParse(techTypeStr, out TechType result))
            {
                return result;
            }
            return TechType.None;
        }

        private void CreateCustomGroups()
        {
            // Create or get the tech group
            var groupName = "EggInfo";
            TechGroup group;
            if (!EnumHandler.TryGetValue(groupName, out group))
            {
                group = EnumHandler.AddEntry<TechGroup>(groupName).WithPdaInfo($"EggInfo");
            }

            // Create custom tech categories
            CreateCustomCategories(group);
        }

        private void CreateCustomCategories(TechGroup group)
        {
            RequirementsLoader requirementsLoader = new RequirementsLoader();
            Dictionary<string, EggInfoData> eggInfoData = requirementsLoader.LoadEggInfo("egg_requirements.json");

            if (eggInfoData != null)
            {
                foreach (var kvp in eggInfoData)
                {
                    EggInfoData eggData = kvp.Value;

                    // Generate a descriptive category name
                    string categoryName = $"Egg_{eggData.InternalName}";

                    // Check if the category already exists
                    if (!EnumHandler.TryGetValue<TechCategory>(categoryName, out var category))
                    {
                        // Create and register the category
                        category = EnumHandler.AddEntry<TechCategory>(categoryName).WithPdaInfo($"{eggData.FriendlyName} Egg").RegisterToTechGroup(group);
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to load egg requirements from JSON.");
            }
        }
    }
}
