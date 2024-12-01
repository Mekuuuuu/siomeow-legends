using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class Traps : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Reset(){
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collission){
        if(collission.tag == "Untagged"){
            Debug.Log($"{name} Trap Triggered");
        }
    }
}
