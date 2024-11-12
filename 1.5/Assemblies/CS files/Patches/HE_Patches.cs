using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using static Verse.PawnCapacityUtility;

namespace HE_AntiReality
{

    /// <summary>
    /// Note:今後過負荷を利用したものに変更する
    /// </summary>
    [StaticConstructorOnStartup]
    public static class HE_Patches
    {
        private static readonly Type patchType = typeof(HE_Patches);

        static HE_Patches()
        {
            try
            {
                // Harmonyのインスタンスを作成
                Harmony harmony = new Harmony("HE_AntiReality");

                harmony.Patch(original: AccessTools.Method(typeof(PawnCapacityUtility), nameof(CalculateCapacityLevel)),
                    postfix: new HarmonyMethod(patchType, nameof(Postfix)));

                harmony.Patch(original: AccessTools.Method(typeof(Pawn_HealthTracker), "PreApplyDamage"),
                    prefix: new HarmonyMethod(patchType, nameof(PreApplyDamage_Prefix)));
            }
            catch (Exception ex)
            {
                Log.Error($"HE__ Failed to create Harmony instance: {ex}");
            }
        }

        // キャパシティ補正
        private static readonly float[] FixedCapacity = { 1f, 1.5f, 2f, 2.5f, 5f };

        public static void Postfix(ref float __result, HediffSet diffSet, PawnCapacityDef capacity, List<CapacityImpactor> impactors = null, bool forTradePrice = false)
        {
            if (diffSet.pawn.health.hediffSet.HasHediff(HE_HediffDefOf.AR_InfinityAnchor))
            {
                Hediff firstHediffOfDef = diffSet.pawn.health.hediffSet.GetFirstHediffOfDef(HE_HediffDefOf.AR_InfinityAnchor, false);
                if (firstHediffOfDef != null)
                {
                    int level = CheckSeverarity(firstHediffOfDef.Severity);
                    if (level == 4)
                    {
                        if (__result < FixedCapacity[4]) { __result = FixedCapacity[4]; }
                        return;
                    }
                    if (__result != FixedCapacity[level])
                    {
                        __result = FixedCapacity[level];
                    }
                }
            }
        }

        private static int CheckSeverarity(float sev)
        {
            if (sev < 0.01f)
            {
                return 0;
            }
            else if (sev < 0.05f)
            {
                return 1;
            }
            else if (sev < 0.1f)
            {
                return 2;
            }
            else if (sev < 0.5f)
            {
                return 3;
            }
            return 4;
        }

        //ダメージ吸収
        //ｲﾝﾌｨﾆﾃｨｱﾝｶｰの重症度が0.5以上の時、ダメージを無効化し、重症度を0.01減らす
        public static bool PreApplyDamage_Prefix(DamageInfo dinfo, ref bool absorbed, Pawn_HealthTracker __instance)
        {
            absorbed = false;
            Pawn pawn = Traverse.Create(root: __instance).Field(name: "pawn").GetValue<Pawn>();
            if (pawn == null || !(pawn.Faction?.IsPlayer ?? true)) return true;
            Hediff anchor = __instance.hediffSet.GetFirstHediffOfDef(HE_HediffDefOf.AR_InfinityAnchor);
            if (anchor != null && anchor.Severity > 0.5f)
            {
                anchor.Severity -= 0.01f;
                MoteMaker.ThrowText(new Vector3((float)pawn.Position.x + 1f, pawn.Position.y, (float)pawn.Position.z + 1f), pawn.Map, "ダメージ無効".Translate(), new Color(0.25f, 0.12f, 0.38f, 1f));
                absorbed = true;
                return false;
            }
            return true;
        }
    }
}