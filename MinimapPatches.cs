using HarmonyLib;
using UnityEngine;

namespace ClockMod
{
    [HarmonyPatch(typeof(Minimap))]
    public static class MinimapPatches
    {
        private static int _cachedDay = -1;
        private static int _cachedHours = -1;
        private static int _cachedMinutes = -1;
        private static TextLayoutStyle _cachedLayout = TextLayoutStyle.SingleLine;

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void Start_Postfix(Minimap __instance)
        {
            if (__instance.m_smallRoot == null) return;
            ClockUI.Build(__instance.m_smallRoot.transform);
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void Update_Postfix(Minimap __instance)
        {
            if (ClockUI.ClockText == null || EnvMan.instance == null || ZNet.instance == null) return;

            var smallMapActive = __instance.m_smallRoot.activeSelf;
            ClockUI.Panel.SetActive(smallMapActive);

            if (!smallMapActive) return;

            int day = EnvMan.instance.GetDay(ZNet.instance.GetTimeSeconds());
            float fraction = EnvMan.instance.GetDayFraction();

            int hours = Mathf.FloorToInt(fraction * 24f);
            int minutes = Mathf.FloorToInt((fraction * 24f - hours) * 60f);
            var layout = ClockPlugin.ConfigLayoutStyle.Value;

            if (day == _cachedDay && hours == _cachedHours && minutes == _cachedMinutes && layout == _cachedLayout)
            {
                return;
            }

            _cachedDay = day;
            _cachedHours = hours;
            _cachedMinutes = minutes;
            _cachedLayout = layout;

            var separator = layout == TextLayoutStyle.Stacked ? "\n" : "  ";
            ClockUI.ClockText.text = $"Day {day}{separator}{hours:D2}:{minutes:D2}";
        }
    }
}