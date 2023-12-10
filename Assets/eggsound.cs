using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eggsound : MonoBehaviour
{
    public AudioClip eggbreakClip;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void eggbraekSound()
    {
        if (eggbreakClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(eggbreakClip);
        }
    }
}
