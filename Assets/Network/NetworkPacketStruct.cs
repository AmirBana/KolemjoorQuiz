using System.Collections;
using System.Collections.Generic;

public static class NetworkPacketStruct
{
    #region Account
    public static JSONObject UniqueIdentifier(string uid) 
    {
        JSONObject e = new JSONObject(JSONObject.Type.OBJECT);
        e.AddField("uid",uid);
        return e;
    }

    public static JSONObject Account_Login(string Username , string Password)
    {
        JSONObject e = new JSONObject(JSONObject.Type.OBJECT);
        e.AddField("Username", Username);
        e.AddField("Password", Password);
        return e;
    }
    #endregion

    #region Game Data
    public static JSONObject GameData_Coin(int value, string uid)
    {
        JSONObject e = new JSONObject(JSONObject.Type.OBJECT);
        e.AddField("value", value);
        e.AddField("uid", uid);
        return e;
    }
    public static JSONObject GameData_Score(int value, string uid)
    {
        JSONObject e = new JSONObject(JSONObject.Type.OBJECT);
        e.AddField("value", value);
        e.AddField("uid", uid);
        return e;
    }
    public static JSONObject GameData_Heart(int value, string uid)
    {
        JSONObject e = new JSONObject(JSONObject.Type.OBJECT);
        e.AddField("value", value);
        e.AddField("uid", uid);
        return e;
    }
    #endregion
}
