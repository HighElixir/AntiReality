using RimWorld;
using System.Collections.Generic;
using Verse;

namespace HE_AntiReality
{
    public class HediffSets
    {
        public List<HediffDef> hediffDefs;
    }
    public class HediffCompProperties_NeedHediff : HediffCompProperties
    {
        public List<HediffSets> needHediffs;
        public string mode = string.Empty;
        public HediffCompProperties_NeedHediff()
        {
            compClass = typeof(NeedHediff);
        }
    }

    public class NeedHediff : HediffComp
    {
        public override bool CompShouldRemove => !CheckPawnHasHediff();
        public HediffCompProperties_NeedHediff Props => (HediffCompProperties_NeedHediff)props;

        // Multiフラグが有効の場合、親hediffが1.0以上なら無条件でtrueを返す
        private bool Multi()
        {
            return parent.Severity >= 1.0;
        }

        private bool CheckPawnHasHediff()
        {
            if (Props.mode == "Multi" && Multi())
                return true;
            if (Props.needHediffs == null || Props.needHediffs.Count == 0)
                return false;

            foreach (HediffSets needhediff in Props.needHediffs)
            {
                if (CheckHavingHediffs(needhediff))
                {
                    return true;
                }
            }
            return false;
        }
        private bool CheckHavingHediffs(HediffSets hediffSet)
        {
            foreach (var h in hediffSet.hediffDefs)
            {
                if (!Pawn.health.hediffSet.HasHediff(h))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

