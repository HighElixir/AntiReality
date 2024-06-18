using RimWorld;
using Verse;
using HarmonyLib;
using System.Reflection;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace HE_AntiReality
{
    //ダメージを吸収するパッチ(未完成)

    [StaticConstructorOnStartup]
    public static class HarmonyPatches_AbsorbedDamage
    {
        private static readonly Type patchType = typeof(HarmonyPatches_AbsorbedDamage);
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
            Log.Message("hello");
            absorbed = false;
            Pawn pawn = Traverse.Create(root: __instance).Field(name: "pawn").GetValue<Pawn>();
            if (pawn == null || !(pawn.Faction?.IsPlayer ?? true)) return true;

            if (pawn != null && pawn.health.hediffSet.HasHediff(HE_HediffDefOf.AR_InfinityAnchor))
            {

                Log.Message("Absorbed Sucsess");
                absorbed = true;
                return false;
            }
            return true;
        }
    }

}