using PipeSystem;
using RimWorld;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.Noise;

namespace HE_AntiReality
{
    public class CompChargeFE : CompResourceTrader
    {
        private float idleConsumptionPerTickBase; // 基本的なアイドル時の消費
        private Comp_StoreFE comp_StoreFE;

        // FEをストアしているコンポーネントを取得する
        public Comp_StoreFE StoreFE
        {
            get
            {
                Thing heldThing = HeldThing; // 保持しているアイテムを取得
                return heldThing != null ? heldThing.TryGetComp<Comp_StoreFE>() : null; // Comp_StoreFEを取得
            }
        }

        // 現在の位置に存在するアイテムを取得する
        public Thing HeldThing
        {
            get
            {
                List<Thing> thingsAtPosition = parent.Map.thingGrid.ThingsListAt(parent.Position); // 現在の位置にあるアイテム一覧を取得
                if (Props.category != null)
                {
                    // カテゴリ内の子要素に該当するアイテムがあるか確認
                    foreach (ThingDef childThingDef in Props.category.childThingDefs)
                    {
                        foreach (Thing thing in thingsAtPosition)
                        {
                            if (thing.def == childThingDef)
                            {
                                return thing; // 該当するアイテムを返す
                            }
                        }
                    }
                }
                return null; // 該当するものがない場合はnullを返す
            }
        }

        // プロパティを取得する
        public new CompProperties_FECharge Props => (CompProperties_FECharge)props;

        // 初期セットアップ時に呼ばれる
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad); // ベースメソッドを呼ぶ
            idleConsumptionPerTickBase = Props.idleConsumptionPerTick; // アイドル時の消費を設定
        }

        // 毎ティック呼ばれるメソッド
        public override void CompTick()
        {
            if (comp_StoreFE != StoreFE)
            {
                comp_StoreFE = StoreFE;
            }
            if (comp_StoreFE != null) // StoreFEが存在する場合
            {
                // チャージが可能であり、FEの最大量を超えていないかを確認
                if (Props.chargepertick > 0 && comp_StoreFE.CurrentFE < comp_StoreFE.MaxFE)
                {
                    // FEをチャージし、効率も考慮
                    comp_StoreFE.CurrentFE = comp_StoreFE.CurrentFE + Props.chargepertick * comp_StoreFE.ChargeFEEfficiency * Props.chargeEfficiency;

                    // アイドル時の消費量も更新
                    if (Props.idleConsumptionPerTick != idleConsumptionPerTickBase + Props.chargepertick * Props.chargeEfficiency)
                    {
                        Props.idleConsumptionPerTick = idleConsumptionPerTickBase + Props.chargepertick * Props.chargeEfficiency;
                    }
                    base.CompTick(); // ベースメソッドを呼ぶ
                }
            }

            // アイドル時の消費量をリセット
            if (Props.idleConsumptionPerTick != idleConsumptionPerTickBase)
            {
                Props.idleConsumptionPerTick = idleConsumptionPerTickBase;
            }
            base.CompTick(); // ベースメソッドを呼ぶ
        }

        // インスペクトパネルに表示される追加の情報を返す
        public override string CompInspectStringExtra()
        {
            if (comp_StoreFE != StoreFE)
            {
                comp_StoreFE = StoreFE;
            }
            StringBuilder sb = new StringBuilder(); // 文字列を構築

            if (comp_StoreFE != null)
            {
                // 蓄積中のFEアイテム情報を追加
                sb.AppendLine($"蓄積中：{comp_StoreFE.parent.def.label}");
            }

            // 蓄積速度と効率を追加表示
            sb.AppendLine($"蓄積速度：{(Props.chargepertick * 2500).ToString("0.00")}fu/h");
            sb.AppendLine($"蓄積効率：{Props.chargeEfficiency.ToString("0.00")}");

            // ベースクラスの情報も追加
            return sb.ToString() + base.CompInspectStringExtra();
        }
    }
}
