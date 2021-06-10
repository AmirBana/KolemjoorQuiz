using System;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Net;

public class DailyChallengeSetTimer : MonoBehaviour
{
    DateTime dt1;
    public Button dailyBtn;
    public Text dateTxt;
    private void Start() {
       
        if(ObscuredPrefs.GetString("DailyChallengeDate") != "")
            dailyBtn.interactable = false;
        else
            dailyBtn.interactable = true;
    }
    public void SetTimer() {
        GetHtmlFromURL HTMLURL = new GetHtmlFromURL();
        String HTMLText = HTMLURL.GetHtmlFromUri("http://google.com");
        if(HTMLText=="") {       
                StartCoroutine("NoInternet");
        }
        else {
            if(WorldTimeAPI.Instance.IsTimeLoaded) {
                ObscuredPrefs.SetString("DailyChallengeDate", WorldTimeAPI.Instance.GetCurrentDateTime().AddDays(1).ToString());
                dailyBtn.interactable = false;
                SceneManager.LoadScene("DailyChallenge Scene");
            }
        }
    }
    IEnumerator NoInternet() {
        dateTxt.text = "ﺪﯾﻮﺷ ﻞﺼﺘﻣ ﺖﻧﺮﺘﻨﯾﺍ ﻪﺑ ﺎﻔﻄﻟ";
        yield return new WaitForSeconds(1f);

    }
    
}
