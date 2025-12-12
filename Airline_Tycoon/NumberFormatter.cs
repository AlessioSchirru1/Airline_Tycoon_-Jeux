using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Tycoon
{
    internal class NumberFormatter
    {
        private static readonly string[] Suffixes = GenerateSuffixes().ToArray();

        public static string Format( BigInteger number )
        {
            if(number < 1000)
                return number.ToString(); // pas de suffixe si < 1000

            double value = (double)number;
            int suffixIndex = -1;

            // On divise par 1000 jusqu'à trouver le bon suffixe
            while(value >= 1000 && suffixIndex < Suffixes.Length - 1)
            {
                value /= 1000;
                suffixIndex++;
            }

            // On affiche avec 2 décimales et le suffixe
            //return $"{value:0.00}{Suffixes[suffixIndex]}";
            return value.ToString("0.00", CultureInfo.InvariantCulture) + Suffixes[suffixIndex];
        }

        private static List<string> GenerateSuffixes()
        {
            var suffixes = new List<string> { "K", "M", "B", "T" }; // déjà connus

            for(char first = 'a' ; first <= 'z' ; first++)
            {
                for(char second = 'a' ; second <= 'z' ; second++)
                {
                    suffixes.Add($"{first}{second}");
                }
            }

            return suffixes;
        }
    }
}
