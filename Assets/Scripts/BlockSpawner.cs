using UnityEngine;


public class BlockSpawner : MonoBehaviour {
    public GameObject prefab;

    private GameObject _block;

    private const float _cooldown = 1.0f;

    private float _blockHeight;
    private float _currentCooldown;
    private bool _paused = false;


    public void OnBlockAttached() {
        Vector3 position = transform.position;
        position.y += _blockHeight;
        transform.position = position;

        HingeJoint hingeJoint = GetComponent<HingeJoint>();
        Vector3 anchor = hingeJoint.connectedAnchor;
        anchor.y += _blockHeight;
        hingeJoint.connectedAnchor = anchor;

        SpawnBlock();

        CameraController camera = GameObject.Find(Object.camera).GetComponent<CameraController>();
        camera.OnBlockSpawned(_blockHeight);

        GameObject.Find(Object.game).GetComponent<GameController>().IncreaseScore();

        Debug.Log("Block attached");
    }

    public void OnBlockMissed(Block block) {
        if (block.State != BlockState.InFlight && block.State != BlockState.Settled) {
            return;
        }

        if (block.State == BlockState.InFlight) {
            block.State = BlockState.Missed;

            if (!_block) {
                SpawnBlock();
            }
        }

        GameObject.Find(Object.game).GetComponent<GameController>().DecreaseLives();

        Debug.Log("Block missed");
    }


    private void Start() {
        _blockHeight = prefab.GetComponent<Renderer>().bounds.size.x;

        SpawnBlock();
    }

    private void Update() {
        if (Input.GetKeyDown(Key.pause)) {
            _paused = !_paused;
        }

        if (!_paused) {
            _currentCooldown -= Time.deltaTime;
            _currentCooldown = Mathf.Clamp(_currentCooldown, 0.0f, _currentCooldown);

            UpdateMovement();
        }

        if (Input.GetKeyDown(Key.blockSpawn)) {
            GameController gameController = GameObject.Find(Object.game).GetComponent<GameController>();
            bool debug = gameController.IsDebug;
            bool gameOver = gameController.GameOver;
            if (debug || (!gameOver && !_paused && _block && _currentCooldown <= 0.0f )) {
                ReleaseBlock();
            }
        }
    }


    private void UpdateMovement() {
        HingeJoint hingeJoint = GetComponent<HingeJoint>();
        JointLimits limits = hingeJoint.limits;
        if (hingeJoint.angle <= limits.min || hingeJoint.angle >= limits.max) {
            JointMotor motor = hingeJoint.motor;
            motor.targetVelocity *= -1;
            hingeJoint.motor = motor;
        }
    }

    private void SpawnBlock() {
        if (_block) {
            Debug.LogError("Block aleady exists");
            return;
        }

        _block = Instantiate(prefab, transform.position, transform.rotation);
        _block.transform.parent = transform;

        Rigidbody rigidbody = _block.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.detectCollisions = false;
    }

    private void ReleaseBlock() {
        if (!_block) {
            Debug.LogError("Block doesn't exist");
            return;
        }

        _block.transform.parent = null;

        Rigidbody rigidbody = _block.GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.detectCollisions = true;

        _block = null;
        _currentCooldown = _cooldown;
    }
}
