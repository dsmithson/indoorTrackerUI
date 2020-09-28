using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IndoorLocationViewer.ViewModel
{
    public class DetectedDevice : INotifyPropertyChanged
    {
        private DateTime foundTime;
        public DateTime FoundTime
        {
            get { return foundTime; }
            set
            {
                if(foundTime != value)
                {
                    foundTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private string mac;
        public string Mac
        {
            get { return mac; }
            set
            {
                if(mac != value)
                {
                    mac = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime pillar1LastDetectedTime;
        public DateTime Pillar1LastDetectedTime
        {
            get { return pillar1LastDetectedTime; }
            set
            {
                if(pillar1LastDetectedTime != value)
                {
                    pillar1LastDetectedTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private string pillar1Rssi;
        public string Pillar1Rssi
        {
            get { return pillar1Rssi; }
            set
            {
                if (pillar1Rssi != value)
                {
                    pillar1Rssi = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime pillar2LastDetectedTime;
        public DateTime Pillar2LastDetectedTime
        {
            get { return pillar2LastDetectedTime; }
            set
            {
                if (pillar2LastDetectedTime != value)
                {
                    pillar2LastDetectedTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private string pillar2Rssi;
        public string Pillar2Rssi
        {
            get { return pillar2Rssi; }
            set
            {
                if (pillar2Rssi != value)
                {
                    pillar2Rssi = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime pillar3LastDetectedTime;
        public DateTime Pillar3LastDetectedTime
        {
            get { return pillar3LastDetectedTime; }
            set
            {
                if (pillar3LastDetectedTime != value)
                {
                    pillar3LastDetectedTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private string pillar3Rssi;
        public string Pillar3Rssi
        {
            get { return pillar3Rssi; }
            set
            {
                if (pillar3Rssi != value)
                {
                    pillar3Rssi = value;
                    OnPropertyChanged();
                }
            }
        }

        private string vendorName;
        public string VendorName
        {
            get { return vendorName; }
            set
            {
                if(vendorName != value)
                {
                    vendorName = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
