using UnityEngine;

public class Block : MonoBehaviour {
    private const int _maxContactCount = 6;
    private ContactPoint[] _contactPoints = new ContactPoint[_maxContactCount];

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Collision");

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

            //if (!gameObject.GetComponent<SpringJoint>()) {
            //    Debug.Log("Attached with SpringJoint");

            //    gameObject.AddComponent<SpringJoint>();
            //    gameObject.GetComponent<SpringJoint>().connectedBody = collision.rigidbody;
            //}
        } else if (gameObject.tag == Object.ground) {
            Debug.Log("Ground hit!");

            if (!gameObject.GetComponent<FixedJoint>()) {
                Debug.Log("Attached with FixedJoint");

                gameObject.AddComponent<FixedJoint>();
                gameObject.GetComponent<FixedJoint>().connectedBody = collision.rigidbody;
            }
        } else {
            Debug.LogError("Collision with unknown object");
        }
    }
}
