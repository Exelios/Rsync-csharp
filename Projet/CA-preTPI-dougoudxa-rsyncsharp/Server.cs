/// ETML
/// Author: Xavier Dougoud
/// Date: 18.04.2016

using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace CA_preTPI_dougoudxa_rsyncsharp
{
    /// <summary>
    /// Class that listens to specified port and awaits incoming data (File, IP addrass, etc.)
    /// </summary>
    class Server
    {
        //////////////////////////////////////////Class Attributes///////////////////////////////////////////////////////////////

        /// <summary>
        /// This string allows us to use a common application directory like OneDrive or DropBox, and also getting it from user to user (solving some security issues).
        /// http://stackoverflow.com/questions/1240373/how-do-i-get-the-current-username-in-net-using-c
        /// </summary>
        public static string DEFAULT_DIRECTORY_PATH = "C:\\Users\\" + Environment.UserName + "\\Rsync#\\";

        /// <summary>
        /// Thread executing the Server.receive methode.
        /// </summary>
        private static Thread tcpListenerThread;
        
        /// <summary>
        /// Attribut reinitialized in Server.receive.
        /// </summary>
        private static TcpListener tcpListener;

        ////////////////////////////////////////Class Methods///////////////////////////////////////////////////////////////////

        /// <summary>
        /// Creates the home directory in the user's personnal folder if it doesn't already exist.
        /// </summary>
        public static void createHomeDirectory()
        {
            if (!Directory.Exists("C:\\Users\\" + Environment.UserName + "\\Rsync#\\"))
            {
                Directory.CreateDirectory("C:\\Users\\" + Environment.UserName + "\\Rsync#\\");
            }
        }
        /*--------------------------------------------------------------------------------------*/

        /// <summary>
        /// Method that listens port 4400 for incoming transmissions. Repeats itself !
        /// </summary>
        public static void receive()
        {
            while (!Program.getExitStatus())
            {
                tcpListener = new TcpListener(new IPEndPoint(IPAddress.Any, 4400));
                tcpListener.Start();
                NetworkConfig.tcpClient = tcpListener.AcceptTcpClient();
                tcpListener.Stop();
                NetworkStream networkStreamServer = NetworkConfig.tcpClient.GetStream();


                //Receive File infos
                BinaryFormatter binaryFormatterServer = new BinaryFormatter();
                FileTransfer fileTransferServer = (FileTransfer)binaryFormatterServer.Deserialize(networkStreamServer);

                //Writes on the reveiving screen what was received
                Console.Write(fileTransferServer.name + " - " + fileTransferServer.size + " o\n\nRsync#> ");

                Int64 fileSize = fileTransferServer.size;
                String fullFilePath = fileTransferServer.name;
                String fileHash = fileTransferServer.hash;

                //ok, receive/write the file
                //By default if the directory in which should be contained doesn't exist then it will go into the Home Folder.
                String[] directoryList = fullFilePath.Split('\\');

                //Only takes the last part of the path which is the file name.
                FileStream fileStreamServer = new FileStream(DEFAULT_DIRECTORY_PATH + directoryList[directoryList.Length - 1], FileMode.Create);
                Int64 bytesReceived = 0;

                while (bytesReceived < fileSize)
                {
                    Byte[] bufferArray = (Byte[])binaryFormatterServer.Deserialize(networkStreamServer);
                    NetworkConfig.bufferSize = bufferArray.Length;
                    fileStreamServer.Write(bufferArray, 0, (int)NetworkConfig.bufferSize);
                    bytesReceived += NetworkConfig.bufferSize;

                    //Add a progress bar here.
                }

                binaryFormatterServer.Serialize(networkStreamServer, new FileTransfer("", 0, fileHash));

                //Close ALL objects
                fileStreamServer.Close();
                networkStreamServer.Close();

                tcpListener = null;
            }
        }
        /*----------------------------------------------------------------------------------------------*/

        /// <summary>
        /// Start listening thread in Program.Main()
        /// </summary>
        public static void startTCPlisteners()
        {
            tcpListenerThread = new Thread(receive);
            tcpListenerThread.Start();
        }
        /*--------------------------------------------------------------------*/
    }
}
