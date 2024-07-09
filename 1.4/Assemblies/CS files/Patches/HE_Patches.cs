using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using HarmonyLib;
using HarmonyMod;
using RimWorld;
using Verse;
using static Verse.PawnCapacityUtility;

namespace HE_AntiReality
{

    [StaticConstructorOnStartup]
    public static class HE_Patches
    {
        private static readonly Type patchType = typeof(HE_Patches);

        static HE_Patches()
        {

            // Harmonyのインスタンスを作成
            Harmony harmony = new Harmony("HE_AntiReality");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            harmony.Patch(original: AccessTools.Method(type: typeof(PawnCapacityUtility), name: "CalculateCapacityLevel"),
                postfix: new HarmonyMethod(patchType, nameof(Postfix)));

            harmony.Patch(original: AccessTools.Method(type: typeof(Pawn_HealthTracker), name: "PreApplyDamage"),
                    prefix: new HarmonyMethod(patchType, nameof(PreApplyDamage_Prefix)));
        }

        // キャパシティ補正
        private static readonly float[] FixedCapacity = { 1f, 1.5f, 2f, 2.5f, 5f };
        private static readonly Dictionary<float, int> SeverityLevelToIndex = new Dictionary<float, int>
        {
            { 0.01f, 0 },
            { 0.05f, 1 },
            { 0.1f, 2 },
            { 0.5f, 3 }
        };

        public static void Postfix(ref float __result, HediffSet diffSet, PawnCapacityDef capacity, List<CapacityImpactor> impactors = null, bool forTradePrice = false)
        {
            if (diffSet.pawn.health.hediffSet.HasHediff(HE_HediffDefOf.AR_InfinityAnchor))
            {
                Hediff firstHediffOfDef = diffSet.pawn.health.hediffSet.GetFirstHediffOfDef(HE_HediffDefOf.AR_InfinityAnchor, false);
                if (firstHediffOfDef != null)
                {
                    float severityLevel = firstHediffOfDef.Severity;
                    int level = SeverityLevelToIndex.TryGetValue(severityLevel, out int index) ? index : 4;

                    if (level < FixedCapacity.Length && __result != FixedCapacity[level])
                    {
                        __result = FixedCapacity[level];
                    }
                    else if (level == 4 && __result < FixedCapacity[level])
                    {
                        __result = FixedCapacity[level];
                    }
                }
            }
        }

        //ダメージ吸収
        public static bool PreApplyDamage_Prefix(DamageInfo dinfo, ref bool absorbed, Pawn_HealthTracker __instance)
        {
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