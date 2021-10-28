using HarmonyLib;

// ReSharper disable InconsistentNaming

namespace Toolbox.Features
{
    public static class FluorescentFlicker
    {
        [HarmonyPatch(typeof(Object_Light), "Flicker")]
        [HarmonyPrefix]
        public static bool Object_LightFlicker(ref bool __runOriginal)
        {
            if (!Mod.Flicker.Value)
            {
                return true;
            }

            __runOriginal = false;
            return false;
        }
    }
}
