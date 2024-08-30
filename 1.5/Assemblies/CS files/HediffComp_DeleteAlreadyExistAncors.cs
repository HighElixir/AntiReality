using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HE_AntiReality
{
    public class HediffComp_DeleteAlreadyExistAncors : HediffComp
    {
        List<HediffDef> ancors = new List<HediffDef>()
            {
                HediffDef.Named("AR_InfinityAnchor"),
                HediffDef.Named("AR_ZeroDimensionAnchor")
            };

        public HediffCompProperties_DeleteAlreadyExistAncors Props
        {
            get
            {
                return (HediffCompProperties_DeleteAlreadyExistAncors)props;
            }
        }
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            Pawn p = parent.pawn;
            Hediff hediff = Props.hediff;
            if (hediff != null) 
            {
                foreach(HediffDef def in ancors)
                {
                    Hediff target = p.health.hediffSet.GetFirstHediffOfDef(def);
                    if (target != null && target != hediff)
                    {
                        p.health.RemoveHediff(target);
                    }
                }
            }
        }
    }
}
