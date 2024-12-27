using UnityEngine;
using System.Collections;
using UnityEngine.XR.ARFoundation;

public class FoodSpawner : MonoBehaviour
{
    public GameObject[] foodItems; // Array of food prefabs
    public float spawnRate = 1f; // Time interval between spawns
    public float spawnOffset = 0.2f; // Horizontal/vertical spawn offset range

    private Transform target; // Reference to the user's face (detected by ARFaceManager)

    private void Start()
    {
        // Start the coroutine to spawn food
        StartCoroutine(SpawnFoodCoroutine());

        // Find and assign the detected face from ARFaceManager
        ARFaceManager faceManager = FindObjectOfType<ARFaceManager>();
        if (faceManager != null && faceManager.trackables.count > 0)
        {
            foreach (var face in faceManager.trackables)
            {
                target = face.transform; // Assign the first detected face as the target
                break;
            }
        }

        if (target == null)
        {
            Debug.LogError("No face detected by ARFaceManager. Spawning may fail.");
        }
    }

    private IEnumerator SpawnFoodCoroutine()
    {
        while (true)
        {
            SpawnFood();
            yield return new WaitForSeconds(spawnRate); // Wait for the defined spawn rate
        }
    }

    void SpawnFood()
    {
        if (Camera.main == null)
        {
            Debug.LogError("AR Camera not found. Ensure the AR Camera is tagged as 'MainCamera'.");
            return;
        }

        // Get the AR Camera's height (Y position)
        float cameraHeight = Camera.main.transform.position.y;

        // Calculate spawn height relative to the AR Camera
        float spawnHeight = cameraHeight ; // no offset

        // Randomize spawn position
        Vector3 spawnPosition = new Vector3(
            Random.Range(-spawnOffset, spawnOffset),
            spawnHeight,
            Camera.main.transform.position.z - 0.1f // Spawn behind the camera
        );

        // Instantiate food
        if (foodItems.Length > 0)
        {
            GameObject food = Instantiate(
                foodItems[Random.Range(0, foodItems.Length)],
                spawnPosition,
                Quaternion.identity
            );

            // Assign target and movement to the food
            FoodFall foodFall = food.AddComponent<FoodFall>();
            foodFall.target = target;
            foodFall.moveSpeed = Random.Range(2f, 5f);
        }
        else
        {
            Debug.LogError("No food items assigned to the FoodSpawner.");
        }
    }
}
