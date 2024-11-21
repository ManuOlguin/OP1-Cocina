using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Onion : MonoBehaviour
{
    // Reference to the whole apple and sliced apple models
    public GameObject onion;
    public GameObject NextOnion; // This should be a prefab for spawning

    private bool canBeSliced = false;

    void Start()
    {
        // Start the cooldown coroutine
        StartCoroutine(SliceCooldown());
    }
    // The tag to identify the knife
    public string knifeTag = "knife";

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is the knife
        if (collision.gameObject.CompareTag(knifeTag) && canBeSliced)
        {
            SliceOnion();
        }
    }

    void SliceOnion()
    {


        // Hide the whole apple
        onion.SetActive(false);

        // Get the position and rotation of the whole apple in world space
        Vector3 applePosition = onion.transform.position;
        Quaternion appleRotation = onion.transform.rotation;

        // Offset positions for two halves
        Vector3 offset = onion.transform.right * 0.1f; // Right is based on apple's orientation

        // Instantiate two sliced apple pieces with slight offset
        Instantiate(NextOnion, applePosition, appleRotation);
        Debug.Log("Apple sliced" + applePosition + "," + appleRotation);

        appleRotation.y = appleRotation.y + 180;
        Instantiate(NextOnion, applePosition, appleRotation);
        Debug.Log("Apple sliced" + applePosition + "," + appleRotation);

        // Optional: Add additional logic here, like playing a sound or particle effect
    }
     private IEnumerator SliceCooldown()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Allow the onion to be sliced
        canBeSliced = true;
    }
}