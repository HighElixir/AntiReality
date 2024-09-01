using Verse;
using RimWorld;

namespace HE_AntiReality
{
    public class Verb_Warp : Verb
    {
        protected override bool TryCastShot()
        {
            // ワープアクションの実行ロジック
            if (EquipmentSource != null && EquipmentSource is ThingWithComps thingWithComps)
            {
                var comp = thingWithComps.GetComp<CompUseEffect_WarpToAnchor>();
                if (comp != null)
                {
                    comp.DoEffect(this.CasterPawn);
                    return true;
                }
            }
            return false;
        }
    }
}