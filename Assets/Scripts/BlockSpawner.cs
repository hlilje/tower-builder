using UnityEngine;

public class BlockSpawner : MonoBehaviour {
    public GameObject _prefab;

    private float _blockHeight;

    private float _speed = 2.0f;
    private int _sideLimits = 2;
    private int _direction = 1;

    private void Start() {
        _blockHeight = _prefab.GetComponent<Renderer>().bounds.size.x;

        SpawnParentBlock();
        SpawnInitBlock();
    }

    private void Update() {
        UpdatePosition();

        if (Input.GetKeyDown(KeyCode.Q)) {
            SpawnBlock();
        }
    }

    private void UpdatePosition() {
        Vector3 position = transform.position;
        position.x += _direction * (_speed * Time.deltaTime);
        if (position.x <= -_sideLimits || position.x >= _sideLimits)
        {
            position.x = position.x < 0 ? -_sideLimits : _sideLimits;
            _direction *= -1;
        }
        transform.position = position;
    }

    private void SpawnParentBlock()
    {
        GameObject parent = Instantiate(_prefab, transform.position, Quaternion.identity);
        parent.transform.parent = transform;
        parent.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void SpawnInitBlock()
    {
        GameObject ground = GameObject.Find(Object.ground);
        Vector3 groundPos = ground.transform.position;
        Vector3 initPos = transform.position;
        initPos.y = groundPos.y;
        Instantiate(_prefab, initPos, Quaternion.identity);

        // TODO: Anchor to ground
    }

    private void SpawnBlock() {
        Instantiate(_prefab, transform.position, Quaternion.identity);

        Vector3 position = transform.position;
        position.y += _blockHeight;
        transform.position = position;

        GameObject camera = GameObject.Find(Object.camera);
        camera.GetComponent<CameraController>().OnBlockSpawned(_blockHeight);
    }
}
