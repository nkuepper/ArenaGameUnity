using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float maxDist = 4f;
    private Transform target;
    private float speed = 0.08f;
    private Vector3 desiredPosition, position;
    private float offset = -10f;

    private void Start()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        // Sets camera to follow player
        desiredPosition = new Vector3(
            Mathf.Clamp(desiredPosition.x, target.position.x - maxDist, target.position.x + maxDist),
            Mathf.Clamp(desiredPosition.y, target.position.y - maxDist + 3.5f, target.position.y + maxDist -1),
            offset);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, speed);
    }

}
