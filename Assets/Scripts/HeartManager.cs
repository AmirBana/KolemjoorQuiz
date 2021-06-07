using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public Image[] hearts;
    // Start is called before the first frame update
    void Start() {
        ArrangeHearts();
        
    }
    void ArrangeHearts() {
        for(int i = 0; i < ObscuredPrefs.GetInt("LostHeart"); i++) {
            if(hearts[i].color == Color.white) {
                hearts[i].color = Color.gray;
            }
        }
       
    }
}
