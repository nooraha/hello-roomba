using UnityEngine;

public class PlayerHunger : MonoBehaviour
{
    [SerializeField] float maxHunger = 100f;
    public float currentHunger { get; private set; }
    [SerializeField] float hungerDecayPerSecond = 0.1f;

    public float maxHungerDecayFromJumping = 10f;

    HungerBar hungerBar;
    PlayerMovement playerMovement;

    void Awake()
    {
        hungerBar = FindFirstObjectByType<HungerBar>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();

    }

    void Start()
    {
        currentHunger = maxHunger;
        playerMovement.playerJumped.AddListener(OnPlayerJump);
    }

    void Update()
    {
        DoHungerDecay();
        hungerBar.UpdateHungerBar(currentHunger / maxHunger);
        
    }

    void DoHungerDecay()
    {
        ChangeHunger(-hungerDecayPerSecond * Time.deltaTime);
    }

    public void ChangeHunger(float amount)
    {
        currentHunger += amount;
    }
    
    void OnPlayerJump(float jumpStrength)
    {
        float hungerChangeAm = jumpStrength * maxHungerDecayFromJumping;
        ChangeHunger(-hungerChangeAm);
    }

}
