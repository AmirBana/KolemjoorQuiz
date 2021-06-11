using System.Collections;
using System;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.UI;

public class QuizLostTimer : MonoBehaviour
{
    public Text[] dateTimeTxt;
    DateTime dt1;
    // Start is called before the first frame update
    void Start() {
        if(ObscuredPrefs.GetString("IsLost") == "") {
            ObscuredPrefs.SetBool("NeedHeart", false);
        }
        else {
            ObscuredPrefs.SetBool("NeedHeart", true);
            dt1 = DateTime.Parse(ObscuredPrefs.GetString("IsLost"));
            StartCoroutine("Counter");
        }
    }

    IEnumerator Counter() {
        DateTime dt2;
        while(true) {
            yield return new WaitForSeconds(1f);
            if(WorldTimeAPI.Instance.IsTimeLoaded) {
                dt2 = WorldTimeAPI.Instance.GetCurrentDateTime();
            }
            else {
                dt2 = DateTime.Now;
            }
            TimeSpan timespan = dt1 - dt2;
            int th = timespan.Hours;
            int tm = timespan.Minutes;
            int ts = timespan.Seconds;
            if(ts > 0) {
                for(int i = 0; i < dateTimeTxt.Length; i++) {
                    dateTimeTxt[i].text = tm + ":" + ts;
                }
            }
                
            if(th <= 0 && tm <= 0 && ts <= 0) {
                ObscuredPrefs.SetString("IsLost", "");
                ObscuredPrefs.SetBool("NeedHeart", false);
                for(int i = 0; i < dateTimeTxt.Length; i++) {
                    dateTimeTxt[i].text = "";
                }
            }
        }
    }
}
