using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeManager : MonoBehaviour
{

    public GameObject mPanel;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                //ȭ�� ��ü�� panel�̴ϱ� ��ġ ������ ���x
                Destroy(mPanel);
                Destroy(this);
            }
        }
    }
}
