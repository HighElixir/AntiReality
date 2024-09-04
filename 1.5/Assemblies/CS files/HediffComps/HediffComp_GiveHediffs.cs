using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HE_AntiReality
{
    public class HediffComp_GiveHediffs : HediffComp
    {
        private HediffCompProperties_GiveHediffs Props
        {
            get
            {
                return (HediffCompProperties_GiveHediffs)this.props;
            }
        }

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            foreach (var prop in Props.hediffs)
            {
                if (prop.skipIfAlreadyExists && this.parent.pawn.health.hediffSet.HasHediff(prop.hediffDef, false))
                {
                    continue;
                }
                this.parent.pawn.health.AddHediff(prop.hediffDef, null, null, null);
            }

        }
    }
}
