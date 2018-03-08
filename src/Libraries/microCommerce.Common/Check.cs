using System;
using System.Collections.Generic;
using System.Linq;

namespace microCommerce.Common
{
    public class Check
    {
        public static void IsNullOrEmpty<T>(IEnumerable<T> collection)
        {
            if (collection == null || !collection.Any())
                throw new ArgumentException("Collection cannot be null or empty!");
        }

        public static void IsNullOrEmpty<T>(string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be null, empty or whitespace!");
        }

        public static void IsNull(object obj)
        {
            if (obj == null)
                throw new ArgumentException("Object cannot be null!");
        }

        public static void IsEmpty(string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Object cannot be empty or whitespace!");
        }
    }
}