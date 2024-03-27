using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heavenly_SMS.BusinessTire
{
    public class Barber
    {
        //Declaring properties
        public string Name { get; set; }
        public string Date { get; set; }
        public string Clients { get; set; }

        public const double RATE_1 = (70 / 100.00);

        public const double RATE_2 = 40 / 100.00;

        public const double RATE_3 = 40 / 100.00;

        public double Amount { get; set; }

        //Default constructor
        public Barber() { }

        //Custom constructor
        public Barber(string name, string date, string clients, double amount)
        {
            this.Name = name;
            this.Date = date;
            this.Clients = clients;
            this.Amount = amount;
        }

        public virtual double CalculateCommission(bool isAgent = true)
        {
            double tempCommission = 0;
            if (this.Amount < 10000)
            {
                tempCommission = this.Amount * RATE_1;
            }
            else if (this.Amount >= 10000 && this.Amount <= 40000)
            {
                tempCommission = this.Amount * RATE_2;
            }
            else
            {
                tempCommission = this.Amount * RATE_3;
            }
            return tempCommission;
        }

        public string GetCommissionRate()
        {
            string tempComRate = "";
            if (this.Amount < 10000)
            {
                tempComRate = (100 * RATE_1) + "%";
            }
            else if (this.Amount >= 10000 && this.Amount <= 40000)
            {
                tempComRate = (100 * RATE_2) + "%";
            }
            else
            {
                tempComRate = (100.00 * RATE_3) + "%";
            }
            return tempComRate;
        }
    }
}
