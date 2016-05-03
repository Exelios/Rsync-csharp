/// ETML
/// Author: Xavier Dougoud
/// Date:   18.04.2016

using System;

namespace CA_preTPI_dougoudxa_rsyncsharp
{
    /// <summary>
    /// Class containing most of the constant text used in the program.
    /// </summary>
    class StatusText
    {
        #region
        ///////////////////////////////////////Class Attribute///////////////////////////////////////////////

        /// <summary>
        /// Used by the parseCommand method in Program class. Splits the command string into the input and path.
        /// Input Values: upload, upgrade, delete, quit.
        /// </summary>
        private static String commandInput;

        /// <summary>
        /// Contains the path of whatever will be modified in any way in the home folder of thre program.
        /// </summary>
        private static String pathInput;

        /// <summary>
        /// Input values for the command to be executed.
        /// </summary>
        private static String[] commandInputValueArray = { "upload", "update", "delete", "open", "quit", "exit", "help", "?", "create", "ip" };
        #endregion

        #region
        //////////////////////////////////////Class Methods/////////////////////////////////////////////////

        /// <summary>
        /// Getter for the commandInput attribute.
        /// </summary>
        /// <returns>commandeInput</returns>
        public static String getCommandInput()
        {
            return commandInput;
        }
        /*-------------------------------------*/

        /// <summary>
        /// Setter that sets a new value to commandInput attribute.
        /// </summary>
        /// <param name="consoleCommandInput">New value parsed form parseCommand method in Program class.</param>
        public static void setCommandInput(String consoleCommandInput)
        {
            commandInput = consoleCommandInput.ToLower();
        }
        /*-------------------------------------------------------------------------------------------*/

        /// <summary>
        /// Getter for the pathInput attribute.
        /// </summary>
        /// <returns>pathInput</returns>
        public static String getPathInput()
        {
            return pathInput;
        }
        /*--------------------------------------------------------------------------------------------*/

        /// <summary>
        /// Setter that sets a new value to pathInput attribute.
        /// </summary>
        /// <param name="consolePathInput">New value parsed from pareCommand method in Program class.</param>
        public static void setPathInput(String consolePathInput)
        {
            pathInput = consolePathInput.ToLower();
        }
        /*---------------------------------------------------------------------------------------------*/

        /// <summary>
        /// Getter to get the length of the commandInputVlueArray
        /// </summary>
        /// <returns>Integer</returns>
        public static int getcommandInputValueArrayLength()
        {
            return commandInputValueArray.Length;
        }
        /*-------------------------------------------------------*/

        /// <summary>
        /// Getter accessing the commandInputValueArray.
        /// </summary>
        /// <param name="arrayIndex">Index of the value we wish</param>
        /// <returns>String typed value</returns>
        public static String getCommandValue(int arrayIndex)
        {
            return commandInputValueArray[arrayIndex];
        }
        /*---------------------------------------------------------------*/
        
        /// <summary>
        /// Shows the welcome message and welcome text when the program starts.
        /// </summary>
        public static void showWelcomeMessage()
        {
            for(int i = 0; i < 3; ++i)
            {
                if(i == 0 || i == 2)
                {
                    for(int j = 0; j < 45; ++j)
                    {
                        Console.Write("*");
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("*             Welcome to RSYNC#             *");
                }
            }
        }
        /*------------------------------------------------------------------------------*/
        #endregion
    }
}
