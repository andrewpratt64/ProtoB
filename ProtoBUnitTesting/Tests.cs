using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoB.Models;
using ProtoB.Native;
using ProtoB.ViewModels;
using System;
using System.Runtime.InteropServices;

namespace ProtoBUnitTesting
{
    [TestClass]
    public class Tests
    {
        // Assert a given object is a blittable type
        internal static void AssertBlittable<T>(ref T obj)
        {
            try
            {
                GCHandle.Alloc(obj, GCHandleType.Pinned).Free();
            }
            catch (System.ArgumentException)
            {
                Assert.Fail("Type is NOT blittable, it was expected to be");
            }
        }


        // Assert that a pointer is valid and can be dereferenced
        internal unsafe static void AssertValidPtr<T>(T* ptr)
            where T: unmanaged
        {
			Assert.IsTrue(ptr != (T*)0);
			T dummy = *ptr;
        }


		// Assert that a double pointer is valid and can be dereferenced
		internal unsafe static void AssertValidDoublePtr<T>(T** ptr)
			where T : unmanaged
		{
			Assert.IsTrue(ptr != (T**)0);
			Assert.IsTrue(*ptr != (T*)0);
			T* dummy = *ptr;
			T doubleDummy = *dummy;
		}


		/*
		// Assert that a pointer is NOT valid and can NOT be dereferenced
		[System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
		[System.Security.SecurityCritical]
		[ExpectedException(typeof(System.NullReferenceException))]
		internal unsafe static void AssertInvalidPtr<T>(T* ptr)
            where T : unmanaged
        {
            Assert.ThrowsException<System.NullReferenceException>(delegate { T dummy = *ptr; });
        }
		*/

		/*
		// Assert that a double pointer is NOT valid and can NOT be dereferenced
		[System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
		[System.Security.SecurityCritical]
		internal unsafe static void AssertInvalidDoublePtr<T>(T** ptr)
		//internal unsafe static void AssertInvalidDoublePtr<T>(T* ptr)
			where T : unmanaged
		{
			try
			{
				T doubleDummy = **ptr;
				T* dummy = *ptr;
				T altDoubleDummy = *dummy;
				Assert.Fail("Double pointer was expected to be null, actual value: " + new IntPtr(ptr).ToString());
			}
			catch (System.NullReferenceException)
			{
				return;
			}
			catch (System.AccessViolationException)
			{ return; }
		}
		*/


		internal unsafe static void AssertNativeStringEquals(string expected, char* actual)
        {
            Assert.AreEqual(expected, new string(actual));

        }


        internal unsafe static void AssertNativeStringEquals(string expected, in DynamicSpan<char> actual)
        {
            Assert.AreEqual(expected.Length + 1, actual.size);
            AssertNativeStringEquals(expected, actual.data);
        }



		// Test that structs that need to be blittable are actually blittable
		[TestMethod]
		public void BlittableTypesTest()
		{
			unsafe
			{
				// Create an entity instance
				var ent = ProtoBCpp.unittest_MakeAnEntity();

				// Assert that types are blittable
				AssertBlittable(ref *ent);
				AssertBlittable(ref *ent->rootSubentity);
				AssertBlittable(ref *((SubentityModel**)ent->rootSubentity->children.data)[0]);

				// Destroy entity
				ProtoBCpp.destroyUnmanagedEntityModel(ent);
			}
		}


		// Basic function to make sure the dll is working as expected
		[TestMethod]
        public void BasicTest()
        {
            Assert.AreEqual(ProtoBCpp.unittest_BasicTest(9), 18);
        }


        // Test creating instances of DynamicSpan
        [TestMethod]
        public void DynamicSpanCreation()
        {
            // Create a collection of DynamicSpan instances to test
            var spans = ProtoBCpp.unittest_DynamicSpanCreation();


            //		==Make sure data was created correctly==
            unsafe
            {
                // Should have default values
                //AssertInvalidPtr(spans.madeViaDefault.data);
                Assert.AreEqual(0, spans.madeViaDefault.capacity);
                Assert.AreEqual(0, spans.madeViaDefault.size);

                // Should be an array of ints with the values: {25, 50, 100, 200}
                AssertValidPtr(spans.madeViaDirectInit.data);
                Assert.AreEqual(4, spans.madeViaDirectInit.capacity);
                Assert.AreEqual(4, spans.madeViaDirectInit.size);
                Assert.AreEqual(25, spans.madeViaDirectInit.data[0]);
                Assert.AreEqual(50, spans.madeViaDirectInit.data[1]);
                Assert.AreEqual(100, spans.madeViaDirectInit.data[2]);
                Assert.AreEqual(200, spans.madeViaDirectInit.data[3]);

                // Should be a utf-16 encoded string with the value "Hello, world!"
                AssertValidPtr(spans.strMadeViaDirectInit.data);
                Assert.AreEqual(14, spans.strMadeViaDirectInit.capacity);
                Assert.AreEqual(14, spans.strMadeViaDirectInit.size);
                AssertNativeStringEquals("Hello, world!", spans.strMadeViaDirectInit);

                // Should be an array of floats with the values: {4, 8, 15, 16, 23, 42}
                AssertValidPtr(spans.initThenRealloc.data);
                Assert.AreEqual(16, spans.initThenRealloc.capacity);
                Assert.AreEqual(6, spans.initThenRealloc.size);
                Assert.AreEqual(4.0f, spans.initThenRealloc.data[0]);
                Assert.AreEqual(8.0f, spans.initThenRealloc.data[1]);
                Assert.AreEqual(15.0f, spans.initThenRealloc.data[2]);
                Assert.AreEqual(16.0f, spans.initThenRealloc.data[3]);
                Assert.AreEqual(23.0f, spans.initThenRealloc.data[4]);
                Assert.AreEqual(42.0f, spans.initThenRealloc.data[5]);

				// Should be an array of int8s with the values: {-121, 17, 200}
				AssertValidPtr(spans.defaultThenAppend.data);
				Assert.AreEqual(3, spans.defaultThenAppend.capacity);
				Assert.AreEqual(3, spans.defaultThenAppend.size);
				Assert.AreEqual((sbyte)-121, spans.defaultThenAppend.data[0]);
				Assert.AreEqual((sbyte)17, spans.defaultThenAppend.data[1]);
				Assert.AreEqual((sbyte)55, spans.defaultThenAppend.data[2]);

				// Should be an array of doubles with the values: {48.0, 4.8, 0.48}
				AssertValidPtr(spans.initThenAppend.data);
				Assert.AreEqual(3, spans.initThenAppend.capacity);
				Assert.AreEqual(3, spans.initThenAppend.size);
				Assert.AreEqual(48.0, spans.initThenAppend.data[0]);
				Assert.AreEqual(4.8, spans.initThenAppend.data[1]);
				Assert.AreEqual(0.48, spans.initThenAppend.data[2]);


				// Cleanup memory
				ProtoBCpp.unittest_DeleteDynamicSpanInt32(ref spans.madeViaDirectInit);
                ProtoBCpp.unittest_DeleteNativeString(ref spans.strMadeViaDirectInit);
                ProtoBCpp.unittest_DeleteDynamicSpanFloat(ref spans.initThenRealloc);
                ProtoBCpp.unittest_DeleteDynamicSpanInt8(ref spans.defaultThenAppend);
                ProtoBCpp.unittest_DeleteDynamicSpanDouble(ref spans.initThenAppend);
            }
        }


        // Test creating a new instance of EntityModel
        [TestMethod]
        public void EntityModelTest()
        {
            unsafe
            {
                // Create an entity instance
                var ent = ProtoBCpp.unittest_MakeAnEntity();

                // Make sure entity state is what was expected
                AssertNativeStringEquals("ImAnExampleEntity", ent->name);
                Assert.AreEqual(2, ent->subtype);

                AssertValidPtr(ent->rootSubentity);
                AssertNativeStringEquals("JohnDoe01", ent->rootSubentity->editorName);

				//		>>> START OF TESTING CHILDREN <<<
				#region ChildrenTesting

				//		>>> Root <<<
				// Subentity exists?
				AssertValidPtr(ent->rootSubentity);

				// Editor name
				AssertNativeStringEquals(
					"JohnDoe01",
					ent->rootSubentity->editorName
				);

				// True name
				AssertNativeStringEquals(
					"johndoe",
					ent->rootSubentity->trueName
				);

				// Type
				Assert.AreEqual(
					60461117653111707U,
					ent->rootSubentity->type
				);
				//Assert.IsTrue("[modules:/zactor.class].pc_entitytype" == ent.rootSubentity->type);

				// Entity id
				Assert.AreEqual(
					144484609747285966U,
					ent->rootSubentity->entityid
				);

				// Flags
				Assert.IsTrue(
					ent->rootSubentity->flags.HasFlag(SubentityFlags.EDITOR_ONLY)
				);

				// Children capacity
				Assert.AreEqual(
					16,
					ent->rootSubentity->children.capacity
				);

				// Children size
				Assert.AreEqual(
					3,
					ent->rootSubentity->children.size
				);

				// Children exists?
				SubentityModel** RootChildren = (SubentityModel**)ent->rootSubentity->children.data;
				AssertValidDoublePtr(RootChildren);


				//		>>> Root->FirstChild01 <<<
				// Editor name
				AssertNativeStringEquals(
					"FirstChild01",
					RootChildren[0]->editorName
				);

				// True name
				AssertNativeStringEquals(
					"FirstChild01",
					RootChildren[0]->trueName
				);

				// Type
				Assert.AreEqual(
					60461117653111707U,
					RootChildren[0]->type
				);

				// Enity id
				Assert.AreEqual(
					8344632611743593109U,
					RootChildren[0]->entityid
				);

				// Flags
				Assert.AreEqual(
					(SubentityFlags)0,
					RootChildren[0]->flags
			);

				// Children capacity
				Assert.AreEqual(
					0,
					RootChildren[0]->children.capacity
				);

				// Children size
				Assert.AreEqual(
					0,
					RootChildren[0]->children.size
				);

				// Children exists?
				//AssertInvalidDoublePtr((SubentityModel**)RootChildren[0]->children.data);


				//		>>> Root->OtherChild01 <<<
				// Editor name
				AssertNativeStringEquals(
					"OtherChild01",
					RootChildren[1]->editorName
				);

				// True name
				AssertNativeStringEquals(
					"OtherChild",
					RootChildren[1]->trueName
				);

				// Type
				Assert.AreEqual(
					13992849215881565U,
					RootChildren[1]->type
				);

				// Enity id
				Assert.AreEqual(
					11864178794592775619U,
					RootChildren[1]->entityid
				);

				// Flags
				Assert.IsTrue(
					RootChildren[1]->flags.HasFlag(SubentityFlags.EXPOSED)
				);

				// Children capacity
				Assert.AreEqual(
					0,
					RootChildren[1]->children.capacity
				);

				// Children size
				Assert.AreEqual(
					0,
					RootChildren[1]->children.size
				);

				//AssertInvalidDoublePtr((SubentityModel**)RootChildren[1]->children.data);



				//		>>> Root->OtherChild02 <<<
				// Editor name
				AssertNativeStringEquals(
					"OtherChild02",
					RootChildren[2]->editorName
				);

				// True name
				AssertNativeStringEquals(
					"OtherChild",
					RootChildren[2]->trueName
				);

				// Type
				Assert.AreEqual(
					24152303398992742U,
					RootChildren[2]->type
				);

				// Entity id
				Assert.AreEqual(
					7833080631943352782U,
					RootChildren[2]->entityid
				);

				// Flags
				Assert.IsTrue(
					RootChildren[2]->flags.HasFlag(SubentityFlags.EDITOR_ONLY)
				);
				Assert.IsTrue(
					RootChildren[2]->flags.HasFlag(SubentityFlags.EXPOSED)
				);

				// Children capacity
				Assert.AreEqual(
					2,
					RootChildren[2]->children.capacity
				);

				// Children size
				Assert.AreEqual(
					1,
					RootChildren[2]->children.size
				);

				// Children exists?
				SubentityModel** OtherChild02Children = (SubentityModel**)RootChildren[2]->children.data;
				AssertValidDoublePtr(OtherChild02Children);



				//		>>> Root->OtherChild02->SecondLvlChild01 <<<
				// Editor name
				AssertNativeStringEquals(
					"SecondLvlChild01",
					OtherChild02Children[0]->editorName
				);

				// True name
				AssertNativeStringEquals(
					"BadIdeaForATrueName",
					OtherChild02Children[0]->trueName
				);

				// Type
				Assert.AreEqual(
					4224163975301228U,
					OtherChild02Children[0]->type
				);

				// Entity id
				Assert.AreEqual(
					14504092475257823047U,
					OtherChild02Children[0]->entityid
				);

				// Flags
				Assert.AreEqual(
					(SubentityFlags)0,
					OtherChild02Children[0]->flags
			);

				// Children capacity
				Assert.AreEqual(
					2,
					OtherChild02Children[0]->children.capacity
				);

				// Children size
				Assert.AreEqual(
					0,
                    OtherChild02Children[0]->children.size
				);

				// Children exists?
				//AssertInvalidDoublePtr((SubentityModel**)OtherChild02Children[0]->children.data);

				#endregion
				//		>>> END OF TESTING CHILDREN <<<


				// Test adding a new subentity to the hierarchy
				var newSubentity = ProtoBCpp.newSubentityModel(RootChildren[1]);
				AssertValidPtr(RootChildren[1]);
				Assert.AreEqual(1, RootChildren[1]->children.size);
				SubentityModel** OtherChild01Children = (SubentityModel**)RootChildren[1]->children.data;
				AssertValidDoublePtr(OtherChild01Children);
				ProtoB.Native.Util.AssignStringToCharArray(
					OtherChild01Children[0]->editorName,
					"ImNewOnTheHeap01"
				);
				AssertNativeStringEquals("ImNewOnTheHeap01", OtherChild01Children[0]->editorName);


				// Destroy entity
				ProtoBCpp.destroyUnmanagedEntityModel(ent);
            };
        }


        // Test creating a new instance of EntityViewModel
        [TestMethod]
        public void EntityViewModelTest()
        {
            EntityViewModel entvm;

            unsafe
            {
                // Create an entity instance
                var ent = ProtoBCpp.unittest_MakeAnEntity();

                // Use the entity model to create an entity viewmodel
                entvm = new EntityViewModel(ent);
            }

            // Make sure initial entity viewmodel state is what was expected
            Assert.AreEqual("ImAnExampleEntity", entvm.Name);
            Assert.AreEqual(2, entvm.Subtype);
            Assert.AreEqual("JohnDoe01", entvm.RootSubentity.EditorName);
            Assert.AreEqual("johndoe", entvm.RootSubentity.TrueName);
            // TODO: Assert.AreEqual(???, entvm.RootSubentity.Type);
            Assert.AreEqual(144484609747285966U, entvm.RootSubentity.EntityId);
            Assert.AreEqual(true, entvm.RootSubentity.EditorOnly);
            Assert.AreEqual(false, entvm.RootSubentity.Exposed);
			// Test some (but not all, just because it's probably not worth the time and likely
			//		wouldn't help anyways) of the properties of the root subentity's children
			Assert.IsNotNull(entvm.RootSubentity.Children);
			Assert.AreEqual(3, entvm.RootSubentity.Children.Count);
			Assert.AreEqual("FirstChild01", entvm.RootSubentity.Children[0].EditorName);
			Assert.AreEqual(11864178794592775619U, entvm.RootSubentity.Children[1].EntityId);
			Assert.IsNotNull(entvm.RootSubentity.Children[1].Children);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.AreEqual(0, entvm.RootSubentity.Children[1].Children.Count);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            Assert.IsNotNull(entvm.RootSubentity.Children[2].Children);
			Assert.AreEqual(true, entvm.RootSubentity.Children[2].EditorOnly);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            Assert.AreEqual(1, entvm.RootSubentity.Children[2].Children.Count);
            Assert.AreEqual("BadIdeaForATrueName", entvm.RootSubentity.Children[2].Children[0].TrueName);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

			// Change entity viewport state
			entvm.Name = "HelloNewName";
            entvm.Subtype = 9;
            entvm.RootSubentity.EditorName = "Rocco";
            entvm.RootSubentity.TrueName = "RealRocco";
            // TODO: entity type
            entvm.RootSubentity.EntityId = 12345678987654321U;
            entvm.RootSubentity.EditorOnly = false;
            entvm.RootSubentity.Exposed = true;
			// TODO: Mutate children of entvm.RootSubentity

            // Make sure entity viewmodel state was set correctly
            Assert.AreEqual("HelloNewName", entvm.Name);
            Assert.AreEqual(9, entvm.Subtype);
            Assert.AreEqual("Rocco", entvm.RootSubentity.EditorName);
            Assert.AreEqual("RealRocco", entvm.RootSubentity.TrueName);
            // TODO: Assert.AreEqual(???, entvm.RootSubentity.Type);
            Assert.AreEqual(12345678987654321U, entvm.RootSubentity.EntityId);
            Assert.AreEqual(false, entvm.RootSubentity.EditorOnly);
            Assert.AreEqual(true, entvm.RootSubentity.Exposed);

            // Shouldn't have to call ProtoBCpp.destroyUnmanagedEntityModel here,
            //  the entity and it's subentities should be destroyed when the
            //  entity viewmodel is disposed.
        }
    }
}
