using UnityEngine;

public class CameraController : MonoBehaviour {
    private const float _baseMoveSpeed = 10.0f;
    private const float _rotationSpeed = 150.0f;
    private const float _distanceSpeedFactor = 0.1f;

    private const float _groundOffset = 0.2f;

    private Vector3 _position;
    private Vector3 _blockSpawnerPosition;

    private bool _free = false;

    private void Start() {
        _position = transform.position;
        _blockSpawnerPosition = GameObject.Find(Object.blockSpawner).transform.position;
    }

    private void Update() {
        UpdatePosition();

        if (Input.GetKeyDown(Key.cameraReset)) {
            Reset();
        }

        if (Input.GetKeyDown(Key.cameraFree)) {
            _free = !_free;
        }
    }

    private void UpdatePosition() {
        bool debug = GameObject.Find(Object.game).GetComponent<GameController>().IsDebug();
        if (!debug) {
            return;
        }

        float moveSpeed = CalcMoveSpeed();
        float moveDelta = Time.deltaTime * moveSpeed;
        float rotationDelta = Time.deltaTime * _rotationSpeed;

        if (Input.GetKey(Key.cameraRotateLeft)) {
            transform.RotateAround(_blockSpawnerPosition, Vector3.up, rotationDelta);
        } else if (Input.GetKey(Key.cameraRotateRight)) {
            transform.RotateAround(_blockSpawnerPosition, Vector3.up, -rotationDelta);
        } else {
            Vector3 position = transform.position;

            if (Input.GetKey(Key.cameraModifier)) {
                if (Input.GetKey(Key.cameraUp)) {
                    position.y += moveDelta;
                } else if (Input.GetKey(Key.cameraDown)) {
                    GameObject ground = GameObject.Find(Object.ground);
                    position.y -= moveDelta;
                    float limit = ground.transform.position.y + _groundOffset;
                    position.y = Mathf.Max(position.y, _groundOffset);
                }
            } else {
                Vector3 moveDeltaVec = CalcDirectionVector(position, true) * moveDelta;
                if (Input.GetKey(Key.cameraForward)) {
                    position -= moveDeltaVec;
                } else if (Input.GetKey(Key.cameraBack)) {
                    position += moveDeltaVec;
                }
            }

            transform.position = position;
        }
    }

    private float CalcMoveSpeed() {
        float distance = CalcDirectionVector(transform.position, false).magnitude;
        return Mathf.Max(_baseMoveSpeed, _baseMoveSpeed * distance * _distanceSpeedFactor);
    }

    private Vector3 CalcDirectionVector(Vector3 position, bool normalise) {
        Vector3 targetPos = _blockSpawnerPosition;
        targetPos.y = position.y;
        return normalise ? (position - targetPos).normalized : (position - targetPos);
    }

    public void Reset() {
        transform.position = _position;
        transform.rotation = Quaternion.identity;
        _free = false;
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
