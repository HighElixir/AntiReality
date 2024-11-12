using System;
using System.Collections.Generic;
using Verse;

namespace HE_AntiReality
{
    public class Hediffs
    {
        public HediffDef hediffDef;

        public bool skipIfAlreadyExists = true;
    }
    public class HediffCompProperties_GiveHediffs : HediffCompProperties
    {
        public List<Hediffs> hediffs;

        public HediffCompProperties_GiveHediffs()
        {
            this.compClass = typeof(HediffComp_GiveHediffs);
        }
    }

    // Pawnに複数のHediffを付与するためのHediffComp
    public class HediffComp_GiveHediffs : HediffComp
    {
        // プロパティを取得。HediffCompProperties_GiveHediffsを参照
        private HediffCompProperties_GiveHediffs Props
        {
            get
            {
                return (HediffCompProperties_GiveHediffs)this.props;
            }
        }

        // Hediffが追加された後に呼ばれるメソッド
        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            // Props.hediffsに定義されたHediffを順番に処理
            foreach (var prop in Props.hediffs)
            {
                // 既に同じHediffがある場合はスキップするオプション
                if (prop.skipIfAlreadyExists && this.parent.pawn.health.hediffSet.HasHediff(prop.hediffDef, false))
                {
                    continue; // 既にHediffが存在する場合、次のループにスキップ
                }

                // Pawnに新しいHediffを付与
                this.parent.pawn.health.AddHediff(prop.hediffDef, null, null, null);
            }
        }
    }
}
