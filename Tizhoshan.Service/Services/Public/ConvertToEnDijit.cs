using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tizhoshan.ServiceLayer.Services.Public
{
    public static class ConvertToEnDijit
    {
        public static string convertToEn(this string input)
        {
            string[] persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (int j = 0; j < persian.Length; j++)
                input = input.Replace(persian[j], j.ToString());

            return input;

        }
    }
}
