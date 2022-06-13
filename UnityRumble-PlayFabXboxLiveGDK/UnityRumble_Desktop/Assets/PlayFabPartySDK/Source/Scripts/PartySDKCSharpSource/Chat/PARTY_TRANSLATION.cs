using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    public class PARTY_TRANSLATION
    {
        internal PARTY_TRANSLATION(Interop.PARTY_TRANSLATION interopStruct)
        {
            this.result = interopStruct.result;
            this.errorDetail = interopStruct.errorDetail;
            this.languageCode = Converters.PtrToStringUTF8(interopStruct.languageCode);
            this.options = interopStruct.options;
            this.translation = Converters.PtrToStringUTF8(interopStruct.translation);
        }

        public PARTY_STATE_CHANGE_RESULT result { get; }
        public UInt32 errorDetail { get; }
        public string languageCode { get; }
        public PARTY_TRANSLATION_RECEIVED_OPTIONS options { get; }
        public string translation { get; }
    }
}