using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VoiceChatState : BaseNetStateObject
{
    public int bufferLength;
    public byte[] buffer;

    public VoiceChatState() : this(null, 0) { }
    public VoiceChatState(byte[] voiceBuffer, int bufferLen) 
        : base(SessionMessageType.VoiceChatState, StateType.Unreliable)
    {
        bufferLength = bufferLen;
        buffer = voiceBuffer;
    }

    protected override void DeserializeFrom(BinaryReader reader)
    {
        bufferLength = reader.ReadInt32();
        buffer = reader.ReadBytes(bufferLength);
    }

    protected override void SerializeTo(BinaryWriter writer)
    {
        writer.Write(bufferLength);
        writer.Write(buffer, 0, bufferLength);
    }
}
