using IndoorLocationViewer.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace IndoorLocationViewer
{
    public delegate void EmptyHandler();

    public partial class MainWindow : Window
    {
        readonly MqttClient client;

        const int timeBeforeShowingInSeconds = 30;
        const string pillar2Mac = "24:62:ab:e0:9f:c8";
        const string pillar3Mac = "24:62:ab:e1:ad:c0";
        const string pillar1Mac = "24:62:ab:e0:e4:5c";

        private Dictionary<string, DetectedDevice> detectedDevicesDictionary = new Dictionary<string, DetectedDevice>();
        public ObservableCollection<DetectedDevice> DetectedDevices { get; } = new ObservableCollection<DetectedDevice>();


        public MainWindow()
        {
            InitializeComponent();

            client = new MqttClient("ds-ubuntu");
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            client.Connect("MyTestApp");
            client.Subscribe(new string[] { "/beacons/office" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string msgString = ASCIIEncoding.UTF8.GetString(e.Message);
            PillarMessage msg = JsonConvert.DeserializeObject<PillarMessage>(msgString);

            Dispatcher.BeginInvoke(new Func<Task>(async () =>
            {
                foreach (var entry in msg.Entries)
                {
                    DetectedDevice device;
                    if (detectedDevicesDictionary.ContainsKey(entry.Mac))
                    {
                        device = detectedDevicesDictionary[entry.Mac];

                        //If this device has been present for at lest 30 seconds, add it to the list of visible devices
                        if(device.Mac.ToLower() == "cb:18:8f:27:1d:76" && !DetectedDevices.Contains(device))
                        {
                            DetectedDevices.Add(device);
                        }
                        //if (device.FoundTime.AddSeconds(timeBeforeShowingInSeconds) > DateTime.Now && !DetectedDevices.Contains(device))
                        //{
                        //    DetectedDevices.Add(device);
                        //}
                    }
                    else
                    {
                        device = new DetectedDevice()
                        {
                            Mac = entry.Mac,
                            FoundTime = DateTime.Now
                        };
                        detectedDevicesDictionary.Add(entry.Mac, device);

                        //Kick off task to get the vendor name
                        Task t = DeviceHelper.LookupVendorByMac(entry.Mac).ContinueWith((tResult) =>
                        {
                            if (tResult.IsCompletedSuccessfully && !string.IsNullOrEmpty(tResult.Result))
                            {
                                Dispatcher.Invoke(() => device.VendorName = tResult.Result);
                            }
                        });
                    }

                    switch(msg.StationMac.ToLower())
                    {
                        case pillar1Mac:
                            device.Pillar1LastDetectedTime = DateTime.Now;
                            device.Pillar1Rssi = entry.RSSI;
                            break;
                        case pillar2Mac:
                            device.Pillar2LastDetectedTime = DateTime.Now;
                            device.Pillar2Rssi = entry.RSSI;
                            break;
                        case pillar3Mac:
                            device.Pillar3LastDetectedTime = DateTime.Now;
                            device.Pillar3Rssi = entry.RSSI;
                            break;
                    }
                }
            }));            
        }
    }
}
