using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Emit;
using FMODUnity;
using HarmonyLib;
using MapHelper;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo

namespace Toolbox.Features
{
    public static class MapSize
    {
        private const int MapSizeFactor = 2;

        // more regions (or they are huge)
        [HarmonyPatch(typeof(MapGenerationManager), "GenerateNewDataMap")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> MapGenerationManagerGenerateNewDataMap(IEnumerable<CodeInstruction> instructions)
        {
            if (!Mod.MapSize.Value)
            {
                return instructions;
            }

            return new CodeMatcher(instructions)
                .MatchForward(false,
                    new CodeMatch(OpCodes.Ldc_I4_S),
                    new CodeMatch(OpCodes.Call),
                    new CodeMatch(OpCodes.Ldarg_0))
                .SetOperandAndAdvance(36 * MapSizeFactor)
                .InstructionEnumeration();
        }

        // overkill, just more iterations "in case?"
        [HarmonyPatch(typeof(MapGenerationManager), "GenerateResidentialAreas")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> MapGenerationManagerGenerateResidentialAreas(IEnumerable<CodeInstruction> instructions)
        {
            if (!Mod.MapSize.Value)
            {
                return instructions;
            }

            return new CodeMatcher(instructions)
                .MatchForward(false,
                    new CodeMatch(OpCodes.Ldc_I4_1),
                    new CodeMatch(OpCodes.Add),
                    new CodeMatch(OpCodes.Stloc_S))
                .SetOperandAndAdvance(1000 * MapSizeFactor)
                .InstructionEnumeration();
        }

        // main configuration
        [HarmonyPatch(typeof(MapGenerationManager), "GenerateMap")]
        [HarmonyPrefix]
        public static void MapGenerationManagerGenerateMapPrefix(ref Vector2Int ___m_mapSize)
        {
            if (!Mod.MapSize.Value)
            {
                return;
            }

            // force it to the centre of the map via PlaceShelter()
            MapGenerationManager.instance.m_shelterIndex.x = -1;
            ___m_mapSize.x *= MapSizeFactor;
            ___m_mapSize.y *= MapSizeFactor;
            MapGenerationManager.instance.m_forestBiome.count.min *= MapSizeFactor;
            MapGenerationManager.instance.m_forestBiome.count.max *= MapSizeFactor;
            MapGenerationManager.instance.m_forestBiome.size.max /= MapSizeFactor * MapSizeFactor;
            MapGenerationManager.instance.m_forestBiome.distanceFromShelter /= MapSizeFactor;
            MapGenerationManager.instance.m_snowBiome.size.min *= MapSizeFactor * MapSizeFactor;
            MapGenerationManager.instance.m_snowBiome.size.max *= MapSizeFactor * MapSizeFactor;
            MapGenerationManager.instance.m_snowBiome.mountainCount =
                new Boundsi(MapGenerationManager.instance.m_snowBiome.mountainCount.min, MapGenerationManager.instance.m_snowBiome.mountainCount.max * MapSizeFactor);
            MapGenerationManager.instance.m_desertBiome.size.min *= MapSizeFactor * MapSizeFactor;
            MapGenerationManager.instance.m_desertBiome.size.max *= MapSizeFactor * MapSizeFactor;
            MapGenerationManager.instance.m_desertBiome.mountainCount =
                new Boundsi(MapGenerationManager.instance.m_desertBiome.mountainCount.min, MapGenerationManager.instance.m_desertBiome.mountainCount.max * MapSizeFactor);
            MapGenerationManager.instance.m_lakeCount *= MapSizeFactor;
            MapGenerationManager.instance.m_minimumRiverTileCount *= MapSizeFactor;
            foreach (var setting in MapGenerationManager.instance.m_residentialSettings)
            {
                setting.amount *= MapSizeFactor * MapSizeFactor;
                setting.tileCount.max *= MapSizeFactor * MapSizeFactor;
                setting.shelterDistance = new Boundsi(4, 10_000);
            }

            foreach (var setting in MapGenerationManager.instance.m_poiSpawnSettings)
            {
                setting.count = new Boundsi(setting.count.min, setting.count.max * MapSizeFactor * MapSizeFactor);
                // departure from vanilla which buffers them a bit
                setting.distanceFromSameType = 0;
                setting.distanceFromPOIs = 0;
                setting.distanceFromShelter = new Boundsi(4, 10_000);
            }

            for (var index = 0; index < MapGenerationManager.instance.m_poiBundleSettings.Count; index++)
            {
                var setting = MapGenerationManager.instance.m_poiBundleSettings[index];
                setting.mainCount = new Boundsi(setting.mainCount.min, setting.mainCount.max * MapSizeFactor * MapSizeFactor);
            }

            for (var index = 0; index < MapGenerationManager.instance.m_specialPOISettings.Count; index++)
            {
                var setting = MapGenerationManager.instance.m_specialPOISettings[index];
                setting.distanceFromShelter = new Boundsi(4, 10_000);
            }

            for (var index = 0; index < MapGenerationManager.instance.m_postReleasePOISpawnSettings.Count; index++)
            {
                var setting = MapGenerationManager.instance.m_postReleasePOISpawnSettings[index];
                setting.distanceFromShelter = new Boundsi(4, 10_000);
                setting.count = new Boundsi(setting.count.min, setting.count.max * MapSizeFactor * MapSizeFactor);
            }
        }

        // unlock camera a bit
        [HarmonyPatch(typeof(MapCamera), "Start")]
        [HarmonyPostfix]
        public static void MapCameraStart(MapCamera __instance, ref Boundsf ___m_zoomBounds)
        {
            if (!Mod.dev)
            {
                return;
            }

            ___m_zoomBounds.max *= 10;
            __instance.cam.farClipPlane *= 10;
        }
    }
}
