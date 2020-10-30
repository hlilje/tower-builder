using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public class GameController : MonoBehaviour {
    private int _lives = 3;
    private int _score = 0;
    private bool _debug = false;


    public bool IsDebug() {
        return _debug;
    }

    public void IncreaseScore() {
        ++_score;

        SetScoreText();

        Debug.Log("Gained score");
    }

    public void DecreaseScore() {
        --_score;
        --_lives;

        SetScoreText();

        Debug.Log("Lost score");

        if (_lives == 0) {
            SetText(Object.notificationText, "GAME OVER");
        }
    }


    private void Start() {
        SetText(Object.notificationText, "");

        var fields = new List<(string, string)>();
        foreach (FieldInfo field in typeof(Key).GetFields()) {
            if (field.FieldType == typeof(KeyCode)) {
                fields.Add((field.Name, field.GetValue(null).ToString()));
            }
        }
        fields.Sort();

        string keyBindings = "";
        foreach (var tuple in fields) {
            keyBindings += tuple.Item1 + ": " + tuple.Item2 + '\n';
        }
        SetText(Object.keyBindingsText, keyBindings);
    }

    private void Update() {
        if (Input.GetKeyDown(Key.debug)) {
            _debug = !_debug;
            if (!_debug) {
                GameObject.Find(Object.camera).GetComponent<CameraController>().Reset();
            }
            Debug.Log("Debug: " + _debug);
        }
    }


    private void SetScoreText() {
        string text = "Score: " + _score;
        SetText(Object.scoreText, text);
    }

    private void SetText(string key, string text) {
        UnityEngine.UI.Text textObj = GameObject.Find(key).GetComponent<UnityEngine.UI.Text>();
        textObj.text = text;
        textObj.enabled = text != "";
    }
}
