using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testMVVM.Models.Test
{
    internal class CatchData 
    {
        public List<Catch> Data { get; set; }


        public CatchData(List<Catch> data=null)
        {
            Data = data;
        }
    }
}
