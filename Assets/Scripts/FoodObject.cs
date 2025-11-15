using UnityEngine;
using UnityEngine.Events;

public class FoodObject : MonoBehaviour
{
    UnityEvent<float> foodEaten;

    float nutritionalValue;
    FoodManager foodManager;

    public void ConstructFood(float nutritionalValue, UnityEvent<float> foodEaten, FoodManager foodManager)
    {
        this.nutritionalValue = nutritionalValue;
        this.foodEaten = foodEaten;
        this.foodManager = foodManager;
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            foodEaten.Invoke(nutritionalValue);
            foodManager.RemoveEatenFoodObject(this);
            Debug.Log("yum yum");
            Destroy(this.gameObject);
        }
    } 
}
