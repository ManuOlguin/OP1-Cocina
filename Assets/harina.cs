using UnityEngine;


public class Harina : MonoBehaviour
{
public GameObject flourParticlePrefab; // Prefab for the flour particle
public Transform flourBagTransform; // Reference to the flour bag transform
public int flourAmount = 100; // Number of flour particles to spawn
public float upsideDownThreshold = 0.5f; // Threshold for detecting "upside down" (adjust as needed)

private void Update()
{
    // Check if the bag is upside down based on its rotation
    if (IsUpsideDown())
    {
        FlipFlourBag();
    }
}

bool IsUpsideDown()
{
    // Calculate the dot product between the bag's up vector and the world up vector
    float dotProduct = Vector3.Dot(flourBagTransform.up, Vector3.up);

    // If the dot product is less than the threshold, it means the bag is upside down
    return dotProduct < -upsideDownThreshold;
}

void FlipFlourBag()
{

        // Calculate a small random offset along the X and Z axes
        Vector3 randomOffset = new Vector3(Random.Range(-0.05f, 0.05f), 0, Random.Range(-0.05f, 0.05f));
        
        // The position for the flour particle should be above the bag, along the 'up' direction
        Vector3 spawnPosition = flourBagTransform.position + flourBagTransform.up * 0.1f + randomOffset;

        // Instantiate the flour particle at the calculated position with no rotation (or use a random rotation if desired)
        Instantiate(flourParticlePrefab, spawnPosition, Quaternion.identity);
}
}