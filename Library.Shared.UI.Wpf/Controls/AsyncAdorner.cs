using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Library.Client.Wpf.Controls
{
    public class AsyncAdorner : Adorner
    {
        private Visual _content ;
        private Rectangle _child;

        public AsyncAdorner(UIElement adornedElement)
            :base(adornedElement)
        {
            InitilizeAsyncComponent();
        }

        public AsyncAdorner(Visual content, UIElement adornedElement)
            :base(adornedElement)
        {
            this._content = content;

            InitilizeAsyncComponent();
        }

        protected void InitilizeAsyncComponent()
        {
            _child = new Rectangle();
            var brush = new VisualBrush(_content as Visual);
            brush.Stretch = Stretch.None;

            _child.Width = this.AdornedElement.RenderSize.Width;
            _child.Height = this.AdornedElement.RenderSize.Height;
            _child.Opacity = 0.6;
            _child.Fill = brush;
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            Rect rect = new Rect(this.AdornedElement.RenderSize);
            
            var renderingBrush = new SolidColorBrush(Colors.Transparent);

            drawingContext.DrawRectangle(renderingBrush, null, rect);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _child.Measure(constraint);
            return _child.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _child.Arrange(new Rect(finalSize));

            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _child;
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

    }
}
