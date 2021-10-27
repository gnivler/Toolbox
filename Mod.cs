using System;
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
        internal static bool WasteEnabled = true;

        private void Awake()
        {
            WasteHotkey = Config.Bind("Hotkey", "Waste Quality Materials", new KeyboardShortcut(KeyCode.F2), "Toggle allowed wasting of high level materials on lower level crafting");
            AddRecipes = Config.Bind("Toggle", "Add recipes (restart required)", true, "Add recipes to refine higher quality plastic, glass and rubber (restart required)");
            ChainBuild = Config.Bind("Toggle", "Chain Building", true, "Lets you hold CTRL while building objects to continue building more");

            if (AddRecipes.Value)
            {
                GoodPlasticInput = Config.Bind("Adjustments", "Good Plastic Input", 25, new ConfigDescription("Amount for making Good quality", new AcceptableValueRange<int>(10, 200)));
                ExcellentPlasticInput = Config.Bind("Adjustments", "Excellent Plastic Input", 25, new ConfigDescription("Amount for making Excellent quality", new AcceptableValueRange<int>(10, 200)));
                GoodRubberInput = Config.Bind("Adjustments", "Good Rubber Input", 25, new ConfigDescription("Amount for making Good quality", new AcceptableValueRange<int>(10, 200)));
                ExcellentRubberInput = Config.Bind("Adjustments", "Excellent Rubber Input", 25, new ConfigDescription("Amount for making Excellent quality", new AcceptableValueRange<int>(10, 200)));
                GoodGlassInput = Config.Bind("Adjustments", "Good Glass Input", 25, new ConfigDescription("Amount for making Good quality", new AcceptableValueRange<int>(10, 200)));
                ExcellentGlassInput = Config.Bind("Adjustments", "Excellent Glass Input", 25, new ConfigDescription("Amount for making Excellent quality", new AcceptableValueRange<int>(10, 200)));
            }

            Harmony harmony = new("ca.gnivler.sheltered2.Toolbox");
            LogFile = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName!, "log.txt");
            dev = SystemInfo.deviceName == "MEOWMEOW";
            Log("Toolbox Startup");
            harmony.PatchAll(typeof(Patches));
            harmony.PatchAll(typeof(ChainBuild));
            harmony.PatchAll(typeof(ExtraRecipes));
            harmony.PatchAll(typeof(Fading));
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
                floatie.transform.SetParent(Fading.LogPanel.transform.parent);
                floatie.transform.position = new Vector3(x - scrollRt.rect.x / 2, y, 0);
                if (WasteEnabled)
                {
                    WasteEnabled = false;
                    text.SetText("NOT WASTING QUALITY MATERIALS");
                    return;
                }

                WasteEnabled = true;
                text.SetText($"CAN WASTE QUALITY MATERIALS");
                return;
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
