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
using System.Diagnostics;

namespace CA_preTPI_dougoudxa_rsyncsharp
{
    /// <summary>
    /// Class that will send data through the network in broadcast mode.
    /// </summary>
    class Client
    {
        ////////////////////////Class Attributes///////////////////////////////////////////

        /// <summary>
        /// 
        /// </summary>
        private static String fileName;


        ///////////////////////////Class Methods///////////////////////////////////////////

        /// <summary>
        /// This method hashes every buffer content passing through the file stream
        /// Source: http://codes-sources.commentcamarche.net/source/53449-transfert-de-fichier
        /// </summary>
        /// <param name="fileName">String containing the Windows path to the file you want to send</param>
        /// <returns></returns>
        static String hashingFile(String fileName)
        {
            SHA1CryptoServiceProvider shaHash1 = new SHA1CryptoServiceProvider();
            byte[] bufferArray;

            using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                bufferArray = shaHash1.ComputeHash(fileStream);
            }

            return BitConverter.ToString(bufferArray).Replace("-", String.Empty);
        }
        /*----------------------------------------------------------------------------*/

        /// <summary>
        /// Method that manages how a file is sent to it's destination.
        /// </summary>
        /// <param name="destinationIP">IP address to which the data will be sent</param>
        /// <param name="fullFilePath">Path of the file that will be sent</param>
        public static void send(IPAddress destinationIP, String fullFilePath)
        {
            NetworkConfig.tcpClient.Connect(new IPEndPoint(destinationIP, 4400));
            NetworkStream networkStreamClient = NetworkConfig.tcpClient.GetStream();

            //Send File infos
            BinaryFormatter binaryFormatterClient = new BinaryFormatter();

            String fileHash = hashingFile(fullFilePath);
            Int64 fileSize = new FileInfo(fullFilePath).Length;
            FileTransfer fileTransferClient = new FileTransfer(fullFilePath, fileSize, fileHash);

            binaryFormatterClient.Serialize(networkStreamClient, fileTransferClient);

            //ok, read/send the file
            FileStream fileStreamClient = new FileStream(fullFilePath, FileMode.Open);
            Int64 bytesTransferred = 0;

                //Loop that shows the transfer in progress.
                while (bytesTransferred < fileSize)
                {
                    if (fileSize - bytesTransferred < NetworkConfig.bufferSize)
                    {
                    NetworkConfig.bufferSize = fileSize - bytesTransferred;
                    }

                    Byte[] buffer = new Byte[NetworkConfig.bufferSize];
                    fileStreamClient.Read(buffer, 0, (int)NetworkConfig.bufferSize);
                    binaryFormatterClient.Serialize(networkStreamClient, buffer);

                    bytesTransferred += NetworkConfig.bufferSize;

                    //It would be great to implement a progress bar here.
                }

            fileTransferClient = (FileTransfer)binaryFormatterClient.Deserialize(networkStreamClient);

            //Neat way to have a conditional output.
            //Console.WriteLine((fileTransferClient.hash == fileHash) ? "File correctly recieved" : "Error during transfer");

            //Close ALL objects
            fileStreamClient.Close();
            networkStreamClient.Close();
        }
        /*------------------------------------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static void upload(String fileName)
        {

        }
        /*-------------------------------------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public static void update(String path)
        {

        }
        /*--------------------------------------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public static void delete(String path)
        {

        }
        /*-------------------------------------------------------------------------------*/

        /// <summary>
        /// Opens the Rsync# directorx, the home folder.
        /// </summary>
        public static void open()
        {
            Process.Start(Server.DEFAULT_DIRECTORY_PATH);
        }
        /*-----------------------------------------------------------------------------*/

        /// <summary>
        /// 
        /// </summary>
        public static void quit()
        {

        }
        /*-------------------------------------------------------------------------------*/
    }
}
