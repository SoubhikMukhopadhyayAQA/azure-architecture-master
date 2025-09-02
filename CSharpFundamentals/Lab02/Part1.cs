using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Lab02.Math;

namespace Lab02
{
    public enum ShippingMethod
    {
        RegularAirMail = 1,
        RegisteredAirMail = 2,
        Express = 3
    }
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



            int[] number = new int[3];
            var numbers = new int[3];
            numbers[0] = 1;

            Console.WriteLine(numbers[0]);
            Console.WriteLine(numbers[1]);
            Console.WriteLine(numbers[2]);

            var flags = new bool[3];
            flags[0] = true;


            Console.WriteLine(flags[0]);
            Console.WriteLine(flags[1]);
            Console.WriteLine(flags[2]);




            var names = new string[3] { "Jack", "John", "Mary" };

            Console.WriteLine(names[0]);



            var firstName = "Jack";
            var lastName = "Smith";
            //String lastName = "Smith";
            //string name = "Jhon";

            var fullName1 = firstName + " " + lastName;
            var fullName2 = string.Format("My name is {0} {1}", firstName, lastName);


            var fullName3 = $"My name is {firstName} {lastName}";
            var fullName4 = @$"My name is {firstName}";


            var names2 = new string[3] { "Jack", "John", "Mary" };
            var formattedNames = string.Join(", ", names2);
             
            Console.WriteLine(formattedNames);


            var text = "Hi Jhon \n Look \nc:\\folder1\\folder2";
            Console.WriteLine(text);


            var text2 = @"Hi Jhon
                          Look 
                          c:\folder1\folder2";
            Console.WriteLine(text2);




            var method = ShippingMethod.Express;
            Console.WriteLine((int)method);


            var methodId = 3;
            Console.WriteLine((ShippingMethod)methodId); // cast int to enum


            Console.WriteLine(method.ToString());
            var methodName = "Express";
            var shippingMethod = (ShippingMethod)Enum.Parse(typeof(ShippingMethod), methodName);

            //Enum.Parse(typeof(ShippingMethod), methodName);

            Console.WriteLine(shippingMethod);
        }
    }
}