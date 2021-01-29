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


    public static string GetTarget() {
        return Level < _FloorsPerLevel.Length ? _FloorsPerLevel[Level].ToString() : "Inf";
    }

    public static bool ShouldSpawnRoof(int floor) {
        if (Level >= _FloorsPerLevel.Length) {
            return false;
        }

        return floor >= _FloorsPerLevel[Level];
    }

    public static void Reset() {
        Level = 0;
        Score = 0;
    }
}
