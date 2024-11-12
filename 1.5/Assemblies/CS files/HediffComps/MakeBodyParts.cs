using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace HE_AntiReality
{
    // 追加するボディパーツ情報を保持するクラス
    public class AddBodyParts
    {
        // Hediffを追加する対象のボディパーツの定義
        public BodyPartDef createHediffOn;

        // 追加するHediffの定義
        public HediffDef createHediff;

        // ボディパーツのラベル（untranslatedCustomLabel）での絞り込み
        public string partsLabel;
    }

    // HediffCompのプロパティクラス
    public class HediffCompProperties_MakeBodyParts : HediffCompProperties
    {
        // 追加するボディパーツのリスト
        public List<AddBodyParts> addBodyParts;

        public HediffCompProperties_MakeBodyParts()
        {
            compClass = typeof(HediffComp_MakeBodyParts);
        }
    }

    // 特定のボディパーツにHediffを付与するコンポーネント
    public class HediffComp_MakeBodyParts : HediffComp
    {
        // プロパティを取得
        public HediffCompProperties_MakeBodyParts Props => (HediffCompProperties_MakeBodyParts)props;

        // Hediffが追加された後に呼ばれるメソッド
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            // addBodyPartsリストがnullでないことを確認
            if (Props.addBodyParts == null || Props.addBodyParts.Count == 0)
            {
                Log.Warning($"[HE_AntiReality] {parent.def.defName} の addBodyParts が設定されていません。");
                return;
            }

            // 各AddBodyPartsを処理
            foreach (var add in Props.addBodyParts)
            {
                SetHediff(add);
            }
        }

        // Hediffを追加する処理
        private void SetHediff(AddBodyParts add)
        {
            // 必要な情報が設定されているか確認
            if (add?.createHediff == null || add.createHediffOn == null)
            {
                Log.Warning("[HE_AntiReality] AddBodyPartsの設定が不完全です。");
                return;
            }

            // ボディパーツを取得
            var parts = Pawn.RaceProps.body.AllParts.Where(p => p.def == add.createHediffOn);

            // partsLabelで絞り込み
            if (!string.IsNullOrEmpty(add.partsLabel))
            {
                parts = parts.Where(p => !string.IsNullOrEmpty(p.untranslatedCustomLabel) &&
                                         p.untranslatedCustomLabel.IndexOf(add.partsLabel, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            // 該当するボディパーツにHediffを追加
            foreach (var part in parts)
            {
                Pawn.health.AddHediff(add.createHediff, part);
            }
        }
    }
}
