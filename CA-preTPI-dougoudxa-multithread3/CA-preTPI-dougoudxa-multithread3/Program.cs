/// ETML
/// Author: Xavier Dougoud
/// Date: 21.03.2016
/// Summary: multithread3
/// This program has for only purpose to familiarize the programmer
/// with the notion of multithreading and locking ressources


using System;
using System.Threading;

namespace CA_preTPI_dougoudxa_multithread3
{
    class Program
    {
        //Lock witness variable
        private static Object g_lock = new Object();

        //Used to initialize the partially random values from internal clock
        private static Random g_rand = new Random((int)DateTime.Now.Ticks);

        //Thread control variable
        private static bool g_quit = false;

        //Global variables affected by the threads
        private static int g_numerator;
        private static int g_denominator;

        static void Main(string[] args)
        {
            Console.Title = "Lock demonstration";

            //We create the threads
            Thread thrInitialize = new Thread(initialize);
            thrInitialize.Start();

            Thread thrReinitialize = new Thread(reinitialize);
            thrReinitialize.Start();

            Thread thrDivide = new Thread(divide);
            thrDivide.Start();

            //5 secondes of execution
            Thread.Sleep(5000);

            //Makes the threads quit
            g_quit = true;

            Console.ReadKey();
        }

        /// <summary>
        /// 
        /// </summary>
        private static void initialize()
        {
            //Controlled infinite loop
            while (!g_quit) //Do remember that syntax, it's really useful
            {
                //We lock access to the variables as long as we're not done
                lock (g_lock)
                {
                    //Value initialization
                    g_numerator = g_rand.Next(20);
                    g_denominator = g_rand.Next(2, 30);
                }

                //Start again in 0.25 seconds
                Thread.Sleep(250);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static void reinitialize()
        {
            //Controlled infinite loop
            while (!g_quit) //Do remember that syntax, it's really useful
            {
                //We lock access to the variables as long as we're not done
                lock (g_lock)
                {
                    //Value initialization
                    g_numerator = g_rand.Next(20);
                    g_denominator = g_rand.Next(2, 30);
                }

                //Start again in 0.3 seconds
                Thread.Sleep(300);
            }
        }

        /// <summary>
        /// There will no divide by 0 error
        /// </summary>
        private static void divide()
        {
            //Controlled infinite loop
            while (!g_quit) //Do remember that syntax, it's really useful
            {
                //We lock access to the variables as long as we're not done
                lock (g_lock)
                {
                    //Value initialization
                    if(g_denominator == 0)
                    {
                        Console.WriteLine("Divide by 0 error");
                    }
                    else
                    {
                        Console.WriteLine("{0} / {1} = {2}", g_numerator, g_denominator, (double)g_numerator / (double)g_denominator);
                    }
                }

                //Start again in 0.275 seconds
                Thread.Sleep(275);
            }
        }
    }
}
