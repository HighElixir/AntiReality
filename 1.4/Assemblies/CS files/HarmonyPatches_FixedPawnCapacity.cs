using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;
using static Verse.PawnCapacityUtility;

namespace HE_AntiReality
{
    

    [HarmonyPatch(typeof(PawnCapacityUtility), nameof(CalculateCapacityLevel))]
    public static class Patch_PawnCapacityUtility_CalculateCapacityLevel
    {
        static Patch_PawnCapacityUtility_CalculateCapacityLevel()
        {
            // Harmonyのインスタンスを作成
            Harmony harmony = new Harmony("HE_AntiReality");
            harmony.PatchAll();
        }

        [HarmonyPostfix]
        public static void Postfix(ref float __result, HediffSet diffSet, PawnCapacityDef capacity, List<CapacityImpactor> impactors = null, bool forTradePrice = false)
        {
            if (diffSet.pawn.health.hediffSet.HasHediff(HE_HediffDefOf.AR_InfinityAnchor))
            {
                Hediff firstHediffOfDef = diffSet.pawn.health.hediffSet.GetFirstHediffOfDef(HE_HediffDefOf.AR_InfinityAnchor, false);
                if (firstHediffOfDef != null)
                {
                    float severityLevel = firstHediffOfDef.Severity;
                    float[] fixedCapacity = { 1f, 1.5f, 2f, 2.5f, 5f };
                    int level;

                    switch (severityLevel)
                    {
                        case 0.01f:
                            level = 0; break;
                        case 0.05f:
                            level = 1; break;
                        case 0.1f:
                            level = 2; break;
                        case 0.5f:
                            level = 3; break;
                        default:
                            level = 4; break;
                    }

                    if (level != 4 && __result != fixedCapacity[level])
                    {
                        __result = fixedCapacity[level];
                    }
                    else if (level == 4 && __result < fixedCapacity[level])
                    {
                        __result = fixedCapacity[level];
                    }
                }
            }
        }
    }
}