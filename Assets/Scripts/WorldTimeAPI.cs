using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class WorldTimeAPI : MonoBehaviour
{
    #region singleton class:worldTimeAPI

    public static WorldTimeAPI Instance;
    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }

    #endregion
    
    //json container
    struct TimeData
    {
        //public string client_ip;
        //...
        public string datetime;
        //...
    }
    const string API_URL = "http://worldtimeapi.org/api/timezone/iran";
    [HideInInspector] public bool IsTimeLoaded = false;
    private DateTime currentDateTime = DateTime.Now;
    private void Start() {
        StartCoroutine(GetRealDateTimeFromAPI()); 
    }
    public DateTime GetCurrentDateTime() {
        //here we don't need to get the datetime from the server again
        // just add elapsed time since the game start to currrentDateTime
        return currentDateTime.AddSeconds(Time.realtimeSinceStartup);
    }
    IEnumerator GetRealDateTimeFromAPI() {
        UnityWebRequest webRequest = UnityWebRequest.Get(API_URL);
        Debug.Log("Getting real datetime...");
        yield return webRequest.Send();
        if(webRequest.isNetworkError) {
            Debug.Log("Error:" + webRequest.error);
        }
        else {
            TimeData timeData = JsonUtility.FromJson<TimeData>(webRequest.downloadHandler.text);
            //timeData.datetime value is : 2021-06-07T09:01:37.127578+04:30

            currentDateTime = ParseDateTime(timeData.datetime);
            IsTimeLoaded = true;
            Debug.Log("Success.");
        }
    }
    DateTime ParseDateTime(String datetime) {
        String date = Regex.Match(datetime, @"^\d{4}-\d{2}-\d{2}").Value;
        String time = Regex.Match(datetime, @"\d{2}:\d{2}:\d{2}").Value;

        return DateTime.Parse(String.Format("{0} {1}", date, time));
    }
}

/* API (json)
     {
        "abbreviation":"+0430",
        "client_ip":"88.150.230.97",
        "datetime":"2021-06-07T09:01:37.127578+04:30",
        "day_of_week":0,
        "day_of_year":158,
        "dst":true,
        "dst_from":"2021-03-21T20:30:00+00:00",
        "dst_offset":3600,"
        "dst_until":"2021-09-21T19:30:00+00:00",
        "raw_offset":12600,
        "timezone":"Iran",
        "unixtime":1623040297,
        "utc_datetime":"2021-06-07T04:31:37.127578+00:00",
        "utc_offset":"+04:30",
        "week_number":23
      }
    we only need "datatime" property
     */