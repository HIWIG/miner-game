using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsSell : MonoBehaviour
{
    public int ItemId;
    public Text PriceTxt;
  //  public Text QualityText;
    public GameObject UI_ShopSell;
    
    // Update is called once per frame
    void Update()
    {
        PriceTxt.text = "Price: $" + UI_ShopSell.GetComponent<UI_ShopSell>().shopItems[2,ItemId].ToString();
     //   QualityText.text =  UI_ShopSell.GetComponent<UI_ShopSell>().shopItems[3, ItemId].ToString();

       }
}
