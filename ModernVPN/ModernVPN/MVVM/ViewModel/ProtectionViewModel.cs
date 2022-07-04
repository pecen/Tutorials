using ModernVPN.Core;
using ModernVPN.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ModernVPN.MVVM.ViewModel
{
    public class ProtectionViewModel : ObservableObject
    {
        public ObservableCollection<ServerModel> Servers { get; set; }

        private string _connectionStatus;
        public string ConnectionStatus
        {
            get { return _connectionStatus; }
            set
            {
                _connectionStatus = value;
                OnPropertyChanged();
            }
        }

        private string _connectionText;
        public string ConnectionText
        {
            get { return _connectionText; }
            set
            {
                _connectionText = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ConnectCommand { get; set; }

        public ProtectionViewModel()
        {
            ConnectionText = "Connect";

            Servers = new ObservableCollection<ServerModel>();
            for (int i = 0; i < 10; i++)
            {
                Servers.Add(new ServerModel
                {
                    Country = "USA"
                });
            }

            ConnectCommand = new RelayCommand(o => 
            {
                Task.Run(() =>
                {
                    ConnectionStatus = string.IsNullOrEmpty(ConnectionStatus) || ConnectionStatus == "Disconnected!" 
                        ? "Connecting..." 
                        : "Disconnecting...";
                
                    var process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;

                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    if (ConnectionText == "Disconnect")
                    {
                        process.StartInfo.ArgumentList.Add(@"/c rasdial /d");
                    }
                    else
                    {
                        process.StartInfo.ArgumentList.Add(@"/c rasdial MyServer vpnbook mzw6x8e /phonebook:./VPN/VPN.pbk");
                    }
                    
                    
                    process.Start();
                    process.WaitForExit();

                    switch (process.ExitCode)
                    {
                        case 0:
                            Debug.WriteLine("Success!");
                            ConnectionStatus = _connectionStatus == "Connecting..." ? "Connected!" : "Disconnected!";
                            ConnectionText = ConnectionText == "Connect" ? "Disconnect" : "Connect";
                            break;
                        case 691:
                            Debug.WriteLine("Wrong Credentials!");
                            ConnectionStatus = "Failed! Wrong Credentials. Try again.";
                            break;
                        default:
                            Console.WriteLine($"Error: {process.ExitCode}");
                            ConnectionStatus = $"Failed! Something went wrong. Error code: {process.ExitCode}";
                            break;
                    }
                });
            });
        }

        private void ServerBuilder()
        {
            var address = "us1.vpnbook.com";
            var folderPath = $"{Directory.GetCurrentDirectory()}/VPN";
            var pbkPath = $"{folderPath}/{address}.pbk";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (File.Exists(pbkPath))
            {
                MessageBox.Show("Connection already exists!");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine("[MyServer]");
            sb.AppendLine("MEDIA=rastapi");
            sb.AppendLine("Port=VPN2-0");
            sb.AppendLine("Device=WAN Miniport (IKEv2)");
            sb.AppendLine("DEVICE=vpn");
            sb.AppendLine($"PhoneNumber={address}");

            File.WriteAllText(pbkPath, sb.ToString());
        }
    }
}
