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
using Library.Test.Attributes;
using System.ComponentModel.Composition;

namespace Library.Test.Views
{
    /// <summary>
    /// Interaction logic for SignInView.xaml
    /// </summary>
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [ExportView(Key=ViewType.SignInView)]
    public partial class SignInView : View
    {
        public SignInView()
        {
            InitializeComponent();
        }
    }
}
