using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Port_Chat_Net
{
    internal class Program
    {
        static int RemotePort;
        static int LocalPort;
        static IPAddress RemoteIPAddr;
        static void Main(string[] args)
        {
            try
            {
                Console.SetWindowSize(40, 20); //IP/PC - 192.168.0.102 //Port1 - 8081 //Port2 - 8082.
                Console.Title = "Chat";
                Console.WriteLine("Enter remote IP");
                RemoteIPAddr = IPAddress.Parse(Console.ReadLine());
                Console.WriteLine("Enter remote port");
                RemotePort = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter local port");
                LocalPort = Convert.ToInt32(Console.ReadLine());
                Thread thread = new Thread(new ThreadStart(ThreadFuncReceive));
                thread.IsBackground = true;
                thread.Start();
                Console.ForegroundColor = ConsoleColor.Red;
                while (true)
                {
                    SendData(Console.ReadLine());
                }
            }
            catch (FormatException formExc)
            {
                Console.WriteLine("Conversion is impossible: " + formExc);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Error: " + exc.Message);
            }
        }
        static void ThreadFuncReceive()
        {
            try
            {
                while (true)
                {
                    UdpClient uClient = new UdpClient(LocalPort);
                    IPEndPoint ipEnd = null;
                    byte[] responce = uClient.Receive(ref ipEnd);
                    string strResult = Encoding.Unicode.GetString(responce);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(strResult);
                    Console.ForegroundColor = ConsoleColor.Red;
                    uClient.Close();
                }
            }
            catch (SocketException sockEx)
            {
                Console.WriteLine("Socket error: " + sockEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        static void SendData(string datagramm)
        {
            UdpClient uClient = new UdpClient();
            IPEndPoint ipEnd = new IPEndPoint(RemoteIPAddr, RemotePort);
            try
            {
                byte[] bytes = Encoding.Unicode.GetBytes(datagramm);
                uClient.Send(bytes, bytes.Length, ipEnd);
            }
            catch (SocketException sockEx)
            {
                Console.WriteLine("Socket error: " + sockEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                uClient.Close();
            }
        }
    }
}
