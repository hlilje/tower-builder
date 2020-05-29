using UnityEngine;

public class CameraController : MonoBehaviour {
    private const float _speed = 10.0f;
    private const float _groundOffset = 0.2f;

    private Vector3 _position;
    private bool _free = false;

    private void Start() {
        _position = transform.position;
    }

    private void Update() {
        UpdatePosition();

        if (Input.GetKeyDown(Key.cameraReset)) {
            transform.position = _position;
            _free = false;
        }

        if (Input.GetKeyDown(Key.cameraFree)) {
            _free = !_free;
        }
    }

    private void UpdatePosition() {
        Vector3 position = transform.position;
        float delta = Time.deltaTime * _speed;

        if (Input.GetKey(Key.cameraModifier)) {
            if (Input.GetKey(Key.cameraUp)) {
                position.y += delta;
            } else if (Input.GetKey(Key.cameraDown)) {
                GameObject ground = GameObject.Find(Object.ground);
                position.y -= delta;
                float limit = ground.transform.position.y + _groundOffset;
                position.y = Mathf.Max(position.y, _groundOffset);
            }
        } else {
            if (Input.GetKey(Key.cameraForward)) {
                position.z += delta;
            } else if (Input.GetKey(Key.cameraBack)) {
                position.z -= delta;
            }
        }

        transform.position = position;
    }

    public void OnBlockSpawned(float blockHeight) {
        _position.y += blockHeight;

        if (!_free) {
            Vector3 position = transform.position;
            position.y += blockHeight;
            transform.position = position;
        }
    }
}
