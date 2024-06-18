using System;
using Verse;
using RimWorld;

namespace HE_AntiReality
{
    public class ThoughtWorker_AddBodyParts_AntiReal_Hand : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.health.hediffSet.HasHediff(HE_HediffDefOf.Non_Existent_Hands, false))
            {
                return ThoughtState.Inactive;
            }
            return ThoughtState.ActiveAtStage(0);
        }
    }
}
