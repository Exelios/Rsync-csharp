/// ETML
/// Author: Xavier Dougoud
/// Date: 21.03.2016
/// Summary: Network Chat Client part
/// This program has for only purpose to familiarize the programmer
/// with the notion of network programming


using System;
using System.Net.Sockets;
using System.Text;

namespace CA_preTPI_dougoudxa_networkchat
{
    class Program
    {
        static void Main(string[] args)
        {
            bool booContinue = true;

            while (booContinue)
            {
                Console.Write("\nType in a message : ");
                string message = Console.ReadLine();

                //Encoding the message into an array of bytes
                byte[] encodedMessageArray = Encoding.Default.GetBytes(message);

                UdpClient udpClient = new UdpClient();

                //Send method sends an UDP message
                udpClient.Send(encodedMessageArray, encodedMessageArray.Length, "192.168.10.2", 4000);

                udpClient.Close();

                Console.Write("\nContinue ? (Y/N) ");

                //Interesting way of affecting a boolean value.
                booContinue = (Console.ReadKey().Key == ConsoleKey.Y);
            }
        }
    }
}
