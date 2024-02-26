using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{
    Transform player;
    public Vector2 minBoundary, maxBoundary;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void Update()
    {
        Vector3 playerPosition = player.position;
        playerPosition.x = Mathf.Clamp(playerPosition.x, minBoundary.x, maxBoundary.x);
        playerPosition.y = Mathf.Clamp(playerPosition.y, minBoundary.y, maxBoundary.y);
        playerPosition.z = transform.position.z;
        transform.position = playerPosition;
    }
}
