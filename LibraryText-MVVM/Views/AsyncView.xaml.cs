using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibraryText_MVVM.ViewModels;
using System.ComponentModel.Composition;
using System.Windows.Controls.Primitives;

namespace LibraryText_MVVM.Views
{
    /// <summary>
    /// Interaction logic for AsyncView.xaml
    /// </summary>
    [Export]
    public partial class AsyncView : UserControl
    {
        //private Popup _popup;

        public AsyncView()
        {
            InitializeComponent();

            //_popup = new Popup();
            //_popup.Width = 200;
            //_popup.Height = 100;
            //_popup.AllowsTransparency = true;
            //_popup.StaysOpen = false;
            //_popup.Child = new InfoTip { Width=200, Height=100};
        }

        [Import]
        AsyncViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }

        //protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        //{
        //    base.OnMouseLeftButtonDown(e);

        //    _popup.PlacementTarget = e.Source as UIElement;
        //    _popup.Placement = PlacementMode.MousePoint;
        //    _popup.VerticalOffset = -100;
        //    _popup.HorizontalOffset = -200;
        //    _popup.IsOpen = true;
        //}

    }
}
