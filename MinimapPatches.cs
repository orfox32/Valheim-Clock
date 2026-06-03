using HarmonyLib;
using UnityEngine;

namespace ClockMod
{
    [HarmonyPatch(typeof(Minimap))]
    public static class MinimapPatches
    {
        private static int _cachedDisplayDay = -1;
        private static int _cachedHours = -1;
        private static int _cachedMinutes = -1;
        private static TextLayoutStyle _cachedLayout = TextLayoutStyle.SingleLine;
        private static bool _cached12HourFormat = false;

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

            double rawTime = ZNet.instance.GetTimeSeconds();
            long dayLengthSec = EnvMan.instance.m_dayLengthSec;

            int day = EnvMan.instance.GetDay(rawTime);
            float fraction = (float)(rawTime % (double)dayLengthSec) / (float)dayLengthSec;

            int hours = Mathf.FloorToInt(fraction * 24f);
            int minutes = Mathf.FloorToInt((fraction * 24f - hours) * 60f);

            var layout = ClockPlugin.ConfigLayoutStyle.Value;
            bool use12Hour = ClockPlugin.ConfigUse12HourFormat.Value;

            if (day == _cachedDisplayDay && hours == _cachedHours && minutes == _cachedMinutes &&
                layout == _cachedLayout && use12Hour == _cached12HourFormat)
            {
                return;
            }

            _cachedDisplayDay = day;
            _cachedHours = hours;
            _cachedMinutes = minutes;
            _cachedLayout = layout;
            _cached12HourFormat = use12Hour;

            var separator = layout == TextLayoutStyle.Stacked ? "\n" : "  ";

            if (use12Hour)
            {
                string amPm = hours >= 12 ? "PM" : "AM";
                int displayHours = hours % 12;
                if (displayHours == 0) displayHours = 12;

                ClockUI.ClockText.text = $"Day {day}{separator}{displayHours:D2}:{minutes:D2} {amPm}";
            }
            else
            {
                ClockUI.ClockText.text = $"Day {day}{separator}{hours:D2}:{minutes:D2}";
            }
        }
    }
}
