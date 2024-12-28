// Created by JulP - Updated for Z-axis bounds

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDestroyBounds : MonoBehaviour
{
    //public float outOfBoundsY = 2.0f; // Vertical out-of-bounds threshold
    public float outOfBoundsZ = -5.0f; // Z-axis out-of-bounds threshold

    void Update()
    {
        // Check if the food item falls below the Y threshold
        // if (transform.position.y < Camera.main.transform.position.y - outOfBoundsY)
        // {
        //     Destroy(gameObject);
        //     Debug.Log("Food missed! Fell below the screen.");
        // }

        // Check if the food item moves too far along the Z-axis
        if (transform.position.z < Camera.main.transform.position.z + outOfBoundsZ)
        {
            Destroy(gameObject);
            Debug.Log("Food missed! Moved too far along Z-axis.");
        }
    }
}
