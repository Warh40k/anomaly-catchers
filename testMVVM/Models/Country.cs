using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace testMVVM.Models
{
    internal class PlaceInfo
    {
        public string Name { get; set; }

        public Point Location { get; set; }
    }

    internal class CountryInfo : PlaceInfo
    {
        public IEnumerable<ProvinceInfo> ProvinceCounts { get; set; }
    }
    internal class ProvinceInfo : PlaceInfo { }
    internal struct ConfirmedCount
    {
        public DateTime Date { get; set; }

        public int Count { get; set; }
    }

    internal struct DataPoint
    {
        public double XValue { get; set; }
        public double YValue { get; set; }

    }


}
