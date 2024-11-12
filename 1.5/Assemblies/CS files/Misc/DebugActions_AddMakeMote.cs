using RimWorld;
using Verse;
using LudeonTK;

namespace HE_AntiReality
{
    [StaticConstructorOnStartup]
    public static class DebugActions_AddMakeMote
    {
        [DebugAction("HE AntiReality", "MakeMoteDescription", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void ExecuteMakeMote()
        {
            // プレイヤーにポーンを選択させる
            Pawn selectedPawn = Find.Selector.SingleSelectedThing as Pawn;
            if (selectedPawn != null)
            {
                LostofRealityUtility.MakeMote(selectedPawn);
            }
            else
            {
                Messages.Message("No Pawn selected. Please select a Pawn first.", MessageTypeDefOf.RejectInput, false);
            }
        }
    }
}
