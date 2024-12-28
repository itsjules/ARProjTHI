using UnityEngine;
using System.Collections;
using UnityEngine.XR.ARFoundation;

public class FoodSpawner : MonoBehaviour
{
    public static FoodSpawner Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public GameObject[] foodItems; // Array of food prefabs
    public float spawnRate = 1f; // Time interval between spawns
    // public float spawnOffset = 0.2f; // Horizontal/vertical spawn offset range
 
    [SerializeField, Tooltip("Multiplier to enlarge the spawn area relative to the screen size.")]
    private float spawnAreaMultiplier = 1f;

    [SerializeField, Tooltip("Distance behind the camera for spawning objects.")]
    private float spawnDepthOffset = 1f;

    public Vector2 moveSpeedRange=new Vector2(0.1f,0.4f);

    public bool testMode = true; // Enable test mode without Face Tracking for Unity editor testing
    private Transform target; // Reference to the user's face (detected by ARFaceManager)

    [SerializeField]
    private ARFaceManager ARFaceManager;

    private void Start()
    {
        // Start the coroutine to spawn food
        StartCoroutine(SpawnFoodCoroutine());

        //Set the Face Position as the target
        SetFaceTarget();
        
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
        //get random spawnpoint for food
        Vector3 spawnPosition = GetSpawnPoint();

        // Instantiate food
        if (foodItems.Length > 0)
        {
            GameObject food = Instantiate(
                foodItems[Random.Range(0, foodItems.Length)],
                spawnPosition,
                Quaternion.identity
            );

            // Assign target and movement to the food
            SetFaceTarget();
            FoodFall foodFall = food.AddComponent<FoodFall>();
            foodFall.target = target;
            foodFall.moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);
        }
        else
        {
            Debug.LogError("No food items assigned to the FoodSpawner.");
        }
    }

    void SetFaceTarget(){
        // Find and assign the detected face from ARFaceManager
        //ARFaceManager ARFaceManager = FindObjectOfType<ARFaceManager>();
        if (ARFaceManager != null && ARFaceManager.trackables.count > 0)
        {
            foreach (var face in ARFaceManager.trackables)
            {
                target = face.transform; // Assign the first detected face as the target
                break;
            }
        }

        if (target == null && !testMode)
        {
            Debug.LogError("No face detected by ARFaceManager. Spawning may fail.");
        }
    }

    Vector3 GetSpawnPoint(){

        //Null-Check
        if (Camera.main == null)
        {
            Debug.LogError("Camera is not assigned.");
            return Vector3.zero;
        }

        // Get the Main Camera's position
        Vector3 cameraPosition = Camera.main.transform.position;

        // Calculate world space dimensions based on screen size (multiplying by 2 to calculate whole screeSpace in World)
        float worldWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 1)).x * 2;
        float worldHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 1)).y * 2;

        // Scale the spawn area
        float spawnAreaWidth = worldWidth * spawnAreaMultiplier;
        float spawnAreaHeight = worldHeight * spawnAreaMultiplier;

        // Randomize spawn position within the larger area behind the camera (divide by 2 to make camera midpoint of spawning area)
        Vector3 spawnPosition = new Vector3(
            cameraPosition.x + Random.Range(-spawnAreaWidth / 2f, spawnAreaWidth / 2f),
            cameraPosition.y + Random.Range(-spawnAreaHeight / 2f, spawnAreaHeight / 2f),
            cameraPosition.z - spawnDepthOffset
        );

        return spawnPosition;
    }
}