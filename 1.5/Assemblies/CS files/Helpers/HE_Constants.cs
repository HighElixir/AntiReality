using UnityEngine;
using Verse;

namespace HE_AntiReality
{
    public static class HE_Constants
    {
        // ゲーム内で使う定数
        public static Color fictionDeepPurple = new Color(0.17f, 0.04f, 0.31f, 1f);
        public static int OnehourTickCount = 2500;
        public static int OneDayTickCount = OnehourTickCount * 24;
        public static int OneSeasonTickCount = OneDayTickCount * 15;
        public static int OneYearTickCount = OneSeasonTickCount * 4;
        public static string debugGizmoIconPath = "UI/Commands/skill_icon_01_nobg";

        // ゲーム内で使われる文章
        // メンタルステート
        [MayTranslate]
        public static string fictionStateReason = "FictionStateReason";

        // ゲーム内ヒント
        [MayTranslate]
        public static string Hint_NoEquipment = "NoEquipmentFound";

        [MayTranslate]
        public static string Hint_NullPawnOrEquipment = "NoPawnOrEquipment";

        [MayTranslate]
        public static string Hint_NoFindOverLoadComp = "NoOverloadComponent";

        [MayTranslate]
        public static string Hint_LostOfReality = "Hint_LostOfReality";

        [MayTranslate]
        public static string Hint_DeleteAnchor = "Hint_DeleteAnchor";

        // Tooltip
        [MayTranslate]
        public static string Tooltip_MoltenHeart_Daytofinish = "Tooltip_MoltenHeart_Daytofinish";

        [MayTranslate]
        public static string Tooltip_OverloadHediff_CurrentAmount = "Tooltip_OverloadHediff_CurrentAmount";

        [MayTranslate]
        public static string Tooltip_OverloadHediff_InHeal = "Tooltip_OverloadHediff_InHeal";

        [MayTranslate]
        public static string Tooltip_OverloadHediff_Penalty = "Tooltip_OverloadHediff_Penalty";

        // UIパーツ
        [MayTranslate]
        public static string Dialog_InputLabel = "Dialog_InputLabel";

        [MayTranslate]
        public static string UI_Confirmed = "UI_Confirmed";

        [MayTranslate]
        public static string UI_Cancel = "UI_Cancel";

        [MayTranslate]
        public static string Error_EnterName = "Hint_RejectInput";
    }

    public static class ExposeUtility
    {
        public static string GetExposeKey<T>(T type, string custom) where T : class
        {
            return $"HE_{nameof(T)}_{custom}";
        } 
    }
}
