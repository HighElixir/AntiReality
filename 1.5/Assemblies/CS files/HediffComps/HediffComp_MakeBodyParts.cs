using RimWorld;
using System;
using System.Collections.Generic;
using Verse.AI;
using Verse;

namespace HE_AntiReality
{
    // HediffComp_MakeBodyParts: 特定のボディパーツにHediffを付与するコンポーネント
    public class HediffComp_MakeBodyParts : HediffComp
    {
        // プロパティを取得。HediffCompProperties_MakeBodyPartsを参照
        public HediffCompProperties_MakeBodyParts Props
        {
            get
            {
                return (HediffCompProperties_MakeBodyParts)props;
            }
        }

        // 指定されたボディパーツラベルを検索し、条件が一致すればHediffを追加
        private void SeartchBodyPart(List<BodyPartRecord> parts, AddBodyParts add)
        {
            // ボディパーツリストをループして検索
            for (int i = 0; i < parts.Count; i++)
            {
                // ボディパーツの定義が一致し、カスタムラベルに特定の文字列が含まれている場合
                if (parts[i].def == add.createHediffOn && parts[i].untranslatedCustomLabel.IndexOf(add.partsLabel, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // Hediffをそのパーツに追加
                    Pawn.health.AddHediff(add.createHediff, parts[i], null, null);
                }
            }
        }

        // Hediffを追加する処理。特定のボディパーツを検索し、Hediffを付与
        private void SetHediff(AddBodyParts add)
        {
            // 追加するHediffが設定されているか確認
            if (add.createHediff != null)
            {
                // 現在のボディパーツを取得
                BodyPartRecord part = parent.Part;

                // 特定のボディパーツにHediffを作成する場合の処理
                if (add.createHediffOn != null)
                {
                    // ボディパーツラベルが設定されているかどうかで分岐
                    if (add.partsLabel != null)
                    {
                        // 全ボディパーツを取得し、指定されたラベルを持つパーツにHediffを追加
                        List<BodyPartRecord> parts = Pawn.RaceProps.body.AllParts;
                        SeartchBodyPart(parts, add);
                    }
                    else
                    {
                        // ラベル指定がない場合、指定されたパーツのみにHediffを追加
                        part = Pawn.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord p) => p.def == add.createHediffOn, null);
                        Pawn.health.AddHediff(add.createHediff, part, null, null);
                    }
                }
            }
        }

        // Hediffが追加された後に呼ばれるメソッド
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            // プロパティに定義されたボディパーツごとの処理を実行
            for (int i = 0; i < Props.addBodyParts.Count; i++)
            {
                SetHediff(Props.addBodyParts[i]);
            }
        }
    }
}
