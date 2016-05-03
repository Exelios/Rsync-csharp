/// ETML
/// Author: Xavier Dougoud
/// Date: 18.04.2016

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace CA_preTPI_dougoudxa_rsyncsharp
{
    /// <summary>
    /// Contains all the common elements between server and client side of the application.
    /// </summary>
    class NetworkConfig
    {
        #region
        //////////////////////////////////////Class Attributes/////////////////////////////////////////////////

        /// <summary>
        /// Array of all the IP addresses on the current network, Used for the TCP connections for the file transfert.
        /// </summary>
        public static List<IPAddress> ipAddressList = new List<IPAddress>();

        /// <summary>
        /// 
        /// </summary>
        public static int ipAddressListSize = 0;

        /// <summary>
        /// 
        /// </summary>
        public static IPAddress myIPAddress;

        /// <summary>
        /// UDP client on the receiving end of the program for broadcast purposes.
        /// </summary>
        public static UdpClient udpClient = new UdpClient(4000);

        /// <summary>
        /// TCP client used for connections and file transfers.
        /// </summary>
        public static TcpClient tcpClient; 

        /// <summary>
        /// 
        /// </summary>
        public static Int64 bufferSize = 32768;

        /// <summary>
        /// 
        /// </summary>
        public static Thread listen;

        /// <summary>
        /// 
        /// </summary>
        public static Thread send;
        #endregion

        #region
        /////////////////////////////////Class methods///////////////////////////////////////////////////////

        /// <summary>
        /// Start the UDP part of the server for braodcasting and catching IP addresses
        /// </summary>
        public static void startUDPServer()
        {
            send = new Thread(broadcastMyIPAddress);

            send.Start();

            listen = new Thread(StartListening);

            listen.Start();
        }
        /*------------------------------------------------------------------------*/

        /// <summary>
        /// Stops the UDP server when application closes.
        /// </summary>
        public static void stopUDPServer()
        {
            send.Join();
            listen.Join();
        }
        /*----------------------------------------------------------------------*/

        /// <summary>
        /// Broadcasts the ip address of the current machine to all other machines in same subnet
        /// </summary>
        public static void broadcastMyIPAddress()
        {
            IPEndPoint broadcastIP = new IPEndPoint(IPAddress.Broadcast, 4000);

            //source http://stackoverflow.com/questions/6803073/get-local-ip-address //
            String hostName = string.Empty;                                          //
            // Getting Ip address of local machine...                                //
            // First get the host name of local machine.                             //
            hostName = Dns.GetHostName();                                            //    
            // Then using host name, get the IP address list..                       //
            IPHostEntry ipEntry = Dns.GetHostEntry(hostName);                        //
            IPAddress[] myAddress = ipEntry.AddressList;                             //
            /*************************************************************************/

            //For only ipv4 addresses
            //http://stackoverflow.com/questions/1059526/get-ipv4-addresses-from-dns-gethostentry
            IPAddress[] ipv4Addresses = Array.FindAll(Dns.GetHostEntry(string.Empty).AddressList, a => a.AddressFamily == AddressFamily.InterNetwork);

            myIPAddress = ipv4Addresses[0];

            byte[] messageArray = Encoding.ASCII.GetBytes(myIPAddress.ToString());

            while (!Program.getExitStatus())
            {
                udpClient.Send(messageArray, messageArray.Length, broadcastIP);

                Thread.Sleep(5000);
            }
        }
        /*------------------------------------------------------------------------------*/

        /// <summary>
        /// Method used by the listening thread
        /// </summary>
        public static void StartListening()
        {
            udpClient.BeginReceive(Receive, new object());
        }
        /*-----------------------------------------------------*/

        /// <summary>
        /// Method receiving incoming packets from any IP on the network
        /// </summary>
        /// <param name="result">Represents the status of an asynchronous operation</param>
        public static void Receive(IAsyncResult result)
        {
            IPEndPoint incomingIP = new IPEndPoint(IPAddress.Any, 4000);
            byte[] incomingData = udpClient.EndReceive(result, ref incomingIP);
            IPAddress tempIPAdress = IPAddress.Parse(Encoding.ASCII.GetString(incomingData));
            
            getNetworkAddresses(tempIPAdress);
                      
            StartListening();
        }
        /*-----------------------------------------------------*/

        /// <summary>
        /// Method collecting and storing all IP addresses sent by the program from other machines
        /// </summary>
        private static void getNetworkAddresses(IPAddress newIp)
        {
            if (newIp != myIPAddress)
            {
                for (int i = 0; i < ipAddressListSize; ++i)
                {
                    if (newIp != ipAddressList[i])
                    {
                        ipAddressList.Add(newIp);
                        ++ipAddressListSize;
                    }
                }
            }
        }
        /*--------------------------------------------------------------------------------------*/
        #endregion
    }
}
