using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FoodEater : MonoBehaviour
{
    
    // This function gets triggered when an object enters the collider
    void OnTriggerEnter(Collider collider)
    {
        // Check if the object has the "food" tag
        if (collider.CompareTag("food"))
        {
            // Destroy the food object
            Destroy(collider.gameObject);
            ScoreManager.Instance.UpdateScore();

        }
    }
}
