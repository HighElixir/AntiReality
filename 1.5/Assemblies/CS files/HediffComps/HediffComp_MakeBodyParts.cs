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
                return (HediffCompProperties_MakeBodyParts)props;
            }
        }

        //指定されたボディラベルを検索し、見つかった場合Hediffを追加する
        private void SeartchBodyPart(List<BodyPartRecord> parts, AddBodyParts add)
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if (parts[i].def == add.createHediffOn && parts[i].untranslatedCustomLabel.IndexOf(add.partsLabel, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    
                    Pawn.health.AddHediff(add.createHediff, parts[i], null, null);
                }
            }
            return;
        }

/*        private bool SeartchHediffOnBodyParts(BodyPartRecord part)
        {
            List<Hediff> hediffs = Pawn.health.hediffSet.hediffs;
            foreach(Hediff item in hediffs)
            {
                if (item.Part.untranslatedCustomLabel == part.untranslatedCustomLabel)
                {
                    return true;
                }
            }
            return false;
        }*/

        private void SetHediff(AddBodyParts add)
        {
            if (add.createHediff != null)
            {
                BodyPartRecord part = parent.Part;
                if (add.createHediffOn != null)
                {
                    if (add.partsLabel != null)
                    {
                        List<BodyPartRecord> parts = Pawn.RaceProps.body.AllParts;
                        SeartchBodyPart(parts, add);
                    }
                    else
                    {
                        part = Pawn.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord p) => p.def == add.createHediffOn, null);
                        Pawn.health.AddHediff(add.createHediff, part, null, null);
                    }
                }


            }
        }


        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            for (int i = 0; i < Props.addBodyParts.Count; i++)
            {
                SetHediff(Props.addBodyParts[i]);
            }
        }
    }
}
