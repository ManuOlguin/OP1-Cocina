using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawerInteraction : MonoBehaviour
{
   private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    private void Start()
    {
        // Get references to the XRGrabInteractable and Rigidbody components
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        // Subscribe to the grab events
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("OnGrab");
        // Detach from parent
        transform.SetParent(null);

        // Enable the Rigidbody to activate physics
        if (rb != null)
        {
            rb.isKinematic = false; // Ensures the Rigidbody responds to physics
            rb.useGravity = true; // Enable gravity if you want it to fall naturally
        }
    }

    private void OnDestroy()
    {
        // Clean up the listener when the object is destroyed
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }
}
