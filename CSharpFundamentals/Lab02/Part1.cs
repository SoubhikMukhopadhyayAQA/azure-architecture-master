using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab02.Math;

namespace Lab02
{
    class Part1
    {
        static void Main(string[] args)
        {
            var jhon = new Person();
            jhon.FirstName = "Jhon";
            jhon.LastName = "Doe";
            jhon.Introduce();


            Calculator calc = new Calculator();
            int result = calc.Add(5, 10);
            Console.WriteLine($"5 + 10 = {result}");
        }
    }
}