using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class harinita : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bowl"))
        {
           
        }
                    Destroy(gameObject); // Despawn the flour particle

    }
}
