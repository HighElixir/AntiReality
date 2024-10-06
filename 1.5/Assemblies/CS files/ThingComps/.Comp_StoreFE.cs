using RimWorld;
using System;
using System.Text;
using UnityEngine;
using Verse;

namespace HE_AntiReality
{
    // FE（Fuel Energy）を蓄積し、消費するためのコンポーネント
    public class Comp_StoreFE : ThingComp
    {
        // フィールド
        // 現在のFE値
        private float currentFE = 0;
        private bool isSet = false;
        private bool canUse = true;

        // プロパティ
        private CompProperties_StoreFE Props => (CompProperties_StoreFE)props;// このコンポーネントのプロパティを取得

        // 最大FE（エネルギーの最大値）
        public float MaxFE => Props.maxFE;
        
        public float CurrentFE
        {
            get
            {
                return currentFE; // 現在のFEを返す
            }
            set
            {
                // FEを0からMaxFEの範囲に制限
                currentFE = Mathf.Clamp(value, 0, MaxFE);
            }
        }

        // 蓄積効率（チャージ効率）
        public float ChargeFEEfficiency => Props.efficiency;

        // 必要なFE（スキルや動作に必要なエネルギー）
        public float RequiredFE => Props.requireFE;

        // 待機中のエネルギー消費量（1ティックあたり）
        public float ConsumptionPerTick => Props.consumptionPerTick;

        // チャージャーがセットされているかどうか
        public bool IsSetCharger
        {
            get => isSet;
            set => isSet = value;
        }

        // 条件が満たされているか（エネルギーの使用が可能かどうかの条件）
        public virtual bool Condition
        {
            get => canUse;
            set => canUse = value;
        }

        // エフェクト（スキル）を使用できるかどうかの判断基準
        public virtual bool CanUseEffect => CurrentFE > RequiredFE && Condition && CurrentFE > 0;

        // 初期化メソッド（コンポーネントが作成されたときに呼ばれる）
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            // 初期FEは最大値に設定
            CurrentFE = Props.maxFE;
        }

        // 毎ティック呼ばれるメソッド（エネルギー消費の処理などを行う）
        public override void CompTick()
        {
            base.CompTick();

            // チャージャーが設定されていない場合、エネルギーを消費
            if (!IsSetCharger && ConsumptionPerTick > 0)
            {
                CurrentFE = Math.Max(0, CurrentFE - ConsumptionPerTick); // FEを減少
            }
        }

        // アイテムのスキルを使用するメソッド（エネルギーを消費）
        public virtual void UseItemSkills()
        {
            CurrentFE -= RequiredFE; // スキル使用時に必要なエネルギーを消費
        }

        // インスペクトパネルに表示する追加情報を返すメソッド
        public override string CompInspectStringExtra()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"最大FE: {MaxFE.ToString("0.00")}fu"); // 最大FE
            sb.AppendLine($"貯蓄FE: {CurrentFE.ToString("0.00")}fu"); // 現在のFE
            sb.AppendLine($"蓄積効率: {ChargeFEEfficiency.ToString("0.00")}"); // 蓄積効率
            sb.AppendLine($"動作に必要なFE: {RequiredFE.ToString("0.00")}fu"); // スキルに必要なエネルギー
            sb.AppendLine($"待機中の消耗FE: {(ConsumptionPerTick * 2500).ToString("0.00")} fu/h"); // 待機中の消費量
            return sb.ToString().TrimEnd(); // 最後の改行を削除して文字列を返す
        }
    }
}
