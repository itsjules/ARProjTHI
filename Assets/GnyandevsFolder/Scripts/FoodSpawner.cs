// using UnityEngine;

// public class FoodSpawner : MonoBehaviour
// {
//     public GameObject[] foodItems; // Array of food prefabs
//     public Transform target; // Reference to user's face (e.g., AR Camera transform)
//     public float spawnRate = 1f; // Time interval between spawns
//     public float spawnHeight = 3f; // Height above AR plane
//     public float spawnOffset = 0.2f; // Horizontal/vertical spawn offset range

//     private void Start()
//     {
//         InvokeRepeating(nameof(SpawnFood), 0f, spawnRate); // Start spawning food at regular intervals
//     }

//     void SpawnFood()
//     {
//         // Randomize spawn position within defined offset
//         Vector3 spawnPosition = new Vector3(
//             Random.Range(-spawnOffset, spawnOffset),
//             spawnHeight,
//             Random.Range(-spawnOffset, spawnOffset)
//         );

//         // Instantiate a random food prefab
//         GameObject food = Instantiate(
//             foodItems[Random.Range(0, foodItems.Length)],
//             spawnPosition,
//             Quaternion.identity
//         );

//         // Assign the target (e.g., AR Camera) to the FoodFall script
//         FoodFall foodFall = food.AddComponent<FoodFall>();
//         foodFall.target = target;

//         // Set a random movement speed for variety
//         foodFall.moveSpeed = Random.Range(2f, 5f);
//     }
// }
