/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.Storage;

public class TimeManager : MonoBehaviour
{
    public int Secound;
    void Start()
    {
        string LoadTime = ObscuredPrefs.GetString("_QuitTime");
        if (!LoadTime.Equals(""))
        {
            DateTime LoadDateTime = DateTime.Parse(LoadTime);
            DateTime NowTime = DateTime.Now;
            if (NowTime > LoadDateTime)
            {
                TimeSpan timeSpan = NowTime - LoadDateTime;
                Secound = (int)timeSpan.TotalSeconds;
                //Debug.Log(Secound);
            }

        }

    }

    private void OnApplicationQuit()
    {
        DateTime QuitTime = DateTime.Now;
        ObscuredPrefs.SetString("_QuitTime", QuitTime.ToString());

    }

}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    /* 
          necessary variables to hold all the things we need.
        php url
        timedata, the data we get back
        current time
        current date
    */

    public static TimeManager sharedInstance = null;
    private string _url = "http://tivanvico.ir/wp-content/OnlineTime.php";
    private string _timeData;
    private string _currentTime;
    private string _currentDate;


    //make sure there is only one instance of this always.
    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else if (sharedInstance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


    //time fether coroutine
    public IEnumerator getTime()
    {
        Debug.Log("==> step 1. Getting info from internet now!");
        WWW www = new WWW(_url);
        yield return www;
        Debug.Log("==> step 2. Got the info from internet!");
        _timeData = www.text;
        string[] words = _timeData.Split('/');
        //timerTestLabel.text = www.text;
        Debug.Log("The date is : " + words[0]);
        Debug.Log("The time is : " + words[1]);

        //setting current time
        _currentDate = words[0];
        _currentTime = words[1];
    }


    //get the current time at startup
    void Start()
    {
        Debug.Log("==> TimeManager script is Ready.");
        StartCoroutine("getTime");
    }

    //get the current date
    public string getCurrentDateNow()
    {
        return _currentDate;
    }


    //get the current Time
    public string getCurrentTimeNow()
    {
        return _currentTime;
    }


}