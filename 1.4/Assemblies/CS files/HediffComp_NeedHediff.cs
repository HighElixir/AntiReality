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
        private bool ChackPawnHasHediff()
		{
			Pawn p = base.Pawn;

			Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(this.Props.needHediff, false);
            if (firstHediffOfDef == null || firstHediffOfDef.def.stages == null)
			{
				return false;
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

