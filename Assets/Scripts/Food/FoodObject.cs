using UnityEngine;
using UnityEngine.Events;

public class FoodObject : MonoBehaviour
{

    float nutritionalValue;
    FoodManager foodManager;

    public void ConstructFood(float nutritionalValue, FoodManager foodManager)
    {
        this.nutritionalValue = nutritionalValue;
        this.foodManager = foodManager;
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            FoodManager.foodEaten.Invoke(nutritionalValue);
            foodManager.RemoveEatenFoodObject(this);
            Debug.Log("yum yum");
            Destroy(this.gameObject);
        }
    } 
}
