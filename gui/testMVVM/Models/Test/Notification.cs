using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testMVVM.Models.Test
{
    internal class Notification
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public Anomaly Anomaly { get; set; }
    }
}
