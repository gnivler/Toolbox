using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

// ReSharper disable Global InconsistentNaming

namespace Toolbox.Features
{
    public static class QualityWaste
    {
        // don't use better ingredients than needed
        [HarmonyPatch(typeof(Inventory), "GetItemsOfLowestToGreatestQuality")]
        [HarmonyPostfix]
        public static void InventoryGetItemsOfLowestToGreatestQuality(ref List<ItemStack> __result, ItemStack.Quality quality)
        {
            if (Mod.WasteEnabled)
            {
                return;
            }

            Mod.Log("wtaf");
            Mod.Log(__result.Count);
            __result.Do(r => Mod.Log($"{r.def.name} {r.amount} {r.quality}"));
            __result = __result.Where(i => i.quality <= quality).ToList();
            //Mod.Log("GetItemsOfLowestToGreatestQuality " + __result);
        }

        [HarmonyPatch(typeof(Inventory), "GetItemCount")]
        [HarmonyPostfix]
        public static void InventoryGetItemCount(Dictionary<string, List<ItemStack>> ___m_inventory, ref int __result, string itemKey, ItemStack.Quality quality)
        {
            if (Mod.WasteEnabled)
            {
                return;
            }

            __result = 0;
            if (___m_inventory.ContainsKey(itemKey))
            {
                for (var i = 0; i < ___m_inventory[itemKey].Count; i++)
                {
                    if (___m_inventory[itemKey][i] is not null
                        && (___m_inventory[itemKey][i].quality == quality || quality == ItemStack.Quality.Any))
                    {
                        __result += ___m_inventory[itemKey][i].amount;
                    }
                }

                //Mod.Log("GetItemCount " + __result);
            }
        }

        //[HarmonyPatch(typeof(ShelterInventoryManager), "GetLowestToHighestQuality")]
        //[HarmonyPostfix]
        //public static void ShelterInventoryManagerGetLowestToHighestQuality(ref List<ItemStack> __result, ItemStack.Quality quality)
        //{
        //    if (Mod.WasteEnabled)
        //    {
        //        return;
        //    }
        //
        //    __result = __result.Where(i => i.quality <= quality).ToList();
        //    //Mod.Log("GetLowestToHighestQuality " + __result);
        //}
    }
}
