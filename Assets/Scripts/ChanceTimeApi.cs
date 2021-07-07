using System.Collections;
using System.Collections.Generic;
using System;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.UI;

public class ChanceTimeApi : MonoBehaviour
{
    DateTime dt1;
    public Button dailyChallengeBtn;
    public Text dateTimeTxt;
    private void Start() {
        if(ObscuredPrefs.GetString("ChanceDate") != "") {
            dt1 = DateTime.Parse(ObscuredPrefs.GetString("ChanceDate"));
            StartCoroutine("Counter");
        }
    }
    IEnumerator Counter() {
        while(true) {
            yield return new WaitForSeconds(1f);
            if(WorldTimeAPI.Instance.IsTimeLoaded) {
                DateTime dt2 = WorldTimeAPI.Instance.GetCurrentDateTime();
                TimeSpan timespan = dt1 - dt2;
                int th = timespan.Hours;
                int tm = timespan.Minutes;
                int ts = timespan.Seconds;
                if(ts > 0)
                    dateTimeTxt.text = th + ":" + tm + ":" + ts;
                if(th <= 0 && tm <= 0 && ts <= 0) {
                    ObscuredPrefs.SetString("ChanceDate", "");
                    dailyChallengeBtn.interactable = true;
                    dateTimeTxt.text = "";
                }
            }
            else {
                dateTimeTxt.text = "ﺪﯾﻮﺷ ﻞﺼﺘﻣ ﺖﻧﺮﺘﻨﯾﺍ ﻪﺑ ﺎﻔﻄﻟ";
            }
        }
    }
    private void Update() {
        if(Input.GetKey(KeyCode.H))
            dt1 = DateTime.Now.AddSeconds(10);
    }
}
