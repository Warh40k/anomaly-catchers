using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testMVVM.Models.Test
{
    internal class Product
    {
        public int Id_ves { get; set; }
        public DateTime Date {get; set;}
        public string Id_prod_designate { get; set; }
        public string Prod_type { get; set; }
        public decimal Prod_volume { get; set; }
        public decimal Prod_board_volume { get; set; }
    }
}
