/// ETML
/// Author: Xavier Dougoud
/// Date: 12.04.2016
/// Summary: Simple File Transfer Program
/// This program has for only purpose to familiarize the programmer
/// with the notion of network programming and using those notions to be able to send a file through 
/// the network to a host.
/// 
/// Based on the source code from http://codes-sources.commentcamarche.net/source/53449-transfert-de-fichier



using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace CA_preTPI_dougoudxa_filetranfert
{
    class Program
    {
        //http://stackoverflow.com/questions/1240373/how-do-i-get-the-current-username-in-net-using-c
        //This string allows us to use a common app directory like OneDrive or DropBox, and also getting it from user to user (solving some security issues)
        static string DEFAULT_DIRECTORY_PATH = "C:\\Users\\" + Environment.UserName + "\\Rsync#\\";

        /// <summary>
        /// Creates the home directory in the user's personnal folder if it doesn't already exist.
        /// </summary>
        static void createHomeDirectory()
        {
            if(!Directory.Exists("C:\\Users\\" + Environment.UserName + "\\Rsync#\\"))
            {
                Directory.CreateDirectory("C:\\Users\\" + Environment.UserName + "\\Rsync#\\");
            }
        }
        /*--------------------------------------------------------------------------------------*/

        /// <summary>
        /// This method hashes every buffer content passing through the file stream
        /// </summary>
        /// <param name="fileName">String containing the Windows path to the file you want to send</param>
        /// <returns></returns>
        static String hashingFile(String fileName)
        {
            SHA1CryptoServiceProvider shaHash1 = new SHA1CryptoServiceProvider();
            byte[] bufferArray;

            using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read))
                bufferArray = shaHash1.ComputeHash(fileStream);

            return BitConverter.ToString(bufferArray).Replace("-", String.Empty);
        }
        /*----------------------------------------------------------------------------*/

        /// <summary>
        /// Core of the program
        /// </summary>
        /// <param name="args">None</param>
        static void Main(string[] args)
        {
            //First of all, we create the home directory of this application.
            createHomeDirectory();

            //
            Console.Write("What kind of machine is this, Client or Server ? (C/S) : ");
            string strMachineMode = Console.ReadLine().ToUpper();

            Int64 bufferSize = 32768;
            DateTime startDateTime;

            if (strMachineMode == "C")
            //Client config
            {
                //Connexion
                Console.Write("Server IP : ");

                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(new IPEndPoint(IPAddress.Parse(Console.ReadLine()), 1337));
                NetworkStream networkStreamClient = tcpClient.GetStream();

                Console.WriteLine("Server connected");
                Console.Write("File name : ");
                String fileName = Console.ReadLine();

                String fullFilePath = DEFAULT_DIRECTORY_PATH + fileName;


                //Send File infos
                BinaryFormatter binaryFormatterClient = new BinaryFormatter();

                String fileHash = hashingFile(fullFilePath);
                Int64 fileSize = new FileInfo(fullFilePath).Length;
                FileTransfer fileTransferClient = new FileTransfer(fullFilePath, fileSize, fileHash);

                binaryFormatterClient.Serialize(networkStreamClient, fileTransferClient);
                startDateTime = DateTime.Now;

                //ok, read/send the file
                FileStream fileStreamClient = new FileStream(fullFilePath, FileMode.Open);
                Int64 bytesTransferred = 0;

                using (var progress = new ProgressBar())
                {
                    //Loop that shows the transfer in progress.
                    while (bytesTransferred < fileSize)
                    {
                        if (fileSize - bytesTransferred < bufferSize)
                        {
                            bufferSize = fileSize - bytesTransferred;
                        }

                        Byte[] buffer = new Byte[bufferSize];
                        fileStreamClient.Read(buffer, 0, (int)bufferSize);
                        binaryFormatterClient.Serialize(networkStreamClient, buffer);

                        bytesTransferred += bufferSize;

                        //It would be great to implement a progress bar here.

                        Console.Write("\rSending : ");

                        //Added progress bar from Daniel S. Wolf

                        progress.Report(bytesTransferred / fileSize);

                        //Problem with progress bar... will be resolved
                    }
                    //Console.WriteLine("Done.");
                }

                Console.WriteLine("\r\nFile sent");

                fileTransferClient = (FileTransfer)binaryFormatterClient.Deserialize(networkStreamClient);

                //Neat way to have a conditional output.
                Console.WriteLine((fileTransferClient.hash == fileHash) ? "File correctly recieved" : "Error during transfer");

                //Close ALL objects
                fileStreamClient.Close();
                networkStreamClient.Close();
                tcpClient.Close();
            }
            //end of Client config
            else
            //Here we are in the Server config
            {
                Console.WriteLine("Waiting for Client");

                //Waiting for a Client

                TcpListener tcpListener = new TcpListener(new IPEndPoint(IPAddress.Any, 1337));     //The port must be the same as line 60
                tcpListener.Start();
                TcpClient tcpServer = tcpListener.AcceptTcpClient();
                tcpListener.Stop();
                NetworkStream networkStreamServer = tcpServer.GetStream();

                Console.WriteLine("Client connecté");


                //Receive File infos
                BinaryFormatter binaryFormatterServer = new BinaryFormatter();
                FileTransfer fileTransferServer = (FileTransfer)binaryFormatterServer.Deserialize(networkStreamServer);

                Console.WriteLine(fileTransferServer.name + " - " + fileTransferServer.size);
                Int64 fileSize = fileTransferServer.size;
                String fullFilePath = fileTransferServer.name;
                String fileHash = fileTransferServer.hash;
                startDateTime = DateTime.Now;

                //ok, receive/write the file
                FileStream fileStreamServer = new FileStream(fullFilePath, FileMode.Create);
                Int64 bytesRecieved = 0;

                using (var progress = new ProgressBar())
                {
                    while (bytesRecieved < fileSize)
                    {
                        Byte[] bufferArray = (Byte[])binaryFormatterServer.Deserialize(networkStreamServer);
                        bufferSize = bufferArray.Length;
                        fileStreamServer.Write(bufferArray, 0, (int)bufferSize);
                        bytesRecieved += bufferSize;

                        //Add a progress bar here.

                        Console.Write("\rReceiving : ");

                        //Added progress bar from Daniel S. Wolf

                        progress.Report(bytesRecieved / fileSize);
                    }
                    //Console.WriteLine("Done.");
                }
            

                Console.WriteLine("\r\nFile recieved");
                Console.WriteLine((fileTransferServer.hash == fileHash) ? "File correctly recieved" : "Error during transfer");

                binaryFormatterServer.Serialize(networkStreamServer, new FileTransfer("", 0, fileHash));

                //Close ALL objects
                fileStreamServer.Close();
                networkStreamServer.Close();
                tcpServer.Close();
            }

            TimeSpan timeElapsedDuringTransfer = DateTime.Now - startDateTime;
            Console.WriteLine("Time elapsed : " + (timeElapsedDuringTransfer.Minutes + "m " + timeElapsedDuringTransfer.Seconds + "s " + timeElapsedDuringTransfer.Milliseconds + "ms"));

            Console.Read();
        }
    }

    /// <summary>
    /// Class used to simplify the implementation of the transfers.
    /// </summary>
    [Serializable]
    public class FileTransfer
    {
        public String name;
        public Int64 size;
        public String hash;

        public FileTransfer(String name, Int64 size, String hash)
        {
            this.name = name;
            this.size = size;
            this.hash = hash;
        }
    }
}
