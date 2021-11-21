#pragma once
/// @file
/// @author Andrew Pratt
/// @brief Contains the dll interface

#define PROTOBCPP_UNIT_TESTING

#include <cstdint>

#include "DynamicSpan.h"
#include "EntityModel.h"
#include "SubentityModel.h"


extern "C"
{

	/// @brief Destroy an unmanaged EntityModel
	/// @param[in] ent EntityModel instance to destroy
	/// @seealso ProtoBCpp::destroyEntityModel
	__declspec(dllexport) void destroyUnmanagedEntityModel(ProtoBCpp::EntityModel* ent)
	{
		ProtoBCpp::destroyEntityModel(ent);
	}


	/// @brief Create a new SubentityModel
	/// @param[in] parent Subentity that the new subentity will be created as a child of
	/// @return Raw pointer to newly allocated subentity
	__declspec(dllexport) ProtoBCpp::SubentityModel* newSubentityModel(ProtoBCpp::SubentityModel* parent)
	{
		return ProtoBCpp::newSubentityModel(parent);
	}


#ifdef PROTOBCPP_UNIT_TESTING

	/// @brief Data to be returned from unittest_DynamicSpanCreation
	/// @detail Each member of this struct is arbitrary, and only used for unit testing
	struct unittest_DynamicSpanCreationT
	{
		/// @brief Object made via default initialization
		ProtoBCpp::DynamicSpan<uint16_t> madeViaDefault{};
		/// @brief Object made via direct initialization
		ProtoBCpp::DynamicSpan<int32_t> madeViaDirectInit;
		/// @brief Native string made via direct initialization
		ProtoBCpp::NativeString strMadeViaDirectInit;
		/// @brief Object that was made via direct initialization, then reallocated with
		///		a different size and capacity
		ProtoBCpp::DynamicSpan<float> initThenRealloc;
		/// @brief Object that was made via default initialization, then appended
		///		with other items
		ProtoBCpp::DynamicSpan<int8_t> defaultThenAppend;
		/// @brief Object that was made via direct initialization, then appended
		///		with other items
		ProtoBCpp::DynamicSpan<double> initThenAppend;
	};


	/// @brief Basic function to make sure the dll is working as expected
	/// @param[in] num Arbitrary number to be doubled
	/// @return The product of num and two
	__declspec(dllexport) int32_t unittest_BasicTest(const int32_t num)
	{
		return num * 2;
	}


	/// @brief Uses different methods to create DynamicSpan objects
	/// @return Array of DynamicSpan objects for unit testing
	__declspec(dllexport) unittest_DynamicSpanCreationT unittest_DynamicSpanCreation()
	{
		ProtoBCpp::DynamicSpan<float> initThenReallocVal{ 3, 3, new float[] {4.0f, 8.0f, 15.0f} };
		ProtoBCpp::resizeDynamicSpan(initThenReallocVal, 16);
#pragma warning(push)
#pragma warning(disable: 6386) // Buffer overrun
		initThenReallocVal.data[3] = 16.0f;
		initThenReallocVal.data[4] = 23.0f;
		initThenReallocVal.data[5] = 42.0f;
		initThenReallocVal.size = 6;
#pragma warning(pop) // Buffer overrun


		ProtoBCpp::DynamicSpan<int8_t> defaultThenAppend{};
		ProtoBCpp::appendToDynamicSpan(defaultThenAppend, static_cast<int8_t>(-121));
		ProtoBCpp::appendToDynamicSpan(defaultThenAppend, static_cast<int8_t>(17));
		ProtoBCpp::appendToDynamicSpan(defaultThenAppend, static_cast<int8_t>(55));


		ProtoBCpp::DynamicSpan<double> initThenAppend{ 1, 2, new double[2] {48.0} };
		ProtoBCpp::appendToDynamicSpan(initThenAppend, 4.8);
		ProtoBCpp::appendToDynamicSpan(initThenAppend, 0.48);
		assert(initThenAppend.size == 3);
		assert(initThenAppend.capacity == 3);


		return unittest_DynamicSpanCreationT{
			ProtoBCpp::DynamicSpan<uint16_t>{},
			ProtoBCpp::DynamicSpan<int32_t>{4, 4, new int[] {25, 50, 100, 200}},
			ProtoBCpp::NativeString{14, 14, new char16_t[] {u"Hello, world!"}},
			initThenReallocVal,
			defaultThenAppend,
			initThenAppend
		};
	}


	/// @brief Creates an example entity
	/// @return A new entity for testing
	__declspec(dllexport) ProtoBCpp::EntityModel* unittest_MakeAnEntity()
	{
		// Sorry for the insane indentation :)
		
		return new ProtoBCpp::EntityModel{
			// Entity name
			.name = u"ImAnExampleEntity",
			// Subtype
			.subtype = 2,

			// Root subentity
			.rootSubentity = new ProtoBCpp::SubentityModel{
				.editorName{u"JohnDoe01"},
				.trueName{u"johndoe"},
				// [modules:/zactor.class].pc_entitytype -> 00D6CD10F06FD39B -> 60461117653111707ULL
				.type{60461117653111707ULL},
				.entityid{144484609747285966ULL},
				.flags{ProtoBCpp::SUBENTITY_FLAGS_EDITOR_ONLY},

				// Children
				.children{
					.size{3},
					.capacity{16},
					.data{new ProtoBCpp::SubentityModel * [16] {

						// Child "FirstChild01"
						new ProtoBCpp::SubentityModel{
							.editorName{u"FirstChild01"},
							.trueName{u"FirstChild01"},
							// [modules:/zactor.class].pc_entitytype
							.type{60461117653111707ULL},
							.entityid{8344632611743593109ULL},
							.flags{0},
							.children{0, 0, nullptr}
						},
						// Child "OtherChild01"
						new ProtoBCpp::SubentityModel{
							.editorName{u"OtherChild01"},
							.trueName{u"OtherChild"},
							// [modules:/zentity.class].pc_entitytype
							.type{13992849215881565ULL},
							.entityid{11864178794592775619ULL},
							.flags{ProtoBCpp::SUBENTITY_FLAGS_EXPOSED},
							.children{0, 0, nullptr}
						},
						// Child "OtherChild02"
						new ProtoBCpp::SubentityModel{
							.editorName{u"OtherChild02"},
							.trueName{u"OtherChild"},
							// [assembly:/_pro/environment/geometry/props/food/fruits_a.wl2?/bananas_boxed_a_00.prim].pc_entitytype
							.type{24152303398992742ULL},
							.entityid{7833080631943352782ULL},
							.flags{ProtoBCpp::SUBENTITY_FLAGS_EDITOR_ONLY | ProtoBCpp::SUBENTITY_FLAGS_EXPOSED},

							// Children of "OtherChild02"
							.children{

								// "OtherChild02" only has one child, but has capacity for another
								.size{1},
								.capacity{2},
								.data{new ProtoBCpp::SubentityModel * [2] {
									
									// Child "SecondLvlChild01"
									new ProtoBCpp::SubentityModel{
										.editorName{u"SecondLvlChild01"},
										.trueName{u"BadIdeaForATrueName"},
										// [assembly:/_pro/design/logic.template?/showhint.entitytemplate].pc_entitytype
										.type{4224163975301228ULL},
										.entityid{14504092475257823047ULL},
										.flags{0},

										// "SecondLvlChild01" has no children, but has capacity for them
										.children{
											.size{0},
											.capacity{2},
											.data{new ProtoBCpp::SubentityModel*[2]}
										}
									}
								}}
							}
						},
					}}
				}
			}
		};
	}


#pragma region Wrapper_for_deleteDynamicSpan_in_CSharp
	/// @brief Allows ProtoBUnitTesting to call ProtoBCpp::deleteDynamicSpan(DynamicSpan<T>&)
	__declspec(dllexport) void unittest_DeleteDynamicSpanInt8(ProtoBCpp::DynamicSpan<int8_t>& span)
	{
		ProtoBCpp::deleteDynamicSpan(span);
	}
	/// @brief Allows ProtoBUnitTesting to call ProtoBCpp::deleteDynamicSpan(DynamicSpan<T>&)
	__declspec(dllexport) void unittest_DeleteDynamicSpanInt32(ProtoBCpp::DynamicSpan<int32_t>& span)
	{
		ProtoBCpp::deleteDynamicSpan(span);
	}
	/// @brief Allows ProtoBUnitTesting to call ProtoBCpp::deleteDynamicSpan(DynamicSpan<T>&)
	__declspec(dllexport) void unittest_DeleteDynamicSpanFloat(ProtoBCpp::DynamicSpan<float>& span)
	{
		ProtoBCpp::deleteDynamicSpan(span);
	}
	/// @brief Allows ProtoBUnitTesting to call ProtoBCpp::deleteDynamicSpan(DynamicSpan<T>&)
	__declspec(dllexport) void unittest_DeleteDynamicSpanDouble(ProtoBCpp::DynamicSpan<double>& span)
	{
		ProtoBCpp::deleteDynamicSpan(span);
	}
	/// @brief Allows ProtoBUnitTesting to call ProtoBCpp::deleteDynamicSpan(DynamicSpan<T>&)
	__declspec(dllexport) void unittest_DeleteNativeString(ProtoBCpp::NativeString& span)
	{
		ProtoBCpp::deleteDynamicSpan(span);
	}

#pragma endregion


#endif // PROTOB_UNIT_TESTING
};