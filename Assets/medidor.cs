using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class medidor : MonoBehaviour
{
    public Image fillBar;  // Reference to the UI Image component
    public ParticleSystem particleSystem;  // Reference to the particle system
    public bool started = false;
    public bool quemado = false;

    void Start()
    {
        // Start the coroutine to decrease the fill amount
    }
    public void StartDecreasingFillAmount()
    {
        particleSystem.Play();
        started = true;
        StartCoroutine(DecreaseFillAmount());
    }
    private IEnumerator DecreaseFillAmount()
    {
        while (started)
        {
            // Wait for 1 second
            yield return new WaitForSeconds(0.4f);

            // Decrease the fill amount by 0.01
            if (fillBar != null)
            {
                fillBar.fillAmount -= 0.01f;

                // Clamp the fill amount to ensure it stays between 0 and 1
                fillBar.fillAmount = Mathf.Clamp(fillBar.fillAmount, 0f, 1f);

                                fillBar.color = Color.Lerp(Color.red, Color.white, fillBar.fillAmount);
                UpdateParticleSystemColor(fillBar.fillAmount);
                if (fillBar.fillAmount == 0 && !quemado)
                {
                    quemado = true;
                }

            }
        }
    }
     private void UpdateParticleSystemColor(float fillAmount)
    {
        var main = particleSystem.main;
                var emission = particleSystem.emission;

        if (fillAmount > 0.5f)
        {
            main.startColor = Color.white;
                    main.startSize = 0.20f;
                                emission.rateOverTime = 0.75f;
            main.startSpeed = 0.7f;

        }
        else if (fillAmount > 0.25f)
        {
            float t = (fillAmount - 0.25f) / 0.25f;
            main.startColor = Color.Lerp(Color.gray, Color.white, t);
                    main.startSize = Mathf.Lerp(0.5f, 0.20f, t);
                                emission.rateOverTime = Mathf.Lerp(2.20f, 0.75f, t);
            main.startSpeed = Mathf.Lerp(1.25f, 0.7f, t);

        }
        else
        {
            float t = fillAmount / 0.25f;
            main.startColor = Color.Lerp(Color.black, Color.gray, t);
             main.startSize = Mathf.Lerp(0.85f, 0.5f, t);
                         emission.rateOverTime = Mathf.Lerp(5.30f, 2.20f, t);
            main.startSpeed = Mathf.Lerp(2.75f, 1.25f, t);
        }
    }
}