using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Weather.Database
{
    public class RealtimeDetail : INotifyPropertyChanged
    {
        public long Id
        {
            get
            {
                return Id;
            }
            set
            {
                Id = value;
                this.OnPropertyChanged();
            }
        }

        public long CountyId
        {
            get
            {
                return CountyId;
            }
            set
            {
                CountyId = value;
                this.OnPropertyChanged();
            }
        }

        public string CountyName
        {
            get
            {
                return CountyName;
            }
            set
            {
                CountyName = value;
                this.OnPropertyChanged();
            }
        }

        public string UpdateTime
        {
            get
            {
                return UpdateTime;
            }
            set
            {
                UpdateTime = value;
                this.OnPropertyChanged();
            }
        }

        public string RealtimeTemp
        {
            get
            {
                return RealtimeTemp;
            }
            set
            {
                RealtimeTemp = value;
                this.OnPropertyChanged();
            }
        }

        public string RealtimeWindDirection
        {
            get
            {
                return RealtimeWindDirection;
            }
            set
            {
                RealtimeWindDirection = value;
                this.OnPropertyChanged();
            }
        }

        public string RealtimeWindPower
        {
            get
            {
                return RealtimeWindPower;
            }
            set
            {
                RealtimeWindPower = value;
                this.OnPropertyChanged();
            }
        }

        public string Sunrise
        {
            get
            {
                return Sunrise;
            }
            set
            {
                Sunrise = value;
                this.OnPropertyChanged();
            }
        }

        public string Sunset
        {
            get
            {
                return Sunset;
            }
            set
            {
                Sunset = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
