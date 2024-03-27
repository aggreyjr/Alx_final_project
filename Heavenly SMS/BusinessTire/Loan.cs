using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heavenly_SMS.BusinessTire
{
    public class Loan : Barber
    {
        //Default constructor
        public Loan() { }

        //Custom constructor that accept parameters from form class
        public Loan(string Name, string Date, string clients, double amount) : base(Name,
            Date, clients, amount)
        {

        }

        public override double CalculateCommission(bool isbarber)
        {
            double tempCommission = 0;
            tempCommission = base.CalculateCommission() + base.CalculateCommission(false) * (70 / 100.0);
            return tempCommission;
        }
    }
}
