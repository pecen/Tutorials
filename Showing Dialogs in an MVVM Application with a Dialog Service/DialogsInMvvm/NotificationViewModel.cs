using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DialogsInMvvm
{
    public class NotificationViewModel : BindableBase
    {
        public DelegateCommand<System.Windows.Data.RelativeSource> CloseDialogCommand { get; private set; }

        public NotificationViewModel()
        {
            CloseDialogCommand = new DelegateCommand<System.Windows.Data.RelativeSource>(CloseDialog);
        }

        private void CloseDialog(System.Windows.Data.RelativeSource parent)
        {
            //var window = Activator.CreateInstance(parent.AncestorType) as Window;
            //window.DialogResult = true;

            throw new NotImplementedException();
        }
    }
}
