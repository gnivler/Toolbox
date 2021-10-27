using System.Collections.Generic;
using HarmonyLib;

namespace Toolbox.Features
{
    public static class ExtraRecipes
    {
        private static bool initialized;
        private static readonly List<CraftingRecipe> GoodQuality = new();
        private static readonly List<CraftingRecipe> ExcellentQuality = new();

        [HarmonyPatch(typeof(CraftingManager), "LoadAllRecipes")]
        [HarmonyPostfix]
        public static void CraftingManagerLoadAllRecipesPostfix()
        {
            if (initialized
                || !Mod.AddRecipes.Value)
            {
                return;
            }

            initialized = true;
            GoodQuality.Add(Recipes.GoodPlasticRecipe());
            GoodQuality.Add(Recipes.GoodRubberRecipe());
            GoodQuality.Add(Recipes.GoodGlassRecipe());
            ExcellentQuality.Add(Recipes.ExcellentPlasticRecipe());
            ExcellentQuality.Add(Recipes.ExcellentRubberRecipe());
            ExcellentQuality.Add(Recipes.ExcellentGlassRecipe());
            foreach (var recipe in GoodQuality)
            {
                CraftingManager.instance.m_recipes.Add(new CraftRecipeInstance(recipe));
            }

            foreach (var recipe in ExcellentQuality)
            {
                CraftingManager.instance.m_recipes.Add(new CraftRecipeInstance(recipe));
            }
        }

        [HarmonyPatch(typeof(CraftingManager), "GetCraftQuality", typeof(MemberReferenceHolder), typeof(CraftingRecipe))]
        public static void Postfix(MemberReferenceHolder memberRH, CraftingRecipe def, ref int __result)
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
