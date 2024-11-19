using Verse;

namespace HE_AntiReality
{
    public class HediffCompProperties_DecreaseReality : HediffCompProperties
    {
        public float increaseLostofReality = 0.004f;// Sev / 1h

        public HediffCompProperties_DecreaseReality() => compClass = typeof(HediffComp_DecreaseReality);
    }

    public class HediffComp_DecreaseReality : HediffComp
    {
        public int tickCount;
        public HediffCompProperties_DecreaseReality Props => (HediffCompProperties_DecreaseReality)props;

        public override void CompPostTick(ref float severityAdjustment)
        {
            tickCount++;
            if (tickCount >= AR_Constants.OnehourTickCount)
            {
                tickCount = 0;
                LostofRealityUtility.Add(Pawn, Props.increaseLostofReality);
            }
        }
        
        public override void CompExposeData()
        {
            Scribe_Values.Look(ref tickCount, ExposeUtility.GetExposeKey(typeof(HediffComp_DecreaseReality), nameof(tickCount)), 0, false);
        }
    }
}
