using RimWorld;
using System;
using System.Text;
using UnityEngine;
using Verse;

namespace HE_AntiReality
{
    public class Comp_StoreFE : ThingComp
    {
        private CompProperties_StoreFE Props
        {
            get { return (CompProperties_StoreFE)props; }
        }

        public float MaxFE { get { return Props.maxFE; } }

        private float currentFE = 0;
        public float CurrentFE
        {
            get
            {
                return currentFE;
            }
            set
            {
                currentFE = Mathf.Clamp(value, 0, MaxFE);
            }
        }
        public float ChargeFEEfficiency { get { return Props.efficiency; } }
        public float RequiredFE { get { return Props.requireFE; } }
        public float ConsumptionPerTick { get { return Props.consumptionPerTick; } }
        public bool IsSetCharger { get; set; } = false;

        public virtual bool Condition { get; private set; } = true;

        public virtual bool CanUseEffect
        {
            get
            {
                return CurrentFE > RequiredFE && Condition && CurrentFE > 0;
            }
        }
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            CurrentFE = Props.maxFE;
        }
        public override void CompTick()
        {
            base.CompTick();

            if (!IsSetCharger && ConsumptionPerTick > 0)
            {
                // チャージャーが設定されていない場合のエネルギー消費
                CurrentFE = Math.Max(0, CurrentFE - ConsumptionPerTick);
            }
        }

        public virtual void UseItemSkills()
        {
            CurrentFE -= RequiredFE;
        }

        public override string CompInspectStringExtra()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"最大FE: {MaxFE.ToString("0.00")}fu");
            sb.AppendLine($"貯蓄FE: {CurrentFE.ToString("0.00")}fu");
            sb.AppendLine($"蓄積効率: {ChargeFEEfficiency.ToString("0.00")}");
            sb.AppendLine($"動作に必要なFE: {RequiredFE.ToString("0.00")}fu");
            sb.AppendLine($"待機中の消耗FE: {(ConsumptionPerTick * 2500).ToString("0.00")} fu/h");
            return sb.ToString().TrimEnd();
        }
    }
}
