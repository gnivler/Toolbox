using HarmonyLib;

namespace Toolbox.Features
{
    public static class Fading
    {
        internal static ActivityLogPanel LogPanel = default;

        // reduce message fading time ActivityLogPanel
        [HarmonyPatch(typeof(ActivityLogPanel), "Awake")]
        [HarmonyPostfix]
        public static void ActivityLogPanelAwakePostfix(ActivityLogPanel __instance, ref float ___m_messageFadeInTime, ref float ___m_messageFadeOutTime, ref float ___m_messageDuration)
        {
            ___m_messageFadeInTime = 3;
            ___m_messageFadeOutTime = 3;
            ___m_messageDuration = 1.5f;
            LogPanel = __instance;
        }
    }
}
