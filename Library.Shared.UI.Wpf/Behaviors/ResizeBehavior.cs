using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;

namespace Library.Client.Wpf.Behaviors
{
    /// <summary>
    /// the resize behavior for pop resizing
    /// </summary>
    public static class ResizeBehavior
    {
        public static ResizeDirection GetResizeDirection(UIElement element)
        {
            return (ResizeDirection)element.GetValue(ResizeDirectionProperty);
        }

        public static void SetResizeDirection(UIElement element, object value)
        {
            element.SetValue(ResizeDirectionProperty, value);
        }

        /// <summary>
        /// set the resizing direction
        /// </summary>
        public static readonly DependencyProperty ResizeDirectionProperty =
            DependencyProperty.RegisterAttached("ResizeDirection", 
                                        typeof(ResizeDirection), 
                                        typeof(ResizeBehavior), 
                                        new UIPropertyMetadata(ResizeDirection.Both));

        #region popup reisizing

        public static bool GetIsPopupOpen(UIElement element)
        {
            return (bool)element.GetValue(IsPopupOpenProperty);
        }
        public static void SetIsPopupOpen(UIElement element, object value)
        {
            element.SetValue(IsPopupOpenProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsPopupOpened.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPopupOpenProperty =
            DependencyProperty.RegisterAttached("IsPopupOpen",
                                                typeof(bool),
                                                typeof(ResizeBehavior),
                                                new FrameworkPropertyMetadata(false,
                                                    (o, e) => {
                                                        var ctl = o as UIElement;
                                                        if (ctl == null)
                                                            return;
                                                        
                                                        var pop = ctl.FindFirstVisualChild<Popup>();
                                                        if (pop == null)
                                                            return;

                                                        var thumb = pop.FindFirstVisualChild<Thumb>();

                                                        if (thumb == null)
                                                            thumb = pop.FindFirstLogicalChild<Thumb>();

                                                        if(thumb == null)
                                                            return;

                                                        if (!GetIsResizeGrip(thumb))
                                                            return;

                                                        SetResizingPopup(thumb, pop);
                                                        var direction = GetResizeDirection(ctl);
                                                        SetResizeDirection(thumb, direction);

                                                        bool isOpen = (bool)e.NewValue;
                                                        if (isOpen)
                                                        {
                                                            thumb.DragStarted += new DragStartedEventHandler(thumb_DragStarted);
                                                            thumb.DragDelta += new DragDeltaEventHandler(thumb_DragDelta);
                                                            thumb.DragCompleted += new DragCompletedEventHandler(thumb_DragCompleted);
                                                        }
                                                        else
                                                        {
                                                            thumb.DragStarted -= new DragStartedEventHandler(thumb_DragStarted);
                                                            thumb.DragDelta -= new DragDeltaEventHandler(thumb_DragDelta);
                                                            thumb.DragCompleted -= new DragCompletedEventHandler(thumb_DragCompleted);
                                                        }
                                                    }));


        public static bool GetIsResizeGrip(UIElement element)
        {
            return (bool)element.GetValue(IsResizeGripProperty);
        }

        public static void SetIsResizeGrip(UIElement element, object value)
        {
            element.SetValue(IsResizeGripProperty, value);
        }

        /// <summary>
        /// indicate if the thumb is resizing grip
        /// </summary>
        public static readonly DependencyProperty IsResizeGripProperty =
            DependencyProperty.RegisterAttached("IsResizeGrip",
                                                typeof(bool),
                                                typeof(ResizeBehavior),
                                                new FrameworkPropertyMetadata(false));

        /// <summary>
        /// resizing 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = sender as Thumb;

            if (thumb == null)
                return;

            Popup pop = GetResizingPopup(thumb) as Popup;

            if (pop == null)
                return;

            var popHeight = 0d;
            var popWidth = 0d;

            //if the pop's height is nan then use the child's height to caculate
            popHeight = double.IsNaN(pop.Height) ? pop.Child.RenderSize.Height : pop.Height;
            //if the pop's wdith is nan the use the child's widht to cacualte
            popWidth = double.IsNaN(pop.Width) ? pop.Child.RenderSize.Width : pop.Width;

            var adujstY = popHeight + e.VerticalChange;
            var adjustX = popWidth + e.HorizontalChange;

            //the height and width must be between the min value and max value
            var x = Math.Min(pop.MaxWidth, Math.Max(adjustX, pop.MinWidth));
            var y = Math.Min(pop.MaxHeight, Math.Max(adujstY, pop.MinHeight));

            var direction = GetResizeDirection(thumb);

            switch (direction)
            {
                case ResizeDirection.Horizontal:
                    pop.Width = x;
                    break;
                case ResizeDirection.Vertical:
                    pop.Height = y;
                    break;
                default:
                    pop.Width = x;
                    pop.Height = y;
                    break;
            }
        }

        /// <summary>
        /// resized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            var t = sender as Thumb;
            t.Cursor = null;
        }

        /// <summary>
        /// resizing started
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void thumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            var t = sender as Thumb;
            t.Cursor = Cursors.SizeNS;
        }

        /// <summary>
        /// get the resizing popup
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Popup GetResizingPopup(UIElement element)
        {
            return (Popup)element.GetValue(ResizingPopupProperty);
        }
        /// <summary>
        /// set the resizing popup
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetResizingPopup(UIElement element, object value)
        {
            element.SetValue(ResizingPopupProperty, value);
        }

        /// <summary>
        /// the resizing popup property which hosts the popup that will be resized
        /// </summary>
        public static readonly DependencyProperty ResizingPopupProperty =
            DependencyProperty.RegisterAttached("ResizingPopup",
                                        typeof(Popup),
                                        typeof(ResizeBehavior),
                                        new UIPropertyMetadata(null));

        #endregion


    }

    /// <summary>
    /// resizing model
    /// </summary>
    public enum ResizeDirection
    {
        Horizontal,
        Vertical,
        Both
    }
}
