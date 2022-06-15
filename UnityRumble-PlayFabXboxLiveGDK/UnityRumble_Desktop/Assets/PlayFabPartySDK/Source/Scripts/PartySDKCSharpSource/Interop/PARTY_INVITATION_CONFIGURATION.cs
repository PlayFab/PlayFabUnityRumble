using System;
using System.Runtime.InteropServices;

namespace PartyCSharpSDK.Interop
{
    //typedef struct PARTY_INVITATION_CONFIGURATION
    //{
    //    _Maybenull_ PartyString identifier;
    //    PARTY_INVITATION_REVOCABILITY revocability;
    //    _Field_range_(0, PARTY_MAX_INVITATION_ENTITY_ID_COUNT) uint32_t entityIdCount;
    //    _Field_size_(entityIdCount) const PartyString* entityIds;
    //} PARTY_INVITATION_CONFIGURATION;
    [StructLayout(LayoutKind.Sequential)]
    internal struct PARTY_INVITATION_CONFIGURATION
    {
        internal readonly UTF8StringPtr identifier;
        internal readonly PARTY_INVITATION_REVOCABILITY revocability;
        internal readonly UInt32 entityIdCount;
        private readonly unsafe UTF8StringPtr* entityIds;

        internal T[] GetEntityIds<T>(Func<UTF8StringPtr,T> ctor) { unsafe { return Converters.PtrToClassArray<T, UTF8StringPtr>((IntPtr)this.entityIds, this.entityIdCount, ctor); } }

        internal PARTY_INVITATION_CONFIGURATION(PartyCSharpSDK.PARTY_INVITATION_CONFIGURATION publicObject, DisposableCollection disposableCollection)
        {
            this.identifier = new UTF8StringPtr(publicObject.Identifier, disposableCollection);
            this.revocability = publicObject.Revocability;
            unsafe
            {
                SizeT size;
                this.entityIds = (UTF8StringPtr*)Converters.ClassArrayToPtr<string, UTF8StringPtr>(
                    publicObject.EntityIds, 
                    (x, d) => new UTF8StringPtr(x, d),
                    disposableCollection,
                    out size);
                this.entityIdCount = size.ToUInt32();
            }
        }
    }
}