using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkState : NetworkBehaviour {

    public static NetworkState Singleton;
    private void Awake()
    {
        Singleton = this;
    }

    [Server]
    private void Update()
    {
        if (!isServer)
            return;
    }

    [ClientRpc]
    public void RpcStartAnimation(string animationName, GameObject objectToAnimate)
    {
        objectToAnimate.GetComponent<Animator>().SetBool("Run", true);
    }
}
