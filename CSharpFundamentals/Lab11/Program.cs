using System;
using System.Collections.Generic;

namespace Lab11
{
    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\nSelect a program to run:");
                Console.WriteLine("1 - Likes program");
                Console.WriteLine("2 - Reverse name");
                Console.WriteLine("3 - Unique numbers");
                Console.WriteLine("4 - Remove duplicates");
                Console.WriteLine("5 - Find 3 smallest numbers");
                Console.WriteLine("0 - Exit");

                Console.Write("Enter your choice: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RunLikes();
                        break;
                    case "2":
                        RunReverseName();
                        break;
                    case "3":
                        RunUniqueNumbers();
                        break;
                    case "4":
                        RunRemoveDuplicates();
                        break;
                    case "5":
                        RunFindThreeSmallest();
                        break;
                    case "0":
                        Console.WriteLine("Exiting...");
                        return; 
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        //----------------------------------------- Likes
        private static void RunLikes()
        {
            var names = new List<string>();

            while (true)
            {
                Console.Write("Enter a name (press Enter to finish): ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    break;

                names.Add(input);
            }

            if (names.Count > 2)
                Console.WriteLine("{0}, {1} and {2} others like your post", names[0], names[1], names.Count - 2);
            else if (names.Count == 2)
                Console.WriteLine("{0} and {1} like your post", names[0], names[1]);
            else if (names.Count == 1)
                Console.WriteLine("{0} likes your post.", names[0]);
            else
                Console.WriteLine("No likes yet.");
        }

        //----------------------------------------- Reverse a name
        private static void RunReverseName()
        {
            Console.Write("Enter your name: ");
            string? name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                var charList = new List<char>(name);
                charList.Reverse();
                Console.Write("Reversed name: ");
                foreach (var ch in charList)
                    Console.Write(ch);
                Console.WriteLine();
            }
        }

        //----------------------------------------- Unique Numbers
        private static void RunUniqueNumbers()
        {
            var numbers = new List<int>();

            while (numbers.Count < 5)
            {
                Console.Write("Enter a number: ");
                var number = Convert.ToInt32(Console.ReadLine());

                if (numbers.Contains(number))
                {
                    Console.WriteLine("You've previously entered " + number);
                    continue;
                }

                numbers.Add(number);
            }

            numbers.Sort();

            Console.WriteLine("Sorted unique numbers:");
            foreach (var number in numbers)
                Console.WriteLine(number);
        }

        //----------------------------------------- Duplicates
        private static void RunRemoveDuplicates()
        {
            var numbers = new List<int>();

            while (true)
            {
                Console.Write("Enter a number (or 'Quit' to exit): ");
                var input = Console.ReadLine();

                if (input?.ToLower() == "quit")
                    break;

                numbers.Add(Convert.ToInt32(input));
            }

            var uniques = new List<int>();
            foreach (var number in numbers)
            {
                if (!uniques.Contains(number))
                    uniques.Add(number);
            }

            Console.WriteLine("Unique numbers:");
            foreach (var number in uniques)
                Console.WriteLine(number);
        }
        //----------------------------------------- 3 smallest numbers
        private static void RunFindThreeSmallest()
        {
            var numbers = new List<int>();
            while (true)
            {
                Console.Write("Enter a number (or 'Quit' to exit): ");
                var input = Console.ReadLine();
                if (input?.ToLower() == "quit")
                    break;
                numbers.Add(Convert.ToInt32(input));
            }
            if (numbers.Count < 5)
            {
                Console.WriteLine("Please enter at least 5 numbers.");
                return;
            }
            numbers.Sort();
            Console.WriteLine("The 3 smallest numbers are:");
            for (int i = 0; i < 3; i++)
                Console.WriteLine(numbers[i]);
        }
    }
}
