using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VoiceChatManager : ManagerBaseClass<VoiceChatManager>
{
    public virtual void Initialize()
    {
    }

    public virtual void Tick(float deltaTime)
    { 
    }

    public abstract void StartVoiceRecording();
    public abstract void StopVoiceRecording();
}
