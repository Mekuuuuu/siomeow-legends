using UnityEngine;
using Unity.Netcode;

public class LootCrate : NetworkBehaviour
{
    public GameObject healthPotionPrefab;
    public GameObject defensePotionPrefab;

    public void DropPotion()
    {
        
        Debug.Log($"Crate here");
        if (Random.Range(0f, 1f) <= 0.5f)
        {
            bool dropHealthPotion = Random.Range(0f, 1f) <= 0.69f;
            bool dropDefensePotion = Random.Range(0f, 1f) <= 0.42f;

            if (dropHealthPotion && !dropDefensePotion)
            {
                SpawnPotion(healthPotionPrefab);
            }
            else if (dropDefensePotion && !dropHealthPotion)
            {
                SpawnPotion(defensePotionPrefab);
            } 
            else
            {
                Debug.Log("No potions dropped.");
            }
        }
        else
        {
            Debug.Log("No potions dropped.");
        }
    }

    void SpawnPotion(GameObject potionPrefab)
    {
        GameObject potion = Instantiate(potionPrefab, transform.position, Quaternion.identity);
        var netObj = potion.GetComponent<NetworkObject>();
        if (netObj != null)
        {
            netObj.Spawn();
            Debug.Log($"Spawned potion: {potionPrefab.name}");
        }
        else
        {
            Debug.LogWarning("Potion prefab is missing NetworkObject!");
        }
    }
}
