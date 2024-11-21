using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class harinita : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Flour Collision detected with " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Harina"))
        {
        }
        else
        {
             if (collision.gameObject.CompareTag("Bowl"))
        {
           
        }
                    Destroy(gameObject); // Despawn the flour particle

        }
       
    }
}
