using System;

namespace Zarus.Systems
{
    public enum GameOutcomeKind
    {
        None,
        Victory,
        Defeat
    }

    /// <summary>
    /// Tracks the last completed run so the End screen can surface stats.
    /// </summary>
    public static class GameOutcomeState
    {
        public static GameOutcomeKind LastOutcome { get; private set; } = GameOutcomeKind.None;
        public static float LastCureProgress01 { get; private set; }
        public static int LastTotalOutposts { get; private set; }
        public static int LastActiveOutposts { get; private set; }
        public static int LastZarBalance { get; private set; }
        public static int LastDayIndex { get; private set; }
        public static int LastSavedProvinces { get; private set; }
        public static int LastFullyInfectedProvinces { get; private set; }

        public static void SetOutcome(GameOutcomeKind outcome, GlobalCureState globalState, int dayIndex, int savedProvinces, int fullyInfectedProvinces)
        {
            LastOutcome = outcome;
            LastDayIndex = Math.Max(1, dayIndex);
            LastSavedProvinces = Math.Max(0, savedProvinces);
            LastFullyInfectedProvinces = Math.Max(0, fullyInfectedProvinces);

            if (globalState != null)
            {
                LastCureProgress01 = Clamp01(globalState.CureProgress01);
                LastTotalOutposts = Math.Max(0, globalState.TotalOutpostCount);
                LastActiveOutposts = Math.Max(0, globalState.ActiveOutpostCount);
                LastZarBalance = globalState.ZarBalance;
            }
            else
            {
                LastCureProgress01 = 0f;
                LastTotalOutposts = 0;
                LastActiveOutposts = 0;
                LastZarBalance = 0;
            }
        }

        public static void Reset()
        {
            LastOutcome = GameOutcomeKind.None;
            LastCureProgress01 = 0f;
            LastTotalOutposts = 0;
            LastActiveOutposts = 0;
            LastZarBalance = 0;
            LastDayIndex = 0;
            LastSavedProvinces = 0;
            LastFullyInfectedProvinces = 0;
        }

        private static float Clamp01(float value)
        {
            if (value < 0f)
            {
                return 0f;
            }

            return value > 1f ? 1f : value;
        }
    }
}
