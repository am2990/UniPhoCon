using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace PGControlv0._1
{
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void StartRemote_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RemoteCreate.xaml?ip="+IPAddress.Text, UriKind.Relative));
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


    }
}