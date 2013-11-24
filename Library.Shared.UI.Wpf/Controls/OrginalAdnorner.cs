using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Shapes;

namespace Library.Client.Wpf.Controls
{
    public class OrginalAdnorner : Adorner
    {
        #region fields

        private Rectangle _child;

        #endregion

        #region constructors

        public OrginalAdnorner(UIElement adornedElment)
            : base(adornedElment)
        {
            CreateChild();

            this.IsHitTestVisible = true;
        }

        #endregion

        /// <summary>
        /// create a rectangle as adorner's child based on adorned element
        /// </summary>
        private void CreateChild()
        {
            var brush = new VisualBrush(this.AdornedElement);

            _child = new Rectangle();
            _child.Width = this.AdornedElement.DesiredSize.Width;
            _child.Height = this.AdornedElement.DesiredSize.Height;
            _child.Opacity = 0.6;
            _child.Fill = brush;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var rect = new Rect(this.AdornedElement.RenderSize);
            var renderingBrush = new SolidColorBrush(Colors.Transparent);
            //draw a rectangle based on adorned elment.
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

        private double _leftOffset = 0;
        public double LeftOffset
        {
            get
            { return _leftOffset; }
            set
            {
                _leftOffset = value;
                UpdatePostion();
            }
        }

        private double _topOffset = 0;
        public double TopOffset
        {
            get
            { return _topOffset; }
            set
            {
                _topOffset = value;
                UpdatePostion();
            }
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup transformGroup = new GeneralTransformGroup();
            transformGroup.Children.Add(base.GetDesiredTransform(transform));
            transformGroup.Children.Add(new TranslateTransform(LeftOffset, TopOffset));

            return transformGroup;
        }

        private void UpdatePostion()
        {
            var adornerLayer = this.Parent as AdornerLayer;

            if (adornerLayer == null)
                return;
            adornerLayer.Update(this.AdornedElement);
        }
    }
}
