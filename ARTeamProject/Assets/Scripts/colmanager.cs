using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colmanager : MonoBehaviour
{
   static public bool rendcheck;


    // Update is called once per frame
   
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlacedBlock"))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.GetComponent<Renderer>().enabled = false;
            }
            rendcheck = true;
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlacedBlock"))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.GetComponent<Renderer>().enabled = true;
            }
            rendcheck = false;
        }
    }

}