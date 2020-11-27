using UnityEngine;


public class BlockSpawner : MonoBehaviour {
    public GameObject prefab;

    private GameObject _block;

    private const float _velocityIncrement = 0.5f;
    private const float _cooldown = 1.0f;
    private const float _heightSpeed = 1.0f;

    private float _blockHeight;
    private float _targetHeight = 0.0f;
    private float _currentCooldown = 0.0f;
    private bool _paused = false;


    public void OnBlockAttached() {
        _targetHeight += _blockHeight;

        SpawnBlock(_velocityIncrement);

        GameObject.Find(Object.game).GetComponent<GameController>().IncreaseScore();

        Debug.Log("Block attached");
    }

    public void OnBlockMissed(Block block) {
        if (block.State != BlockState.InFlight && block.State != BlockState.Attached) {
            return;
        }

        if (block.State == BlockState.InFlight) {
            block.State = BlockState.Missed;

            if (!_block) {
                SpawnBlock(_velocityIncrement);
            }
        }

        GameObject.Find(Object.game).GetComponent<GameController>().DecreaseLives();

        Debug.Log("Block missed");
    }


    private void Start() {
        _blockHeight = prefab.GetComponent<Renderer>().bounds.size.x;

        SpawnBlock(0.0f);
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
            if (debug || (!gameOver && !_paused && _block && _currentCooldown <= 0.0f && _targetHeight <= 0.0f )) {
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

        if (_targetHeight > 0.0f) {
            float heightDelta = _heightSpeed * Time.deltaTime;
            float newHeight = Mathf.Clamp(_targetHeight - heightDelta, 0.0f, _targetHeight);
            float heightDiff = _targetHeight - newHeight;

            Vector3 position = transform.position;
            position.y += heightDiff;
            transform.position = position;

            Vector3 anchor = hingeJoint.connectedAnchor;
            anchor.y += heightDiff;
            hingeJoint.connectedAnchor = anchor;

            CameraController camera = GameObject.Find(Object.camera).GetComponent<CameraController>();
            camera.IncreaseHeight(heightDiff);

            _targetHeight = newHeight;
        }
    }

    private void SpawnBlock(float velocityIncrement) {
        if (_block) {
            Debug.LogError("Block aleady exists");
            return;
        }

        _block = Instantiate(prefab, transform.position, transform.rotation);
        _block.transform.parent = transform;

        Rigidbody rigidbody = _block.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.detectCollisions = false;

        HingeJoint hingeJoint = GetComponent<HingeJoint>();
        JointMotor motor = hingeJoint.motor;
        motor.targetVelocity += velocityIncrement * Mathf.Sign(motor.targetVelocity);
        hingeJoint.motor = motor;
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
