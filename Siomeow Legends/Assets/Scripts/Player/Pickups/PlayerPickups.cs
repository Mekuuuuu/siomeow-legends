using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickups : MonoBehaviour, Pickups
{
    public int Pickup { get => pickup; set => pickup = value; }
 
    public int pickup = 0;
}
 