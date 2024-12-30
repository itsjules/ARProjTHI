//created by JulP
using UnityEngine;
using System.Collections;
using UnityEngine.XR.ARFoundation;

public class FoodSpawner : MonoBehaviour
{
    public GameObject[] foodItems; // Array of food prefabs
    public float spawnRate = 1f; // Time interval between spawns
    public float moveSpeed = 2f; // Speed of the food
    public float spawnDepthOffset = -0.1f; // Slightly behind the near clip plane
    public float targetAreaOffset = 0.2f; // Offset around the player's face (target area)

    [SerializeField]
    private ARFaceManager ARFaceManager; // Reference to ARFaceManager for face tracking

    private Transform faceTarget; // Transform of the detected face
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        // Start spawning food at regular intervals
        StartCoroutine(SpawnFoodCoroutine());
    }

    private IEnumerator SpawnFoodCoroutine()
    {
        while (true)
        {
            SpawnFood();
            yield return new WaitForSeconds(spawnRate); // Wait before spawning the next food
        }
    }

    void SpawnFood()
    {
        if (foodItems.Length == 0 || mainCamera == null)
        {
            Debug.LogError("No food items assigned or Camera not available.");
            return;
        }

        // Update the face target
        SetFaceTarget();

        // Get a random spawn position along the screen edges
        Vector3 spawnPosition = GetRandomEdgeSpawnPosition();

        // Instantiate a random food prefab at the spawn position
        GameObject food = Instantiate(
            foodItems[Random.Range(0, foodItems.Length)],
            spawnPosition,
            Quaternion.identity
        );

        // Get the target position (face position with some offset)
        Vector3 targetPosition = GetTargetPosition();

        // Add a simple movement component to make the food move toward the target
        FoodFall foodMovement = food.AddComponent<FoodFall>();
        foodMovement.Initialize(targetPosition, moveSpeed);
    }

    Vector3 GetRandomEdgeSpawnPosition()
    {
        float randomX, randomY;
        Vector3 viewportPosition;
        float spawnDepth = mainCamera.nearClipPlane + spawnDepthOffset; // Slightly behind near clip plane

        // Randomize edge (0 = left, 1 = right, 2 = top, 3 = bottom)
        int edge = Random.Range(0, 4);

        switch (edge)
        {
            case 0: // Left edge
                randomY = Random.Range(0f, 1f);
                viewportPosition = new Vector3(0, randomY, spawnDepth);
                break;
            case 1: // Right edge
                randomY = Random.Range(0f, 1f);
                viewportPosition = new Vector3(1, randomY, spawnDepth);
                break;
            case 2: // Top edge
                randomX = Random.Range(0f, 1f);
                viewportPosition = new Vector3(randomX, 1, spawnDepth);
                break;
            case 3: // Bottom edge
                randomX = Random.Range(0f, 1f);
                viewportPosition = new Vector3(randomX, 0, spawnDepth);
                break;
            default:
                viewportPosition = new Vector3(0.5f, 0.5f, spawnDepth); // Fallback to center
                break;
        }

        // Convert viewport position to world space
        return mainCamera.ViewportToWorldPoint(viewportPosition);
    }

    Vector3 GetTargetPosition()
    {
        // If a face is detected, use its position; otherwise, default to a central point in front of the camera
        Vector3 targetPosition = faceTarget != null
            ? faceTarget.position
            : mainCamera.transform.position + mainCamera.transform.forward * 1f;

        // Add some random offset around the target for variety
        targetPosition.x += Random.Range(-targetAreaOffset, targetAreaOffset);
        targetPosition.y += Random.Range(-targetAreaOffset, targetAreaOffset);

        return targetPosition;
    }

    void SetFaceTarget()
    {
        // Find and assign the detected face from ARFaceManager
        if (ARFaceManager != null && ARFaceManager.trackables.count > 0)
        {
            foreach (var face in ARFaceManager.trackables)
            {
                faceTarget = face.transform; // Assign the first detected face as the target
                break;
            }
        }

        if (faceTarget == null)
        {
            Debug.LogWarning("No face detected. Defaulting to central forward target.");
        }
    }
}