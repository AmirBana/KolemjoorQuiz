using System.Collections;
using System;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.UI;

public class TestApi : MonoBehaviour
{
    public Text datetimeText;
    public GameObject heartsHolder;
    DateTime dt1;
    // Start is called before the first frame update
    void Start() {
        if(ObscuredPrefs.GetString("LostHeartDate") != "") {
            dt1 = DateTime.Parse(ObscuredPrefs.GetString("LostHeartDate"));
            StartCoroutine("Counter");
        }
    }
    private void Update() {
        if(Input.GetMouseButton(1)) {
            dt1 = WorldTimeAPI.Instance.GetCurrentDateTime().AddSeconds(10);
            ObscuredPrefs.SetString("LostHeartDate", dt1.ToString());
        }
    }
    IEnumerator Counter() {
        while(ObscuredPrefs.GetInt("LostHeart") > 0) {
            yield return new WaitForSeconds(1f);
            if(WorldTimeAPI.Instance.IsTimeLoaded) {
                DateTime dt2 = WorldTimeAPI.Instance.GetCurrentDateTime();
                TimeSpan timespan = dt1 - dt2;
                int th = timespan.Hours;
                int tm = timespan.Minutes;
                int ts = timespan.Seconds;
                if(ts > 0)
                    datetimeText.text = th + ":" + tm + ":" + ts;
                if(th <= 0 && tm <= 0 && ts <= 0) {
                    ObscuredPrefs.SetInt("LostHeart", ObscuredPrefs.GetInt("LostHeart") - 1);
                    heartsHolder.GetComponent<HeartManager>().ArrangeHearts();
                    if(ObscuredPrefs.GetInt("LostHeart") > 0) {
                        ObscuredPrefs.SetString("LostHeartDate", WorldTimeAPI.Instance.GetCurrentDateTime().AddDays(1).AddHours(th).AddMinutes(tm).AddSeconds(ts).ToString());
                        dt1 = DateTime.Parse(ObscuredPrefs.GetString("LostHeartDate"));
                    }
                    else if(ObscuredPrefs.GetInt("LostHeart") == 0) {
                        ObscuredPrefs.SetString("LostHeartDate", "");
                        datetimeText.text = "";
                        StopCoroutine("Counter");
                    }
                }
            }
            else {
                datetimeText.text = "ﺖﻧﺮﺘﻨﯾﺍ ﻪﺑ ﯽﺳﺮﺘﺳﺩ ﻡﺪﻋ";
            }
        }
    }
}
