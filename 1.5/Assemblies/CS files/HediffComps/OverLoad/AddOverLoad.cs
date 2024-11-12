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

        public override void CompPostMake()
        {
            base.CompPostMake();
            if (!(parent.pawn.health.hediffSet.TryGetHediff(HE_HediffDefOf.AR_OverLoad, out var h) && h.TryGetComp(out overLoad)))
            {
                var h2 = parent.pawn.health.AddHediff(HE_HediffDefOf.AR_OverLoad);
                h2.TryGetComp(out overLoad);
            }
        }
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            if (overLoad != null)
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
    }
}
