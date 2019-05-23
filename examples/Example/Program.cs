using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.BrowserBoss;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // This is a sample site designed to be difficult to script.
                Boss.Url = "http://newtours.demoaut.com/";

                Boss.Click("REGISTER");                   // You can click links by text
                Boss.Write("#email", "StephenCleary");          // CSS selectors are commonly used
                Boss.Write("//input[@type='password']", "password"); // XPath also works fine
                // Note that the last line matched both password inputs, and filled them both in.
                Boss.Click("input[name='register']");

                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }
    }
}
