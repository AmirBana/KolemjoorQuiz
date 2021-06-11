using System.Collections;
using System.Collections.Generic;
using System;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadQuizLevel : MonoBehaviour
{
   public Text errorText;
   public void EnterLevel(string scene) {
        if(ObscuredPrefs.GetBool("NeedHeart")) {
            if(ObscuredPrefs.GetInt("LostHeart") < 5) {
                ObscuredPrefs.SetInt("LostHeart", ObscuredPrefs.GetInt("LostHeart") + 1);
                if(ObscuredPrefs.GetString("LostHeartDate") == "") {
                    if(WorldTimeAPI.Instance.IsTimeLoaded) {
                        ObscuredPrefs.SetString("LostHeartDate", WorldTimeAPI.Instance.GetCurrentDateTime().AddDays(1).ToString());
                        Debug.Log("Time from api: " + ObscuredPrefs.GetString("LostHeartDate"));
                    }
                    else {
                        ObscuredPrefs.SetString("LostHeartDate", DateTime.Now.AddDays(1).ToString());
                        Debug.Log("Local Time: " + ObscuredPrefs.GetString("LostHeartDate"));
                    }
                }
                SceneManager.LoadScene(scene);
                ObscuredPrefs.SetString("IsLost","");
                ObscuredPrefs.SetBool("NeedHeart", false);
            }
            else {
                StartCoroutine("ErrorText");
            }
        }
        else {
            SceneManager.LoadScene(scene);
            ObscuredPrefs.SetString("IsLost", "");
            ObscuredPrefs.SetBool("NeedHeart", false);
        }
    }
    IEnumerator ErrorText() {
            errorText.text = "ﺖﺴﯿﻧ ﯽﻓﺎﮐ ﺐﻠﻗ";
            yield return new WaitForSeconds(1f);  
        errorText.text = "";
    }
}
