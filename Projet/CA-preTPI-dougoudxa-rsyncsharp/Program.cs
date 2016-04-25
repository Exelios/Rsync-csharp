﻿/// ETML
/// Author: Xavier Dougoud
/// Date: 18.04.2016
/// Summary: 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CA_preTPI_dougoudxa_rsyncsharp
{
    class Program
    {
        /// <summary>
        /// Attribute insuring correct application closing sequence.
        /// </summary>
        private static bool EXIT_STATUS = false;

        /// <summary>
        /// Method used by the quit method in the Client class.
        /// </summary>
        /// <param name="newStatus">Set as true</param>
        public static void setExitStatus(bool newStatus)
        {
            EXIT_STATUS = newStatus;
        }
        /*--------------------------------------------------------*/

        /// <summary>
        /// Parses the input from the user into a command string and a path string
        /// </summary>
        /// <param name="consoleInput">Input from user</param>
        private static void parseCommand(String consoleInput)
        {
            string[] inputArray = consoleInput.Split(' ');

            StatusText.setCommandInput(inputArray[0]);

            if (inputArray.Length > 1)
            {
                StatusText.setPathInput(inputArray[1]);
            }
        }
        /*--------------------------------------------------------------*/

        /// <summary>
        /// Sends the input from console to the parsing method along with providing a MSDos output.
        /// </summary>
        private static String typeInRequest()
        {
            Console.Write("\nRsync#> ");

            return Console.ReadLine();
        }
        /*--------------------------------------------------------------------------------------*/
        

        /// <summary>
        /// Core of the program, automatically executed
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Server.startTCPlistener();

            StatusText.showWelcomeMessage();


            while (EXIT_STATUS == false)
            {
                parseCommand(typeInRequest());

                //Test debug
                //Console.WriteLine("For debugging purposes\nThe command parsed: " + StatusText.getCommandInput() + "\nThe path parsed: " + StatusText.getPathInput());
                //Client.showIPs();

                Client.executeRequest();

                Server.restartTCPlistener();
            }

            //Closing instructions
        }
        //End of Main()
    }
}
