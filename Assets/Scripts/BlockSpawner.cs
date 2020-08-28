using UnityEngine;

public class BlockSpawner : MonoBehaviour {
    public GameObject prefab;

    private const float _speed = 2.0f;
    private const int _sideLimits = 2;

    private float _blockHeight;
    private int _direction = 1;
    private bool _paused = false;

    private void Start() {
        _blockHeight = prefab.GetComponent<Renderer>().bounds.size.x;

        SpawnParentBlock();
    }

    private void Update() {
        if (!_paused) {
            UpdatePosition();
        }

        if (Input.GetKeyDown(Key.pause)) {
            _paused = !_paused;
        }
        if (Input.GetKeyDown(Key.blockSpawn)) {
            SpawnBlock();
        }
    }

    private void UpdatePosition() {
        Vector3 position = transform.position;
        position.x += _direction * (_speed * Time.deltaTime);
        if (position.x <= -_sideLimits || position.x >= _sideLimits) {
            position.x = position.x < 0 ? -_sideLimits : _sideLimits;
            _direction *= -1;
        }
        transform.position = position;
    }

    private void SpawnParentBlock() {
        GameObject parent = Instantiate(prefab, transform.position, Quaternion.identity);
        parent.transform.parent = transform;
        parent.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void SpawnBlock() {
        Instantiate(prefab, transform.position, Quaternion.identity);

        Vector3 position = transform.position;
        position.y += _blockHeight;
        transform.position = position;

        GameObject camera = GameObject.Find(Object.camera);
        camera.GetComponent<CameraController>().OnBlockSpawned(_blockHeight);
    }
}
