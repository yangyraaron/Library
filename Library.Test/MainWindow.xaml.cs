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
using Library.Client.Wpf;
using Library.Shared.Client.Interfaces;
using Library.Shared;

namespace Library.Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,IUIManager
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        #region IUIManager Members

        public void SetContent(object content)
        {
            this.Content = content;
        }
        #endregion
    }
}
