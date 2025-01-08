//Created by GnyanP

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodEater : MonoBehaviour
{

    public GameObject foodEatenEffect; // Food eaten Particle effect prefab
    private float effectScaleMultiplier = 0.008f; //Decrease scale of effect

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("food"))
        {
            // Play particle effect at the foods position
            if (foodEatenEffect != null)
            {
                GameObject spawnedEffect=Instantiate(foodEatenEffect, collider.transform.position, Quaternion.identity);
                spawnedEffect.transform.localScale *= effectScaleMultiplier;
            }
            
            // Is food healthy or unhealthy for score
            string foodName = collider.gameObject.name;
            if (ScoreManager.Instance.IsHealthyFood(foodName))
            {
                ScoreManager.Instance.AddHealthyScore();
            }
            else if (ScoreManager.Instance.IsUnhealthyFood(foodName))
            {
                ScoreManager.Instance.AddUnhealthyScore();
            }

            Destroy(collider.gameObject);

            // Confirm food ate
            Debug.Log($"Ate food: {foodName}");
        }
        else {
            Debug.Log($"Ignored collision with: {collider.gameObject.name}");
        }
    }
}

