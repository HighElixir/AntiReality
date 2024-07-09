using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse;

namespace HE_AntiReality
{
    public class HediffComp_TransportGizmo : HediffComp
    {
        public HediffCompProperties_TransportGizmo Props
        {
            get
            {
                return (HediffCompProperties_TransportGizmo)props;
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            yield return new Command_Action
            {
                defaultLabel = "瞬間移動",
                defaultDesc = "視界の通る範囲内に瞬間移動します。",
                icon = ContentFinder<Texture2D>.Get("UI/Commands/Teleport"),
                action = () =>
                {
                    Find.Targeter.BeginTargeting(TargetingParameters_Extensions.ForTeleport(), (LocalTargetInfo target) =>
                    {
                        if (target.IsValid)
                        {
                            this.parent.pawn.Position = target.Cell;
                            this.parent.pawn.Notify_Teleported(true, true);
                        }
                    });
                }
            };
        }
    }
}
