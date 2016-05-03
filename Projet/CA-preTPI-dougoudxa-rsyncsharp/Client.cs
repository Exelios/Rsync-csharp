/// ETML
/// Author: Xavier Dougoud
/// Date: 18.04.2016

using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Collections.Generic;

namespace CA_preTPI_dougoudxa_rsyncsharp
{
    /// <summary>
    /// Class that will send data through the network in broadcast mode.
    /// </summary>
    class Client
    {
        #region 
        ///////////////////////////Class Methods///////////////////////////////////////////

        /// <summary>
        /// This method hashes every buffer content passing through the file stream
        /// Source: http://codes-sources.commentcamarche.net/source/53449-transfert-de-fichier
        /// </summary>
        /// <param name="fileName">String containing the Windows path to the file you want to send</param>
        /// <returns>The file's hashprint</returns>
        public static String hashingFile(String fileName)
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
        public static void sendFile(IPAddress destinationIP, String fullFilePath)
        {
            NetworkConfig.tcpClient = new TcpClient();
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

            //Close ALL objects
            fileStreamClient.Close();
            networkStreamClient.Close();

            NetworkConfig.tcpClient.Close();
            NetworkConfig.tcpClient = null;
        }
        /*------------------------------------------------------------------------------*/

        /// <summary>
        /// Uploads the file/directory to every user on the network.
        /// </summary>
        /// <param name="path">Upload target</param>
        public static void upload(String path)
        {
            //The file must existe if we want to send it.
            if (File.Exists(Server.DEFAULT_DIRECTORY_PATH + path))
            {
                //We want to send this file to everyone
                foreach (IPAddress ipAdd in NetworkConfig.ipAddressList)
                {
                    sendFile(ipAdd, Server.DEFAULT_DIRECTORY_PATH + path);
                    
                    //For debugging purposes.
                    //Console.WriteLine("Sent to " + ipAdd.ToString());
                }
                //File has been sent to everyone.
                Console.WriteLine("Done");
            }
            else
            {
                Console.WriteLine("\nNo such file or directory");
            }
        }
        /*-------------------------------------------------------------------------------*/

        /// <summary>
        /// INCOMPLETE !
        /// Updates the current contents of the specified directory for users connected on the network.
        /// </summary>
        /// <param name="path">Directory needed to be updated</param>
        public static void update(String path)
        {

            //For debugging purposes, method not yet defined
            Console.WriteLine("Update function not yet available.");
        }
        /*--------------------------------------------------------------------------------*/

        /// <summary>
        /// Deletes the file/directory specified by the parameter.
        /// </summary>
        /// <param name="path">File/directory needed to be deleted</param>
        public static void delete(String path)
        {
            if (Directory.Exists(Server.DEFAULT_DIRECTORY_PATH + path))
            {
                Directory.Delete(Server.DEFAULT_DIRECTORY_PATH + path, true);
                Console.WriteLine("\nDone");
            }
            else
            {
                if (File.Exists(Server.DEFAULT_DIRECTORY_PATH + path))
                {
                    File.Delete(Server.DEFAULT_DIRECTORY_PATH + path);
                    Console.WriteLine("\nDone");
                }
                else
                {
                    Console.WriteLine("\nNo such file or directory");
                }
            }
        }
        /*-------------------------------------------------------------------------------*/

        /// <summary>
        /// Opens the specified file or directory. If no path was specified opens the Rsync# home directory.
        /// </summary>
        /// <param name="path">Directory needed to be opened</param>
        public static void open(String path)
        {
            if (Directory.Exists(Server.DEFAULT_DIRECTORY_PATH + path))
            {
                Process.Start(Server.DEFAULT_DIRECTORY_PATH + path);
            }
            else
            {
                if (File.Exists(Server.DEFAULT_DIRECTORY_PATH + path))
                {
                    Process.Start(Server.DEFAULT_DIRECTORY_PATH + path);
                }
                else
                {
                    Console.WriteLine("\nNo such file or directory");
                }
            }
        }
        /*-----------------------------------------------------------------------------*/

        /// <summary>
        /// Exits the program.
        /// </summary>
        public static void quit()
        {
            Console.WriteLine("Exiting program... ");

            Program.setExitStatus(true);

            //Stops the 2 udp threads.
            NetworkConfig.stopUDPServer();

            Environment.Exit(0);
        }
        /*-------------------------------------------------------------------------------*/

        /// <summary>
        /// Shows the help screen of the application.
        /// </summary>
        public static void showHelpScreen()
        {
            Console.Write("\nNeed some help ?\n\n");

            Console.Write("Here are the basics: \n");
            Console.Write("This is the standard request syntaxe : Rsync#> \"REQUEST\" \"PATH\"\n");
            Console.Write("\nList of the requests :\n\n");

            Console.Write("Upload \"path\" > uploads the file specified by \"path\" to everyone on the network.\n");
            Console.Write("Update \"path\" > updates the directory specified by \"path\" for everyone on the network.\n");
            Console.Write("Create \"path\" > creates the specified directory in home folder on local machine. \n");
            Console.Write("Delete \"path\" > deletes the specified file or directory on local machine. Directory is recursively deleted.\n");
            Console.Write("Open \"path\"   > opens the specified directory. If no path is specified then the Rsync# home folder is opened.\n");
            Console.Write("Exit and Quit > close the program.\n");
            Console.Write("Help and ?    > show the help screen.\n");
            Console.Write("IP            > shows all the IPv4 addresses this machine can sync with.\n\n");
            Console.Write("When entering a file path, you must specify the file extension : .txt .jpg .pdf etc.\n");
        }
        /*------------------------------------------------------------------------------*/

        /// <summary>
        /// Creates the specified directory if it doesn't already exist.
        /// </summary>
        /// <param name="path">Target path</param>
        public static void create(String path)
        {
            if (!Directory.Exists(Server.DEFAULT_DIRECTORY_PATH + path))
            {
                Directory.CreateDirectory(Server.DEFAULT_DIRECTORY_PATH + path);
            }
            else
            {
                Console.WriteLine("\nDirectory already exists.\n");
            }
        }
        /*-------------------------------------------------------------------------------*/

        /// <summary>
        /// For debugging purposes only. Shows all the ip adresses the program received.
        /// </summary>
        public static void showIPs()
        {
            List<IPAddress> tempIPList = NetworkConfig.ipAddressList;

            Console.WriteLine("Your IP Address: " + NetworkConfig.myIPAddress.ToString());

            foreach (IPAddress ipAdd in tempIPList)
            {
                Console.WriteLine("Remote host    : " + ipAdd);
            }
        }
        /*---------------------------------------------------------------------------*/

        /// <summary>
        /// Executes the command typed in by user.
        /// </summary>
        public static void executeRequest()
        {
            int commandIndex = 0;

            //Compares the command input to the list of possible commands.
            for (int i = 0; i < StatusText.getcommandInputValueArrayLength() + 1; ++i)
            {
                //A correcte instruction was entered
                if (i < StatusText.getcommandInputValueArrayLength() && StatusText.getCommandValue(i) == StatusText.getCommandInput())
                {
                    commandIndex = i;
                    break;
                }
                //A non existing instruction was entered
                else
                {
                    commandIndex = i;
                }
            }
            //End of for

            //Calls the correct method depending on the command input
            switch (commandIndex)
            {
                case 0:
                    upload(StatusText.getPathInput());
                    break;

                case 1:
                    update(StatusText.getPathInput());
                    break;

                case 2:
                    delete(StatusText.getPathInput());
                    break;

                case 3:
                    open(StatusText.getPathInput());
                    break;

                case 4:
                case 5:
                    quit();
                    break;

                case 6:
                case 7:
                case 10:
                    showHelpScreen();
                    break;

                case 8:
                    create(StatusText.getPathInput());
                    break;

                case 9:
                    showIPs();
                    break;

            }
            //End of switch

            //Erases the command just executed to prevent any misbehaviours for future requests.
            StatusText.setCommandInput("");
            StatusText.setPathInput("");
        }
        /*-------------------------------------------------------------------------------*/
        #endregion
    }
}