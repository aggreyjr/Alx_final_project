using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heavenly_SMS.BusinessTire
{
    class FormValidation
    {
        public static bool IsEmpty(string input)
        {
            if (input == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
