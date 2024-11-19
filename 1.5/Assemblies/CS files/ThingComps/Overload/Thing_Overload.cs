using System;
using System.Collections.Generic;
using Verse;

namespace HE_AntiReality
{
    public class CompProperties_OverLoad : CompProperties
    {
        public float overloadLimit;         // 過負荷の上限値
        public float overloadFactor = 1;    // 過負荷増減にかかる係数
        public float overloadHealAmount;    // 過負荷回復の基礎値(FE/tick)
        public float waittoHealTime = 6000; // 回復開始までにかかるTick

        public List<float> penaltyThreshold = new List<float>()
        {
            0.1f,
            0.25f,
            0.37f,
            0.5f,
            0.82f
        };
        public CompProperties_OverLoad() => compClass = typeof(Comp_Overload);
    }

    public class Comp_Overload : ThingComp, ILoadReferenceable
    {
        private string _id = string.Empty;
        private float _time;
        private float _currentOverload;
        private bool isHyperUnRealMode;
        public bool IsHyperUnRealMode => isHyperUnRealMode;

        public CompProperties_OverLoad Props => (CompProperties_OverLoad)props;

        public string Id => _id;
        public float TimeSec => _time / 60f;
        public float OverloadFactor => Props.overloadFactor;
        public float OverloadHealAmount => Props.overloadHealAmount;
        public float OverloadLimit => Props.overloadLimit;
        public float WaittoHealTime => Props.waittoHealTime;

        public float CurrentOverload => _currentOverload;

        public bool IsLimit(float add = 0) => _currentOverload + add >= OverloadLimit;
        public override void CompTick()
        {
            if (!parent.IsHashIntervalTick(10))
                return;
            base.CompTick();
            if (_time >= WaittoHealTime && _currentOverload > 0)
            {
                _currentOverload -= OverloadHealAmount / AR_Constants.OnehourTickCount;
            }
            if (_currentOverload < 0) _currentOverload = 0;
        }

        public void AddOverload(float overload)
        {
            _currentOverload += overload;
        }
        public int GetPenaltyLevel()
        {
            float p = (_currentOverload - OverloadLimit) / OverloadLimit;
            if (p < 0) return 0;
            for (int i = 0; i < Props.penaltyThreshold.Count; i++)
            {
                if (p < Props.penaltyThreshold[i]) return i;
            }
            return Props.penaltyThreshold.Count;
        }

        public void ExposeData()
        {
            Type t = typeof(Comp_Overload);
            Scribe_Values.Look(ref _currentOverload, ExposeUtility.GetExposeKey(t, nameof(_currentOverload)), 0f);
            Scribe_Values.Look(ref _time, ExposeUtility.GetExposeKey(t, nameof(_time)), 0f);
            Scribe_Values.Look(ref isHyperUnRealMode, ExposeUtility.GetExposeKey(t, nameof(isHyperUnRealMode)), false);

            if (string.IsNullOrEmpty(_id))
                GetUniqueLoadID();
            Scribe_Values.Look(ref _id, ExposeUtility.GetExposeKey(t, nameof(_id)), string.Empty);
        }

        public string GetUniqueLoadID()
        {
            if (_id == string.Empty)
            {
                _id = new Guid().ToString();
            }
            return _id;
        }
    }
}
