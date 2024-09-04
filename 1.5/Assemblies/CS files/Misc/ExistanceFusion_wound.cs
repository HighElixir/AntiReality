using Verse;

namespace HE_AntiReality
{
    //削除を防ぐ
    public class ExistanceFusion_wound : HediffWithComps
    {
        public override bool ShouldRemove => false;

        public override float Severity
        {
            get
            {
                return base.Severity;
            }
            set
            {
                if (value <= 0f)
                {
                    value = 0f;
                    value += def.initialSeverity;
                }

                base.Severity = value;
            }
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            if (!DebugSettings.godMode)
            {
                pawn.health.AddHediff(this);
            }
        }
    }
}