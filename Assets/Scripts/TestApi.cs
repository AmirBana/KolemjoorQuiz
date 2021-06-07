using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TestApi : MonoBehaviour
{
    public Text datetimeText;
    DateTime dt1;
    // Start is called before the first frame update
    void Start()
    {
        /*DateTime dt1 = DateTime.Now;
        DateTime dt2 = dt1;
        dt2.AddSeconds(15f);
        TimeSpan timeSpan = dt2 - dt1;
        Debug.Log("dt1: " + dt1);
        Debug.Log("dt2: " + dt2);
        Debug.Log("N of Secnods: " + timeSpan.TotalSeconds);*/
       
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetMouseButton(0) && WorldTimeAPI.Instance.IsTimeLoaded) {
            DateTime currentDateTime = WorldTimeAPI.Instance.GetCurrentDateTime();
            datetimeText.text = currentDateTime.ToString();
        }*/
        if(Input.GetMouseButton(1)) {
            dt1 = WorldTimeAPI.Instance.GetCurrentDateTime();
        }
        DateTime dt2 = dt1.AddSeconds(15);
        //dt2.AddSeconds(15);
        TimeSpan timeSpan= dt2 - dt1;
        if(Input.GetMouseButton(0)) {
            Debug.Log("dt1: " + dt1);
            Debug.Log("dt2: " + dt2);
            Debug.Log("timeSpan: " + timeSpan.TotalSeconds);
        }
    }
}
