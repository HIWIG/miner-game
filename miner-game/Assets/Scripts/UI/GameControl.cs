using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameControl : MonoBehaviour
{
    public GameObject Item;
    public GameObject ItemSell;
    public GameObject Menu;

    public void hight()
    {
        Item.SetActive(false);
    }
    public void Show()
    {
        Item.SetActive(true);
        HideButtonBuySell();
    }

    public void hightSell()
    {
        ItemSell.SetActive(false);
    }
    public void ShowSell()
    {
        ItemSell.SetActive(true);
        HideButtonBuySell();
    }

    public void HideButtonBuySell()
    {
        Menu.SetActive(false);
    }
    public void ShowButtonBuySell()
    {
        Menu.SetActive(true);
        hight();
        hightSell();
    }
}
