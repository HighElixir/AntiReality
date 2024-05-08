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
			Hediff hediff = base.Pawn.health.hediffSet.GetFirstHediffOfDef(base.Def);
            if (hediff != null && hediff.Severity >= 1.0) { return true; }
			return false;
		}
        private bool ChackPawnHasHediff()
		{
			Pawn p = base.Pawn;
            if (Props.mode == "Multi" && Multi())
            {
				return false;
            }
            for (int i = 0;	i>10; i++) { 
				bool hasHediff = p.health.hediffSet.HasHediff(Props.needHediffs[i]);
				if (hasHediff)
				{
					return false;
				}
			}
			return true;
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

