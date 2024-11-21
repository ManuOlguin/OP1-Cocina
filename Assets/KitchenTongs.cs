using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KitchenTongs : MonoBehaviour
{
    public Transform rightArm;
    public Transform grabPosition; // Child transform to define the position where the object will be locked

    public float rightOpenX = 120f; // X rotation for right arm when open
    public float rightCloseX = 100f; // X rotation for right arm when closed

    public float speed = 5f;       // Speed of movement
    private bool isGrabbing = false; // Tracks whether the tongs are squeezed
    private bool isSqueezed = false; // Tracks whether the tongs are squeezed

    private Quaternion targetRightRotation;
    private GameObject grabbedObject = null;


    void Start()
    {
        // Ensure initial rotations are correctly set
        targetRightRotation = Quaternion.Euler(rightOpenX, rightArm.localRotation.eulerAngles.y, rightArm.localRotation.eulerAngles.z);

        rightArm.localRotation = targetRightRotation;
    }

    void Update()
    {
        // Set target rotations based on state

        targetRightRotation = Quaternion.Euler(
            isSqueezed ? rightCloseX : rightOpenX,
            rightArm.localRotation.eulerAngles.y,
            rightArm.localRotation.eulerAngles.z
        );

        // Smoothly interpolate each arm's rotation
        rightArm.localRotation = Quaternion.Slerp(rightArm.localRotation, targetRightRotation, Time.deltaTime * speed);
    }

    public void OnActivate()
    {
        isSqueezed = true;
        if (grabbedObject != null && !isGrabbing && grabbedObject.GetComponent<ObjectProperties>().isHervida)
        {
                        var objectProperties = grabbedObject.GetComponent<ObjectProperties>();

            grabbedObject.GetComponent<UpDownMovement>().isRunning = false;
                        if (objectProperties != null && objectProperties.inPot)
            {
                objectProperties.inPot = false;

                                // Play the particle effect
                if (objectProperties.particleEffect != null)
                {
                    objectProperties.particleEffect.Play();
                }
            }
            grabbedObject.GetComponent<ObjectProperties>().Freeze();



            grabbedObject.transform.SetParent(grabPosition);
            grabbedObject.transform.localPosition = Vector3.zero;
            grabbedObject.transform.localRotation = Quaternion.identity;
            isGrabbing = true;
        }
    }

    public void OnDeactivate()
    {
        isSqueezed = false;
         if (grabbedObject != null)
        {
                            grabbedObject.GetComponent<ObjectProperties>().Unfreeze();

            Debug.Log("Deactivate" + grabbedObject.name);
            grabbedObject.transform.SetParent(null);

            grabbedObject = null;
            isGrabbing = false;


        }
    }

        private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision pinza" + collision.gameObject.tag + collision.gameObject.name);

        if (grabbedObject == null && collision.gameObject.CompareTag("PapaHervida"))
        {
            grabbedObject = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (grabbedObject == collision.gameObject && !isGrabbing)
        {
            Debug.Log("Collision ended" + collision.gameObject.name);
            grabbedObject = null;
        }
    }
}
