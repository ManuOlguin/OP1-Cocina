using UnityEngine;
using UnityEngine.UI;
using Unity.VRTemplate;
public class Pan : MonoBehaviour
{
    public GameObject spoon;  // Reference to the spoon GameObject
    public Image fillBar;     // Reference to the UI Image component
    public GameObject[] carrotSlices;  // Array of carrot slice GameObjects
    public GameObject[] onionSlices;
    public GameObject waterChild;
    public float movementThreshold = 7f;  // Minimum movement to consider as active
    public bool isOnStove = false;  // Flag to check if the pan is on the stove
    public bool isFilledWithFood = false;  // Flag to check if the pan is filled with food
    private Vector3 lastPosition;
    private Vector3 velocity;
    private bool isCollidingWithPan = false;  // Flag to check if spoon is in contact with the pan
    public medidor medidor;
    public GameObject stepManager;
    private int activeCarrotSlices = 0;
    private int activeOnionSlices = 0;
    public bool estaListoParaSalsa = false;
    public bool done = false;
    private bool isMezcladoFirst = false;

    void Start()
    {
        waterChild.SetActive(false);
        if (spoon != null)
        {
            lastPosition = spoon.transform.position;
        }
        foreach (GameObject slice in carrotSlices)
        {
            slice.SetActive(false);
        }
        foreach (GameObject slice in onionSlices)
        {
            slice.SetActive(false);
        }
    }

    void Update()
    {
        // Only track spoon velocity if it is colliding with the pan
        if (isCollidingWithPan && spoon != null && isOnStove && isFilledWithFood)
        {
            if(!isMezcladoFirst)
            {
                stepManager.GetComponent<StepManager>().Next(5);
                isMezcladoFirst = true;
                Debug.Log("Mezclado");
            }
            Debug.Log("Spoon is in contact with the pan");

            // Calculate velocity manually
            Vector3 currentPosition = spoon.transform.position;
            velocity = (currentPosition - lastPosition) / Time.deltaTime;
            lastPosition = currentPosition;


            // Check if the velocity magnitude exceeds the threshold
            if (velocity.magnitude > movementThreshold)
            {
                            Debug.Log("Calculated Velocity: " + velocity.magnitude + " m/s" + " " + movementThreshold);

                Debug.Log("Spoon is being moved with high velocity");

                // Call function to fill the bar here
                FillBar();
            }
        }
    }

    // Function to fill the bar (you can modify this based on your game logic)
    void FillBar()
    {
        if (fillBar != null)
        {
            // Logic to update the sauce's cooking progress based on spoon movement
            // e.g., incrementing the fill amount
            fillBar.fillAmount += 0.01f; // Adjust the increment value as needed

            // Clamp the fill amount to ensure it stays between 0 and 1
            fillBar.fillAmount = Mathf.Clamp(fillBar.fillAmount, 0f, 1f);
        }
    }

    // Detect when the spoon enters collision with the pan
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("aCollision detectedadada" + collision.gameObject.name);
        if(collision.gameObject.CompareTag("tomato") && estaListoParaSalsa && !waterChild.activeSelf)
        {
            waterChild.SetActive(true);
            done = true;
            stepManager.GetComponent<StepManager>().Next(9);
            medidor.started = false;

        }
        if (collision.gameObject.CompareTag("Spoon"))
        {
            Debug.Log("Collision detected" + collision.gameObject.name);

            isCollidingWithPan = true;
        }
        if (collision.gameObject.CompareTag("OnionSlice"))
        {
            if(activeOnionSlices > onionSlices.Length)
            {
            }
            else
            {
            ActivateOnionSlice();
            Destroy(collision.gameObject);  // Destroy the colliding onion slice
            }

        }
        if (collision.gameObject.CompareTag("CarrotSlice"))
        {
            if(activeCarrotSlices > carrotSlices.Length)
            {
            }
            else
            {
            ActivateCarrotSlice();
            Destroy(collision.gameObject);  // Destroy the colliding onion slice
            }

        }
        if (activeCarrotSlices == carrotSlices.Length && activeOnionSlices == onionSlices.Length && !isFilledWithFood)
        {
            isFilledWithFood = true;
            stepManager.GetComponent<StepManager>().Next(4);
            if (isOnStove && medidor.GetComponent<medidor>().started == false)
            {
                medidor.StartDecreasingFillAmount();
            }
        }
    }

    // Detect when the spoon exits collision with the pan
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spoon"))
        {
            Debug.Log("Collision ended" + collision.gameObject.name);

            isCollidingWithPan = false;
        }
    }
    void ActivateCarrotSlice()
    {
        if (activeCarrotSlices < carrotSlices.Length)
        {
            carrotSlices[activeCarrotSlices].SetActive(true);
            activeCarrotSlices++;
        }
    }

    void ActivateOnionSlice()
    {
        if (activeOnionSlices < onionSlices.Length)
        {
            onionSlices[activeOnionSlices].SetActive(true);
            activeOnionSlices++;
        }
    }
}