using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeStart;
    public TMPro.TMP_Text textBox;
    
    public int min;
    public int sec;
    
    void Start()
    {
        min = Convert.ToInt32(timeStart) / 60;
        sec = Convert.ToInt32(timeStart) - (min * 60);
        textBox.text = "Temps " + min + ':' + sec;
    }

    // Update is called once per frame
    void Update()
    {
        timeStart -= Time.deltaTime;
        float seconds = Mathf.Round(timeStart);
        min = Convert.ToInt32(seconds) / 60;
        sec = Convert.ToInt32(seconds) - (min * 60);
        //Mort a 14 min test
        if (min < 10)
        {
            if (sec < 10)
            {
                textBox.text = "Temps 0" + min + ":0" + sec;
            }
            else
            {
                textBox.text = "Temps 0" + min + ":" + sec;
            }
        }
        else
        {
            if (sec < 10)
            {
                textBox.text = "Temps " + min + ":0" + sec;
            }
            else
            {
                textBox.text = "Temps " + min + ":" + sec;
            }
        }
    }
}
