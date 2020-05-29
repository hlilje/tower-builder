using UnityEngine;

public class Block : MonoBehaviour
{
    private void OnCollisionEnter(Collision collider) {
        Debug.Log("Collision");

        if (collider.gameObject.tag == Object.block) {
            // TODO: Attach with spring
            // TODO: Notify block spawner to increase height
            Debug.Log("Block hit!");
        } else if (collider.gameObject.tag == Object.ground) {
            // TODO: Attach to ground
            // TODO: Notify block spawner to increase height
            Debug.Log("Ground hit!");
        } else {
            Debug.LogError("Collision with unknown object");
        }
    }
}
