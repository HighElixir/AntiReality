using RimWorld;
using System;
using System.Collections.Generic;
using Verse;

namespace HE_AntiReality
{
    public class HediffCompProperties_ForceDeath : HediffCompProperties
    {
        public List<HediffDef> exceptionHediffDefs;
        public List<TraitDef> exceptionTraitDefs;

        public HediffCompProperties_ForceDeath()
        {
            compClass = typeof(HediffComp_ForceDeath);
        }
    }

    /// <summary>
    /// ポーンを特定の条件下で強制的に死亡させるHediffコンポーネント。
    /// 動物やメカノイドにも適用可能。
    /// 将来的に死亡判定はスタティッククラスにまとめる予定。
    /// </summary>
    public class HediffComp_ForceDeath : HediffComp
    {
        // コンポーネントのプロパティを取得
        public HediffCompProperties_ForceDeath Props => (HediffCompProperties_ForceDeath)props;

        // Hediffが作成された直後に呼ばれるメソッド
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostMake();

            // ポーンが存在しない、または既に死亡・削除されている場合は処理しない
            if (Pawn == null || Pawn.Dead || Pawn.Destroyed)
            {
                return;
            }

            // ポーンにヘルス情報がない場合は処理しない
            if (Pawn.health == null)
            {
                return;
            }

            // 例外Hediffを持っているかチェック
            if (Props.exceptionHediffDefs != null && Props.exceptionHediffDefs.Any())
            {
                if (Pawn.health.hediffSet.hediffs.Any(h => Props.exceptionHediffDefs.Contains(h.def)))
                {
                    return; // 例外Hediffを持っていれば何もしない
                }
            }

            // ポーンがTraitsを持つ場合のみチェック
            if (Pawn.story != null && Props.exceptionTraitDefs != null && Props.exceptionTraitDefs.Any())
            {
                if (Pawn.story.traits.allTraits.Any(t => Props.exceptionTraitDefs.Contains(t.def)))
                {
                    return; // 例外Traitを持っていれば何もしない
                }
            }

            // ポーンを死亡させる
            Pawn.Kill(dinfo);

            // 死体を削除する
            if (Pawn.Corpse != null && Pawn.Corpse.Spawned)
            {
                Pawn.Corpse.Destroy(DestroyMode.Vanish);
            }
        }
    }
}
