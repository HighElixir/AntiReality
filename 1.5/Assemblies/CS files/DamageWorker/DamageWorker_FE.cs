using RimWorld;
using UnityEngine;
using Verse;

namespace HE_AntiReality
{
    public class DamageWorker_FE : DamageWorker_AddInjury
    {

        public override DamageResult Apply(DamageInfo dinfo, Thing victim)
        {
            if ((victim is Pawn pawn) && Rand.Chance(0.02f))
            {
                MoteMaker.ThrowText(new Vector3(pawn.Position.x + 1f, pawn.Position.y, pawn.Position.z + 1f), pawn.Map, HE_Constants.fictionStateReason.Translate(), HE_Constants.fictionDeepPurple);
                pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Psychotic, HE_Constants.fictionStateReason.Translate());
            }
            return base.Apply(dinfo, victim);
        }
    }
}
