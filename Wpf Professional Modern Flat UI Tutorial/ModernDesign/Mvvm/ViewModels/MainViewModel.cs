using ModernDesign.Core;

namespace ModernDesign.Mvvm.ViewModels {
  public class MainViewModel : ObservableObject {
    public MainViewModel() {
      HomeVM = new HomeViewModel();
      DiscoveryVM = new DiscoveryViewModel();

      CurrentView = HomeVM;

      HomeViewCommand = new RelayCommand(o => {
        CurrentView = HomeVM;
      });

      DiscoveryViewCommand = new RelayCommand(o => {
        CurrentView = DiscoveryVM;
      });
    }

    public HomeViewModel HomeVM { get; set; }
    public DiscoveryViewModel DiscoveryVM { get; set; }

    public RelayCommand HomeViewCommand { get; set; }
    public RelayCommand DiscoveryViewCommand { get; set; }

    private object _currentView;
    public object CurrentView {
      get { return _currentView; }
      set { 
        _currentView = value;
        RaisePropertyChanged();
      }
    }

    
  }
}
