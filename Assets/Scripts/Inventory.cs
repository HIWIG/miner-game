using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory")]
public class Inventory : ScriptableObject
{
    
    public int Diamonds { get; set; } =0;

    public int Coal { get; set; }
    public int Iron { get; set; }

    public void addDiamond()
    {
        Diamonds++;
    }
    public void addCoal()
    {
        Coal++;
    }
    public void addIron()
    {
        Iron++;
    }

}
