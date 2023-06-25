using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QLBaiDoXe.Classes
{
    public static class Validation
    {
        public static readonly Regex isNumber = new Regex(@"^\d+$");
        public static readonly Regex isPhoneNumber = new Regex(@"^\d{10}$");
        public static readonly Regex isCivilId = new Regex(@"^\d{9}$|^\d{12}$");
    }
}
