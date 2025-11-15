using UnityEngine;

public class PlayerSoundEffects : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip eatingSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip meow;

    void Start()
    {
        FoodManager.foodEaten.AddListener(PlayEatingSound);
        RoombaController.attackedPlayer.AddListener(PlayDeathSound);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            PlayMeow();
        }
    }

    public void PlayEatingSound(float _)
    {
        audioSource.clip = eatingSound;
        audioSource.Play();
    }

    public void PlayDeathSound()
    {
        Debug.Log("Played death sound effect");
        audioSource.clip = deathSound;
        audioSource.Play();
    }

    public void PlayMeow()
    {
        audioSource.clip = meow;
        audioSource.Play();
    }
}
