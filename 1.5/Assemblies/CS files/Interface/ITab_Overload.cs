using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace HE_AntiReality
{
    public class ITab_Overload : ITab
    {
        private const float Margin = 10f;
        private Vector2 scrollPosition;

        private Pawn PawnForOverload
        {
            get
            {
                if (SelPawn != null)
                {
                    return SelPawn;
                }
                Corpse corpse;
                if ((corpse = (SelThing as Corpse)) != null)
                {
                    return corpse.InnerPawn;
                }
                return null;
            }
        }
        public ITab_Overload()
        {
            size = new Vector2(400f, 300f); // タブのサイズを調整
            labelKey = "AR_TabOverload"; // 翻訳キーを適切に設定
        }

        protected override void FillTab()
        {
            bool flg = false;
            Pawn pawn = PawnForOverload;
            if (pawn == null || pawn.equipment == null)
            {
                Rect rect = new Rect(Margin, Margin, size.x - Margin * 2, size.y - Margin * 2);
                Widgets.Label(rect, AR_Constants.Hint_NullPawnOrEquipment.Translate()); // 翻訳キーを使用
                return;
            }

            List<ThingWithComps> allEquipment = pawn.equipment.AllEquipmentListForReading;
            if (allEquipment == null || allEquipment.Count == 0)
            {
                Rect rect = new Rect(Margin, Margin, size.x - Margin * 2, size.y - Margin * 2);
                Widgets.Label(rect, AR_Constants.Hint_NoEquipment.Translate()); // 翻訳キーを使用
                return;
            }

            float lineHeight = Text.LineHeight + 5f;
            float contentHeight = allEquipment.Count * (lineHeight * 6); // コンテンツの高さを調整

            Rect outRect = new Rect(Margin, Margin, size.x - Margin * 2, size.y - Margin * 2);
            Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, contentHeight);

            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);

            float curY = 0f;

            foreach (var item in allEquipment)
            {
                if (item == null) continue;

                var overloadComp = item.GetComp<Comp_Overload>();
                if (overloadComp != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("EquipmentLabel".Translate(item.Label));
                    sb.AppendLine("OverloadValue".Translate(overloadComp.CurrentOverload.ToString("0.00"), overloadComp.OverloadLimit));
                    sb.AppendLine("OverloadHealRate".Translate(overloadComp.OverloadHealAmount.ToString("0.00")));
                    sb.AppendLine("CooldownTime".Translate((overloadComp.WaittoHealTime / 60f).ToString("0.00")));
                    sb.AppendLine("PenaltyLevel".Translate(overloadComp.GetPenaltyLevel()));

                    Rect labelRect = new Rect(0f, curY, viewRect.width, lineHeight * 5);
                    Widgets.Label(labelRect, sb.ToString());

                    // オーバーロードバーを描画
                    Rect barRect = new Rect(0f, curY + lineHeight * 5, viewRect.width, lineHeight);
                    Widgets.FillableBar(barRect, overloadComp.CurrentOverload / overloadComp.OverloadLimit);

                    curY += lineHeight * 6; // 次のアイテムに位置を移動
                    flg = true;
                }                
            }
            if (!flg)
            {
                Rect labelRect = new Rect(0f, curY, viewRect.width, lineHeight);
                Widgets.Label(labelRect, AR_Constants.Hint_NoFindOverLoadComp.Translate());
                curY += lineHeight;
            }
            Widgets.EndScrollView();
        }
    }
}
