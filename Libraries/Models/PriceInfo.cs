using Facilitate.Libraries.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Models
{
    public class PriceInfo
    {
        public PriceInfo()
        {
            PriceType = string.Empty;
            Total = 0.00;
            PricePerSquare = 0.00;
            Monthly = 0.00;
            Apr = 0.00;
            Months = 0;
        }

        public string PriceType { get; set; }
        public double Total { get; set; }
        public double PricePerSquare { get; set; }
        public double Monthly { get; set; }
        public double Apr { get; set; }
        public int Months { get; set; }
    }
}
