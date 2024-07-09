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
                return (HediffCompProperties_forceDeath)props;
            }
        }

        private bool ChackPawnHasTraits(TraitDef traitDef)
        {
            List<Trait> traits = Pawn.story.traits.allTraits;
            foreach (Trait item in traits)
            {
                if (item.def.defName == traitDef.defName)
                {
                    return true;
                }
            }
            return false;
        }
        private bool ChackPawnHasHediffs(HediffDef hediffDef)
        {
            if (Pawn.health.hediffSet.HasHediff(hediffDef))
            {
                return true;
            }
            return false;
        }

        public override void CompPostMake()
        {

            
            if (Pawn.health.Dead)
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

            Pawn.Destroy(mode: DestroyMode.KillFinalize);

        }
    }
}
