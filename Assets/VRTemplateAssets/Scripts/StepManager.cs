using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Unity.VRTemplate
{
    /// <summary>
    /// Controls the steps in the in coaching card.
    /// </summary>
    public class StepManager : MonoBehaviour
    {
        public GameObject[] steps;
        public AudioSource audioSource; // Add this line
        public AudioClip stepChangeClip; // Add this line

        public void Next(int step)
        {
            foreach (GameObject stepy in steps)
            {
                stepy.SetActive(false);
            }
            steps[step].SetActive(true);

            if (audioSource != null && stepChangeClip != null) // Add this block
            {
                audioSource.PlayOneShot(stepChangeClip);
            }
        }
    }
}