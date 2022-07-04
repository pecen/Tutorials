using ModernVPN.Core;
using ModernVPN.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ModernVPN.MVVM.ViewModel
{
    public class ProtectionViewModel : ObservableObject
    {
        public ObservableCollection<ServerModel> Servers { get; set; }

        public GlobalViewModel Global { get; } = GlobalViewModel.Instance;

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

        private string _externalIp;
        public string ExternalIp
        {
            get { return _externalIp; }
            set
            {
                _externalIp = value;
                OnPropertyChanged();
            }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set 
            { 
                _location = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand ConnectCommand { get; set; }

        public ProtectionViewModel()
        {
            var isConnected = CheckConnectionStatus();
            var externalIpInfo = GetExternalIPInfo();

            ConnectionStatus = isConnected ? "Connected!" : "Disconnected!";
            ConnectionText = CheckConnectionStatus() ? "Disconnect" : "Connect";
            ExternalIp = externalIpInfo.Ip;
            Location = $"Country: {externalIpInfo.Country}, City: {externalIpInfo.City}";


            //var localIp = GetLocalIPAddress();
            //var ip = GetIPAddress();
            //var vpnLocalIp = GetLocalVPNAddress();

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
                    //ConnectionStatus = string.IsNullOrEmpty(ConnectionStatus) || ConnectionStatus == "Disconnected!" 
                    ConnectionStatus = ConnectionText == "Connect"
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
                            externalIpInfo = GetExternalIPInfo();
                            ExternalIp = externalIpInfo.Ip;
                            Location = $"Country: {externalIpInfo.Country}, City: {externalIpInfo.City}";
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

        private bool CheckConnectionStatus()
        {
            var process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.WorkingDirectory = Environment.CurrentDirectory;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.StartInfo.ArgumentList.Add(@"/c rasdial MyServer");
            process.Start();
            process.WaitForExit();
            process.StartInfo.ArgumentList.Remove(@"/c rasdial MyServer");

            return process.ExitCode == 0;
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private string GetIPAddress()
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP; 
        }

        private string GetLocalVPNAddress()
        {
            var vpn = NetworkInterface.GetAllNetworkInterfaces()
                                      .FirstOrDefault(x => x.Name == "MyServer");

            var ip = vpn?.GetIPProperties().UnicastAddresses.First(x => x.Address.AddressFamily == AddressFamily.InterNetwork).Address.ToString();

            return ip;
        }

        private IPInfoModel GetExternalIPInfo()
        {
            //string externalIp;
            string extIpInfo;
            string url = "http://icanhazip.com";
            string url2 = "https://ipinfo.io/";
            IPInfoModel ipInfoModel = new IPInfoModel();

            try
            {
                HttpClient client = new HttpClient();

                using (HttpResponseMessage response = client.GetAsync(url2).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        extIpInfo = content.ReadAsStringAsync().Result;
                    }
                }

                ipInfoModel = Deserialize<IPInfoModel>(extIpInfo);


                //string externalIpString = new WebClient().DownloadString(url).Replace("\\r\\n", "").Replace("\\n", "").Trim();
                //externalIp = IPAddress.Parse(externalIpString).ToString();
            }
            catch(Exception ex)
            {
                extIpInfo = $"Failed to get IP-address: {ex.Message}";
            }

            return ipInfoModel;
        }

        private const bool DefaultCaseInsensitive = true;

        /// <summary>
        /// JSON Deserialization of a given json string.
        /// </summary>
        /// <param name="json">The json string to be deserialize into object.</param>
        /// <param name="options">The options to be used for deserialization.</param>
        /// <returns>The deserialized object.</returns>
        //internal static T Deserialize<T>(string json, JsonSerializerOptions options = null)
        private T Deserialize<T>(string json, JsonSerializerOptions options = null)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            if (options is null)
            {
                options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = DefaultCaseInsensitive
                };
            }

            return JsonSerializer.Deserialize<T>(json, options);
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
