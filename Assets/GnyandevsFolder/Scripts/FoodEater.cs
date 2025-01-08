// Created by JulP

using UnityEngine;

public class FoodEater : MonoBehaviour
{

    public GameObject foodEatenEffect; // Particle effect prefab


    void OnTriggerEnter(Collider collider)
    {
        // Check if the object has the "food" tag
        if (collider.CompareTag("food"))
        {
            // Play particle effect at the food's position (if assigned)
            if (foodEatenEffect != null)
            {
                Instantiate(foodEatenEffect, collider.transform.position, Quaternion.identity);
            }

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

            //Play Particle effect if not null
            FXateFood?.Play();

            // Destroy the food object
            Destroy(collider.gameObject);

            // Debug to confirm collision
            Debug.Log($"Ate food: {foodName}");
        }
        else
        {
            Debug.Log($"Ignored collision with: {collider.gameObject.name}");
        }
    }
}
