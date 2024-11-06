using UnityEngine;

public class Bowl : MonoBehaviour
{
    public GameObject appleSlice1;
    public GameObject appleSlice2;
    public GameObject pearSlice1;
    public GameObject pearSlice2;
    public GameObject banana;

    public GameObject whiskIn;


    public GameObject bowlEmpty;
    public GameObject bowlFull;
    public GameObject whisk; // Assign the whisk GameObject in the inspector

    private int appleCount = 0;
    private int pearCount = 0;
    private bool bananaIn = false;

    private bool isWhiskingMoment = false;

    private bool isWhisking = false;
    private bool isWhiskIn = false;

    private float whiskingTime = 3.0f; // Duration of whisking in seconds
    private float whiskingTimer = 0.0f;

    private Vector3 lastWhiskPosition;
    private float whiskingThreshold = 0.1f; // Adjust this value based on your needs

    void Start()
    {
        appleSlice1.SetActive(false);
        appleSlice2.SetActive(false);
        pearSlice1.SetActive(false);
        pearSlice2.SetActive(false);
        banana.SetActive(false);
        bowlEmpty.SetActive(true);
        bowlFull.SetActive(false);
        whiskIn.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);
        GameObject other = collision.gameObject;

        if (other.CompareTag("AppleSlice"))
        {
            if (appleCount < 2)
            {
                appleCount++;
                if (appleCount == 1)
                {
                    appleSlice1.SetActive(true);
                }
                else if (appleCount == 2)
                {
                    appleSlice2.SetActive(true);
                }
                Destroy(other);
            }
        }
        else if (other.CompareTag("PearSlice"))
        {
            if (pearCount < 2)
            {
                pearCount++;
                if (pearCount == 1)
                {
                    pearSlice1.SetActive(true);
                }
                else if (pearCount == 2)
                {
                    pearSlice2.SetActive(true);
                }
                Destroy(other);
            }
        }
        else if (other.CompareTag("Banana"))
        {
            if (!bananaIn)
            {
                bananaIn = true;
                banana.SetActive(true);
                Destroy(other);
            }
        }
        else if (other.CompareTag("Whisk"))
        {
            if (isWhiskingMoment && !isWhiskIn)
            {
                    Animator animator = GetComponent<Animator>();

                whiskIn.SetActive(true);
                Destroy(other);
                isWhiskIn = true;
                if (animator != null)
                {
                            animator.SetTrigger("trgierazo");

                }

            }
            Debug.Log("Whisk detected!");
        }

        CheckIfBowlIsFull();
    }
    private void CheckIfBowlIsFull()
    {
        if (appleCount == 2 && pearCount == 2 && bananaIn)
        {
            isWhiskingMoment = true;
        }
    }
}
    