using UnityEngine;

public class playersound : MonoBehaviour
{
    public AudioClip attackClip;
    public AudioClip dashClip;
    public AudioClip longClip;
    public AudioClip parryingClip;
    public AudioClip executeClip;
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
    public void parryingSound()
    {
        if (parryingClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(parryingClip);
        }
    }
    public void executeSound()
    {
        if (executeClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(executeClip);
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
