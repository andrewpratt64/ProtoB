#define PROTOB_UNIT_TESTING

using ProtoB.Models;
using System.Runtime.InteropServices;

namespace ProtoB.Native
{
    public static class ProtoBCpp
    {

        [DllImport("ProtoBCpp.dll")]
        internal unsafe static extern void destroyUnmanagedEntityModel(EntityModel* ent);

        [DllImport("ProtoBCpp.dll")]
        internal unsafe static extern SubentityModel* newSubentityModel(SubentityModel* parent);


        #region UnitTesting
#if PROTOB_UNIT_TESTING
#pragma warning disable IDE1006 // Naming Styles
        [StructLayout(LayoutKind.Sequential)]

        public struct unittest_DynamicSpanCreationT
        {
            public DynamicSpan<ushort> madeViaDefault;
            public DynamicSpan<int> madeViaDirectInit;
            public DynamicSpan<char> strMadeViaDirectInit;
            public DynamicSpan<float> initThenRealloc;
            public DynamicSpan<sbyte> defaultThenAppend;
            public DynamicSpan<double> initThenAppend;
        }  
#pragma warning restore IDE1006 // Naming Styles


        [DllImport("ProtoBCpp.dll")]
        internal static extern int unittest_BasicTest(int num);

        [DllImport("ProtoBCpp.dll")]
        internal static extern unittest_DynamicSpanCreationT unittest_DynamicSpanCreation();

        [DllImport("ProtoBCpp.dll")]
        internal unsafe static extern EntityModel* unittest_MakeAnEntity();


        #region Wrapper for deleteDynamicSpan in C#
        [DllImport("ProtoBCpp.dll")]
        internal static extern void unittest_DeleteDynamicSpanInt8(ref DynamicSpan<sbyte> span);
        [DllImport("ProtoBCpp.dll")]
        internal static extern void unittest_DeleteDynamicSpanInt32(ref DynamicSpan<int> span);
        [DllImport("ProtoBCpp.dll")]
        internal static extern void unittest_DeleteNativeString(ref DynamicSpan<char> span);
        [DllImport("ProtoBCpp.dll")]
        internal static extern void unittest_DeleteDynamicSpanFloat(ref DynamicSpan<float> span);
        [DllImport("ProtoBCpp.dll")]
        internal static extern void unittest_DeleteDynamicSpanDouble(ref DynamicSpan<double> span);
        #endregion
#endif
        #endregion

    }
}
