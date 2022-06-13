using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    Inventory inventory;
    public void AddDiamod() 
    {
        inventory.addDiamond();
    }
    public void AddCoal() 
    { 
        inventory.addCoal();
    }
    public void AddIron() 
    { 
        inventory.addIron();
    }
    public void Show()
    {
        Debug.Log(inventory.Diamonds);
    }

    void Start()
    {
        inventory = new Inventory();    
    }
    void Update()
    {
        
    }
}
