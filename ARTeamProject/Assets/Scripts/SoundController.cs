using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public AudioSource bgm;
    public Sprite[] switchSprites;
    private Image switchImg;
    private int state = 0; //0:mute, 1:volume

    // Start is called before the first frame update
    void Start()
    {
        switchImg = GetComponent<Button>().image;
        switchImg.sprite = switchSprites[state];
    }

    public void bgmControl()
    {
        state = 1 - state; // switch
        switchImg.sprite = switchSprites[state];

        switch (state)
        {
            case 0: //mute
                bgm.Stop();
                break;

            case 1: //volume
                bgm.Play();
                break;
        }
    }
}
