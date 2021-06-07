using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.Storage;
public class HomeScene : MonoBehaviour
{
    public GameObject chanceWheel;
    public Text userScoreTxt;
    public Text userCoinTxt;

    private void Start()
    {
        userScoreTxt.text = "" + ObscuredPrefs.GetInt("Score");
        userCoinTxt.text = "" + ObscuredPrefs.GetInt("Coin");
    }
    public void WheelChance()
    {
        chanceWheel.GetComponent<Canvas>().sortingOrder = 2;
    }
    public void BackWheelChance()
    {
        chanceWheel.GetComponent<Canvas>().sortingOrder = -1;
    }

}
