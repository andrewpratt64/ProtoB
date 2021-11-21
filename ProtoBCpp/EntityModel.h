#pragma once
/// @file
/// @author Andrew Pratt

#include <cstdint>

#include "SubentityModel.h"
//#include "DynamicSpan.h"


namespace ProtoBCpp
{
	constexpr size_t ENTITY_MODEL_NAME_MAXLEN{ 255ULL };

	/// @brief A single entity
	/// @detail Model for MVVM, used in interop
	struct EntityModel
	{
		/// @brief Name of the entity
		char16_t name[ENTITY_MODEL_NAME_MAXLEN]{};
		/// @brief Entity subtype
		/// @todo How does subtype work?
		int32_t subtype{};
		/// @brief The root subentity
		/// @detail This is the subentity at the root of the entity's composition hierarchy
		// TODO: Is composition hierarchy actually the right word? A lot of the data relationships
		//	between subentities is technically aggregation as well.
		SubentityModel* rootSubentity{ nullptr };
		// TODO: externalScenes
		// TODO: propertyOverrides
		// TODO: overrideDeletes
		// TODO: pinOverrides
		// TODO: pinDeleteOverrides
	};


	
	/// @brief Destroy an EntityModel
	/// @param[in] ent EntityModel instance to destroy
	void destroyEntityModel(EntityModel* ent)
	{
		// Recursively destroy the root subentity
		destroySubentityModel(ent->rootSubentity);
	}
}