using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoB.Native
{
    static class Util
    {
        public const int ENTITY_MODEL_NAME_MAXLEN = 255;

        public const int SUBENTITY_MODEL_EDITOR_NAME_MAXLEN = 48;
        public const int SUBENTITY_MODEL_TRUE_NAME_MAXLEN = 48;


        /// <summary>Assigns a string to a char array</summary>
        /// <param name="arr">Pointer to first character of array to mutate</param>
        /// <param name="newValue">Value to set array to. A nullchar will be appended to this value as well</param>
        /// <remarks>arr is assumed to be longer than newValue. If newValue is longer or the same length, undefined behavior(?) and/or errors may occur</remarks>
        public unsafe static void AssignStringToCharArray(char* arr, string newValue)
        {
            // Get the length of the new string
            var strlen = newValue.Length;

            // Add each character from the new string to the char array
            for (int i = 0; i < strlen; ++i)
            {
                arr[i] = newValue[i];
            }

            // Add a nullchar at the end
            arr[strlen] = (char)0;
        }


        /// <summary>Safely assigns a value to a char array</summary>
        /// <param name="arr">Pointer to first character of array to mutate</param>
        /// <param name="maxlen">Max string size, including the nullchar</param>
        /// <param name="newValue">Value to set array to. A nullchar will be appended to this value as well</param>
        /// <remarks>Value will be truncated if it's longer than maxlen</remarks>
        public unsafe static void AssignStringToCharArray(char* arr, int maxlen, string newValue)
        {
            if (newValue.Length >= maxlen)
                AssignStringToCharArray(arr, newValue.Substring(0, maxlen - 1));
            else
                AssignStringToCharArray(arr, newValue);
        }
    }
}
