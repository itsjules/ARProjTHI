// //Created by JulP

// using UnityEngine;
// using System.Collections;

// public class FoodSpawnerNew : MonoBehaviour
// {
//     public GameObject[] foodPrefabs; // Array to hold multiple food prefabs
//     public float spawnDepth = 1.5f; // Depth before the camera for food to spawn

//     public float spawnInterval = 0.5f; // Time between spawns

//     public float fallSpeed = 2.0f; // Controll Fall speed for all foods from Food Spawner (can also be disabled to tweak different falling rates for different foods)

//     void Start()
//     {
//         StartCoroutine(SpawnFood());
//     }

//     IEnumerator SpawnFood()
//     {
//         while (true)
//         {
//             SpawnFoodAboveCamera();
//             yield return new WaitForSeconds(spawnInterval);
//         }
//     }

//     void SpawnFoodAboveCamera()
//     {
//         if (foodPrefabs == null || foodPrefabs.Length == 0 || Camera.main == null)
//             return;

//         // Get the camera's viewport bounds
//         Camera cam = Camera.main;

//         // Calculate random position within the viewport
//         float randomX = Random.Range(0.1f, 0.9f); // Horizontal viewport range (avoid edges)
//         // float randomY = Random.Range(0.1f, 0.9f); // Vertical viewport range (avoid edges)
//         Vector3 viewportPosition = new Vector3(randomX, 1, cam.nearClipPlane + spawnDepth);

//         // Convert the viewport position to world space
//         Vector3 spawnPosition = cam.ViewportToWorldPoint(viewportPosition);

//         // Pick a random food prefab
//         int randomIndex = Random.Range(0, foodPrefabs.Length);
//         GameObject foodPrefab = foodPrefabs[randomIndex];

//         // Instantiate the selected food prefab
//         Instantiate(foodPrefab, spawnPosition, foodPrefab.transform.rotation);


//         // Get the Falling script attached to the food to make the food fall constantly
//         FoodFall foodFall = foodPrefab.GetComponent<FoodFall>();
//         foodFall.fallSpeed = fallSpeed; // Set fall speed for the food object
//     }
// }