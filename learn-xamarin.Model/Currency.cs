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

        public static Currency[] All => new[] { Pln, Gbp, Eur };
        public static readonly Currency Pln = new PlnCurrency();
        public static readonly Currency Gbp = new GbpCurrency();
        public static readonly Currency Eur = new EurCurrency();

        public static Currency FromCodeString(string code)
        {
            return All.First(c => c.Code == code);
        }

        private class PlnCurrency : Currency
        {
            public override string Code => "PLN";
            public override string Name => "Polish Złoty";
        }

        private class GbpCurrency : Currency
        {
            public override string Code => "GBP";
            public override string Name => "British Pound";
        }

        private class EurCurrency : Currency
        {
            public override string Code => "EUR";
            public override string Name => "Euro";
        }
    }
}
