using LudeonTK;
using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace HE_AntiReality
{
    public class HediffCompProperties_AddOverLoad : HediffCompProperties
    {
        public float addLimitAmount = 0;
        public float addLimitFactor = 0;
        public float healAmount = 0;
        public float healFactor = 0;

        public HediffCompProperties_AddOverLoad() => compClass = typeof(HediffComp_AddOverLoad);
    }

    public class HediffComp_AddOverLoad : HediffComp
    {
        private HediffComp_OverLoad overLoad;
        public HediffCompProperties_AddOverLoad Props => (HediffCompProperties_AddOverLoad)props;

        public override void CompPostPostAdd(DamageInfo? _)
        {
            var hd = Pawn.health.hediffSet;
            if (!(hd.TryGetHediff(AR_HediffDefOf.AR_OverLoad, out var h) && h.TryGetComp(out overLoad)))
            {
                var newHediff = new Hediff()
                {
                    def = AR_HediffDefOf.AR_OverLoad,
                    Severity = AR_HediffDefOf.AR_OverLoad.initialSeverity
                };
                hd.AddDirect(newHediff);
                newHediff.TryGetComp(out overLoad);
            }

            if (overLoad == null)
            {
                Debug.LogError("Don's assigned 'HediffCompProperties_OverLoad' in AR_OverLoad");
            }
            else
            {
                overLoad.AddLimit(Props.addLimitAmount, Props.addLimitFactor);
                overLoad.AddHeal(Props.healAmount, Props.healFactor);
            }
        }
        public override void CompPostPostRemoved()
        {
            base.CompPostPostRemoved();
            overLoad.SubLimit(Props.addLimitAmount, Props.addLimitFactor);
            overLoad.SubHeal(Props.healAmount, Props.healFactor);
        }

        public override void CompExposeData()
        {
            Type t = typeof(HediffComp_AddOverLoad);
            base.CompExposeData();
            if (overLoad != null)
            {
                Scribe_References.Look(ref overLoad, ExposeUtility.GetExposeKey(t, nameof(overLoad)));
            }
        }
    }
}
