using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace HE_AntiReality
{
    public class CompProperties_StoreFE : CompProperties // FE(Fiction Energy)
    {
        public float maxFE;
        public float requireFE = 0f;
        public float efficiency = 1f;
        public float consumptionPerTick = 0f;
        public bool overloadShouldBeWithinSafeValue = false;
        public CompProperties_StoreFE() => compClass = typeof(Comp_StoreFE);
    }

    public class Comp_StoreFE : ThingComp
    {
        // フィールド
        // 現在のFE値
        private float _currentFE = 0;
        private bool _isSet = false;
        private bool _canUse = true;
        private Texture2D _icon;
        private Comp_Overload _overload;

        // プロパティ
        private CompProperties_StoreFE Props => (CompProperties_StoreFE)props;// このコンポーネントのプロパティを取得

        // 最大FE（エネルギーの最大値）
        public float MaxFE => Props.maxFE;

        public float CurrentFE
        {
            get =>_currentFE; // 現在のFEを返す
            set => _currentFE = Mathf.Clamp(value, 0, MaxFE); // FEを0からMaxFEの範囲に制限
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
            get => _isSet;
            set => _isSet = value;
        }

        // 条件が満たされているか（エネルギーの使用が可能かどうかの条件）
        public bool Condition
        {
            get => _canUse;
            set => _canUse = value;
        }

        // エフェクト（スキル）を使用できるかどうかの判断基準
        public bool CanUseEffect()
        {
           return CurrentFE > RequiredFE && Condition && CurrentFE > 0;
        }

        // 初期化メソッド（コンポーネントが作成されたときに呼ばれる）
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            if (Props.overloadShouldBeWithinSafeValue)
            {
                _overload = parent.TryGetComp<Comp_Overload>();
                if (_overload == null)
                {
                    Log.Error("");
                }
            }
        }

        // 毎ティック呼ばれるメソッド（エネルギー消費の処理などを行う）
        public override void CompTick()
        {
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
            return GetFEInfo();
        }

        public override string CompTipStringExtra()
        {
            return GetFEInfo();
        }

        private string GetFEInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"最大FE: {MaxFE.ToString("0.00")}fu"); // 最大FE
            sb.AppendLine($"貯蓄FE: {CurrentFE.ToString("0.00")}fu"); // 現在のFE
            sb.AppendLine($"蓄積効率: {ChargeFEEfficiency.ToString("0.00")}"); // 蓄積効率
            sb.AppendLine($"動作に必要なFE: {RequiredFE.ToString("0.00")}fu"); // スキルに必要なエネルギー
            sb.AppendLine($"待機中の消耗FE: {(ConsumptionPerTick * 2500).ToString("0.00")} fu/h"); // 待機中の消費量
            return sb.ToString().TrimEnd(); // 最後の改行を削除して文字列を返す
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref _currentFE, ExposeUtility.GetExposeKey(typeof(Comp_StoreFE), "currentFE"), Props.maxFE);
            Scribe_Values.Look(ref _isSet, ExposeUtility.GetExposeKey(typeof(Comp_StoreFE), "_isSet"), false);
            Scribe_Values.Look(ref _canUse, ExposeUtility.GetExposeKey(typeof(Comp_StoreFE), "_canUse"), true);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (!DebugSettings.godMode)
                yield break;
            foreach (var item in InspectorGizmo())
            {
                yield return item;
            }
        }

        // インスペクタに表示されるGizmo（デバッグ用）を返すメソッド
        public IEnumerable<Command_Action> InspectorGizmo()
        {
            // アイコンをロード（初回のみ）
            if (_icon == null)
            {
                _icon = ContentFinder<Texture2D>.Get(AR_Constants.debugGizmoIconPath);
            }
            if (DebugSettings.godMode)
            {
                // コマンドリストを作成
                List<Command_Action> actions = new List<Command_Action>();
                var defaultDesc = AR_Constants.Gizmo_FESetDebugDesc.Translate();

                // FEを100%に設定するアクション
                actions.Add(new Command_Action
                {
                    defaultLabel = "100%",
                    defaultDesc = defaultDesc + "100%",
                    icon = _icon,
                    action = () =>
                    {
                        CurrentFE = MaxFE; // FEを最大値に設定
                    }
                });

                // FEを50%に設定するアクション
                actions.Add(new Command_Action
                {
                    defaultLabel = "50%",
                    defaultDesc = defaultDesc + "50%",
                    icon = _icon,
                    action = () =>
                    {
                        CurrentFE = MaxFE * 0.5f; // FEを50%に設定
                    }
                });

                // FEを0%に設定するアクション
                actions.Add(new Command_Action
                {
                    defaultLabel = "0%",
                    defaultDesc = defaultDesc + "0%",
                    icon = _icon,
                    action = () =>
                    {
                        CurrentFE = 0; // FEを0に設定
                    }
                });

                // 作成したアクションリストを返す
                return actions;
            }

            // 条件が合わない場合はnullを返す
            return null;
        }
    }
}
