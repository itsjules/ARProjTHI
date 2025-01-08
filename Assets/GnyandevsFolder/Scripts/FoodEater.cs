//Created by JulP

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodEater : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        // Check if the object has the "food" tag
        if (collider.CompareTag("food"))
        {
            // Determine if the food is healthy or unhealthy
            string foodName = collider.gameObject.name;
            if (ScoreManager.Instance.IsHealthyFood(foodName))
            {
                ScoreManager.Instance.AddHealthyScore();
            }
            else if (ScoreManager.Instance.IsUnhealthyFood(foodName))
            {
                ScoreManager.Instance.AddUnhealthyScore();
            }

            // Destroy the food object
            Destroy(collider.gameObject);
        }
    }
}

