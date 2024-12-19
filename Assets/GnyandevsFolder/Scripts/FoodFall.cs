using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FoodFall : MonoBehaviour
{
    private Transform target; // Transform of the user's face (tracked via ARFaceManager)
    public float moveSpeed = 3f; // Speed at which the food moves

    void Start()
    {
        // Find the ARFaceManager in the scene
        ARFaceManager faceManager = FindObjectOfType<ARFaceManager>();

        if (faceManager != null && faceManager.trackables.count > 0)
        {
            // Get the first face tracked by ARFaceManager
            foreach (var face in faceManager.trackables)
            {
                target = face.transform; // Set target to the face's transform
                break;
            }
        }
    }

    void Update()
    {
        if (target != null)
        {
            // Make the food face the target
            transform.LookAt(target);

            // Move food toward the target
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("No face detected. Food cannot move towards the target.");
        }
    }
}
