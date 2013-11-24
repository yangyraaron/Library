using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Library.Client.Wpf.Controls
{
    [TemplatePart(Name=PART_TOGGLE,Type=typeof(ToggleButton))]
    [TemplatePart(Name = PART_IMG, Type = typeof(Image))]
    [TemplatePart(Name = PART_TEXT, Type = typeof(TextBlock))]
    [TemplatePart(Name = PART_POP, Type = typeof(Popup))]
    public class InfoTip : Control
    {
        #region constants

        private const string PART_TOGGLE = "PART_TOGGLE";
        private const string PART_IMG = "PART_IMG";
        private const string PART_TEXT = "PART_TEXT";
        private const string PART_POP = "PART_POP";

        #endregion

        private ToggleButton _toggle;
        private Image _img;
        private TextBlock _txt;
        private Popup _pop;
        private FrameworkElement _topParent;

        static InfoTip()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InfoTip),
                            new FrameworkPropertyMetadata(typeof(InfoTip)));
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _toggle = this.GetTemplateChild(PART_TOGGLE) as ToggleButton;
            _img = this.GetTemplateChild(PART_IMG) as Image;
            _txt = this.GetTemplateChild(PART_TEXT) as TextBlock;
            _pop = this.GetTemplateChild(PART_POP) as Popup;

            _topParent = FindTopParent();

            if (_topParent != null)
            {
                _topParent.PreviewKeyUp += new System.Windows.Input.KeyEventHandler(_topParent_PreviewKeyUp);
                _topParent.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(_topParent_PreviewMouseUp);
            }

        }

        void _topParent_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_pop.IsMouseOver)
                return;

            if (_toggle.IsMouseOver)
                return;

            _toggle.IsChecked = false;
        }

        void _topParent_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            _toggle.IsChecked = false;
        }

        private FrameworkElement FindTopParent()
        {
            FrameworkElement iterator, nextUp = this;
            do
            {
                iterator = nextUp;
                nextUp = VisualTreeHelper.GetParent(iterator) as FrameworkElement;
            } while (nextUp != null);
            return iterator;
        }


        #region dependency properties

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", 
                                        typeof(string), 
                                        typeof(InfoTip), 
                                        new FrameworkPropertyMetadata(string.Empty));


        public ImageSource ShapeImaegSource
        {
            get { return (ImageSource)GetValue(ShapeImaegSourceProperty); }
            set { SetValue(ShapeImaegSourceProperty, value); }
        }

        public static readonly DependencyProperty ShapeImaegSourceProperty =
            DependencyProperty.Register("ShapeImaegSource", 
                                        typeof(ImageSource), 
                                        typeof(InfoTip), 
                                        new FrameworkPropertyMetadata(
                                            new BitmapImage(
                                                new Uri("pack://application:,,,/Library.Client.Wpf;component/Images/bg.png",
                                                    UriKind.RelativeOrAbsolute))));

        public double ShapeWidth
        {
            get { return (double)GetValue(ShapeWidthProperty); }
            set { SetValue(ShapeWidthProperty, value); }
        }

        public static readonly DependencyProperty ShapeWidthProperty =
            DependencyProperty.Register("ShapeWidth",
                                        typeof(double),
                                        typeof(InfoTip),
                                        new FrameworkPropertyMetadata((double)200));


        public double ShapeHeight
        {
            get { return (double)GetValue(ShapeHeightProperty); }
            set { SetValue(ShapeHeightProperty, value); }
        }

        public static readonly DependencyProperty ShapeHeightProperty =
            DependencyProperty.Register("ShapeHeight", 
                                        typeof(double), 
                                        typeof(InfoTip),
                                        new FrameworkPropertyMetadata((double)100));


        public static readonly DependencyProperty HorizontalOffsetProperty = Popup.HorizontalOffsetProperty.AddOwner(typeof(InfoTip));
        public double HorizontalOffset
        {
            get { return (double)this.GetValue(HorizontalOffsetProperty); }
            set { this.SetValue(HorizontalOffsetProperty, value); }
        }

        public static readonly DependencyProperty VerticalOffsetProperty = Popup.VerticalOffsetProperty.AddOwner(typeof(InfoTip));
        public double VerticalOffset
        {
            get { return (double)this.GetValue(VerticalOffsetProperty); }
            set { this.SetValue(VerticalOffsetProperty, value); }
        }



        public double ContentWidth
        {
            get { return (double)GetValue(ContentWidthProperty); }
            set { SetValue(ContentWidthProperty, value); }
        }

        public static readonly DependencyProperty ContentWidthProperty =
            DependencyProperty.Register("ContentWidth", 
                                        typeof(double), 
                                        typeof(InfoTip));



        public double ContentHeight
        {
            get { return (double)GetValue(ContentHeightProperty); }
            set { SetValue(ContentHeightProperty, value); }
        }

        public static readonly DependencyProperty ContentHeightProperty =
            DependencyProperty.Register("ContentHeight", typeof(double), typeof(InfoTip));


        public Thickness ContentMargin
        {
            get { return (Thickness)GetValue(ContentMarginProperty); }
            set { SetValue(ContentMarginProperty, value); }
        }

        public static readonly DependencyProperty ContentMarginProperty =
            DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(InfoTip), new UIPropertyMetadata(new Thickness(20,30,20,30)));

        

        
        
        #endregion

    }
}
