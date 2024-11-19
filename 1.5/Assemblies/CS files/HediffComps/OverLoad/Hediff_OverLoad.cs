using RimWorld;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using Verse;
using HighElixir.Utility;

namespace HE_AntiReality
{
    public class HediffCompProperties_OverLoad : HediffCompProperties
    {
        public int waittoHealTime = 6000; // 回復開始までにかかるTick
        public HediffCompProperties_OverLoad() => compClass = typeof(HediffComp_OverLoad);
    }

    /// <summary>
    /// 
    /// </summary>
    public class HediffComp_OverLoad : HediffComp, ILoadReferenceable
    {
        private string _uniqueId = string.Empty;
        private int _time;
        private float _currentOverload;
        private float _overloadLimit;         // 過負荷の上限値
        private float _overloadLimitFactor = 1;
        private float _overloadFactor = 1;    // 過負荷増減にかかる係数
        private float _overloadHealFactor = 1;    // 過負荷回復の基礎値(FE/tick)
        private float _healAmount;

        private HediffCompProperties_OverLoad Props => (HediffCompProperties_OverLoad)props;
        public float CurrentOverLoad => _currentOverload;
        public float OverloadLimit => _overloadLimit * _overloadLimitFactor;
        public float OverloadFactor => _overloadFactor;
        public float HealAmount => _healAmount;
        public float HealFactor => _overloadHealFactor;

        public override bool CompShouldRemove => false;

        public override string CompTipStringExtra
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(AR_Constants.Tooltip_OverloadHediff_CurrentAmount.Translate(_currentOverload, OverloadLimit));
                if (_time > Props.waittoHealTime && _currentOverload > 0)
                    sb.AppendLine(AR_Constants.Tooltip_OverloadHediff_InHeal.Translate());
                int p = GetPenaltyLevel();
                if (p > 0)
                    sb.AppendLine(AR_Constants.Tooltip_OverloadHediff_Penalty.Translate(p));
                return sb.ToString().TrimEnd();
            }
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            if (_time > Props.waittoHealTime && _currentOverload > 0)
            {
                _currentOverload -= _healAmount * _overloadHealFactor;
            }
            else if (_time > Props.waittoHealTime && _currentOverload != 0)
            {
                _currentOverload = 0;
            }
            else
            {
                _time++;
            }
        }
        public int GetPenaltyLevel()
        {
            if (OverloadLimit == 0) return 0;
            float p = (_currentOverload - OverloadLimit) / OverloadLimit;
            if (p < 0.1f) return 1;
            if (p < 0.25f) return 2;
            if (p < 0.37f) return 3;
            if (p < 0.5f) return 4;
            if (p < 0.82f) return 5;
            return 0;
        }

        public void AddLimit(float amount = 0, float factor = 0)
        {
            _overloadLimitFactor.GetClamptoPositive(factor);
            _overloadLimit.GetClamptoPositive(amount);
        }

        public void SubLimit(float amount = 0, float factor = 0)
        {
            _overloadLimitFactor.GetClamptoPositive(-factor);
            _overloadLimit.GetClamptoPositive(-amount);
        }

        public void ChengeOverLoadFactor(float amount)
        {
            _overloadFactor.GetClamptoPositive(amount);
        }

        public void AddHeal(float amount = 0, float factor = 0)
        {
            _overloadHealFactor.GetClamptoPositive(factor);
            _healAmount.GetClamptoPositive(amount);
        }

        public void SubHeal(float amount = 0, float factor = 0)
        {
            _overloadHealFactor.GetClamptoPositive(-factor);
            _healAmount.GetClamptoPositive(-amount);
        }

        public bool AddOverLoad(float overload)
        {
            _time = 0;
            var f = false;
            if (_currentOverload + overload >= OverloadLimit)
            {
                f = true;
            }
            _currentOverload += overload;
            return f;
        }

        public void HealOverLoad(float overload)
        {
            _currentOverload.GetClamptoPositive(-overload);
        }

        private Texture2D icon;
        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            // アイコンをロード（初回のみ）
            if (icon == null)
            {
                icon = ContentFinder<Texture2D>.Get(AR_Constants.debugGizmoIconPath);
            }
            if (DebugSettings.godMode)
            {
                var a1 = new Command_Action()
                {
                    defaultLabel = "+100%",
                    defaultDesc = "IncreaseOverLoad",
                    icon = icon,
                    action = () =>
                    {
                        _currentOverload += 100;
                    }
                };
                yield return a1;

                var a2 = new Command_Action()
                {
                    defaultLabel = "-100%",
                    defaultDesc = "DecreaseOverLoad",
                    icon = icon,
                    action = () =>
                    {
                        _currentOverload -= 100;
                    }
                };
                yield return a2;
            }
            yield break;
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
            var t = typeof(HediffComp_OverLoad);
            Scribe_Values.Look(ref _currentOverload, ExposeUtility.GetExposeKey(t, nameof(_currentOverload)));
            Scribe_Values.Look(ref _overloadFactor, ExposeUtility.GetExposeKey(t, nameof(_overloadFactor)));
            Scribe_Values.Look(ref _overloadLimit, ExposeUtility.GetExposeKey(t, nameof(_overloadLimit)));
            Scribe_Values.Look(ref _overloadLimitFactor, ExposeUtility.GetExposeKey(t, nameof(_overloadLimitFactor)));
            Scribe_Values.Look(ref _overloadHealFactor, ExposeUtility.GetExposeKey(t, nameof(_overloadHealFactor)));
            Scribe_Values.Look(ref _healAmount, ExposeUtility.GetExposeKey(t, nameof(_healAmount)));
            Scribe_Values.Look(ref _time, ExposeUtility.GetExposeKey(t, nameof(_time)));
            Scribe_Values.Look(ref _uniqueId, ExposeUtility.GetExposeKey(t, nameof(_uniqueId)), Set());
        }

        public string GetUniqueLoadID()
        {
            if (string.IsNullOrEmpty(_uniqueId))
                Set();
            return _uniqueId;
        }

        private string Set()
        {
            _uniqueId = new Guid().ToString();
            return _uniqueId;
        }
    }
}
