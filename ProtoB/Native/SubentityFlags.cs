using System;


namespace ProtoB.Native
{
    [Flags]
    public enum SubentityFlags : byte
    {
        EDITOR_ONLY = 0x1,
        EXPOSED = 0x2
    }
}