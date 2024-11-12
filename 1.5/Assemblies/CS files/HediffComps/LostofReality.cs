using System.Text;
using Verse;

namespace HE_AntiReality
{
    public class HediffCompProperties_LostofReality : HediffCompProperties_SeverityPerDay
    {
        public HediffCompProperties_LostofReality() => compClass = typeof(LostofReality);
    }

    public class LostofReality : HediffComp_SeverityPerDay
    {
        private bool _isStop;

        public void Stop()
        {
            if (!_isStop)
            {
                _isStop = true;
            }
        }

        public void Cont()
        {
            if (_isStop)
            {
                _isStop = false;
            }
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            if (!_isStop)
            {
                base.CompPostTick(ref severityAdjustment);
            }
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref _isStop, ExposeUtility.GetExposeKey(typeof(LostofReality), nameof(_isStop)), false, false);
        }

        public override string CompTipStringExtra
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(base.CompTipStringExtra);
                if (_isStop) sb.AppendLine("重症度減少：停止中");
                return sb.ToString();
            }
        }
    }
}
