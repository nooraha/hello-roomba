using UnityEngine;
using UnityEngine.Events;

public class FoodObject : MonoBehaviour
{
    UnityEvent<float> foodEaten;

    float nutritionalValue;

    public void ConstructFood(float nutritionalValue, UnityEvent<float> foodEaten)
    {
        this.nutritionalValue = nutritionalValue;
        this.foodEaten = foodEaten;
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            foodEaten.Invoke(nutritionalValue);
            Debug.Log("yum yum");
            Destroy(this.gameObject);
        }
    } 
}
