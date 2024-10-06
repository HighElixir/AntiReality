using Verse;
using RimWorld;
using System.Collections.Generic;

namespace HE_AntiReality
{
    // Infinity AnchorのHediffの重症度を調整するレシピの実装
    public class RecipeWorker_InfinityAdjustSeverity : RecipeWorker
    {
        // このレシピが現在のThingに適用可能かを確認するメソッド
        public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
        {
            Pawn pawn = thing as Pawn; // ThingがPawnかどうかを確認
            if (pawn != null)
            {
                // ポーンがAR_InfinityAnchorのHediffを持っているかどうかをチェック
                return pawn.health.hediffSet.HasHediff(HE_HediffDefOf.AR_InfinityAnchor);
            }

            // もしThingがPawnでなければ、このレシピは適用できない
            return false;
        }

        // 実際にポーンにレシピを適用するメソッド
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            // Pawnがnullでないかチェック
            if (pawn == null)
            {
                Log.Error("pawn is null in RecipeWorker_AdjustSeverity");
                return;
            }

            // PawnがAR_InfinityAnchorを持っているか確認し、取得
            Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(HE_HediffDefOf.AR_InfinityAnchor);
            if (hediff != null)
            {
                // 増加させるSeverityの量
                float increment = 0.05f;
                float newSeverity = hediff.Severity + increment; // 新しいSeverityを計算

                // MaximumSeverityを超えないように調整
                if (newSeverity > hediff.def.maxSeverity)
                {
                    newSeverity = hediff.def.maxSeverity;
                }

                // Severityを新しい値に設定
                hediff.Severity = newSeverity;

                // 調整完了メッセージを表示
                Messages.Message($"{pawn.Name}のｲﾝﾌｨﾆﾃｨｱﾝｶｰの調整が完了しました。現在のシンクロ率は{hediff.Severity}です。", MessageTypeDefOf.PositiveEvent);
            }
            else
            {
                // Infinity Anchorが見つからない場合のメッセージ
                Messages.Message($"{pawn.Name}にはAR_InfinityAnchorが見つかりませんでした。", MessageTypeDefOf.NegativeEvent);
            }
        }

        // レシピがPawnに対して倫理的に問題ないかを判断するメソッド（今回は常に問題なし）
        public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
        {
            return false; // 常にfalseを返す。違反行為とみなされない。
        }
    }
}
