using RimWorld;
using System;
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
            bool b = false;
            for (int i = 0; i < traits.Count; i++)
            {
                if (traits[i].def.defName == traitDef.defName)
                {
                    return true;
                }
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
            for (int i = 0; i < this.Props.exceptionDefs.Count; i++) 
            {  
                ExceptionDefs exep = this.Props.exceptionDefs[i];
                if (exep.hediffDef != null) {
                    Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(exep.hediffDef, false);
                    if (firstHediffOfDef != null)
                    {
                        return;
                    }
                }
                if (exep.traitDef != null)
                {
                    if (ChackPawnHasTraits(exep.traitDef)) { return; }
}
            }
            p.health.SetDead();
            
        }

        public override void Notify_PawnKilled()
        {
            Pawn p = this.parent.pawn;
            if (p.health.Dead) 
            { 
                p.Destroy();
            }
        }
    }
}
