using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Library.Client.Wpf.Controls
{
    [TemplatePart(Name = TRANSFORM, Type = typeof(RotateTransform))]
    public class SnakeLoading : Control
    {
        #region Constants

        private const string TRANSFORM = "PART_SpinnerRotate";

        #endregion

        #region Fields

        private RotateTransform _transform;

        #endregion

        #region Constructors

        public SnakeLoading()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SnakeLoading),
            new FrameworkPropertyMetadata(typeof(SnakeLoading)));
        }

        #endregion

        #region Overrid Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _transform = GetTemplateChild(TRANSFORM) as RotateTransform;

        }

        #endregion

        #region DendencyProperties

        private static readonly DependencyProperty IsLoadingProperty =
       DependencyProperty.Register("IsLoading", typeof(Boolean), typeof(SnakeLoading));
        public bool IsLoading
        {
            get { return (Boolean)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        private static readonly DependencyProperty TransformAngleProperty =
          DependencyProperty.Register("TransformAngle", typeof(double), typeof(SnakeLoading),
          new FrameworkPropertyMetadata((d, e) =>
          {
              var view = d as SnakeLoading;
              if (view == null)
                  return;

              view._transform.Angle = (double)e.NewValue;
          }));
        public double TransformAngle
        {
            get { return (double)GetValue(TransformAngleProperty); }
        }

        #endregion
    }
}
