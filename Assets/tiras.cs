using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class CuttingDetection : MonoBehaviour
{
    [SerializeField] private GameObject doughWorm; // Reference to the dough worm
    public GameObject doughWormItself; // Reference to the dough worm
    [SerializeField] private float cuttingForceThreshold = 1.0f; // Minimum force required for a cut

    private MeshRenderer doughRenderer; // To get the dough worm's material for feedback
    private Material originalMaterial;
    private bool isCortable = false;
    private float minWorm = 0.015f; // Distance between worms

    private bool isInBoard = false;
    public bool isGnocci = false;
    private bool seCocino = false;
    public bool isOnFuente = false;

    public bool isOnPot = false;
    public float zetacio = 0.0f;

    public bool isFloating = false;

    public bool isCooked = false;
    private bool reachedSpecificPoint = false;

    public int cooked = 0;
    private bool movingToPoint = false;
    private Vector3 targetPosition;
    public Vector3 initialPosition;
    private Quaternion initialRotation;
    private float randomNumber = 0.0f;
    void Start()
    {
        StartCoroutine(SliceCooldown());
        Renderer renderer = doughWormItself.GetComponent<Renderer>();
        float minZ1 = 0;
        float maxZ1 = 0;
        if (renderer != null)
        {
            Bounds bounds = renderer.bounds;
            minZ1 = bounds.min.z;
            maxZ1 = bounds.max.z;

        }
        float zeta = Mathf.Abs(minZ1 - maxZ1);
        if (zeta < 0.04f)
        {
            isGnocci = true;
        }
        zetacio = zeta;
        Debug.Log("ZetaInicial: " + zeta + doughWormItself.name + " " + isGnocci);
        if (isFloating)
        {
            StartCoroutine(FloatingBehavior());
            StartCoroutine(UpdateCooked());
        }


    }

    public IEnumerator FloatingBehavior()
    {
        randomNumber = Random.Range(-1.0f, 1.0f);
        while (isFloating)
        {
            if (isFloating)
            {
                if (cooked >= 40 && !reachedSpecificPoint && !movingToPoint)
                {
                    // Start moving to the specific point

                    targetPosition = new Vector3(transform.position.x, transform.position.y + 0.065f, transform.position.z);
                    Debug.Log("Target position: " + targetPosition.x + " " + targetPosition.y + " " + targetPosition.z);
                    movingToPoint = true;
                }

                if (movingToPoint)
                {
                    // Move slowly to the specific point
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);

                    // Check if the position is reached
                    if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
                    {
                        transform.position = targetPosition;
                        reachedSpecificPoint = true;
                        movingToPoint = false;
                        initialPosition = transform.position;
                        isCooked = true;

                    }
                }
                else
                {
                    // Move up and down slightly
                    float newY = initialPosition.y + Mathf.Sin(Time.time + randomNumber) * 0.01f;
                    transform.position = new Vector3(transform.position.x, newY, transform.position.z);

                    // Rotate randomly
                    transform.rotation = initialRotation * Quaternion.Euler(0, Time.time * 10, 0);
                }
            }

            yield return null;
        }
    }

    public IEnumerator UpdateCooked()
    {
        while (isFloating)
        {
            if (isFloating)
            {
                // Update cooked every second
                yield return new WaitForSeconds(1);
                cooked += Random.Range(1, 6);


            }
        }
    }

    private IEnumerator SliceCooldown()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(0.2f);

        // Allow the onion to be sliced
        isCortable = true;
    }
    void OnCollisionEnter(Collision collision)
    {

        if (isCortable && collision.gameObject.CompareTag("knife") && isInBoard)
        {

            // Detect collision point
            Vector3 contactPoint = collision.contacts[0].point;
            Debug.Log("Tirita Collision detected at " + contactPoint);


            Renderer renderer = doughWormItself.GetComponent<Renderer>();
            float minZ1 = 0;
            float maxZ1 = 0;
            if (renderer != null)
            {
                Bounds bounds = renderer.bounds;
                minZ1 = bounds.min.z;
                maxZ1 = bounds.max.z;

            }

            float zeta1 = Mathf.Abs(contactPoint.z - maxZ1);
            float zeta2 = Mathf.Abs(contactPoint.z - minZ1);
            Debug.Log("Zeta1: " + zeta1 + " " + (zeta2 > minWorm) + " Zeta2: " + zeta2 + " " + (zeta1 > minWorm) + " MinWorm: " + minWorm);

            // Perform the cut
            if (zeta1 > minWorm && zeta2 > minWorm)
            {
                Debug.Log("ZetaSI: " + zeta1 + " Zeta: " + zeta2);
                CutDough(contactPoint);
            }
            else
            {
                Debug.Log("sos alto tonto");
            }


            // Optional haptic feedback
            // This requires integration with your VR SDK (e.g., Oculus Integration, SteamVR).

        }
        else
        {
            if (collision.gameObject.CompareTag("cuttingBoard"))
            {
                isInBoard = true;
                doughWormItself.transform.rotation = Quaternion.Euler(90, 0, 0);
            }
            else
            {
                Debug.Log("No se puede cortar" + isCortable + " " + collision.gameObject.tag + " " + collision.gameObject.name);
            }
        }


    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("cuttingBoard"))
        {
            isInBoard = false;

        }
    }

    void CutDough(Vector3 cutPoint)
    {

        // Ensure the dough worm has a collider and a scale representing its length
        Transform doughTransform = doughWormItself.transform;

        // Calculate the local position of the cut relative to the dough worm
        Vector3 localCutPoint = doughTransform.InverseTransformPoint(cutPoint);
        Debug.Log("Local cut point: " + localCutPoint.y + " " + localCutPoint.x + " " + localCutPoint.z);
        // Get the total length of the worm (assumes the worm's Y-axis represents its length)
        float totalLength = doughTransform.localScale.y;

        float porcentaje = (localCutPoint.y - (-0.01f)) / (0.01f - (-0.01f));


        // Determine the lengths of the two new segments
        float leftLength = totalLength * porcentaje; // Distance from start to cut point
        Debug.Log("Porcentaje: " + porcentaje + " Left Length: " + leftLength + " calcu " + localCutPoint.y + "Local cut point: " + localCutPoint.y + " " + localCutPoint.x + " " + localCutPoint.z);

        float rightLength = totalLength - leftLength; // Remaining length

        if (leftLength <= 0 || rightLength <= 0)
        {
            Debug.LogWarning("Invalid cut! Worm length is too small.");
            return;
        }
        Debug.Log("Cutting dough at point: " + cutPoint + " with lengths: " + leftLength + " and " + rightLength + " total: " + totalLength);
        // Create the two new worms
        CreateNewWorm(cutPoint, leftLength, "Left Worm", -1, totalLength);
        CreateNewWorm(cutPoint, rightLength, "Right Worm ", 1, totalLength);

        // Destroy the original worm
        Destroy(doughWormItself);
    }

    void CreateNewWorm(Vector3 origin, float length, string wormName, int direction, float totalLength)
    {
        // Create a new worm GameObject
        GameObject newWorm = Instantiate(doughWorm, origin, doughWormItself.transform.rotation);

        // Update the scale of the new worm
        Vector3 newScale = newWorm.transform.localScale;
        newScale.y = length; // Adjust the length along the X-axis
        newWorm.transform.localScale = newScale;

        Renderer renderer = newWorm.GetComponent<Renderer>();
        float minZ1 = 0;
        float maxZ1 = 0;
        if (renderer != null)
        {
            Bounds bounds = renderer.bounds;
            minZ1 = bounds.min.z;
            maxZ1 = bounds.max.z;

            Debug.Log($"Min Z: {minZ1}, Max Z: {maxZ1}");
        }
        float zeta = Mathf.Abs(minZ1 - maxZ1);

        // Adjust the position to prevent overlap
        float fixedOffset = zeta / 2;

        Vector3 offset = new Vector3(0, 0, fixedOffset * direction); // Offset along X-axis
        newWorm.transform.position += offset;

        // Rename for easier debugging
        int randomId = Random.Range(1000, 9999); // Generate a random number between 1000 and 9999
        newWorm.name = $"{wormName}_{randomId}";
        Debug.Log("Zeta: " + zeta);



        Debug.Log($"{wormName} created at {newWorm.transform.position} with length {length}");
    }



}
