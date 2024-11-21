using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VRTemplate;
public class espumadera : MonoBehaviour
{
    public int contador = 0;
    public int contadorTotal = 0;
    public GameObject[] gnocci;
    public GameObject final;
public GameObject stepManager;
public GameObject stove;
    public int acumulador = 0;
    private bool isFirstGnocci = true;
    public GameObject pancito;
    public float acumSize = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gnocci.Length; i++)
        {
            gnocci[i].SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "tirita" && collision.gameObject.GetComponent<CuttingDetection>().isCooked && contador < gnocci.Length)
        {
            Debug.Log("Colision con espumadera");
            contador++;
            contadorTotal++;
            acumulador+= collision.gameObject.GetComponent<CuttingDetection>().cooked;
            gnocci[contador].SetActive(true);
            acumSize += collision.gameObject.GetComponent<CuttingDetection>().zetacio;
                        collision.gameObject.SetActive(false);

        }
        if(collision.gameObject.tag == "Plato")
        {
            for (int i = 0; i < gnocci.Length; i++)
            {
                gnocci[i].SetActive(false);
            }
            stove.GetComponent<PotSnapToStove>().UnsnapPan(pancito);
            final.GetComponent<platoFinal>().agregarGnocci(contador);
            contador = 0;
        }
    }
}
