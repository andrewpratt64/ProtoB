#pragma once
/// @file
/// @author Andrew Pratt

#include <cstdint>
#include <assert.h>

#include "DynamicSpan.h"


namespace ProtoBCpp
{
	constexpr size_t SUBENTITY_MODEL_EDITOR_NAME_MAXLEN{ 48ULL };
	constexpr size_t SUBENTITY_MODEL_TRUE_NAME_MAXLEN{ 48ULL };

	/// @brief Subentity flag; Subentity is editor-only when true
	constexpr uint8_t SUBENTITY_FLAGS_EDITOR_ONLY{ 0x1U };
	/// @brief Subentity flag; Subentity is exposed when true
	// TODO: Exposed to what?
	constexpr uint8_t SUBENTITY_FLAGS_EXPOSED{ 0x2U };


	/// @brief An instance of an entity that is a child of another entity
	/// @detail Model for MVVM, used in interop
	struct SubentityModel
	{
		/// @brief Name of the subentity used in the editor
		/// @detail Name should be unique; no two subentities
		///		in the same entity should have the same editor name
		char16_t editorName[SUBENTITY_MODEL_EDITOR_NAME_MAXLEN]{};

		/// @brief Name of the subentity
		/// @detail TODO: Optional, may be nullptr in which case editorName is
		///		used instead. Use this for when you need to have several
		///		subentities with the same name
		char16_t trueName[SUBENTITY_MODEL_TRUE_NAME_MAXLEN]{};

		/// @brief Type of the subentity
		uint64_t type{ 0 };

		/// @brief Unique identifier for this subentity
		uint64_t entityid{ 0 };

		/// @brief Boolean flags for this subentity
		/// @detail Uses the SUBENTITY_FLAGS_* bitmasks
		uint8_t flags{ 0 };

		/// @brief Collection of pointers to subentities that are parented to this subentity
		DynamicSpan<SubentityModel*> children{};

		// TODO: properties
		// TODO: subsets
		// TODO: exposedInterfaces
	};



	/// @brief Recursively destroy a SubentityModel, along with all of it's children
	/// @param[in] subent Pointer to the SubentityModel instance to destroy
	/*__declspec(noinline)*/ void destroySubentityModel(SubentityModel* subent)
	{
		// Iterate over the immediate children of this subentity and destroy them
		for (int32_t i{ 0 }; i < subent->children.size; i++)
			destroySubentityModel(subent->children.data[i]);

		// Destroy the collection holding pointers to the now-deleted children
		deleteDynamicSpan(subent->children);

		// Destroy this subentity
		delete subent;
	}


	
	/// @brief Allocate a new, unmanaged SubentityModel on the heap
	/// @param[in] parent Subentity that the new subentity will be created as a child of.
	///		May be nullptr if subentity has no parent
	/// @return Raw pointer to newly allocated subentity
	SubentityModel* newSubentityModel(SubentityModel* parent)
	{
		// Create a new subentity
		auto* newSubentity{ new SubentityModel{} };
		
		// Parent the subentity, if it has a parent
		if (parent)
			appendToDynamicSpan(parent->children, newSubentity);

		// Return the new subentity
		return newSubentity;
	}
}