using System;
using System.Collections.Generic;
using Verse;

namespace HE_AntiReality
{
    // HediffCompクラスは、Hediffに付随する追加の機能を提供する
    public class HediffComp_DeleteAlreadyExistAnchors : HediffComp
    {
        // 削除対象となるアンカーのリストを定義
        List<HediffDef> Anchors = new List<HediffDef>()
        {
            HediffDef.Named("AR_InfinityAnchor"), // 無限アンカー
            HediffDef.Named("AR_ZeroDimensionAnchor") // ゼロ次元アンカー
        };

        // Propsで現在のプロパティを取得
        public HediffCompProperties_DeleteAlreadyExistAnchors Props
        {
            get
            {
                return (HediffCompProperties_DeleteAlreadyExistAnchors)props;
            }
        }

        // Hediffが追加されたときに呼ばれるメソッド
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            // 現在のPawnを取得
            Pawn p = parent.pawn;

            // プロパティから特定のHediffDefを取得
            HediffDef hediffDef = Props.hediffDef;

            // もし現在のHediffDefが設定されているなら
            if (hediffDef != null)
            {
                // 削除対象のアンカーリストを走査
                foreach (HediffDef def in Anchors)
                {
                    // PawnのHediffSetから、該当するHediffを取得
                    Hediff target = p.health.hediffSet.GetFirstHediffOfDef(def);

                    // 既に該当するHediffが存在し、現在のHediffDefとは異なる場合
                    if (target != null && def != hediffDef)
                    {
                        // そのHediffを削除
                        p.health.RemoveHediff(target);
                    }
                }
            }
        }
    }
}
