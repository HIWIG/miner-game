using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDropController : MonoBehaviour
{

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            //dodawanie mineralu do ekwipunku gracza

            if (this.gameObject.GetComponent<SpriteRenderer>().sprite.ToString() == "ore_coal (UnityEngine.Sprite)")
            {
                GameObject.FindGameObjectWithTag("Stat").GetComponent<StatsController>().AddCoal();
            }

            else if (this.gameObject.GetComponent<SpriteRenderer>().sprite.ToString() == "ore_iron (UnityEngine.Sprite)")
            {
                GameObject.FindGameObjectWithTag("Stat").GetComponent<StatsController>().AddIron();
            }

            else if (this.gameObject.GetComponent<SpriteRenderer>().sprite.ToString() == "ore_diamond (UnityEngine.Sprite)")
            {
                GameObject.FindGameObjectWithTag("Stat").GetComponent<StatsController>().AddDiamod();
            }
        }
    }
}
