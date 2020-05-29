using UnityEngine;

public static class Object {
    public static readonly string camera = "Main Camera";
    public static readonly string ground = "Ground";
    public static readonly string block = "Block";
}

public static class Key {
    public static readonly string cameraUp = "up";
    public static readonly string cameraDown = "down";
    public static readonly string cameraForward = "up";
    public static readonly string cameraBack = "down";
    public static readonly string cameraModifier = "left shift";
    public static readonly KeyCode cameraReset = KeyCode.R;
    public static readonly KeyCode cameraFree = KeyCode.F;

    public static readonly KeyCode blockSpawn = KeyCode.Q;
}
