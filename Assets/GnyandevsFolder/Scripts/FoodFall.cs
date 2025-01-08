//Created by JulP

using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FoodFall : MonoBehaviour
{
    private Vector3 targetPosition; //Target of Food to fly towards to
    private float moveSpeed; //Default moveSpeed 
    
    [SerializeField]
    private GameObject foodMissedEffect; // Food missed Particle effect prefab
    
    private float effectScaleMultiplier = 0.005f;//Decrease scale of effect

    /// <summary>
/// Intitializes the target the food flies towards to and the speed of it flying
/// </summary>
/// <param name="param1">target point in World Space the food can fly towards</param>
/// <param name="param2">speed at which the food flies</param>
    public void Initialize(Vector3 target, float speed)
    {
        targetPosition = target;
        moveSpeed = speed;
    }

    void Update()
    {
        // Move toward the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Destroy the object if it reaches the inital face target before its eaten
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {   
            //Play and resize particle effect
            if (foodMissedEffect != null)
            {   
                GameObject spawnedEffect=Instantiate(foodMissedEffect, transform.position, Quaternion.identity);
                spawnedEffect.transform.localScale *= effectScaleMultiplier;
            }

            
            //deytroy and let food rest in peace
            Destroy(gameObject);
        }
    }
}
