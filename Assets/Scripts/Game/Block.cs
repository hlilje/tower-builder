using UnityEngine;


public enum BlockState {
    InFlight,
    CollisionTarget,
    Missed,
    Attached
}

public class Block : MonoBehaviour {
    private const float _jointBreakForce = Mathf.Infinity;

    private const int _maxContactCount = 16;
    private ContactPoint[] _contactPoints = new ContactPoint[_maxContactCount];

    private float _halfWidth;

    private BlockState _state = BlockState.InFlight;


    public BlockState State {
        get => _state;
        set => _state = value;
    }


    private void Start() {
        _halfWidth = GetComponent<Renderer>().bounds.size.x / 2;

        Debug.Log("Created block: " + GetInstanceID());
    }

    private void OnCollisionEnter(Collision collision) {
        if (GetComponent<Rigidbody>().isKinematic) {
            return;
        }

        GameObject gameObject = collision.gameObject;

        if (gameObject.tag == GameUObject.block) {
            Debug.Log("Block hit");

            BlockHit(collision, gameObject);
        } else if (gameObject.tag == GameUObject.ground) {
            Debug.Log("Ground hit");

            BlockSpawner blockSpawner = GameObject.Find(GameUObject.blockSpawner).GetComponent<BlockSpawner>();

            // Only the first block is attached to the ground
            if (!gameObject.GetComponent<Joint>()) {
                AddJoint(gameObject);

                blockSpawner.OnBlockAttached();

                SetCollisionTarget();

                Debug.Log("Attached to ground");
            } else {
                blockSpawner.OnBlockMissed(this);
            }
        } else {
            Debug.LogError("Collision with unknown object");
        }
    }


    private void SetCollisionTarget() {
        _state = BlockState.CollisionTarget;
        Debug.Log("New collision target: " + GetInstanceID());
    }

    private void AddJoint(GameObject gameObject) {
        if (GetComponents<Joint>().Length > 0 || gameObject.GetComponents<Joint>().Length > 0) {
            return;
        }

        gameObject.AddComponent<FixedJoint>();

        FixedJoint joint = gameObject.GetComponent<FixedJoint>();
        joint.connectedBody = GetComponent<Rigidbody>();
        joint.breakForce = _jointBreakForce;

        Debug.Log("Added joint with break force: " + _jointBreakForce);
    }

    private void BlockHit(Collision collision, GameObject gameObject) {
        if (gameObject.GetComponent<Rigidbody>().isKinematic) {
            return;
        }

        // Always handle the collision from the dropped blocks perspective
        if (_state != BlockState.InFlight) {
            return;
        }

        BlockSpawner blockSpawner = GameObject.Find(GameUObject.blockSpawner).GetComponent<BlockSpawner>();
        Block block = gameObject.GetComponent<Block>();

        if (block.State != BlockState.CollisionTarget) {
            blockSpawner.OnBlockMissed(this);
            return;
        }

        // Naive check to ensure balanced joints
        Vector3 collisionPosition = collision.transform.position;
        float distance = Mathf.Abs(transform.position.x - collisionPosition.x);
        if (distance > _halfWidth) {
            blockSpawner.OnBlockMissed(this);
            return;
        }

        AddJoint(gameObject);

        block.State = BlockState.Attached;
        SetCollisionTarget();

        blockSpawner.OnBlockAttached();
    }

    private void OnJointBreak(float breakForce) {
        Debug.Log("Broke joint with force: " + breakForce);
    }
}
