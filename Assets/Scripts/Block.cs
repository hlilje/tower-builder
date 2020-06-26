using UnityEngine;

public class Block : MonoBehaviour {
    private const int _maxContactCount = 6;
    private ContactPoint[] _contactPoints = new ContactPoint[_maxContactCount];

    private const float _jointBreakForce = 20.0f;

    private void AddJoint<T>(GameObject gameObject) where T : Joint {
        if (GetComponents<Joint>().Length > 0 || gameObject.GetComponents<Joint>().Length > 0) {
            return;
        }

        Debug.Log("Added joint of type: " + typeof(T).Name);

        gameObject.AddComponent<T>();
        T joint = gameObject.GetComponent<T>();
        joint.connectedBody = GetComponent<Rigidbody>();
        joint.breakForce = _jointBreakForce;
    }

    private void OnCollisionEnter(Collision collision) {
        if (GetComponent<Rigidbody>().isKinematic) {
            return;
        }

        int contacts = collision.GetContacts(_contactPoints);
        if (contacts == 0) {
            return;
        }

        // TODO: Only the last dropped block and the last attached block
        // should be able to attach

        // TODO: Pick a clever contact point
        Vector3 contactPoint = _contactPoints[0].point;
        GameObject gameObject = collision.gameObject;

        if (gameObject.tag == Object.block) {
            Debug.Log("Block hit");

            // TODO: Notify block spawner to increase height

            if (!gameObject.GetComponent<Rigidbody>().isKinematic) {
                Debug.Log("Attached to block");

                AddJoint<SpringJoint>(gameObject);
            }
        } else if (gameObject.tag == Object.ground) {
            Debug.Log("Ground hit");

            // Only the first block is attached to the ground
            if (!gameObject.GetComponent<Joint>()) {
                Debug.Log("Attached to ground");

                AddJoint<FixedJoint>(gameObject);
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
