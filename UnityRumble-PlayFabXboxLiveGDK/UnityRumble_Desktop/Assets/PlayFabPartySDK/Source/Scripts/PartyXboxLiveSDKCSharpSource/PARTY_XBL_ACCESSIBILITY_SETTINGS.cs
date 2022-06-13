using System;
using PartyCSharpSDK;

namespace PartyXBLCSharpSDK
{
    public class PARTY_XBL_ACCESSIBILITY_SETTINGS
    {
        internal PARTY_XBL_ACCESSIBILITY_SETTINGS(Interop.PARTY_XBL_ACCESSIBILITY_SETTINGS interopStruct)
        {
            this.SpeechToTextEnabled = interopStruct.speechToTextEnabled;
            this.TextToSpeechEnabled = interopStruct.textToSpeechEnabled;
            this.LanguageCode = interopStruct.GetLanguageCode();
            this.Gender = interopStruct.gender;
        }

        public Byte SpeechToTextEnabled { get; }
        public Byte TextToSpeechEnabled { get; }
        public string LanguageCode { get; }
        public PARTY_GENDER Gender { get; }
    }
}