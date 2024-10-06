using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace HE_AntiReality
{
    // 強制的にPawnを死亡させるためのHediffコンポーネント
    /// <summary>
    /// 今後死亡判定は１つのスタティッククラスにまとめる予定
    /// </summary>
    public class HediffComp_forceDeath : HediffComp
    {
        // コンポーネントのプロパティを取得
        public HediffCompProperties_forceDeath Props
        {
            get
            {
                return (HediffCompProperties_forceDeath)props;
            }
        }

        // Pawnが特定のTraitを持っているかどうかを確認
        private bool ChackPawnHasTraits(TraitDef traitDef)
        {
            List<Trait> traits = Pawn.story.traits.allTraits; // Pawnの全Traitを取得
            foreach (Trait item in traits)
            {
                if (item.def.defName == traitDef.defName) // 指定されたTraitDefと一致するか確認
                {
                    return true; // 一致するTraitがあればtrueを返す
                }
            }
            return false; // 一致しなければfalse
        }

        // Pawnが特定のHediffを持っているかどうかを確認
        private bool ChackPawnHasHediffs(HediffDef hediffDef)
        {
            if (Pawn.health.hediffSet.HasHediff(hediffDef)) // Pawnが指定されたHediffを持っているか確認
            {
                return true;
            }
            return false;
        }

        // Hediffが作成された直後に呼ばれるメソッド
        public override void CompPostMake()
        {
            // もしPawnがすでに死亡している場合は、何もしない
            if (Pawn.health.Dead)
            {
                return;
            }

            // 例外Hediffが設定されている場合、それを持っているかチェック
            if (Props.exceptionHediffDefs.Count > 0)
            {
                foreach (HediffDef item in Props.exceptionHediffDefs)
                {
                    if (ChackPawnHasHediffs(item)) { return; } // 例外Hediffを持っていれば何もしない
                }
            }

            // 例外Traitが設定されている場合、それを持っているかチェック
            if (Props.exceptionTraitDefs.Count > 0)
            {
                foreach (TraitDef item in Props.exceptionTraitDefs)
                {
                    if (ChackPawnHasTraits(item)) { return; } // 例外Traitを持っていれば何もしない
                }
            }

            // 例外条件に当てはまらなければ、Pawnを強制的に死亡させる
            Pawn.Destroy(mode: DestroyMode.KillFinalize);
        }
    }
}
