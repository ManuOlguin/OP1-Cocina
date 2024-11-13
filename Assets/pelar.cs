using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pelar : MonoBehaviour
{
// Reference to the whole apple and sliced apple models
    public GameObject papa;
    public GameObject papaParticles; // This should be a prefab for spawning
    // The tag to identify the knife
    public string knifeTag = "knife";
    public Material newMaterial;

    void OnCollisionEnter(Collision collision)
    {
                    Debug.Log("Papa peladasadasd");

        // Check if the colliding object is the knife
        if (collision.gameObject.CompareTag(knifeTag))
        {
            Debug.Log("Papa pelada");
            pelarPapa();
        }
    }

    void pelarPapa()
    {
        Renderer renderer = papa.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = newMaterial;
        }

        ParticleSystem particleSystem = papaParticles.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }
}
