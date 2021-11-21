#include <string>
#include <string_view>
#include <cstdint>

#include "CppUnitTest.h"
#include "interface.hpp"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using namespace std::string_view_literals;


namespace ProtoBCppUnitTesting
{
	TEST_CLASS(ProtoBCppUnitTesting)
	{
	private:
		/// @brief Assert that a native string holds a specific value
		/// @param[in] expected Value string is expected to be
		/// @param[in] actual Native string to test against expected
		static void AssertNativeStringEquals(
			const std::u16string_view expected,
			const ProtoBCpp::NativeString& actual
		)
		{
			Assert::AreEqual(expected.size() + 1, static_cast<size_t>(actual.size));
			Assert::AreEqual(0, expected.compare(actual.data));
		}


	public:

		/// @brief Basic function to make sure the dll is working as expected
		TEST_METHOD(BasicTest)
		{
			Assert::AreEqual(unittest_BasicTest(12), 24);
		}


		/// @brief Test creating instances of DynamicSpan
		TEST_METHOD(DynamicSpanCreation)
		{
			// Create a collection of DynamicSpan instances to test
			auto spans{ unittest_DynamicSpanCreation() };


			//		==Make sure data was created correctly==
			
			// Should have default values
			Assert::IsNull(spans.madeViaDefault.data);
			Assert::AreEqual(0, spans.madeViaDefault.size);

			// Should be an array of int32s with the values: {25, 50, 100, 200}
			Assert::IsNotNull(spans.madeViaDirectInit.data);
			Assert::AreEqual(4, spans.madeViaDirectInit.capacity);
			Assert::AreEqual(4, spans.madeViaDirectInit.size);
			Assert::AreEqual(25, spans.madeViaDirectInit.data[0]);
			Assert::AreEqual(50, spans.madeViaDirectInit.data[1]);
			Assert::AreEqual(100, spans.madeViaDirectInit.data[2]);
			Assert::AreEqual(200, spans.madeViaDirectInit.data[3]);

			// Should be a utf-16 encoded string with the value "Hello, world!"
			Assert::IsNotNull(spans.strMadeViaDirectInit.data);
			Assert::AreEqual(14, spans.strMadeViaDirectInit.capacity);
			Assert::AreEqual(14, spans.strMadeViaDirectInit.size);
			AssertNativeStringEquals(u"Hello, world!", spans.strMadeViaDirectInit);
			
			// Should be an array of floats with the values: {4, 8, 15, 16, 23, 42}
			Assert::IsNotNull(spans.initThenRealloc.data);
			Assert::AreEqual(16, spans.initThenRealloc.capacity);
			Assert::AreEqual(6, spans.initThenRealloc.size);
			Assert::AreEqual(4.0f, spans.initThenRealloc.data[0]);
			Assert::AreEqual(8.0f, spans.initThenRealloc.data[1]);
			Assert::AreEqual(15.0f, spans.initThenRealloc.data[2]);
			Assert::AreEqual(16.0f, spans.initThenRealloc.data[3]);
			Assert::AreEqual(23.0f, spans.initThenRealloc.data[4]);
			Assert::AreEqual(42.0f, spans.initThenRealloc.data[5]);

			// Should be an array of int8s with the values: {-121, 17, 200}
			Assert::IsNotNull(spans.defaultThenAppend.data);
			Assert::AreEqual(3, spans.defaultThenAppend.capacity);
			Assert::AreEqual(3, spans.defaultThenAppend.size);
			Assert::AreEqual(static_cast<int8_t>(-121), spans.defaultThenAppend.data[0]);
			Assert::AreEqual(static_cast<int8_t>(17), spans.defaultThenAppend.data[1]);
			Assert::AreEqual(static_cast<int8_t>(55), spans.defaultThenAppend.data[2]);

			// Should be an array of doubles with the values: {48.0, 4.8, 0.48}
			Assert::IsNotNull(spans.initThenAppend.data);
			Assert::AreEqual(3, spans.initThenAppend.capacity);
			Assert::AreEqual(3, spans.initThenAppend.size);
			Assert::AreEqual(48.0, spans.initThenAppend.data[0]);
			Assert::AreEqual(4.8, spans.initThenAppend.data[1]);
			Assert::AreEqual(0.48, spans.initThenAppend.data[2]);

			// Cleanup memory
			ProtoBCpp::deleteDynamicSpan(spans.madeViaDirectInit);
			ProtoBCpp::deleteDynamicSpan(spans.strMadeViaDirectInit);
			ProtoBCpp::deleteDynamicSpan(spans.initThenRealloc);
			ProtoBCpp::deleteDynamicSpan(spans.defaultThenAppend);
			ProtoBCpp::deleteDynamicSpan(spans.initThenAppend);
		}


		/// @brief Test creating a new instance of EntityModel
		TEST_METHOD(EntityModelTest)
		{
			// Create an entity instance
			auto* ent{ unittest_MakeAnEntity() };

			// Make sure entity state is what was expected
			Assert::IsTrue(u"ImAnExampleEntity"sv == ent->name);
			Assert::AreEqual(ent->subtype, 2);
			

#pragma region ChildrenTesting
			//		>>> START OF TESTING CHILDREN <<<


			//		>>> Root <<<
			// Subentity exists?
			Assert::IsNotNull(ent->rootSubentity);
			
			// Editor name
			Assert::IsTrue(u"JohnDoe01"sv == ent->rootSubentity->editorName);

			// True name
			Assert::IsTrue(u"johndoe"sv == ent->rootSubentity->trueName);

			// Type
			Assert::AreEqual(
				60461117653111707ULL,
				ent->rootSubentity->type
			);
			//Assert::IsTrue(u"[modules:/zactor.class].pc_entitytype"sv == ent.rootSubentity->type);
			
			// Entity id
			Assert::AreEqual(
				144484609747285966ULL,
				ent->rootSubentity->entityid
			);

			// Flags
			Assert::AreEqual(
				ProtoBCpp::SUBENTITY_FLAGS_EDITOR_ONLY,
				ent->rootSubentity->flags
			);

			// Children capacity
			Assert::AreEqual(
				16,
				ent->rootSubentity->children.capacity
			);

			// Children size
			Assert::AreEqual(
				3,
				ent->rootSubentity->children.size
			);

			// Children exists?
			Assert::IsNotNull(ent->rootSubentity->children.data);


			//		>>> Root->FirstChild01 <<<
			// Editor name
			Assert::IsTrue(u"FirstChild01"sv == ent->rootSubentity->children.data[0]->editorName);
			
			// True name
			Assert::IsTrue(u"FirstChild01"sv == ent->rootSubentity->children.data[0]->trueName);

			// Type
			Assert::AreEqual(
				60461117653111707ULL,
				ent->rootSubentity->children.data[0]->type
			);

			// Enity id
			Assert::AreEqual(
				8344632611743593109ULL,
				ent->rootSubentity->children.data[0]->entityid
			);

			// Flags
			Assert::AreEqual(
				uint8_t{ 0 },
				ent->rootSubentity->children.data[0]->flags
			);

			// Children capacity
			Assert::AreEqual(
				0,
				ent->rootSubentity->children.data[0]->children.capacity
			);

			// Children size
			Assert::AreEqual(
				0,
				ent->rootSubentity->children.data[0]->children.size
			);

			// Children exists?
			Assert::IsNull(ent->rootSubentity->children.data[0]->children.data);


			//		>>> Root->OtherChild01 <<<
			// Editor name
			Assert::IsTrue(u"OtherChild01"sv == ent->rootSubentity->children.data[1]->editorName);

			// True name
			Assert::IsTrue(u"OtherChild"sv == ent->rootSubentity->children.data[1]->trueName);

			// Type
			Assert::AreEqual(
				13992849215881565ULL,
				ent->rootSubentity->children.data[1]->type
			);

			// Enity id
			Assert::AreEqual(
				11864178794592775619ULL,
				ent->rootSubentity->children.data[1]->entityid
			);

			// Flags
			Assert::AreEqual(
				ProtoBCpp::SUBENTITY_FLAGS_EXPOSED,
				ent->rootSubentity->children.data[1]->flags
			);

			// Children capacity
			Assert::AreEqual(
				0,
				ent->rootSubentity->children.data[1]->children.capacity
			);

			// Children size
			Assert::AreEqual(
				0,
				ent->rootSubentity->children.data[1]->children.size
			);

			Assert::IsNull(ent->rootSubentity->children.data[1]->children.data);



			//		>>> Root->OtherChild02 <<<
			// Editor name
			Assert::IsTrue(u"OtherChild02"sv == ent->rootSubentity->children.data[2]->editorName);

			// True name
			Assert::IsTrue(u"OtherChild"sv == ent->rootSubentity->children.data[2]->trueName);

			// Type
			Assert::AreEqual(
				24152303398992742ULL,
				ent->rootSubentity->children.data[2]->type
			);

			// Entity id
			Assert::AreEqual(
				7833080631943352782ULL,
				ent->rootSubentity->children.data[2]->entityid
			);

			// Flags
			Assert::AreEqual(
				uint8_t{ ProtoBCpp::SUBENTITY_FLAGS_EDITOR_ONLY | ProtoBCpp::SUBENTITY_FLAGS_EXPOSED },
				ent->rootSubentity->children.data[2]->flags
			);

			// Children capacity
			Assert::AreEqual(
				2,
				ent->rootSubentity->children.data[2]->children.capacity
			);

			// Children size
			Assert::AreEqual(
				1,
				ent->rootSubentity->children.data[2]->children.size
			);

			// Children exists?
			Assert::IsNotNull(ent->rootSubentity->children.data[2]->children.data);



			//		>>> Root->OtherChild02->SecondLvlChild01 <<<
			// Editor name
			Assert::IsTrue(u"SecondLvlChild01"sv == ent->rootSubentity->children.data[2]->children.data[0]->editorName);

			// True name
			Assert::IsTrue(u"BadIdeaForATrueName"sv == ent->rootSubentity->children.data[2]->children.data[0]->trueName);

			// Type
			Assert::AreEqual(
				4224163975301228ULL,
				ent->rootSubentity->children.data[2]->children.data[0]->type
			);

			// Entity id
			Assert::AreEqual(
				14504092475257823047ULL,
				ent->rootSubentity->children.data[2]->children.data[0]->entityid
			);

			// Flags
			Assert::AreEqual(
				uint8_t{ 0 },
				ent->rootSubentity->children.data[2]->children.data[0]->flags
			);

			// Children capacity
			Assert::AreEqual(
				2,
				ent->rootSubentity->children.data[2]->children.data[0]->children.capacity
			);

			// Children size
			Assert::AreEqual(
				0,
				ent->rootSubentity->children.data[2]->children.data[0]->children.size
			);

			// Children exists?
			Assert::IsNotNull(ent->rootSubentity->children.data[2]->children.data[0]->children.data);


			//		>>> END OF TESTING CHILDREN <<<
#pragma endregion


			// Test adding a new subentity to the hierarchy
			auto* newSubentity{ ProtoBCpp::newSubentityModel(ent->rootSubentity->children.data[1]) };
			Assert::IsNotNull(ent->rootSubentity->children.data[1]->children.data[0]);
			Assert::AreEqual(1, ent->rootSubentity->children.data[1]->children.size);
			constexpr char16_t newSubentityNewEditorName[]{ u"ImNewOnTheHeap01" };
			memcpy(
				ent->rootSubentity->children.data[1]->children.data[0]->editorName,
				newSubentityNewEditorName,
				sizeof(newSubentityNewEditorName)
			);
			Assert::IsTrue(u"ImNewOnTheHeap01"sv == ent->rootSubentity->children.data[1]->children.data[0]->editorName);


			// Destroy entity
			destroyUnmanagedEntityModel(ent);
		}
	};
}
