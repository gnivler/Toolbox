using System.Collections.Generic;

namespace Toolbox
{
    public static class Helper
    {
        internal static void RefreshGridAfterChangingItemCounts(CraftingPanel craftingPanel)
        {
            var last = craftingPanel.m_recipeGrid.m_lastHovered;
            craftingPanel.DisplayRecipes(craftingPanel.m_selectedLevel);
            craftingPanel.m_recipeGrid.m_lastHovered = last;
            last.ToggleLastSelected(true);
            craftingPanel.m_recipeGrid.m_itemButtonOnHover(last, craftingPanel.m_recipeGrid, true);
        }

        internal static void RevealFogOfWar()
        {
            for (var x = 0; x < MapGenerationManager.instance.mapSize.x; x++)
            {
                for (var y = 0; y < MapGenerationManager.instance.mapSize.y; y++)
                {
                    var mapTile = MapGenerationManager.instance.map[x, y];
                    mapTile.SetDiscovered(discovered: true);
                    if (MapManager.instance.volumetricFog != null
                        && MapManager.instance.volumetricFog.gameObject.activeInHierarchy)
                    {
                        MapManager.instance.volumetricFog.SetFogOfWarAlpha(mapTile.transform.position, 16f, 0f, 0f, 0.5f);
                    }
                }
            }

            // block the four corners to make the circumnavigate achievement worthwhile
            var left = MapGenerationManager.instance.map[0, 0];
            var bottom = MapGenerationManager.instance.map[MapGenerationManager.instance.map.GetLength(0) - 1, 0];
            var right = MapGenerationManager.instance.map[MapGenerationManager.instance.map.GetLength(0) - 1, MapGenerationManager.instance.map.GetLength(1) - 1];
            var top = MapGenerationManager.instance.map[MapGenerationManager.instance.map.GetLength(0) - 1, 0];
            var tiles = new List<MapTile>
            {
                left,
                bottom,
                right,
                top
            };

            foreach (var tile in tiles)
            {
                MapManager.instance.volumetricFog.SetFogOfWarAlpha(tile.transform.position, 16f, 0f, 0f, 0.5f);
            }

            MapManager.instance.volumetricFog.UpdateFogOfWar();
        }
    }
}
