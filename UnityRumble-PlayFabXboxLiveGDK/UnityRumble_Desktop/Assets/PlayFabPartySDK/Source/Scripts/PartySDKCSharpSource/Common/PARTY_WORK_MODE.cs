using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    //typedef enum PARTY_WORK_MODE
    //{
        // PARTY_WORK_MODE_AUTOMATIC = 0,
        // PARTY_WORK_MODE_MANUAL = 1,
    //} PARTY_WORK_MODE;
    public enum PARTY_WORK_MODE : UInt32
    {
        PARTY_WORK_MODE_AUTOMATIC = 0,
        PARTY_WORK_MODE_MANUAL = 1,
    }
}