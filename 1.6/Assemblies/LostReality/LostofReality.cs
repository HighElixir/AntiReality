using System.Collections.Generic;
using Verse;

namespace AntiReality
{
    public class AddPenalty
    {
        public HediffDef hediffDef;
        public float severity;
        // すでにhediffが存在した場合の追加重症度。初期値の場合はseverityの値が追加される
        public float addSeverityIfExist = float.NaN;
        public float chance;
        public int intervalTick;
    }
    public class HediffCompProperties_LostofReality : HediffCompProperties_SeverityPerDay
    {
        public List<AddPenalty> penaltyHediffs;

        public HediffCompProperties_LostofReality() => compClass = typeof(LostofReality);
    }

    public class LostofReality : HediffComp_SeverityPerDay
    {
        private HediffCompProperties_LostofReality Props => (HediffCompProperties_LostofReality)base.props;

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            foreach (var penalty in Props.penaltyHediffs)
            {
                if (penalty?.hediffDef == null)
                    continue;

                if (parent.pawn.IsHashIntervalTick(penalty.intervalTick) && Rand.Chance(penalty.chance))
                {
                    var hediff = parent.pawn.health.hediffSet.GetFirstHediffOfDef(penalty.hediffDef);
                    if (hediff != null)
                    {
                        float addPenalty = float.IsNaN(penalty.addSeverityIfExist) ? penalty.severity : penalty.addSeverityIfExist;
                        hediff.Severity += addPenalty;
                    }
                    else
                    {
                        Hediff h = HediffMaker.MakeHediff(penalty.hediffDef, parent.pawn);
                        h.Severity = penalty.severity;
                        parent.pawn.health.AddHediff(h);
                    }
                }
            }
        }
    }
}
