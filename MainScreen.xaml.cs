using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;

namespace PGControlv0._1
{
    public partial class MainScreen : PhoneApplicationPage
    {
        public MainScreen()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(SetupPage);
        }

        void SetupPage(object sender, RoutedEventArgs e)
        {
            Panorama panorama = (Panorama)FindName("panorama");
            panorama.Title = AppConstants.appTitle;
            PanoramaItem page = (PanoramaItem)FindName("page1");
            page.Header = AppConstants.mainPage1;
            page = (PanoramaItem)FindName("page2");
            page.Header = AppConstants.mainPage2;
            page = (PanoramaItem)FindName("page3");
            page.Header = AppConstants.mainPage3;
            initialize();
            ListBox remoteList = (ListBox)FindName("remoteList");
            remoteList.ItemsSource = RemoteReader.LoadData();
        }

        private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/RemoteList.xaml?ip=", UriKind.Relative));
        }



        //function to set the first remote if first time install

        private void Tap_LeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StackPanel rect = sender as StackPanel;
            // Change the size of the Rectangle.
            if (null != rect)
            {
                rect.Opacity = 0.5;


            }
        }
        private void Tap_LeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StackPanel rect = sender as StackPanel;
            // Reset the dimensions on the Rectangle.
            if (null != rect)
            {
                rect.Opacity = 1.0;

            }
        }

        private void Tap_MouseLeave(object sender, MouseEventArgs e)
        {
            StackPanel rect = sender as StackPanel;
            // Finger moved out of Rectangle before the mouse up event.
            // Reset the dimensions on the Rectangle.


            if (null != rect)
            {
                rect.Opacity = 1.0;

            }
        }

        private void remoteList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                // get RSS item from ListBox and store in class var. Store page to navigate to in class var.              
                Remote selectedItem = (Remote)e.AddedItems[0];
                Uri targetPage = new Uri("/RemotePage.xaml?id="+selectedItem.id, UriKind.Relative);
                // reset selection of ListBox 
                ((ListBox)sender).SelectedIndex = -1;
                // change page navigation 
                NavigationService.Navigate(targetPage);
                //FrameworkElement root = Application.Current.RootVisual as FrameworkElement;
                //root.DataContext = selectedItem;
            }
        }


        private void initialize()
        {
            IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
            if (!appSettings.Contains("initialized"))
            {
                //appSettings.Add("initialized", true);
                //appSettings.Add("LastRemoteId", 1);
                IsolatedStorageFile appStore = IsolatedStorageFile.GetUserStoreForApplication();
                // Create a new folder and call it "MyFolder".
                //appStore.CreateDirectory("RemoteFolder");
                Remote remote = new Remote(48, 24, "testRemote");
                remote.id = 1;
                RemoteButton rb1 = new RemoteButton("88", "X", 120, 4, 2, 20, 20);
                remote.addButton(rb1);
                rb1 = new RemoteButton("90", "Z", 122, 27, 2, 35, 12);
                remote.addButton(rb1);
                //remote.setReference();
                string json = JsonConvert.SerializeObject(remote);
                System.Diagnostics.Debug.WriteLine("json------------\n" + json);
                // Specify the file path and options.
                using (var isoFileStream = new IsolatedStorageFileStream("Remotes.txt", FileMode.OpenOrCreate, appStore))
                {
                    //Write the data
                    using (var isoFileWriter = new StreamWriter(isoFileStream))
                    {
                        isoFileWriter.WriteLine(json);
                    }
                }
            }
        }


    }

}