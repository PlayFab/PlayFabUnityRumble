using System;
using System.Runtime.InteropServices;
using PartyCSharpSDK;

namespace PartyXBLCSharpSDK.Interop
{
    //typedef struct PARTY_XBL_ACCESSIBILITY_SETTINGS
    //{
    //    PartyBool speechToTextEnabled;
    //    PartyBool textToSpeechEnabled;
    //    char languageCode[PARTY_MAX_LANGUAGE_CODE_STRING_LENGTH + 1];
    //    PARTY_GENDER gender;
    //} PARTY_XBL_ACCESSIBILITY_SETTINGS;
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_XBL_ACCESSIBILITY_SETTINGS
    {
        internal readonly Byte speechToTextEnabled;
        internal readonly Byte textToSpeechEnabled;
        private unsafe fixed Byte languageCode[85]; // char languageCode[85]
        internal readonly PARTY_GENDER gender;

        internal string GetLanguageCode() { unsafe { fixed (Byte* ptr = this.languageCode) { return Converters.BytePointerToString(ptr, 85); } } }

        internal PARTY_XBL_ACCESSIBILITY_SETTINGS(PartyXBLCSharpSDK.PARTY_XBL_ACCESSIBILITY_SETTINGS publicObject)
        {
            this.speechToTextEnabled = publicObject.SpeechToTextEnabled;
            this.textToSpeechEnabled = publicObject.TextToSpeechEnabled;
            unsafe
            {
                fixed (Byte* ptr = this.languageCode)
                {
                    Converters.StringToNullTerminatedUTF8FixedPointer(publicObject.LanguageCode, ptr, 85);
                }
            }
            this.gender = publicObject.Gender;
        }
    }
}