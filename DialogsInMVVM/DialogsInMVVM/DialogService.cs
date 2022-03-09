using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogsInMVVM
{
    public interface IDialogService
    {
        void ShowDialog();
    }
    public class DialogService : IDialogService
    {
        public void ShowDialog()
        {
            var dialog = new DialogWindow();

            dialog.ShowDialog();
        }
    }
}
