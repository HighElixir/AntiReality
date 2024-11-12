using Verse;

namespace HE_AntiReality
{
    /// <summary>
    /// Note:基本は継承前提
    /// </summary>
    public abstract class Comp_OverloadFE : Comp_StoreFE, IExposable
    {
        private float _time;
        private float _currentOverload;
        private bool isHyperUnRealMode;
        public bool IsHyperUnRealMode => isHyperUnRealMode;

        // CompPropertiesで定義すること
        protected float overloadFactor;

        protected float overloadHealFactor;

        protected float overloadLimit;

        protected float waittoHealTime; // 任意（基本値6000tick）
        // ここまで
        protected float TimeSec => _time / 60f;
        public float OverloadFactor => overloadFactor;
        public float OverloadHealFactor => overloadHealFactor;
        public float OverloadLimit => overloadLimit;
        public float WaittoHealTime => waittoHealTime;

        public float CurrentOverload => _currentOverload;

        public bool IsLimit(float add = 0) => _currentOverload + add >= overloadLimit;
        public override void CompTick()
        {
            base.CompTick();
            if (_time >= waittoHealTime && _currentOverload > 0)
            {
                _currentOverload -= overloadHealFactor;
            }
            if (_currentOverload < 0) _currentOverload = 0;
        }

        public virtual void AddOverload(float overload)
        {
            _currentOverload += overload;
        }
        public virtual int GetPenaltyLevel()
        {
            switch ((_currentOverload - overloadLimit) / overloadLimit)
            {
                case 0:
                    return 1;
                case 0.1f:
                    return 2;
                case 0.25f:
                    return 3;
                case 0.37f:
                    return 4;
                case 0.5f:
                    return 5;
            }
            return 6;
        }
        public virtual void MoltenReality(int level)
        {
            //parent
        }
        public virtual void ExposeData()
        {
            Scribe_Values.Look(ref _currentOverload, GetExposeKey(nameof(_currentOverload)), overloadLimit);
            Scribe_Values.Look(ref _time, GetExposeKey(nameof(_time)), 0);
            Scribe_Values.Look(ref isHyperUnRealMode, GetExposeKey(nameof(isHyperUnRealMode)), false);
        }

        /*public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            Command_ActionWithCooldown command = new Command_ActionWithCooldown()
            {
                
            };
            command.Disable();
            yield return command;
        }*/
    }
}
