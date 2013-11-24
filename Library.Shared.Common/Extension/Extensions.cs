using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Library.Common.Extension
{
    /// <summary>
    /// define some extenion methods in this class
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// indicate if the object instance is null
        /// </summary>
        /// <param name="value">the object instance</param>
        /// <returns>if the value  is null then return </returns>
        public static bool IsNull(this object value)
        {
            if (value == null)
                return true;
            return false;
        }

        /// <summary>
        /// convert a object value to a specific type value;
        /// </summary>
        /// <typeparam name="T">the type the original value will be converted</typeparam>
        /// <param name="obj">the original value</param>
        /// <returns>if the object can convert to the specific value,it will return the converted value,
        /// else return the default(T) value.</returns>
        public static T To<T>(this object obj)
        {
            if (obj is T)
                return (T)obj;

            return ValueHelper.To<T>(obj);
        }

        /// <summary>
        /// get nullable value's actual value
        /// </summary>
        /// <typeparam name="T">the value type</typeparam>
        /// <param name="value">the nullable value</param>
        /// <param name="defaultValue">if the nullable value does not have value,the default value will be returned</param>
        /// <returns></returns>
        public static T Get<T>(this T? value, T defaultValue) where T : struct
        {
            if (value.HasValue)
                return value.Value;
            return defaultValue;
        }

        /// <summary>
        /// indicate if the <see cref="IEnumerable"/> instance is null or empty
        /// </summary>
        /// <typeparam name="T">any type</typeparam>
        /// <param name="collection">the instance of <see cref="IEnumerable"/></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            if (collection.IsNull())
                return true;
            if (!collection.Any())
                return true;
            return false;
        }

        /// <summary>
        /// get obj as type of T,if the obj isn't type of T,then the return value if default(T)
        /// </summary>
        /// <typeparam name="T">the type which the object is going to be casted to</typeparam>
        /// <param name="obj">the object instance which is going to be casted </param>
        /// <returns></returns>
        public static T As<T>(this object obj) 
        {
            if (obj is T)
                return (T)obj;
            else
                return default(T);
        }

        /// <summary>
        /// fire the specifc delegate
        /// </summary>
        /// <param name="action">the delegate action</param>
        /// <param name="args">arguments for excuting the delegate</param>
        /// <returns>retun a object by excuting the action
        /// <remarks>
        /// if the action is null then the retun value is null
        /// </remarks>
        /// </returns>
        public static object SafeFire(this Delegate action, params object[] args)
        {
            var handler = action;
            if (handler != null)
                return handler.DynamicInvoke(args);
            return null;
        }

        /// <summary>
        /// fire the specific delegate which return a geneirc type
        /// </summary>
        /// <typeparam name="T">the return type</typeparam>
        /// <param name="action">the delegate action</param>
        /// <param name="args">arguments for excuting the delegate</param>
        /// <returns>retun a object by excuting the action
        /// <remarks>
        /// because the return type is generic type,so if the action is null
        /// the the return value is Default
        /// </remarks>
        /// </returns>
        public static T SafeFire<T>( this Delegate action, params object[] args)
        {
            var handler = action;
            if (handler != null)
                return ValueHelper.To<T>(handler.DynamicInvoke(args));
            return default(T);
        }

        /// <summary>
        /// if the string is null or empty then return default string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultString"></param>
        /// <returns></returns>
        public static string DefaultIfNull(this string str, string defaultString)
        {
            if (str.IsNullOrEmpty())
                return defaultString;
            else
                return str;
        }
    }

    /// <summary>
    /// the class contains the extensions for type instance
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// create a instance of type
        /// </summary>
        /// <typeparam name="T">the type of return value</typeparam>
        /// <param name="type">the type</param>
        /// <returns></returns>
        public static T CreateInstance<T>(this Type type)
        {
            return Activator.CreateInstance(type).As<T>();
        }
    }
}
