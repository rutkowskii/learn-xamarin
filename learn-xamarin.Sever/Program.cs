using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
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
                Console.WriteLine("Running on http://localhost:19666");
                Console.ReadLine();
            }
        }
    }

    public class ExpendituresModule : NancyModule
    {
        public ExpendituresModule()
        {
            Get["/expenditure"] = parameters => GetAllExpenditures();
            Post["/expenditure"] = parameters => AddExpenditure(parameters);
        }

        private object AddExpenditure(object parameters)
        {
            throw new NotImplementedException();
        }

        private object GetAllExpenditures()
        {
            throw new NotImplementedException();
        }
    }
}
