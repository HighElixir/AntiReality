using UnityEngine;
using Verse;

namespace AntiReality
{
    public static class Constants
    {
        // ゲーム内で使う定数
        public static readonly Color fictionDeepPurple = new Color(0.17f, 0.04f, 0.31f, 1f);
        public static readonly int OnehourTickCount = 2500;
        public static readonly int OneDayTickCount = OnehourTickCount * 24;
        public static readonly int OneSeasonTickCount = OneDayTickCount * 15;
        public static readonly int OneYearTickCount = OneSeasonTickCount * 4;

        [MayTranslate]
        public static readonly string Hint_LostOfReality = "Hint_LostOfReality";
    }

    public static class ExposeUtility
    {
        public static string GetExposeKey<T>(T type, string custom) where T : class
        {
            return $"AR_{nameof(T)}_{custom}";
        } 
    }
}
