using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class masita : MonoBehaviour
{
    // Start is called before the first frame update
    private Renderer objectRenderer;
    private Rigidbody rb;
    private Collider objectCollider;
    public GameObject tira;  // Array of carrot slice GameObjects
    public GameObject bowlcito;  // Reference to the GameObject

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            // Make the object invisible
            objectRenderer.enabled = false;
        }
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        objectCollider = GetComponent<Collider>();
        if (objectCollider != null)
        {
            objectCollider.enabled = true;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("cuttingBoard"))
        {
            Instantiate(tira, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    public void ActivateMasita()
    {
        // Make the object visible
        objectRenderer.enabled = true;
        objectCollider.enabled = true;
        rb.constraints = RigidbodyConstraints.None;
        transform.SetParent(null);
        if (bowlcito.GetComponent<bowlcito>().masaLista)
        {
            bowlcito.GetComponent<bowlcito>().grabMasa();
        }

    }
}
