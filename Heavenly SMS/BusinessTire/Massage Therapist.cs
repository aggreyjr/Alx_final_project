using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heavenly_SMS.BusinessTire
{
    public class Massage_Therapist : Barber
    {
        private bool istherapist;
        public Massage_Therapist() { }

        public Massage_Therapist(string Name, string Date, string clients, double Amount, bool istherapist) :
            base(Name, Date, clients, Amount)
        {
            this.istherapist = istherapist;
        }

        public override double CalculateCommission(bool istherapist)
        {
            double tempCommission = 0;
            tempCommission = base.CalculateCommission(false) - base.CalculateCommission(false) * (40 / 100.0);
            return tempCommission;
        }
    }
}
