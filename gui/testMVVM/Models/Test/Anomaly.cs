using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testMVVM.Models.Test
{
    internal class Anomaly
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Status Priority{ get; set; }

        public enum Status
        {
            Minor,
            Middle,
            Dangerous,
            Critical
        }
        public override string ToString()
        {
            return Name;
        }


    }
}
