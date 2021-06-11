using System.Collections;
using System.Collections.Generic;
using System;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using UnityEngine;

public class BackBeforeEnd : MonoBehaviour
{
    public void ExitBeforeEnd() {
        if(WorldTimeAPI.Instance.IsTimeLoaded)
            ObscuredPrefs.SetString("IsLost", WorldTimeAPI.Instance.GetCurrentDateTime().AddMinutes(10).ToString());
        else
            ObscuredPrefs.SetString("IsLost", DateTime.Now.AddMinutes(10).ToString());
    }
}
