using System.Collections.Generic;
using Verse;

namespace HE_AntiReality
{
    
    public class HediffComp_FixedPawnCapacity : HediffComp
    {
        private float[] fixedCapacity = { 1f, 1.5f, 2f, 2.5f, 5f };
        private int level = 4;

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            Pawn p = Pawn;
            Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(HE_HediffDefOf.AR_InfinityAnchor, false);
            if (firstHediffOfDef != null)
            {
                float severarytyLevel = firstHediffOfDef.Severity;

                switch (severarytyLevel)
                {
                    case 0.01f:
                        level = 0; break;
                    case 0.05f:
                        level = 1; break;
                    case 0.1f:
                        level = 2; break;
                    case 0.5f:
                        level = 3; break;
                    default: break;
                }
            }
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            Pawn p = Pawn;
            if (!p.health.hediffSet.HasHediff(HE_HediffDefOf.AR_InfinityAnchor, false)) { return; }
            List<PawnCapacityDef> capacityDefs = DefDatabase<PawnCapacityDef>.AllDefsListForReading;
            if (p != null) 
            {
                for (int i = 0; i < capacityDefs.Count; i++)
                {
                    if (level != 4 && p.health.capacities.GetLevel(capacityDefs[i]) != fixedCapacity[level])
                    {

                    }else if (level == 4 && p.health.capacities.GetLevel(capacityDefs[i]) < fixedCapacity[level])
                    {

                    }
                }
                
            }
        }


    }
}
