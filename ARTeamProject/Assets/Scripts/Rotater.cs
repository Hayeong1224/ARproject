using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    public float xAngle, yAngle, zAngle;
    void Update()
    {
        // Rotate the object according to the three angles, considering the time between frames (deltatime)
        transform.Rotate(xAngle * Time.deltaTime, yAngle * Time.deltaTime, zAngle * Time.deltaTime, Space.World);
    }

}
