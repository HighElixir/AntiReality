using RimWorld;
using Verse;
using HighElixir.Utility.RimWorld;

namespace HE_AntiReality
{
    public class DamageWorker_FE : DamageWorker_AddInjury
    {
        public override DamageResult Apply(DamageInfo dinfo, Thing victim)
        {
            float adjustedChance = 0.01f + (dinfo.Amount / 100f); // ダメージ量に応じて増加
                if ((victim is Pawn pawn) && Rand.Chance(adjustedChance))
            {
                MoteMaker.ThrowText(HE_RimUtility.MakeMotePosition(pawn), pawn.Map, AR_Constants.fictionStateReason.Translate(), AR_Constants.fictionDeepPurple);
                pawn.mindState.mentalStateHandler.TryStartMentalState(AR_MentalStateDefOf.AR_WanderConfused, AR_Constants.fictionStateReason.Translate());
            }
            return base.Apply(dinfo, victim);
        }
    }
}
