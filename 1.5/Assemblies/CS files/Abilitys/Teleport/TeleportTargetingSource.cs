using System;
using UnityEngine;
using Verse;
using RimWorld;

namespace HE_AntiReality
{
    public class TeleportTargetingSource : ITargetingSource
    {
        private Pawn casterPawn; // テレポートを使用するPawn
        private float maxDistance; // テレポートの最大距離

        public TeleportTargetingSource(Pawn pawn, float maxDistance)
        {
            this.casterPawn = pawn;
            this.maxDistance = maxDistance; // 距離を指定してターゲットを決定
        }

        // プロパティ。UIでターゲットが収集できるか
        public bool IsCollect { get; set; }
        public bool CasterIsPawn => true; // 常にPawnであることを前提としている

        public bool IsMeleeAttack => false; // テレポートは攻撃ではないのでfalse

        public bool Targetable => true; // ターゲットできることを示す

        public bool MultiSelect => false; // 複数選択はしない

        public bool HidePawnTooltips => false; // ツールチップを隠すかどうか（今回は隠さない）

        public Thing Caster => casterPawn; // キャスター（Pawn）を返す

        public Pawn CasterPawn => casterPawn; // CasterとしてのPawnを返す

        public Verb GetVerb => null; // テレポートは攻撃の一部ではないため、nullを返す

        public Texture2D UIIcon => ContentFinder<Texture2D>.Get("UI/Commands/Teleport");
        // テレポート用のUIアイコン。ファイルパスを確認する必要があるかも

        public TargetingParameters targetParams => TargetingParameters_Extensions.ForTeleport(casterPawn, maxDistance);
        // 独自のTargetingParametersを使用して、テレポート範囲を計算

        public ITargetingSource DestinationSelector => null; // 特に別のターゲットソースはなし

        public bool CanHitTarget(LocalTargetInfo target)
        {
            // 距離と視界の判定
            return target.Cell.IsValid // ターゲット位置が有効か
                && casterPawn.Position.InHorDistOf(target.Cell, maxDistance) // 最大距離内か
                && GenSight.LineOfSight(casterPawn.Position, target.Cell, casterPawn.Map); // 視線が通るか
        }

        public bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
        {
            if (CanHitTarget(target)) // ターゲットが有効であれば
            {
                return true;
            }

            if (showMessages) // ターゲットが無効なら、範囲外メッセージを表示
            {
                Messages.Message("範囲外", MessageTypeDefOf.RejectInput, false);
            }
            return false;
        }

        public void DrawHighlight(LocalTargetInfo target)
        {
            // 最大距離の範囲を描画（ビジュアル的なフィードバック）
            GenDraw.DrawRadiusRing(casterPawn.Position, maxDistance);
        }

        public void OrderForceTarget(LocalTargetInfo target)
        {
            if (ValidateTarget(target)) // 有効なターゲットなら
            {
                casterPawn.Position = target.Cell; // ターゲット地点にキャラクターを移動
                casterPawn.Notify_Teleported(true, true); // テレポート通知を送信
                IsCollect = true; // ターゲットが収集されたことを記録
            }
        }

        public void OnGUI(LocalTargetInfo target)
        {
            DrawHighlight(target); // ターゲット選択時に範囲を強調
            // 必要なら、ここで追加のGUI要素を描画できる
        }
    }
}
