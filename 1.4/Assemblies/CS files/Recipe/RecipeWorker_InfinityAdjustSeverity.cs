using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace HE_AntiReality
{
    public class RecipeWorker_InfinityAdjustSeverity : RecipeWorker//Severityを増加させるレシピ（作業中）
    {
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            if (pawn == null)
            {
                Log.Error("pawn is null in RecipeWorker_AdjustSeverity");
                return;
            }

            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("InfinityAnchorAdjustSeverity"));
            if (hediff != null)
            {
                float newSeverity = hediff.Severity + 0.05f; // 重症度を増加させる（必要に応じて変更）
                hediff.Severity = newSeverity;
                Messages.Message($"{pawn.Name}のｲﾝﾌｨﾆﾃｨｱﾝｶｰの調整が完了しました。現在のシンクロ率は{hediff.Severity}です。", MessageTypeDefOf.PositiveEvent);
            }
        }

        public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
        {
            return false; // 適用が違反行為かどうか
        }
    }
}
