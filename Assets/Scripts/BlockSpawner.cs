using UnityEngine;


public class BlockSpawner : MonoBehaviour {
    public GameObject prefab;

    private GameObject _parent;

    private const float _speed = 2.0f;
    private const float _cooldown = 1.0f;
    private const int _sideLimits = 2;

    private float _blockHeight;
    private float _currentCooldown;
    private int _direction = 1;
    private bool _available = true;
    private bool _paused = false;


    public void OnBlockAttached() {
        Vector3 position = transform.position;
        position.y += _blockHeight;
        transform.position = position;

        SetAvailable(true);

        CameraController camera = GameObject.Find(Object.camera).GetComponent<CameraController>();
        camera.OnBlockSpawned(_blockHeight);

        GameObject.Find(Object.game).GetComponent<GameController>().IncreaseScore();
    }

    public void OnBlockMissed() {
        SetAvailable(true);

        GameObject.Find(Object.game).GetComponent<GameController>().DecreaseScore();

        Debug.Log("Block missed");
    }


    private void Start() {
        _blockHeight = prefab.GetComponent<Renderer>().bounds.size.x;

        SpawnParentBlock();
    }

    private void Update() {
        if (Input.GetKeyDown(Key.pause)) {
            _paused = !_paused;
        }

        if (!_paused) {
            _currentCooldown -= Time.deltaTime;
            Mathf.Clamp(_currentCooldown, 0.0f, _currentCooldown);

            UpdatePosition();
        }

        if (Input.GetKeyDown(Key.blockSpawn)) {
            bool debug = GameObject.Find(Object.game).GetComponent<GameController>().IsDebug();
            if (debug || (!_paused && _available && _currentCooldown <= 0.0f )) {
                SpawnBlock();
            }
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
        _parent = Instantiate(prefab, transform.position, Quaternion.identity);
        _parent.transform.parent = transform;
        Rigidbody rigidbody = _parent.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.detectCollisions = false;
    }

    private void SetAvailable(bool available) {
        _available = available;
        _parent.GetComponent<Renderer>().enabled = available;
    }

    private void SpawnBlock() {
        Instantiate(prefab, transform.position, Quaternion.identity);
        SetAvailable(false);
        _currentCooldown = _cooldown;
    }
}
