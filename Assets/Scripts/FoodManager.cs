using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class FoodManager : MonoBehaviour
{
    public static UnityEvent<float> foodEaten = new UnityEvent<float>();
    PlayerHunger playerHunger;
    [SerializeField] List<FoodObject> foodObjects = new List<FoodObject>();
    [SerializeField] List<Transform> spawningAreas = new List<Transform>();
    [SerializeField] float foodNutritionalValue = 10f;

    [SerializeField] float spawnFoodTimer = 0f;
    [SerializeField] float spawnFoodIntervalSeconds = 60f;
    [SerializeField] int maxFoodItemsCount = 6;

    public GameObject foodObjectPrefab;

    void Awake()
    {
        playerHunger = FindFirstObjectByType<PlayerHunger>();
        foreach(Transform area in GameObject.FindGameObjectWithTag("FoodSpawningAreas").transform)
        {
            spawningAreas.Add(area.transform);
        }
    }
    void Start()
    {
        foodEaten.AddListener(IncreasePlayerHunger);
        SpawnFoodObject();
        SpawnFoodObject();
        SpawnFoodObject();
    }

    void Update()
    {
        DoRandomFoodSpawning();
        spawnFoodTimer += Time.deltaTime;
    }

    void DoRandomFoodSpawning()
    {
        if (foodObjects.Count < maxFoodItemsCount && spawnFoodTimer >= spawnFoodIntervalSeconds)
        {
            SpawnFoodObject();
            spawnFoodTimer = 0f;
        }
    }
    
    public void RemoveEatenFoodObject(FoodObject foodObject)
    {
        foodObjects.Remove(foodObject);
    }

    void SpawnFoodObject()
    {
        // select a random parent transform area
        Transform parentArea = spawningAreas[Random.Range(0, spawningAreas.Count)];

        // instantiate obj with random parent transform
        GameObject obj = Instantiate(foodObjectPrefab, parentArea);


        // move obj to random location within parent transform, randomize coords
        Collider parentAreaCollider = parentArea.GetComponent<Collider>();
        Vector3 newObjPosition = RandomVectorInRange(parentAreaCollider.bounds.min, parentAreaCollider.bounds.max);
        obj.transform.SetPositionAndRotation(newObjPosition, obj.transform.rotation);

        // instantiate foodObject script
        FoodObject foodObjectComp = obj.GetComponent<FoodObject>();
        foodObjectComp.ConstructFood(foodNutritionalValue, this);
        foodObjects.Add(foodObjectComp);

        
    }

    void IncreasePlayerHunger(float amount)
    {
        playerHunger.ChangeHunger(amount);
    }

    Vector3 RandomVectorInRange(Vector3 minVector, Vector3 maxVector)
    {
        float xCoord = Random.Range(minVector.x, maxVector.x);
        float yCoord = Random.Range(minVector.y, maxVector.y);
        float zCoord = Random.Range(minVector.z, maxVector.z);
        return new Vector3(xCoord, yCoord, zCoord);
    }
}
