using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DialogsInMvvm
{
    public interface IDialogService
    {
        void ShowDialog(string name, Action<string> callback);
        void ShowDialog<TViewModel>(Action<string> callback);
    }

    public class DialogService : IDialogService
    {
        static Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>();

        public static void RegisterDialog<TView, TViewModel>()
        {
            _mappings.Add(typeof(TViewModel), typeof(TView));
        }

        public void ShowDialog(string name, Action<string> callback)
        {
            var type = Type.GetType($"DialogsInMvvm.{name}");

            ShowDialogInternal(type, callback);
        }

        public void ShowDialog<TViewModel>(Action<string> callback)
        {
            var type = _mappings[typeof(TViewModel)];

            ShowDialogInternal(type, callback);
        }

        private static void ShowDialogInternal(Type type, Action<string> callback)
        {
            var dialog = new DialogWindow();

            EventHandler closeEventHandler = null;
            closeEventHandler = (s, e) =>
            {
                callback(dialog.DialogResult.ToString());
                dialog.Closed -= closeEventHandler;
            };

            dialog.Closed += closeEventHandler;

            //var type = Type.GetType($"DialogsInMvvm.{name}");

            dialog.Content = Activator.CreateInstance(type);

            dialog.ShowDialog();
        }
    }
}
