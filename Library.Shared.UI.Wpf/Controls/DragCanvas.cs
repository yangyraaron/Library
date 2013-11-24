using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Library.Client.Wpf.Controls
{
    public class DragCanvas : Canvas
    {
        #region fileds

        private UIElement _elementBeingDragged;

        private Point _origCursorLocation;

        private double _origHorizOffset, _origVertOffset;

        private bool _modifyLeftOffset, _modifyTopOffset;

        private bool _isDragInProgress;

        private OrginalAdnorner _adnorner;


        #endregion


        #region Dependency properties

        public bool AllowDragging
        {
            get { return (bool)GetValue(AllowDraggingProperty); }
            set { SetValue(AllowDraggingProperty, value); }
        }

        public static readonly DependencyProperty AllowDraggingProperty =
            DependencyProperty.Register("AllowDragging", typeof(bool), typeof(DragCanvas), new UIPropertyMetadata(true));



        public bool AllowDragOutOfView
        {
            get { return (bool)GetValue(AllowDragOutOfViewProperty); }
            set { SetValue(AllowDragOutOfViewProperty, value); }
        }

        public static readonly DependencyProperty AllowDragOutOfViewProperty =
            DependencyProperty.Register("AllowDragOutOfView", typeof(bool), typeof(DragCanvas), new UIPropertyMetadata(false));


        public static bool GetCanBeDragged(DependencyObject obj)
        {
            if (obj == null)
                return false;
            return (bool)obj.GetValue(CanBeDraggedProperty);
        }

        public static void SetCanBeDragged(DependencyObject obj, bool value)
        {
            if (obj != null)
                obj.SetValue(CanBeDraggedProperty, value);
        }

        public static readonly DependencyProperty CanBeDraggedProperty =
            DependencyProperty.RegisterAttached("CanBeDragged", typeof(bool), typeof(DragCanvas), new UIPropertyMetadata(false));

        #endregion

        public UIElement ElementBeingDragged
        {
            get
            {
                if (!this.AllowDragging)
                    return null;
                return this._elementBeingDragged;
            }
            protected set
            {
                if (this._elementBeingDragged != null)
                {
                    this._elementBeingDragged.ReleaseMouseCapture();
                }
                if (!this.AllowDragging)
                    this._elementBeingDragged = null;
                else
                {
                    if (DragCanvas.GetCanBeDragged(value))
                    {
                        this._elementBeingDragged = value;
                        this._elementBeingDragged.CaptureMouse();
                    }
                    else
                        this._elementBeingDragged = null;
                }
            }
        }

        public void BringToFront(UIElement element)
        {
            this.UpdateZOrder(element, true);
        }

        public void SendToBack(UIElement element)
        {
            this.UpdateZOrder(element, false);
        }


        private UIElement FindCanvasChild(DependencyObject depObj)
        {
            while (depObj != null)
            {
                // If the current object is a UIElement which is a child of the
                // Canvas, exit the loop and return it.
                UIElement elem = depObj as UIElement;
                if (elem != null && base.Children.Contains(elem))
                    break;

                // VisualTreeHelper works with objects of type Visual or Visual3D.
                // If the current object is not derived from Visual or Visual3D,
                // then use the LogicalTreeHelper to find the parent element.
                if (depObj is Visual || depObj is Visual3D)
                    depObj = VisualTreeHelper.GetParent(depObj);
                else
                    depObj = LogicalTreeHelper.GetParent(depObj);
            }
            return depObj as UIElement;
        }

        private static double ResolveOffset(double side1, double side2, out bool useSide1)
        {
            useSide1 = true;

            if (!double.IsNaN(side1))
            {
                return side1;
            }
            else if (double.IsNaN(side2))
            {
                return 0;
            }
            else
            {
                useSide1 = false;
                return side2;
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(this);
            DependencyObject element = e.Source as DependencyObject;

            OnDragStarting(position, element);

            base.OnPreviewMouseLeftButtonDown(e);

            //e.Handled = true;
        }

        protected override void OnPreviewMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);

            if (this.ElementBeingDragged == null || !this._isDragInProgress)
                return;

            Point position = e.GetPosition(this);

            OnDragMoving(position);
        }

        protected override void OnPreviewMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            Point position = e.GetPosition(this);

            OnDragFinished(position);
        }

        private void OnDragStarting(Point position, DependencyObject element)
        {
            this._isDragInProgress = false;
            this._origCursorLocation = position;
            this.ElementBeingDragged = FindCanvasChild(element);

            if (this.ElementBeingDragged == null)
                return;

            double left = Canvas.GetLeft(this.ElementBeingDragged);
            double right = Canvas.GetRight(this.ElementBeingDragged);
            double top = Canvas.GetTop(this.ElementBeingDragged);
            double bottom = Canvas.GetBottom(this.ElementBeingDragged);

            this._origHorizOffset = ResolveOffset(left, right, out this._modifyLeftOffset);
            this._origVertOffset = ResolveOffset(top, bottom, out this._modifyTopOffset);

            this._isDragInProgress = true;

            AdornerLayer layer = AdornerLayer.GetAdornerLayer(this.ElementBeingDragged);
            if (layer == null)
                return;
            this._adnorner = new OrginalAdnorner(this.ElementBeingDragged);
            layer.Add(this._adnorner);
        }

        private void OnDragMoving(Point position)
        {
            this._adnorner.LeftOffset = position.X - this._origCursorLocation.X;
            this._adnorner.TopOffset = position.Y - this._origCursorLocation.Y;
        }

        private void OnDragFinished(Point cursorLocation)
        {
            if (this.ElementBeingDragged == null)
                return;

            double newHorizOffset, newVertOffset;
            // Determine the horizontal offset.
            if (this._modifyLeftOffset)
                newHorizOffset = this._origHorizOffset + (cursorLocation.X - this._origCursorLocation.X);
            else
                newHorizOffset = this._origHorizOffset - (cursorLocation.X - this._origCursorLocation.X);

            // Determine the vertical offset.
            if (this._modifyTopOffset)
                newVertOffset = this._origVertOffset + (cursorLocation.Y - this._origCursorLocation.Y);
            else
                newVertOffset = this._origVertOffset - (cursorLocation.Y - this._origCursorLocation.Y);

            if (!this.AllowDragOutOfView)
            {
                #region Verify Drag Element Location

                // Get the bounding rect of the drag element.
                Rect elemRect = this.CalculateDragElementRect(newHorizOffset, newVertOffset);

                //
                // If the element is being dragged out of the viewable area, 
                // determine the ideal rect location, so that the element is 
                // within the edge(s) of the canvas.
                //
                bool leftAlign = elemRect.Left < 0;
                bool rightAlign = elemRect.Right > this.ActualWidth;

                if (leftAlign)
                    newHorizOffset = _modifyLeftOffset ? 0 : this.ActualWidth - elemRect.Width;
                else if (rightAlign)
                    newHorizOffset = _modifyLeftOffset ? this.ActualWidth - elemRect.Width : 0;

                bool topAlign = elemRect.Top < 0;
                bool bottomAlign = elemRect.Bottom > this.ActualHeight;

                if (topAlign)
                    newVertOffset = _modifyTopOffset ? 0 : this.ActualHeight - elemRect.Height;
                else if (bottomAlign)
                    newVertOffset = _modifyTopOffset ? this.ActualHeight - elemRect.Height : 0;

                #endregion // Verify Drag Element Location
            }

            if (this._modifyLeftOffset)
                Canvas.SetLeft(this.ElementBeingDragged, newHorizOffset);
            else
                Canvas.SetRight(this.ElementBeingDragged, newHorizOffset);

            if (this._modifyTopOffset)
                Canvas.SetTop(this.ElementBeingDragged, newVertOffset);
            else
                Canvas.SetBottom(this.ElementBeingDragged, newVertOffset);

            var layer = AdornerLayer.GetAdornerLayer(this.ElementBeingDragged);
            if (layer != null)
                layer.Remove(_adnorner);

            this.ElementBeingDragged = null;
        }

        private Rect CalculateDragElementRect(double newHorizOffset, double newVertOffset)
        {
            if (this.ElementBeingDragged == null)
                throw new InvalidOperationException("ElementBeingDragged is null");

            Size elemSize = this.ElementBeingDragged.RenderSize;

            double x, y;
            if (this._modifyLeftOffset)
                x = newHorizOffset;
            else
                x = this.ActualWidth - newHorizOffset - elemSize.Width;

            if (this._modifyTopOffset)
                y = newVertOffset;
            else
                y = this.ActualHeight - newVertOffset - elemSize.Height;

            Point p = new Point(x, y);

            return new Rect(p, elemSize);
        }

        private void UpdateZOrder(UIElement element, bool bringToFront)
        {
            #region Safety Check

            if (element == null)
                throw new ArgumentNullException("element");

            if (!base.Children.Contains(element))
                throw new ArgumentException("Must be a child element of the Canvas.", "element");

            #endregion // Safety Check

            #region Calculate Z-Indici And Offset

            // Determine the Z-Index for the target UIElement.
            int elementNewZIndex = -1;
            if (bringToFront)
            {
                foreach (UIElement elem in base.Children)
                    if (elem.Visibility != Visibility.Collapsed)
                        ++elementNewZIndex;
            }
            else
            {
                elementNewZIndex = 0;
            }

            // Determine if the other UIElements' Z-Index 
            // should be raised or lowered by one. 
            int offset = (elementNewZIndex == 0) ? +1 : -1;

            int elementCurrentZIndex = Canvas.GetZIndex(element);

            #endregion // Calculate Z-Indici And Offset

            #region Update Z-Indici

            // Update the Z-Index of every UIElement in the Canvas.
            foreach (UIElement childElement in base.Children)
            {
                if (childElement == element)
                    Canvas.SetZIndex(element, elementNewZIndex);
                else
                {
                    int zIndex = Canvas.GetZIndex(childElement);

                    // Only modify the z-index of an element if it is  
                    // in between the target element's old and new z-index.
                    if (bringToFront && elementCurrentZIndex < zIndex ||
                        !bringToFront && zIndex < elementCurrentZIndex)
                    {
                        Canvas.SetZIndex(childElement, zIndex + offset);
                    }
                }
            }

            #endregion // Update Z-Indici
        }

    }
}
