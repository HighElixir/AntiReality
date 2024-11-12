namespace HE_AntiReality
{
    public class CompProperties_OverLoadFE : CompProperties_StoreFE
    {
        public float overloadLimit;         // 過負荷の上限値
        public float overloadFactor = 1;    // 過負荷増減にかかる係数
        public float overloadHealFactor;    // 過負荷回復の基礎値(FE/tick)
        public float waittoHealTime = 6000; // 回復開始までにかかるTick
        public CompProperties_OverLoadFE() => compClass = typeof(CompProperties_OverLoadFE);
    }
}
