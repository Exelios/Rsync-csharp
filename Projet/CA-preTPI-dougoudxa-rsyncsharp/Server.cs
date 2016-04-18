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
        /// 
        /// </summary>
        public static void receive()
        {
            TcpListener tcpListener = new TcpListener(new IPEndPoint(IPAddress.Any, 4400));
            tcpListener.Start();
            NetworkConfig.tcpClient = tcpListener.AcceptTcpClient();
            tcpListener.Stop();
            NetworkStream networkStreamServer = NetworkConfig.tcpClient.GetStream();


            //Receive File infos
            BinaryFormatter binaryFormatterServer = new BinaryFormatter();
            FileTransfer fileTransferServer = (FileTransfer)binaryFormatterServer.Deserialize(networkStreamServer);

            Console.WriteLine(fileTransferServer.name + " - " + fileTransferServer.size);
            Int64 fileSize = fileTransferServer.size;
            String fullFilePath = fileTransferServer.name;
            String fileHash = fileTransferServer.hash;

            //ok, receive/write the file
            FileStream fileStreamServer = new FileStream(fullFilePath, FileMode.Create);
            Int64 bytesReceived = 0;
            
            while (bytesReceived < fileSize)
            {
                Byte[] bufferArray = (Byte[])binaryFormatterServer.Deserialize(networkStreamServer);
                NetworkConfig.bufferSize = bufferArray.Length;
                fileStreamServer.Write(bufferArray, 0, (int)NetworkConfig.bufferSize);
                bytesReceived += NetworkConfig.bufferSize;

                //Add a progress bar here.
            }

            //Console.WriteLine((fileTransferServer.hash == fileHash) ? "File correctly recieved" : "Error during transfer");

            binaryFormatterServer.Serialize(networkStreamServer, new FileTransfer("", 0, fileHash));

            //Close ALL objects
            fileStreamServer.Close();
            networkStreamServer.Close();
        }
    }
}
