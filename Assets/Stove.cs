using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VRTemplate;
public class PotSnapToStove : MonoBehaviour
{
    public Transform snapPosition; // Reference for where the pot should snap on the stove
    public Transform snapPositionPan; // Reference for where the pot should snap on the stove
    public medidor medidor;
    public GameObject potienzo;
    public float heatingRate = 0.00000167f; // Degrees per second
    public Slider progressBar; // Reference to the progress bar slider

    public float coolingRate = 5f; // Degrees per second
    public float maxTemperature = 10f; // Boiling point
    public float minTemperature = 0f; // Starting temperature
    public ParticleSystem steamParticles;
    private Transform snappedPot; // Reference to the currently snapped pot
    public float waterTemperature = 0f; // Current temperature of the water
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.MainModule mainModule;
    private Coroutine temperatureCoroutine;
    private bool yaEsta = false;

    public GameObject stepManager;

    void Start()
    {
        progressBar.gameObject.SetActive(false); // Hide the progress bar at the start

        if (steamParticles != null)
        {
            emissionModule = steamParticles.emission;
            mainModule = steamParticles.main;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is the pot
        if (other.CompareTag("Pot"))
        {
            Debug.Log("Pot detected" + other.GetComponent<olla>().isFilled);

            if (other.GetComponent<olla>().isFilled == true)
            {
stepManager.GetComponent<StepManager>().Next(1);
                progressBar.gameObject.SetActive(true); // Show the progress bar when the pot is snapped to the stove

                // Snap the pot to the specified position
                other.transform.position = snapPosition.position;
                other.transform.rotation = snapPosition.rotation;

                // Freeze the pot's position and rotation
                Rigidbody potRb = other.GetComponent<Rigidbody>();
                if (potRb != null)
                {
                    potRb.constraints = RigidbodyConstraints.FreezeAll;

                }

                // Disable the XRGrabInteractable component to prevent grabbing
                UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabComponent = other.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
                if (grabComponent != null)
                {
                    grabComponent.enabled = false;
                }
                if (temperatureCoroutine != null) StopCoroutine(temperatureCoroutine);
                temperatureCoroutine = StartCoroutine(ChangeTemperature(heatingRate));
            }

        }
        if (other.CompareTag("Pan") && !yaEsta)
        {
            // Snap the pan to the specified position
            other.transform.position = snapPositionPan.position;
            other.transform.rotation = snapPositionPan.rotation;
            stepManager.GetComponent<StepManager>().Next(3);
            // Get the Rigidbody component
            Rigidbody panRb = other.GetComponent<Rigidbody>();
            if (panRb != null)
            {
                // Freeze the position and rotation of the pan to simulate snapping
                panRb.constraints = RigidbodyConstraints.FreezeAll; // Locks movement and rotation
            }

            // Disable the XRGrabInteractable component to prevent grabbing
            UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabComponent = other.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            if (grabComponent != null)
            {
                grabComponent.enabled = false;
            }

            // Mark the pan as being on the stove
            Pan panComponent = other.GetComponent<Pan>();
            if (panComponent != null)
            {
                panComponent.isOnStove = true;

                // Start the medidor logic if conditions are met
                if (panComponent.isFilledWithFood && !medidor.GetComponent<medidor>().started)
                {
                    medidor.StartDecreasingFillAmount();
                }
            }
        }

    }

    public void UnsnapPan(GameObject pan){

    // Get the Rigidbody component
    Rigidbody panRb = pan.GetComponent<Rigidbody>();
    if (panRb != null)
    {
        // Unfreeze the position and rotation of the pan
        panRb.constraints = RigidbodyConstraints.None; // Unlocks movement and rotation
    }

    // Enable the XRGrabInteractable component to allow grabbing
    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabComponent = pan.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    if (grabComponent != null)
    {
        grabComponent.enabled = true;
    }

    // Mark the pan as not being on the stove
    Pan panComponent = pan.GetComponent<Pan>();
    if (panComponent != null)
    {
        panComponent.isOnStove = false;
        yaEsta = true;
    }
}

    IEnumerator ChangeTemperature(float rate)
    {

        while (true)
        {
            // Update the temperature based on the rate
            waterTemperature += rate * Time.deltaTime;
            waterTemperature = Mathf.Clamp(waterTemperature, minTemperature, maxTemperature);

            // Update the particle system
            UpdateParticleSystem();

            // Stop the coroutine if temperature reaches min or max
            if ((rate > 0 && waterTemperature >= maxTemperature) ||
                (rate < 0 && waterTemperature <= minTemperature))
            {
                 if (rate > 0 && waterTemperature >= maxTemperature)
            {
                Debug.Log("Pot is boiling");
                var potProperties = potienzo.GetComponent<olla>();

                if (potProperties != null)
                {
                    potProperties.isBoiling = true;
                }
                Debug.Log("Pot is boiling" + potProperties.isBoiling);
            }
                break;
            }

            yield return null; // Wait for the next frame
        }
    }

    void UpdateParticleSystem()
    {
        if (steamParticles == null) return;

        // Calculate normalized temperature (0 = min, 1 = max)
        float normalizedTemperature = waterTemperature / maxTemperature;

        // Adjust emission rate and particle size based on temperature
        emissionModule.rateOverTime = Mathf.Lerp(0f, 10f, normalizedTemperature); // Adjust as needed
        mainModule.startSize = Mathf.Lerp(0.1f, 0.5f, normalizedTemperature); // Adjust as needed

        // Start or stop the particle system based on temperature
        if (waterTemperature > 0 && !steamParticles.isPlaying)
        {
            steamParticles.Play();
        }
        else if (waterTemperature <= 0 && steamParticles.isPlaying)
        {
            steamParticles.Stop();
        }
        if (progressBar != null)
        {
            progressBar.value = normalizedTemperature; // Set slider value (0 to 1)

            // Change the color based on the normalized temperature
            var fillImage = progressBar.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                fillImage.color = Color.Lerp(Color.blue, Color.red, normalizedTemperature);
            }

        }
    }

    /*    private void OnTriggerExit(Collider other)
        {
            // Re-enable grabbing and physics when pot is removed from stove
            if (other.CompareTag("Pot"))
            {
                Rigidbody potRb = other.GetComponent<Rigidbody>();
                if (potRb != null)
                {
                    potRb.isKinematic = false;
                }

                XRGrabInteractable grabComponent = other.GetComponent<XRGrabInteractable>();
                if (grabComponent != null)
                {
                    grabComponent.enabled = true;
                }
            }
        }*/
}
