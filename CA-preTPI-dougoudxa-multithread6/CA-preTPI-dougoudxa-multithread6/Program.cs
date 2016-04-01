/// ETML
/// Author: Xavier Dougoud
/// Date: 21.03.2016
/// Summary: multithread6
/// This program has for only purpose to familiarize the programmer
/// with the notion of multithreading using Thread.Join()


using System;
using System.Threading;

namespace CA_preTPI_dougoudxa_multithread6
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread th = new Thread(new ParameterizedThreadStart(Show));
            
            Thread th2 = new Thread(new ParameterizedThreadStart(Show));
            
            th.Start("A");
            
            //We wait for thread A to finish before starting thread B. 
            th.Join();
            //This gives us control for a sequential execution of multiple threads.
            //Most programmers discourage the use of Thread.Abort() because it kills to abruptily threads that can have an impact on the long term program.

            th2.Start("B");
            
            Console.ReadKey();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        static void Show(object text)
        {
            for (int i = 0; i < 10000; i++)
            {
                Console.Write((string)text);
            }
        }
    }
}
