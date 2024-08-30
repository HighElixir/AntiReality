using RimWorld;
using System.Collections.Generic;
using Verse;

namespace HE_AntiReality
{
    public class HediffComp_NeedHediff : HediffComp
    {

        public HediffCompProperties_NeedHediff Props
        {
            get
            {
                return (HediffCompProperties_NeedHediff)props;
            }
        }

        private bool Multi()
        {
            Hediff hediff = Pawn.health.hediffSet.GetFirstHediffOfDef(parent.def);
            if (hediff != null && hediff.Severity >= 1.0) { return true; }
            return false;
        }
        private bool ChackPawnHasHediff()
        {
            if (Props.mode != null)
            {
                if (Props.mode == "Multi" && Multi())
                {
                    return true;
                }

            }
            foreach (HediffDef needhediff in Props.needHediffs)
            {
                bool hasHediff = Pawn.health.hediffSet.HasHediff(needhediff);
                if (hasHediff)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool CompShouldRemove
        {
            get
            {

                return !ChackPawnHasHediff();
            }
        }
    }
}

