using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("Van Speed")]
    public float maxSpeed = 10;
    public float acceleration = 1;
    public float braking = 1;
    public float speedBehindAnotherVan = 10;

    [Header("Vans")]
    public GameObject vanPrefab;
    public Transform vansHolder;
    public Transform playerSpawnPosition;
    public Transform rightObstacleSpawn;
    public Transform leftObstacleSpawn;
    public Vector2 minMaxVanSpawnDistance = new Vector2(3f, 10f);
    public int maxObstaclesCount = 5;

    [Header("Player Controls Settings")]
    public Transform playerVan;
    public float roundTime = 45;
    public float rightLineVanOffset = 1f;
    private float vanTargetOffset;
    public float xSpeed = 0.5f;
    public float yRotation = 15f;
    public float ySpeed = 0.5f;
    public float rotationOffset = 0.1f;
    public float finishedMinigameSpeed = 0.1f;
    public float distanceModifier = 10f;
    private Vector3 targetEulerAngles;
    private int lastTurn = 0;
    private float obstacleSpawnerTimer = 0;

    [Header("Player Van Elements")]
    public Transform playerChild;
    public Transform raycastTransform;
    public float raycastLength = 1;

    public static float MinigameSpeed { get; private set; }
    public static float CurrentSpeed { get; private set; }
    public static float CurrentDistance { get; private set; }
    public static float CurrentTime { get; private set; }
    public bool isStraight { get; private set; }
    private bool isPlaying = false;

    public void TurnLeft()
    {
        Turn(-1);
    }

    public void TurnRight()
    {
        Turn(1);
    }

    private void Turn(int value)
    {
        if (!isStraight)
            return;

        vanTargetOffset = rightLineVanOffset * value;
        isStraight = false;
        lastTurn = value;
    }

    public void Lose()
    {
        FinishMinigame(false);
        MinigameSpeed = finishedMinigameSpeed;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    }

    public void PrepareMinigame()
    {
        vanTargetOffset = rightLineVanOffset;
        Obstacle.ObstaclesCount = 0;
        SpawnPlayer();
        TurnRight();
        CurrentTime = 0;
        CurrentDistance = 0;
        CurrentSpeed = -0.2f;
        MinigameSpeed = 1;
        isStraight = false;
        isPlaying = true;
        AudioManager.instance.UpdateMusic(true);
        UIManager.Instance.ShowGameUI();
    }

    public void DestroyOldObjects()
    {
        if (playerVan != null)
        {
            Destroy(playerVan.gameObject);
        }

        Obstacle[] obstacles = GetComponentsInChildren<Obstacle>();

        for (int i = 0; i < obstacles.Length; i++)
        {
            Destroy(obstacles[i].gameObject);
        }
    }
        
    private void Update()
    {
        if (isPlaying == false)
            return;

        UpdateRaycast();
        HandleInput();
        ControlPlayerPosition();
        ManageObstacles();
        UpdateProgress();
    }

    void OnDrawGizmos()
    {
        Debug.DrawRay(raycastTransform.position, Vector3.forward * raycastLength, Color.red);
    }

    private void SpawnPlayer()
    {
        DestroyOldObjects();

        playerVan = Instantiate(vanPrefab, playerSpawnPosition.position + new Vector3(3, 0, 0), playerSpawnPosition.rotation, vansHolder).transform;
        Destroy(playerVan.GetComponent<Obstacle>());//.enabled = false;
        playerVan.gameObject.name = "PlayerVan";
        playerVan.Find("Collider").gameObject.AddComponent<VanCollider>();
    }

    private void HandleInput()
    {
        if (CurrentTime >= roundTime)
            return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            TurnLeft();

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            TurnRight();
    }

    private void ControlPlayerPosition()
    {
        //position
        float targetSpeed = GetTargetSpeed();
        float speedChange = CurrentSpeed < targetSpeed ? acceleration : braking;
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, targetSpeed, Time.deltaTime * speedChange);
        Vector3 targetPosition = playerVan.position;
        targetPosition.x = vanTargetOffset;
        playerVan.position = Vector3.MoveTowards(playerVan.position, targetPosition, Time.deltaTime * xSpeed * MinigameSpeed);

        //rotation
        if (targetPosition.x > playerVan.position.x + rotationOffset) // turn left
            targetEulerAngles = new Vector3(playerVan.eulerAngles.x, yRotation, playerVan.eulerAngles.z);
        else if (targetPosition.x < playerVan.position.x - rotationOffset) // turn right
            targetEulerAngles = new Vector3(playerVan.eulerAngles.x, -yRotation, playerVan.eulerAngles.z);
        else // go straight
            targetEulerAngles = new Vector3(playerVan.eulerAngles.x, 0, playerVan.eulerAngles.z);

        if (targetPosition.x == playerVan.position.x)
            isStraight = true;

        if (playerVan.eulerAngles.y > 180)
            targetEulerAngles.y += 360;

        playerVan.eulerAngles = Vector3.MoveTowards(playerVan.eulerAngles, targetEulerAngles, Time.deltaTime * ySpeed * MinigameSpeed);
    }

    private void ManageObstacles()
    {
        obstacleSpawnerTimer -= Time.deltaTime;

        if (obstacleSpawnerTimer > 0)
            return;

        if (Obstacle.ObstaclesCount >= maxObstaclesCount)
            return;

        if (Random.value > 0.5f)
            Instantiate(vanPrefab, rightObstacleSpawn.position, rightObstacleSpawn.rotation, vansHolder).name = "VanObstacle";
        else
            Instantiate(vanPrefab, leftObstacleSpawn.position, leftObstacleSpawn.rotation, vansHolder).name = "VanObstacle";

        obstacleSpawnerTimer = Random.Range(minMaxVanSpawnDistance.x, minMaxVanSpawnDistance.y) / CurrentSpeed;

        if (obstacleSpawnerTimer > 7)
            obstacleSpawnerTimer = 7;
    }

    private void UpdateProgress()
    {
        CurrentDistance += CurrentSpeed * Time.deltaTime * distanceModifier;
        CurrentTime += Time.deltaTime;

        if (CurrentTime >= roundTime)
        {
            FinishMinigame(true);
            MinigameSpeed = finishedMinigameSpeed;
        }
    }

    private void UpdateRaycast()
    {
        playerChild.transform.position = playerVan.transform.position;
    }

    public void FinishMinigame(bool result)
    {
        if (isPlaying == false)
            return;

        isPlaying = false;

        AudioManager.instance.UpdateMusic(false);

        if (result == true)
        {
            UIManager.Instance.ShowResultScreen(CurrentDistance, BestScoresManager.Instance.CanAddScore(CurrentDistance));
        }
        else
        {
            UIManager.Instance.ShowGameOverScreen();
        }
    }

    private float GetTargetSpeed()
    {
        RaycastHit raycastHit;

        if (Physics.Raycast(raycastTransform.position, Vector3.forward, out raycastHit, raycastLength))
            return speedBehindAnotherVan;

        return maxSpeed;
    }
}