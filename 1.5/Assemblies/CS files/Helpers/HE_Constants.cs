using UnityEngine;
using Verse;

namespace HE_AntiReality
{
    public static class AR_Constants
    {
        // ゲーム内で使う定数
        public static readonly Color fictionDeepPurple = new Color(0.17f, 0.04f, 0.31f, 1f);
        public static readonly int OnehourTickCount = 2500;
        public static readonly int OneDayTickCount = OnehourTickCount * 24;
        public static readonly int OneSeasonTickCount = OneDayTickCount * 15;
        public static readonly int OneYearTickCount = OneSeasonTickCount * 4;
        public static readonly string debugGizmoIconPath = "UI/Commands/skill_icon_01_nobg";

        // ゲーム内で使われる文章
        // メンタルステート
        [MayTranslate]
        public static readonly string fictionStateReason = "FictionStateReason";

        // ゲーム内ヒント
        [MayTranslate]
        public static readonly string Hint_NoEquipment = "NoEquipmentFound";

        [MayTranslate]
        public static readonly string Hint_NullPawnOrEquipment = "NoPawnOrEquipment";

        [MayTranslate]
        public static readonly string Hint_NoFindOverLoadComp = "NoOverloadComponent";

        [MayTranslate]
        public static readonly string Hint_LostOfReality = "Hint_LostOfReality";

        [MayTranslate]
        public static readonly string Hint_DeleteAnchor = "Hint_DeleteAnchor";

        // Tooltip
        [MayTranslate]
        public static readonly string Tooltip_MoltenHeart_Daytofinish = "Tooltip_MoltenHeart_Daytofinish";

        [MayTranslate]
        public static readonly string Tooltip_OverloadHediff_CurrentAmount = "Tooltip_OverloadHediff_CurrentAmount";

        [MayTranslate]
        public static readonly string Tooltip_OverloadHediff_InHeal = "Tooltip_OverloadHediff_InHeal";

        [MayTranslate]
        public static readonly string Tooltip_OverloadHediff_Penalty = "Tooltip_OverloadHediff_Penalty";

        // UIパーツ
        [MayTranslate]
        public static readonly string Dialog_InputLabel = "Dialog_InputLabel";

        [MayTranslate]
        public static readonly string UI_Confirmed = "UI_Confirmed";

        [MayTranslate]
        public static readonly string UI_Cancel = "UI_Cancel";

        [MayTranslate]
        public static readonly string Error_EnterName = "Hint_RejectInput";

        [MayTranslate]
        public static readonly string Mote_ForceDeath = "Mote_ForceDeath";

        [MayTranslate]
        public static readonly string Gizmo_FESetDebugDesc = "Gizmo_FESetDebugDesc";
    }

    public static class ExposeUtility
    {
        public static string GetExposeKey<T>(T type, string custom) where T : class
        {
            return $"AR_{nameof(T)}_{custom}";
        } 
    }
}
