using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject[] foodItems; // Array of food prefabs (healthy/unhealthy)
    public float spawnRate = 1f; // Time interval between spawns
    public float spawnHeight = 3f; // Height above AR plane

    public float spawnOffset=0.2f;

    public GameObject spawnheightMarker;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnFood), 0f, spawnRate);
        Instantiate(spawnheightMarker,new Vector3(0,spawnHeight,0),Quaternion.identity);
    }

    void SpawnFood()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(-spawnOffset,spawnOffset), 
            spawnHeight, 
            Random.Range(-spawnOffset,spawnOffset)
        );

        GameObject food = Instantiate(
            foodItems[Random.Range(0, foodItems.Length)], 
            spawnPosition, 
            Quaternion.identity
        );

        food.AddComponent<Rigidbody>(); // Adds gravity to the food
    }
}
