
using System.Runtime.InteropServices;

namespace ProtoB.Native
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DynamicSpan<T>
        where T: unmanaged
    {
        public int size;
        public int capacity;
        public unsafe T* data;
    }
}
