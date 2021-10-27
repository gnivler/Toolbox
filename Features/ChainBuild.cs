using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace Toolbox.Features
{
    public static class ChainBuild
    {
        private static CraftRecipeInstance recipeInstance;
        private static Object_Base placementObject;

        [HarmonyPatch(typeof(CraftingManager), "StartCraft", typeof(CraftRecipeInstance), typeof(Member), typeof(Object_Base))]
        [HarmonyPostfix]
        public static void CraftingManagerStartCraftPostfix(CraftRecipeInstance recipe, bool __result)
        {
            if (!Mod.ChainBuild.Value)
            {
                return;
            }

            Mod.Log("Recipe captured");
            recipeInstance = recipe;
        }

        [HarmonyPatch(typeof(InteractionManager), "PlaceCraftingObject")]
        public static void Postfix(CraftRecipeInstance __state)
        {
            if (!Mod.ChainBuild.Value)
            {
                return;
            }

            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                && recipeInstance is not null
                && !placementObject.CollidingDuringPlacement)
            {
                Mod.Log(recipeInstance?.def);
                CraftingManager.instance.StartCraft(recipeInstance, InteractionManager.instance.SelectedMember.member);
            }
        }


        [HarmonyPatch(typeof(InteractionManager), "StartCraftingPlacement", typeof(CraftRecipeInstance), typeof(List<ItemStack>))]
        public static void Postfix()
        {
            if (!Mod.ChainBuild.Value)
            {
                return;
            }

            placementObject = InteractionManager.instance.PlacementObject;
        }
    }
}
