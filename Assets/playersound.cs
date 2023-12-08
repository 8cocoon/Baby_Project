using UnityEngine;

public class playersound : MonoBehaviour
{
    public AudioClip attackClip;
    public AudioClip dashClip;
    public AudioClip longClip;
    public AudioClip deadClip;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAttackSound()
    {
        if (attackClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackClip);
        }
    }
    public void dashSound()
    {
        if (dashClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(dashClip);
        }
    }
    public void longSound()
    {
        if (longClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(longClip);
        }
    }
    public void deadSound()
    {
        if (deadClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(deadClip);
        }
    }
}
