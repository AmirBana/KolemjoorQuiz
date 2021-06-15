using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.Storage;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class LevelControl : MonoBehaviour
{
    int LevelsUnlocked;
    public Button Btn;
    public bool LockLevel;
    public string[] names;
    public GameObject Lock;
    public GameObject[] Stars;
    public int CurrentLevel;
    public string LevelName;
    string address;
    int NumOfStars;
    // Start is called before the first frame update
    private void Awake()
    {
        DisableStars();
           EditorBuildSettingsScene[] AllScene = EditorBuildSettings.scenes;
        names = new string[AllScene.Length];
        for (int k = 0; k < AllScene.Length; k++)
        {
            names[k] = Path.GetFileNameWithoutExtension(AllScene[k].path);
        }

        LevelName = names[CurrentLevel+ 3];
         address = "stars" + LevelName;
        //print("Compare: " + address);
    }
    
    void Start()
    {
        print("Addres:" + address);
        NumOfStars = ObscuredPrefs.GetInt(address);

        //print("cur:" + CurrentLevel + "  " + "maxL:" + ObscuredPrefs.GetInt("MaxLevel"));
        if (CurrentLevel <= ObscuredPrefs.GetInt("MaxLevel"))
        {
            LockLevel = false;
        }else
        {
            LockLevel = true;
        }
       
        //LevelsUnlocked = ObscuredPrefs.GetInt("LevelsUnlockedPart1", 1);
        if (LockLevel)
        {
            Lock.SetActive(true);
            Btn.interactable = false;
            DisableStars();
        }
        else
        {
            
            Lock.SetActive(false);
            Btn.interactable = true;
            print("Ob Stars :  " + NumOfStars);
            if (NumOfStars == 1)
            {
                print("111111 starss");
                Stars[0].SetActive(true);
            }
            if (NumOfStars == 2)
            {
                Stars[0].SetActive(true);
                Stars[1].SetActive(true);

            }
            if (NumOfStars == 3)
            {
                Stars[0].SetActive(true);
                Stars[1].SetActive(true);
                Stars[2].SetActive(true);

            }
           // else DisableStars();

        }


    }
    void DisableStars()
    {
        for (int i = 0; i < Stars.Length; i++)
        {
            Stars[i].SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
/*
 *     LevelsUnlocked = ObscuredPrefs.GetInt("LevelsUnlockedPart1", 1);
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
            Buttons[i].transform.GetChild(0).gameObject.SetActive(true); //lock

            Buttons[i].transform.GetChild(1).gameObject.SetActive(false); //star01
            Buttons[i].transform.GetChild(2).gameObject.SetActive(false); //star02
            Buttons[i].transform.GetChild(3).gameObject.SetActive(false); //start03
            
        }
        for (int i = 0; i < LevelsUnlocked; i++)
        {
            Buttons[i].interactable = true;
            Buttons[i].transform.GetChild(0).gameObject.SetActive(false); //lock

            Buttons[i].transform.GetChild(1).gameObject.SetActive(true); //star01
            Buttons[i].transform.GetChild(2).gameObject.SetActive(true); //star02
            Buttons[i].transform.GetChild(3).gameObject.SetActive(true); //start03
        }
        
 * 
 * */