using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace HE_AntiReality
{
    public class RecipeWorker_InfinityAdjustSeverity : RecipeWorker
    {
        public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
        {
            Pawn pawn = thing as Pawn;
            if (pawn != null)
            {
                // ポーンがAR_InfinityAnchorのHediffを持っているかどうかをチェック
                return pawn.health.hediffSet.HasHediff(HE_HediffDefOf.AR_InfinityAnchor);
            }

            // もしThingがPawnでなければ、このレシピは適用できない
            return false;
        }
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            if (pawn == null)
            {
                Log.Error("pawn is null in RecipeWorker_AdjustSeverity");
                return;
            }

            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HE_HediffDefOf.AR_InfinityAnchor);
            if (hediff != null)
            {
                float increment = 0.05f; // 増加させる量
                float newSeverity = hediff.Severity + increment;

                // MaximumSeverityチェック（最大値は適宜設定）
                if (newSeverity > hediff.def.maxSeverity)
                {
                    newSeverity = hediff.def.maxSeverity;
                }

                hediff.Severity = newSeverity;
                Messages.Message($"{pawn.Name}のｲﾝﾌｨﾆﾃｨｱﾝｶｰの調整が完了しました。現在のシンクロ率は{hediff.Severity}です。", MessageTypeDefOf.PositiveEvent);
            }
            else
            {
                Messages.Message($"{pawn.Name}にはAR_InfinityAnchorが見つかりませんでした。", MessageTypeDefOf.NegativeEvent);
            }
        }

        public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
        {
            return false;
        }
    }
}

