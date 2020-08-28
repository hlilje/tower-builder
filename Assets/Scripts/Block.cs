using UnityEngine;

public class Block : MonoBehaviour {
    private const int _maxContactCount = 6;
    private ContactPoint[] _contactPoints = new ContactPoint[_maxContactCount];

    private const float _jointBreakForce = 20.0f;

    private float _halfWidth;

    private void Start() {
        _halfWidth = GetComponent<Renderer>().bounds.size.x / 2;
    }

    private void AddJoint<T>(GameObject gameObject, Vector3 contactPoint, bool anchor) where T : Joint {
        if (GetComponents<Joint>().Length > 0 || gameObject.GetComponents<Joint>().Length > 0) {
            return;
        }

        // TODO: Visualise connection points

        gameObject.AddComponent<T>();

        T joint = gameObject.GetComponent<T>();
        joint.connectedBody = GetComponent<Rigidbody>();
        joint.breakForce = _jointBreakForce;
        joint.autoConfigureConnectedAnchor = anchor;
        if (anchor)
        {
            joint.connectedAnchor = contactPoint;
        }

        Debug.Log("Added " + typeof(T).Name + " at anchor " + joint.connectedAnchor);
    }

    private void BlockHit(Collision collision, GameObject gameObject) {
        // TODO: Notify block spawner to increase height

        Vector3 collisionPosition = collision.transform.position;

        // Ensure consistency in who gets the joint
        if (transform.position.y < collisionPosition.y) {
            return;
        }

        int contacts = collision.GetContacts(_contactPoints);
        if (contacts == 0) {
            return;
        }

        // Naive check to ensure balanced joints
        float distance = Mathf.Abs(transform.position.x - collisionPosition.x);
        if (distance > _halfWidth) {
            return;
        }

        float closestContactDistance = float.MaxValue;
        Vector3 closestContactPoint = _contactPoints[0].point;
        for (int i = 0; i < contacts; ++i) {
            Vector3 contactPoint = _contactPoints[i].point;
            float contactDistance = (contactPoint - transform.position).magnitude;
            if (contactDistance < closestContactDistance) {
                closestContactDistance = contactDistance;
                closestContactPoint = contactPoint;
            }
        }

        if (!gameObject.GetComponent<Rigidbody>().isKinematic) {
            Debug.Log("Attached to block");

            AddJoint<SpringJoint>(gameObject, closestContactPoint, true);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (GetComponent<Rigidbody>().isKinematic) {
            return;
        }

        // TODO: Only the last dropped block and the last attached block
        // should be able to attach

        GameObject gameObject = collision.gameObject;

        if (gameObject.tag == Object.block) {
            Debug.Log("Block hit");

            BlockHit(collision, gameObject);
        } else if (gameObject.tag == Object.ground) {
            Debug.Log("Ground hit");

            // Only the first block is attached to the ground
            if (!gameObject.GetComponent<Joint>()) {
                Debug.Log("Attached to ground");

                AddJoint<FixedJoint>(gameObject, Vector3.zero, false);
            }
        } else {
            Debug.LogError("Collision with unknown object");
        }
    }

    private void OnJointBreak(float breakForce) {
        Debug.Log("Broke joint with force: " + breakForce);

        // TODO: Game Over
    }
}
