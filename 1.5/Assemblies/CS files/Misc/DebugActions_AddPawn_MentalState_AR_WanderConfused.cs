using RimWorld;
using Verse;
using LudeonTK;
using System.Linq;
using HighElixir.Utility.RimWorld;

namespace HE_AntiReality
{
    [StaticConstructorOnStartup]
    public static class DebugActions_AddPawn_MentalState_AR_WanderConfused
    {
        public static bool forceConfused = false;

        [DebugAction("HE AntiReality", "Add MentalState_AR_WanderConfused", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
        private static void ExecuteMakeMote()
        {
            var mouseCell = UI.MouseCell();
            var map = Find.CurrentMap;
            var thing = map.thingGrid.ThingsAt(mouseCell).FirstOrDefault(t => t is Pawn) as Pawn;

            if (thing != null)
            {
                var pawn = thing as Pawn;
                if (pawn != null)
                {
                    // 精神状態を付与
                    bool started = pawn.mindState.mentalStateHandler.TryStartMentalState(AR_MentalStateDefOf.AR_WanderConfused, "Debug Action");

                    if (started)
                    {
                        // モートを表示
                        MoteMaker.ThrowText(HE_RimUtility.MakeMotePosition(pawn), pawn.Map, AR_Constants.fictionStateReason.Translate(), AR_Constants.fictionDeepPurple);

                        // メッセージを表示
                        Messages.Message($"{pawn.Name} に『虚構の浸食』精神状態を付与しました。", MessageTypeDefOf.TaskCompletion);
                    }
                    else
                    {
                        Messages.Message($"{pawn.Name} はすでに精神状態にあります。", MessageTypeDefOf.RejectInput);
                    }
                }
            }
        }
    }
}
