using PipeSystem;
using Verse;

namespace HE_AntiReality
{
    public class CompProperties_WarpSystem : CompProperties
    {
        public CompProperties_WarpSystem()
        {
            this.compClass = typeof(CompWarpSystem);
        }
    }

    public class CompWarpSystem : ThingComp
    {
        CompResourceTrader trader;
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            trader = parent.GetComp<CompResourceTrader>();
            base.PostSpawnSetup(respawningAfterLoad);
        }

        public void Notyfyed_SuccessWarp()
        {//未実装
        }
    }
}
