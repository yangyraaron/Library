using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Library.Client.Wpf.Controls;

namespace Library.Client.Wpf.Behaviors
{
    public static class AsyncBehavior
    {
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.RegisterAttached(
            "IsBusy",
            typeof(Boolean),
            typeof(AsyncBehavior),
            new FrameworkPropertyMetadata(false,
                (d, e) =>
                {
                    UIElement element = d as UIElement;
                    if (element == null)
                        return;
                    Boolean value = (Boolean)e.NewValue;

                    AdornerLayer layer = AdornerLayer.GetAdornerLayer(element);
                    if (layer == null)
                        return;
                    if (value)
                    {
                        element.IsEnabled = false;

                        AsyncAdorner adorner = new AsyncAdorner(AsyncBehavior.GetIndicator(element), element);
                        AsyncBehavior.SetAsyncAdorner(element, adorner);
                        layer.Add(adorner);
                    }
                    else
                    {
                        var adorner = AsyncBehavior.GetAsyncAdorner(element);

                        if (adorner == null)
                            return;
                        layer.Remove(adorner);

                        element.IsEnabled = true;
                    }
                        
                }));

        public static Boolean GetIsBusy(UIElement element)
        {
            return (Boolean)element.GetValue(IsBusyProperty);
        }
        public static void SetIsBusy(UIElement element, Boolean value)
        {
            element.SetValue(IsBusyProperty, value);
        }

        public static readonly DependencyProperty IndicatorProperty = DependencyProperty.RegisterAttached(
            "Indicator",
            typeof(Visual),
            typeof(AsyncBehavior),
            new FrameworkPropertyMetadata(null));

        public static Visual GetIndicator(UIElement element)
        {
            return (Visual)element.GetValue(IndicatorProperty);
        }
        public static void SetIndicator(UIElement element, Object value)
        {
            element.SetValue(IndicatorProperty, value);
        }

        private static readonly DependencyProperty AsyncAdornerproperty = DependencyProperty.RegisterAttached(
            "AsyncAdorner",
            typeof(AsyncAdorner),
            typeof(AsyncBehavior),
            new FrameworkPropertyMetadata(null));

        private static AsyncAdorner GetAsyncAdorner(UIElement element)
        {
            return (AsyncAdorner) element.GetValue(AsyncAdornerproperty);
        }
        private static void SetAsyncAdorner(UIElement element, AsyncAdorner value)
        {
            element.SetValue(AsyncAdornerproperty,value);
        }
    }
}
