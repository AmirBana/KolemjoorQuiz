using CodeStage.AntiCheat.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    // Start is called before the first frame update
    public int ThisLevel;
    public Text errorText;

    void Start()
    {
        
    }
    public void LoadLevelFunction()
    {
        SceneManager.LoadScene(ThisLevel+3);
    }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex  + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void EnterLevel()
    {
        if (ObscuredPrefs.GetBool("NeedHeart"))
        {
            if (ObscuredPrefs.GetInt("LostHeart") < 5)
            {
                ObscuredPrefs.SetInt("LostHeart", ObscuredPrefs.GetInt("LostHeart") + 1);
                if (ObscuredPrefs.GetString("LostHeartDate") == "")
                {
                    if (WorldTimeAPI.Instance.IsTimeLoaded)
                    {
                        ObscuredPrefs.SetString("LostHeartDate", WorldTimeAPI.Instance.GetCurrentDateTime().AddDays(1).ToString());
                        Debug.Log("Time from api: " + ObscuredPrefs.GetString("LostHeartDate"));
                    }
                    else
                    {
                        ObscuredPrefs.SetString("LostHeartDate", DateTime.Now.AddDays(1).ToString());
                        Debug.Log("Local Time: " + ObscuredPrefs.GetString("LostHeartDate"));
                    }
                }
                SceneManager.LoadScene(ThisLevel + 3);
                //SceneManager.LoadScene(scene);
                ObscuredPrefs.SetString("IsLost", "");
                ObscuredPrefs.SetBool("NeedHeart", false);
            }
            else
            {
                StartCoroutine("ErrorText");
            }
        }
        else
        {
            //SceneManager.LoadScene(scene);
            SceneManager.LoadScene(ThisLevel + 3);
            ObscuredPrefs.SetString("IsLost", "");
            ObscuredPrefs.SetBool("NeedHeart", false);
        }
    }
    IEnumerator ErrorText()
    {
        errorText.text = "ﺖﺴﯿﻧ ﯽﻓﺎﮐ ﺐﻠﻗ";
        yield return new WaitForSeconds(1f);
        errorText.text = "";
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
