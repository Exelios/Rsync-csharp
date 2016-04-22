/// ETML
/// Author: Xavier Dougoud
/// Date:   18.04.2016

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA_preTPI_dougoudxa_rsyncsharp
{
    /// <summary>
    /// 
    /// </summary>
    class StatusText
    {
        /// <summary>
        /// Used by the parseCommand method in Program class. Splits the command string into the input and path.
        /// Input Values: upload, upgrade, delete, quit.
        /// </summary>
        private static String commandInput;

        /// <summary>
        /// Getter for the commandInput attribute.
        /// </summary>
        /// <returns>commandeInput</returns>
        public static String getCommandInput()
        {
            return commandInput;
        }

        /// <summary>
        /// Setter that sets a new value to commandInput attribute.
        /// </summary>
        /// <param name="consoleCommandInput">New value parsed form parseCommand method in Program class.</param>
        public static void setCommandInput(String consoleCommandInput)
        {
            commandInput = consoleCommandInput;
        }

        /// <summary>
        /// Contains the path of whatever will be modified in any way in the home folder of thre program.
        /// </summary>
        private static String pathInput;

        /// <summary>
        /// Getter for the pathInput attribute.
        /// </summary>
        /// <returns>pathInput</returns>
        public static String getCommandPath()
        {
            return pathInput;
        }

        /// <summary>
        /// Setter that sets a new value to pathInput attribute.
        /// </summary>
        /// <param name="consolePathInput">New value parsed from pareCommand method in Program class.</param>
        public static void setPathInput(String consolePathInput)
        {
            pathInput = consolePathInput;
        }

        /// <summary>
        /// Input values for the command to be executed.
        /// </summary>
        private static String[] commandInputValueArray = {"upload", "upgrade", "delete", "quit"};

        /// <summary>
        /// Getter accessing the commandInputValueArray.
        /// </summary>
        /// <param name="arrayIndex">Index of the value we wish</param>
        /// <returns>String typed value</returns>
        public static String getCommandValue(int arrayIndex)
        {
            return commandInputValueArray[arrayIndex];
        }

        /*---------------------------------------------------------------------------------*/
    }
}
