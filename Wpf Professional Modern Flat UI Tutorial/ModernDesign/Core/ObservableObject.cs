using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ModernDesign.Core {
  public class ObservableObject : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged([CallerMemberName] string name = null) {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
  }
}
