using System;
using PartyCSharpSDK.Interop;

namespace PartyCSharpSDK
{
    public class PARTY_INVITATION_CONFIGURATION
    {
        internal PARTY_INVITATION_CONFIGURATION(Interop.PARTY_INVITATION_CONFIGURATION interopStruct)
        {
            this.Identifier = interopStruct.identifier.GetString();
            this.Revocability = interopStruct.revocability;
            this.EntityIds = interopStruct.GetEntityIds(x => x.GetString());
        }

        public PARTY_INVITATION_CONFIGURATION()
        {
        }

        public string Identifier { get; set; }
        public PARTY_INVITATION_REVOCABILITY Revocability { get; set; }
        public string[] EntityIds { get; set; }
    }
}