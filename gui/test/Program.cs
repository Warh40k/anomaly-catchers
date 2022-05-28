using System;
using System.IO;
using System.Linq;
using System.Collections;

namespace test
{
    class Program
    {
        static void Main()
        {
            var ext1 = File.ReadAllLines(@"C:\Users\user\Documents\Data\db2\Ext.csv");
            var ext2 = File.ReadAllLines(@"C:\Users\user\Documents\Data\db2\Ext2.csv");

            IEnumerable<string> scoreQuery1 =
             from name in ext1
             let ext1Fields = name.Split(',')
             from id in ext2
             let ext2Fields = id.Split(',')
             where ext1Fields[5] == ext2Fields[0]
             select string.Join(',', ext1Fields) + "," + String.Join(',', ext2Fields);

            scoreQuery1 = scoreQuery1.ToList();
                
        }
    }
}