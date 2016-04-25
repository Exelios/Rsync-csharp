/// ETML
/// Author: Xavier Dougoud
/// Date: 18.04.2016

using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA_preTPI_dougoudxa_rsyncsharp
{
    /// <summary>
    /// Contains all the common elements between server and client side of the application.
    /// </summary>
    class NetworkConfig
    {
        //////////////////////////////////////Class Attributes/////////////////////////////////////////////////

        /// <summary>
        /// Array of all the IP addresses on the current network, Used for the TCP connections for the file transfert.
        /// </summary>
        public static IPAddress[] networkAddressesArray = new IPAddress[50];

        /// <summary>
        /// Variable storing temporarly an IP address.
        /// </summary>
        private static IPAddress tempIPAdress;

        /// <summary>
        /// UDP client on the receiving end of the program for broadcast purposes.
        /// </summary>
        public static UdpClient udpClient = new UdpClient(4000);

        /// <summary>
        /// TCP client used for connections and file transfers.
        /// </summary>
        public static TcpClient tcpClient = new TcpClient();


        /// <summary>
        /// 
        /// </summary>
        public static Int64 bufferSize = 32768;


        /////////////////////////////////Class methods///////////////////////////////////////////////////////

        /// <summary>
        /// Broadcasts the ip address of the current machine to all other machines in same subnet
        /// </summary>
        public static void broadcastMyIPAddress()
        {
            IPEndPoint broadcastIP = new IPEndPoint(IPAddress.Broadcast, 4000);

            //source http://stackoverflow.com/questions/6803073/get-local-ip-address /////
            String strHostName = string.Empty;                                          //
            // Getting Ip address of local machine...                                   //
            // First get the host name of local machine.                                //
            strHostName = Dns.GetHostName();                                            //    
            // Then using host name, get the IP address list..                          //
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);                        //
            IPAddress[] myAddress = ipEntry.AddressList;                                //
            /*--------------------------------------------------------------------------*/    

            byte[] messageArray = Encoding.ASCII.GetBytes(myAddress[0].ToString());

            udpClient.Send(messageArray, messageArray.Length, broadcastIP);
        }
        /*------------------------------------------------------------------------------*/

        /// <summary>
        /// Method used by the listening thread
        /// </summary>
        public void StartListening()
        {
            udpClient.BeginReceive(Receive, new object());
        }
        /*-----------------------------------------------------*/

        /// <summary>
        /// Method receiving incoming packets from any IP on the network
        /// </summary>
        /// <param name="result">Represents the status of an asynchronous operation</param>
        public void Receive(IAsyncResult result)
        {
            IPEndPoint incomingIP = new IPEndPoint(IPAddress.Any, 4000);
            byte[] incomingData = udpClient.EndReceive(result, ref incomingIP);
            tempIPAdress = IPAddress.Parse(Encoding.ASCII.GetString(incomingData));
            getNetworkAddresses(tempIPAdress);
            StartListening();
        }
        /*-----------------------------------------------------*/

        /// <summary>
        /// Method collecting and storing all IP addresses sent by the program from other machines
        /// </summary>
        public void getNetworkAddresses(IPAddress ip)
        {
            int arrayIndex = 0;

            while(arrayIndex < networkAddressesArray.Length)
            {
                ++arrayIndex;
                if(networkAddressesArray[arrayIndex] == null)
                {
                    networkAddressesArray[arrayIndex] = ip;
                    break;
                }
            }
        }
        /*--------------------------------------------------------------------------------------*/
    }
}
