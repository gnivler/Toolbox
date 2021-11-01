using System.Linq;
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
            ExtraRecipes.Initialized = false;
        }

        [HarmonyPatch(typeof(SaveManager), "LoadFromCurrentSlot")]
        [HarmonyPostfix]
        public static void SaveManagerLoadFromCurrentSlotPostfix()
        {
            Mod.Log("Load game");
            Mod.Log(new string('=', 80));
            ExtraRecipes.Initialized = false;
        }

        [HarmonyPatch(typeof(CraftingPanel), "DisplayRecipeDetails")]
        [HarmonyPostfix]
        public static void CraftingPanelDisplayRecipeDetails(CraftingPanel __instance, CraftRecipeInstance craftingRecipe)
        {
            if (!string.IsNullOrEmpty(craftingRecipe.def.descriptionKey)
                && craftingRecipe.def.descriptionKey.Contains("Toolbox"))
            {
                var text = craftingRecipe.def.descriptionKey.Substring(8);
                __instance.m_itemDescriptionText.text = text;
            }
        }
    }
}
