using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Devices.Sensors;

namespace PGControlv0._1
{
    public partial class Page2 : PhoneApplicationPage
    {

        Motion motion;
        const int ECHO_PORT = 9908;
        SocketClient client;
        String IPAddr = "192.168.1.2";
        Remote remote;
        public Page2()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Remote_Loaded);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Check to see if the Motion API is supported on the device.
            Touch.FrameReported += new TouchFrameEventHandler(Touch_FrameReported);
            IDictionary<string, string> par = NavigationContext.QueryString;
            if (par.ContainsKey("id"))
            {
                remote = RemoteReader.LoadRemote(int.Parse(par["id"]));
            }
            if (!Motion.IsSupported)
            {
                MessageBox.Show("the Motion API is not supported on this device.");
                return;
            }

            // If the Motion object is null, initialize it and add a CurrentValueChanged
            // event handler.
            if (motion == null)
            {
                motion = new Motion();
                IDictionary<string, string> parameters = NavigationContext.QueryString;
                if (parameters.ContainsKey("ip"))
                {
                    if (parameters["ip"] != "s")
                    {
                        //  IPAddr = parameters["ip"];
                    }
                }
                // motion.TimeBetweenUpdates = TimeSpan.FromMilliseconds(0);
                motion.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<MotionReading>>(motion_CurrentValueChanged);
                client = new SocketClient();
            }

            // Try to start the Motion API.
            try
            {
                motion.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("unable to start the Motion API.");
            }
        }

        void Remote_Loaded(object sender, RoutedEventArgs e)
        {
            if (remote == null)
            {
                remote = new Remote(48, 24, "testRemote");
                RemoteButton rb1 = new RemoteButton("88", "X", 120, 4, 2, 20, 20);
                remote.addButton(rb1);
                rb1 = new RemoteButton("90", "Z", 122, 27, 2, 44, 20);
                remote.addButton(rb1);
                remote.setReference();
                CreateControls(remote);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("yaayyyyyyyyyy\n");
                remote.setReference();
                CreateControls(remote);
            }
            //    attachEvents(remote);
        }


        private void CreateControls(Remote remote)
        {
            System.Diagnostics.Debug.WriteLine("Remote Creation!");
            // <Grid Margin="10">
            Grid rootGrid = new Grid();
            //rootGrid.Margin = new Thickness(0);

            // <Grid.ColumnDefinitions>
            //   <ColumnDefinition Width="100" />
            //   <ColumnDefinition Width="*" />
            //</Grid.ColumnDefinitions>
            double colWidth = System.Windows.Application.Current.Host.Content.ActualWidth / remote.column;
            double rowHeight = System.Windows.Application.Current.Host.Content.ActualHeight / remote.row;
            for (int i = 0; i < remote.column; i++)
            {
                rootGrid.ColumnDefinitions.Add(
                    new ColumnDefinition() { Width = new GridLength(colWidth) });
                //rootGrid.ColumnDefinitions.Add(
                //    new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }

            //<Grid.RowDefinitions>
            //  <RowDefinition Height="Auto" />
            //  <RowDefinition Height="Auto" />
            //  <RowDefinition Height="Auto" />
            //  <RowDefinition Height="*" />
            //</Grid.RowDefinitions>

            for (int i = 0; i < remote.row; i++)
            {
                rootGrid.RowDefinitions.Add(
                new RowDefinition() { Height = new GridLength(rowHeight) });
            }


            //<Button x:Name="SubmitChanges"
            //        Grid.Row="3"
            //        Grid.Column="3"
            //        HorizontalAlignment="Right"
            //        VerticalAlignment="Top"
            //        Margin="3"
            //        Width="80"
            //        Height="25"
            //        Content="Save" />

            foreach (RemoteButton rb in remote.rbArray)
            {
                System.Windows.Shapes.Rectangle button = new System.Windows.Shapes.Rectangle();
                button.Name = rb.name;
                button.HorizontalAlignment = HorizontalAlignment.Left;
                button.VerticalAlignment = VerticalAlignment.Top;
                button.Margin = new Thickness(0);
                Grid.SetColumnSpan(button, (int)((rb.bottomRightColumn - rb.topLeftColumn) )); //*colWidth
                Grid.SetRowSpan(button, (int)((rb.bottomRightRow - rb.topLeftRow)));//* rowHeight
                button.Width = (rb.bottomRightColumn - rb.topLeftColumn) * colWidth;
                button.Height = (rb.bottomRightRow - rb.topLeftRow) * rowHeight;
                button.Fill = new SolidColorBrush(Colors.Green);
                button.UpdateLayout();
                Grid.SetRow(button, rb.topLeftRow);
                Grid.SetColumn(button, rb.topLeftColumn);
                rootGrid.Children.Add(button);
            }



            LayoutRoot.Children.Add(rootGrid);
        }

        private void attachEvents(Remote remote)
        {

            foreach (RemoteButton rb in remote.rbArray)
            {
                var btn = (Button)FindName(rb.name);

                // Add a Button Click Event handler
                if (btn != null)
                {
                    btn.ClickMode = ClickMode.Press;
                    //btn.Click += new RoutedEventHandler(DynamicButton_Click);

                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(rb.name + " Nulla Saala!");
                }
            }

        }

        private void DynamicButton_Click(object sender, EventArgs e)
        {
            System.Windows.Shapes.Rectangle button_clicked = sender as System.Windows.Shapes.Rectangle;
            RemoteButton rButton = remote.rbArray[remote.buttonRefer[int.Parse(button_clicked.Name)]];

            if (!rButton.clicked)
            {
                rButton.clicked = true;
                button_clicked.Fill = new SolidColorBrush(Colors.Blue);
                //button_clicked.ClickMode = ClickMode.Release;
                //button_clicked.Content = "Pressed";
                string text = "1:" + button_clicked.Name;
                //client.Send(IPAddr, ECHO_PORT, text);
            }

            else if (rButton.clicked)
            {
                rButton.clicked = false;
                button_clicked.Fill = new SolidColorBrush(Colors.Green);
                //button_clicked.ClickMode = ClickMode.Press;
                //button_clicked.Content = "Released";
                string text = "0:" + button_clicked.Name;
                //client.Send(IPAddr, ECHO_PORT, text);
            }
        }

        void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Touched");
            foreach (RemoteButton rb in remote.rbArray)
            {
                //System.Diagnostics.Debug.WriteLine("Searched button " + rb.name);
                var btn = (System.Windows.Shapes.Rectangle)FindName(rb.name);
                // Add a Button Click Event handler
                if (btn != null)
                {
                    //System.Diagnostics.Debug.WriteLine("Found button " + rb.name);
                    int pointsNumber = e.GetTouchPoints(btn).Count;
                    TouchPointCollection pointCollection = e.GetTouchPoints(btn);
                    for (int i = 0; i < pointsNumber; i++)
                    {
                        if (pointCollection[i].Action == TouchAction.Down || pointCollection[i].Action == TouchAction.Up)
                        {
                            if (pointCollection[i].Position.Y < btn.ActualHeight && pointCollection[i].Position.X < btn.ActualWidth
                                 && pointCollection[i].Position.Y > 0 && pointCollection[i].Position.X > 0)
                            {
                                DynamicButton_Click(btn, null);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(rb.name + " Nulla Saala!");
                }
            }

        }


        void motion_CurrentValueChanged(object sender, SensorReadingEventArgs<MotionReading> e)
        {
            // This event arrives on a background thread. Use BeginInvoke to call
            // CurrentValueChanged on the UI thread.
            Dispatcher.BeginInvoke(() => CurrentValueChanged(e.SensorReading));
        }
        private void CurrentValueChanged(MotionReading e)
        {
            if (motion.IsDataValid)
            {
                string text = MathHelper.ToDegrees(e.Attitude.Roll).ToString("0") + ":" +
                         MathHelper.ToDegrees(e.Attitude.Pitch).ToString("0") + ":"
                         + e.DeviceAcceleration.X.ToString("0.00") + ":"
                         + e.DeviceAcceleration.Y.ToString("0.00");

                // Attempt to send our message to be echoed to the echo server

                string result = client.Send(IPAddr, ECHO_PORT, text);

            }
        }


    }
}