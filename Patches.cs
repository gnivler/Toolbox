using HarmonyLib;
using Toolbox.Features;

namespace Toolbox
{
    public static class Patches
    {
        [HarmonyPatch(typeof(OptionsPanel), "OnQuitToMainMenuConfirm")]
        [HarmonyPostfix]
        public static void OptionsPanelOnQuitToMainMenuConfirmPostfix()
        {
            ExtraRecipes.initialized = false;
        }

        [HarmonyPatch(typeof(SaveManager), "LoadFromCurrentSlot")]
        [HarmonyPostfix]
        public static void SaveManagerLoadFromCurrentSlotPostfix()
        {
            Mod.Log("Load game");
            Mod.Log(new string('=', 80));
            ExtraRecipes.initialized = false;
        }
    }
}
