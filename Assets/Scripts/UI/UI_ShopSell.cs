using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ShopSell : MonoBehaviour
{
    public Text CoinsTxt;
    public int[,] shopItems = new int[6, 6];
    public float Coins;
    public int length;

    void Start()
    {
        CoinsTxt.text = "Coins:" + Coins.ToString();
        //ID
        for (int i = 1; i < length; i++)
        {
            shopItems[1, i] = i + 1;
        }
        //Price
        for (int i = 1; i < length; i++)
        {
            shopItems[2, i] = (i + 1) * 10;
        }

        //Qualitu
        //for (int i = 1; i < length; i++)
        //{
        //   shopItems[3, i] = 20;
        //}


    }
    public void Sell()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        
            Coins += shopItems[2, ButtonRef.GetComponent<ItemsSell>().ItemId];
           // shopItems[3, ButtonRef.GetComponent<ItemsSell>().ItemId]--;
            CoinsTxt.text = "Coins:" + Coins.ToString();

           // ButtonRef.GetComponent<ItemsSell>().QualityText.text = shopItems[3, ButtonRef.GetComponent<ItemsSell>().ItemId].ToString();
        

    }
  

}
