using System;
using Verse;
using RimWorld;

namespace HE_AntiReality
{
    public class ThoughtWorker_AddBodyParts_AntiReal_Hand : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(HE_HediffDefOf.Non_Existent_Hands, false);
            if (firstHediffOfDef == null || firstHediffOfDef.def.stages == null)
            {
                return ThoughtState.Inactive;
            }
            return ThoughtState.ActiveAtStage(0);
        }
    }
}
