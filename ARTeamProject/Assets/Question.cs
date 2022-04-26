using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class Question : MonoBehaviour
{

    public LayerMask cubelayer;
    public List<GameObject> ACube = new List<GameObject>();
    Canvas canvas;


    // Start is called before the first frame update
    void Start()
    {
        canvas = gameObject.GetComponentInChildren<Canvas>();
        canvas.enabled = false;
    }
        // Update is called once per frame
        void Update()
        {

            if (Input.touchCount > 0)
            {
                Debug.Log("touch!");
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out hit, cubelayer))
                {
                    if (hit.collider.gameObject.name == ACube[0].name)
                    {
                        Debug.Log("True " + hit.collider.gameObject);
                        canvas.enabled = true;


                    }
                    else
                    {
                        Debug.Log("False " + hit.collider.gameObject);
                    }
                }
            }
        }
    }

    


