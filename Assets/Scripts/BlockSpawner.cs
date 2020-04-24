using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {
    public GameObject prefab;

    private int blockHeight = 1; // TODO: Base off mesh size
    private int sideLimits = 2;
    private int direction = 1;
    private float Speed = 2;

    void Start() {
        Debug.Log("BlockSpawner");

        GameObject parent = Instantiate(prefab, transform.position, Quaternion.identity);
        parent.transform.parent = transform;
        parent.GetComponent<Rigidbody>().isKinematic = true;

        // TODO: Pre-spawn initial block
    }

    void Update() {
        UpdatePosition();

        if (Input.GetKeyDown(KeyCode.Q)) {
            SpawnBlock();
        }
    }

    void UpdatePosition() {
        Vector3 position = transform.position;
        position.x += direction * (Speed * Time.deltaTime);
        if (position.x <= -sideLimits || position.x >= sideLimits)
        {
            position.x = position.x < 0 ? -sideLimits : sideLimits;
            direction *= -1;
        }
        transform.position = position;
    }

    void SpawnBlock() {
        Instantiate(prefab, transform.position, Quaternion.identity);

        Vector3 position = transform.position;
        position.y += blockHeight;
        transform.position = position;

        GameObject camera = GameObject.Find("Main Camera");
        Vector3 cameraPos = camera.transform.position;
        cameraPos.y += blockHeight;
        camera.transform.position = cameraPos;
    }
}
