using CodeStage.AntiCheat.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinControl : MonoBehaviour
{

    public static void ManageCoin(int MCoin)
    {
        ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") + MCoin);

    }
    public static void ManageScore(int MScore)
    {
        ObscuredPrefs.SetInt("Score", ObscuredPrefs.GetInt("Score") + MScore);

    }

}
