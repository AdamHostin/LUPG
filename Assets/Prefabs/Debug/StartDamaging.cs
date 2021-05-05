using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDamaging : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        App.playerManager.StartDealingDamage();
    }

    
}
