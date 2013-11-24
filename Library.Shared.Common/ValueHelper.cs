using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Library.Common
{
    /// <summary>
    /// provides some functions to operate value
    /// </summary>
    internal class ValueHelper
    {
        /// <summary>
        /// get the actual value from the nullable struct value
        /// </summary>
        /// <typeparam name="T">value type</typeparam>
        /// <param name="value">nullable value</param>
        /// <param name="defaultValue">default value if the nullable doesn't have value</param>
        /// <returns></returns>
        public static T GetValue<T>(T? value, T defaultValue) where T : struct
        {
            if (value.HasValue)
                return value.Value;

            return defaultValue;
        }

        /// <summary>
        /// get a specific type value from a struct type
        /// </summary>
        /// <typeparam name="T">the source type</typeparam>
        /// <typeparam name="TTarget">the target type which the reture value will be </typeparam>
        /// <param name="sourceValue">the source value</param>
        /// <param name="defaultValue">the value will be returned in case of the source value doesn't have value
        /// or the source value can't be converted to the target type</param>
        /// <returns></returns>
        public static TTarget GetValue<T, TTarget>(T? sourceValue, TTarget defaultValue) where T : struct
        {
            if (sourceValue.HasValue)
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                Type type = typeof(TTarget);

                if (converter.CanConvertTo(type))
                {
                    return (TTarget)converter.ConvertTo(sourceValue.Value, type);
                }
                else
                    return defaultValue;
            }
            else
                return defaultValue;
        }

        /// <summary>
        /// get a string value from nullabe value
        /// </summary>
        /// <typeparam name="T">the source value type</typeparam>
        /// <param name="sourceValue">the source value</param>
        /// <param name="defaultValue">the value will be returned in case of the source have doesn't have value</param>
        /// <param name="formatAction">the action will be used to format the value</param>
        /// <returns></returns>
        public static string GetStringValue<T>(T? sourceValue, string defaultValue, Func<T, string> formatAction)
            where T : struct
        {
            if (sourceValue.HasValue)
            {
                if (formatAction != null)
                    return formatAction(sourceValue.Value);
                else
                    return sourceValue.Value.ToString();
            }
            else
                return defaultValue;
        }

        /// <summary>
        /// get a specific type value from a string value
        /// </summary>
        /// <typeparam name="TValue">the target type which the returned value will be</typeparam>
        /// <param name="value">the source value</param>
        /// <param name="defaultValue">the value will be returned in case of the source doesn't have value</param>
        /// <returns></returns>
        public static TValue? GetValueFromString<TValue>(string value, TValue? defaultValue) where TValue : struct
        {
            if (value == string.Empty)
                return defaultValue;
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(TValue));
            Type type = typeof(string);

            if (converter.CanConvertFrom(type))
            {
                return (TValue?)converter.ConvertFrom(value);
            }

            return defaultValue;
        }

        /// <summary>
        /// convert a value to a specific type
        /// </summary>
        /// <typeparam name="T">the target type</typeparam>
        /// <param name="originalValue">original value</param>
        /// <returns></returns>
        public static T To<T>(object originalValue)
        {
            T value = default(T);
            Type sourceType = originalValue.GetType();

            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter == null)
                return value;

            if (converter.CanConvertFrom(sourceType))
            {
                value = (T)converter.ConvertFrom(originalValue);
            }

            return value;
        }

        /// <summary>
        /// convert a value to a specific enum value
        /// </summary>
        /// <typeparam name="T">the enum type</typeparam>
        /// <typeparam name="U">the original value type</typeparam>
        /// <param name="value">the original value</param>
        /// <returns>a value of the enum type</returns>
        public static T ToEnum<T,U>(U value) where T:struct
        {
            T reValue = default(T);

            if (value == null)
                return reValue;

           return (T) Enum.Parse(typeof(T), value.ToString());
            
        }
    }
}
