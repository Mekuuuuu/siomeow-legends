using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
  
    public int powers = 1;
 
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") {
            Pickups powerup = other.GetComponent<Pickups>();
 
            if(powerup != null){
                powerup.Pickup = powerup.Pickup + powers;
                print("Player inventory has " + powerup.Pickup + "pickup(s)");
                // Notify the spawner and deactivate this object
                RandomSpawner spawner = FindObjectOfType<RandomSpawner>();
                if (spawner != null)
                {
                    spawner.NotifyPickup(this.gameObject);
                }
                gameObject.SetActive(false);
            }
        }
    }
}
