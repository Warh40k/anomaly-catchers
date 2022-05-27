using System;
using System.Collections.Generic;

namespace testMVVM.Models.Test
{
    internal class Catch
    {
        public int id_ves { get; set; }
        public DateTime date { get; set; }
        
        public int id_region { get; set; }
        public int id_fish { get; set; }
        public decimal catch_volume { get; set; }
        public int id_regime { get; set; }
        public int permit { get; set; }
        public int own { get; set; }

    }

    //internal class Group
    //{
    //    public string Name { get; set; }
        
    //    public IList<Catch> DataBase { get; set; } // Может быть любой коллекцией

    //    public string Description { get; set; }
    //}
}

