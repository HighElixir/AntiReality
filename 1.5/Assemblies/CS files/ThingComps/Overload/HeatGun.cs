using HighElixir.Utility.RimWorld;
using RimWorld;
using System;
using Verse;

namespace HE_AntiReality
{
    public class CompProperties_HeatGun : CompProperties
    {
        public float addOverLoadPerFire;
        public float addLostofRealityPerPLevelAndhour;
        public float addLostofRealityPerPLevel;
        public CompProperties_HeatGun() => compClass = typeof(Comp_HeatGun);
    }

    public class Comp_HeatGun : Comp_OverloadUserBase
    {
        public Pawn equipped;
        public int moteInterval = 180;

        protected override bool DoesHaveReference => base.DoesHaveReference && equipped != null;
        public CompProperties_HeatGun Props => (CompProperties_HeatGun)props;
        public override void MoltenReality(int level)
        {
            var increaseAmount = Props.addLostofRealityPerPLevel * level;
            Add(increaseAmount);
        }

        public override void HandleOverloadLimitExceededTick(int level)
        {
            float increaseAmount = Props.addLostofRealityPerPLevelAndhour * level / AR_Constants.OnehourTickCount;
            Add(increaseAmount);
        }

        private void Add(float amount)
        {
            LostofRealityUtility.Add(equipped, amount);

            if (parent.IsHashIntervalTick(moteInterval))
            {
                MoteMaker.ThrowText(HE_RimUtility.MakeMotePosition(equipped), equipped.Map, "精神が溶けている".Translate(), AR_Constants.fictionDeepPurple);
            }
        }

        public override void Notify_UsedVerb(Pawn pawn, Verb verb)
        {
            if (!DoesHaveReference)
                return;
            AddOverload(Props.addOverLoadPerFire);
        }

        public override void Notify_Equipped(Pawn pawn)
        {
            equipped = pawn;
        }
        public override void Notify_Unequipped(Pawn pawn)
        {
            base.Notify_Unequipped(pawn);
            equipped = null;
        }
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_References.Look(ref equipped, ExposeUtility.GetExposeKey(typeof(Comp_HeatGun), nameof(equipped)));
        }
    }
}
