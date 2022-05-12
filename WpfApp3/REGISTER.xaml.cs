using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for REGISTER.xaml
    /// </summary>
    public partial class REGISTER : Window
    {
        public string Name { get; set; }
        int ID;
        Action<int, string> Func;
        public REGISTER(Action<int, string> func)
        {
            InitializeComponent();
            Func = func;
        }

        private void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            Name = name_tb.Text;
            IPAddress ipAddr = new IPAddress(0x0100007F); //כתובת 
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 565);
            Socket server = new Socket(ipAddr.AddressFamily,
                   SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Connecting to server...");
            server.Connect(localEndPoint);
            Console.WriteLine("Connected");
            //Send message


            string hostName = Dns.GetHostName();
            Console.WriteLine(hostName);

            // Get the IP from GetHostByName method of dns class.
            string IP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            Console.WriteLine("IP Address is : " + IP);

            String str = "REG" + "$" + Name + "$" + IP + "$1$2";

            //location

            byte[] messageSent = Encoding.ASCII.GetBytes(str);
            int byteSent = server.Send(messageSent);

            byte[] messageReceived = new byte[1024];

            // We receive the message using
            // the method Receive(). This
            // method returns number of bytes
            // received, that we'll use to
            // convert them to string
            int byteRecv = server.Receive(messageReceived);
            Console.WriteLine("Message from Server -> {0}",
                  Encoding.ASCII.GetString(messageReceived,
                                             0, byteRecv));
           
            int.TryParse(Encoding.ASCII.GetString(messageReceived, 0, byteRecv), out ID);
            this.Close();
            Func(ID, Name);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
