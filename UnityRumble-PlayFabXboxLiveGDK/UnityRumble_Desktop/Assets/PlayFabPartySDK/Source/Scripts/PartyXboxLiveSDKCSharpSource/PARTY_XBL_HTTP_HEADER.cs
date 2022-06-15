using System;
using PartyXBLCSharpSDK.Interop;
using PartyCSharpSDK;

namespace PartyXBLCSharpSDK
{
    public class PARTY_XBL_HTTP_HEADER
    {
        internal PARTY_XBL_HTTP_HEADER(Interop.PARTY_XBL_HTTP_HEADER interopStruct)
        {
            this.name = Converters.PtrToStringUTF8(interopStruct.name);
            this.value = Converters.PtrToStringUTF8(interopStruct.value);
        }

        public string name { get; }
        public string value { get; }
    }
}