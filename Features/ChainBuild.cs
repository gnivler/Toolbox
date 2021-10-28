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
        public static void CraftingManagerStartCraft(CraftRecipeInstance recipe)
        {
            if (!Mod.ChainBuild.Value)
            {
                return;
            }

            Mod.Log("StartCraft " + recipe.def);
            recipeInstance = recipe;
        }

        [HarmonyPatch(typeof(InteractionManager), "PlaceCraftingObject")]
        [HarmonyPostfix]
        public static void InteractionManagerPlaceCraftingObject()
        {
            if (!Mod.ChainBuild.Value)
            {
                return;
            }

            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                && recipeInstance is not null
                && !placementObject.CollidingDuringPlacement
                && ShelterInventoryManager.instance.ContainsItems(recipeInstance.ingredients))
            {
                Mod.Log(recipeInstance?.def);
                CraftingManager.instance.StartCraft(recipeInstance, InteractionManager.instance.SelectedMember.member);
            }
        }

        [HarmonyPatch(typeof(InteractionManager), "PlaceRoom")]
        [HarmonyPostfix]
        public static void InteractionManagerPlaceRoom()
        {
            if (!Mod.ChainBuild.Value)
            {
                return;
            }

            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                && recipeInstance is not null
                && ShelterInventoryManager.instance.ContainsItems(recipeInstance.ingredients))
            {
                Mod.Log(recipeInstance?.def);
                CraftingManager.instance.StartCraft(recipeInstance, InteractionManager.instance.SelectedMember.member);
            }
        }

        
        
        [HarmonyPatch(typeof(InteractionManager), "StartCraftingPlacement", typeof(CraftRecipeInstance), typeof(List<ItemStack>))]
        [HarmonyPostfix]
        public static void InteractionManagerStartCraftingPlacement()
        {
            if (!Mod.ChainBuild.Value)
            {
                return;
            }

            placementObject = InteractionManager.instance.PlacementObject;
        }
    }
}
