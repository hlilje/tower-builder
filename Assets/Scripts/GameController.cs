using UnityEngine;

public class GameController : MonoBehaviour {
    private int _lives = 3;
    private int _score = 0;
    private bool _debug = false;

    private void Update() {
        if (Input.GetKeyDown(Key.debug)) {
            _debug = !_debug;
            if (!_debug) {
                GameObject.Find(Object.camera).GetComponent<CameraController>().Reset();
            }
            Debug.Log("Debug: " + _debug);
        }
    }

    public bool IsDebug() {
        return _debug;
    }

    public void IncreaseScore() {
        ++_score;
        Debug.Log("Gained score");
    }

    public void DecreaseScore() {
        --_score;
        --_lives;
        Debug.Log("Lost score");

        if (_lives == 0) {
            Debug.Log("GAME OVER");
            Debug.Log("Score: " + _score);
        }
    }
}
