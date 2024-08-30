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
        private int lastUsedTick = -99999; // 最後にテレポートを使用したゲーム内時間（Tick）
        private const int CooldownTicks = 6000; // クールダウンの時間（600 ticks = 10秒）

        public HediffCompProperties_TransportGizmo Props
        {
            get
            {
                return (HediffCompProperties_TransportGizmo)props;
            }
        }

        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            Command_Action teleportCommand = new Command_Action
            {
                defaultLabel = "座標替え",
                defaultDesc = "視界の通る範囲内に座標を変える",
                icon = ContentFinder<Texture2D>.Get("UI/Commands/Teleport"),
                action = () =>
                {
                    int currentTick = Find.TickManager.TicksGame; // 現在のゲーム内時間（Tick）

                    if (currentTick - lastUsedTick < CooldownTicks)
                    {
                        Messages.Message("クールダウン中。あと " + ((CooldownTicks - (currentTick - lastUsedTick)) / 60f).ToString("0.0") + " 秒", MessageTypeDefOf.RejectInput, false);
                    }
                    else
                    {
                        Find.Targeter.BeginTargeting(TargetingParameters_Extensions.ForTeleport(parent.pawn, 20f), (LocalTargetInfo target) =>
                        {
                            if (target.IsValid)
                            {
                                this.parent.pawn.Position = target.Cell;
                                this.parent.pawn.Notify_Teleported(true, true);
                                lastUsedTick = currentTick; // 使用時間を更新
                            }
                        });
                    }
                }
            };

            // クールダウン中は無効化
            if (Find.TickManager.TicksGame - lastUsedTick < CooldownTicks)
            {
                teleportCommand.Disable("クールダウン中です");
            }

            yield return teleportCommand;
        }
    }
}

