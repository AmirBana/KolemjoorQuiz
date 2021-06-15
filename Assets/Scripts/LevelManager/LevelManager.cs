using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.Storage;

public class LevelManager : MonoBehaviour
{
    //public int Mlevel;
    // Start is called before the first frame update
    void Awake()
    {
        if (ObscuredPrefs.GetInt("MaxLevel") == 0) ObscuredPrefs.SetInt("MaxLevel", 1);
        //ObscuredPrefs.SetInt("MaxLevel", Mlevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
