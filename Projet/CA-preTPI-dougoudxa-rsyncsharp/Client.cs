﻿/// ETML
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
        /// Uploads the file/directory to every user on the network.
        /// </summary>
        /// <param name="path">Upload target</param>
        public static void upload(String path)
        {

        }
        /*-------------------------------------------------------------------------------*/

        /// <summary>
        /// Updates the current contents of the specified directory for users connected on the network.
        /// </summary>
        /// <param name="path">Directory needed to be updated</param>
        public static void update(String path)
        {

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
        /// Opens the specified directory. If no path was specified opens the Rsync# home directory.
        /// </summary>
        /// <param name="path">Directory needed to be opened</param>
        public static void open(String path)
        {
            Process.Start(Server.DEFAULT_DIRECTORY_PATH + path);
        }
        /*-----------------------------------------------------------------------------*/

        /// <summary>
        /// Exits the program.
        /// </summary>
        public static void quit()
        {
            Program.setExitStatus(true);
        }
        /*-------------------------------------------------------------------------------*/

        /// <summary>
        /// Shows the help screen of the application.
        /// </summary>
        public static void showHelpScreen()
        {
            Console.Write("\nNeed some help ?\n");

            Console.Write("Here are the basics: \n");
            Console.Write("This is the standard request syntaxe : Rsync#> \"REQUEST\" \"PATH\"\n");
            Console.Write("\nList of the requests :\n\nUpload\nUpdate\nCreate\nDelete\nOpen\nExit Quit\nHelp ?\n\n");

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
        /// Executes the command typed in by user.
        /// </summary>
        public static void executeRequest()
        {
            int commandIndex = 0;

            //Compares the command input to the list of possible commands.
            for(int i = 0; i < StatusText.getcommandInputValueArrayLength(); ++i)
            {
                if(StatusText.getCommandValue(i) == StatusText.getCommandInput())
                {
                    commandIndex = i;
                    break;
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
                    showHelpScreen();
                    break;

                case 8:
                    create(StatusText.getPathInput());
                    break;

                default:
                    showHelpScreen();
                    break;

            }
            //End of switch

            //Erases the command just executed to prevent any misbehaviours for future requests.
            StatusText.setCommandInput("");
            StatusText.setPathInput("");
        }
        /*-------------------------------------------------------------------------------*/
    }
}
