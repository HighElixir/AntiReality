using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace HE_AntiReality
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches_FixedPawnCapacity
    {
        static HarmonyPatches_FixedPawnCapacity()
        {
            var harmony = new Harmony("rimworld.HE.AntiReality");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(PawnCapacityUtility), "CalculateCapacityLevel")]
    public static class Patch_PawnCapacityUtility_CalculateCapacityLevel
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn pawn, PawnCapacityDef capacity, ref float __result)
        {
            if (pawn.health.hediffSet.HasHediff(HE_HediffDefOf.AR_InfinityAnchor))
            {
                Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HE_HediffDefOf.AR_InfinityAnchor, false);
                if (firstHediffOfDef != null)
                {
                    float severityLevel = firstHediffOfDef.Severity;
                    float[] fixedCapacity = { 1f, 1.5f, 2f, 2.5f, 5f };
                    int level = 4;

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