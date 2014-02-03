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
using System.IO;
using System.IO.IsolatedStorage;


namespace PGControlv0._1
{
    public partial class RemoteList : PhoneApplicationPage
    {
        public RemoteList()
        {
            InitializeComponent();
        }

        private void add_Remote(object sender, EventArgs e)
        {
            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
            
        }

    }
}