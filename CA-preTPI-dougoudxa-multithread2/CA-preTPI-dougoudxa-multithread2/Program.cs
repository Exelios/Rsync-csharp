/// ETML
/// Author: Xavier Dougoud
/// Date: 21.03.2016
/// Summary: multithread2
/// This program has for only purpose to familiarize the programmer
/// with the notion of multithreading and use of "Control Variables"

using System;
using System.Threading;

namespace CA_preTPI_dougoudxa_multithread2
{
    class Program
    {
        //A few global variables for this test
        private static bool g_quit = false;
        private static int g_id = 0;

        static void Main(string[] args)
        {
            Console.Title = "Control variables";    //Title of the console window

            //Creation of a Thread array.
            Thread[] threadsArray = new Thread[5];

            //Loop to start all the threads in the array one after another.
            for(int i = 0; i < 5; ++i)
            {
                //Creation and launch of the n-th thread
                threadsArray[i] = new Thread(openThread);
                threadsArray[i].Start();

                //Let 1/2 a second pass between the creation of each thread.
                Thread.Sleep(500);

            }//End of for

            //Asks all threads to quit.
            g_quit = true;

            Console.ReadKey();
        }

        /// <summary>
        /// 
        /// </summary>
        static void openThread()
        {
            //We give to each thread a unique ID. Also increments g_id every time openThread is used.
            int id = ++g_id;

            Console.WriteLine("Start of thread n° {0}", id);

            while (!g_quit) // "!g_quit" is equivelent to "g_quit == false" or "g_quit != true"
            {
                //Stuff done while thread still active
                Console.WriteLine("Thread n° {0} has control of the program", id);

                //Pause of the current thread
                Thread.Sleep(1000);
            }

            Console.WriteLine("Thread n° {0} has quit", id);
        }
    }
}
