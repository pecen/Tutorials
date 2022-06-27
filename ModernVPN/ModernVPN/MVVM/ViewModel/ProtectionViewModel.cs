using ModernVPN.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernVPN.MVVM.ViewModel
{
    public class ProtectionViewModel
    {
        public ObservableCollection<ServerModel> Servers { get; set; }
        
        public ProtectionViewModel()
        {
            Servers = new ObservableCollection<ServerModel>();
            for (int i = 0; i < 10; i++)
            {
                Servers.Add(new ServerModel
                {
                    Country = "USA"
                });
            }
        }
    }
}
