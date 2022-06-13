﻿using System;

namespace PartyXBLCSharpSDK
{
    //typedef enum PARTY_XBL_STATE_CHANGE_RESULT
    //{
    //    PARTY_XBL_STATE_CHANGE_RESULT_SUCCEEDED,
    //    PARTY_XBL_STATE_CHANGE_RESULT_UNKNOWN_ERROR,
    //    PARTY_XBL_STATE_CHANGE_RESULT_CANCELED_BY_TITLE,
    //    PARTY_XBL_STATE_CHANGE_RESULT_USER_NOT_AUTHORIZED,
    //    PARTY_XBL_STATE_CHANGE_RESULT_LOGIN_TO_PLAYFAB_THROTTLED,
    //    PARTY_XBL_STATE_CHANGE_RESULT_PARTY_SERVICE_ERROR,
    //    PARTY_XBL_STATE_CHANGE_RESULT_XBOX_LIVE_SERVICE_TEMPORARILY_UNAVAILABLE,
    //    PARTY_XBL_STATE_CHANGE_RESULT_INTERNET_CONNECTIVITY_ERROR,
    //    PARTY_XBL_STATE_CHANGE_RESULT_PLAYFAB_RATE_LIMIT_EXCEEDED,
    //}
    //PARTY_XBL_STATE_CHANGE_RESULT;
    public enum PARTY_XBL_STATE_CHANGE_RESULT : UInt32
    {
        PARTY_XBL_STATE_CHANGE_RESULT_SUCCEEDED,
        PARTY_XBL_STATE_CHANGE_RESULT_UNKNOWN_ERROR,
        PARTY_XBL_STATE_CHANGE_RESULT_CANCELED_BY_TITLE,
        PARTY_XBL_STATE_CHANGE_RESULT_USER_NOT_AUTHORIZED,
        PARTY_XBL_STATE_CHANGE_RESULT_LOGIN_TO_PLAYFAB_THROTTLED,
        PARTY_XBL_STATE_CHANGE_RESULT_PARTY_SERVICE_ERROR,
        PARTY_XBL_STATE_CHANGE_RESULT_XBOX_LIVE_SERVICE_TEMPORARILY_UNAVAILABLE,
        PARTY_XBL_STATE_CHANGE_RESULT_INTERNET_CONNECTIVITY_ERROR,
        PARTY_XBL_STATE_CHANGE_RESULT_PLAYFAB_RATE_LIMIT_EXCEEDED,
    }
}