/// ETML
/// Author: Xavier Dougoud
/// Date: 21.03.2016
/// Summary: multithread5
/// This program has for only purpose to familiarize the programmer
/// with the notion of multithreading and limiting ressouces using SemaphoreSlim



using System;
using System.Threading;

namespace CA_preTPI_dougoudxa_multithread5
{
    class Program
    {
        //Declaration of the SemaphoreSlim that take as parameter the number of guests allowed
        static SemaphoreSlim doorman = new SemaphoreSlim(3);
        
        static void Main(string[] args)
        {
            Console.Title = "SemaphoreSlim example";
            
            //Creation of the threads
            for (int i = 0; i < 10; i++)
                new Thread(Enter).Start(i);

            Console.ReadKey();
        }
        
        static void Enter(object n)
        {
            Console.WriteLine("Person #{0} wants to enter", n);
            
            //Le doorman will wait for room to be made.
            doorman.Wait();
            Console.WriteLine("#{0} just entered the bar", n);
            Thread.Sleep((int)n * 1000);
            Console.WriteLine("#{0} left the building !", n);
            
            //The doorman can now let someone in.
            doorman.Release();
        }
    }
}
