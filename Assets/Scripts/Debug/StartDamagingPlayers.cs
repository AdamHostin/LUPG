using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StartDamagingPlayers : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        App.playerManager.StartDealingDamage();
    }


}
