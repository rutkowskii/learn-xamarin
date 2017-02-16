using System;
using Nancy.Hosting.Self;

namespace learn_xamarin.Sever
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new NancyHost(new Uri("http://localhost:19666")))
            {
                host.Start();
                Console.WriteLine("Running on http://localhost:19666, press any key to quit");
                Console.ReadLine();
            }
        }
    }
}
