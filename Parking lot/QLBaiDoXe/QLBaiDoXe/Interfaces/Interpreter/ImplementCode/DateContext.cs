using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBaiDoXe.Interfaces.Interpreter
{
    internal class DateContext
    {
        public string expression { get; set; }
        public DateTime date { get; set; }
        public DateContext(DateTime date)
        {
            this.date = date;
        }
    }
}
