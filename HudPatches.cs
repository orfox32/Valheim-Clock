using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace ClockMod
{
    [HarmonyPatch(typeof(Hud), "UpdateStatusEffects", new Type[] { typeof(List<StatusEffect>) })]
    public static class HudPatches
    {
        [HarmonyAfter("randyknapp.mods.minimalstatuseffects")]
        [HarmonyPostfix]
        public static void Postfix(RectTransform ___m_statusEffectListRoot)
        {
            if (!ClockPlugin.HasMSE || ___m_statusEffectListRoot == null) return;

            float shiftAmount = ClockPlugin.ConfigHeight.Value + 15f;

            ___m_statusEffectListRoot.anchoredPosition = new Vector2(
                ___m_statusEffectListRoot.anchoredPosition.x,
                ___m_statusEffectListRoot.anchoredPosition.y - shiftAmount
            );
        }
    }
}