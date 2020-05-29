using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float groundOffset = 0.2f;
    private float speed = 10.0f;

    private void Update()
    {
        Vector3 position = transform.position;
        float delta = Time.deltaTime * speed;

        if (Input.GetKey(Key.cameraModifier)) {
            if (Input.GetKey(Key.cameraUp)) {
                position.y += delta;
            } else if (Input.GetKey(Key.cameraDown)) {
                GameObject ground = GameObject.Find(Object.ground);
                position.y -= delta;
                float limit = ground.transform.position.y + groundOffset;
                position.y = Mathf.Max(position.y, groundOffset);
            }
        } else {
            if (Input.GetKey(Key.cameraForward)) {
                position.z += delta;
            } else if (Input.GetKey(Key.cameraBack)) {
                position.z -= delta;
            }
        }

        transform.position = position;
    }

    public void OnBlockSpawned(float blockHeight)
    {
        Vector3 position = transform.position;
        position.y += blockHeight;
        transform.position = position;
    }
}
