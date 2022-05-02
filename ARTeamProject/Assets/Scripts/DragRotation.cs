using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragRotation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch screenTouch = Input.GetTouch(0);
            if (screenTouch.phase == TouchPhase.Moved)
                transform.Rotate(0f, -screenTouch.deltaPosition.x, 0f);
        }
    }
}
