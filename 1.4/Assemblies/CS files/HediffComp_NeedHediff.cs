using RimWorld;
using System.Collections.Generic;
using Verse;

namespace HE_AntiReality
{
    public class HediffComp_NeedHediff : HediffComp
	{

		public HediffCompProperties_NeedHediff Props {
            get
            {
                return (HediffCompProperties_NeedHediff)this.props;
            }
        }

		private bool Multi()
		{
			Hediff hediff = base.Pawn.health.hediffSet.GetFirstHediffOfDef(this.parent.def);
            if (hediff != null && hediff.Severity >= 1.0) { return true; }
			return false;
		}
        private bool ChackPawnHasHediff()
		{
			Pawn p = base.Pawn;
            if (Props.mode!=null)
            {
				if (Props.mode == "Multi" && Multi())
				{
					return true;
				}
                    
            }
            foreach (HediffDef needhediff in Props.needHediffs){ 
				bool hasHediff = p.health.hediffSet.HasHediff(needhediff);
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

				return base.CompShouldRemove || !ChackPawnHasHediff();
			}
		}
	}
}

