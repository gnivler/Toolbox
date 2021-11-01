using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using TMPro;
using Toolbox.Features;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Toolbox
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Mod : BaseUnityPlugin
    {
        private const string PluginGUID = "ca.gnivler.sheltered2.Toolbox";
        private const string PluginName = "Toolbox";
        private const string PluginVersion = "1.1.0";
        private static string LogFile;
        internal static bool dev;

        internal static ConfigEntry<int> GoodRubberInput;
        internal static ConfigEntry<int> ExcellentRubberInput;
        internal static ConfigEntry<int> GoodPlasticInput;
        internal static ConfigEntry<int> ExcellentPlasticInput;
        internal static ConfigEntry<int> GoodGlassInput;
        internal static ConfigEntry<int> ExcellentGlassInput;
        internal static ConfigEntry<int> GoodWoodInput;
        internal static ConfigEntry<int> ExcellentWoodInput;
        internal static ConfigEntry<int> GoodSiliconInput;
        internal static ConfigEntry<int> ExcellentSiliconInput;
        internal static ConfigEntry<bool> AddRecipes;
        internal static ConfigEntry<bool> ChainBuild;
        internal static ConfigEntry<bool> Banners;
        internal static ConfigEntry<bool> Flicker;
        internal static ConfigEntry<bool> MapSize;
        internal static ConfigEntry<bool> AddRecipesPlastic;
        internal static ConfigEntry<bool> AddRecipesRubber;
        internal static ConfigEntry<bool> AddRecipesGlass;
        internal static ConfigEntry<bool> AddRecipesWood;
        internal static ConfigEntry<bool> AddRecipesSilicon;
        private static ConfigEntry<KeyboardShortcut> CheatHotkey;
        private static ConfigEntry<KeyboardShortcut> WasteHotkey;
        private static ConfigEntry<KeyboardShortcut> FogHotkey;
        internal static bool WasteEnabled = true;

        private void Awake()
        {
            WasteHotkey = Config.Bind("Hotkey", "Waste Quality Materials", new KeyboardShortcut(KeyCode.F2), "Toggle allowed wasting of high level materials on lower level crafting");
            FogHotkey = Config.Bind("Hotkey", "Reveal Fog of War", new KeyboardShortcut(KeyCode.F3), "Must have cheat mode enabled");
            CheatHotkey = Config.Bind("Hotkey", "Toggle Cheating", new KeyboardShortcut(KeyCode.F6), "Build anything for free");
            AddRecipes = Config.Bind("Toggle", "Add recipes (restart required)", true, "Add recipes to refine higher quality resources (restart required)");
            ChainBuild = Config.Bind("Toggle", "Chain Building", true, "Lets you hold CTRL while building objects to continue building more");
            Banners = Config.Bind("Toggle", "Banner Messages", true, "Speeds up the messages across the top of the screen");
            Flicker = Config.Bind("Toggle", "Fluorescent Flickering", true, "Disables the flickering (just couldn't handle it)");
            MapSize = Config.Bind("Toggle", "Big Map (NEW GAME AND RESTART required)", false, "!!!EXPERIMENTAL!!!");
            AddRecipesPlastic = Config.Bind("Recipes To Add", "Plastic", true, "Add Plastic Recipes");
            AddRecipesRubber = Config.Bind("Recipes To Add", "Rubber", true, "Add Rubber Recipes");
            AddRecipesGlass = Config.Bind("Recipes To Add", "Glass", true, "Add Glass Recipes");
            AddRecipesWood = Config.Bind("Recipes To Add", "Wood", true, "Add Wood Recipes");
            AddRecipesSilicon = Config.Bind("Recipes To Add", "Silicon", true, "Add Silicon Recipes");

            if (AddRecipes.Value)
            {
                GoodPlasticInput = Config.Bind("Adjustments", "Good Plastic Input", 10, new ConfigDescription("Amount for making Good quality", new AcceptableValueRange<int>(1, 50)));
                ExcellentPlasticInput = Config.Bind("Adjustments", "Excellent Plastic Input", 10, new ConfigDescription("Amount for making Excellent quality", new AcceptableValueRange<int>(1, 50)));
                GoodRubberInput = Config.Bind("Adjustments", "Good Rubber Input", 10, new ConfigDescription("Amount for making Good quality", new AcceptableValueRange<int>(1, 50)));
                ExcellentRubberInput = Config.Bind("Adjustments", "Excellent Rubber Input", 10, new ConfigDescription("Amount for making Excellent quality", new AcceptableValueRange<int>(1, 50)));
                GoodGlassInput = Config.Bind("Adjustments", "Good Glass Input", 10, new ConfigDescription("Amount for making Good quality", new AcceptableValueRange<int>(1, 50)));
                ExcellentGlassInput = Config.Bind("Adjustments", "Excellent Glass Input", 10, new ConfigDescription("Amount for making Excellent quality", new AcceptableValueRange<int>(1, 50)));
                GoodWoodInput = Config.Bind("Adjustments", "Good Wood Input", 10, new ConfigDescription("Amount for making Good quality", new AcceptableValueRange<int>(1, 50)));
                ExcellentWoodInput = Config.Bind("Adjustments", "Excellent Wood Input", 10, new ConfigDescription("Amount for making Excellent quality", new AcceptableValueRange<int>(1, 50)));
                GoodSiliconInput = Config.Bind("Adjustments", "Good Silicon Input", 10, new ConfigDescription("Amount for making Good quality", new AcceptableValueRange<int>(1, 50)));
                ExcellentSiliconInput = Config.Bind("Adjustments", "Excellent Silicon Input", 10, new ConfigDescription("Amount for making Excellent quality", new AcceptableValueRange<int>(1, 50)));
            }

            Harmony harmony = new("ca.gnivler.sheltered2.Toolbox");
            LogFile = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName!, "log.txt");
            dev = SystemInfo.deviceName == "MEOWMEOW";
            Log("Toolbox Startup");
            Log(new string('=', 80));
            harmony.PatchAll(typeof(Patches));
            harmony.PatchAll(typeof(ChainBuild));
            harmony.PatchAll(typeof(ExtraRecipes));
            harmony.PatchAll(typeof(BannerMessages));
            harmony.PatchAll(typeof(QualityWaste));
            harmony.PatchAll(typeof(FluorescentFlicker));
            //harmony.PatchAll(typeof(Fixes));
            harmony.PatchAll(typeof(Cheats));
            harmony.PatchAll(typeof(MapSize));
        }

        private void Update()
        {
            if (Input.GetKeyDown(WasteHotkey.Value.MainKey)
                && WasteHotkey.Value.Modifiers.All(Input.GetKey))
            {
                var x = Screen.width / 2;
                var y = Screen.height / 2;
                var floatie = new GameObject("WasteFloatie");
                var text = floatie.AddComponent<TextMeshProUGUI>();
                var scrollRt = floatie.GetComponentInChildren<RectTransform>();
                scrollRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
                floatie.AddComponent<FloatieBehaviour>();
                floatie.AddComponent<FadeText>();
                floatie.transform.SetParent(BannerMessages.LogPanel.transform.parent);
                floatie.transform.position = new Vector3(x - scrollRt.rect.x / 2, y, 0);
                if (WasteEnabled)
                {
                    WasteEnabled = false;
                    text.SetText("NOT WASTING QUALITY MATERIALS");
                    if (PanelManager.instance.GetTopPanel() is CraftingPanel craftingPanel)
                    {
                        Helper.RefreshGridAfterChangingItemCounts(craftingPanel);
                    }

                    if (Time.timeScale == 0)
                    {
                        // fare thee well, cpu cycles
                        Destroy(floatie);
                    }

                    return;
                }

                {
                    WasteEnabled = true;
                    text.SetText("CAN WASTE QUALITY MATERIALS");
                    if (PanelManager.instance.GetTopPanel() is CraftingPanel craftingPanel)
                    {
                        Helper.RefreshGridAfterChangingItemCounts(craftingPanel);
                    }

                    if (Time.timeScale == 0)
                    {
                        Destroy(floatie);
                    }

                    return;
                }
            }

            if (Input.GetKeyDown(CheatHotkey.Value.MainKey)
                && CheatHotkey.Value.Modifiers.All(Input.GetKey))
            {
                Cheats.Cheat = !Cheats.Cheat;
                var x = Screen.width / 2;
                var y = Screen.height / 2;
                var floatie = new GameObject("CheatsFloatie");
                var text = floatie.AddComponent<TextMeshProUGUI>();
                var scrollRt = floatie.GetComponentInChildren<RectTransform>();
                scrollRt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
                floatie.AddComponent<FloatieBehaviour>();
                floatie.AddComponent<FadeText>();
                floatie.transform.SetParent(BannerMessages.LogPanel.transform.parent);
                floatie.transform.position = new Vector3(x - scrollRt.rect.x / 2, y, 0);
                text.SetText("Cheats Enabled: " + Cheats.Cheat);
                if (Time.timeScale == 0)
                {
                    // fare thee well, cpu cycles
                    Destroy(floatie);
                }

                return;
            }

            if (Input.GetKeyDown(FogHotkey.Value.MainKey)
                && CheatHotkey.Value.Modifiers.All(Input.GetKey))
            {
                if (Cheats.Cheat)
                {
                    Helper.RevealFogOfWar();
                }
            }

            if (!dev)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.F7))
            {
            }
        }

        public static void Log(object input)
        {
            if (dev)
            {
                File.AppendAllText(LogFile, $"{input ?? "null"}\n");
            }
        }
    }
}
