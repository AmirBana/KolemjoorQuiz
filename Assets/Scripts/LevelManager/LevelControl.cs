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
    public int NumPart;

    // Start is called before the first frame update
    private void Awake()
    {
        DisableStars();
        /*
        EditorBuildSettingsScene[] AllScene = EditorBuildSettings.scenes;
        names = new string[AllScene.Length];
        for (int k = 0; k < AllScene.Length; k++)
        {
            names[k] = Path.GetFileNameWithoutExtension(AllScene[k].path);
        }
        */
        //LevelName = names[CurrentLevel+ 3];
        //address = "stars" + LevelName;
        address = "stars" + "Part0" + NumPart + "Level" + CurrentLevel;
        print(address);



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
