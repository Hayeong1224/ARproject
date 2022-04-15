using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class springManager : MonoBehaviour
{
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount%2==0)
        {
            audio.Play();
        }
        else if (Input.touchCount % 2 == 1)
        {
            audio.Stop();
        }
    }
}
