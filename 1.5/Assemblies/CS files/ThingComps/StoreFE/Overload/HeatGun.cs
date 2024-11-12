using RimWorld;
using UnityEngine;
using Verse;

namespace HE_AntiReality
{
    public class CompProperties_HeatGun : CompProperties_OverLoadFE
    {
        public float addOverLoadPerFire;
        public CompProperties_HeatGun() => compClass = typeof(Comp_HeatGun);
    }

    public class Comp_HeatGun : Comp_OverloadFE
    {
        public Pawn equiped;
        public int moteInterval = 180;
        public int countTick = 180;

        public CompProperties_HeatGun Props => (CompProperties_HeatGun)props;

        public override void Initialize(CompProperties _)
        {
            base.Initialize(_);

            overloadLimit = Props.overloadLimit;
            overloadFactor = Props.overloadFactor;
            overloadHealFactor = Props.overloadHealFactor;
            waittoHealTime = Props.waittoHealTime;
        }
        public override void MoltenReality(int level)
        {
            var l = new Hediff();
            l.def = HE_HediffDefOf.LostOfReality;
            l.Severity = level / (100 * 60);
            equiped.health.hediffSet.AddDirect(l);
        }

        public override void CompTick()
        {
            base.CompTick();
            if (IsLimit())
            {
                MoltenReality(GetPenaltyLevel());
                if (countTick >= moteInterval)
                {
                    countTick = 0;
                    var v = new Vector3(equiped.Position.x + 1f, equiped.Position.y, equiped.Position.z + 1f);
                    MoteMaker.ThrowText(v, equiped.Map, "精神が溶けている".Translate(), HE_Constants.fictionDeepPurple);
                }
                else
                {
                    countTick++;
                }
            }
        }
        public override void Notify_UsedVerb(Pawn pawn, Verb verb)
        {
            AddOverload(Props.addOverLoadPerFire);
        }

        public override void Notify_Equipped(Pawn pawn)
        {
            equiped = pawn;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_References.Look(ref equiped, GetExposeKey(nameof(equiped)));
        }
    }
}
