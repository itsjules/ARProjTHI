using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FoodFall : MonoBehaviour
{
     private Vector3 targetPosition;
    private float moveSpeed;

    public void Initialize(Vector3 target, float speed)
    {
        targetPosition = target;
        moveSpeed = speed;
    }

    void Update()
    {
        // Move toward the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Destroy the object if it reaches the target before its eaten
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            Destroy(gameObject);
        }
    }
}
