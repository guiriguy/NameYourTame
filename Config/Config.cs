using BepInEx;
using BepInEx.Configuration;
using BepInEx.Bootstrap;
using HarmonyLib;
using UnityEngine;

namespace NameYourTame.Config
{
    [BepInPlugin("NameYourTame.Config", "NameYourTame", "0.1.0")]
    public class NameYourTame_Config : BaseUnityPlugin
    {
        public static NameYourTame_Config Instance { get; private set; }

        public void Awake()
        {
            Debug.Log("Loading NameYourTame...");

            NameYourTame_Config.Instance = this;

            //1. Enablers
            //Taming
            EnableMod = base.Config.Bind<bool>("1. Enablers", "Enable the Mod", true, "Enables or disables the Mod");
            //AllTamingCompatibility
            EnableHasToCrouch = base.Config.Bind<bool>("1. Enablers", "Enable Has to Crouch to Change the Name Mod", true, "Enables or disables the Need to Crouch to Change the Name");
            //Keybind
            ChangeName = base.Config.Bind<KeyCode>("2. KeyBind", "Keybind To Change Tame Name", KeyCode.N, "Change the button to name your tame. Keys: https://docs.unity3d.com/Manual/class-InputManager.html");

            //--
            Debug.Log("Name Your Tame Patched!");
            harmonyNameYourTame = new Harmony("NameYourTame.Config.GuiriGuyMods");
            harmonyNameYourTame.PatchAll();

            //Logs
            if (!EnableMod.Value)
                Debug.LogWarning("[NameYourTame] Mod Disabled");
            else
            {
                Debug.Log("[NameYourTame] Mod Enabled");
                if (!EnableHasToCrouch.Value)
                {
                    Debug.LogWarning("[NameYourTame] Has to Crouch Disabled");
                }
                else
                    Debug.Log("[NameYourTame] Has to Crouch Enabled");
            }

            Debug.Log("NameYourTame Loaded!");

            foreach (var plugin in Chainloader.PluginInfos)
            {
                var metadata = plugin.Value.Metadata;
                if (metadata.GUID.Contains("MoreSkills"))
                {
                    foundMS = true;
                    Debug.LogError("Found MoreSkills Mod, This Mod will be Deactivated.");
                    break;
                }
                else
                {
                    foundMS = false;
                }
            }
        }
        private void OnDestroy()
        {
            Debug.Log("NameYourTame UnPatched!");
            harmonyNameYourTame.UnpatchSelf();
        }

        private Harmony harmonyNameYourTame;

        //Skill Increases Multpliers

        //Enables

        public static ConfigEntry<bool> EnableMod;

        public static ConfigEntry<bool> EnableHasToCrouch;

        public static ConfigEntry<KeyCode> ChangeName;

        public static bool foundMS;

    }
}

