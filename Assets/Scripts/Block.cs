using UnityEngine;

public class Block : MonoBehaviour {
    private const float _jointBreakForce = 20.0f;

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
            Debug.Log("Block hit");

            // TODO: Notify block spawner to increase height

            // TODO: This will add two springs for each collision
            if (!gameObject.GetComponent<Rigidbody>().isKinematic) {
                Debug.Log("Attached to block with SpringJoint");

                gameObject.AddComponent<SpringJoint>();
                SpringJoint joint = gameObject.GetComponent<SpringJoint>();
                joint.connectedBody = GetComponent<Rigidbody>();
                joint.breakForce = _jointBreakForce;
            }
        } else if (gameObject.tag == Object.ground) {
            Debug.Log("Ground hit");

            // Only the first block is attached to the ground
            if (!gameObject.GetComponent<FixedJoint>()) {
                Debug.Log("Attached to ground with FixedJoint");

                gameObject.AddComponent<FixedJoint>();
                FixedJoint joint = gameObject.GetComponent<FixedJoint>();
                joint.connectedBody = GetComponent<Rigidbody>();
                joint.breakForce = _jointBreakForce;
            }
        } else {
            Debug.LogError("Collision with unknown object");
        }
    }

    private void OnJointBreak(float breakForce) {
        Debug.Log("Broken joint with force: " + breakForce);

        // TODO: Game Over
    }
}
