using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NiceLife.Weather.Database
{
    public class RealtimeDetail
    {
        public long Id { get; set; }

        public long CountyId { get; set; }

        public string CountyName { get; set; }

        public string UpdateTime { get; set; }

        public string RealtimeTemp { get; set; }

        public string Humidity { get; set; }

        public string RealtimeWindDirection { get; set; }

        public string RealtimeWindPower { get; set; }

        public string Sunrise { get; set; }

        public string Sunset { get; set; }
    }

    public class RealtimeDetailViewModel : INotifyPropertyChanged
    {
        private RealtimeDetail realtimeDetail = new RealtimeDetail();

        public RealtimeDetail DefaultRealtimeDetail
        {
            get
            {
                return this.realtimeDetail;
            }
            set
            {
                this.realtimeDetail = value;
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
