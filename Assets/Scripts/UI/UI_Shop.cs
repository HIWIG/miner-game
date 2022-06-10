using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    
    public int[,] shopItems = new int[6, 6];
    
    public Text CoinsTxt;
    public int length;
    public float Coins;
    


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
            shopItems[2, i] = (i + 1 )*10;
        }

        //Qualitu
        for (int i = 1; i < length; i++)
        {
            shopItems[3, i] = 0;
        }
         
        
    }
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (GameObject.Find("Coin").GetComponent<CoinControler>().GetCoin() >= shopItems[2, ButtonRef.GetComponent<Items>().ItemId])
        {
            GameObject.Find("Coin").GetComponent<CoinControler>().Delete(shopItems[2, ButtonRef.GetComponent<Items>().ItemId]);
            shopItems[3, ButtonRef.GetComponent<Items>().ItemId]++;
            //CoinsTxt.text = "Coins:" + Coins.ToString();

            CoinsTxt.text = "Coins:" + GameObject.Find("Coin").GetComponent<CoinControler>().GetCoin();

            ButtonRef.GetComponent<Items>().QualityText.text = shopItems[3, ButtonRef.GetComponent<Items>().ItemId].ToString();
        }
        
    }

}
