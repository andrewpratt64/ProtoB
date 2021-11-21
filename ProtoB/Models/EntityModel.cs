using System.Runtime.InteropServices;


namespace ProtoB.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct EntityModel
    {
        public unsafe fixed char name[Native.Util.ENTITY_MODEL_NAME_MAXLEN];
        public int subtype;
        public unsafe SubentityModel* rootSubentity;
    }
}
