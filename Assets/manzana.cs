using UnityEngine;

public class AppleSlicer : MonoBehaviour
{
    // Reference to the whole apple and sliced apple models
    public GameObject wholeApple;
    public GameObject slicedApplePrefab; // This should be a prefab for spawning

    // The tag to identify the knife
    public string knifeTag = "Knife";

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is the knife
        if (collision.gameObject.CompareTag(knifeTag))
        {
            SliceApple();
        }
    }

    void SliceApple()
    {
        // Hide the whole apple
        wholeApple.SetActive(false);

        // Get the position and rotation of the whole apple in world space
        Vector3 applePosition = wholeApple.transform.position;
        Quaternion appleRotation = wholeApple.transform.rotation;

        // Offset positions for two halves
        Vector3 offset = wholeApple.transform.right * 0.1f; // Right is based on apple's orientation

        // Instantiate two sliced apple pieces with slight offset
        Instantiate(slicedApplePrefab, applePosition, appleRotation);
        Debug.Log("Apple sliced" + applePosition + "," + appleRotation);
        
        appleRotation.y = appleRotation.y + 180;
        Instantiate(slicedApplePrefab, applePosition, appleRotation);
        Debug.Log("Apple sliced" + applePosition + "," + appleRotation);

        // Optional: Add additional logic here, like playing a sound or particle effect
    }
}