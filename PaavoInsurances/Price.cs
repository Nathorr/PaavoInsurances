using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaavoInsurances
{
    public class Price
    {
        public string postalCode { get; set; }
        public string address { get; set; }
        public string area { get; set; }
        public string buildYear { get; set; }
        public string insuranceStartDate { get; set; }
        public string houseType { get; set; }
        public string billingPeriod { get; set; }
        public string currency { get; set; }
    }
}
