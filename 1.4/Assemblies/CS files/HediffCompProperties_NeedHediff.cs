using Verse;
using System.Collections.Generic;

namespace HE_AntiReality 
{
    public class HediffCompProperties_NeedHediff : HediffCompProperties
    {
        public List<HediffDef> needHediffs;
        public string mode = "None";
        public HediffCompProperties_NeedHediff()
        {
            this.compClass = typeof(HediffComp_NeedHediff);
        }
    }
}
