using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pelar : MonoBehaviour
{
// Reference to the whole apple and sliced apple models
    public GameObject papa;
    public GameObject papaParticles; // This should be a prefab for spawning

    public string knifeTag = "pelapapa";
    public Material newMaterial;
    public bool isPelada = false;
    public bool isHervida = false; 

    void OnCollisionEnter(Collision collision)
    {

        // Check if the colliding object is the knife
        if (collision.gameObject.CompareTag(knifeTag) && !isPelada)
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
        isPelada = true;

        ParticleSystem particleSystem = papaParticles.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }
}
