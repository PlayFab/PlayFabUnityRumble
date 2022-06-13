using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabSessionNetwork : SessionNetwork
{
    public override void SendMessageToAll(BaseNetStateObject netState, bool isIncludeSelf = true)
    {
        throw new System.NotImplementedException();
    }

    public override void SendMessageToMember(ulong memberUserId, BaseNetStateObject netState)
    {
        throw new System.NotImplementedException();
    }

    public override void StartNetworking(ulong userId, string networkID)
    {
        throw new System.NotImplementedException();
    }

    public override void StopNetworking()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
