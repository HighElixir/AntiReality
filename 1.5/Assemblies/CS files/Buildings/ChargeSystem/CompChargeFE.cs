using PipeSystem;
using RimWorld;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.Noise;

namespace HE_AntiReality
{
    public class CompChargeFE : CompResourceTrader
    {
        private float idleConsumptionPerTickBase;

        public Comp_StoreFE StoreFE
        {
            get
            {
                Thing heldThing = HeldThing;
                return heldThing != null ? heldThing.TryGetComp<Comp_StoreFE>() : null;
            }
        }
        public Thing HeldThing
        {
            get
            {
                List<Thing> thingsAtPosition = parent.Map.thingGrid.ThingsListAt(parent.Position);
                if (Props.category != null)
                {
                    foreach (ThingDef childThingDef in Props.category.childThingDefs)
                    {
                        foreach (Thing thing in thingsAtPosition)
                        {
                            if (thing.def == childThingDef)
                            {
                                return thing;
                            }
                        }
                    }
                }
                return null;
            }
        }

        public new CompProperties_FECharge Props => (CompProperties_FECharge)props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            idleConsumptionPerTickBase = Props.idleConsumptionPerTick;
        }

        public override void CompTick()
        {
            Comp_StoreFE storeFE = StoreFE;
            if (storeFE != null)
            {
                if (Props.chargepertick > 0 && storeFE.CurrentFE < storeFE.MaxFE)
                {
                    storeFE.CurrentFE = storeFE.CurrentFE + Props.chargepertick * storeFE.ChargeFEEfficiency * Props.chargeEfficiency;
                    if (Props.idleConsumptionPerTick != idleConsumptionPerTickBase + Props.chargepertick * Props.chargeEfficiency)
                    {
                        Props.idleConsumptionPerTick = idleConsumptionPerTickBase + Props.chargepertick * Props.chargeEfficiency;

                    }
                    base.CompTick();
                }
            }
            if (Props.idleConsumptionPerTick != idleConsumptionPerTickBase)
            {
                Props.idleConsumptionPerTick = idleConsumptionPerTickBase;
            }
            base.CompTick();
        }
        public override string CompInspectStringExtra()
        {
            Comp_StoreFE storeFE = StoreFE;
            StringBuilder sb = new StringBuilder();
            if (storeFE != null)
            {
                sb.AppendLine($"蓄積中：{storeFE.parent.def.label}");
            }
            sb.AppendLine($"蓄積速度：{(Props.chargepertick * 2500).ToString("0.00")}fu/h");
            sb.AppendLine($"蓄積効率：{Props.chargeEfficiency.ToString("0.00")}");
            return sb.ToString() + base.CompInspectStringExtra();
        }
    }
}
