using UnityEngine;

public class FoodFall : MonoBehaviour
{
    public float fallSpeed; // Speed at which the food falls 

    void Update()
    {
        // Move the food object downward each frame based on the fallSpeed
        transform.Translate(Vector3.down *0.1f* fallSpeed * Time.deltaTime);
    }
}