using UnityEngine;
using System.Collections;

public class Carrot : MonoBehaviour
{
    // Reference to the whole carrot and sliced carrot models
    public GameObject carrot;
    public GameObject carrotSlice; // This should be a prefab for spawning

    // The tag to identify the knife
    public string knifeTag = "knife";

    // Boolean to track if the carrot can be sliced
    private bool canBeSliced = false;

    // Configurable offset direction and magnitude
    public Vector3 sliceOffsetDirection = Vector3.right;
    public float sliceOffsetMagnitude = 0.05f;

    // Configurable rotation offset
    public Vector3 sliceRotationOffset = Vector3.zero;

    void Start()
    {
        // Start the cooldown coroutine
        StartCoroutine(SliceCooldown());
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is the knife and if the carrot can be sliced
        if (collision.gameObject.CompareTag(knifeTag) && canBeSliced)
        {
            SliceCarrot();
        }
    }

    void SliceCarrot()
    {
        // Hide the whole carrot
        carrot.SetActive(false);

        // Get the position and rotation of the whole carrot in world space
        Vector3 carrotPosition = carrot.transform.position;
        Quaternion carrotRotation = carrot.transform.rotation;

        // Offset positions for six slices
        Vector3 offset = carrotRotation * sliceOffsetDirection * sliceOffsetMagnitude;

        // Instantiate six sliced carrot pieces with slight offset
        for (int i = 0; i < 6; i++)
        {
            Vector3 slicePosition = carrotPosition + offset * (i - 2.5f); // Adjust the offset to center the slices
            Quaternion sliceRotation = carrotRotation * Quaternion.Euler(sliceRotationOffset); // Apply rotation offset
            Instantiate(carrotSlice, slicePosition, sliceRotation);
        }

        // Optional: Add additional logic here, like playing a sound or particle effect
    }

    private IEnumerator SliceCooldown()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Allow the carrot to be sliced
        canBeSliced = true;
    }
}