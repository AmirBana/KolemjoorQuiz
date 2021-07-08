using System.Collections;
using System.Collections.Generic;

public static class NetworkUtil
{
    public static string FixString(string e)
    {
        e = e.Replace('"',' ').Trim();
        return e;
    }
}
