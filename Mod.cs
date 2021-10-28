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
        private const string PluginVersion = "1.0.0";
        private static string LogFile;
        private static bool dev;

        internal static ConfigEntry<int> GoodRubberInput;
        internal static ConfigEntry<int> ExcellentRubberInput;
        internal static ConfigEntry<int> GoodPlasticInput;
        internal static ConfigEntry<int> ExcellentPlasticInput;
        internal static ConfigEntry<int> GoodGlassInput;
        internal static ConfigEntry<int> ExcellentGlassInput;
        private static ConfigEntry<KeyboardShortcut> WasteHotkey;
        internal static ConfigEntry<bool> AddRecipes;
        internal static ConfigEntry<bool> ChainBuild;
        internal static ConfigEntry<bool> Banners;
        internal static bool WasteEnabled = true;

        private void Awake()
        {
            WasteHotkey = Config.Bind("Hotkey", "Waste Quality Materials", new KeyboardShortcut(KeyCode.F2), "Toggle allowed wasting of high level materials on lower level crafting");
            AddRecipes = Config.Bind("Toggle", "Add recipes (restart required)", true, "Add recipes to refine higher quality plastic, glass and rubber (restart required)");
            ChainBuild = Config.Bind("Toggle", "Chain Building", true, "Lets you hold CTRL while building objects to continue building more");
            Banners = Config.Bind("Toggle", "Banner Messages", true, "Speeds up the messages across the top of the screen");

            if (AddRecipes.Value)
            {
                GoodPlasticInput = Config.Bind("Adjustments", "Good Plastic Input", 20, new ConfigDescription("Amount for making Good quality", new AcceptableValueRange<int>(10, 100)));
                ExcellentPlasticInput = Config.Bind("Adjustments", "Excellent Plastic Input", 10, new ConfigDescription("Amount for making Excellent quality", new AcceptableValueRange<int>(10, 100)));
                GoodRubberInput = Config.Bind("Adjustments", "Good Rubber Input", 20, new ConfigDescription("Amount for making Good quality", new AcceptableValueRange<int>(10, 100)));
                ExcellentRubberInput = Config.Bind("Adjustments", "Excellent Rubber Input", 10, new ConfigDescription("Amount for making Excellent quality", new AcceptableValueRange<int>(10, 100)));
                GoodGlassInput = Config.Bind("Adjustments", "Good Glass Input", 20, new ConfigDescription("Amount for making Good quality", new AcceptableValueRange<int>(10, 100)));
                ExcellentGlassInput = Config.Bind("Adjustments", "Excellent Glass Input", 10, new ConfigDescription("Amount for making Excellent quality", new AcceptableValueRange<int>(10, 100)));
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
                        DoThings(craftingPanel);
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
                        DoThings(craftingPanel);
                    }

                    if (Time.timeScale == 0)
                    {
                        Destroy(floatie);
                    }

                    return;
                }

                void DoThings(CraftingPanel craftingPanel)
                {
                    var last = craftingPanel.m_recipeGrid.m_lastHovered;
                    craftingPanel.DisplayRecipes(craftingPanel.m_selectedLevel);
                    craftingPanel.m_recipeGrid.m_lastHovered = last;
                    last.ToggleLastSelected(true);
                    craftingPanel.m_recipeGrid.m_itemButtonOnHover(last, craftingPanel.m_recipeGrid, true);
                }
            }

            if (!dev)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.F6))
            {
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
