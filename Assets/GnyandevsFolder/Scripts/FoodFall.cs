//Created by JulP

using UnityEngine;

public class FoodFall : MonoBehaviour
{
    public float fallSpeed; // Speed at which the food falls 

    void Update()
    {
        // Move the food object downward each frame based on the fallSpeed
        transform.Translate(Vector3.down *0.1f* fallSpeed * Time.deltaTime); // if food prefab has a tilt in rotation to display it better, then the food prefab needs a parent that has 0 rotation, and then the food model as a child with rotation, otherwise this line lets them fall down in a tilt
    }
}