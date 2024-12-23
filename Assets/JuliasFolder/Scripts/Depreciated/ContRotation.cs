//Created by JulP

using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ContRotation : MonoBehaviour
{
     // Rotation speed in degrees per second
    public float rotationSpeed = 50f;

    void Update()
    {
        // Rotate the coin around its Y-axis
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
