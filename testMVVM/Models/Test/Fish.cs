using System;
using System.Collections.Generic;

namespace testMVVM.Models.Test
{
    internal class DataBase
    {
        public string Name { get; set; }

        public string Surname {get; set;}

        public string Patronymic { get; set; }
        
        public DateTime Birthday { get; set; }

        public double Rating { get; set; }

        public string Description { get; set; }
    }

    internal class Group
    {
        public string Name { get; set; }
        
        public IList<DataBase> DataBase { get; set; } // Может быть любой коллекцией

        public string Description { get; set; }
    }
}

