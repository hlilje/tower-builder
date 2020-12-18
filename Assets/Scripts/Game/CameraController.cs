using UnityEngine;


public class CameraController : MonoBehaviour {
    private const float _baseMoveSpeed = 10.0f;
    private const float _rotationSpeed = 150.0f;
    private const float _distanceSpeedFactor = 0.1f;

    private const float _groundOffset = 0.2f;

    private Vector3 _position;
    private Vector3 _blockSpawnerPosition;

    private bool _free = false;


    public void Reset() {
        transform.position = _position;
        transform.rotation = Quaternion.identity;
        _free = false;
    }

    public void IncreaseHeight(float height) {
        _position.y += height;

        if (!_free) {
            Vector3 position = transform.position;
            position.y += height;
            transform.position = position;
        }
    }


    private void Start() {
        _position = transform.position;
        _blockSpawnerPosition = GameObject.Find(GameUObject.blockSpawner).transform.position;
    }

    private void Update() {
        UpdatePosition();

        if (Input.GetKeyDown(GameKey.cameraReset)) {
            Reset();
        }

        if (Input.GetKeyDown(GameKey.cameraFree)) {
            _free = !_free;
        }
    }


    private void UpdatePosition() {
        bool debug = GameObject.Find(GameUObject.game).GetComponent<GameController>().IsDebug;
        if (!debug) {
            return;
        }

        float moveSpeed = CalcMoveSpeed();
        float moveDelta = Time.deltaTime * moveSpeed;
        float rotationDelta = Time.deltaTime * _rotationSpeed;

        if (Input.GetKey(GameKey.cameraRotateLeft)) {
            transform.RotateAround(_blockSpawnerPosition, Vector3.up, rotationDelta);
        } else if (Input.GetKey(GameKey.cameraRotateRight)) {
            transform.RotateAround(_blockSpawnerPosition, Vector3.up, -rotationDelta);
        } else {
            Vector3 position = transform.position;

            if (Input.GetKey(GameKey.cameraModifier)) {
                if (Input.GetKey(GameKey.cameraUp)) {
                    position.y += moveDelta;
                } else if (Input.GetKey(GameKey.cameraDown)) {
                    GameObject ground = GameObject.Find(GameUObject.ground);
                    position.y -= moveDelta;
                    float limit = ground.transform.position.y + _groundOffset;
                    position.y = Mathf.Max(position.y, _groundOffset);
                }
            } else {
                Vector3 moveDeltaVec = CalcDirectionVector(position, true) * moveDelta;
                if (Input.GetKey(GameKey.cameraForward)) {
                    position -= moveDeltaVec;
                } else if (Input.GetKey(GameKey.cameraBack)) {
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
}
