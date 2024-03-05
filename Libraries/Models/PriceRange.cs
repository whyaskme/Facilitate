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
            TotalMin = 0;
            TotalMax = 0;
            MonthlyMin = 0;
            MonthlyMax = 0;
        }

        public double TotalMin { get; set; }
        public double TotalMax { get; set; }
        public double MonthlyMin { get; set; }
        public double MonthlyMax { get; set; }
    }
}
