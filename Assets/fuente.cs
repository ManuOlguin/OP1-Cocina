using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    using Unity.VRTemplate;
using UnityEngine.XR.Interaction.Toolkit;


public class fuente : MonoBehaviour
{
    public int cantidad = 0;
    public GameObject neutral;
    public GameObject stepManager;

    void Start()
    {


    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("tirita") && collision.gameObject.transform.parent != this.transform && collision.gameObject.GetComponent<CuttingDetection>().isOnFuente == false && collision.gameObject.GetComponent<CuttingDetection>().isOnPot == false && collision.gameObject.GetComponent<CuttingDetection>().isGnocci)
        {
            collision.gameObject.GetComponent<CuttingDetection>().isOnFuente = true;
            cantidad++;
            if(cantidad == 5)
            {
                stepManager.GetComponent<StepManager>().Next(10);
            }
            // Make the collided object a child of this GameObject
            collision.transform.SetParent(neutral.transform);

            // Stop the physics of the collided object
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Disable the XRGrabInteractable component
            UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable = collision.gameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            if (grabInteractable != null)
            {
                grabInteractable.enabled = false;
            }
        }
    }
}
