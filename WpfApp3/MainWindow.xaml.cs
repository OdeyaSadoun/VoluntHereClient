using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Threading;
//using System.Device.Location;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int ID;
        string name;
        Task _task; 
        public MainWindow()
        {
            InitializeComponent();
            _task = Task.Run(() => {
                startListening();
            });
            
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void Help_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Help is on the way!");
            IPAddress ipAddr = new IPAddress(0x0100007F); //כתובת 
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 565);
            Socket server = new Socket(ipAddr.AddressFamily,
                   SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Connecting to server...");
            server.Connect(localEndPoint);
            Console.WriteLine("Connected");
            //Send message

            //location

            byte[] messageSent = Encoding.ASCII.GetBytes("HELP$1$2");
            int byteSent = server.Send(messageSent);
        }

        private void Button_Click_Register(object sender, RoutedEventArgs e)
        {
            (new REGISTER(UpdateDet)).Show();
        }

        private void handle_data(string data)
        {
            Dispatcher.Invoke(() => {
                string[] newdata = data.Split('$');
                if (newdata[0] == "Help")
                {
                    MessageBox.Show("Help needed");
                }
            });
        }

        private void startListening()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 600);

            try
            {

                // Create a Socket that will use Tcp protocol
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // A Socket must be associated with an endpoint using the Bind method
                listener.Bind(localEndPoint);
                // Specify how many requests a Socket can listen before it gives Server busy response.
                // We will listen 10 requests at a time
                listener.Listen(10);


                // Incoming data from the client.
                string data = null;
                byte[] bytes = null;

                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    Socket handler = listener.Accept();

                    // Incoming data from the client.
                    data = null;
                    bytes = null;
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    handle_data(data);
                }
            }
            catch { }

        }
            
        public static int GetRandFromServer()
        {
            IPAddress ipAddr = new IPAddress(0x0100007F); //כתובת 
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 565);
            Socket sender = new Socket(ipAddr.AddressFamily,
                   SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Connecting to server...");
            sender.Connect(localEndPoint);
            Console.WriteLine("Connected");
            //Send message

            //location

            byte[] messageSent = Encoding.ASCII.GetBytes("GET RAND");
            int byteSent = sender.Send(messageSent);


            // Data buffer
            byte[] messageReceived = new byte[1024];

            // We receive the message using
            // the method Receive(). This
            // method returns number of bytes
            // received, that we'll use to
            // convert them to string
            int byteRecv = sender.Receive(messageReceived);
            Console.WriteLine("Message from Server -> {0}",
                  Encoding.ASCII.GetString(messageReceived,
                                             0, byteRecv));
            int RandNum;
            int.TryParse(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), out RandNum);
            return RandNum;


//            private WithEvents watcher As GeoCoordinateWatcher
//            public Sub GetLocationDataEvent()
//            watcher = New System.Device.Location.GeoCoordinateWatcher()
//            AddHandler watcher.PositionChanged, AddressOf watcher_PositionChanged
//            watcher.Start()
//            End Sub

//Private Sub watcher_PositionChanged(ByVal sender As Object, ByVal e As GeoPositionChangedEventArgs(Of GeoCoordinate))
//    MsgBox(e.Position.Location.Latitude.ToString & ", " & _
//           e.Position.Location.Longitude.ToString)
//    ' Stop receiving updates after the first one.
//    watcher.Stop()
//End Sub
        }

        public void UpdateDet(int Id, string name)
        {
            ID = Id;
            Name = name;
            (new Thread(() => {
                IPAddress ipAddr = new IPAddress(0x0100007F); //כתובת 
                IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 565);
                Socket sender = new Socket(ipAddr.AddressFamily,
                       SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine("Connecting to server...");
                sender.Connect(localEndPoint);
                Console.WriteLine("Connected");
                //Send message

                //location
                string IP;
                string hostName;

                while (true)
                {
                    Thread.Sleep(3000);
                    hostName = Dns.GetHostName();
                    Console.WriteLine(hostName);
                    // Get the IP from GetHostByName method of dns class.
                    IP = Dns.GetHostByName(hostName).AddressList[0].ToString();
                    Console.WriteLine("IP Address is : " + IP);
                    String str = "UPD" + "$" + Id + "$" + name + "$" + IP + "$1$2";

                    byte[] messageSent = Encoding.ASCII.GetBytes(str);
                    try
                    {
                    int byteSent = sender.Send(messageSent);

                    }
                    catch
                    {

                    }
                }
            })).Start();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
