using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogsInMvvm
{
    public class MainWindowViewModel : BindableBase
    {
        IDialogService _dialogService = new DialogService();

        private DelegateCommand _showDialog;

        public DelegateCommand ShowDialog =>
            _showDialog ?? (_showDialog = new DelegateCommand(ExecuteShowDialog));

        private void ExecuteShowDialog()
        {
            _dialogService.ShowDialog("Notification", result =>
            {
                var test = result;
            });
        }
    }
}
