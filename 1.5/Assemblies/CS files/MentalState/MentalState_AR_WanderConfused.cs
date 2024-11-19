using HE_AntiReality;
using Verse;
using Verse.AI;

namespace HE_AntiReality
{
    public class MentalState_AR_WanderConfused : MentalState_WanderConfused
    {
        public float amount = 0.015f;
        public override void MentalStateTick()
        {
            base.MentalStateTick();
            if (pawn.IsHashIntervalTick(500) && Rand.Chance(0.2f))
            {
                LostofRealityUtility.Add(pawn, amount);
            }
        }
    }
}
