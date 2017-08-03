using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManagerTwo : NetworkManager {
    
    private int maxPlayers = 2;
    public int countPlayers = 0;

    public void makeServerGreatAgain()
    {
        StartHost();
    }
    
    public void makeClientGreatAgain()
    {
        StartClient();
    }

    private void Update()
    {
        if (countPlayers >= maxPlayers)
            Network.maxConnections = -1;
    }

}
