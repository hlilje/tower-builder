using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float speed = 10.0f;

    void Update()
    {
        Vector3 position = transform.position;
        float delta = Time.deltaTime * speed;

        if (Input.GetKey("up")) {
            position.z += delta;
        } else if (Input.GetKey("down")) {
            position.z -= delta;
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
