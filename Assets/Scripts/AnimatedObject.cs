using UnityEngine;

public class AnimatedObject : MonoBehaviour
{
    [SerializeField] Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayInteractionAnimation()
    {
        animator.SetTrigger("Interaction");
    }
}
