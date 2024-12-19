//Created by JulP

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDestroyBounds : MonoBehaviour
{
    public float outOfBoundsY=2.0f;
    void Update()
    {
        if (transform.position.y < Camera.main.transform.position.y - outOfBoundsY)
        {
            Destroy(gameObject);
            Debug.Log("Food missed!");
            Debug.Log("Effect missing!");
        }
    }
}
