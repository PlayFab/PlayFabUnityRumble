using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    //typedef enum PARTY_THREAD_ID
    //{
        // PARTY_THREAD_ID_AUDIO = 0,
        // PARTY_THREAD_ID_NETWORKING = 1,
    //} PARTY_THREAD_ID;
    public enum PARTY_THREAD_ID : UInt32
    {
        PARTY_THREAD_ID_AUDIO = 0,
        PARTY_THREAD_ID_NETWORKING = 1,
    }
}