using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {
    public GameObject prefab;

    private float blockHeight;

    private float Speed = 2.0f;
    private int sideLimits = 2;
    private int direction = 1;

    private void Start() {
        blockHeight = prefab.GetComponent<Renderer>().bounds.size.x;

        SpawnParentBlock();
        //SpawnInitBlock();
    }

    private void Update() {
        UpdatePosition();

        if (Input.GetKeyDown(KeyCode.Q)) {
            SpawnBlock();
        }
    }

    private void SpawnParentBlock()
    {
        GameObject parent = Instantiate(prefab, transform.position, Quaternion.identity);
        parent.transform.parent = transform;
        parent.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void SpawnInitBlock()
    {
        GameObject ground = GameObject.Find("Ground");
        Vector3 groundPos = ground.transform.position;
        Vector3 initPos = transform.position;
        initPos.y -= groundPos.y;
        Instantiate(prefab, initPos, Quaternion.identity);
    }

    private void UpdatePosition() {
        Vector3 position = transform.position;
        position.x += direction * (Speed * Time.deltaTime);
        if (position.x <= -sideLimits || position.x >= sideLimits)
        {
            position.x = position.x < 0 ? -sideLimits : sideLimits;
            direction *= -1;
        }
        transform.position = position;
    }

    private void SpawnBlock() {
        Instantiate(prefab, transform.position, Quaternion.identity);

        Vector3 position = transform.position;
        position.y += blockHeight;
        transform.position = position;

        GameObject camera = GameObject.Find("Main Camera");
        camera.GetComponent<CameraController>().OnBlockSpawned(blockHeight);
    }
}
