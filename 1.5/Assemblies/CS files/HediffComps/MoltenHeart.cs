using RimWorld;
using UnityEngine;
using Verse;

namespace HE_AntiReality
{
    public class HediffCompProperties_MoltenHeart : HediffCompProperties
    {
        public int finishDay = 60;
        public HediffCompProperties_MoltenHeart() => compClass = typeof(HediffComp_MoltenHeart);
    }

    public class HediffComp_MoltenHeart : HediffComp
    {
        private Faction _originalFaction;

        private HediffCompProperties_MoltenHeart Props => (HediffCompProperties_MoltenHeart)props;

        public Faction OriginalFaction => _originalFaction;
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostMake();
            Pawn p = parent.pawn;
            _originalFaction = p.Faction;
            if (p.Faction != null && p.Faction.IsPlayer)
            {
                parent.Severity = parent.def.maxSeverity;
            }
            else
            {
                p.SetFaction(Faction.OfPlayer);
            }
        }
        public override string CompTipStringExtra
        {
            get
            {
                int finishTick = AR_Constants.OneDayTickCount * Props.finishDay;
                float sevPerTick = parent.def.maxSeverity / finishTick;
                int day = AR_Constants.OneDayTickCount;

                return AR_Constants.Tooltip_MoltenHeart_Daytofinish.Translate(Mathf.CeilToInt(Props.finishDay - (parent.Severity / sevPerTick) / day));
            }
        }
        public override void CompPostTick(ref float severityAdjustment)
        {
            if (severityAdjustment >= parent.def.maxSeverity) return;

            int finishTick = AR_Constants.OneDayTickCount * Props.finishDay;
            float max = parent.def.maxSeverity;
            float adjustment = max / finishTick;
            if (parent.Severity + adjustment > max)
            {
                severityAdjustment = parent.def.maxSeverity;
            }
            else
            {
                severityAdjustment += adjustment;
            }
        }

        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            if (parent.Severity >= parent.def.maxSeverity && _originalFaction != parent.pawn.Faction)
            {
                parent.pawn.SetFaction(_originalFaction);
            }
        }
        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_References.Look(ref _originalFaction, ExposeUtility.GetExposeKey(typeof(HediffComp_MoltenHeart), nameof(_originalFaction)));
        }
    }
}
