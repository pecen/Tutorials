using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernVPN.MVVM.Model
{
    public class IPInfoModel
    {
        public string Ip { get; set; }
        public string Hostname { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string LatLong { get; set; } // 59.3294,18.0687
        public string Organization { get; set; } // AS3301 Telia Company AB
        public string PostalCode { get; set; } // 100 05
        public string TimeZone { get; set; } // "Europe/Stockholm\
        public string Readme { get; set; } // "https://ipinfo.io/missingauth
    }
}
