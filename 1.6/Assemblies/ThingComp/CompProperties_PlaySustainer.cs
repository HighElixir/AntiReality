using Verse;
using Verse.Sound;

namespace AntiReality
{
    public class CompProperties_PlaySustainer : CompProperties
    {
        public SoundDef sound;
        public CompProperties_PlaySustainer()
        {
            compClass = typeof(CompPlaySustainer);
        }
    }

    public class CompPlaySustainer : ThingComp
    {
        private Sustainer sustainer;
        public CompProperties_PlaySustainer Props => (CompProperties_PlaySustainer)props;
        public override void PostPostMake()
        {
            if (Props.sound != null)
            {
                sustainer = Props.sound.TrySpawnSustainer(SoundInfo.InMap(parent, MaintenanceType.PerTick));
            }
        }
        public override void CompTick()
        {
            base.CompTick();
            if (sustainer != null && !sustainer.Ended)
            {
                sustainer.Maintain();
            }
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {
            if (sustainer != null)
            {
                sustainer.End();
                sustainer = null;
            }
        }
        public override void PostExposeData()
        {
            Scribe_Deep.Look(ref sustainer, "sustainer");
        }
    }
}
