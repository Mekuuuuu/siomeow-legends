using UnityEngine;

public class Crate : MonoBehaviour
{
    public GameObject healthPotionPrefab;
    public GameObject defensePotionPrefab;

    void OnDestroy()
    {
        DropPotion();
    }

    void DropPotion()
    {
        if (Random.Range(0f, 1f) <= 0.5f)
        {
            bool dropHealthPotion = Random.Range(0f, 1f) <= 0.69f;
            bool dropDefensePotion = Random.Range(0f, 1f) <= 0.42f;

            if (dropHealthPotion && !dropDefensePotion)
            {
                Heal();
            }
            else if (dropDefensePotion && !dropHealthPotion)
            {
                Shield();
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

    void Heal()
    {
        Instantiate(healthPotionPrefab, transform.position, Quaternion.identity);
        Debug.Log("Health potion dropped.");
    }    
    
    void Shield()
    {
        Instantiate(defensePotionPrefab, transform.position, Quaternion.identity);
        Debug.Log("Defense potion dropped.");
    }
}
