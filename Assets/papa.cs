using UnityEngine;

public class PapaObj : MonoBehaviour
{
    // Reference to the whole apple and sliced apple models
    public GameObject papa;
    public GameObject slicedPapa; // This should be a prefab for spawning


    // The tag to identify the knife
    public string knifeTag = "knife";

    void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is the knife
        if (collision.gameObject.CompareTag(knifeTag))
        {
            SlicePapa();
        }
    }

    void SlicePapa()
    {
        pelar pelarScript = papa.GetComponent<pelar>();

        if (pelarScript != null)
        {
            if (pelarScript.isPelada)
            {
                // Hide the whole apple
                papa.SetActive(false);

                // Get the position and rotation of the whole apple in world space
                Vector3 applePosition = papa.transform.position;
                Quaternion appleRotation = papa.transform.rotation;

                // Offset positions for two halves
                Vector3 offset = papa.transform.right * 0.1f; // Right is based on apple's orientation

                // Instantiate two sliced apple pieces with slight offset
                Instantiate(slicedPapa, applePosition, appleRotation);
                Debug.Log("Apple sliced" + applePosition + "," + appleRotation);

                appleRotation.y = appleRotation.y + 180;
                Instantiate(slicedPapa, applePosition, appleRotation);
                Debug.Log("Apple sliced" + applePosition + "," + appleRotation);

                // Optional: Add additional logic here, like playing a sound or particle effect
            }
            else
            {
                Debug.Log("Papa is not pelada yet");
            }
        }
    }
}