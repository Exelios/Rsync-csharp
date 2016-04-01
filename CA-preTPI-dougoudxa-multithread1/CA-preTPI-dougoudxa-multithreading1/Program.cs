/// ETML
/// Author: Xavier Dougoud
/// Date: 17.03.2016
/// Summary: multithreading1
/// This program has for only purpose to familiarize the programmer
/// with the notion of multithreading.


using System;
using System.Threading;

namespace CA_preTPI_dougoudxa_multithreading1
{
    class Program
    {
        static void Main(string[] args)
        {
            //This is how we create a thread. 
            //new ParameterizedThreadStart() allows us to create a thread that can have take the parameter of the method it's threading as his own.
            Thread th1 = new Thread(new ParameterizedThreadStart(WriteTestLetters));

            //Second thread to compare how threads are synched
            Thread th2 = new Thread(new ParameterizedThreadStart(WriteTestLetters));

            //Starting the threads. Here we give the thread a parameter and the thread gives that parameter to the method it's generating.
            th1.Start("X");
            th2.Start("O");

            Console.ReadKey();
        }

        /// <summary>
        /// Method tested as thread parameter
        /// </summary>
        /// <param name="message">The type of this parameter must be "object" so Thread.Start can accept a parameter when called</param>
        static void WriteTestLetters(object message)
        {
            for(int i = 0; i < 10000; ++i)
            {
                //Since "message" is of "object" type, we must cast it as "string" so "Consol.Write" will not have a type issue
                Console.Write((string)message);
            }
            Console.WriteLine("----------------Thread {0} terminated------------------", (string)message);
        }
    }
}
