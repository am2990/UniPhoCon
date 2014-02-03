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
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Devices.Sensors;
using System.Windows.Shapes;



namespace PGControlv0._1
{
    public partial class MainPage : PhoneApplicationPage
    {
        Motion motion;
        const int ECHO_PORT = 9906;
        SocketClient client;
        String IPAddr;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Check to see if the Motion API is supported on the device.
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
                if (parameters.ContainsKey("IPAddress")) {
                    IPAddr = parameters["IPAddress"]; 
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
                    string text=MathHelper.ToDegrees(e.Attitude.Roll).ToString("0")+":"+
                             MathHelper.ToDegrees(e.Attitude.Pitch).ToString("0") + ":"
                             + e.DeviceAcceleration.X.ToString("0.00")+":"
                             + e.DeviceAcceleration.Y.ToString("0.00");
                    
                    // Attempt to send our message to be echoed to the echo server
    
                    string result = client.Send(IPAddr, ECHO_PORT, text);
                    
            }
        }
    }

}