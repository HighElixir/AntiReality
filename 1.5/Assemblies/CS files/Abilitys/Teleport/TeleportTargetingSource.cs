using System;
using UnityEngine;
using Verse;
using RimWorld;

namespace HE_AntiReality
{
    public class TeleportTargetingSource : ITargetingSource
    {
        private Pawn casterPawn;
        private float maxDistance;

        public TeleportTargetingSource(Pawn pawn, float maxDistance)
        {
            this.casterPawn = pawn;
            this.maxDistance = maxDistance;
        }

        public bool IsCollect { get; set; }
        public bool CasterIsPawn => true;

        public bool IsMeleeAttack => false;

        public bool Targetable => true;

        public bool MultiSelect => false;

        public bool HidePawnTooltips => false;

        public Thing Caster => casterPawn;

        public Pawn CasterPawn => casterPawn;

        public Verb GetVerb => null; // テレポートは直接の攻撃動作ではないため、null

        public Texture2D UIIcon => ContentFinder<Texture2D>.Get("UI/Commands/Teleport");

        public TargetingParameters targetParams => TargetingParameters_Extensions.ForTeleport(casterPawn, maxDistance);

        public ITargetingSource DestinationSelector => null;

        public bool CanHitTarget(LocalTargetInfo target)
        {
            
            // 距離と視界の判定
            return target.Cell.IsValid
                && casterPawn.Position.InHorDistOf(target.Cell, maxDistance)
                && GenSight.LineOfSight(casterPawn.Position, target.Cell, casterPawn.Map);
        }

        public bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            if (CanHitTarget(target))
            {
                return true;
            }

            if (showMessages)
            {
                Messages.Message("範囲外", MessageTypeDefOf.RejectInput, false);
            }
            return false;
        }

        public void DrawHighlight(LocalTargetInfo target)
        {
            GenDraw.DrawRadiusRing(casterPawn.Position, maxDistance);
        }

        public void OrderForceTarget(LocalTargetInfo target)
        {
            if (ValidateTarget(target))
            {
                casterPawn.Position = target.Cell;
                casterPawn.Notify_Teleported(true, true);
                IsCollect = true;
            }
        }

        public void OnGUI(LocalTargetInfo target)
        {
            DrawHighlight(target);
            // 必要に応じて、ターゲット選択時にGUIで何かを描画する場合に実装
        }
    }
}
