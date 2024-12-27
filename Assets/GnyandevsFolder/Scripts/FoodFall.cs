using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FoodFall : MonoBehaviour
{
    public Transform target; // Transform of the user's face (tracked via ARFaceManager or set in editor)
    public float moveSpeed = 0.1f; // Speed at which the food moves
    public bool testMode = false; // Enable test mode for Unity editor testing
    public float destroyAfterSeconds = 15f; // Time after which the food item is destroyed

    void Start()
    {
        // Schedule destruction after the specified time
        Destroy(gameObject, destroyAfterSeconds);
    }

    void Update()
    {
        if (testMode)
        {
            // Test target position: 1.5 meters in front of the AR Camera
            Vector3 testTargetPos = new Vector3(
                Camera.main.transform.position.x,
                Camera.main.transform.position.y,
                Camera.main.transform.position.z + 1.5f
            );

            // Test food moving
            transform.position = Vector3.MoveTowards(transform.position, testTargetPos, moveSpeed * Time.deltaTime);
        }
        else if (target != null)
        {
            // Make the food face the target
            transform.LookAt(target);

            // Move food toward the actual target
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("No target detected. Food cannot move towards the target.");
        }
    }
}
