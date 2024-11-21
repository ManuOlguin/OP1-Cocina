using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VRTemplate;
public class bowlcito : MonoBehaviour
{
    public GameObject[] PapasHervidas;  // Array of carrot slices
    public GameObject spoon;  // Reference to the spoon GameObject
    public GameObject stepManager;
    public GameObject pan;
    public int activePapasHervidas = 0;
    public bool isFilledWithPapas = false;
    public GameObject[] PapasPisadas;
    public GameObject finalGameObject; // Final game object to show
    public int collisionThreshold = 10; // Set amount to meet the counter
    private int collisionCounter = 0;
    public ParticleSystem particleSystem;  // Reference to the particle system
    public int harinitaCounter = 0;
    private bool isCollidingWithPan = false;  // Flag to check if spoon is in contact with the pan
    public bool isHarinado = false;
    private Vector3 velocity;
    private Vector3 lastPosition;
    public GameObject final;
    private int masaStep = 0;
    public GameObject[] harinitas;
    public GameObject[] masitas;
        public GameObject[] masas;

    public bool masaLista = false;
    public float movementThreshold = 1f;  // Minimum movement to consider as active
    private int cony = 0;

    private bool isPisada = false;
    void Start()
    {
        foreach (GameObject slice in PapasHervidas)
        {
            slice.SetActive(false);
        }
        foreach (GameObject slice in PapasPisadas)
        {
            slice.SetActive(false);
        }
        foreach (GameObject harinita in harinitas)
        {
            harinita.SetActive(false);
        }
        foreach (GameObject masa in masitas)
        {
            masa.SetActive(false);
        }
        foreach (GameObject masa in masas)
        {
            masa.SetActive(false);
        }
        finalGameObject.SetActive(false);
        final.SetActive(false);
    }

    void Update()
    {
        // Only track spoon velocity if it is colliding with the pan
        if (isCollidingWithPan && spoon != null && isHarinado)
        {
            Debug.Log("Spoon is in contact with the pan");

            // Calculate velocity manually
            Vector3 currentPosition = spoon.transform.position;
            velocity = (currentPosition - lastPosition) / Time.deltaTime;
            lastPosition = currentPosition;


            // Check if the velocity magnitude exceeds the threshold
            if (velocity.magnitude > movementThreshold)
            {
                
                cony++;
                            // Rotate finalGameObject around the Y-axis
            finalGameObject.transform.Rotate(0, 2, 0);

            // Rotate harinitas[4] around the Y-axis
            harinitas[4].transform.Rotate(0, -2, 0);
                

            }
            if(cony > 200 && !masaLista) 
            {
                stepManager.GetComponent<StepManager>().Next(8);
                pan.GetComponent<Pan>().estaListoParaSalsa = true;
                masaLista = true;
                final.SetActive(true);
                finalGameObject.SetActive(false);
                
                harinitas[4].SetActive(false);
                masitas[masaStep].SetActive(true);

            }
        }
    }

    public void grabMasa()
    {
        Debug.Log("Grabbing masa");
        final.SetActive(false);
        if (masaLista)
        {
            foreach (GameObject masa in masas)
            {
                masa.SetActive(false);
            }
            if(masas.Length > masaStep)
            {
                masas[masaStep].SetActive(true);
                masitas[masaStep+1].SetActive(true);
                masaStep++;
            }



        }
    }

    void OnCollisionEnter(Collision collision)
    {

        Debug.Log("Bowl Collision detected with " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Spoon"))
        {
            Debug.Log("Collision detected" + collision.gameObject.name);

            isCollidingWithPan = true;
        }
        if (collision.gameObject.CompareTag("Harinita") && isPisada)
        {
            harinitaCounter++;
            if (harinitaCounter > 300)
            {
                return;
            }
            else
            {
                ActivateHarinita(harinitaCounter);
            }
        }

        if (collision.gameObject.CompareTag("PapaHervida") && !isFilledWithPapas)
        {
            Destroy(collision.gameObject);
            ActivatePapaHervida();
            Debug.Log("PapaHervida activatedasdas" + activePapasHervidas + " " + PapasHervidas.Length);
            if (activePapasHervidas >= PapasHervidas.Length)
            {
                Debug.Log("Mondongo");
                isFilledWithPapas = true;
                stepManager.GetComponent<StepManager>().Next(6);
            }
        }

        if (collision.gameObject.CompareTag("PisaPapa") && isFilledWithPapas)
        {
            collisionCounter++;

            if (collisionCounter > collisionThreshold)
            {
                return;
            }
            Debug.Log("Papas pisadas");
            if (collisionCounter == 1)
            {
                foreach (GameObject slice in PapasHervidas)
                {
                    slice.SetActive(false);
                }
            }


            if (collisionCounter == collisionThreshold)
            {
                foreach (GameObject slice in PapasPisadas)
                {
                    slice.SetActive(false);
                }
                finalGameObject.SetActive(true);
                particleSystem.Play();
                stepManager.GetComponent<StepManager>().Next(7);
                isPisada = true;

            }
            else
            {
                int index = collisionCounter % PapasPisadas.Length;

                for (int i = 0; i < PapasPisadas.Length; i++)
                {
                    if (i != index)
                    {
                        PapasPisadas[i].SetActive(false);
                    }
                }
                if (PapasPisadas.Length > 0)
                {
                    GameObject papasPisada = PapasPisadas[index];
                    papasPisada.SetActive(true);
                    particleSystem.Play();
                    papasPisada.transform.rotation = Quaternion.Euler(
                        papasPisada.transform.rotation.eulerAngles.x,
                        Random.Range(0f, 360f),
                        papasPisada.transform.rotation.eulerAngles.z
                    );
                }
            }
        }


    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spoon"))
        {
            Debug.Log("Collision ended" + collision.gameObject.name);

            isCollidingWithPan = false;
        }
    }

    void ActivateHarinita(int count)
    {

        if (count > 100)
        {
            isHarinado = true;
            return;
        }

        // Activate the appropriate Harinita based on the count
        if (count >= 0)
        {
            float scaleFactor = Mathf.Lerp(1.0f, 10.0f, count / 100.0f); // Adjust the scale range as needed
            harinitas[4].transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            harinitas[4].SetActive(true);
        }
    }

    public void ActivatePapaHervida()
    {
        Debug.Log("PapaHervida");
        if (activePapasHervidas < PapasHervidas.Length)
        {
            Debug.Log("PapaHervida activated" + activePapasHervidas + " " + PapasHervidas.Length);
            PapasHervidas[activePapasHervidas].SetActive(true);
            activePapasHervidas++;
        }
    }
}
