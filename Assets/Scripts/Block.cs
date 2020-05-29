using UnityEngine;

public class Block : MonoBehaviour {
    private const int _maxContactCount = 6;
    private ContactPoint[] _contactPoints = new ContactPoint[_maxContactCount];

    private void OnCollisionEnter(Collision collision) {
        if (GetComponent<Rigidbody>().isKinematic) {
            return;
        }

        int contacts = collision.GetContacts(_contactPoints);
        if (contacts == 0) {
            return;
        }

        // TODO: Pick a clever contact point
        Vector3 contactPoint = _contactPoints[0].point;
        GameObject gameObject = collision.gameObject;

        if (gameObject.tag == Object.block) {
            Debug.Log("Block hit!");

            // TODO: Notify block spawner to increase height

            // TODO: This will add two springs for each collision
            if (!gameObject.GetComponent<Rigidbody>().isKinematic) {
                Debug.Log("Attached to block with SpringJoint");

                gameObject.AddComponent<SpringJoint>();
                gameObject.GetComponent<SpringJoint>().connectedBody = GetComponent<Rigidbody>();
            }
        } else if (gameObject.tag == Object.ground) {
            Debug.Log("Ground hit!");

            // Only the first block is attached to the ground
            if (!gameObject.GetComponent<FixedJoint>()) {
                Debug.Log("Attached to ground with FixedJoint");

                gameObject.AddComponent<FixedJoint>();
                gameObject.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();
            }
        } else {
            Debug.LogError("Collision with unknown object");
        }
    }
}
