using UnityEngine;

public class bosssound : MonoBehaviour
{
    public AudioClip bosswalkClip;
    public AudioClip atkClip;
    public AudioClip bosslongClip;
    public AudioClip bossdeadClip;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void atkSound()
    {
        if (atkClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(atkClip);
        }
    }
    public void bosswalkSound()
    {
        if (bosswalkClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(bosswalkClip);
        }
    }
    public void bosslongSound()
    {
        if (bosslongClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(bosslongClip);
        }
    }
    public void bossdeadSound()
    {
        if (bossdeadClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(bossdeadClip);
        }
    }
}
