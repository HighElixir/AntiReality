using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace HE_AntiReality
{
    public abstract class Comp_OverloadUserBase : ThingComp
    {
        protected Comp_Overload overload;

        protected virtual bool DoesHaveReference => overload != null; 

        public override void Initialize(CompProperties _)
        {
            base.Initialize(_);
            overload = parent.TryGetComp<Comp_Overload>();
            if (overload == null)
            {
                Log.Error($"Don't assigned 'Comp_Overload' on {parent.def.defName}");
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            if (!parent.IsHashIntervalTick(10)) return;
            if (!DoesHaveReference)
            {
                return;
            }

            if (overload.IsLimit())
            {
                HandleOverloadLimitExceededTick(overload.GetPenaltyLevel());
            }
        }

        public virtual void AddOverload(float amount)
        {
            if (!DoesHaveReference)
                return;
            overload.AddOverload(amount);
            if (overload.IsLimit())
            {
                MoltenReality(overload.GetPenaltyLevel());
            }
        }
        public virtual void HandleOverloadLimitExceededTick(int level)
        {
        }

        public virtual void MoltenReality(int level)
        {
        }

        public override void PostExposeData()
        {
            base.PostExposeData();

            Scribe_References.Look(ref overload, ExposeUtility.GetExposeKey(typeof(Comp_OverloadUserBase), nameof(overload)));
        }

    }
}
