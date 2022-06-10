using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinControler : MonoBehaviour
{
    public int Coins;
    void Start()
    {
        
    }
    public void Add(int ilosc )
    {
        Coins += ilosc; 
    }
    public void Delete(int ilosc)
    {
        Coins-= ilosc;
    }
    public int GetCoin()
    {
        return Coins;
    }
    
    void Update()
    {
        
    }
}
