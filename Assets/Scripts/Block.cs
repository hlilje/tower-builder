using UnityEngine;

public class Block : MonoBehaviour {
    private const int _maxContactCount = 6;
    private ContactPoint[] _contactPoints = new ContactPoint[_maxContactCount];

    private const float _jointBreakForce = 20.0f;

    private float _halfWidth;
    private bool _collisionTarget = false;
    private bool _abortCollision = false;

    private void Start() {
        _halfWidth = GetComponent<Renderer>().bounds.size.x / 2;

        Debug.Log("Created block: " + GetInstanceID());
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

        if (_abortCollision) {
            _abortCollision = false;
            return;
        }

        BlockSpawner blockSpawner = GameObject.Find(Object.blockSpawner).GetComponent<BlockSpawner>();
        Block block = gameObject.GetComponent<Block>();

        if (!block.IsCollisionTarget()) {
            if (!_collisionTarget) {
                blockSpawner.OnBlockMissed();
            }
            return;
        }

        // Naive check to ensure balanced joints
        Vector3 collisionPosition = collision.transform.position;
        float distance = Mathf.Abs(transform.position.x - collisionPosition.x);
        if (distance > _halfWidth) {
            blockSpawner.OnBlockMissed();
            return;
        }

        Vector3 contactPoint = FindClosestContactPoint(collision);
        AddJoint<SpringJoint>(gameObject, contactPoint, true, true);

        // Avoid flip-flopping during the collison update
        block.SetAbortCollision(true);

        block.SetCollisionTarget(false);
        SetCollisionTarget(true);

        blockSpawner.OnBlockAttached();

        Debug.Log("Attached to block");
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

                SetCollisionTarget(true);

                Debug.Log("Attached to ground");
            } else {
                blockSpawner.OnBlockMissed();
            }
        } else {
            Debug.LogError("Collision with unknown object");
        }
    }

    private void OnJointBreak(float breakForce) {
        GameObject.Find(Object.game).GetComponent<GameController>().DecreaseScore();

        Debug.Log("Broke joint with force: " + breakForce);
    }

    public bool IsCollisionTarget() {
        return _collisionTarget;
    }

    public void SetCollisionTarget(bool target) {
        _collisionTarget = target;

        Debug.Log("New collision target: " + GetInstanceID() );
    }

    public void SetAbortCollision(bool abort) {
        _abortCollision = abort;
    }
}
