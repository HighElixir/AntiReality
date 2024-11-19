using Verse;
using RimWorld;

namespace HE_AntiReality
{
    public class ThoughtWorker_MoltenHeart : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p.health.hediffSet.TryGetHediff(AR_HediffDefOf.AR_MoltenHeart, out var h))
            {
                if (h.TryGetComp<HediffComp_MoltenHeart>(out var c) && c.OriginalFaction.IsPlayer)
                {
                    return ThoughtState.ActiveAtStage(4);
                }
                return ThoughtState.ActiveAtStage(h.CurStageIndex);
            }

            return ThoughtState.Inactive;
        }
    }
}
