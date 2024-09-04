using PipeSystem;
using RimWorld;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
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
        private CompResourceTrader trader;
        private string custamLabel;
        public string CustamLabel { get { return custamLabel; } set { custamLabel = value; } }
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            trader = parent.GetComp<CompResourceTrader>();
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            Command_Action command = new Command_Action
            {
                defaultLabel = "名前変更",
                defaultDesc = "このアンカーの名前を変更する。",
                icon = ContentFinder<Texture2D>.Get("UI/Commands/UI_Skill_Icon_Recon"),
                action = () =>
                {
                    Find.WindowStack.Add(new Dialog_Rename(this));
                }
            };
            yield return command;
        }

        public override string CompInspectStringExtra()
        {
            if (custamLabel != null)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine($"カスタムネーム:{custamLabel}");
                return sb.ToString().TrimEnd();
            }
            return base.CompInspectStringExtra();
        }
    }

    public class Dialog_Rename : Window
    {
        private readonly CompWarpSystem compWarpSystem;
        private string newLabel;

        public Dialog_Rename(CompWarpSystem compWarpSystem)
        {
            this.compWarpSystem = compWarpSystem;
            newLabel = compWarpSystem.CustamLabel;
            this.doCloseButton = true;
            this.doCloseX = true;
            this.absorbInputAroundWindow = true;
            this.forcePause = true;
        }

        public override Vector2 InitialSize => new Vector2(300f, 300f);

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;
            Widgets.Label(new Rect(0f, 15f, inRect.width, 35f), "新しいラベルを入力:");
            newLabel = Widgets.TextField(new Rect(0f, 50f, inRect.width, 35f), newLabel);

            if (Widgets.ButtonText(new Rect(0f, 100f, inRect.width / 2f, 35f), "保存"))
            {
                compWarpSystem.CustamLabel = newLabel;
                Close();
            }
            if (Widgets.ButtonText(new Rect(inRect.width / 2f, 100f, inRect.width / 2f, 35f), "キャンセル"))
            {
                Close();
            }
        }
    }

}
