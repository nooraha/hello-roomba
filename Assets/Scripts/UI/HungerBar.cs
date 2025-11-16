using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    Slider hungerBar;

    void Awake()
    {
        hungerBar = GetComponent<Slider>();
    }

    public void UpdateHungerBar(float amount)
    {
        hungerBar.value = amount;
    }
}
