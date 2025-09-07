using System;

namespace Lab1
{
    public class Person
    {
        // 🔹 Instance field (each object has its own copy)
        public string Name = string.Empty;

        // 🔹 Instance method (must be called on an object)
        public void Introduce(string to)
        {
            Console.WriteLine("Hi {0}, I am {1}", to, Name);
        }

        // 🔹 Static method (belongs to the class, not to any object)
        public static Person Parse(string str)
        {
            var person = new Person(); // create a new Person object
            person.Name = str;         // set its Name
            return person;             // return the new object
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            // ---------- Using Static Member ----------
            // Call the static Parse method directly on the class
            var person1 = Person.Parse("Kalle");
            person1.Introduce("Johanna");

            // ---------- Using Instance Members ----------
            // Create a new instance using 'new'
            var person2 = new Person();
            person2.Name = "Anders";
            person2.Introduce("Mikael");
        }
    }
}
