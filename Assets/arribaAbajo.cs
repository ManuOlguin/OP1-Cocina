using UnityEngine;

public class UpDownMovement : MonoBehaviour
{
    public float amplitude = 1f;  // The height of the movement
    public float speed = 1f;      // The speed of the movement
    public float delay = 0f;      // The delay or phase offset

    private Vector3 startLocalPosition;
    public bool isRunning = false;
    void Start()
    {
        // Store the initial local position of the object relative to its parent
        startLocalPosition = transform.localPosition;
    }

    void Update()
    {
        /*if (!isRunning)
        {
            return;
        }
        // Calculate the new local Y position with a delay (phase offset)
        float newY = startLocalPosition.y + Mathf.Sin((Time.time * speed) + delay) * amplitude;

        // Apply the new local position
        transform.localPosition = new Vector3(startLocalPosition.x, newY, startLocalPosition.z);*/
    }
}