#pragma once
/// @file
/// @author Andrew Pratt

#include <assert.h>
#include <string>
#include <cstdint>


namespace ProtoBCpp
{

	/// @brief A dynamic span of data
	/// @tparam T Datatype of each item
	/// @detail Used to store an unmanaged set of contiguous items of the same data type.
	///		The size or amount of data may or may not change at run-time
	template<typename T>
	struct DynamicSpan
	{
		/// @brief Amount of items
		int32_t size{ 0 };
		/// @brief Maximum amount of items
		/// @detail May be changed, but requires a reallocation to do so
		int32_t capacity{ 0 };
		/// @brief Raw pointer to the first item
		/// @detail May be nullptr if no items are stored
		T* data{ nullptr };
	};


	/// @brief String stored in native memory
	/// @detail Holds a set of char16_t items, since C# stores characters as UTF-16
	typedef DynamicSpan<char16_t> NativeString;


	/// @brief Deletes a DynamicSpan's heap-allocated data
	/// @param[in] span The DynamicSpan to delete
	/// @tparam T DynamicSpan type
	/// @warning Do NOT attempt to use the "data" property of the span after calling this function
	template <typename T>
	inline void deleteDynamicSpan(DynamicSpan<T>& span)
	{
		delete[] span.data;
	}


	/// @brief Resizes a DynamicSpan
	/// @param[in] span The DynamicSpan instance to resize
	/// @param[in] capacity New maximum amount of items of span
	/// @tparam T DynamicSpan type
	// TODO: Test for memory overflow
	template <typename T>
	void resizeDynamicSpan(DynamicSpan<T>& span, const int32_t capacity)
	{
		assert(span.size < capacity);
		// Assert that capacity isn't too small
		/*if (capacity < span.size)
			throw "Capacity too small";*/

		// Allocate new data
		T* newData{ new T[capacity]{} };

		// Copy the old data into the new data
		for (auto i{ 0 }; i < span.size; i++)
		{
			newData[i] = span.data[i];
		}

		// Delete old data, if it exists
		if (span.data)
			delete[] span.data;
		
		// Update span to point to the new data
		span.data = newData;
		span.capacity = capacity;
	}


	/// @brief Append an item to a DynamicSpan, resizing if necessary
	/// @param[in] span The DynamicSpan to append to
	/// @param[in] v Value to append to the span
	/// @tparam T DynamicSpan type
	/// @detail Item is appended by-value, so a copy may possibly be created
	// TODO: Test for memory overflow
	template <typename T>
	void appendToDynamicSpan(DynamicSpan<T>& span, T v)
	{
		assert(span.size <= span.capacity);

		// Resize the span if it's at it's current full capacity
		if (span.size == span.capacity)
			resizeDynamicSpan(span, span.size + 1);
		
		// Add the new item to the span
		span.data[span.size] = v;

		// Update the span's size property
		++span.size;
	}


	/// @brief Deletes a DynamicSpan's heap-allocated data
	/// @param[in] span The DynamicSpan to delete
	/// @tparam T DynamicSpan type
	/// @detail This is a safer version of deleteDynamicSpan. In addition to deleting the span's
	///		data, it's size and capacity properties are set to zero and it's data property is set to nullptr.
	///		This allows for testing if the span has been deleted after calling, but may create more overhead if used.
	///		Do NOT attempt to use the "data" property of the span after calling this function
	template <typename T>
	inline void safeDeleteDynamicSpan(DynamicSpan<T>& span)
	{
		delete[] span.data;
		span.size = 0;
		span.capacity = 0;
		span.data = nullptr;
	}
}