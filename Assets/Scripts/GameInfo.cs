public static class GameInfo {
    public static int Level { get; set; } = 0;
    public static int Score { get; set; } = 0;
    public static int HighScore { get; set; } = 0;

    private static int[] _FloorsPerLevel = {
        5,
        10,
        15,
        20,
        30,
        50,
        100
    };


    public static string GetLevelText() {
        int numLevels = _FloorsPerLevel.Length;
        return Level < numLevels ? (Level + 1).ToString() + '/' + numLevels : "Infinite";
    }

    public static string GetFloorsText() {
        return Level < _FloorsPerLevel.Length ? _FloorsPerLevel[Level].ToString() : "Infinite";
    }

    public static bool ShouldSpawnRoof(int floors) {
        if (Level >= _FloorsPerLevel.Length) {
            return false;
        }

        return floors >= _FloorsPerLevel[Level];
    }

    public static void Reset() {
        Level = 0;
        Score = 0;
    }
}
