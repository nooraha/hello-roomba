using UnityEngine;

public class AnimatedObject : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] bool playSound;
    [SerializeField] AudioClip audioClip;
    AudioSource audioSource;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if(playSound)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayInteractionAnimation()
    {
        animator.SetTrigger("Interaction");
        if(playSound)
        {
            PlaySound();
        }
        
    }

    void PlaySound()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
