using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    //typedef enum PARTY_GENDER
    //{
        // PARTY_GENDER_NEUTRAL = 0,
        // PARTY_GENDER_FEMALE = 1,
        // PARTY_GENDER_MALE = 2,
    //} PARTY_GENDER;
    public enum PARTY_GENDER : UInt32
    {
        PARTY_GENDER_NEUTRAL = 0,
        PARTY_GENDER_FEMALE = 1,
        PARTY_GENDER_MALE = 2,
    }
}