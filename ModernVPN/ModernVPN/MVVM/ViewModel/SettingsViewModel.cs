using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernVPN.MVVM.ViewModel
{
    public class SettingsViewModel
    {
        public GlobalViewModel Global { get; } = GlobalViewModel.Instance;

        public SettingsViewModel()
        {

        }
    }
}
