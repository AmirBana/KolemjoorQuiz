using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [System.Serializable]class ShopItem
    {
        public Sprite Image;
        public int Coin;
        public int Price;
        public bool ISPurchased;
    }
    [SerializeField] List<ShopItem> ShopItemList;
    // Start is called before the first frame update
    GameObject Item;
    GameObject New;
    [SerializeField] Transform ShopScrolView;
    void Start()
    {
        Item = ShopScrolView.GetChild(0).gameObject;
        for (int i = 0; i < ShopItemList.Count; i++)
        {
            New = Instantiate(Item, ShopScrolView);
            New.transform.GetChild(0).GetComponent<Image>().sprite = ShopItemList[i].Image;
            New.transform.GetChild(1).GetComponent<Text>().text = ShopItemList[i].Coin.ToString();
            New.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = ShopItemList[i].Price.ToString();
            if (!ShopItemList[i].ISPurchased)
            {
                New.transform.GetChild(2).GetComponent<Button>().interactable = false;
            }

        }
        Destroy(Item);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
