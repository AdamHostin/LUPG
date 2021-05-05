using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustumNetworkManager : NetworkManager
{
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log("new conn");
    }
}
