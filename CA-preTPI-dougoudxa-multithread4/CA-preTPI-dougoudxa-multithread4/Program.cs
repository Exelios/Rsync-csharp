/// ETML
/// Author: Xavier Dougoud
/// Date: 21.03.2016
/// Summary: multithread4
/// This program has for only purpose to familiarize the programmer
/// with the notion of multithreading and locking ressources using Mutex



using System;
using System.Threading;

namespace CA_preTPI_dougoudxa_multithread4
{
    class Program
    {
        private const int ARRAY_SIZE = 2;

        //Creation of the different Mutex
        private static Mutex g_muxMultiply = new Mutex();
        private static Mutex g_muxDivide = new Mutex();


        //Creation of value arrays
        private static int[] g_valDiv = new int[ARRAY_SIZE];
        private static int[] g_valMul = new int[ARRAY_SIZE];


        //Random object and control variable
        private static Random g_rand = new Random((int)DateTime.Now.Ticks);
        private static bool g_quit = false;
        
        static void Main(string[] args)

        {
            Console.Title = "Mutex example";
            
            //Creation and start of the threads
            Thread init = new Thread(Initialize);
            init.Start();
            
            Thread mul = new Thread(Multiply);
            mul.Start();
            
            Thread div = new Thread(Divide);
            div.Start();


            //Time allowed for the threads to work
            Thread.Sleep(3000);

            //Responsible for the threads to quit
            g_quit = true;
            
            Console.ReadKey();
        }

        /// <summary>
        /// 
        /// </summary>
        private static void Initialize()
        {
            while (!g_quit)
            {
                //We tell the thread to wait until he has control over these Mutex
                g_muxMultiply.WaitOne();
                g_muxDivide.WaitOne();
                
                for (int i = 0; i < ARRAY_SIZE; i++)
                {
                    //Assigne new values to the array
                    g_valMul[i] = g_rand.Next(2, 20);
                    g_valDiv[i] = g_rand.Next(2, 20);
                }
                
                Console.WriteLine("New values !");
                
                //Releases the Mutex
                g_muxDivide.ReleaseMutex();
                g_muxMultiply.ReleaseMutex();
                
                //pause of the thread for 0.1 seconde
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static void Multiply()
        {
            while (!g_quit)
            {
                //We ask for the multiplication mutex
                g_muxMultiply.WaitOne();
                
                //We multiply
                Console.WriteLine("{0} x {1} = {2}", g_valMul[0], g_valMul[1], g_valMul[0] * g_valMul[1]);

                //Releases the multiplication Mutex
                g_muxMultiply.ReleaseMutex();

                //pause of the thread for 0.2 seconde
                Thread.Sleep(200);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static void Divide()
        {
            while (!g_quit)
            {
                //We ask for the division division.
                g_muxDivide.WaitOne();
                
                //We divide.
                Console.WriteLine("{0} / {1} = {2}", g_valDiv[0], g_valDiv[1], g_valDiv[0] * g_valDiv[1]);

                //Releases the division Mutex.
                g_muxDivide.ReleaseMutex();

                //pause of the thread for 0.2 seconde
                Thread.Sleep(200);
            }
        }
    }
}
