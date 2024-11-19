using PipeSystem;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace HE_AntiReality
{
    public class CompProperties_WarpSystem : CompProperties
    {
        public CompProperties_WarpSystem()
        {
            compClass = typeof(CompWarpSystem);
        }
    }

    public class CompWarpSystem : ThingComp
    {
        [MayTranslate]
        private readonly string GizmoLabel = "AR_WarpAnchorNameChange";
        [MayTranslate]
        private readonly string GizmoDesc = "AR_WarpAnchorDescription";
        [MayTranslate]
        private readonly string AnchorCustomLabel = "AnchorCustomLabel";

        private string customLabel;
        public string CustomLabel => customLabel;

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            WarpAnchorHelper.AddAnchor(parent);
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);
            WarpAnchorHelper.RemoveAnchor(parent);
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            yield return new Command_Action
            {
                defaultLabel = GizmoLabel.Translate(),
                defaultDesc = GizmoDesc.Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/Commands/UI_Skill_Icon_Recon"),
                action = () =>
                {
                    Find.WindowStack.Add(new Dialog_Rename(this));
                }
            };
        }

        public override string CompInspectStringExtra()
        {
            string labelString = !string.IsNullOrEmpty(customLabel)
                ? AnchorCustomLabel.Translate(customLabel)
                : null;

            string baseString = base.CompInspectStringExtra();

            if (!string.IsNullOrEmpty(labelString) && !string.IsNullOrEmpty(baseString))
            {
                return $"{labelString}\n{baseString}";
            }
            else if (!string.IsNullOrEmpty(labelString))
            {
                return labelString;
            }
            else
            {
                return baseString;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref customLabel, ExposeUtility.GetExposeKey(typeof(CompWarpSystem), nameof(customLabel)));
        }

        public void SetLabel(string newLabel)
        {
            customLabel = newLabel;
        }
    }

    public class Dialog_Rename : Window
    {
        private readonly CompWarpSystem compWarpSystem;
        private string newLabel;

        public Dialog_Rename(CompWarpSystem compWarpSystem)
        {
            this.compWarpSystem = compWarpSystem;
            newLabel = compWarpSystem.CustomLabel;
            doCloseButton = true;
            doCloseX = true;
            absorbInputAroundWindow = true;
            forcePause = true;
        }

        public override Vector2 InitialSize => new Vector2(420, 270);

        public override void DoWindowContents(Rect inRect)
        {
            Text.Font = GameFont.Small;
            Widgets.Label(new Rect(0f, 15f, inRect.width, 35f), AR_Constants.Dialog_InputLabel.Translate());
            newLabel = Widgets.TextField(new Rect(0f, 50f, inRect.width, 35f), newLabel);

            if (Widgets.ButtonText(new Rect(0f, 100f, inRect.width / 2f, 35f), AR_Constants.UI_Confirmed.Translate()))
            {
                if (string.IsNullOrWhiteSpace(newLabel))
                {
                    Messages.Message(AR_Constants.Error_EnterName.Translate(), MessageTypeDefOf.RejectInput, false);
                }
                else
                {
                    compWarpSystem.SetLabel(newLabel);
                    Close();
                }
            }
        }
    }
}
