using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class GameController : MonoBehaviour {
    private const int _floorScore = 1;
    private const int _winScore = 10;

    private int _lives = 3;
    private bool _debug = false;
    private bool _gameOver = false;


    public bool IsDebug {
        get => _debug;
    }

    public bool GameOver {
        get => _gameOver;
    }

    public void IncreaseFloors(int newFloors) {
        if (_gameOver) {
            return;
        }

        GameInfo.Score += _floorScore;

        SetFloorsText(newFloors);
        SetScoreText();

        Debug.Log("Gained score");
    }

    public void DecreaseLives() {
        --_lives;
        _lives = Mathf.Clamp(_lives, 0, _lives);

        SetLivesText();

        Debug.Log("Lost life");

        if (_lives == 0) {
            EndGame();
        }
    }

    public void WinGame() {
        GameInfo.Score += _winScore;
        SetScoreText();

        ++GameInfo.Level;

        UpdateHighScore();

        SetText(GameUObject.notificationText, "LEVEL COMPLETE");

        GameObject.Find(GlobalObject.sceneController).GetComponent<SceneController>().OnGameWon();

        Debug.Log("Game won");
    }


    private void Start() {
        SetText(GameUObject.notificationText, "");
        SetHighScoreText();
        SetScoreText();
        SetLivesText();
        SetFloorsText(0);
        SetLevelText();

        var fields = new List<(string, string)>();
        foreach (FieldInfo field in typeof(GameKey).GetFields()) {
            if (field.FieldType == typeof(KeyCode)) {
                fields.Add((field.Name, field.GetValue(null).ToString()));
            }
        }
        fields.Sort();

        string keyBindings = "";
        foreach (var tuple in fields) {
            keyBindings += tuple.Item1 + ":\t" + tuple.Item2 + '\n';
        }
        keyBindings.TrimEnd();
        SetText(GameUObject.keyBindingsText, keyBindings);
    }

    private void Update() {
        if (Input.GetKeyDown(GameKey.debug)) {
            _debug = !_debug;
            if (!_debug) {
                GameObject.Find(GameUObject.camera).GetComponent<CameraController>().Reset();
            }
            Debug.Log("Debug: " + _debug);
        }
    }


    private void EndGame() {
        string text = "GAME OVER\nScore: " + GameInfo.Score;
        SetText(GameUObject.notificationText, text);

        UpdateHighScore();

        GameInfo.Reset();

        _gameOver = true;

        Debug.Log("Game over");
    }

    private void UpdateHighScore() {
        GameInfo.HighScore = Mathf.Max(GameInfo.HighScore, GameInfo.Score);
        SetHighScoreText();
    }

    private void SetHighScoreText() {
        string text = "High Score:\t" + GameInfo.HighScore;
        SetText(GameUObject.highScoreText, text);
    }

    private void SetScoreText() {
        string text = "Score:\t" + GameInfo.Score;
        SetText(GameUObject.scoreText, text);
    }

    private void SetLivesText() {
        string text = "Lives:\t" + _lives;
        SetText(GameUObject.livesText, text);
    }

    public void SetFloorsText(int floors) {
        string text = "Floors:\t" + floors;
        SetText(GameUObject.floorsText, text);
    }

    private void SetLevelText() {
        string text = "Level " + GameInfo.GetLevelText() + " (" + GameInfo.GetFloorsText() + " floors)";
        SetText(GameUObject.levelText, text);
    }

    private void SetText(string key, string text) {
        UnityEngine.UI.Text textObj = GameObject.Find(key).GetComponent<UnityEngine.UI.Text>();
        textObj.text = text;
        textObj.enabled = text != "";
    }
}
