using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn_xamarin.Model
{
    public abstract class Currency
    {
        public abstract string Code { get; }
        public abstract string Name { get; }

        public static Currency[] All => new Currency[] { new Pln(), new Gbp(), new Eur() };

        private class Pln : Currency
        {
            public override string Code => "PLN";
            public override string Name => "Polish Złoty";
        }

        private class Gbp : Currency
        {
            public override string Code => "GBP";
            public override string Name => "British Pound";
        }

        private class Eur : Currency
        {
            public override string Code => "EUR";
            public override string Name => "Euro";
        }
    }
}
