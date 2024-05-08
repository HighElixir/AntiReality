using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HE_AntiReality
{
    public class HediffComp_forceDeath : HediffComp
    {
        public HediffCompProperties_forceDeath Props
        {
            get
            {
                return (HediffCompProperties_forceDeath)this.props;
            }
        }

        private bool ChackPawnHasTraits(TraitDef traitDef)
        {
            Pawn p = base.Pawn;

            List<Trait> traits = p.story.traits.allTraits;
            for (int i = 0; i < traits.Count; i++)
            {
                if (traits[i].def.defName == traitDef.defName)
                {
                    return true;
                }
            }
            return false;
        }
        private bool ChackPawnHasHediffs(HediffDef hediffDef)
        {
            Pawn p = base.Pawn;

            if (p.health.hediffSet.HasHediff(hediffDef))
            {
                return true;
            }
            return false;
        }

        public override void CompPostMake()
        {

            Pawn p = this.parent.pawn;
            if (p.health.Dead)
            {
                return;
            }

            if (Props.exceptionHediffDefs.Count > 0)
            {
                foreach (HediffDef item in Props.exceptionHediffDefs)
                {
                    if (ChackPawnHasHediffs(item)) { return; }
                }
            }
            if (Props.exceptionTraitDefs.Count > 0)
            {
                foreach (TraitDef item in Props.exceptionTraitDefs)
                {
                    if (ChackPawnHasTraits(item)) { return; }
                }
            }

            p.health.SetDead();

        }
    }
}
