using System;

namespace learn_xamarin.Server
{
    public static class Logger
    {
        public static void Info(string content)
        {
            Console.WriteLine(content);
        }

        public static void Debug(string content)
        {
            Console.WriteLine(content);
        }
    }
}