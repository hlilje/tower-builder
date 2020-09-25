using UnityEngine;

public class GameController : MonoBehaviour {
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
}
