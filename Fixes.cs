using FMOD.Studio;
using FMODUnity;
using HarmonyLib;

namespace Toolbox
{
    public static class Fixes
    {
        [HarmonyPatch(typeof(TimeManager), "set_IsFastForwarding")]
        [HarmonyPrefix]
        public static bool TimeManagerIsFastForwarding(bool value, ref bool __runOriginal)
        {
            if (value)
            {
                var tm = TimeManager.instance;
                if (!tm.fastForward)
                {
                    FMODAudioManager.Instance.PlaySFX(tm.fastForwardStartSFX);
                    tm.m_fastForwardSFxEvent = RuntimeManager.CreateInstance(tm.m_fastForwardSFXRef);
                    tm.m_fastForwardSFxEvent.start();
                    tm.m_fastForwardSFxEvent.release();
                    FMODAudioManager.Instance.TriggerFFSnapshot(val: true);
                }
            }
            else if (TimeManager.instance.m_fastForwardSFxEvent.isValid())
            {
                FMODAudioManager.Instance.TriggerFFSnapshot(val: false);
                FMODAudioManager.Instance.PlaySFX(TimeManager.instance.fastforwardEndSFX);
                TimeManager.instance.m_fastForwardSFxEvent.stop(STOP_MODE.IMMEDIATE);
            }
            
            TimeManager.instance.fastForward = value;
            __runOriginal = false;
            return false;
        }
    }
}
