using RimWorld;
using Verse;
using HarmonyLib;
using System.Reflection;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HE_AntiReality
{


    [StaticConstructorOnStartup]
    public static class HarmonyPatches_AbsorbedDamage
    {
        static HarmonyPatches_AbsorbedDamage()
        {
            // Harmonyのインスタンスを作成
            Harmony harmony = new Harmony("HE_AntiReality");
            harmony.PatchAll();
        }

        // ダメージが適用される前に呼び出されるメソッド
        [HarmonyPatch(typeof(Pawn_HealthTracker), nameof(Pawn_HealthTracker.PreApplyDamage))]
        [HarmonyPrefix]
        public static bool PreApplyDamage_Prefix(DamageInfo dinfo, ref bool absorbed, Pawn_HealthTracker __instance)
        {
            absorbed = false;
            Pawn pawn = Traverse.Create(__instance).Field(name: "pawn").GetValue<Pawn>();
            if (pawn == null || !(pawn.Faction?.IsPlayer ?? true)) return true;

            if (pawn != null)
            {
                if (pawn.health.hediffSet.HasHediff(HE_HediffDefOf.AR_InfinityAnchor))
                {
                    Console.WriteLine("Absorbed Sucsess");
                    absorbed = true;
                    return false;
                }
            }
            return true;
        }
    }

}