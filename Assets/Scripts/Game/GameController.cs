using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class GameController : MonoBehaviour {
    private int _lives = 3;
    private int _score = 0;
    private bool _debug = false;
    private bool _gameOver = false;


    public bool IsDebug {
        get => _debug;
    }

    public bool GameOver {
        get => _gameOver;
    }

    public void IncreaseScore() {
        if (_gameOver) {
            return;
        }

        ++_score;

        SetScoreText();

        Debug.Log("Gained score");
    }

    public void DecreaseLives() {
        --_lives;
        _lives = Mathf.Clamp(_lives, 0, _lives);

        SetLivesText();

        Debug.Log("Lost life");

        if (_lives == 0) {
            string text = "GAME OVER\nScore: " + _score;
            SetText(GameUObject.notificationText, text);
            _gameOver = true;
        }
    }


    private void Start() {
        SetText(GameUObject.notificationText, "");
        SetScoreText();
        SetLivesText();

        var fields = new List<(string, string)>();
        foreach (FieldInfo field in typeof(GameKey).GetFields()) {
            if (field.FieldType == typeof(KeyCode)) {
                fields.Add((field.Name, field.GetValue(null).ToString()));
            }
        }
        fields.Sort();

        string keyBindings = "";
        foreach (var tuple in fields) {
            keyBindings += tuple.Item1 + ": " + tuple.Item2 + '\n';
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


    private void SetScoreText() {
        string text = "Score: " + _score;
        SetText(GameUObject.scoreText, text);
    }

    private void SetLivesText() {
        string text = "Lives: " + _lives;
        SetText(GameUObject.livesText, text);
    }

    private void SetText(string key, string text) {
        UnityEngine.UI.Text textObj = GameObject.Find(key).GetComponent<UnityEngine.UI.Text>();
        textObj.text = text;
        textObj.enabled = text != "";
    }
}
