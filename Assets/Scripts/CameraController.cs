using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float groundOffset = 0.2f;
    private float speed = 10.0f;

    private void Update()
    {
        Vector3 position = transform.position;
        float delta = Time.deltaTime * speed;

        if (Input.GetKey("left shift")) {
            if (Input.GetKey("up")) {
                position.y += delta;
            } else if (Input.GetKey("down")) {
                GameObject ground = GameObject.Find("Ground");
                position.y -= delta;
                float limit = ground.transform.position.y + groundOffset;
                position.y = Mathf.Max(position.y, groundOffset);
            }
        } else {
            if (Input.GetKey("up")) {
                position.z += delta;
            } else if (Input.GetKey("down")) {
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
