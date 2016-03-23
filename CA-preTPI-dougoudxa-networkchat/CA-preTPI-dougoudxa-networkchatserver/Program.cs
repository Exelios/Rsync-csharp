/// ETML
/// Author: Xavier Dougoud
/// Date: 21.03.2016
/// Summary: Network Chat Server part
/// This program has for only purpose to familiarize the programmer
/// with the notion of network programming


using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CA_preTPI_dougoudxa_networkchatserver
{
    class Program
    {
        private static Thread g_thListen;

        static void Main(string[] args)
        {
            //Preparing and starting the listening thread
            g_thListen = new Thread(new ThreadStart(Listen));
            g_thListen.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private static void Listen()
        {
            Console.WriteLine("Preparing to listen...");

            //Creating server and specifying the port he will be listening to.
            UdpClient udpServer = new UdpClient(4000);

            //Infinite loop that's purpose is to listen
            while (true)
            {
                //Creation of an IPEndPoint object that will recieve data from the remote socket
                IPEndPoint IPclient = null;
                Console.WriteLine("LISTENING...");

                //We listen until we recieve a message
                byte[] dataArray = udpServer.Receive(ref IPclient);
                Console.WriteLine("Data recieved from {0}:{1}", IPclient.Address, IPclient.Port);

                //Decyphering and reading of the message
                string strMessage = Encoding.Default.GetString(dataArray);
                Console.WriteLine("Message contents : {0}\n", strMessage);
            }
        }
    }
}
