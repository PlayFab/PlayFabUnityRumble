namespace PlayFab.Multiplayer.Interop
{
    public unsafe partial struct PFEntityKey
    {
        [NativeTypeName("const char *")]
        public sbyte* id;

        [NativeTypeName("const char *")]
        public sbyte* type;
    }
}
