using System.Collections.Generic;
using HarmonyLib;

namespace Toolbox.Features
{
    public static class ExtraRecipes
    {
        internal static bool Initialized;
        private static readonly List<CraftingRecipe> GoodQuality = new();
        private static readonly List<CraftingRecipe> ExcellentQuality = new();

        [HarmonyPatch(typeof(CraftingManager), "LoadAllRecipes")]
        [HarmonyPostfix]
        public static void CraftingManagerLoadAllRecipes()
        {
            if (Initialized || !Mod.AddRecipes.Value)
            {
                return;
            }

            Initialized = true;
            if (Mod.AddRecipesPlastic.Value)
            {
                GoodQuality.Add(Recipes.GoodPlasticRecipe());
                ExcellentQuality.Add(Recipes.ExcellentPlasticRecipe());
            }

            if (Mod.AddRecipesRubber.Value)
            {
                GoodQuality.Add(Recipes.GoodRubberRecipe());
                ExcellentQuality.Add(Recipes.ExcellentRubberRecipe());
            }

            if (Mod.AddRecipesGlass.Value)
            {
                GoodQuality.Add(Recipes.GoodGlassRecipe());
                ExcellentQuality.Add(Recipes.ExcellentGlassRecipe());
            }

            if (Mod.AddRecipesWood.Value)
            {
                GoodQuality.Add(Recipes.GoodWoodRecipe());
                ExcellentQuality.Add(Recipes.ExcellentWoodRecipe());
            }

            if (Mod.AddRecipesSilicon.Value)
            {
                GoodQuality.Add(Recipes.GoodSiliconRecipe());
                ExcellentQuality.Add(Recipes.ExcellentSiliconRecipe());
            }

            foreach (var recipe in GoodQuality)
            {
                CraftingManager.instance.m_recipes.Add(new CraftRecipeInstance(recipe));
            }

            foreach (var recipe in ExcellentQuality)
            {
                CraftingManager.instance.m_recipes.Add(new CraftRecipeInstance(recipe));
            }

            Mod.Log("Added recipes");
        }

        [HarmonyPatch(typeof(CraftingManager), "GetCraftQuality", typeof(MemberReferenceHolder), typeof(CraftingRecipe))]
        [HarmonyPostfix]
        public static void CraftingManagerGetCraftQuality(MemberReferenceHolder memberRH, CraftingRecipe def, ref int __result)
        {
            if (!Mod.AddRecipes.Value)
            {
                return;
            }

            if (GoodQuality.Contains(def))
            {
                __result = 1;
                return;
            }

            if (ExcellentQuality.Contains(def))
            {
                __result = 2;
            }
        }
    }
}
