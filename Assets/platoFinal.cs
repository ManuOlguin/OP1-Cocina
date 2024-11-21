using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VRTemplate;
public class platoFinal : MonoBehaviour
{
    public GameObject stepManager;
    public GameObject[] gnoccis;  // Array of carrot slices
    public GameObject water;
    public bool isFilledWithGnocci = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject gnocci in gnoccis)
        {
            gnocci.SetActive(false);
        }
        water.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void agregarGnocci(int cantidad)
    {
        if (!isFilledWithGnocci)
        {
            isFilledWithGnocci = true;
            stepManager.GetComponent<StepManager>().Next(13);
        }
        Debug.Log("Agregando gnoccis" + cantidad);
        int activatedCount = 0;
        for (int i = 0; activatedCount <= cantidad; i++)
        {
            if (!gnoccis[i].activeSelf)
            {
                gnoccis[i].SetActive(true);
                activatedCount++;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pan") && collision.gameObject.GetComponent<Pan>().done)
        {
            foreach (Transform child in collision.gameObject.transform)
        {
            Destroy(child.gameObject);
            water.SetActive(true);
            stepManager.GetComponent<StepManager>().Next(14);

        }
        }
    }
}
