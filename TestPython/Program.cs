using System;
using System.Diagnostics;
using System.Management.Automation;
using PowerShell = System.Management.Automation.PowerShell;
using System.Management.Automation.Runspaces;

namespace TestPython
{
    class Program
    {
        static void Main()
        {
            using (StreamReader reader = new StreamReader(@"C:\Users\user\Desktop\history_parser_pycharm\venv\bin\activate.ps1"))
            {
                reader.ReadToEnd();
            }

            Console.WriteLine();
            using (PowerShell powershell = PowerShell.Create().AddCommand("get - process"))
            {
                foreach (PSObject result in powershell.Invoke())
                {
                    Console.WriteLine(
                                "{0,-20} {1}",
                                result.Members["ProcessName"].Value,
                                result.Members["HandleCount"].Value);
                }
            }
        }
    }
}