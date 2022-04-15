using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummerManager : MonoBehaviour
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
        if (Input.touchCount > 0)
        {
            audio.Play();
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(240, 24, 24);
        }
    }
}
