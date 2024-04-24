using RimWorld;
using System.Collections.Generic;
using Verse;

namespace HE_AntiReality
{
    public class HediffComp_NeedMultiInplantor : HediffComp
	{
        private bool ChackPawnHasHediff_2()
		{
			Pawn p = base.Pawn;

			Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(HE_HediffDefOf.AR_MultiImplantor, false);
			Hediff firstHediffOfDef2 = p.health.hediffSet.GetFirstHediffOfDef(this.parent.def, false);
            bool flg = firstHediffOfDef2.Severity < 1.0;
            if (flg && (firstHediffOfDef == null || firstHediffOfDef.def.stages == null) )
            {
                return false;
            }
            return true;
		}

		public override bool CompShouldRemove
		{
			get
			{

				return base.CompShouldRemove || !ChackPawnHasHediff_2();
			}
		}
	}
}

