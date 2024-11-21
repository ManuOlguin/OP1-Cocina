using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VRTemplate;
using TMPro; // Add this at the top of your script

public class contador : MonoBehaviour
{
public TMP_Text countdownText; // Reference to the TextMeshPro UI Text element    public GameObject fuente; // Reference to the fuente GameObject
    public GameObject particleEffectPrefab; // Reference to the particle effect prefab
    private float countdownTime = 80f; // Countdown time in seconds
    public GameObject fuente;
public GameObject stepManager;
    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        while (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            countdownText.text =  Mathf.Ceil(countdownTime).ToString() + "s";
            yield return null;
        }

        countdownText.text = "0s";
        stepManager.GetComponent<StepManager>().Next(11);
        DestroyGnocci();
    }

    void DestroyGnocci()
    {
        GameObject[] gnoccis = GameObject.FindGameObjectsWithTag("tirita");
        foreach (GameObject gnocci in gnoccis)
        {
            if (gnocci.transform.parent == null || gnocci.transform.parent.gameObject != fuente)
            {
                Instantiate(particleEffectPrefab, gnocci.transform.position, Quaternion.identity);
                Destroy(gnocci);
            }
        }
        /*Rigidbody fuenteRigidbody = fuente.GetComponent<Rigidbody>();
    if (fuenteRigidbody != null)
    {
        fuenteRigidbody.constraints = RigidbodyConstraints.None;
    }

    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable xrGrab = fuente.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    if (xrGrab != null)
    {
        xrGrab.enabled = true;
    }
      */  
    }
}