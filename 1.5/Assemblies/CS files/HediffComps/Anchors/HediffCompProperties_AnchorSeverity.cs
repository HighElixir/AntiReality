using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HE_AntiReality
{
    /// <summary>
    ///  未完成
    /// </summary>
    public class HediffCompProperties_AnchorSeverity : HediffCompProperties
    {
        public float postTakeDamageDecreaseSeverity;


        public HediffCompProperties_AnchorSeverity() => compClass = typeof(HediffCompProperties_AnchorSeverity);
    }

    public class HediffComp_AnchorSeverity : HediffComp
    {
        // public float GetDecreaseFactor(float Severity) => 
        public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
        {
            base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
        }
    }
}
