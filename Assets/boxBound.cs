using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxBound : MonoBehaviour
{
    public GameObject bowlcito;  // Reference to the spoon GameObject
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OutOfBounds Collided with " + other.gameObject.tag);
        if(other.gameObject.tag == "PapaHervida")
        {
            Destroy(other.gameObject);
            bowlcito.GetComponent<bowlcito>().ActivatePapaHervida();
            if (bowlcito.GetComponent<bowlcito>().activePapasHervidas == bowlcito.GetComponent<bowlcito>().PapasHervidas.Length)
            {
                bowlcito.GetComponent<bowlcito>().isFilledWithPapas = true;
            }
        }
    }
}
