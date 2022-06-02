using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    public int ItemId;
    public Text PriceTxt;
    public Text QualityText;
    public GameObject UI_Shop;
    
    // Update is called once per frame
    void Update()
    {
        PriceTxt.text = "Price: $" + UI_Shop.GetComponent<UI_Shop>().shopItems[2,ItemId].ToString();
        QualityText.text =  UI_Shop.GetComponent<UI_Shop>().shopItems[3, ItemId].ToString();
    }
}
