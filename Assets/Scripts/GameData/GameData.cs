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
        SyncCoin();
    }
    public static void SumCoin(int MCoin)
    {
        ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") + MCoin);
        SyncCoin();
    }
    public static void TakeCoin(int MCoin)
    {
        ObscuredPrefs.SetInt("Coin", ObscuredPrefs.GetInt("Coin") - MCoin);
        SyncCoin();
    }
    public static int GetCoin()
    {
        return ObscuredPrefs.GetInt("Coin");
    }
    private static void SyncCoin()
    {
        Network.Socket.Emit("gamedata:UpdateCoin",NetworkPacketStruct.GameData_Coin(GetCoin(),GetUniqID()));
    }
    #endregion

    #region Score
    public static void SetScore(int MScore)
    {
        ObscuredPrefs.SetInt("Score", MScore);
        SyncScore();
    }
    public static void SumScore(int MScore)
    {
        ObscuredPrefs.SetInt("Score", ObscuredPrefs.GetInt("Score") + MScore);
        SyncScore();
    }
    public static void TakeScore(int MScore)
    {
        ObscuredPrefs.SetInt("Score", ObscuredPrefs.GetInt("Score") - MScore);
        SyncScore();
    }
    public static int GetScore()
    {
        return ObscuredPrefs.GetInt("Score");
    }
    private static void SyncScore()
    {
        Network.Socket.Emit("gamedata:UpdateScore", NetworkPacketStruct.GameData_Score(GetScore(), GetUniqID()));
    }
    #endregion

    #region Heart
    public static void SetHeart(int MHeart)
    {
        ObscuredPrefs.SetInt("Heart", MHeart);
        SyncHeart();
    }
    public static void SumHeart(int MHeart)
    {
        ObscuredPrefs.SetInt("Heart", ObscuredPrefs.GetInt("Heart") + MHeart);
        SyncHeart();
    }
    public static void TakeHeart(int MHeart)
    {
        ObscuredPrefs.SetInt("Heart", ObscuredPrefs.GetInt("Heart") - MHeart);
        SyncHeart();
    }
    public static int GetHeart()
    {
        return ObscuredPrefs.GetInt("Heart");
    }
    private static void SyncHeart()
    {
        Network.Socket.Emit("gamedata:UpdateHeart", NetworkPacketStruct.GameData_Heart(GetHeart(), GetUniqID()));
    }
    #endregion
}
