using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01
{
    class Part1
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            byte number = 2; // Initialize the variable
            int count = 10;
            float totalPrice = 20.95f;
            char character = 'A';
            string firstName = "John";
            bool isWorking = true;

            var isRegistered = false; // Implicitly typed variable
            var character2 = 'B'; // Implicitly typed variable


            Console.WriteLine(number);
            Console.WriteLine(count);
            Console.WriteLine(totalPrice);
            Console.WriteLine(character);
            Console.WriteLine(firstName);
            Console.WriteLine(isWorking);
            Console.WriteLine(isRegistered);
            Console.WriteLine(character2);


            Console.WriteLine("{0} {1}", byte.MinValue, byte.MaxValue);
            Console.WriteLine("{0} {1}", float.MinValue, float.MaxValue);



            const float Pi = 3.14f;
            Console.WriteLine(Pi);

            byte age = 30;
            int no = age;

            int i = 1000;
            byte b = (byte)i;

            Console.WriteLine(b);


            string str = "123";
            int j = Convert.ToInt32(str);
            int k = int.Parse(str);
            Console.WriteLine(k);

            string str2 = "true";
            bool b1 = Convert.ToBoolean(str2);
            Console.WriteLine(b1);

            try
            {
                var str1 = "1234";
                byte l = Convert.ToByte(str1);
                Console.WriteLine(l);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}