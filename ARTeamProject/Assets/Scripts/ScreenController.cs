using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public bool portrait;
    void Start()
    {
        switch (portrait)
        {
            case true:
                Screen.orientation = ScreenOrientation.Portrait;
                break;
            case false:
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                break;
        }
    }

}
