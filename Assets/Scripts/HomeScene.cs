using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.Storage;
public class HomeScene : MonoBehaviour
{
    public GameObject chanceWheel;
    public GameObject FreeCoin;
    public  Text userScoreTxt;
    public  Text userCoinTxt;

    private void Start()
    {
        //chanceWheel.SetActive(false);
        userScoreTxt.text = "" + ObscuredPrefs.GetInt("Score");
        userCoinTxt.text = "" + ObscuredPrefs.GetInt("Coin");
    }
 
    public void WheelChance()
    {

        chanceWheel.GetComponent<Canvas>().sortingOrder = 50;
    }
    public void FreeCoins()
    {
        FreeCoin.GetComponent<Canvas>().sortingOrder = 51;

    }
    public void BackWheelChance()
    {
        // chanceWheel.SetActive(false);

        chanceWheel.GetComponent<Canvas>().sortingOrder = -6;
    }

    public void ClearAll()
    {
        ObscuredPrefs.DeleteAll();
        userScoreTxt.text = "" + ObscuredPrefs.GetInt("Score");
        userCoinTxt.text = "" + ObscuredPrefs.GetInt("Coin");
    }

}
