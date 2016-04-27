/// ETML
/// Author: Xavier Dougoud
/// Date: 18.04.2016
/// Summary: 

using System;
using System.Threading;

namespace CA_preTPI_dougoudxa_rsyncsharp
{
    class Program
    {
        #region
        //////////////////////////////////////////////Class Attributes/////////////////////////////////////////////////////

        /// <summary>
        /// Attribute insuring correct application closing sequence.
        /// </summary>
        private static bool EXIT_STATUS = false;
        #endregion

        #region
        //////////////////////////////////////////////Class Methods///////////////////////////////////////////////////////

        /// <summary>
        /// Getter to access EXIT_STATUS attribute outside Program class.
        /// </summary>
        /// <returns>Boolean value</returns>
        public static bool getExitStatus()
        {
            return EXIT_STATUS;
        }
        /*--------------------------------------------------------------*/

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
        #endregion

        /// <summary>
        /// Core of the program, automatically executed
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Thread thread = new Thread(Server.startTCPlisteners);
            thread.Start();

            StatusText.showWelcomeMessage();


            while (EXIT_STATUS == false)
            {
                parseCommand(typeInRequest());

                Client.executeRequest();
            }
        }
        //End of Main()
    }
}
