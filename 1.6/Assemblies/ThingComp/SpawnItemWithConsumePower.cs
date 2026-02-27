using RimWorld;
using System.Collections.Generic;
using Verse;

namespace AntiReality
{
    public class CompProperties_SpawnerItemsWithPower : CompProperties
    {
        public int spawnInterval = 60000;
        public List<ThingDef> spawnThings = new List<ThingDef>();
        public CompProperties_SpawnerItemsWithPower()
        {
            compClass = typeof(CompSpawnerItemsWithPower);
        }
    }
    public class CompSpawnerItemsWithPower : ThingComp
    {
        private int ticksPassed;

        public CompProperties_SpawnerItemsWithPower Props => (CompProperties_SpawnerItemsWithPower)props;

        public bool Active => parent.GetComp<CompPowerTrader>()?.PowerOn ?? true;
        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            Command_Action command_Action = new Command_Action();
            command_Action.defaultLabel = "DEV: Spawn items";
            command_Action.action = delegate
            {
                SpawnItems();
            };
            yield return command_Action;
        }

        private void SpawnItems()
        {
            if (Props.spawnThings.TryRandomElement(out var result))
            {
                Thing thing = ThingMaker.MakeThing(result);
                thing.stackCount = 1;
                GenPlace.TryPlaceThing(thing, parent.Position, parent.Map, ThingPlaceMode.Near);
            }
        }

        public override void CompTickRare()
        {
            if (Active)
            {
                ticksPassed += 250;
                if (ticksPassed >= Props.spawnInterval)
                {
                    SpawnItems();
                    ticksPassed -= Props.spawnInterval;
                }
            }
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref ticksPassed, "ticksPassed", 0);
        }

        public override string CompInspectStringExtra()
        {
            if (Active)
            {
                return "NextSpawnedResourceIn".Translate() + ": " + (Props.spawnInterval - ticksPassed).ToStringTicksToPeriod();
            }

            return null;
        }
    }
}
