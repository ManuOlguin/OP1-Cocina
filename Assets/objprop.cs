using UnityEngine;

public class ObjectProperties : MonoBehaviour
{
    public bool inPot = false;
    public ParticleSystem particleEffect; // Reference to the particle system
    public bool isHervida = false;
    public bool unfrez = false;
    private Rigidbody rb;
    public GameObject bowlcito;  // Reference to the spoon GameObject

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log("FreezeStart" + gameObject.name);
        }
    }


    public void Unfreeze()
    {
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.None;
            Debug.Log("Unfreeze" + gameObject.name);
        }
    }

    public void Freeze()
    {
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log("Freeze" + gameObject.name);
        }
    }
    void Update()
    {
        if (!inPot && transform.position.y < -0.1f)
        {
            Destroy(gameObject);
            bowlcito.GetComponent<bowlcito>().ActivatePapaHervida();
            if (bowlcito.GetComponent<bowlcito>().activePapasHervidas == bowlcito.GetComponent<bowlcito>().PapasHervidas.Length)
            {
                bowlcito.GetComponent<bowlcito>().isFilledWithPapas = true;
            }
        }
    }
}