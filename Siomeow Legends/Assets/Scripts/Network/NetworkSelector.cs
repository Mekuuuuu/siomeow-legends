using System;
using UnityEngine;

public class NetworkSelector : MonoBehaviour
{
    public static NetworkSelector Instance { get; private set; }

    public bool isLAN { get; private set; }
    
    public void setLAN()
    {
        isLAN = true;
    }

    public void setMultiplayer()
    {
        isLAN = false;
    }
}
