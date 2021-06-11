using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public Image[] hearts;
    // Start is called before the first frame update
    void Start() {
        ArrangeHearts();
        
    }
    public void ArrangeHearts() {
        print("arrange is done");
        for(int i = 0; i < hearts.Length; i++) {
            if(hearts[i].color == Color.gray) {
                hearts[i].color = Color.white;
            }
        }
        for(int i = 0; i < ObscuredPrefs.GetInt("LostHeart"); i++) {
            if(hearts[i].color == Color.white) {
                hearts[i].color = Color.gray;
            }
        }
       
    }
}
