    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Library.Client.Wpf.Controls
{
    [TemplatePart(Name = ANIMATION_ELEMENT1, Type = typeof(Rectangle))]
    [TemplatePart(Name = ANIMATION_ELEMENT2, Type = typeof(Rectangle))]
    [TemplatePart(Name = ANIMATION_ELEMENT3, Type = typeof(Rectangle))]
    [TemplatePart(Name = ANIMATION_ELEMENT4, Type = typeof(Rectangle))]
    [TemplatePart(Name = ANIMATION_ELEMENT5, Type = typeof(Rectangle))]
    [TemplatePart(Name = ANIMATION_ELEMENT6, Type = typeof(Rectangle))]
    [TemplatePart(Name = ANIMATION_ELEMENT7, Type = typeof(Rectangle))]
    [TemplatePart(Name = ANIMATION_ELEMENT8, Type = typeof(Rectangle))]

    public class FlowerLoading : Control
    {
        #region Constants

        private const string ANIMATION_ELEMENT1 = "PART_AnimationElement1";
        private const string ANIMATION_ELEMENT2 = "PART_AnimationElement2";
        private const string ANIMATION_ELEMENT3 = "PART_AnimationElement3";
        private const string ANIMATION_ELEMENT4 = "PART_AnimationElement4";
        private const string ANIMATION_ELEMENT5 = "PART_AnimationElement5";
        private const string ANIMATION_ELEMENT6 = "PART_AnimationElement6";
        private const string ANIMATION_ELEMENT7 = "PART_AnimationElement7";
        private const string ANIMATION_ELEMENT8 = "PART_AnimationElement8";


        #endregion

        #region Fields

        private Rectangle _animation1;
        private Rectangle _animation2;
        private Rectangle _animation3;
        private Rectangle _animation4;
        private Rectangle _animation5;
        private Rectangle _animation6;
        private Rectangle _animation7;
        private Rectangle _animation8;

        #endregion

        static FlowerLoading()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlowerLoading),
             new FrameworkPropertyMetadata(typeof(FlowerLoading)));
        }

        #region Overrid Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _animation1 = GetTemplateChild(ANIMATION_ELEMENT1) as Rectangle;
            _animation2 = GetTemplateChild(ANIMATION_ELEMENT2) as Rectangle;
            _animation3 = GetTemplateChild(ANIMATION_ELEMENT3) as Rectangle;
            _animation4 = GetTemplateChild(ANIMATION_ELEMENT4) as Rectangle;
            _animation5 = GetTemplateChild(ANIMATION_ELEMENT5) as Rectangle;
            _animation6 = GetTemplateChild(ANIMATION_ELEMENT6) as Rectangle;
            _animation7 = GetTemplateChild(ANIMATION_ELEMENT7) as Rectangle;
            _animation8 = GetTemplateChild(ANIMATION_ELEMENT8) as Rectangle;

            InitializeElements();
        }

        private void InitializeElements()
        {
            var totalLen = this.CenterLength + this.ElementHeight;
            var centerX = 6 + totalLen;
            var centerY = 2 + totalLen;

            SetTransformValue(centerX, 2, _animation1,null);
            SetTransformValue(centerX, centerY + totalLen, _animation2, null);
            SetTransformValue(centerX + totalLen, centerY, _animation3,90);
            SetTransformValue(6, centerY, _animation4,-90);
            SetTransformValue(centerX+this.ElementHeight, 2+this.CenterLength, _animation5,45);
            SetTransformValue(centerX + this.ElementHeight, centerY+this.ElementHeight, _animation6,135);
            SetTransformValue(6 + this.CenterLength, centerY + this.ElementHeight, _animation7,-135);
            SetTransformValue(6 + this.CenterLength, 2 + this.CenterLength, _animation8,-45);
        }

        private void SetTransformValue(double x, double y,Rectangle rect,double? angle)
        {
            if (rect == null)
                return;

            var group = new TransformGroup();
            if (angle.HasValue)
            {
                RotateTransform rt = new RotateTransform(angle.Value);
                group.Children.Add(rt);
            }

            TranslateTransform transf = new TranslateTransform(x,y);

            group.Children.Add(transf);

            rect.RenderTransform = group;

        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty Element1XProperty =
            DependencyProperty.Register("Element1X",
                                        typeof(double),
                                        typeof(FlowerLoading),
                                        new FrameworkPropertyMetadata((double)16));

        

        public double ElementWidth
        {
            get { return (double)GetValue(ElementWidthProperty); }
            set { SetValue(ElementWidthProperty, value); }
        }
        public static readonly DependencyProperty ElementWidthProperty =
            DependencyProperty.Register("ElementWidth", 
                                        typeof(double), 
                                        typeof(FlowerLoading), 
                                        new FrameworkPropertyMetadata((double) 8));

        public double ElementHeight
        {
            get { return (double)GetValue(ElementHeightProperty); }
            set { SetValue(ElementHeightProperty, value); }
        }
        public static readonly DependencyProperty ElementHeightProperty =
            DependencyProperty.Register("ElementHeight",
                                         typeof(double),
                                         typeof(FlowerLoading),
                                         new FrameworkPropertyMetadata((double)16,
                                             OnResizeElements));
        private static void OnResizeElements(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FlowerLoading ctl = d as FlowerLoading;
            if (ctl == null)
                return;

            ctl.InitializeElements();
        }


        public double CenterLength
        {
            get { return (double)GetValue(CenterLengthProperty); }
            set { SetValue(CenterLengthProperty, value); }
        }

        public static readonly DependencyProperty CenterLengthProperty =
            DependencyProperty.Register("CenterLength",
                                        typeof(double), 
                                        typeof(FlowerLoading),
                                        new FrameworkPropertyMetadata((double)4, OnResizeElements));

        
        
        private static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(
          "IsLoading", typeof(Boolean), typeof(FlowerLoading),
          new FrameworkPropertyMetadata(false, (d, e) =>
          {
              var view = d as FlowerLoading;
              if (view == null)
                  return;
              var value = (Boolean)e.NewValue;
              if (value)
                  view.OnBeginLoading();
              else
                  view.OnEndLoading();

          }));
        public bool IsLoading
        {
            get { return (Boolean)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }


        #endregion

        #region Route Events

        private static readonly RoutedEvent BeginLoadingEvent = EventManager.RegisterRoutedEvent(
          "BeginLoading", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(FlowerLoading));

        public event RoutedEventHandler BeginLoading
        {
            add { AddHandler(BeginLoadingEvent, value); }
            remove { RemoveHandler(BeginLoadingEvent, value); }
        }

        protected virtual void OnBeginLoading()
        {
            var e = new RoutedEventArgs(BeginLoadingEvent, this);
            RaiseEvent(e);
        }

        private static readonly RoutedEvent EndLoadingEvent = EventManager.RegisterRoutedEvent(
          "EndLoading", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(FlowerLoading));
        public event RoutedEventHandler EndLoading
        {
            add { AddHandler(EndLoadingEvent, value); }
            remove { RemoveHandler(EndLoadingEvent, value); }
        }

        protected virtual void OnEndLoading()
        {
            var e = new RoutedEventArgs(EndLoadingEvent, this);
            RaiseEvent(e);
        }

        #endregion
    }
}
