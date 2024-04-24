using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse.AI;
using Verse;

namespace HE_AntiReality
{
    public class HediffComp_MakeBodyParts : HediffComp
    {
        public HediffCompProperties_MakeBodyParts Props
        {
            get
            {
                return (HediffCompProperties_MakeBodyParts)this.props;
            }
        }

        private void SeartchBodyPart(List<BodyPartRecord> parts, AddBodyParts add)
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if (parts[i].def == add.createHediffOn && parts[i].untranslatedCustomLabel.IndexOf(add.partsLabel, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    this.parent.pawn.health.AddHediff(add.createHediff, parts[i], null, null);
                }
            }
            return;
        }


        private void SetHediff(AddBodyParts add)
        {
            if (add.createHediff != null)
            {
                BodyPartRecord part = this.parent.Part;
                if (add.createHediffOn != null)
                {
                    if (add.partsLabel != null)
                    {
                        List<BodyPartRecord> parts = this.parent.pawn.RaceProps.body.AllParts;
                        SeartchBodyPart(parts, add);
                    }
                    else
                    {
                        part = this.parent.pawn.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord p) => p.def == add.createHediffOn, null);
                        this.parent.pawn.health.AddHediff(add.createHediff, part, null, null);
                    }
                }

                
            }
        }
        

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            for (int i = 0; i < this.Props.addBodyParts.Count; i++)
            {
                SetHediff(this.Props.addBodyParts[i]);
            }
        }
    }
}
