namespace Lab06
{
    public class Part1
    {
        public static void Main(string[] args)
        {
            //----------------------------------------------------------------- case for loop
            for (var i = 1; i <= 10; i++)
            {
                if (i % 2 == 0)
                {
                    Console.WriteLine($"{i} is even");
                }
                else
                {
                    Console.WriteLine($"{i} is odd");
                }
            }
            for (var i = 10; i >= 1; i--)
            {
                if (i % 2 == 0)
                {
                    Console.WriteLine($"{i} is even");
                }
                else
                {
                    Console.WriteLine($"{i} is odd");
                }
            }

            //----------------------------------------------------------------- case foreach loop

            //var name = "John Doe";
            //for (var i = 0; i < name.Length; i++)
            //{
            //    Console.WriteLine(name[i]);
            //}

            var name = "John Doe";
            for (var i = 0; i < name.Length; i++)
            {
                if (name[i] == ' ')
                {
                    continue;
                }
                Console.WriteLine(name[i]);
            }


            foreach (var character in name)
            {
                if (character == ' ')
                {
                    continue;
                }
                Console.WriteLine(character);
            }

            //----------------------------------------------------------------- case foreach loop

            var numbers = new int[] { 1, 2, 3, 4, 5 };
            foreach (var number in numbers)
            {
                Console.WriteLine(number);
            }


            //----------------------------------------------------------------- case while loop

            int index = 0;
            while (index <= 10)
            {
                if (index % 2 == 0)
                {
                    Console.WriteLine($"{index} is even");
                }
                else
                {
                    Console.WriteLine($"{index} is odd");
                }
                index++; 
            }

            while (true)
            {
                Console.Write("Enter a number (or 'exit' to quit): ");
                var input = Console.ReadLine();

                //if(String.IsNullOrWhiteSpace(input))
                //    break;
                //Console.WriteLine("@Echo: " + input);

                if (!String.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("@Echo: " + input);
                    continue;
                }
                break;
            }

            //----------------------------------------------------------------- case do while loop

            index = 0;
            do
            {
                if (index % 2 == 0)
                {
                    Console.WriteLine($"{index} is even");
                }
                else
                {
                    Console.WriteLine($"{index} is odd");
                }
                index++;
            } while (index <= 10);
        }
    }
}