using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDropController : MonoBehaviour
{
    public ItemClass item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //dodawanie mineralu do ekwipunku gracza
            if(collision.GetComponent<Inventory>().Add(item))
                Destroy(this.gameObject);
        }
    }
}
