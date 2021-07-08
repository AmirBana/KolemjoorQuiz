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
}
