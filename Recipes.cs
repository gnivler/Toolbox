using System.Collections.Generic;
using UnityEngine;

namespace Toolbox
{
    public static class Recipes
    {
        internal static CraftingRecipe GoodPlasticRecipe()
        {
            var recipe = ScriptableObject.CreateInstance<CraftingRecipe>();
            recipe.m_ingredients = new List<ItemStack> { new("Plastic", ItemStack.Quality.Poor, Mod.GoodPlasticInput.Value) };
            recipe.m_nameKey = "Good Plastic";
            recipe.m_outputItem = ItemManager.items["Plastic"];
            recipe.m_baseCraftTime = 5;
            recipe.name = "Good Plastic";
            recipe.ID = "Good Plastic";
            recipe.m_type = CraftingRecipe.OutputType.Item;
            recipe.m_randomIngredientsPool = new();
            recipe.m_factionSkillCheck = FactionAchievementManager.AchievementType.Null;
            recipe.m_descriptionKey = "Toolbox Good Plastic";
            recipe.m_recipeLevel = 1;
            recipe.m_outputCount = 1;
            recipe.m_recipeType = CraftingRecipe.RecipeType.BasicItems;
            recipe.m_objectSprite = ItemManager.items["Plastic"].sprite;
            recipe.m_unlockedByDefault = true;
            return recipe;
        }

        internal static CraftingRecipe ExcellentPlasticRecipe()
        {
            var recipe = ScriptableObject.CreateInstance<CraftingRecipe>();
            recipe.m_ingredients = new List<ItemStack> { new("Plastic", ItemStack.Quality.Good, Mod.ExcellentPlasticInput.Value) };
            recipe.m_nameKey = "Excellent Plastic";
            recipe.m_outputItem = ItemManager.items["Plastic"];
            recipe.m_baseCraftTime = 5;
            recipe.name = "Excellent Plastic";
            recipe.ID = "Excellent Plastic";
            recipe.m_type = CraftingRecipe.OutputType.Item;
            recipe.m_randomIngredientsPool = new();
            recipe.m_factionSkillCheck = FactionAchievementManager.AchievementType.Null;
            recipe.m_descriptionKey = "Toolbox Excellent Plastic";
            recipe.m_recipeLevel = 2;
            recipe.m_outputCount = 1;
            recipe.m_recipeType = CraftingRecipe.RecipeType.BasicItems;
            recipe.m_objectSprite = ItemManager.items["Plastic"].sprite;
            recipe.m_unlockedByDefault = true;
            return recipe;
        }

        internal static CraftingRecipe GoodRubberRecipe()
        {
            var recipe = ScriptableObject.CreateInstance<CraftingRecipe>();
            recipe.m_ingredients = new List<ItemStack> { new("Rubber", ItemStack.Quality.Poor, Mod.GoodRubberInput.Value) };
            recipe.m_nameKey = "Good Rubber";
            recipe.m_outputItem = ItemManager.items["Rubber"];
            recipe.m_baseCraftTime = 5;
            recipe.name = "Good Rubber";
            recipe.ID = "Good Rubber";
            recipe.m_type = CraftingRecipe.OutputType.Item;
            recipe.m_randomIngredientsPool = new();
            recipe.m_factionSkillCheck = FactionAchievementManager.AchievementType.Null;
            recipe.m_descriptionKey = "Toolbox Good Rubber";
            recipe.m_recipeLevel = 1;
            recipe.m_outputCount = 1;
            recipe.m_recipeType = CraftingRecipe.RecipeType.BasicItems;
            recipe.m_objectSprite = ItemManager.items["Rubber"].sprite;
            recipe.m_unlockedByDefault = true;
            return recipe;
        }

        internal static CraftingRecipe ExcellentRubberRecipe()
        {
            var recipe = ScriptableObject.CreateInstance<CraftingRecipe>();
            recipe.m_ingredients = new List<ItemStack> { new("Rubber", ItemStack.Quality.Good, Mod.ExcellentRubberInput.Value) };
            recipe.m_nameKey = "Excellent Rubber";
            recipe.m_outputItem = ItemManager.items["Rubber"];
            recipe.m_baseCraftTime = 5;
            recipe.name = "Excellent Rubber";
            recipe.ID = "Excellent Rubber";
            recipe.m_type = CraftingRecipe.OutputType.Item;
            recipe.m_randomIngredientsPool = new();
            recipe.m_factionSkillCheck = FactionAchievementManager.AchievementType.Null;
            recipe.m_descriptionKey = "Toolbox Good Rubber";
            recipe.m_recipeLevel = 2;
            recipe.m_outputCount = 1;
            recipe.m_recipeType = CraftingRecipe.RecipeType.BasicItems;
            recipe.m_objectSprite = ItemManager.items["Rubber"].sprite;
            recipe.m_unlockedByDefault = true;
            return recipe;
        }

        internal static CraftingRecipe GoodGlassRecipe()
        {
            var recipe = ScriptableObject.CreateInstance<CraftingRecipe>();
            recipe.m_ingredients = new List<ItemStack> { new("Glass", ItemStack.Quality.Poor, Mod.GoodGlassInput.Value) };
            recipe.m_nameKey = "Good Glass";
            recipe.m_outputItem = ItemManager.items["Glass"];
            recipe.m_baseCraftTime = 5;
            recipe.name = "Good Glass";
            recipe.ID = "Good Glass";
            recipe.m_type = CraftingRecipe.OutputType.Item;
            recipe.m_randomIngredientsPool = new();
            recipe.m_factionSkillCheck = FactionAchievementManager.AchievementType.Null;
            recipe.m_descriptionKey = "Toolbox Good Glass";
            recipe.m_recipeLevel = 1;
            recipe.m_outputCount = 1;
            recipe.m_recipeType = CraftingRecipe.RecipeType.BasicItems;
            recipe.m_objectSprite = ItemManager.items["Glass"].sprite;
            recipe.m_unlockedByDefault = true;
            return recipe;
        }

        internal static CraftingRecipe ExcellentGlassRecipe()
        {
            var recipe = ScriptableObject.CreateInstance<CraftingRecipe>();
            recipe.m_ingredients = new List<ItemStack> { new("Glass", ItemStack.Quality.Good, Mod.ExcellentGlassInput.Value) };
            recipe.m_nameKey = "Excellent Glass";
            recipe.m_outputItem = ItemManager.items["Glass"];
            recipe.m_baseCraftTime = 5;
            recipe.name = "Excellent Glass";
            recipe.ID = "Excellent Glass";
            recipe.m_type = CraftingRecipe.OutputType.Item;
            recipe.m_randomIngredientsPool = new();
            recipe.m_factionSkillCheck = FactionAchievementManager.AchievementType.Null;
            recipe.m_descriptionKey = "Toolbox Good Glass";
            recipe.m_recipeLevel = 2;
            recipe.m_outputCount = 1;
            recipe.m_recipeType = CraftingRecipe.RecipeType.BasicItems;
            recipe.m_objectSprite = ItemManager.items["Glass"].sprite;
            recipe.m_unlockedByDefault = true;
            return recipe;
        }
        internal static CraftingRecipe GoodWoodRecipe()
        {
            var recipe = ScriptableObject.CreateInstance<CraftingRecipe>();
            recipe.m_ingredients = new List<ItemStack> { new("Wood", ItemStack.Quality.Poor, Mod.GoodWoodInput.Value) };
            recipe.m_nameKey = "Good Wood";
            recipe.m_outputItem = ItemManager.items["Wood"];
            recipe.m_baseCraftTime = 5;
            recipe.name = "Good Wood";
            recipe.ID = "Good Wood";
            recipe.m_type = CraftingRecipe.OutputType.Item;
            recipe.m_randomIngredientsPool = new();
            recipe.m_factionSkillCheck = FactionAchievementManager.AchievementType.Null;
            recipe.m_descriptionKey = "Toolbox Good Wood";
            recipe.m_recipeLevel = 1;
            recipe.m_outputCount = 1;
            recipe.m_recipeType = CraftingRecipe.RecipeType.BasicItems;
            recipe.m_objectSprite = ItemManager.items["Wood"].sprite;
            recipe.m_unlockedByDefault = true;
            return recipe;
        }

        internal static CraftingRecipe ExcellentWoodRecipe()
        {
            var recipe = ScriptableObject.CreateInstance<CraftingRecipe>();
            recipe.m_ingredients = new List<ItemStack> { new("Wood", ItemStack.Quality.Good, Mod.ExcellentWoodInput.Value) };
            recipe.m_nameKey = "Excellent Wood";
            recipe.m_outputItem = ItemManager.items["Wood"];
            recipe.m_baseCraftTime = 5;
            recipe.name = "Excellent Wood";
            recipe.ID = "Excellent Wood";
            recipe.m_type = CraftingRecipe.OutputType.Item;
            recipe.m_randomIngredientsPool = new();
            recipe.m_factionSkillCheck = FactionAchievementManager.AchievementType.Null;
            recipe.m_descriptionKey = "Toolbox Good Wood";
            recipe.m_recipeLevel = 2;
            recipe.m_outputCount = 1;
            recipe.m_recipeType = CraftingRecipe.RecipeType.BasicItems;
            recipe.m_objectSprite = ItemManager.items["Wood"].sprite;
            recipe.m_unlockedByDefault = true;
            return recipe;
        }
        internal static CraftingRecipe GoodSiliconRecipe()
        {
            var recipe = ScriptableObject.CreateInstance<CraftingRecipe>();
            recipe.m_ingredients = new List<ItemStack> { new("Silicon", ItemStack.Quality.Poor, Mod.GoodSiliconInput.Value) };
            recipe.m_nameKey = "Good Silicon";
            recipe.m_outputItem = ItemManager.items["Silicon"];
            recipe.m_baseCraftTime = 5;
            recipe.name = "Good Silicon";
            recipe.ID = "Good Silicon";
            recipe.m_type = CraftingRecipe.OutputType.Item;
            recipe.m_randomIngredientsPool = new();
            recipe.m_factionSkillCheck = FactionAchievementManager.AchievementType.Null;
            recipe.m_descriptionKey = "Toolbox Good Silicon";
            recipe.m_recipeLevel = 1;
            recipe.m_outputCount = 1;
            recipe.m_recipeType = CraftingRecipe.RecipeType.BasicItems;
            recipe.m_objectSprite = ItemManager.items["Silicon"].sprite;
            recipe.m_unlockedByDefault = true;
            return recipe;
        }

        internal static CraftingRecipe ExcellentSiliconRecipe()
        {
            var recipe = ScriptableObject.CreateInstance<CraftingRecipe>();
            recipe.m_ingredients = new List<ItemStack> { new("Silicon", ItemStack.Quality.Good, Mod.ExcellentSiliconInput.Value) };
            recipe.m_nameKey = "Excellent Silicon";
            recipe.m_outputItem = ItemManager.items["Silicon"];
            recipe.m_baseCraftTime = 5;
            recipe.name = "Excellent Silicon";
            recipe.ID = "Excellent Silicon";
            recipe.m_type = CraftingRecipe.OutputType.Item;
            recipe.m_randomIngredientsPool = new();
            recipe.m_factionSkillCheck = FactionAchievementManager.AchievementType.Null;
            recipe.m_descriptionKey = "Toolbox Excellent Silicon";
            recipe.m_recipeLevel = 2;
            recipe.m_outputCount = 1;
            recipe.m_recipeType = CraftingRecipe.RecipeType.BasicItems;
            recipe.m_objectSprite = ItemManager.items["Silicon"].sprite;
            recipe.m_unlockedByDefault = true;
            return recipe;
        }
    }
}
