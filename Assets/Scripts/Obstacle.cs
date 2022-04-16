using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float destroyOffset = -20f;
    public float playerSpeedMultuplier = 1;
    public float obstacleSpeed = 10;

    public static int ObstaclesCount;

    void Start()
    {
        ObstaclesCount++;
        if (transform.position.x < 0)
            obstacleSpeed *= -1f;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y,
            transform.position.z - PlayerController.CurrentSpeed * Time.deltaTime * playerSpeedMultuplier * PlayerController.MinigameSpeed + obstacleSpeed * Time.deltaTime * PlayerController.MinigameSpeed);

        if (transform.position.z < destroyOffset)
        {
            Destroy(gameObject);
            ObstaclesCount--;
        }
    }
}
