using System;
using UnityEngine;
using DanielLochner.Assets.SimpleScrollSnap;
public class PartSelector : MonoBehaviour
{
    GameObject[] parts;
    public GameObject ScrollSnap;
    // Start is called before the first frame update
    private void Start() {
        parts = GameObject.FindGameObjectsWithTag("Parts");
        Array.Reverse(parts);
        foreach(GameObject part in parts) {
            part.SetActive(false);
        }
    }
    public void ShowParts() {
        int currentPanel = ScrollSnap.GetComponent<SimpleScrollSnap>().CurrentPanel;
        for(int i = 0; i < parts.Length; i++) {
            if(i == currentPanel) {
                GameObject.Find("Level Canvas").SetActive(false);
                parts[i].SetActive(true);
                break;
            }
        }
    }
}
