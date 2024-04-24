using Verse;

namespace HE_AntiReality 
{
    public class HediffCompProperties_NeedHediff : HediffCompProperties
    {
        public HediffDef needHediff;
        public HediffCompProperties_NeedHediff()
        {
            this.compClass = typeof(HediffComp_NeedHediff);
        }
    }
}
