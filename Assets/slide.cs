using UnityEngine;

public class DrawerLimit : MonoBehaviour
{
    public float maxDistance = 0.3f;  // Max distance the drawer can open
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;  // Save the drawer’s starting position
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        float distanceMoved = Vector3.Distance(initialPosition, currentPosition);

        // Clamp the drawer's position so it doesn't exceed max distance
        if (distanceMoved > maxDistance)
        {
            Vector3 direction = (currentPosition - initialPosition).normalized;
            transform.position = initialPosition + direction * maxDistance;
        }
    }
}