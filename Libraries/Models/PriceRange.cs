using Facilitate.Libraries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Models
{
    public class PriceRange
    {
        public PriceRange()
        {
            totalMin = 0;
            totalMax = 0;
            monthlyMin = 0;
            monthlyMax = 0;
        }

        public double totalMin { get; set; }
        public double totalMax { get; set; }
        public double monthlyMin { get; set; }
        public double monthlyMax { get; set; }
    }
}
