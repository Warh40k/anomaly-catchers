using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testMVVM.Models.Test
{
    internal class Ext
    {
        public int Id_fishery { get; set; }
        public int Id_own { get; set; }
        public DateTime Date_fishery { get; set; }

        public int Num_part { get; set; }
       
        public int Id_Plat { get; set; }

        public int Id_vsd { get; set; }
        public string Name_plat { get; set; }
        public DateTime Product_period { get; set; }
        public string Region_plat { get; set; }
    }
}
