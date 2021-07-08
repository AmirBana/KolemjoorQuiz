using CodeStage.AntiCheat.Storage;
using System.Collections;
using System.Collections.Generic;

public static class GameData
{
    #region Account
    public static string GetUniqID()
    {
        return ObscuredPrefs.GetString("uid");
    }
    #endregion

    #region Coin
    public static void SetCoin(int MCoin)
    {
        ObscuredPrefs.SetInt("Coin", MCoin);
    }
    public static void SumCoin(int MCoin)
    {
        ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") + MCoin);
    }
    public static void TakeCoin(int MCoin)
    {
        ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") - MCoin);
    }
    public static int GetCoin()
    {
        return ObscuredPrefs.GetInt("Coin");
    }
    #endregion

    #region Score
    public static void SetScore(int MScore)
    {
        ObscuredPrefs.SetInt("Score", MScore);
    }
    public static void SumScore(int MScore)
    {
        ObscuredPrefs.SetInt("Score", ObscuredPrefs.GetInt("Score") + MScore);
    }
    public static void TakeScore(int MScore)
    {
        ObscuredPrefs.SetInt("Score", ObscuredPrefs.GetInt("Score") - MScore);
    }
    public static int GetScore()
    {
        return ObscuredPrefs.GetInt("Score");
    }
    #endregion

    #region Heart
    public static void SetHeart(int MHeart)
    {
        ObscuredPrefs.SetInt("Score", MHeart);
    }
    public static void SumHeart(int MHeart)
    {
        ObscuredPrefs.SetInt("Score", ObscuredPrefs.GetInt("Score") + MHeart);
    }
    public static void TakeHeart(int MHeart)
    {
        ObscuredPrefs.SetInt("Score", ObscuredPrefs.GetInt("Score") - MHeart);
    }
    public static int GetHeart()
    {
        return ObscuredPrefs.GetInt("Score");
    }
    #endregion
}
