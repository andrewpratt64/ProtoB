using ProtoB.Native;
using System.Runtime.InteropServices;


namespace ProtoB.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SubentityModel
    {
        public unsafe fixed char editorName[Native.Util.SUBENTITY_MODEL_EDITOR_NAME_MAXLEN];
        public unsafe fixed char trueName[Native.Util.SUBENTITY_MODEL_TRUE_NAME_MAXLEN];
        public ulong type;
        public ulong entityid;
        public SubentityFlags flags;
        // TODO: This assumes pointers have a size of 8 bytes, will that always be the case?
        public unsafe DynamicSpan<long> children;

        // TODO: properties
        // TODO: subsets
        // TODO: exposedInterfaces

    }
}