using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDropController : MonoBehaviour
{
<<<<<<< HEAD
    public void OnTriggerEnter2D(Collider2D collision)
=======
    public ItemClass item;

    private void OnTriggerEnter2D(Collider2D collision)
>>>>>>> main
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //dodawanie mineralu do ekwipunku gracza
<<<<<<< HEAD
            
=======
            if(collision.GetComponent<Inventory>().Add(item))
                Destroy(this.gameObject);
>>>>>>> main
        }
    }
}
