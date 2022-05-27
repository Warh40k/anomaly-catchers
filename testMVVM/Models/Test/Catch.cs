using System;
using System.Collections.Generic;

namespace testMVVM.Models.Test
{
    internal class Catch
    {
        public int Id_ves { get; set; }
        public DateTime Date { get; set; }
        
        public int Id_region { get; set; }
        public int Id_fish { get; set; }
        public decimal Catch_volume { get; set; }
        public int Id_regime { get; set; }
        public int Permit { get; set; }
        public int Id_own { get; set; }

    }

    //internal class Group
    //{
    //    public string Name { get; set; }
        
    //    public IList<Catch> DataBase { get; set; } // Может быть любой коллекцией

    //    public string Description { get; set; }
    //}
}

