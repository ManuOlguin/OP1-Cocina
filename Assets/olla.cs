using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Unity.VRTemplate;

public class olla : MonoBehaviour
{
    public GameObject waterChild;
    private bool isFilling = false;
    private float fillTime = 4.0f;
    private float contactTime = 0f;
    private bool isInContact = false;
    public bool isFilled = false;
    public bool isBoiling = false;
    public GameObject[] papas;  // Array of carrot slice GameObjects
    private int activePapas = 0;
    public Slider fillBar;  // Reference to the UI Image component
    public float duration = 5f;  // Duration over which the fill bar decreases
    private bool isFilledWithFood = false;
    private bool isFirstGnocci = false;

    public GameObject stepManager;
    public Transform signalObject; // The object that will emit the signal
    public float radius = 0.005f; // The radius within which the signal will be emitted

    public GameObject fuenton;


    void Start()
    {
        waterChild.SetActive(false);
        foreach (GameObject slice in papas)
        {
            slice.SetActive(false);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger" + other.tag);
        if (other.CompareTag("WaterStream"))
        {
            isInContact = true;
            if (!isFilling && contactTime < fillTime)
            {
                Debug.Log("Filling pot");
                waterChild.SetActive(true);
                StartCoroutine(FillPot());
            }
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Papa"))
        {
            Debug.Log("data" + isFilled + isBoiling + collision.gameObject.GetComponent<pelar>().isPelada);
            if (isFilled && isBoiling && collision.gameObject.GetComponent<pelar>().isPelada)
            {
                ActivateOnionSlice();
                Destroy(collision.gameObject);
                if (activePapas == papas.Length)
                {
                    isFilledWithFood = true;
                    stepManager.GetComponent<StepManager>().Next(2);

                    StartCoroutine(DecreaseFillBarOverTime(duration));

                }

            }
        }
        if (collision.gameObject.CompareTag("Fuente"))
        {
            GameObject fuente = collision.gameObject;

            // Iterate through all child objects of "fuente"
            Debug.Log("Processing fuente: " + fuente.transform.childCount + " children found.");
            foreach (Transform child in fuenton.transform)
            {
                if (child != null)
                {
                    Debug.Log("Processing child: " + child.name + " with tag: " + child.tag);
                }
                else
                {
                    Debug.LogWarning("Found a null child in the transform.");
                }
                if (child.CompareTag("tirita"))
                {
                    if(isFirstGnocci == false)
                    {
                        isFirstGnocci = true;
                        stepManager.GetComponent<StepManager>().Next(12);
                    }
                    child.GetComponent<CuttingDetection>().isOnPot = true;
                    child.SetParent(transform); // Re-parent to this GameObject


                    // Place the object randomly within the radius of the signal
                    if (signalObject != null)
                    {
                        Vector3 randomOffset = Random.insideUnitSphere * radius;
                        randomOffset.y = 0; // Optional: Keep everything on the same horizontal plane
                        child.position = signalObject.position + randomOffset;
                    }
                    else
                    {
                        Debug.LogWarning("Signal Object not assigned! Placing gnocci at the center.");
                        child.localPosition = Vector3.zero;
                    }
                    
                    child.GetComponent<CuttingDetection>().isFloating = true;
                    child.GetComponent<CuttingDetection>().initialPosition = child.position;
                    StartCoroutine(child.GetComponent<CuttingDetection>().FloatingBehavior());
                    StartCoroutine(child.GetComponent<CuttingDetection>().UpdateCooked());

                }
            }
        }
    }
    void ActivateOnionSlice()
    {
        if (activePapas < papas.Length)
        {
            papas[activePapas].SetActive(true);
            papas[activePapas].GetComponent<UpDownMovement>().isRunning = true;
            activePapas++;
        }

    }
    private IEnumerator DecreaseFillBarOverTime(float time)
    {

        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            fillBar.value = Mathf.Lerp(1f, 0f, elapsedTime / time);
            yield return null;  // Wait for the next frame
        }

        // Perform the desired action when the time is met
        OnFillBarDepleted();
    }
    private void OnFillBarDepleted()
    {

        Debug.Log("Fill bar is depleted. Perform the desired action here.");
        foreach (GameObject papa in papas)
        {
            papa.GetComponent<ObjectProperties>().isHervida = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WaterStream"))
        {
            isInContact = false;
        }
    }

    IEnumerator FillPot()
    {
        isFilling = true;
        float elapsedTime = 0f;
        Vector3 initialPosition = waterChild.transform.localPosition;
        Vector3 targetPosition = new Vector3(initialPosition.x, initialPosition.y + 0.2f, initialPosition.z); // Adjust target position as needed

        while (contactTime < fillTime)
        {
            if (isInContact)
            {
                contactTime += Time.deltaTime;
            }

            waterChild.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, contactTime / fillTime);
            yield return null;
        }

        waterChild.transform.localPosition = targetPosition;
        isFilling = false;
        isFilled = true;

    }
}