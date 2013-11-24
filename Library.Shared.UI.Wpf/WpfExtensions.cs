using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Markup;
using System.Xml;
using System.IO;

namespace Library.Client.Wpf
{
    public static class WpfExtensions
    {
        /// <summary>
        /// find the specifical type child from  visual tree
        /// </summary>
        /// <typeparam name="T">the child type</typeparam>
        /// <param name="dependencyObject">the root object</param>
        /// <returns></returns>
        public static T FindFirstVisualChild<T>(this DependencyObject dependencyObject)
            where T:DependencyObject
        {
            var targetType = typeof(T);
            int childCount = VisualTreeHelper.GetChildrenCount(dependencyObject);

            if (childCount == 0)
                return null;

            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);

                if (child.DependencyObjectType.SystemType == targetType)
                    return child as T;
                
                var final =  WpfExtensions.FindFirstVisualChild<T>(child);

                if (final != null)
                    return final;
            }

            return null;
        }

        /// <summary>
        /// find the specifical type child from  logical tree
        /// </summary>
        /// <typeparam name="T">the child type</typeparam>
        /// <param name="dependencyObject">the root object</param>
        /// <returns></returns>
        public static T FindFirstLogicalChild<T>(this DependencyObject dependencyObject)
            where T:DependencyObject
        {
            var targetType = typeof(T);
            var children = LogicalTreeHelper.GetChildren(dependencyObject);

            foreach (var child in children)
            {
                DependencyObject obj = child as DependencyObject;
                if (obj == null)
                    return null;

                if (obj.DependencyObjectType.SystemType == targetType)
                    return obj as T;

                var final = FindFirstLogicalChild<T>(obj);

                if (final != null)
                    return final;
            }

            return null;
        }

        /// <summary>
        /// find the specifical type parent from  logic tree
        /// </summary>
        /// <typeparam name="T">the parent type</typeparam>
        /// <param name="dependencyObject">the root object</param>
        /// <returns></returns>
        public static T FindLogicParent<T>(this DependencyObject dependencyObject)
            where T:DependencyObject
        {
            var parent = LogicalTreeHelper.GetParent(dependencyObject);

            if (parent == null)
                return null;

            T tp = parent as T;
            if (tp == null)
                return FindLogicParent<T>(parent);

            return tp;
        }

        /// <summary>
        /// find the first specific type visual parent
        /// </summary>
        /// <typeparam name="T">the sepcific type</typeparam>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        public static T FindVisualParent<T>(this DependencyObject dependencyObject)
            where T:DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null)
                return null;

            if (parent is T)
                return parent as T;

            return FindVisualParent<T>(parent);

        }

        /// <summary>
        /// clone the element
        /// </summary>
        /// <param name="dependencyObject"></param>
        /// <returns></returns>
        public static T CloneElement<T>(this T dependencyObject)
        {
            string xaml = XamlWriter.Save(dependencyObject);
            var obj = XamlReader.Load(new XmlTextReader(new StringReader(xaml)));

            return (T)obj;
        }

    }
}
