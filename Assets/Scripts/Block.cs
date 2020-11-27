using UnityEngine;


public enum BlockState {
    InFlight,
    CollisionTarget,
    Missed,
    Attached
}

public class Block : MonoBehaviour {
    private const float _jointBreakForce = Mathf.Infinity;

    private const int _maxContactCount = 6;
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

        if (gameObject.tag == Object.block) {
            Debug.Log("Block hit");

            BlockHit(collision, gameObject);
        } else if (gameObject.tag == Object.ground) {
            Debug.Log("Ground hit");

            BlockSpawner blockSpawner = GameObject.Find(Object.blockSpawner).GetComponent<BlockSpawner>();

            // Only the first block is attached to the ground
            if (!gameObject.GetComponent<Joint>()) {
                AddJoint<FixedJoint>(gameObject, Vector3.zero, true, false);

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

    private Vector3 FindClosestContactPoint(Collision collision) {
        int contacts = collision.GetContacts(_contactPoints);

        float closestContactDistance = float.MaxValue;
        Vector3 closestContactPoint = Vector3.zero;

        for (int i = 0; i < contacts; ++i) {
            Vector3 contactPoint = _contactPoints[i].point;
            float contactDistance = (contactPoint - transform.position).magnitude;
            if (contactDistance < closestContactDistance) {
                closestContactDistance = contactDistance;
                closestContactPoint = contactPoint;
            }
        }

        return closestContactPoint;
    }

    private void AddJoint<T>(GameObject gameObject, Vector3 contactPoint, bool autoAnchor, bool breakable) where T : Joint {
        if (GetComponents<Joint>().Length > 0 || gameObject.GetComponents<Joint>().Length > 0) {
            return;
        }

        gameObject.AddComponent<T>();

        T joint = gameObject.GetComponent<T>();
        joint.connectedBody = GetComponent<Rigidbody>();
        joint.autoConfigureConnectedAnchor = autoAnchor;
        if (!autoAnchor) {
            joint.connectedAnchor = contactPoint;
        }
        if (breakable) {
            joint.breakForce = _jointBreakForce;
        }

        Debug.Log("Added " + typeof(T).Name + " at anchor " + joint.connectedAnchor);
    }

    private void BlockHit(Collision collision, GameObject gameObject) {
        if (gameObject.GetComponent<Rigidbody>().isKinematic) {
            return;
        }

        // Always handle the collision from the dropped blocks perspective
        if (_state != BlockState.InFlight) {
            return;
        }

        BlockSpawner blockSpawner = GameObject.Find(Object.blockSpawner).GetComponent<BlockSpawner>();
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

        Vector3 contactPoint = FindClosestContactPoint(collision);
        AddJoint<FixedJoint>(gameObject, contactPoint, true, true);

        block.State = BlockState.Attached;
        SetCollisionTarget();

        blockSpawner.OnBlockAttached();
    }

    private void OnJointBreak(float breakForce) {
        Debug.Log("Broke joint with force: " + breakForce);
    }
}
