using UnityEngine;


public static class GameScene {
    public static readonly string play = "Play";
}

public static class GameUObject {
    public static readonly string game = "Game Controller";
    public static readonly string camera = "Main Camera";

    public static readonly string ground = "Ground";
    public static readonly string blockSpawner = "Block Spawner";
    public static readonly string block = "Block";

    public static readonly string highScoreText = "High Score Text";
    public static readonly string scoreText = "Score Text";
    public static readonly string livesText = "Lives Text";
    public static readonly string keyBindingsText = "Key Bindings Text";
    public static readonly string notificationText = "Notification Text";
}

public static class GameKey {
    public static readonly KeyCode gameReset = KeyCode.F5;
    public static readonly KeyCode gameMenu = KeyCode.Escape;

    public static readonly string cameraUp = "up";
    public static readonly string cameraDown = "down";
    public static readonly string cameraForward = "up";
    public static readonly string cameraBack = "down";
    public static readonly string cameraRotateLeft = "left";
    public static readonly string cameraRotateRight = "right";
    public static readonly string cameraModifier = "left shift";
    public static readonly KeyCode cameraReset = KeyCode.R;
    public static readonly KeyCode cameraFree = KeyCode.F;

    public static readonly KeyCode debug = KeyCode.D;
    public static readonly KeyCode pause = KeyCode.P;
    public static readonly KeyCode blockSpawn = KeyCode.Space;
}
